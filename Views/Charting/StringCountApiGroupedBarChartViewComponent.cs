using DirectKeyDashboard.Charting.Domain;
using InformationLibraries;

// Used to help ASP.NET's trouble with generic
// view components. Can probably be removed
// after refactoring (replace with NonProjecting
// rather than string counting)
namespace DirectKeyDashboard.Views.Charting {
    public class StringCountApiGroupedBarChartViewComponent : ApiGroupedBarChartViewComponent<string>
    {
        public StringCountApiGroupedBarChartViewComponent(DKApiAccess apiAccess) : base(apiAccess){}
    }
}