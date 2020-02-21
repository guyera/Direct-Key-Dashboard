using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringProjectingApiBarChartViewComponent : ApiBarChartViewComponent<string>
    {
        public StringProjectingApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}