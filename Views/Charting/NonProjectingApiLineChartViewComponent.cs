using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Views.Charting
{
    // Represents a line chart which categorizes but does not
    // project data from the API
    public class NonProjectingApiLineChartViewComponent : LineChartViewComponent {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public NonProjectingApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<LineChart> ProjectChart(Summary<JObject, float> summary, Filter<Criterion> preFilter,
                Filter<Criterion> filter, TimeSeries timeSeries) {
            // For each time interval, add a datum to the dataset
            var vertices = new List<Vertex>();
            foreach (var interval in timeSeries.TimeIntervals) {
                var rawData = await apiAccess.PullKeyDeviceActivity(interval.Start, interval.End);
                // Parse string to JObject
                var rootObject = JObject.Parse(rawData);
                if (!rootObject.TryGetValue("Data", out var dataArrayToken)) {
                    throw new JsonArgumentException();
                }

                // Convert data array token to JEnumerable and
                // filter out unwanted data
                var dataArray = dataArrayToken.AsJEnumerable();
                dataArray = preFilter.FilterData(dataArray);
                dataArray = filter.FilterData(dataArray);

                // Summarize the data and add it to the dataset
                var objList = dataArray.Where(token => token is JObject).Select(token => token as JObject);
                if (objList.Any()) {
                    var datum = summary.Summarize(objList);
                    vertices.Add(new Vertex(datum));
                } else {
                    vertices.Add(new Vertex(0));
                }
            }

            return new LineChart() {
                Lines = new List<Line>() {
                    new Line() {
                        Vertices = vertices,
                        Label = "Example label",
                        Color = "hsla(0, 100%, 50%, 0.3)",
                        PointColor = "hsla(0, 100%, 50%, 0.3)"
                    }
                },
                CategoryLabels = timeSeries.TimeIntervals.Select(f => f.Name).ToList()
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(Summary<JObject, float> summary, Filter<Criterion> preFilter,
                Filter<Criterion> filter, TimeSeries timeSeries) {
            var lineChart = await ProjectChart(summary, preFilter, filter, timeSeries);
            return await Task.Run(() => View(lineChart));
        }
    }
}