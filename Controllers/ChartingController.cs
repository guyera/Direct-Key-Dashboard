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
        
        public IActionResult CreateCustomView() {
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
        public IActionResult StringCountApiLineChart(CountSummary<string> summary, Filter<ProjectionCriterion<string, SimpleProjection<string>>> preFilter, Filter<ProjectionCriterion<string, CategoryProjection<string, SimpleGroupedProjection<string>>>> filter, TimeSeries timeSeries, ValueProjection<string, SimpleGroupedProjection<string>> projection) {
            return ViewComponent(typeof(StringCountApiLineChartViewComponent), new {
                summary,
                preFilter,
                filter,
                timeSeries,
                projection
            });
        }

        [HttpPost]
        public IActionResult NonProjectingApiLineChart(Summary<JObject, float> summary, Filter<Criterion> preFilter, Filter<CategorizerCriterion> filter, TimeSeries timeSeries) {
            return ViewComponent(typeof(NonProjectingApiLineChartViewComponent), new {
                preFilter,
                filter = new Filter<Criterion>(filter.Criteria),
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
