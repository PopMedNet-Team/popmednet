var DataChecker;
(function (DataChecker) {
    (function (Charting) {
        function init() {
            $.jqplot.config.enablePlugins = true;

            var currentColors = $.jqplot.config.defaultColors;
            currentColors.push('#40FF00');
            currentColors.push('#01DFD7');
            currentColors.push('#0000FF');
            currentColors.push('#F2F5A9');
            currentColors.push('#FFFF00');
            currentColors.push('#61210B');
            currentColors.push('#BCA9F5');
            currentColors.push('#FF0000');
            currentColors.push('#00FF00');
            currentColors.push('#DEC280');
            currentColors.push('#FF4AF1');
        }
        init();

        function plotBarChart(element, source) {

            var options = {
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    pointLabels: {
                        show: true,
                        formatString: source.pointLabelFormatString,
                        ypadding: 0,
                        hideZeros: false
                    },
                    highlighter: {
                        show: false,
                        showMarker: false,
                        showTooltip: false,
                    },
                    cursor: {
                        show: false
                    }
                },
                axes: {
                    xaxis: {
                        renderer: $.jqplot.CategoryAxisRenderer,
                        label: source.xaxis_label,
                        tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                        tickOptions: {
                            angle: -30
                        }
                    },
                    yaxis: {
                        label: source.yaxis_label,
                        min: 0,
                    }
                },
                highlighter: {
                    show: false
                },
                cursor: {
                    show: false
                }
            };

            if (source.isPercentage) {
                options.axes.yaxis.min = 0;
                options.axes.yaxis.max = 110;
                options.axes.yaxis.numberTicks = 12;
                options.axes.yaxis.ticks = [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110];
                options.axes.yaxis.tickOptions = { formatString: '%.0f%' };
                options.seriesDefaults.pointLabels.formatString = '%.2f%';
            }

            if (source.hasTitle()) {
                options.title = source.title;
            }

            if (source.rotateXAxisLabels()) {
                options.axes.xaxis.tickRenderer = $.jqplot.CanvasAxisTickRenderer;
                options.axes.xaxis.tickOptions = { angle: 45 };
            }

            if (source.multiSeriesData != null) {
                options.legend = { show: true, placement: 'outsideGrid' };
                options.series = source.multiSeriesData.series_labels;
                options.axes.xaxis.ticks = source.multiSeriesData.ticks;
                //options.seriesDefaults.highlighter.show = false;
                //options.seriesDefaults.highlighter.showMarker = false;
                //options.seriesDefaults.showMarker = false;
                //options.seriesDefaults.pointLabels.show = true;

                return $.jqplot(element[0].id, source.data, options);
            }

            return $.jqplot(element[0].id, [source.data], options);
        }

        function plotPieChart(element, source) {
            var options = {
                seriesDefaults: {
                    shadow: false,
                    renderer: $.jqplot.PieRenderer,
                    trendline: { show: false },
                    rendererOptions: {
                        showDataLabels: source.showPiePointLabels,
                        startAngle: -90
                    },
                    highlighter: {
                        show: true,
                        showMarker: false,
                        showTooltip: true,
                        fadeTooltip: true,
                        tooltipAxes: 'x',
                        useAxesFormatters: false,
                        tooltipContentEditor: function (str, seriesIndex, pointIndex, jqPlot) {
                            return source.data[pointIndex][0];
                        }
                    }
                },
                legend: {
                    show: true,
                    rendererOptions: { numberColumns: Math.min(3, source.data.length / 10) }
                }
            };

            if (source.hasTitle()) {
                options.title = source.title;
            }

            return $.jqplot(element[0].id, [source.data], options);
        }

        function replot(element) {
            element.replot();
        }

        Charting.plotBarChart = plotBarChart;
        Charting.plotPieChart = plotPieChart;
        Charting.replot = replot;
    })(DataChecker.Charting || (DataChecker.Charting = {}));
    var Charting = DataChecker.Charting;
})(DataChecker || (DataChecker = {}));