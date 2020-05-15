using InformationLibraries;

// Used to help with ASP.NET's troubles with generic
// view components
namespace DirectKeyDashboard.Views.Charting
{
    public class FloatProjectingApiLineChartViewComponent : ApiLineChartViewComponent<float>
    {
        public FloatProjectingApiLineChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}