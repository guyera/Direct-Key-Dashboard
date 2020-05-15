using System.Collections.Generic;
using Newtonsoft.Json.Linq;

// The initial implementation of grouped bar chart
// projections. It requires a property / token key
// to determine what dataset the object should
// be classified as, as well as a series of
// tokens to specify each category (x-axis label)
// which the object should contribute to, and how (through
// simple selection)
namespace DirectKeyDashboard.Charting.Domain {
    public class SimpleCompositeGroupedProjection<TProjection> : CompositeGroupedProjection<TProjection>
    {
        public string DatasetKeyToken {get; set;}
        public IList<string> CategoryKeyTokens {get; set;}

        // For model binding
        public SimpleCompositeGroupedProjection() : base(typeof(SimpleCompositeGroupedProjection<TProjection>).FullName){}
        public SimpleCompositeGroupedProjection(string datasetKeyToken, IList<string> categoryKeyTokens)
                : base(typeof(SimpleCompositeGroupedProjection<TProjection>).FullName) {
            DatasetKeyToken = datasetKeyToken;
            CategoryKeyTokens = categoryKeyTokens;
        }

        public override KeyValuePair<string, IDictionary<string, TProjection>> Project(JObject jsonObject)
        {
            // Get token which decides the dataset of this json object
            var token = jsonObject.GetValue(DatasetKeyToken);
            if (token == null) {
                throw new JsonArgumentException();
            }

            // Get the value of the token, which will be the name of the dataset
            var dataset = token.ToString();

            // Get each category value by their keys and construct
            // a dictionary relating the key tokens to the values
            IDictionary<string, TProjection> valueDictionary = new Dictionary<string, TProjection>();
            foreach (var categoryKeyToken in CategoryKeyTokens ?? new List<string>()) {
                // Get token associated with this category key property
                // If it doesn't have the property / the property is
                // null, throw an exception. Such objects should
                // be filtered out by a provided filter model, or
                // this error should be handled elsewhere.
                token = jsonObject.GetValue(categoryKeyToken);
                if (token == null) {
                    throw new JsonArgumentException();
                }

                var value = (TProjection) token.ToObject(typeof(TProjection));

                valueDictionary.Add(categoryKeyToken, value);
            }

            // Return the key value pair, with the key being the dataset, and the
            // value being the dictionary relating categories to sub-values.
            return new KeyValuePair<string, IDictionary<string, TProjection>>(dataset, valueDictionary);
        }
    }
}