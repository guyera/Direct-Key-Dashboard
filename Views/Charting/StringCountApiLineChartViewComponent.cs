using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringCountApiLineChartViewComponent : ApiLineChartViewComponent<string, Criterion, Criterion>
    {
        public StringCountApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}