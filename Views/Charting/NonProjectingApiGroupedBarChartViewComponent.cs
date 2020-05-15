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
    // Represents a grouped bar chart which categorizes
    // but does not project data from the API.
    public class NonProjectingApiGroupedBarChartViewComponent : GroupedBarChartViewComponent {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public NonProjectingApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<GroupedBarChart> ProjectChart(Filter<Criterion> filter, TimeInterval timeInterval,
                PropertyValueCategorizer superDatasetCategorizer, PropertyValueCategorizer subDatasetCategorizer,
                Summary<JObject, float> summary, string drilldownAction, string drilldownController) {
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

            var labeledCollections = new Dictionary<string, IDictionary<string, IEnumerable<JObject>>>();

            // Categorize each token to a super- and sub- dataset,
            // and store in the appropriate collection
            foreach (var token in dataArray) {
                if (token.Type != JTokenType.Object) {
                    throw new JsonArgumentException();
                }
                try{
                    var obj = (JObject) token;
                    var superDatasetCategory = superDatasetCategorizer.Categorize(obj);
                    var subDatasetCategory = subDatasetCategorizer.Categorize(obj);
                    // Create dictionary entries if they don't already exist
                    if (!labeledCollections.ContainsKey(superDatasetCategory)) {
                        labeledCollections.Add(superDatasetCategory, new Dictionary<string, IEnumerable<JObject>>());
                    }

                    if (!labeledCollections[superDatasetCategory].ContainsKey(subDatasetCategory)) {
                        labeledCollections[superDatasetCategory].Add(subDatasetCategory, new Collection<JObject>());
                    }

                    // Add the object to the corresponding collection
                    labeledCollections[superDatasetCategory][subDatasetCategory].Append(obj);
                } catch(JsonArgumentException) {}
            }

            // Summarize each sub-dataset of each dataset into a summary value,
            // such as a mean, median, maximum, etc.
            var summaryValues = new Dictionary<string, IDictionary<string, float>>();
            foreach (var superDataset in labeledCollections) {
                summaryValues.Add(superDataset.Key, new Dictionary<string, float>());

                // Summarize each category within the dataset
                foreach (var subDataset in superDataset.Value) {
                    // For each sub-dataset in this super-dataset,
                    // construct a summary and store it in the summaryValues
                    summaryValues[superDataset.Key].Add(
                        subDataset.Key,
                        summary.Summarize(subDataset.Value));
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
                    filter = new Filter<Criterion>(
                        new List<Criterion>() {
                            new CategorizerCriterion(kvp.Key, superDatasetCategorizer),
                            new CategorizerCriterion(subLabel, subDatasetCategorizer)
                        }
                    ), // Drilldown data should match the same super- and sub-datasets (filter)
                    summary,
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
                }).ToList()
            }).ToList();

            return new GroupedBarChart() {
                BarGroups = barGroups,
                Labels = subLabels
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(Summary<JObject, float> summary, Filter<Criterion> filter,
                TimeInterval timeInterval, PropertyValueCategorizer superDatasetCategorizer, PropertyValueCategorizer subDatasetCategorizer,
                string drilldownAction, string drilldownController, bool pivot = false) {
            var barChart = await ProjectChart(filter, timeInterval, superDatasetCategorizer, subDatasetCategorizer,
                    summary, drilldownAction, drilldownController);
            return View(pivot ? barChart.Pivot() : barChart);
        }
    }
}