using System.Collections.Generic;
using DirectKeyDashboard.Charting.Domain;

namespace DirectKeyDashboard.Views.Charting {
    public class IndexModel {
        public List<CustomBarChart> CustomBarCharts {get; set;}

        public List<CustomGroupedBarChart> CustomGroupedBarCharts {get; set;}
    }
}