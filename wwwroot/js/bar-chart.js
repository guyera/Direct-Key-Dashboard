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
var scriptId = window["barChartScriptIds"].shift();
var script = $('script[id="' + scriptId + '"]'); // Find script tag associated with this bar chart
var canvasID = script.attr('data-canvas-id'); // Get canvas ID attribute
var ctx = $('#' + canvasID); // Get chart context
var jsId = script.attr('data-js-id'); // Get the canvas ID encoded as a valid JS variable name

// Simple details about the chart display (such as hyperparameters) via
// the attributes of this script
var chartLabel = script.attr('data-chart-label');
var barBorderWidth = parseInt(script.attr('data-bar-border-width'));

// Use the CURRENT JS ID to generate
// a click handler. As this script is duplicated
// for other charts, the JS ID variable will get changed,
// and the handler will only ever use the most
// recent value of the JS ID variable. To force it to
// remember the current value,
// pass it into a function-generating function.
// Since the arguments get evaluated immediately,
// it will be remembered and localized to the handler
// permanently. That way, each chart's handler uses
// its own data.
var handleClick = (function(jsId, ctx) {
    return function handleClick(evt, activeElements) {
        if (activeElements != null && activeElements != undefined && activeElements[0] != null &&
            activeElements[0] != undefined) {
                console.log("Label: " + window['barChartData'][jsId].labels[activeElements[0].index]);
                console.log("Value: " + window['barChartData'][jsId].values[activeElements[0].index]);
                console.log("Loading line chart...")
                var tempChart = $(document.createElement('div'));
                ctx.parent().append(tempChart);
                tempChart.load("Charting/FloatProjectingApiLineChart");
            } else {
                console.log("Did not click on bar.");
            }
        }
})(jsId, ctx); // evaulate jsId IMMEDIATELY and permanently localize it to the returned function

// Get the data itself reflectively using the js ID and generate the chart

var chart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: window['barChartData'][jsId].labels,
        datasets: [{
            label: chartLabel,
            data: window['barChartData'][jsId].values,
            backgroundColor: window['barChartData'][jsId].backgroundColors,
            borderColor: window['barChartData'][jsId].borderColors,
            borderWidth: barBorderWidth
        }]
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

