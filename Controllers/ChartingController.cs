using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DirectKeyDashboard.Models;
using InformationLibraries;
using System.Threading.Tasks;
using DirectKeyDashboard.Views.Charting;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
