using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    // A filter is based on the intersection
    // of a list of criteria which restrict the
    // range of possible floating point values
    public class Filter<TCriterion>
        where TCriterion : Criterion
     {
        public IEnumerable<TCriterion> Criteria {get; set;}

        // For model bounding
        public Filter(){}

        public Filter(IEnumerable<TCriterion> criteria) {
            Criteria = criteria;
        }

        public IJEnumerable<JToken> FilterData(IJEnumerable<JToken> data) {
            if (Criteria == null)
                return data;
            return data.Where(d => d is JObject j && Criteria.All(c => c.SatisfiedBy(j))).AsJEnumerable();
        }
    }
}