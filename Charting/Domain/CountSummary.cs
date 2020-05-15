using System.Collections.Generic;
using System.Linq;

// Summarizes multiple datapoints by computing the count.
// This does not require projection (e.g. CountSummary<JObject>
// simply computes the number of JObjects in the filtered dataset,
// which is pretty common)
namespace DirectKeyDashboard.Charting.Domain {
    public class CountSummary<T> : Summary<T, float> {
        // For model binding
        public CountSummary() : base(typeof(CountSummary<T>).FullName) {}
        public override float Summarize(IEnumerable<T> data) => data.Count();
    }
}