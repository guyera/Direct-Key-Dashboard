using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringCountApiBarChartViewComponent : ApiBarChartViewComponent<string, CountSummary<string>, SimpleGroupedProjection<string>, ProjectionCriterion<string, SimpleProjection<string>>>
    {
        public StringCountApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}