using InformationLibraries;

// Used to help with ASP.NET's troubles with generic
// view components
namespace DirectKeyDashboard.Views.Charting {
    public class FloatProjectingApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<float>
    {
        public FloatProjectingApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess)
        {
        }
    }
}