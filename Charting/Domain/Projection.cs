using Newtonsoft.Json.Linq;

// Projects a JObject into a more specific, funneled
// type (T). The most common type of projection
// is a simple selection, e.g. select the floating
// point property titled "OperationCommDurationMs" out
// of this JObject (that would be a SimpleProjection<float>).
namespace DirectKeyDashboard.Charting.Domain {
    public abstract class Projection<T> {
        public string SubtypeName {get; set;}
        public Projection(string subtypeName) {
            SubtypeName = subtypeName;
        }
        
        public abstract T Project(JObject jsonObject);
    }

    // For referencing the nameof(Projection.SubtypeName), since
    // reflective member name references do not work through
    // unbounded generic types, like Projection<>
    public abstract class Projection : Projection<int>
    {
        public Projection(string subtypeName) : base(subtypeName) {}
    }

    // An enumeration of possible projection result types.
    // Currently, only numbers are supported.
    public enum ProjectionResult {
        Number,
        String
    }
}