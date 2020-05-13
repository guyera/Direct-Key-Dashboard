using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DirectKeyDashboard.Models;
using DirectKeyDashboard.Views.Charting;
using System.Collections.Generic;
using DirectKeyDashboard.Charting.Domain;
using DirectKeyDashboard.Data;
using System.Linq;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

/* TODO Custom model binding for polymorphic types like summaries, projections, and filters */

namespace DirectKeyDashboard.Controllers
{
    public class ChartingController : Controller
    {
        private readonly ILogger<ChartingController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ChartingController(ILogger<ChartingController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index() {
            if (!_dbContext.CustomBarCharts.Any()) {
                _dbContext.CustomBarCharts.Add(new CustomBarChart() {
                    Id = Guid.NewGuid(),
                    Name = "Average OperationDurationMs per device owner",
                    ApiEndpoint = "KeyDeviceActivity",
                    ProjectionResult = ProjectionResult.Number,
                    SummaryMethod = SummaryMethod.Average,
                    CriterionType = CriterionType.Float,
                    FloatCriteria = new List<CustomBarChart.CustomBarChartFloatCriterion>(),
                    IntervalStart = DateTime.ParseExact("2019-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    IntervalEnd = DateTime.ParseExact("2019-06-30", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    CategoryTokenKey = "DeviceOwnerID",
                    ValueTokenKey = "OperationDurationMs"
                });
                _dbContext.SaveChanges();
            }
            return View(_dbContext.CustomBarCharts.ToList());
        }
        
        [HttpGet]
        public IActionResult CreateCustomView() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomView(CreateCustomViewModel.ViewType chartViewType, string chartTitle, string apiEndpoint, SummaryMethod summaryMethod,
                                              ProjectionResult projectionResult, string groupedBarChartDatasetPropertyKey, string groupedBarChartCategoryPropertyKey,
                                              List<string> groupedBarChartValuePropertyKeys, string barChartCategoryPropertyKey, string barChartValuePropertyKey,
                                              bool timeRelative, int relativeTimeStartValue, CreateCustomViewModel.TimeGranularity relativeTimeStartGranularity,
                                              string absoluteTimeStartDate, string absoluteTimeEndDate, List<string> floatCriteriaJsonPropertyNames,
                                              List<FloatCriterion.Relation> floatCriteriaRelations, List<float> floatCriteriaComparedValues) {
            
            Console.WriteLine($"View type: {chartViewType}");
            Console.WriteLine($"Chart title: {chartTitle}");
            Console.WriteLine($"Api endpoint: {apiEndpoint}");
            Console.WriteLine($"Summary method: {summaryMethod}");
            Console.WriteLine($"Projection result: {projectionResult}");
            Console.WriteLine($"groupedBarChartDatasetPropertyKey: {groupedBarChartDatasetPropertyKey}");
            Console.WriteLine($"groupedBarChartCategoryPropertyKey: {groupedBarChartCategoryPropertyKey}");
            Console.WriteLine($"groupedBarChartValuePropertyKeys length: {groupedBarChartValuePropertyKeys.Count}");
            Console.WriteLine($"barChartCategoryPropertyKey: {barChartCategoryPropertyKey}");
            Console.WriteLine($"barChartValuePropertyKey: {barChartValuePropertyKey}");
            Console.WriteLine($"timeRelative: {timeRelative}");
            Console.WriteLine($"relativeTimeStartValue: {relativeTimeStartValue}");
            Console.WriteLine($"relativeTimeStartGranularity: {relativeTimeStartGranularity}");
            Console.WriteLine($"absoluteTimeStartDate: {absoluteTimeStartDate}");
            Console.WriteLine($"absoluteTimeEndDate: {absoluteTimeEndDate}");
            Console.WriteLine($"floatCriteriaJsonPropertyNames length: {floatCriteriaJsonPropertyNames.Count}");
            Console.WriteLine($"floatCriteriaRelations length: {floatCriteriaRelations.Count}");
            Console.WriteLine($"floatCriteriaComparedValues length: {floatCriteriaComparedValues.Count}");

            return RedirectToAction("Index");
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

        public IActionResult ViewFive() {
            return View();
        }

        public async Task<IActionResult> CustomBarChartView(Guid id) {
            Console.WriteLine($"ID: {id}");
            var chart = await _dbContext.CustomBarCharts.FindAsync(id);
            if (chart == null) {
                throw new ArgumentException($"Failed to find chart with the id {id}");
            }
            return View(chart);
        }

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
        public IActionResult StringCountApiLineChart(Summary<string, float> summary, Filter<Criterion> preFilter, Filter<Criterion> filter, TimeSeries timeSeries, Projection<string> projection) {
            return ViewComponent(typeof(StringCountApiLineChartViewComponent), new {
                summary,
                preFilter,
                filter,
                timeSeries,
                projection
            });
        }

        [HttpPost]
        public IActionResult NonProjectingApiLineChart(Summary<JObject, float> summary, Filter<Criterion> preFilter, Filter<Criterion> filter, TimeSeries timeSeries) {
            return ViewComponent(typeof(NonProjectingApiLineChartViewComponent), new {
                preFilter,
                filter,
                summary,
                timeSeries
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
