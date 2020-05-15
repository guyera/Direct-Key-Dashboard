using System.Collections.Generic;

// For representing multiple time intervals to filter
// the data to be projected by a chart, mostly for
// line charts.
namespace DirectKeyDashboard.Charting.Domain {
    public class TimeSeries {
        public IEnumerable<TimeInterval> TimeIntervals {get; set;}

        // For model bounding
        public TimeSeries(){}

        public TimeSeries(IEnumerable<TimeInterval> timeIntervals) {
            TimeIntervals = timeIntervals;
        }
    }
}