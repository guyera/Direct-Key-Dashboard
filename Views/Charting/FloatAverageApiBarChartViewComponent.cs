using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatAverageApiBarChartViewComponent : ApiBarChartViewComponent<float, AverageSummary, SimpleGroupedProjection<float>, FloatCriterion>
    {
        public FloatAverageApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}