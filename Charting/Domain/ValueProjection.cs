using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class ValueProjection<TProjection, TGroupedProjection> : Projection<TProjection>
        where TGroupedProjection : GroupedProjection<TProjection>
    {
        public TGroupedProjection UnderlyingProjection {get; set;}
        
        // For model binding
        public ValueProjection(){}
        public ValueProjection(TGroupedProjection projection) {
            UnderlyingProjection = projection;
        }
        public override TProjection Project(JObject jsonObject)
        {
            return UnderlyingProjection.Project(jsonObject).Value;
        }
    }
}