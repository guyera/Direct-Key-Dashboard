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
using Microsoft.EntityFrameworkCore;

// The primary controller used to display and interact with
// charts.
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
            return View(new IndexModel() { 
                CustomBarCharts = _dbContext.CustomBarCharts.ToList(),
                CustomGroupedBarCharts = _dbContext.CustomGroupedBarCharts.ToList()
            });
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
                    floatCriteria.Add(new CustomBarChart.CustomBarChartFloatCriterion() {
                        Key = floatCriteriaJsonPropertyNames[i],
                        Value = floatCriteriaComparedValues[i],
                        Relation = floatCriteriaRelations[i]
                    });
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

        public async Task InsertCustomGroupedBarChartAsync(string chartTitle, string apiEndpoint, SummaryMethodDescriptor summaryMethodDescriptor,
                                              ProjectionResult? projectionResult, string datasetPropertyKey, string categoryPropertyKey,
                                              List<string> valuePropertyKeys, bool timeRelative, int? relativeTimeStartValue,
                                              RelativeTimeGranularity? relativeTimeStartGranularity, DateTime? absoluteTimeStartDate,
                                              DateTime? absoluteTimeEndDate, List<string> floatCriteriaJsonPropertyNames,
                                              List<FloatCriterion.Relation> floatCriteriaRelations, List<float> floatCriteriaComparedValues) {
            // Construct a list of float criteria from the individual property lists
            
            var floatCriteria = new List<CustomGroupedBarChart.CustomGroupedBarChartFloatCriterion>();
            if (floatCriteriaJsonPropertyNames.Count == floatCriteriaRelations.Count
                    && floatCriteriaJsonPropertyNames.Count == floatCriteriaComparedValues.Count) {
                for (var i = 0; i < floatCriteriaJsonPropertyNames.Count; i++) {
                    floatCriteria.Add(new CustomGroupedBarChart.CustomGroupedBarChartFloatCriterion() {
                        Key = floatCriteriaJsonPropertyNames[i],
                        Value = floatCriteriaComparedValues[i],
                        Relation = floatCriteriaRelations[i]
                    });
                }
            }

            // Construct a list of value token keys (entities)
            // from the list of valuePropertyKeys
            var valueTokenKeys = new List<CustomGroupedBarChart.CustomGroupedBarChartValueTokenKey>();
            foreach (var valuePropertyKey in valuePropertyKeys) {
                valueTokenKeys.Add(new CustomGroupedBarChart.CustomGroupedBarChartValueTokenKey() {
                    Key = valuePropertyKey
                });
            }

            // Construct the chart and put it in the database
            var chart = new CustomGroupedBarChart() {
                Title = chartTitle,
                ApiEndpoint = apiEndpoint,
                ProjectionResult = projectionResult,
                SummaryMethodDescriptor = summaryMethodDescriptor,
                FloatCriteria = floatCriteria,
                TimeRelative = timeRelative,
                RelativeTimeValue = relativeTimeStartValue,
                RelativeTimeGranularity = relativeTimeStartGranularity,
                IntervalStart = absoluteTimeStartDate,
                IntervalEnd = absoluteTimeEndDate,
                DatasetTokenKey = datasetPropertyKey,
                CategoryTokenKey = categoryPropertyKey,
                ValueTokenKeys = valueTokenKeys
            };

            await _dbContext.CustomGroupedBarCharts.AddAsync(chart);
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
                if (DateTime.TryParseExact(absoluteTimeStartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var candidate)) {
                    absoluteTimeStartDateTime = candidate;
                }
                
                if (DateTime.TryParseExact(absoluteTimeEndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out candidate)) {
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
                    await InsertCustomGroupedBarChartAsync(chartTitle, apiEndpoint, summaryMethodDescriptor, projectionResult, groupedBarChartDatasetPropertyKey,
                                                    groupedBarChartCategoryPropertyKey, groupedBarChartValuePropertyKeys, timeRelative, relativeTimeStartValue,
                                                    relativeTimeStartGranularity, absoluteTimeStartDateTime, absoluteTimeEndDateTime, floatCriteriaJsonPropertyNames,
                                                    floatCriteriaRelations, floatCriteriaComparedValues);
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
            var chart = await _dbContext.CustomBarCharts.Include(c => c.FloatCriteria)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (chart == null) {
                throw new ArgumentException($"Failed to find chart with the id {id}");
            }
            return View(chart);
        }

        public async Task<IActionResult> CustomGroupedBarChartView(Guid id) {
            var chart = await _dbContext.CustomGroupedBarCharts.Include(c => c.FloatCriteria)
                .Include(c => c.ValueTokenKeys)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chart == null) {
                throw new ArgumentException($"Failed to find chart with the id {id}");
            }
            
            return View(chart);
        }

        [HttpPost]
        public IActionResult FloatProjectingApiLineChart(Summary<float, float> summary, Filter<Criterion> preFilter, Filter<Criterion> filter, TimeSeries timeSeries, Projection<float> projection) {
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
