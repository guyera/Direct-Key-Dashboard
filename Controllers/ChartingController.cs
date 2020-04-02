using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DirectKeyDashboard.Models;
using DirectKeyDashboard.Views.Charting;
using System.Collections.Generic;
using DirectKeyDashboard.Charting.Domain;
using System;
using System.Globalization;

namespace DirectKeyDashboard.Controllers
{
    public class ChartingController : Controller
    {
        private readonly ILogger<ChartingController> _logger;

        public ChartingController(ILogger<ChartingController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult ViewOne() {
            return View();
        }
        
        public IActionResult ViewTwo() {
            return View();
        }

        public IActionResult ViewThree() {
            return View();
        }

        public IActionResult ViewFour() {
            return View();
        }

        // public IActionResult FloatProjectingApiLineChart() {
        //     return ViewComponent(typeof(FloatProjectingApiLineChartViewComponent), new {
        //         summary = new AverageSummary(),
        //         timeSeries = new TimeSeries(new List<TimeInterval>() {
        //             new TimeInterval(DateTime.ParseExact("2019-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-06-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "June"),
        //             new TimeInterval(DateTime.ParseExact("2019-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-07-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "July"),
        //             new TimeInterval(DateTime.ParseExact("2019-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-08-31", "yyyy-MM-dd", CultureInfo.InvariantCulture), "August"),
        //             new TimeInterval(DateTime.ParseExact("2019-09-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-09-30", "yyyy-MM-dd", CultureInfo.InvariantCulture), "September")
        //         }),
        //         projection = new SimpleProjection<float>("OperationDurationMs")
        //     });
        // }

        [HttpPost]
        public IActionResult FloatProjectingApiLineChart(AverageSummary summary, Filter<FloatCriterion> preFilter, Filter<ProjectionCriterion<string, CategoryProjection<IDictionary<string, float>, SimpleCompositeGroupedProjection<float>>>> filter, TimeSeries timeSeries, CompositeValueProjection<float, SimpleCompositeGroupedProjection<float>> projection) {
            return ViewComponent(typeof(FloatProjectingApiLineChartViewComponent), new {
                summary,
                preFilter,
                filter,
                timeSeries,
                projection
            });
        }

        [HttpPost]
        public IActionResult StringProjectingApiLineChart(CountSummary<string> summary, Filter<ProjectionCriterion<string, SimpleProjection<string>>> preFilter, Filter<ProjectionCriterion<string, CategoryProjection<string, SimpleGroupedProjection<string>>>> filter, TimeSeries timeSeries, ValueProjection<string, SimpleGroupedProjection<string>> projection) {
            return ViewComponent(typeof(StringProjectingApiLineChartViewComponent), new {
                summary,
                preFilter,
                filter,
                timeSeries,
                projection
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
