// Defer this script within a view component so that it is rendered after
// the rest of the page. This allows dependencies at the bottom of
// the layout to be rendered before this, despite this script being modularized
// within a view component which is rendered before the dependencies

// If properly deferred, this script should have access to all dependencies,
// including ChartJS, JQuery, and Bootstrap

// Receive script attribute for data passed in from razor page.
// We could use document.currentScript, but it doesn't have great
// browser support, so instead we use a global array as a
// queue in which we push() script IDs in the razor page and then shift()
// them out in the same order. Deferred scripts maintain their relative order,
// so a script will not accidentally take the information intended for another
// script.
var scriptId = window["groupedBarChartScriptIds"].shift();
var script = $('script[id="' + scriptId + '"]'); // Find script tag associated with this bar chart
var canvasID = script.attr('data-canvas-id'); // Get canvas ID attribute
var ctx = $('#' + canvasID); // Get chart context
var jsId = script.attr('data-js-id'); // Chart ID encoded as proper JS global variable name

// Simple details about the chart display (such as hyperparameters) via
// the attributes of this script
var barBorderWidth = parseInt(script.attr('data-bar-border-width'));
var numGroups = parseInt(script.attr('data-num-groups')); // Get number of bar chart datasets / groups for slicing

var datasets = [];
var dataCount = window['groupedBarChartData'][jsId].values.length / numGroups; // Number of data points in each set
for (var i = 0; i < numGroups; i++) { // For each dataset
    var values = [];
    for (var j = 0; j < dataCount; j++) { // For each data point
        // Construct a list of values for this dataset
        values.push(window['groupedBarChartData'][jsId].values[i * dataCount + j]);
    }
    // Construct the dataset object
    var backgroundColor = window['groupedBarChartData'][jsId].backgroundColors[i];
    var borderColor = window['groupedBarChartData'][jsId].borderColors[i];
    var label = window['groupedBarChartData'][jsId].groupLabels[i];
    datasets.push({
        label: label,
        data: values,
        backgroundColor: backgroundColor,
        borderColor: borderColor,
        borderWidth: barBorderWidth
    });
}

var handleClick = (function(jsId, ctx, numGroups) {
    return function handleClick(evt, activeElements) {
        if (activeElements != null && activeElements != undefined && activeElements[0] != null &&
            activeElements[0] != undefined) {
                var elem = this.getElementAtEvent(evt)[0];
                var dataCount = window['groupedBarChartData'][jsId].values.length / numGroups;
                console.log("Label: " + window['groupedBarChartData'][jsId].labels[elem.index]);
                var absIndex = elem.datasetIndex * dataCount + elem.index;
                console.log("Value: " + window['groupedBarChartData'][jsId].values[absIndex]);
                var tempChart = $(document.createElement('div'));
                ctx.parent().append(tempChart);

                console.log(window['groupedBarChartData'][jsId].drilldownUris[absIndex]);
                console.log(window['groupedBarChartData'][jsId].drilldownData[absIndex]);
                
                tempChart.load(window['groupedBarChartData'][jsId].drilldownUris[absIndex], window['groupedBarChartData'][jsId].drilldownData[absIndex]);
            } else {
                console.log("Did not click on bar.");
            }
        }
})(jsId, ctx, numGroups); // evaulate jsId IMMEDIATELY and permanently localize it to the returned function

var chart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: window['groupedBarChartData'][jsId].labels,
        datasets: datasets
    },
    options: {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        },
        onClick: handleClick
    }
});