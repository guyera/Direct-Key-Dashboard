using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class CategoryProjection<TProjection, TGroupedProjection> : Projection<string>
            where TGroupedProjection : GroupedProjection<TProjection> {
        public TGroupedProjection UnderlyingProjection {get; set;}

        // For model binding
        public CategoryProjection() : base(typeof(CategoryProjection<TProjection, TGroupedProjection>).FullName){}
        public CategoryProjection(TGroupedProjection projection) : base(typeof(CategoryProjection<TProjection, TGroupedProjection>).FullName) {
            UnderlyingProjection = projection;
        }
        public override string Project(JObject jsonObject)
        {
            return UnderlyingProjection.Project(jsonObject).Key;
        }
    }
}