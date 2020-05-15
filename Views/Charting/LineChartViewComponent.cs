using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;

// Abstract base class used to represent line charts,
// including some helper functions.

// TODO Refactor helper functions
namespace DirectKeyDashboard.Views.Charting
{
    // Creates a test line chart
    [ViewComponent(Name = "LineChart")]
    public abstract class LineChartViewComponent : ViewComponent {
        protected DKApiAccess apiAccess;

        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        protected LineChartViewComponent(DKApiAccess apiAccess) {
            this.apiAccess = apiAccess;
        }

        protected static int postIncHue(ref int hue, int hueIncrement) {
            int res = hue; // Store existing hue value
            hue += hueIncrement; // Increment hue
            return res; // Return original hue value
        }
    }
}