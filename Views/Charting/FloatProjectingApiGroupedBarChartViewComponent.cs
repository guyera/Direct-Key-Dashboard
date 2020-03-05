using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatProjectingApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<float, FloatCriterion>
    {
        public FloatProjectingApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}