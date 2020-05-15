using Newtonsoft.Json.Linq;

// Derives the value of a JSON object given
// a composite grouped projection and a particular sub-category
namespace DirectKeyDashboard.Charting.Domain {
    public class CompositeValueProjection<TProjectionValue> : Projection<TProjectionValue>
    {
        public CompositeGroupedProjection<TProjectionValue> UnderlyingProjection {get; set;}
        public string SubKey {get; set;}
        
        // For model binding
        public CompositeValueProjection() : base(typeof(CompositeValueProjection<TProjectionValue>).FullName){}
        public CompositeValueProjection(CompositeGroupedProjection<TProjectionValue> projection, string subKey) : base(typeof(CompositeValueProjection<TProjectionValue>).FullName) {
            UnderlyingProjection = projection;
            SubKey = subKey;
        }

        public override TProjectionValue Project(JObject jsonObject)
        {
            return UnderlyingProjection.Project(jsonObject).Value[SubKey];
        }
    }
}