using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    public class PropertyValueCategorizer : Categorizer
    {
        public string PropertyName {get; set;}

        // For model binding
        public PropertyValueCategorizer(){}

        public PropertyValueCategorizer(string propertyName) {
            PropertyName = propertyName;
        }

        public override string Categorize(JObject obj)
        {
            var token = obj.GetValue(PropertyName);
            if (token == null) {
                throw new JsonArgumentException();
            }
            var value = token.ToString();
            if (value == null) {
                throw new JsonArgumentException();
            }

            return value;
        }
    }
}