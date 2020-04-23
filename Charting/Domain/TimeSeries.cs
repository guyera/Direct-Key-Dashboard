using System.Collections.Generic;

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