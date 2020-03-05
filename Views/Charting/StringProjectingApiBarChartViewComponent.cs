using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringProjectingApiBarChartViewComponent : ApiBarChartViewComponent<string, CountSummary<string>, SimpleGroupedProjection<string>, ProjectionCriterion<string, CategoryProjection<string, SimpleGroupedProjection<string>>>>
    {
        public StringProjectingApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}