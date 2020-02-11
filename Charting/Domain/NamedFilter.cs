using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain
{
    // A named filter is useful for representing
    // e.g. time intervals. A time interval
    // has a label (e.g. "March" or "3:00 PM")
    // as well as a list of criteria in order for
    // an object to fall within that time interval
    public class NamedFilter : Filter {
        public string Name {get;}

        public NamedFilter(IEnumerable<Criterion> criteria, string name) : base(criteria) {
            Name = name;
        }
    }
}