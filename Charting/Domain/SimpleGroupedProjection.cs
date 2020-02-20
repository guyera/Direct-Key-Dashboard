using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain
{
    // Represents a simple grouped projection,
    // wherein the value is simply a float, and
    // it is pulled straight out of the object
    // by its token name.
    public class SimpleGroupedProjection<T> : GroupedProjection<T> {
        private readonly string CategoryTokenKey;
        private readonly string ValueTokenKey;
        
        public SimpleGroupedProjection(string categoryTokenKey, string valueTokenKey) {
            CategoryTokenKey = categoryTokenKey;
            ValueTokenKey = valueTokenKey;
        }

        public override KeyValuePair<string, T> Project(JObject jsonObject) {
            var token = jsonObject.GetValue(ValueTokenKey);
            if (token == null) {
                throw new JsonArgumentException();
            }

            var obj = token.ToObject(typeof(T));
            if (obj == null) {
                throw new JsonArgumentException();
            }

            var value = (T) obj;

            token = jsonObject.GetValue(CategoryTokenKey);
            string category = token.ToString();

            return new KeyValuePair<string, T>(category, value);
        }
    }
}