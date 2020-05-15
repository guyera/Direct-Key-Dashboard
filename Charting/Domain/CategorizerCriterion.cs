using Newtonsoft.Json.Linq;

// Used to specify that a JSON object's
// category must be of a specific value
// in order to be considered relevant in
// the chart being projected (used in drilldown
// charts)
namespace DirectKeyDashboard.Charting.Domain {
    public class CategorizerCriterion : Criterion {
        public string DesiredCategory {get; set;}
        public PropertyValueCategorizer Categorizer {get; set;}

        // For model binding
        public CategorizerCriterion() : base(typeof(CategorizerCriterion).FullName) {}

        public CategorizerCriterion(string desiredCategory, PropertyValueCategorizer categorizer) : base(typeof(CategorizerCriterion).FullName) {
            DesiredCategory = desiredCategory;
            Categorizer = categorizer;
        }

        public override bool SatisfiedBy(JObject jobject)
        {
            try {
                var category = Categorizer.Categorize(jobject);
                return category == DesiredCategory;
            } catch(JsonArgumentException) {
                return false;
            }
        }
    }
}