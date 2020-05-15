using System.Collections.Generic;
using System.Linq;

// Summarizes a series of data points by computing
// the median (requires projection to float)
namespace DirectKeyDashboard.Charting.Domain
{
    public class MedianSummary : Summary<float, float> {
        public MedianSummary() : base(typeof(MedianSummary).FullName) {}
        public override float Summarize(IEnumerable<float> data) {
            var orderedData = data.OrderBy(d => d);
            var count = orderedData.Count();
            // If there are no elements, return zero as the median.
            // If there is an odd number of elements, take the middle one.
            // If there is an even number, take the average of the middle two.
            return count == 0 ? 0 : (count % 2 == 1 ? orderedData.Skip(count / 2).First() : orderedData.Skip(count / 2 - 1).Take(2).Average());
        }
    }
}