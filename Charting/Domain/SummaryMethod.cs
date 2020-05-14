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
    }
    // An enumeration of possible summary methods.
    public enum SummaryMethodDescriptor {
        Count,
        Average,
        Median
    }
}