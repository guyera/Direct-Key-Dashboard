using System;

namespace DirectKeyDashboard.Charting.Domain {
    public class TimeInterval {
        public DateTime Start {get;}
        public DateTime End {get;}
        public string Name {get;}

        public TimeInterval(DateTime start, DateTime end, string name) {
            Start = start;
            End = end;
            Name = name;
        }
    }
}