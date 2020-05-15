using System.Collections.Generic;

// Used to project a JSON object into a composite
// group or series of composite groups. A composite
// group is a key value pair, the key specifying a
// dataset, and the value being a dictionary. Said
// dictionary includes keys, representing x-axis
// labels / sub-categories, and TProjection values,
// which are generally floating point numbers.
namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class CompositeGroupedProjection<TProjection> : GroupedProjection<IDictionary<string, TProjection>> {
        public CompositeGroupedProjection(string subtypeName) : base(subtypeName) {}
    }
}