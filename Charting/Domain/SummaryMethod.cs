// Enum representation of a summary for storage / reference
// in a database entity (such as a CustomBarChart). Also
// determines whether or not the summary requires a projection
// into a number beforehand
namespace DirectKeyDashboard.Charting.Domain {
    public class SummaryMethod {
        public SummaryMethodDescriptor Descriptor {get; set;}
        public bool RequiresProjection {get; set;}

        public static readonly SummaryMethod Count = new SummaryMethod(){
            Descriptor = SummaryMethodDescriptor.Count,
            RequiresProjection = false
        };

        public static readonly SummaryMethod Average = new SummaryMethod(){
            Descriptor = SummaryMethodDescriptor.Average,
            RequiresProjection = true
        };

        public static readonly SummaryMethod Median = new SummaryMethod(){
            Descriptor = SummaryMethodDescriptor.Median,
            RequiresProjection = true
        };

        public static SummaryMethod FromDescriptor(SummaryMethodDescriptor descriptor) {
            switch(descriptor) {
                case SummaryMethodDescriptor.Count:
                    return Count;
                case SummaryMethodDescriptor.Average:
                    return Average;
                case SummaryMethodDescriptor.Median:
                default:
                    return Median;
            }
        }
    }
    // An enumeration of possible summary methods.
    public enum SummaryMethodDescriptor {
        Count,
        Average,
        Median
    }
}