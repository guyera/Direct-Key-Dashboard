// A model for a bar, including all information necessary
// to represent a single bar of a bar chart

using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain {
    public class BarGroup {
        public const string BackgroundSaturation = "100%";
        public const string BackgroundLightness = "50%";
        public const string BackgroundAlpha = "0.5";
        public const string BorderSaturation = "100%";
        public const string BorderLightness = "50%";
        public const string BorderAlpha = "1.0";

        // List of values within this category
        public IList<int> Values {get; set;}

        // Label for display
        public string Label {get; set;}

        // Colors for display
        public string BackgroundColor {get; set;}
        public string BorderColor {get; set;}
    }
}