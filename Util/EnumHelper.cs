using System.ComponentModel;

namespace Util {
    static class EnumHelper<T> {
        public static string GetPropertyDisplayName(string propertyName) {
            var property = typeof(T).GetProperty(propertyName);
            var attributes = property.GetCustomAttributes(typeof(DisplayNameAttribute), true) as DisplayNameAttribute[];
            if (attributes.Length == 0) {
                return null; // This property doesn't have a display name
            }
            return attributes[0].DisplayName;
        }
    }
}