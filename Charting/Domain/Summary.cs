using System.Collections.Generic;

// Summarizes / aggregates a series of datapoints into
// a displayable value, such as an average,
// median, or count
namespace DirectKeyDashboard.Charting.Domain
{
    public abstract class Summary<TIn, TOut> {
        // For polymorphic model binding
        public string SubtypeName {get; set;}

        public Summary(string subtypeName) {
            SubtypeName = subtypeName;
        }
        
        public abstract TOut Summarize(IEnumerable<TIn> data);
    }

    // For referencing the name of the SubtypeName property via reflection
    // without having to work around unbound generic types
    public abstract class Summary : Summary<float, float>
    {
        public Summary (string subtypeName) : base(subtypeName) {}
    }
}