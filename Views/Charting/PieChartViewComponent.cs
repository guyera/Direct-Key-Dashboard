using System.Collections.Generic;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;

namespace DirectKeyDashboard.Views.Charting
{
    // Creates a test bar chart
    [ViewComponent(Name = "PieChart")]
    public abstract class PieChartViewComponent : ViewComponent {
        protected DKApiAccess apiAccess;

        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        protected PieChartViewComponent(DKApiAccess apiAccess) {
            this.apiAccess = apiAccess;
        }

        protected static int postIncHue(ref int hue, int hueIncrement) {
            int res = hue; // Store existing hue value
            hue += hueIncrement; // Increment hue
            return res; // Return original hue value
        }

        public abstract Task<PieChart> ProjectChart();

        public async Task<IViewComponentResult> InvokeAsync() {
            var pieChart = await ProjectChart();
            return await Task.Run(() => View(pieChart));
        }
    }
}