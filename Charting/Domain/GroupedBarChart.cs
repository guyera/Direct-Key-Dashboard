// A model for a full grouped bar chart to be displayed via a
// ViewComponent

using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain
{
    public class GroupedBarChart {
        // A list of bar groups / datasets used to
        // store the values across various sample points
        // related by a categorical qualfification
        public IList<BarGroup> BarGroups {get; set;}
        
        // Labels for each sample point. Note that these
        // are NOT the labels for the datasets. Each
        // sample will have a data point from eah dataset.
        // The datasets are color coded and labelled in the
        // legend.
        public IList<string> Labels {get; set;}
    }
}