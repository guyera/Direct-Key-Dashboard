using System;
using System.Collections.Generic;

namespace DirectKeyDashboard.Charting.Domain
{
    // CustomBarChart represents a custom-built single-
    // grouped bar chart. 
    public class CustomBarChart {
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

        // SummaryMethodDescriptor represents the type of summary performed,
        // such as a count vs an average
        public SummaryMethodDescriptor SummaryMethodDescriptor {get; set;}

        public IList<CustomBarChartFloatCriterion> FloatCriteria {get; set;}
        
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
        
        // CategoryTokenKey denotes the name of the JSON property
        // which represents the data point's category (x-axis)
        public string CategoryTokenKey {get; set;}

        // ValueTokenKey denotes the name of the JSON property
        // which represents the data point's value (y-axis).
        // The type of the property should match what is
        // depicted by this chart model's ProjectionResult.
        // It should be a string (for count summaries only),
        // or it should be a number (for numerical summaries,
        // like average).
        public string ValueTokenKey {get; set;}

        public CustomBarChart() {}

        public class CustomBarChartFloatCriterion {
            public Guid Id {get; set;}
            public Guid CustomBarChartId {get; set;}
            public CustomBarChart CustomBarChart {get; set;}

            // Token key of this float JSON property
            public string Key {get; set;}
            // Value to compare to
            public float Value {get; set;}
            // Type of relation
            public FloatCriterion.Relation Relation {get; set;}
        }
    }
    
    public enum CriterionType {
        Float
    }
}