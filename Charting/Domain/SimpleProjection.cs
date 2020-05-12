using System;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    // Represent a simple projection wherein
    // a JSON object is projected to a single
    // value pulled from a token with
    // the specified key
    public class SimpleProjection<T> : Projection<T> {
        public string TokenKey {get; set;}
        
        // For model binding
        public SimpleProjection() : base(typeof(SimpleProjection<T>).FullName){}

        public SimpleProjection(string tokenKey) : base(typeof(SimpleProjection<T>).FullName) {
            TokenKey = tokenKey;
        }

        public override T Project(JObject jsonObject) {
            var token = jsonObject.GetValue(TokenKey);
            if (token == null || (token.Type != JTokenType.Float && token.Type != JTokenType.Integer)) {
                throw new JsonArgumentException();
            }
            var obj = token.ToObject(typeof(T));
            if (obj == null) {
                throw new JsonArgumentException();
            }

            return (T) obj;
        }
    }
}