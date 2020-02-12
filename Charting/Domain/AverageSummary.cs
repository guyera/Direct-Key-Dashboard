using System.Collections.Generic;
using System.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class AverageSummary : Summary<float> {
        public override float Summarize(IEnumerable<float> data) => data.Average();
    }
}