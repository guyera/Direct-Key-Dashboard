using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatProjectingApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<float, AverageSummary, FloatCriterion, SimpleCompositeGroupedProjection<float>>
    {
        public FloatProjectingApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}