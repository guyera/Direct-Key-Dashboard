using System;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    // Representation of a time interval criterion,
    // restricting a value to a particular time
    // interval
    public class TimeIntervalCriterion : Criterion {
        public DateTime Start {get;}
        public DateTime End {get;}

        public TimeIntervalCriterion(string key, DateTime start, DateTime end) : base(key) {
            Start = start;
            End = end;
        }

        public override bool SatisfiedBy(JObject jobject) {
            if (!jobject.TryGetValue(Key, out var token) || (token.Type != JTokenType.String && token.Type != JTokenType.Date)) {
                return false;
            }

            DateTime dateTime;

            if (token.Type == JTokenType.String) {
                var value = (string) token.ToObject(typeof(string));
                if (value == null) {
                    return false;
                }

                if (!DateTime.TryParse(value, out dateTime)) {
                    return false;
                }
            } else {
                dateTime = (DateTime) token.ToObject(typeof(DateTime));
                if (dateTime == null) {
                    return false;
                }
            }

            return dateTime.CompareTo(Start) > 0 && dateTime.CompareTo(End) < 0;
        }
    }
}