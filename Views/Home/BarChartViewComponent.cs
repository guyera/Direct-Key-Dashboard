using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DirectKeyDashboard.Views.Home
{
    // Creates a test bar chart
    public class BarChartViewComponent : ViewComponent {
        private DKApiAccess apiAccess;

        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public BarChartViewComponent(DKApiAccess apiAccess) {
            this.apiAccess = apiAccess;
        }

        private static int postIncHue(ref int hue, int hueIncrement) {
            int res = hue; // Store existing hue value
            hue += hueIncrement; // Increment hue
            return res; // Return original hue value
        }

        public async Task<BarChart> ProjectChart() {
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
            var groups = apiDataModel.Data.GroupBy(m => m.OperationCode);
            var values = groups.Select(g => g.Count());
            // Entities are grouped by operation codes which correspond one-to-one
            // with operation descriptions, so they're also grouped by descriptions
            var labels = groups.Select(g => g.First().OperationDescription);
            
            // Generate bar chart colors using a linearly distributed hue, all with
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
            var backgroundColors = groups.Select(g => $"hsla({postIncHue(ref hueBackground, hueIncrement)}, {Bar.BackgroundSaturation}, {Bar.BackgroundLightness}, {Bar.BackgroundAlpha})");
            var hueBorder = 0; // Reset hue for border colors
            var borderColors = groups.Select(g => $"hsla({postIncHue(ref hueBorder, hueIncrement)}, {Bar.BorderSaturation}, {Bar.BorderLightness}, {Bar.BorderAlpha})");

            // Now we have enumerables of background colors, border colors, labels, and counts.
            // We just have to zip the lists together to get a nested quadruple
            var bars = values.Zip(labels.Zip(backgroundColors.Zip(borderColors)))
                               .Select(tuple => new Bar{
                                   Value = tuple.First,
                                   Label = tuple.Second.First,
                                   BackgroundColor = tuple.Second.Second.First,
                                   BorderColor = tuple.Second.Second.Second
                               });

            // Lastly, we just have to return the bar chart
            return new BarChart {
                Bars = bars.ToList(),
                Label = "Number of Operations"
            };
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var barChart = await ProjectChart();
            return await Task.Run(() => View(barChart));
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