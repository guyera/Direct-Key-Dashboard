using System;

// For representing a time interval to filter
// the data to be projected by a chart.
namespace DirectKeyDashboard.Charting.Domain {
    public class TimeInterval {
        public DateTime Start {get; set;}
        public DateTime End {get; set;}
        public string Name {get; set;}

        // For model binding
        public TimeInterval(){}
        public TimeInterval(DateTime start, DateTime end, string name) {
            Start = start;
            End = end;
            Name = name;
        }
    }

    public enum RelativeTimeGranularity {
        Day,
        Month,
        Year
    }
}