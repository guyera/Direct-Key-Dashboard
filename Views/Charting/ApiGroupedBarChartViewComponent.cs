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

/* TODO Reduce the generics used and increase polymorphism in its place.
    e.g. instead of requiring TSummary as a generic type parameter,
    simply accept Summary<TProjection, float> as an invocation argument. 
    The only reason to use generics and generic type constraints enforcing
    inheritance is to allow more flexibility with the type. e.g. IList<Dog>
    is not substitutable for IList<Animal> (though covariance IS allowed with
    classes, just not interfaces), or to guarantee that types match in certain
    locations (e.g. the summary takes as an argument a list of the same type which the
    projection produces). However, I do not need such flexibility, nor do I need
    such constraints with summaries, projections, or criteria. The only
    real case for generics here is TProjection (which should probably
    be named to TProjectionValue, as it represents the type of the thing
    which is projected, not the type of the projection itself), for the
    reason mentioned (summary must be able to summarize the type of thing
    which was projected)  */

namespace DirectKeyDashboard.Views.Charting
{
    // Represents a bar chart which projects data from the API.
    // The data is filtered by the Filter model supplied,
    // projected by the GropuedProjection model, and summarized
    // by the Summary model.
    public class ApiGroupedBarChartViewComponent<TProjection, TSummary, TCriterion, TCompositeProjection> : GroupedBarChartViewComponent
            where TSummary : Summary<TProjection, float>
            where TCriterion : Criterion
            where TCompositeProjection : CompositeGroupedProjection<TProjection> {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public ApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<GroupedBarChart> ProjectChart(Filter<TCriterion> filter, TimeInterval timeInterval, TCompositeProjection projection, TSummary summary, string drilldownAction, string drilldownController) {
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
            var projectedData = new Dictionary<string, Collection<IDictionary<string, TProjection>>>();
            foreach (var token in dataArray) {
                if (token.Type != JTokenType.Object) {
                    throw new JsonArgumentException();
                }
                try{
                    var kvp = projection.Project((JObject) token);
                    // If the dataset does not yet exist, add it
                    if (!projectedData.ContainsKey(kvp.Key)) {
                        projectedData.Add(kvp.Key, new Collection<IDictionary<string, TProjection>>());
                    }

                    // Add the value to the dataset's collection
                    projectedData[kvp.Key].Add(kvp.Value);
                } catch(JsonArgumentException) {}
            }

            // Summarize each category of each dataset into a summary value,
            // such as a mean, median, maximum, etc.
            var summaryValues = new Dictionary<string, IDictionary<string, float>>();
            foreach (var datasetKvp in projectedData) {
                summaryValues.Add(datasetKvp.Key, new Dictionary<string, float>());
                // We must assume every inner dictionary has the same keys.
                // Otherwise, some datasets may have more / less summary values
                // than others. We must summarize the list of all values associated
                // with each given key, across every inner dictionary.

                var first = datasetKvp.Value.First();

                // Summarize each category within the dataset
                foreach (var category in first) {
                    // For each category in this dataset,
                    // find every inner dictionary which
                    // contains a value for the category.
                    // Then, get said value of each inner
                    // dictionary, compiled into an enumerable.
                    // Lastly, summarize that enumerable with
                    // the summary model provided.
                    summaryValues[datasetKvp.Key].Add(
                        category.Key,
                        summary.Summarize(datasetKvp.Value.Where(
                            innerDict => innerDict.ContainsKey(category.Key))
                            .Select(innerDict => innerDict[category.Key])));
                }
            }

            // Construct an ordered list of subLabels for the inner data points.
            // This order must be maintained across all datasets, so this
            // list can be used as the ordering of keys.
            var subLabels = summaryValues.Any() ? summaryValues.First().Value.Select(kvp => kvp.Key).ToList() : new List<string>();

            // Each dataset gets its own color
            int backgroundHue = 0;
            int borderHue = 0;
            var hueIncrement = summaryValues.Count() == 0 ? 0 : 360 / summaryValues.Count();

            // Construct a list of bar groups using the summaryValues and the subLabels as a
            // guide for inner key ordering. Note: Select() preserves ordering, so using
            // subLabels.Select() will allow a projection of values preserving
            // the inner ordering.
            var drilldownActions = subLabels.Select(subLabel => drilldownAction).ToList();
            var drilldownControllers = subLabels.Select(subLabel => drilldownController).ToList();
            var barGroups = summaryValues.Select(kvp => new BarGroup() {
                Label = kvp.Key,
                BackgroundColor = $"hsla({PostIncHue(ref backgroundHue, hueIncrement)}, {BarGroup.BackgroundSaturation}, {BarGroup.BackgroundLightness}, {BarGroup.BackgroundAlpha})",
                BorderColor = $"hsla({PostIncHue(ref borderHue, hueIncrement)}, {BarGroup.BorderSaturation}, {BarGroup.BorderLightness}, {BarGroup.BorderAlpha})",
                Values = subLabels.Select(label => (int) kvp.Value[label]).ToList(),
                DrilldownActions = drilldownActions,
                DrilldownControllers = drilldownControllers,
                DrilldownQueryParameters = subLabels.Select(subLabel => (object) new {
                    preFilter = filter,
                    filter = new Filter<ProjectionCriterion<string, CategoryProjection<IDictionary<string, TProjection>, TCompositeProjection>>>(
                        new List<ProjectionCriterion<string, CategoryProjection<IDictionary<string, TProjection>, TCompositeProjection>>>() {
                            new ProjectionCriterion<string, CategoryProjection<IDictionary<string, TProjection>, TCompositeProjection>>(
                                new CategoryProjection<IDictionary<string, TProjection>, TCompositeProjection>(projection),
                                kvp.Key
                            )
                        }
                    ), // Drilldown data should match the same dataset (filter)
                    summary,
                    timeSeries = new TimeSeries(new List<TimeInterval>() {
                            new TimeInterval(DateTime.ParseExact("2019-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-06-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "June"),
                            new TimeInterval(DateTime.ParseExact("2019-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "July"),
                            new TimeInterval(DateTime.ParseExact("2019-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-08-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "August"),
                            new TimeInterval(DateTime.ParseExact("2019-09-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-09-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "September")
                    }),
                    projection = new CompositeValueProjection<TProjection, TCompositeProjection>(projection, subLabel)
                }).ToList()
            }).ToList();

            return new GroupedBarChart() {
                BarGroups = barGroups,
                Labels = subLabels
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(TSummary summary, Filter<TCriterion> filter, TimeInterval timeInterval, TCompositeProjection projection, string drilldownAction, string drilldownController, bool pivot = false) {
            var barChart = await ProjectChart(filter, timeInterval, projection, summary, drilldownAction, drilldownController);
            return View(pivot ? barChart.Pivot() : barChart);
        }
    }
}