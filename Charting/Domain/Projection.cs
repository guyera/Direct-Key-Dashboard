using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public abstract class Projection {
        public abstract float Project(JObject jsonObject);
    }
}