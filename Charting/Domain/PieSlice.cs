// A model for a pie slice, including all information necessary
// to represent a single slice of a pie chart

namespace DirectKeyDashboard.Charting.Domain {
    public class PieSlice {
        public const string BackgroundSaturation = "100%";
        public const string BackgroundLightness = "50%";
        public const string BackgroundAlpha = "0.5";
        public const string BorderSaturation = "100%";
        public const string BorderLightness = "50%";
        public const string BorderAlpha = "1.0";
        // Count represents the total number
        // of entries in this category. Used by
        // ChartJS to calculate proportions for
        // display.
        public int Count {get; set;}

        // Label for display
        public string Label {get; set;}

        // Colors for display
        public string BackgroundColor {get; set;}
        public string BorderColor {get; set;}
    }
}