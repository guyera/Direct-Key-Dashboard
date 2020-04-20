using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class StringCountApiLineChartViewComponent : ApiLineChartViewComponent<string, ProjectionCriterion<string, SimpleProjection<string>>, ProjectionCriterion<string, CategoryProjection<string, SimpleGroupedProjection<string>>>>
    {
        public StringCountApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}