// A model for a full grouped bar chart to be displayed via a
// ViewComponent

using System.Collections.Generic;
using System.Linq;

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

        public GroupedBarChart Pivot() {
            var tempLabels = BarGroups.Select(bg => bg.Label).ToList();
            var tempBarGroups = new List<BarGroup>();
            var backgroundHue = 0;
            var borderHue = 0;
            var hueInc = Labels.Count() == 0 ? 0 : 360 / Labels.Count();
            for (var i = 0; i < Labels.Count(); i++) {
                var barGroup = new BarGroup() {
                    Label = Labels[i],
                    Values = BarGroups.Select(bg => bg.Values.Count() > i ? bg.Values[i] : 0).ToList(),
                    BackgroundColor = $"hsla({backgroundHue}, {BarGroup.BackgroundSaturation}, {BarGroup.BackgroundLightness}, {BarGroup.BackgroundAlpha})",
                    BorderColor = $"hsla({borderHue}, {BarGroup.BorderSaturation}, {BarGroup.BorderLightness}, {BarGroup.BorderAlpha})",
                };
                backgroundHue += hueInc;
                borderHue += hueInc;
                tempBarGroups.Add(barGroup);
            }

            return new GroupedBarChart() {
                BarGroups = tempBarGroups,
                Labels = tempLabels
            };
        }
    }
}