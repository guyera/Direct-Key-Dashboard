using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public abstract class Projection<T> {
        public abstract T Project(JObject jsonObject);
    }

    // An enumeration of possible projection result types.
    // Currently, only numbers and strings are supported.
    public enum ProjectionResult {
        String,
        Number
    }
}