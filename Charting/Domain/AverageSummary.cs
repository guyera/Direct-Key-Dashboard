using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class AverageSummary : Summary<float, float> {
        public override float Summarize(IEnumerable<float> data) => data.Average();
    }
}