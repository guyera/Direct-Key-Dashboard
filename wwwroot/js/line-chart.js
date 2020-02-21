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
var scriptId = window["lineChartScriptIds"].shift();
var script = $('script[id="' + scriptId + '"]'); // Find script tag associated with this line chart
var canvasID = script.attr('data-canvas-id'); // Get canvas ID attribute
var ctx = $('#' + canvasID); // Get chart context
var jsId = script.attr('data-js-id');

// Simple details about the chart display (such as hyperparameters) via
// the attributes of this script
var chartLabel = script.attr('data-chart-label');
var lineColor = script.attr('data-line-color');
var lineWidth = parseInt(script.attr('data-line-width'));
var pointColor = script.attr('data-point-color');
var pointBorderWidth = parseInt(script.attr('data-point-border-width'));

// Get the data itself reflectively using the global variable names and
// inject into the chart

var datasets = [];
for (var i = 0; i < window['lineChartData'][jsId].values.length; i++) {
    datasets.push({
        label: window['lineChartData'][jsId].labels[i],
        data: window['lineChartData'][jsId].values[i],
        lineTension: 0.4,
        fill: false,
        borderColor: window['lineChartData'][jsId].colors[i],
        borderWidth: lineWidth,
        pointBorderColor: window['lineChartData'][jsId].pointColors[i],
        pointBorderWidth: pointBorderWidth
    });
}

var chart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: window['lineChartData'][jsId].categoryLabels,
        datasets: datasets
    },
    options: {
        title: {
            display: true,
            text: 'Test line chart'
        },
        scales: {
            xAxes: [{
                type: 'category',
                display: true,
                scaleLabel: {
                    display: true,
                    labelString: 'Month'
                }
            }],
            yAxes: [{
                display: true,
                scaleLabel: {
                    display: true,
                    labelString: 'Value'
                }
            }]
        }
    }
});