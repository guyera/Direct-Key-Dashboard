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
                    Title = "Average OperationDurationMs per device owner",
                    ApiEndpoint = "KeyDeviceActivity",
                    ProjectionResult = ProjectionResult.Number,
                    SummaryMethodDescriptor = SummaryMethodDescriptor.Average,
                    TimeRelative = false,
                    RelativeTimeValue = null,
                    RelativeTimeGranularity = null,
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

        public async Task InsertCustomBarChartAsync(string chartTitle, string apiEndpoint, SummaryMethodDescriptor summaryMethodDescriptor,
                                              ProjectionResult? projectionResult, string categoryPropertyKey, string valuePropertyKey,
                                              bool timeRelative, int? relativeTimeStartValue,
                                              RelativeTimeGranularity? relativeTimeStartGranularity, DateTime? absoluteTimeStartDate,
                                              DateTime? absoluteTimeEndDate, List<string> floatCriteriaJsonPropertyNames,
                                              List<FloatCriterion.Relation> floatCriteriaRelations, List<float> floatCriteriaComparedValues) {
            // Construct a list of float criteria from the individual property lists
            
            var floatCriteria = new List<CustomBarChart.CustomBarChartFloatCriterion>();
            if (floatCriteriaJsonPropertyNames.Count == floatCriteriaRelations.Count
                    && floatCriteriaJsonPropertyNames.Count == floatCriteriaComparedValues.Count) {
                for (var i = 0; i < floatCriteriaJsonPropertyNames.Count; i++) {
                    var criterion = new CustomBarChart.CustomBarChartFloatCriterion() {
                        Key = floatCriteriaJsonPropertyNames[i],
                        Value = floatCriteriaComparedValues[i],
                        Relation = floatCriteriaRelations[i]
                    };
                }
            }

            // Construct the chart and put it in the database
            var chart = new CustomBarChart() {
                Title = chartTitle,
                ApiEndpoint = apiEndpoint,
                SummaryMethodDescriptor = summaryMethodDescriptor,
                ProjectionResult = projectionResult,
                CategoryTokenKey = categoryPropertyKey,
                ValueTokenKey = valuePropertyKey,
                TimeRelative = timeRelative,
                RelativeTimeValue = relativeTimeStartValue,
                RelativeTimeGranularity = relativeTimeStartGranularity,
                IntervalStart = absoluteTimeStartDate,
                IntervalEnd = absoluteTimeEndDate,
                FloatCriteria = floatCriteria
            };

            await _dbContext.CustomBarCharts.AddAsync(chart);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomView(CreateCustomViewModel.ViewType chartViewType, string chartTitle, string apiEndpoint, 
                                              SummaryMethodDescriptor summaryMethodDescriptor, ProjectionResult projectionResult,
                                              string groupedBarChartDatasetPropertyKey, string groupedBarChartCategoryPropertyKey,
                                              List<string> groupedBarChartValuePropertyKeys, string barChartCategoryPropertyKey, string barChartValuePropertyKey,
                                              bool timeRelative, int? relativeTimeStartValue, RelativeTimeGranularity? relativeTimeStartGranularity,
                                              string absoluteTimeStartDate, string absoluteTimeEndDate, List<string> floatCriteriaJsonPropertyNames,
                                              List<FloatCriterion.Relation> floatCriteriaRelations, List<float> floatCriteriaComparedValues) {
            // Attempt to parse date times from datepicker elements if the time is set to an absolute scale
            DateTime? absoluteTimeStartDateTime = null, absoluteTimeEndDateTime = null;
            if (!timeRelative) {
                if (DateTime.TryParseExact(absoluteTimeStartDate, "mm/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var candidate)) {
                    absoluteTimeStartDateTime = candidate;
                }
                
                if (DateTime.TryParseExact(absoluteTimeEndDate, "mm/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out candidate)) {
                    absoluteTimeEndDateTime = candidate;
                }
            }

            switch (chartViewType) {
                case CreateCustomViewModel.ViewType.Bar:
                    await InsertCustomBarChartAsync(chartTitle, apiEndpoint, summaryMethodDescriptor, projectionResult, barChartCategoryPropertyKey,
                                                    barChartValuePropertyKey, timeRelative, relativeTimeStartValue, relativeTimeStartGranularity,
                                                    absoluteTimeStartDateTime, absoluteTimeEndDateTime, floatCriteriaJsonPropertyNames,
                                                    floatCriteriaRelations, floatCriteriaComparedValues);
                    break;
                case CreateCustomViewModel.ViewType.GroupedBar:

                    break;
            }

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
