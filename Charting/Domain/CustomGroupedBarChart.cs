using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DirectKeyDashboard.Charting.Domain {
    // CustomBarChart represents a custom-built single-
    // grouped bar chart. 
    public class CustomGroupedBarChart {
        public Guid Id {get; set;}
        public string Name {get; set;}
        
        // Endpoint from which to retrieve the data
        // to be parsed. For now, assume the data
        // is in the form of an object with an array
        // called "data" containing the data points.
        // This is the form of KeyDeviceActivity's response.
        public string ApiEndpoint {get; set;}

        // ProjectionResult represents the type of data
        // being projected, such as strings (for count summaries)
        // or numbers (for numerical summaries like averages)
        public ProjectionResult ProjectionResult {get; set;}

        // SummaryMethod represents the type of summary performed,
        // such as a count vs an average
        public SummaryMethod SummaryMethod {get; set;}

        // CriterionType represents the type of criterion used
        // to filter out unwanted datapoints. ProjectionCriteria
        // are simple criteria which just check to see if
        // some projected value is equal to (.equals()) a
        // predetermined value. FloatCriteria allow for comparing
        // numerical values (<, >, <=, >=, ==, !=). ProjectionCriteria
        // are mostly used for drilldown charts (e.g. show me all
        // of the data associated with the object whose operation code
        // has a value of "01"). FloatCriteria are used, for example,
        // to distinguish between quick-connect operations (user intent duration
        // ms != 0) and non-quick-connect operations (user intent duration
        // ms == 0).
        public CriterionType CriterionType {get; set;}

        // This gets a little messy. Since there's no easy way to represent
        // generic types in relational databases, each custom bar chart
        // can have many FloatCriteria and ProjectionCriterionModels
        // associated with it. However, there should only be one or the other;
        // there should be be both types of criteria associated with the
        // custom bar chart. The type of criteria which does exist (if any)
        // should match the CriterionType field.
        public IList<CustomBarChartFloatCriterion> FloatCriteria {get; set;}
        

        // Start and end times for the window of data to be projected
        // and summarized
        public DateTime IntervalStart {get; set;}
        public DateTime IntervalEnd {get; set;}
        
        // SuperDatasetCategoryTokenKey denotes the name of the JSON property
        // which represents the data point's super dataset
        public string SuperDatasetCategoryTokenKey {get; set;}

        // ValueTokenKeys denotes the names of the JSON properties
        // which represent the data point's values (y-axis).
        // The type of the property should match what is
        // depicted by this chart model's ProjectionResult.
        // It should be a string (for count summaries only),
        // or it should be a number (for numerical summaries,
        // like average).
        public List<string> ValueTokenKeys {get; set;}

        public bool Pivot {get; set;}

        public CustomGroupedBarChart() {}

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
}