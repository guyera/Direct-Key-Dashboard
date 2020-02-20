using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;

namespace DirectKeyDashboard.Views.Charting
{
    // Creates a test bar chart
    [ViewComponent(Name = "GroupedBarChart")]
    public abstract class GroupedBarChartViewComponent : ViewComponent {
        protected DKApiAccess apiAccess;

        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        protected GroupedBarChartViewComponent(DKApiAccess apiAccess) {
            this.apiAccess = apiAccess;
        }

        protected static int PostIncHue(ref int hue, int hueIncrement) {
            int res = hue; // Store existing hue value
            hue += hueIncrement; // Increment hue
            return res; // Return original hue value
        }
    }
}