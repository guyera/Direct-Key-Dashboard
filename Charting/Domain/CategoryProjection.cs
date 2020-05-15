using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class CategoryProjection<TProjection> : Projection<string> {
        public GroupedProjection<TProjection> UnderlyingProjection {get; set;}

        // For model binding
        public CategoryProjection() : base(typeof(CategoryProjection<TProjection>).FullName){}
        public CategoryProjection(GroupedProjection<TProjection> projection) : base(typeof(CategoryProjection<TProjection>).FullName) {
            UnderlyingProjection = projection;
        }
        public override string Project(JObject jsonObject)
        {
            return UnderlyingProjection.Project(jsonObject).Key;
        }
    }
}