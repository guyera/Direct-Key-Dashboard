using DirectKeyDashboard.Charting.Domain;

// Page model for CreateCustomView.cshtml.
// Stores enumerables of enums, such as
// possible summary options, for dropdown
// boxes.
namespace DirectKeyDashboard.Views.Charting {
    public class CreateCustomViewModel {
        // Chart view types dropdown
        public static ViewTypeOption[] ViewOptions {get; set;} = {
            new ViewTypeOption{
                DisplayName = "Bar Chart",
                ViewType = ViewType.Bar
            },
            new ViewTypeOption{
                DisplayName = "Grouped Bar Chart",
                ViewType = ViewType.GroupedBar
            }
        };

        // Summary methods dropdown
        public static SummaryMethodOption[] SummaryMethodOptions {get; set;} = {
            new SummaryMethodOption{
                DisplayName = "Average",
                SummaryMethod = SummaryMethod.Average
            },
            new SummaryMethodOption{
                DisplayName = "Median",
                SummaryMethod = SummaryMethod.Median
            },
            new SummaryMethodOption{
                DisplayName = "Count",
                SummaryMethod = SummaryMethod.Count
            }
        };

        // Projection type options dropdown
        public static ProjectionResultOption[] ProjectionResultOptions {get; set;} = {
            new ProjectionResultOption{
                DisplayName = "Number",
                ProjectionResult = ProjectionResult.Number
            }
        };

        // Time granularities dropdown
        public static TimeGranularityOption[] TimeGranularityOptions {get; set;} = {
            new TimeGranularityOption{
                DisplayName = "Days",
                TimeGranularity = RelativeTimeGranularity.Day
            },
            new TimeGranularityOption{
                DisplayName = "Months",
                TimeGranularity = RelativeTimeGranularity.Month
            },
            new TimeGranularityOption{
                DisplayName = "Years",
                TimeGranularity = RelativeTimeGranularity.Year
            }
        };

        // Float criteria relations dropdown (e.g. = vs < vs >...)
        public static FloatCriterionRelationOption[] FloatCriterionRelationOptions {get; set;} = {
            new FloatCriterionRelationOption{
                DisplayName = "=",
                Relation = FloatCriterion.Relation.Equal
            },
            new FloatCriterionRelationOption{
                DisplayName = "\\u2260",
                Relation = FloatCriterion.Relation.NotEqual
            },
            new FloatCriterionRelationOption{
                DisplayName = "<",
                Relation = FloatCriterion.Relation.Less
            },
            new FloatCriterionRelationOption{
                DisplayName = "\\u2264",
                Relation = FloatCriterion.Relation.LessOrEqual
            },
            new FloatCriterionRelationOption{
                DisplayName = ">",
                Relation = FloatCriterion.Relation.Greater
            },
            new FloatCriterionRelationOption{
                DisplayName = "\\u2265",
                Relation = FloatCriterion.Relation.GreaterOrEqual
            },
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
            public SummaryMethod SummaryMethod;
        }

        public class ProjectionResultOption {
            public string DisplayName {get; set;}
            public ProjectionResult ProjectionResult {get; set;}
        }

        public class TimeGranularityOption {
            public string DisplayName {get; set;}
            public RelativeTimeGranularity TimeGranularity {get; set;}
        }

        public class FloatCriterionRelationOption {
            public string DisplayName {get; set;}
            public FloatCriterion.Relation Relation {get; set;}
        }
    }
}