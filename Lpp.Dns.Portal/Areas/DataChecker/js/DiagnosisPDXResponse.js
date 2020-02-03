/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
var DataChecker;
(function (DataChecker) {
    var DiagnosesPDX;
    (function (DiagnosesPDX) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
            function ViewModel(parameters) {
                this.requestID = ko.observable(null);
                this.responseID = ko.observable(null);
                this.isLoaded = ko.observable(false);
                this.JQCharts = [];
                var self = this;
                if (parameters == null) {
                    return;
                }
                else if (parameters.ResponseID == null || parameters.ResponseID() == null) {
                    return;
                }
                else if (parameters.RequestID == null || parameters.RequestID() == null) {
                    return;
                }
                self.responseID(parameters.ResponseID());
                self.requestID(parameters.RequestID());
                $.when($.get('/DataChecker/DiagnosisPDX/ProcessMetricsByResponse?responseID=' + self.responseID().toString())).then(function (result) {
                    self.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Count by DataPartner", 1)]);
                    self.SelectedMetric = ko.observable(self.Metrics()[0]);
                    self.DataPartners = result.DataPartners;
                    self.EncounterTypes = result.EncounterTypes;
                    self.OverallMetrics = result.OverallMetrics;
                    self.PercentWithinDataPartner = result.PercentWithinDataPartner;
                    self.HasResults = (self.DataPartners.length > 0 && self.OverallMetrics.length > 0 && self.PercentWithinDataPartner.length > 0);
                    self.OverallTicks = $.Enumerable.From(self.EncounterTypes).Select(function (x) { return x.EncType_Display; }).ToArray();
                    self.OverallSeriesLabels = $.Enumerable.From(self.OverallMetrics).Select(function (x) { return { 'label': x.PDX_Display }; }).ToArray();
                    self.SelectedMetricChangedSubscription = self.SelectedMetric.subscribe(function (value) {
                        setTimeout(function () {
                            self.JQCharts.forEach(function (chart) {
                                if (chart._drawCount === 0) {
                                    DataChecker.Charting.replot(chart);
                                }
                            });
                        }, 100);
                    });
                    self.isLoaded(true);
                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }).fail(function (error) {
                    alert(error);
                    return;
                });
            }
            return ViewModel;
        }());
        DiagnosesPDX.ViewModel = ViewModel;
        ko.bindingHandlers.pdxPercentChart = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var source = valueAccessor();
                var overallSeriesData = [];
                source.encounterTypes.forEach(function (encounterType, i) {
                    var v = $.Enumerable.From(source.data).SelectMany(function (x) { return $.Enumerable.From(x.Encounters).Where(function (ec) { return ec.EncType == encounterType.EncType; }).Select(function (ec) { return Global.Helpers.ToPercent(ec.Count, ec.Total); }); }).ToArray();
                    v.forEach(function (x, k) {
                        if (typeof overallSeriesData[k] == 'undefined')
                            overallSeriesData[k] = [];
                        overallSeriesData[k].push(x);
                    });
                });
                if (overallSeriesData.length > 0) {
                    var overallPercentBarSrc = new DataChecker.ChartSource(overallSeriesData);
                    overallPercentBarSrc.isPercentage = true;
                    overallPercentBarSrc.yaxis_label = '%';
                    overallPercentBarSrc.xaxis_label = 'Encounter Type';
                    overallPercentBarSrc.title = source.title;
                    overallPercentBarSrc.pointLabelFormatString = '%.2f';
                    overallPercentBarSrc.multiSeriesData = { series_labels: source.overallSeriesLabels, ticks: source.overallTicks };
                    var chart = DataChecker.Charting.plotBarChart($(element), overallPercentBarSrc);
                    source.jqCharts.push(chart);
                }
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            }
        };
    })(DiagnosesPDX = DataChecker.DiagnosesPDX || (DataChecker.DiagnosesPDX = {}));
})(DataChecker || (DataChecker = {}));
