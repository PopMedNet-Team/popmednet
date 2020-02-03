/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
var DataChecker;
(function (DataChecker) {
    var DiagnosesPDX;
    (function (DiagnosesPDX) {
        var vm;
        var _bindingControl;
        var ViewModel = (function () {
            function ViewModel(model, result) {
                var _this = this;
                this._model = model;
                this._documentID = this._model.ResponseDocumentIDs[0];
                this.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Percent within Data Partner", 1)]);
                this.SelectedMetric = ko.observable(this.Metrics()[0]);
                this.DataPartners = result.DataPartners;
                this.EncounterTypes = result.EncounterTypes;
                this.OverallMetrics = result.OverallMetrics;
                this.PercentWithinDataPartner = result.PercentWithinDataPartner;
                this.HasResults = (this.DataPartners.length > 0 && this.OverallMetrics.length > 0 && this.PercentWithinDataPartner.length > 0);
                this.OverallTicks = $.Enumerable.From(this.EncounterTypes).Select(function (x) { return x.EncType_Display; }).ToArray();
                this.OverallSeriesLabels = $.Enumerable.From(this.OverallMetrics).Select(function (x) { return { 'label': x.PDX_Display }; }).ToArray();
                this.JQCharts = [];
                this.SelectedMetricChangedSubscription = this.SelectedMetric.subscribe(function (value) {
                    //html canvas will not render if the container is not visible by display:none
                    //since the binding of the charts is done at the same time the visibility of the metrics container is bound the charts are not actually rendered.
                    //The subscription event fires just before the actual visibility happens, so need to put a short timeout to wait before trying to replot the graph.
                    //Only needs to be done once, after the first time it does not need to be replotted unless the data has changed.
                    if (value.value == 1) {
                        _this.SelectedMetricChangedSubscription.dispose();
                        setTimeout(function () {
                            vm.JQCharts.forEach(function (chart) {
                                if (chart._drawCount === 0) {
                                    DataChecker.Charting.replot(chart);
                                }
                            });
                        }, 100);
                    }
                });
            }
            ViewModel.prototype.toPercent = function (count, total) {
                return parseFloat((count / total * 100).toFixed(2));
            };
            return ViewModel;
        }());
        DiagnosesPDX.ViewModel = ViewModel;
        function init(model, bindingControl) {
            var documentID = model.ResponseDocumentIDs[0];
            $.get('/DataChecker/DiagnosisPDX/ProcessMetrics?documentID=' + documentID).done(function (result) {
                _bindingControl = bindingControl;
                vm = new ViewModel(model, result);
                ko.applyBindings(vm, bindingControl[0]);
            }).fail(function (error) {
                alert(error);
            });
        }
        DiagnosesPDX.init = init;
        ko.bindingHandlers.pdxPercentChart = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var source = valueAccessor();
                var overallSeriesData = [];
                vm.EncounterTypes.forEach(function (encounterType, i) {
                    var v = $.Enumerable.From(source.data).SelectMany(function (x) { return $.Enumerable.From(x.Encounters).Where(function (ec) { return ec.EncType == encounterType.EncType; }).Select(function (ec) { return vm.toPercent(ec.Count, ec.Total); }); }).ToArray();
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
                    overallPercentBarSrc.multiSeriesData = { series_labels: vm.OverallSeriesLabels, ticks: vm.OverallTicks };
                    var chart = DataChecker.Charting.plotBarChart($(element), overallPercentBarSrc);
                    vm.JQCharts.push(chart);
                }
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            }
        };
    })(DiagnosesPDX = DataChecker.DiagnosesPDX || (DataChecker.DiagnosesPDX = {}));
})(DataChecker || (DataChecker = {}));
