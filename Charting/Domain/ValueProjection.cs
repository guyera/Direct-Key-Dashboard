using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class ValueProjection<TProjection, TGroupedProjection> : Projection<TProjection>
        where TGroupedProjection : GroupedProjection<TProjection>
    {
        public TGroupedProjection UnderlyingProjection {get; set;}
        
        // For model binding
        public ValueProjection() : base(typeof(ValueProjection<TProjection, TGroupedProjection>).FullName){}
        public ValueProjection(TGroupedProjection projection) : base(typeof(ValueProjection<TProjection, TGroupedProjection>).FullName) {
            UnderlyingProjection = projection;
        }
        public override TProjection Project(JObject jsonObject)
        {
            return UnderlyingProjection.Project(jsonObject).Value;
        }
    }
}