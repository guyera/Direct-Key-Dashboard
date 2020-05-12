using Newtonsoft.Json.Linq;

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