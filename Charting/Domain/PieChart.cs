// A model for a full pie chart to be displayed via a
// ViewComponent

using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain {
    public class PieChart {
        // A list of pie slices which constitute the chart.
        // An IList is used as the order helps determine
        // how the slices are displayed, so it should
        // be customizable.
        public IList<PieSlice> Slices {get; set;}
        public string Label {get; set;}
    }
}