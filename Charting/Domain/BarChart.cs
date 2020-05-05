// A model for a full bar chart to be displayed via a
// ViewComponent

using System;
using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain {
    public class BarChart {
        // A list of bars which constitute the chart.
        // An IList is used as the order helps determine
        // how the slices are displayed, so it should
        // be customizable.
        public IList<Bar> Bars {get; set;}
        public string Label {get; set;}
    }
}