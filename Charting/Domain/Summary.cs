using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain {
    public abstract class Summary<T> {
        public abstract T Summarize(IEnumerable<T> data);
    }
}