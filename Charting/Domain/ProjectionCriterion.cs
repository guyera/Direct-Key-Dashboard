using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class ProjectionCriterion<TProjectionValue, TProjection> : Criterion
            where TProjection : Projection<TProjectionValue> {
        public TProjectionValue DesiredValue {get; set;}
        public TProjection Projection {get; set;}

        // For model binding
        public ProjectionCriterion(){}
        public ProjectionCriterion (TProjection projection, TProjectionValue desiredValue) {
            DesiredValue = desiredValue;
            Projection = projection;
        }

        public override bool SatisfiedBy(JObject jobject)
        {
            return Projection.Project(jobject).Equals(DesiredValue);
        }
    }
}