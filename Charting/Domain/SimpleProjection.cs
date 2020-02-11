using System;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    // Represent a simple projection wherein
    // a JSON object is projected to a single
    // value pulled from a token with
    // the specified key
    public class SimpleProjection : Projection {
        private readonly string TokenKey;
        
        public SimpleProjection(string tokenKey) {
            TokenKey = tokenKey;
        }

        public override float Project(JObject jsonObject) {
            var token = jsonObject.GetValue(TokenKey);
            if (token == null || (token.Type != JTokenType.Float && token.Type != JTokenType.Integer)) {
                throw new JsonArgumentException();
            }
            float value;
            if (token.Type == JTokenType.Float) {
                float? valueNullable = (float?) token.ToObject(typeof(float));
                if (valueNullable == null) {
                    throw new JsonArgumentException();
                }
                value = valueNullable.Value;
            } else {
                int? valueNullable = (int?) token.ToObject(typeof(int));
                if (valueNullable == null) {
                    throw new JsonArgumentException();
                }
                value = valueNullable.Value;
            }
            return value;
        }
    }
}