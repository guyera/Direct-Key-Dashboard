using System.Collections.Generic;
using System.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class CountSummary<T> : Summary<T, float> {
        public override float Summarize(IEnumerable<T> data) => data.Count();
    }
}