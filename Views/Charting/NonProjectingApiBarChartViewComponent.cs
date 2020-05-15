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
    // Bar chart which does not require a projection from each object,
    // but rather the JSON objects themselves are summarized (such as a count
    // of how many objects are returned after filtering, etc.)
    public class NonProjectingApiBarChartViewComponent : BarChartViewComponent {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public NonProjectingApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<BarChart> ProjectChart(Summary<JObject, float> summary, Filter<Criterion> filter,
                TimeInterval timeInterval, PropertyValueCategorizer categorizer, string drilldownController, string drilldownAction) {
            // For each time interval, add a datum to the dataset
            var rawData = await apiAccess.PullKeyDeviceActivity(timeInterval.Start, timeInterval.End);
            // Parse string to JObject
            var rootObject = JObject.Parse(rawData);
            if (!rootObject.TryGetValue("Data", out var dataArrayToken)) {
                throw new JsonArgumentException();
            }

            // Convert data array token to JEnumerable and
            // filter out unwanted data
            var dataArray = dataArrayToken.AsJEnumerable();
            dataArray = filter.FilterData(dataArray);

            // Project each token to a value, and store
            // the value in the associated category's
            // collection
            var projectedData = new Dictionary<string, Collection<JObject>>();
            foreach (var token in dataArray) {
                if (token.Type != JTokenType.Object) {
                    throw new JsonArgumentException();
                }
                try{
                    var obj = (JObject) token;
                    var key = categorizer.Categorize(obj);
                    // If the category does not yet exist, add it
                    if (!projectedData.ContainsKey(key)) {
                        projectedData.Add(key, new Collection<JObject>());
                    }

                    // Add the value to the category's collection
                    projectedData[key].Add(obj);
                } catch(JsonArgumentException) {}
            }

            // Summarize each category's collection into a summary value,
            // such as a count, etc.
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
                    preFilter = filter,
                    filter = new Filter<CategorizerCriterion>() { // Match data with the same category / key as this bar
                        Criteria = new List<CategorizerCriterion>() {
                            new CategorizerCriterion(s.Key, categorizer)
                        }
                    },
                    timeSeries = new TimeSeries(new List<TimeInterval>() {
                            new TimeInterval(DateTime.ParseExact("2019-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                DateTime.ParseExact("2019-06-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "June"),
                            new TimeInterval(DateTime.ParseExact("2019-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                DateTime.ParseExact("2019-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "July"),
                            new TimeInterval(DateTime.ParseExact("2019-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                DateTime.ParseExact("2019-08-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "August"),
                            new TimeInterval(DateTime.ParseExact("2019-09-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                DateTime.ParseExact("2019-09-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "September")
                    })
                }
            }).ToList();

            return new BarChart() {
                Bars = bars,
                Label = "Dataset"
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(Summary<JObject, float> summary, Filter<Criterion> filter,
                TimeInterval timeInterval, PropertyValueCategorizer categorizer, string drilldownController, string drilldownAction) {
            var barChart = await ProjectChart(summary, filter, timeInterval, categorizer, drilldownController, drilldownAction);
            return await Task.Run(() => View(barChart));
        }
    }
}