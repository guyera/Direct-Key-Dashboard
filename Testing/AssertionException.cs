namespace DirectKeyDashboard.Testing {
    public class AssertionException : System.Exception
    {
        public AssertionException() { }
        public AssertionException(string message) : base(message) { }
        public AssertionException(string message, System.Exception inner) : base(message, inner) { }
        protected AssertionException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}