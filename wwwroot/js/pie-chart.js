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
var scriptId = window["pieChartScriptIds"].shift();
var script = $('script[id="' + scriptId + '"]'); // Find script tag associated with this bar chart
var canvasID = script.attr('data-canvas-id'); // Get canvas ID attribute
var ctx = $('#' + canvasID); // Get chart context
var jsId = script.attr('data-js-id'); // Chart ID encoded as proper JS global variable name

// Simple details about the chart display (such as hyperparameters) via
// the attributes of this script
var chartLabel = script.attr('data-chart-label');
var sliceBorderWidth = parseInt(script.attr('data-slice-border-width'));

var chart = new Chart(ctx, {
    type: 'pie',
    data: {
        labels: window['pieChartData'][jsId].labels,
        datasets: [{
            data: window['pieChartData'][jsId].counts,
            backgroundColor: window['pieChartData'][jsId].backgroundColors,
            borderColor: window['pieChartData'][jsId].borderColors,
            borderWidth: sliceBorderWidth
        }]
    },
    options: {
        title: {
            display: true,
            text: chartLabel
        }
    }
});