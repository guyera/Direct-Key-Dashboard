using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringCountApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<string>
    {
        public StringCountApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}