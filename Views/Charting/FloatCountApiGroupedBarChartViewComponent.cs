using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatCountApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<float, CountSummary<float>, FloatCriterion, SimpleCompositeGroupedProjection<float>>
    {
        public FloatCountApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}