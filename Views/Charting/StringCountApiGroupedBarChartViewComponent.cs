using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringCountApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<string, CountSummary<string>, FloatCriterion, SimpleCompositeGroupedProjection<string>>
    {
        public StringCountApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}