using InformationLibraries;

// ASP.NET does not allow InvokeAsync to have generic types, so this class
// extends ApiBarChartViewComponent<float> to force
// the parent's TProjection type to be a float.
namespace DirectKeyDashboard.Views.Charting
{
    public class StringProjectingApiBarChartViewComponent : ApiBarChartViewComponent<string>
    {
        public StringProjectingApiBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess) {}
    }
}