using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class CompositeGroupedProjection<TProjection> : GroupedProjection<IDictionary<string, TProjection>> {
        public CompositeGroupedProjection(string subtypeName) : base(subtypeName) {}
    }
}