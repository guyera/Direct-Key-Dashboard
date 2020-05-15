using InformationLibraries;

// Used to help with ASP.NET's troubles with generic
// view components
namespace DirectKeyDashboard.Views.Charting {
    public class FloatProjectingApiBarChartViewComponent : ApiBarChartViewComponent<float>
    {
        public FloatProjectingApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess)
        {
        }
    }
}