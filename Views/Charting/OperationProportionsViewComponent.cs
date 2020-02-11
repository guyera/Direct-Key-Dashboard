using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Newtonsoft.Json;

namespace DirectKeyDashboard.Views.Charting
{
    // Displays operation class proportions in a pie chart
    public class OperationProportionsViewComponent : PieChartViewComponent {

        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public OperationProportionsViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        public override async Task<PieChart> ProjectChart() {
            // Eventually, attempt to pull the pie chart
            // itself straight from the cache so that the
            // full data doesn't need to be retrieved and
            // the pie chart reprojected on, for example,
            // page refresh. For now, pull the data every
            // time.
            string rawData = await apiAccess.PullKeyDeviceActivity();
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var apiDataModel = JsonConvert.DeserializeObject<ApiDataModel>(rawData, serializerSettings);
            var groups = apiDataModel.Data.GroupBy(m => m.OperationCode);
            var counts = groups.Select(g => g.Count());
            // Entities are grouped by operation codes which correspond one-to-one
            // with operation descriptions, so they're also grouped by descriptions
            var labels = groups.Select(g => g.First().OperationDescription);
            
            // Generate pie chart colors using a linearly distributed hue, all with
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
            var backgroundColors = groups.Select(g => $"hsla({postIncHue(ref hueBackground, hueIncrement)}, {PieSlice.BackgroundSaturation}, {PieSlice.BackgroundLightness}, {PieSlice.BackgroundAlpha})");
            var hueBorder = 0; // Reset hue for border colors
            var borderColors = groups.Select(g => $"hsla({postIncHue(ref hueBorder, hueIncrement)}, {PieSlice.BorderSaturation}, {PieSlice.BorderLightness}, {PieSlice.BorderAlpha})");

            // Now we have enumerables of background colors, border colors, labels, and counts.
            // We just have to zip the lists together to get a nested quadruple
            var slices = counts.Zip(labels.Zip(backgroundColors.Zip(borderColors)))
                               .Select(tuple => new PieSlice{
                                   Count = tuple.First,
                                   Label = tuple.Second.First,
                                   BackgroundColor = tuple.Second.Second.First,
                                   BorderColor = tuple.Second.Second.Second
                               });

            // Lastly, we just have to return the pie chart
            return new PieChart {
                Slices = slices.ToList(),
                Label = "Proportion of Operations"
            };
        }

        private class ApiDataModel {
            public List<ApiDataSubModel> Data {get; set;}
        }

        private class ApiDataSubModel {
            public string OperationCode {get; set;}
            public string OperationDescription {get; set;}
        }
    }
}