using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class CompositeValueProjection<TProjectionValue, TCompositeProjection> : Projection<TProjectionValue>
        where TCompositeProjection : CompositeGroupedProjection<TProjectionValue>
    {
        public TCompositeProjection UnderlyingProjection {get; set;}
        public string SubKey {get; set;}
        
        // For model binding
        public CompositeValueProjection(){}
        public CompositeValueProjection(TCompositeProjection projection, string subKey) {
            UnderlyingProjection = projection;
            SubKey = subKey;
        }

        public override TProjectionValue Project(JObject jsonObject)
        {
            return UnderlyingProjection.Project(jsonObject).Value[SubKey];
        }
    }
}