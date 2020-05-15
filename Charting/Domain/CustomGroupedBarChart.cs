using System;
using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain
{
    // CustomBarChart represents a custom-built single-
    // grouped bar chart. 
    public class CustomGroupedBarChart {
        public Guid Id {get; set;}
        public string Title {get; set;}
        
        // Endpoint from which to retrieve the data
        // to be parsed. For now, assume the data
        // is in the form of an object with an array
        // called "data" containing the data points.
        // This is the form of KeyDeviceActivity's response.
        public string ApiEndpoint {get; set;}

        // ProjectionResult represents the type of data
        // being projected, such as strings (for count summaries)
        // or numbers (for numerical summaries like averages)
        public ProjectionResult? ProjectionResult {get; set;}

        // SummaryMethod represents the type of summary performed,
        // such as a count vs an average
        public SummaryMethodDescriptor SummaryMethodDescriptor {get; set;}

        // This represents the floating point / number criteria associated
        // with this chart (e.g. only project data with a DurationMs property greater than 0)
        public IList<CustomGroupedBarChartFloatCriterion> FloatCriteria {get; set;}
        
        // Determines whether this chart's time interval is relative to the present
        // (true) or absolute (false)
        public bool TimeRelative {get; set;}

        /* For intervals relative to the present, the following fields
           describe how to construct the time interval */
        public int? RelativeTimeValue {get; set;}
        public RelativeTimeGranularity? RelativeTimeGranularity {get; set;}

        /* For absolute time intervals, these mark the start and end points */
        public DateTime? IntervalStart {get; set;}
        public DateTime? IntervalEnd {get; set;}
        
        // SuperDatasetCategoryTokenKey denotes the name of the JSON property
        // which represents the data point's super dataset
        public string DatasetTokenKey {get; set;}

        // This is one option for sub-categorization. The other is by
        // providing multiple value token keys, wherein each JObject
        // contributes to every x-axis label through different properties.
        // Currently, CategoryTokenKeys are only supported for
        // non-projecting grouped bar charts.
        public string CategoryTokenKey {get; set;}

        // ValueTokenKeys denotes the names of the JSON properties
        // which represent the data point's values (y-axis). By
        // having multiple y-axis sources, each JObject contributes
        // to every x-axis label.
        // The type of the property should match what is
        // depicted by this chart model's ProjectionResult.
        // It should be a string (for count summaries only),
        // or it should be a number (for numerical summaries,
        // like average).
        public IList<CustomGroupedBarChartValueTokenKey> ValueTokenKeys {get; set;}

        // Pivoting a grouped bar chart refers to switching the x
        // axis with the sub-groupings. Normally, the X-axis
        // labels represent the sub-dataset category, and the
        // legend / color coding of the bars represents the
        // super dataset. Setting pivot to true will invert
        // these two by default.
        public bool Pivot {get; set;}

        public CustomGroupedBarChart() {}

        public class CustomGroupedBarChartValueTokenKey {
            public Guid Id {get; set;}
            public Guid CustomGroupedBarChartId {get; set;}
            public CustomGroupedBarChart CustomGroupedBarChart {get; set;}
            public string Key {get; set;}
        }

        // TODO extend this from the existing FloatCriterion class, if possible
        public class CustomGroupedBarChartFloatCriterion {
            public Guid Id {get; set;}
            public Guid CustomGroupedBarChartId {get; set;}
            public CustomGroupedBarChart CustomGroupedBarChart {get; set;}

            // Token key of this float JSON property
            public string Key {get; set;}
            // Value to compare to
            public float Value {get; set;}
            // Type of relation
            public FloatCriterion.Relation Relation {get; set;}
        }
    }
}