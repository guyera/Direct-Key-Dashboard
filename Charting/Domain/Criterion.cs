using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class Criterion {
        public string SubtypeName {get; set;}
        public Criterion(string subtypeName) {
            SubtypeName = subtypeName;
        }
        public abstract bool SatisfiedBy(JObject jobject);
    }
}