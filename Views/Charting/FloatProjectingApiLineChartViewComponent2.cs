using System.Collections.Generic;
using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

namespace DirectKeyDashboard.Views.Charting {
    public class FloatProjectingApiLineChartViewComponent : ApiLineChartViewComponent<float, FloatCriterion, ProjectionCriterion<string, CategoryProjection<IDictionary<string, float>, SimpleCompositeGroupedProjection<float>>>>
    {
        public FloatProjectingApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}