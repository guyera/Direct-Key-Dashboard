using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class Summary<TIn, TOut> {
        public abstract TOut Summarize(IEnumerable<TIn> data);
    }
}