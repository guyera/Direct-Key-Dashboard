using System.Collections.ObjectModel;
using System.Linq;

namespace DirectKeyDashboard.Charting.Domain
{
    public class MedianSummary : Summary<float, float> {
        public override float Summarize(Collection<float> data) {
            var orderedData = data.OrderBy(d => d);
            var count = orderedData.Count();
            // If there are no elements, return zero as the median.
            // If there is an odd number of elements, take the middle one.
            // If there is an even number, take the average of the middle two.
            return count == 0 ? 0 : (count % 2 == 1 ? orderedData.Skip(count / 2).First() : orderedData.Skip(count / 2 - 1).Take(2).Average());
        }
    }
}