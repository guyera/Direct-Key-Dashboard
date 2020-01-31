// Defer this script within a view component so that it is rendered after
// the rest of the page. This allows dependencies at the bottom of
// the layout to be rendered before this, despite this script being modularized
// within a view component which is rendered before the dependencies

// If properly deferred, this script should have access to all dependencies,
// including ChartJS, JQuery, and Bootstrap

// Receive script attribute containing canvas ID passed from view component
script = $('script[src="/js/pie-chart.js"]'); // Find script tag importing this file
var canvasID = script.attr('data-canvas-id'); // Get canvas ID attribute
var ctx = $('#' + canvasID); // Get chart context

// Simple details about the chart display (such as hyperparameters) via
// the attributes of this script
var chartLabel = script.attr('data-chart-label');
var sliceBorderWidth = parseInt(script.attr('data-slice-border-width'));

// Get the names of the global variables injected into the razor page
// representing the data
var labelsGlobal = script.attr('data-labels-global');
var countsGlobal = script.attr('data-counts-global');
var backgroundColorsGlobal = script.attr('data-background-colors-global');
var borderColorsGlobal = script.attr('data-border-colors-global');

// Get the data itself reflectively using the global variable names and
// inject into the chart

var chart = new Chart(ctx, {
    type: 'pie',
    data: {
        labels: window[labelsGlobal],
        datasets: [{
            data: window[countsGlobal],
            backgroundColor: window[backgroundColorsGlobal],
            borderColor: window[borderColorsGlobal],
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