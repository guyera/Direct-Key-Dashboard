using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Views.Charting
{
    // Represents a line chart which projects data from the API.
    // The data is filtered by the Filter model supplied,
    // projected by the Projection model, and summarized
    // by the Summary model.
    public class ApiLineChartViewComponent<TProjection, TCriterion> : LineChartViewComponent
            where TCriterion : Criterion {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public ApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<LineChart> ProjectChart(LineChartContext ctx, Projection<TProjection> projection, Summary<TProjection, float> summary) {
            // For each time interval, add a datum to the dataset
            var vertices = new List<Vertex>();
            Console.WriteLine($"Num time series: {ctx.TimeSeries.TimeIntervals.Count()}");
            foreach (var interval in ctx.TimeSeries.TimeIntervals) {
                var rawData = await apiAccess.PullKeyDeviceActivity(interval.Start, interval.End);
                // Parse string to JObject
                var rootObject = JObject.Parse(rawData);
                if (!rootObject.TryGetValue("Data", out var dataArrayToken)) {
                    throw new JsonArgumentException();
                }

                // Convert data array token to JEnumerable and
                // filter out unwanted data
                var dataArray = dataArrayToken.AsJEnumerable();
                Console.WriteLine($"Unfiltered count: {dataArray.Count()}");
                dataArray = ctx.Filter.FilterData(dataArray);
                Console.WriteLine($"Filtered count: {dataArray.Count()}");

                // Project each token to a value
                var projectedData = new Collection<TProjection>();
                foreach (var token in dataArray) {
                    if (token.Type != JTokenType.Object) {
                        throw new JsonArgumentException();
                    }
                    try{
                        projectedData.Add(projection.Project((JObject) token));
                    } catch(JsonArgumentException) {}
                }

                // Summarize the data and add it to the dataset
                if (projectedData.Any()) {
                    var datum = summary.Summarize(projectedData);
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
                CategoryLabels = ctx.TimeSeries.TimeIntervals.Select(f => f.Name).ToList()
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(Summary<TProjection, float> summary, Filter<TCriterion> filter, TimeSeries timeSeries, Projection<TProjection> projection) {
            var lineChart = await ProjectChart(new LineChartContext(filter, timeSeries), projection, summary);
            return await Task.Run(() => View(lineChart));
        }

        protected class LineChartContext {
            public Filter<TCriterion> Filter {get;}
            public TimeSeries TimeSeries {get;}

            public LineChartContext(Filter<TCriterion> filter, TimeSeries timeSeries) {
                Filter = filter;
                TimeSeries = timeSeries;
            }
        }
    }
}