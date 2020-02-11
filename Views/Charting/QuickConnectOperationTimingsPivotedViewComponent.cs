using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Newtonsoft.Json;
using Util;

namespace DirectKeyDashboard.Views.Charting
{
    // Displays various operation class average timings in a bar chart
    public class QuickConnectOperationTimingsPivotedViewComponent : GroupedBarChartViewComponent {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public QuickConnectOperationTimingsPivotedViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected override async Task<GroupedBarChart> ProjectChart() {
            // Eventually, attempt to pull the bar chart
            // itself straight from the cache so that the
            // full data doesn't need to be retrieved and
            // the bar chart reprojected on, for example,
            // page refresh. For now, pull the data every
            // time.
            string rawData = await apiAccess.PullKeyDeviceActivity();
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            var apiDataModel = JsonConvert.DeserializeObject<ApiDataModel>(rawData, serializerSettings);
            var groups = apiDataModel.Data.Where(d => d.OperationUserIntentDurationMs != 0).GroupBy(m => m.OperationCode);
            var averageUserIntentTimes = new Dictionary<string, int>(
                groups.Select(
                    g => new KeyValuePair<string, int>(
                        g.First().OperationCode,
                        (int) g.Select(d => d.OperationUserIntentDurationMs).Average())).ToList());
            var averageTotalTimes = new Dictionary<string, int>(
                groups.Select(
                    g => new KeyValuePair<string, int>(
                        g.First().OperationCode,
                        (int) g.Select(d => d.OperationTotalDurationMs).Average())).ToList());
            var averageTimes = new Dictionary<string, int>(
                groups.Select(
                    g => new KeyValuePair<string, int>(
                        g.First().OperationCode,
                        (int) g.Select(d => d.OperationDurationMs).Average())).ToList());

            // Construct a map from an operation type / super group
            // to its list of averages for durations. Make sure the
            // durations are in a consistent sub-group ordering. The
            // imposed order is comm duration,
            // connect duration.
            var valuesDict = new Dictionary<string, IList<int>>();
            foreach (var g in groups) {
                var key = g.First().OperationCode;
                valuesDict.Add(key,
                    new List<int>() {
                        averageUserIntentTimes[key],
                        averageTotalTimes[key],
                        averageTimes[key]
                    });
            }

            // Entities are grouped by operation codes which correspond one-to-one
            // with operation descriptions, so they're also grouped by descriptions
            var labelsDict = new Dictionary<string, string>(
                groups.Select(
                    g => new KeyValuePair<string, string>(
                        g.First().OperationCode,
                        g.First().OperationDescription)).ToList());
            
            // Make sure the order of labels matches the sub-order of values as well
            var labels = new List<string>() {
                EnumHelper<ApiDataSubModel>.GetPropertyDisplayName(nameof(ApiDataSubModel.OperationUserIntentDurationMs)),
                EnumHelper<ApiDataSubModel>.GetPropertyDisplayName(nameof(ApiDataSubModel.OperationTotalDurationMs)),
                EnumHelper<ApiDataSubModel>.GetPropertyDisplayName(nameof(ApiDataSubModel.OperationDurationMs))
            };
            
            // Generate bar group colors using a linearly distributed hue, all with
            // the same saturation, lightness, and transparency
            var hueBackground = 0; // Start at zero degrees / red

            // Increment 1/n of the color wheel each iteration
            var hueIncrement = 360 / groups.Count();

            // Use the postIncHue function to get the original value of the hue
            // while simultaneously updating it to a new value by incrementation.
            // Note that LINQ uses lazy evaluation, so it will compute the
            // enumerable as they are iterated the first time, resulting in the bacgkround
            // and border colors to be computed alternatingly rather than
            // all of the background colors being computed, then all of the border colors. To prevent
            // asynchronous conflicts (where they use alternating hues rather
            // than corresponding hues), simply use two different hue variables
            // that increment separately.
            var backgroundColors = new Dictionary<string, string>();
            foreach (var group in groups) {
                backgroundColors.Add(group.First().OperationCode, $"hsla({postIncHue(ref hueBackground, hueIncrement)}, {BarGroup.BackgroundSaturation}, {BarGroup.BackgroundLightness}, {BarGroup.BackgroundAlpha})");
            }

            var hueBorder = 0; // Reset hue for border colors
            var borderColors = new Dictionary<string, string>();
            foreach (var group in groups) {
                borderColors.Add(group.First().OperationCode, $"hsla({postIncHue(ref hueBorder, hueIncrement)}, {BarGroup.BackgroundSaturation}, {BarGroup.BackgroundLightness}, {BarGroup.BackgroundAlpha})");
            }

            // Construct list of bar groups
            var barGroups = valuesDict.Select(kvp => new BarGroup() {
                Values = kvp.Value,
                Label = labelsDict[kvp.Key],
                BackgroundColor = backgroundColors[kvp.Key],
                BorderColor = borderColors[kvp.Key]
            });

            // Lastly, we just have to return the grouped bar chart
            return new GroupedBarChart() {
                BarGroups = barGroups.ToList(),
                Labels = labels
            };
        }

        private class ApiDataModel {
            public List<ApiDataSubModel> Data {get; set;}
        }

        private class ApiDataSubModel {
            public string OperationCode {get; set;}
            public string OperationDescription {get; set;}
            [DisplayName("User Intent Duration (MS)")]
            public int? OperationUserIntentDurationMs {get; set;}
            [DisplayName("Total Duration (MS)")]
            public int? OperationTotalDurationMs {get; set;}
            [DisplayName("Duration (MS)")]
            public int? OperationDurationMs {get; set;}
        }
    }
}