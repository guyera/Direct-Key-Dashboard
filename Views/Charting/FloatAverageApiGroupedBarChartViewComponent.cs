using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatAverageApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<float, AverageSummary, FloatCriterion, SimpleCompositeGroupedProjection<float>>
    {
        public FloatAverageApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}