using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

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

    // An enumeration of possible summary methods.
    // Count and Average are currently supported.
    public enum SummaryMethod {
        Count,
        Average,
        Median
    }
}