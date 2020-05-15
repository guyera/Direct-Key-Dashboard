using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting
{
    public class FloatProjectingApiLineChartViewComponent : ApiLineChartViewComponent<float>
    {
        public FloatProjectingApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}