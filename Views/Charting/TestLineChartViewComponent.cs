using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;
using Microsoft.AspNetCore.Mvc;

namespace DirectKeyDashboard.Views.Charting
{
    // Creates a test line chart
    public class TestLineChartViewComponent : LineChartViewComponent {
        // Inject DKApiAccess with dependency injection so that
        // this view component can access the API
        public TestLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}

        protected virtual async Task<LineChart> ProjectChart() {
            return await Task.Run(() => {
                var values = new int[] {1, 2, 3, 2, 1, 2, 3, 2, 1};
                var categoryLabels = new List<string>(){
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September"
                };
                LineChart test = new LineChart() {
                    CategoryLabels = categoryLabels,
                    Lines = new List<Line>() {
                        new Line() {
                            Vertices = values.Select(v => new Vertex(v)).ToList(),
                            Label = "TestDataset",
                            Color = "hsla(120, 100%, 50%, 0.3)",
                            PointColor = "hsla(120, 100%, 50%, 0.3)",
                        },
                        new Line() {
                            Vertices = values.Select(v => new Vertex(v*2)).ToList(),
                            Label = "TestDataset",
                            Color = "hsla(0, 100%, 50%, 0.3)",
                            PointColor = "hsla(0, 100%, 50%, 0.3)",
                        }
                    }
                };
                return test;
            });
        }

        public virtual async Task<IViewComponentResult> InvokeAsync() {
            var lineChart = await ProjectChart();
            return await Task.Run(() => View(lineChart));
        }
    }
}