using DirectKeyDashboard.Charting.Domain;

namespace DirectKeyDashboard.Views.Charting {
    public class CreateCustomViewModel {
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

        public static SummaryMethodOption[] SummaryMethodOptions {get; set;} = {
            new SummaryMethodOption{
                DisplayName = "Average",
                RequiresProjection = true,
                SummaryMethod = SummaryMethod.Average
            },
            new SummaryMethodOption{
                DisplayName = "Median",
                RequiresProjection = true,
                SummaryMethod = SummaryMethod.Median
            },
            new SummaryMethodOption{
                DisplayName = "Count",
                RequiresProjection = false,
                SummaryMethod = SummaryMethod.Count
            }
        };

        public static ProjectionResultOption[] ProjectionResultOptions {get; set;} = {
            new ProjectionResultOption{
                DisplayName = "Number",
                ProjectionResult = ProjectionResult.Number
            }
        };

        public static TimeGranularityOption[] TimeGranularityOptions {get; set;} = {
            new TimeGranularityOption{
                DisplayName = "Days",
                TimeGranularity = TimeGranularity.Day
            },
            new TimeGranularityOption{
                DisplayName = "Months",
                TimeGranularity = TimeGranularity.Month
            },
            new TimeGranularityOption{
                DisplayName = "Years",
                TimeGranularity = TimeGranularity.Year
            }
        };

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
            public bool RequiresProjection {get; set;}
            public SummaryMethod SummaryMethod;
        }

        public class ProjectionResultOption {
            public string DisplayName {get; set;}
            public ProjectionResult ProjectionResult {get; set;}
        }

        public enum TimeGranularity {
            Day,
            Month,
            Year
        }

        public class TimeGranularityOption {
            public string DisplayName {get; set;}
            public TimeGranularity TimeGranularity {get; set;}
        }

        public class FloatCriterionRelationOption {
            public string DisplayName {get; set;}
            public FloatCriterion.Relation Relation {get; set;}
        }
    }
}