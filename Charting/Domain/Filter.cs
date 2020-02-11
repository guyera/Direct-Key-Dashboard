using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    // A filter is based on the intersection
    // of a list of criteria which restrict the
    // range of possible floating point values
    public class Filter {
        public IEnumerable<Criterion> Criteria {get;}

        public Filter(IEnumerable<Criterion> criteria) {
            Criteria = criteria;
        }

        public IJEnumerable<JToken> FilterData(IJEnumerable<JToken> data) {
            return data.Where(d => d is JObject j && Criteria.All(c => c.SatisfiedBy(j))).AsJEnumerable();
        }
    }
}