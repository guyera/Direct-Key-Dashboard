// A model for a full bar chart to be displayed via a
// ViewComponent

using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain {
    public class GroupedBarChart {
        // A list of bar groups / datasets used to
        // store the values across various sample points
        // related by a categorical qualfification
        public IList<BarGroup> BarGroups {get; set;}
        
        // Labels for each sample point. Note that these
        // are NOT the labels for the categories. Each
        // sample will have a data point from eah category.
        // The categories are color coded and labelled in the
        // legend.
        public IList<string> Labels {get; set;}
    }
}