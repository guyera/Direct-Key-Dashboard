using System.Collections.Generic;
using System.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class CountSummary<T> : Summary<T, float> {
        // For model binding
        public CountSummary() : base(typeof(CountSummary<T>).FullName) {}
        public override float Summarize(IEnumerable<T> data) => data.Count();
    }
}