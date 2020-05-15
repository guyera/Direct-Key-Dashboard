using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringCountApiBarChartViewComponent : ApiBarChartViewComponent<string>
    {
        public StringCountApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}