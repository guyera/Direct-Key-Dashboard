using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Views.Charting
{
    // Represents a bar chart which projects data from the API.
    // The data is filtered by the Filter model supplied,
    // projected by the GropuedProjection model, and summarized
    // by the Summary model.
    public abstract class ApiBarChartViewComponent<TProjection, TSummary, TGroupedProjection, TCriterion> : BarChartViewComponent
            where TSummary : Summary<TProjection, float>
            where TGroupedProjection : GroupedProjection<TProjection>
            where TCriterion : Criterion {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public ApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<BarChart> ProjectChart(BarChartContext ctx, TGroupedProjection projection, TSummary summary, string drilldownController, string drilldownAction) {
            // For each time interval, add a datum to the dataset
            var rawData = await apiAccess.PullKeyDeviceActivity(ctx.TimeInterval.Start, ctx.TimeInterval.End);
            // Parse string to JObject
            var rootObject = JObject.Parse(rawData);
            if (!rootObject.TryGetValue("Data", out var dataArrayToken)) {
                throw new JsonArgumentException();
            }

            // Convert data array token to JEnumerable and
            // filter out unwanted data
            var dataArray = dataArrayToken.AsJEnumerable();
            dataArray = ctx.Filter.FilterData(dataArray);

            // Project each token to a value, and store
            // the value in the associated category's
            // collection
            var projectedData = new Dictionary<string, Collection<TProjection>>();
            foreach (var token in dataArray) {
                if (token.Type != JTokenType.Object) {
                    throw new JsonArgumentException();
                }
                try{
                    var kvp = projection.Project((JObject) token);
                    // If the category does not yet exist, add it
                    if (!projectedData.ContainsKey(kvp.Key)) {
                        projectedData.Add(kvp.Key, new Collection<TProjection>());
                    }

                    // Add the value to the category's collection
                    projectedData[kvp.Key].Add(kvp.Value);
                } catch(JsonArgumentException) {}
            }

            // Summarize each category's collection into a summary value,
            // such as a mean, median, maximum, etc.
            var summaryValues = new Dictionary<string, float>();
            foreach (var kvp in projectedData) {
                summaryValues.Add(kvp.Key, summary.Summarize(kvp.Value));
            }

            int backgroundHue = 0;
            int borderHue = 0;
            var hueIncrement = 360 / summaryValues.Count();

            var orderedSummaryValues = summaryValues.OrderByDescending(s => s.Value).ToList();
            var bars = orderedSummaryValues.Select(s => new Bar() {
                Value = (int) s.Value,
                Label = s.Key,
                BackgroundColor = $"hsla({PostIncHue(ref backgroundHue, hueIncrement)}, {Bar.BackgroundSaturation}, {Bar.BackgroundLightness}, {Bar.BackgroundAlpha})",
                BorderColor = $"hsla({PostIncHue(ref borderHue, hueIncrement)}, {Bar.BorderSaturation}, {Bar.BorderLightness}, {Bar.BorderAlpha})",
                DrilldownController = drilldownController,
                DrilldownAction = drilldownAction,
                DrilldownQueryParameters = new {
                    summary, // Summarize drilldown data in the same way
                    preFilter = ctx.Filter,
                    filter = new Filter<ProjectionCriterion<string, CategoryProjection<TProjection, TGroupedProjection>>>(new List<ProjectionCriterion<string, CategoryProjection<TProjection, TGroupedProjection>>>() { // Match data with the same category / key as this bar
                        new ProjectionCriterion<string, CategoryProjection<TProjection, TGroupedProjection>>(new CategoryProjection<TProjection, TGroupedProjection>(projection), s.Key)
                    }),
                    timeSeries = new TimeSeries(new List<TimeInterval>() {
                            new TimeInterval(DateTime.ParseExact("2019-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-06-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "June"),
                            new TimeInterval(DateTime.ParseExact("2019-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "July"),
                            new TimeInterval(DateTime.ParseExact("2019-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-08-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "August"),
                            new TimeInterval(DateTime.ParseExact("2019-09-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-09-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "September")
                    }),
                    projection = new ValueProjection<TProjection, TGroupedProjection>(projection) // Project drilldown data in the same way, but only project the value, not the key / value pair (since all keys / categories are the same in a single drilldown chart)
                }
            }).ToList();

            return new BarChart() {
                Bars = bars,
                Label = "Dataset"
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(TSummary summary, Filter<TCriterion> filter, TimeInterval timeInterval, TGroupedProjection projection, string drilldownController, string drilldownAction) {
            var barChart = await ProjectChart(new BarChartContext(filter, timeInterval), projection, summary, drilldownController, drilldownAction);
            return await Task.Run(() => View(barChart));
        }

        protected class BarChartContext {
            public Filter<TCriterion> Filter {get;}
            public TimeInterval TimeInterval {get;}
            public BarChartContext(Filter<TCriterion> filter, TimeInterval timeInterval) {
                Filter = filter;
                TimeInterval = timeInterval;
            }
        }
    }
}