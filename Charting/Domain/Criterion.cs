using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class Criterion {
        protected string Key {get;}

        public Criterion(string key) {
            Key = key;
        }

        public abstract bool SatisfiedBy(JObject jobject);
    }
}