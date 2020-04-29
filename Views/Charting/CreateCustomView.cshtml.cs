using DirectKeyDashboard.Charting.Domain;

namespace DirectKeyDashboard.Views.Charting {
    public class CreateCustomViewModel {
        public static ViewTypeOption[] ViewTypes {get; set;} = {
            new ViewTypeOption{
                DisplayName = "Bar Chart",
                ViewType = ViewType.Bar
            },
            new ViewTypeOption{
                DisplayName = "Grouped Bar Chart",
                ViewType = ViewType.GroupedBar
            }
        };

        public static SummaryMethodOption[] SummaryMethods {get; set;} = {
            new SummaryMethodOption{
                DisplayName = "Average",
                RequiresProjection = true,
                SummaryMethod = SummaryMethod.Average
            },
            new SummaryMethodOption{
                DisplayName = "Count",
                RequiresProjection = false,
                SummaryMethod = SummaryMethod.Count
            }
        };

        public static ProjectionResultOption[] ProjectionResultTypes {get; set;} = {
            new ProjectionResultOption{
                DisplayName = "Number",
                ProjectionResult = ProjectionResult.Number
            }
        };

        public class ViewTypeOption {
            public string DisplayName {get; set;}
            public ViewType ViewType {get; set;}

        }
        public enum ViewType {
            Bar,
            GroupedBar
        }

        public class SummaryMethodOption {
            public string DisplayName {get; set;}
            public bool RequiresProjection {get; set;}
            public SummaryMethod SummaryMethod;
        }

        public class ProjectionResultOption {
            public string DisplayName {get; set;}
            public ProjectionResult ProjectionResult {get; set;}
        }
    }
}