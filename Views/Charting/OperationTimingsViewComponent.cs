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
    public class OperationTimingsViewComponent : GroupedBarChartViewComponent {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public OperationTimingsViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

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
            var apiDataModel = JsonConvert.DeserializeObject<ApiDataModel>(rawData, serializerSettings);
            var groups = apiDataModel.Data.Where(d => d.OperationUserIntentDurationMs == 0).GroupBy(m => m.OperationCode);
            var averageCommTimes = new Dictionary<string, int>(
                groups.Select(
                    g => new KeyValuePair<string, int>(
                        g.First().OperationCode,
                        (int) g.Select(d => d.OperationCommDurationMs).Average())).ToList());
            var averageConnectTimes = new Dictionary<string, int>(
                groups.Select(
                    g => new KeyValuePair<string, int>(
                        g.First().OperationCode,
                        (int) g.Select(d => d.OperationConnectDurationMs).Average())).ToList());
            var averageTotalTimes = new Dictionary<string, int>(
                groups.Select(
                    g => new KeyValuePair<string, int>(
                        g.First().OperationCode,
                        (int) g.Select(d => d.OperationDurationMs).Average())).ToList());

            // Flatten dictionaries to just integer values, making sure each
            // list of integer values has the same sub-ordering of operation codes
            var commTimeKvps = averageCommTimes.ToList();
            var orderedCommTimes = commTimeKvps.Select(kvp => kvp.Value).ToList();
            var orderedConnectTimes = new List<int>();
            foreach (var kvp in commTimeKvps) {
                orderedConnectTimes.Add(averageConnectTimes[kvp.Key]);
            }
            var orderedTotalTimes = new List<int>();
            foreach (var kvp in commTimeKvps) {
                orderedTotalTimes.Add(averageTotalTimes[kvp.Key]);
            }

            // The exact sub-ordering doesn't matter, so long as it's consistent

            // Entities are grouped by operation codes which correspond one-to-one
            // with operation descriptions, so they're also grouped by descriptions
            var labelsDict = new Dictionary<string, string>(
                groups.Select(
                    g => new KeyValuePair<string, string>(
                        g.First().OperationCode,
                        g.First().OperationDescription)).ToList());
            
            // Make sure the order of labels matches the sub-order of values as well
            var labels = new List<string>();
            foreach (var kvp in commTimeKvps) {
                labels.Add(labelsDict[kvp.Key]);
            }
            
            // Generate bar group colors using a linearly distributed hue, all with
            // the same saturation, lightness, and transparency
            var hueBackground = 0; // Start at zero degrees / red

            // Increment 1/n of the color wheel each iteration
            var hueIncrement = 360 / 3; // There are 3 datasets (one for each type of duration stacked up)

            // Use the postIncHue function to get the original value of the hue
            // while simultaneously updating it to a new value by incrementation.
            // Note that LINQ uses lazy evaluation, so it will compute the
            // enumerable as they are iterated the first time, resulting in the bacgkround
            // and border colors to be computed alternatingly rather than
            // all of the background colors being computed, then all of the border colors. To prevent
            // asynchronous conflicts (where they use alternating hues rather
            // than corresponding hues), simply use two different hue variables
            // that increment separately.
            var backgroundColors = new List<string>();
            for (int i = 0; i < 3; i++) {
                backgroundColors.Add($"hsla({postIncHue(ref hueBackground, hueIncrement)}, {BarGroup.BackgroundSaturation}, {BarGroup.BackgroundLightness}, {BarGroup.BackgroundAlpha})");
            }

            var hueBorder = 0; // Reset hue for border colors
            var borderColors = new List<string>();
            for (int i = 0; i < 3; i++) {
                borderColors.Add($"hsla({postIncHue(ref hueBorder, hueIncrement)}, {BarGroup.BorderSaturation}, {BarGroup.BorderLightness}, {BarGroup.BorderAlpha})");
            }

            // The order goes Comm, Connect. This ordering
            // must be consistent throughout the entire chart projection.
            // Else the groups will be mislabeled.
            var groupLabels = new List<string>(){
                EnumHelper<ApiDataSubModel>.GetPropertyDisplayName(nameof(ApiDataSubModel.OperationCommDurationMs)),
                EnumHelper<ApiDataSubModel>.GetPropertyDisplayName(nameof(ApiDataSubModel.OperationConnectDurationMs)),
                EnumHelper<ApiDataSubModel>.GetPropertyDisplayName(nameof(ApiDataSubModel.OperationDurationMs))
            };

            // Construct list of bar groups
            var barGroups = new List<BarGroup>(){
                new BarGroup(){
                    Label = groupLabels[0],
                    BackgroundColor = backgroundColors[0],
                    BorderColor = borderColors[0],
                    Values = orderedCommTimes
                },
                new BarGroup(){
                    Label = groupLabels[1],
                    BackgroundColor = backgroundColors[1],
                    BorderColor = borderColors[1],
                    Values = orderedConnectTimes
                },
                new BarGroup(){
                    Label = groupLabels[2],
                    BackgroundColor = backgroundColors[2],
                    BorderColor = borderColors[2],
                    Values = orderedTotalTimes
                }
            };

            // Lastly, we just have to return the grouped bar chart
            return new GroupedBarChart() {
                BarGroups = barGroups,
                Labels = labels
            };
        }

        private class ApiDataModel {
            public List<ApiDataSubModel> Data {get; set;}
        }

        private class ApiDataSubModel {
            public string OperationCode {get; set;}
            public string OperationDescription {get; set;}
            public int OperationUserIntentDurationMs {get; set;}

            [DisplayName("Comm Duration (MS)")]
            public int OperationCommDurationMs {get; set;}
            [DisplayName("Connect Duration (MS)")]
            public int OperationConnectDurationMs {get; set;}
            [DisplayName("Duration (MS)")]
            public int OperationDurationMs {get; set;}
        }
    }
}