using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

// Summarizes several numerical datapoints by computing the average
namespace DirectKeyDashboard.Charting.Domain {
    public class AverageSummary : Summary<float, float> {
        public AverageSummary() : base(typeof(AverageSummary).FullName) {}
        public override float Summarize(IEnumerable<float> data) => data.Average();
    }
}