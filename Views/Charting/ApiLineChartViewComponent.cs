using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

/* TODO Reduce the generics used and increase polymorphism in its place.
    e.g. instead of requiring TCriteria as a generic type parameter,
    simply accept List<Criteria> as an invocation argument. 
    The only reason to use generics and generic type constraints enforcing
    inheritance is to allow more flexibility with the type. e.g. IList<Dog>
    is not substitutable for IList<Animal> (though covariance IS allowed with
    classes, just not interfaces), or to guarantee that types match in certain
    situations (e.g. the summary takes as an argument a list of the same type which the
    projection produces). However, I do not need such flexibility, nor do I need
    such constraints with summaries, projections, or criteria. The only
    real case for generics here is TProjection (which should probably
    be named to TProjectionValue, as it represents the type of the thing
    which is projected, not the type of the projection itself), for the
    reason mentioned (summary must be able to summarize the type of thing
    which was projected) */

namespace DirectKeyDashboard.Views.Charting
{
    // Represents a line chart which projects data from the API.
    // The data is filtered by the Filter model supplied,
    // projected by the Projection model, and summarized
    // by the Summary model.
    public class ApiLineChartViewComponent<TProjection, TPreCriterion, TCriterion> : LineChartViewComponent
            where TPreCriterion : Criterion
            where TCriterion : Criterion {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public ApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<LineChart> ProjectChart(Filter<TPreCriterion> preFilter, LineChartContext ctx, Projection<TProjection> projection, Summary<TProjection, float> summary) {
            // For each time interval, add a datum to the dataset
            var vertices = new List<Vertex>();
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
                if (summary == null)
                    Console.WriteLine("Summary is null");
                if (preFilter == null)
                    Console.WriteLine("Prefilter is null");
                else if (preFilter.Criteria == null)
                    Console.WriteLine("Prefilter criteria is null");
                if (ctx.Filter == null)
                    Console.WriteLine("Filter is null");
                else if (ctx.Filter.Criteria == null)
                    Console.WriteLine("Filter criteria is null");
                dataArray = preFilter.FilterData(dataArray);
                dataArray = ctx.Filter.FilterData(dataArray);

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
                CategoryLabels = ctx.TimeSeries.TimeIntervals.Select(f => f.Name).ToList()
            };
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(Filter<TPreCriterion> preFilter, Summary<TProjection, float> summary, Filter<TCriterion> filter, TimeSeries timeSeries, Projection<TProjection> projection) {
            var lineChart = await ProjectChart(preFilter, new LineChartContext(filter, timeSeries), projection, summary);
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