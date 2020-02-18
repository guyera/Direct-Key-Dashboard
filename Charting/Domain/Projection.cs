using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public abstract class Projection<T> {
        public abstract T Project(JObject jsonObject);
    }
}