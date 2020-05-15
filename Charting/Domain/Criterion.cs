using Newtonsoft.Json.Linq;

// Used with a filter to filter out data points (JObjects) which
// do not satisfy this criterion
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