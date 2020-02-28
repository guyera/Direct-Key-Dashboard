// Defer this script within a view component so that it is rendered after
// the rest of the page. This allows dependencies at the bottom of
// the layout to be rendered before this, despite this script being modularized
// within a view component which is rendered before the dependencies

// If properly deferred, this script should have access to all dependencies,
// including ChartJS, JQuery, and Bootstrap

var dynamicChartContainer = $('#dynamic-chart-container');
// Create a div to temporarily store the chart in
var chartElem = document.createElement('div');
$(chartElem).load('Charting/FloatProjectingApiLineChart');
// TODO Get the chart element child from the chartElem
// TODO Append the chart itself to the chart container
dynamicChartContainer.append(chartElem);