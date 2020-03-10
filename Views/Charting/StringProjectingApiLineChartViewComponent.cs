using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringProjectingApiLineChartViewComponent : ApiLineChartViewComponent<string, ProjectionCriterion<string, CategoryProjection<string, SimpleGroupedProjection<string>>>, ProjectionCriterion<string, CategoryProjection<string, SimpleGroupedProjection<string>>>>
    {
        public StringProjectingApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}