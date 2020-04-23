using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatCountApiBarChartViewComponent : ApiBarChartViewComponent<float, CountSummary<float>, SimpleGroupedProjection<float>, FloatCriterion>
    {
        public FloatCountApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}