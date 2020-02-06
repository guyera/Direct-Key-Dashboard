// A model for a bar, including all information necessary
// to represent a single bar of a bar chart

namespace DirectKeyDashboard.Charting.Domain {
    public class Bar {
        public const string BackgroundSaturation = "100%";
        public const string BackgroundLightness = "50%";
        public const string BackgroundAlpha = "0.5";
        public const string BorderSaturation = "100%";
        public const string BorderLightness = "50%";
        public const string BorderAlpha = "1.0";

        // Value is essentially the "y value" of the sample point,
        // or the relative "height" of the bar.
        public int Value {get; set;}

        // Label for display
        public string Label {get; set;}

        // Colors for display
        public string BackgroundColor {get; set;}
        public string BorderColor {get; set;}
    }
}