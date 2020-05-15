using Newtonsoft.Json.Linq;

// Used to categorize a JSON object to a particular
// category on the X axis (or a particular dataset)
// without projecting a value out of it
namespace DirectKeyDashboard.Charting.Domain {
    public abstract class Categorizer {
        public abstract string Categorize(JObject obj);
    }
}