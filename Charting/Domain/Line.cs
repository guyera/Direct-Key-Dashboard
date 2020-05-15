using System.Collections.Generic;

// One line in a line chart (one dataset)
namespace DirectKeyDashboard.Charting.Domain {
    public class Line {
        public IList<Vertex> Vertices {get; set;}
        public string Color {get; set;}
        public string PointColor {get; set;}
        public string Label {get; set;}
    }
}