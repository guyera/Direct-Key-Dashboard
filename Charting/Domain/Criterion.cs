using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class Criterion {
        public abstract bool SatisfiedBy(JObject jobject);
    }
}