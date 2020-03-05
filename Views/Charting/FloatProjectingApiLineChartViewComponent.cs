using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatProjectingApiLineChartViewComponent : ApiLineChartViewComponent<float, FloatCriterion>
    {
        public FloatProjectingApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}