using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatProjectingApiBarChartViewComponent : ApiBarChartViewComponent<float>
    {
        public FloatProjectingApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess)
        {
        }
    }
}