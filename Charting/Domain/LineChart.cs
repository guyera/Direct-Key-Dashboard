// A model for a full line chart to be displayed via a
// ViewComponent

using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain {
    public class LineChart {
        public IList<Line> Lines {get; set;}
        public IList<string> CategoryLabels {get; set;}
    }
}