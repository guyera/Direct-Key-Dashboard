using System;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    // Represent a simple projection wherein
    // a JSON object is projected to a single
    // value pulled from a token with
    // the specified key
    public class SimpleProjection<T> : Projection<T> {
        private readonly string TokenKey;
        
        public SimpleProjection(string tokenKey) {
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