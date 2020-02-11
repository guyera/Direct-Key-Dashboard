using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectKeyDashboard.Charting.Domain
{
    // A time filter is a named filter whose
    // name denotes some time interval,
    // and whose criteria restrict a time
    // property to a certain range
    public class TimeFilter : NamedFilter {
        public TimeFilter(string name, string tokenKey, DateTime start, DateTime end) : base(new List<Criterion>(){
                new TimeIntervalCriterion(tokenKey, start, end)
            }, name) {}

        public TimeIntervalCriterion GetTimeIntervalCriterion() {
            return Criteria.First() as TimeIntervalCriterion;
        }
    }
}