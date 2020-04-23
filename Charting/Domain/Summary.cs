using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class Summary<TIn, TOut> {
        public abstract TOut Summarize(IEnumerable<TIn> data);
    }

    // An enumeration of possible summary methods.
    // Count and Average are currently supported.
    public enum SummaryMethod {
        Count,
        Average
    }
}