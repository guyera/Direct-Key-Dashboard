using System;

// Custom exception intended to be thrown when
// a failure to parse a JObject, JToken, or other
// flexible json type occurs.
namespace DirectKeyDashboard.Charting.Domain {
    public class JsonArgumentException : ArgumentException {
        private const string JsonArgumentMessage = "Json object is missing required properties";
        public JsonArgumentException() : base(JsonArgumentMessage) {}
    }
}