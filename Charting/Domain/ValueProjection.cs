using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class ValueProjection<TProjection> : Projection<TProjection>
    {
        public GroupedProjection<TProjection> UnderlyingProjection {get; set;}
        
        // For model binding
        public ValueProjection() : base(typeof(ValueProjection<TProjection>).FullName){}
        public ValueProjection(GroupedProjection<TProjection> projection) : base(typeof(ValueProjection<TProjection>).FullName) {
            UnderlyingProjection = projection;
        }
        public override TProjection Project(JObject jsonObject)
        {
            return UnderlyingProjection.Project(jsonObject).Value;
        }
    }
}