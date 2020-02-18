using System.Collections.ObjectModel;

namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class Summary<TIn, TOut> {
        public abstract TOut Summarize(Collection<TIn> data);
    }
}