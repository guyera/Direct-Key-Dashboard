using Newtonsoft.Json.Linq;

// Used to enforce that objects must have
// a particular projection value (may no
// longer be necessary after refactoring.)
namespace DirectKeyDashboard.Charting.Domain {
    public class ProjectionCriterion<TProjectionValue, TProjection> : Criterion
            where TProjection : Projection<TProjectionValue> {
        public TProjectionValue DesiredValue {get; set;}
        public TProjection Projection {get; set;}

        // For model binding
        public ProjectionCriterion() : base(typeof(ProjectionCriterion<TProjectionValue, TProjection>).FullName){}
        public ProjectionCriterion (TProjection projection, TProjectionValue desiredValue) : base(typeof(ProjectionCriterion<TProjectionValue, TProjection>).FullName) {
            DesiredValue = desiredValue;
            Projection = projection;
        }

        public override bool SatisfiedBy(JObject jobject)
        {
            return Projection.Project(jobject).Equals(DesiredValue);
        }
    }
}