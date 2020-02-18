using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class CountSummary<T> : Summary<T, float> {
        public override float Summarize(Collection<T> data) => data.Count();
    }
}