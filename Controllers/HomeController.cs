using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DirectKeyDashboard.Models;
using InformationLibraries;
using System.Threading.Tasks;
using DirectKeyDashboard.Views.Home;

namespace DirectKeyDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> ApiTest(DKApiAccess apiAccess)
        {
            var online = await apiAccess.IsResponding();
            var responseString = await apiAccess.PullNewest();
            return View(new ApiTestViewModel(online, responseString));
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
