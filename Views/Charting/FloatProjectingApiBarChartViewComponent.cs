using InformationLibraries;

// ASP.NET does not allow InvokeAsync to have generic types, so this class
// extends ApiBarChartViewComponent<float> to force
// the parent's TProjection type to be a float.
namespace DirectKeyDashboard.Views.Charting
{
    public class FloatProjectingApiBarChartViewComponent : ApiBarChartViewComponent<float>
    {
        public FloatProjectingApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}