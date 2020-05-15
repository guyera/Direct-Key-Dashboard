namespace DirectKeyDashboard.Testing {
    public static class Util {
        public static void Assert(bool value) {
            if (!value) {
                throw new AssertionException();
            }
        }
    }
}