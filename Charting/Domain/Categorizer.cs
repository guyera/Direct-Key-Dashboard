using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public abstract class Categorizer {
        public abstract string Categorize(JObject obj);
    }
}