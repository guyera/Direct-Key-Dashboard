using Newtonsoft.Json.Linq;

// Used to categorize a JObject based on the
// value of a particular JSON property, such
// as the value of the OperationDescription property.
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