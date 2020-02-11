using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

//TODO Stop storing time intervals as filters and criterion. Instead,
//TODO filter simply using the REST api filters, since that's the
//TODO easiest way to ensure that data is retrieved from each interval.
//TODO Using a single overarching interval with takes=2000 may only take
//TODO from the first few days, depending on the data distribution.

namespace DirectKeyDashboard.Views.Charting
{
    // Represents a line chart which projects data from the API.
    // The data is filtered by the Filter model supplied,
    // projected by the Projection model, and summarized
    // by the Summary model.
    public class ApiLineChartViewComponent : LineChartViewComponent {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public ApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<LineChart> ProjectChart(LineChartContext ctx) {
            // For each time interval, add a datum to the dataset
            var vertices = new List<Vertex>();
            foreach (var filter in ctx.TimeFilters) {
                var criterion = filter.GetTimeIntervalCriterion();
                var rawData = await apiAccess.PullKeyDeviceActivity(criterion.Start, criterion.End);
                // Parse string to JObject
                var rootObject = JObject.Parse(rawData);
                if (!rootObject.TryGetValue("Data", out var dataArrayToken)) {
                    throw new JsonArgumentException();
                }

                // Convert data array token to JEnumerable and
                // filter out unwanted data
                var dataArray = dataArrayToken.AsJEnumerable();
                dataArray = ctx.Filter.FilterData(dataArray);

                // Filter dataArray by time interval TODO remove this part, as stated
                var timedDataArray = filter.FilterData(dataArray);
                // Project each token to a value
                var projectedData = new Collection<float>();
                foreach (var token in timedDataArray) {
                    if (token.Type != JTokenType.Object) {
                        throw new JsonArgumentException();
                    }
                    try{
                        projectedData.Add(ctx.Projection.Project((JObject) token));
                    } catch(JsonArgumentException) {}
                }

                // Summarize the data and add it to the dataset
                Console.WriteLine($"Num data: {projectedData.Count()}");
                if (projectedData.Any()) {
                    var datum = ctx.Summary.Summarize(projectedData);
                    vertices.Add(new Vertex(datum));
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
                CategoryLabels = ctx.TimeFilters.Select(f => f.Name).ToList()
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(Summary<float> summary, Filter filter, IList<TimeFilter> timeFilters, Projection projection) {
            var lineChart = await ProjectChart(new LineChartContext(summary, filter, timeFilters, projection));
            return await Task.Run(() => View(lineChart));
        }

        protected class LineChartContext {
            public Summary<float> Summary {get;}
            public Filter Filter {get;}
            public IList<TimeFilter> TimeFilters {get;}
            public Projection Projection {get;}
            public LineChartContext(Summary<float> summary, Filter filter, IList<TimeFilter> timeFilters, Projection projection) {
                Summary = summary;
                Filter = filter;
                TimeFilters = timeFilters;
                Projection = projection;
            }
        }
    }
}