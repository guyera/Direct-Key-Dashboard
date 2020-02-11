// A model for a full bar chart to be displayed via a
// ViewComponent

using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain {
    public class Line {
        public IList<Vertex> Vertices {get; set;}
        public string Color {get; set;}
        public string PointColor {get; set;}
        public string Label {get; set;}
    }
}