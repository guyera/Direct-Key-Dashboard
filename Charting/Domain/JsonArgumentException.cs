using System;

namespace DirectKeyDashboard.Charting.Domain {
    public class JsonArgumentException : ArgumentException {
        private const string JsonArgumentMessage = "Json object is missing required properties";
        public JsonArgumentException() : base(JsonArgumentMessage) {}
    }
}