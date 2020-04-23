using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain
{
    // Represents a grouped projection, wherein
    // each value corresponds to a named group.
    // This is useful in bar charts, where the
    // categories are named and certain values
    // belong to certain categories. Group is
    // represented as a string in the returned
    // KVP, and the value is a generic type.
    public abstract class GroupedProjection<T> : Projection<KeyValuePair<string, T>> {}
}