var DataChecker;
(function (DataChecker) {
    var RxAmt;
    (function (RxAmt) {
        var vm;
        var _bindingControl;
        var ViewModel = (function () {
            function ViewModel(parameters) {
                this.requestID = ko.observable(null);
                this.responseID = ko.observable(null);
                this.isLoaded = ko.observable(false);
                this.chartPlots = [];
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
                self.buildCharts = function (rxAmts) {
                    var plotArr = [];
                    var overallPercentBarSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.RxAmt_Display, x.Percent]; }).ToArray());
                    overallPercentBarSrc.yaxis_label = '%';
                    overallPercentBarSrc.xaxis_label = 'RxAmt';
                    overallPercentBarSrc.title = 'RxAmt Distribution among Selected Data Partners*';
                    overallPercentBarSrc.pointLabelFormatString = '%.2f';
                    overallPercentBarSrc.isPercentage = true;
                    plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));
                    var overallPercentPieSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.RxAmt_Display + ' ' + x.Percent.toFixed(2) + '%', x.Percent / 100]; }).ToArray());
                    overallPercentPieSrc.title = 'RxAmt Distribution among Selected Data Partners*';
                    plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));
                    var index = 1;
                    var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                    var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                    self.CodesByPartner.forEach(function (item) {
                        var id = 'procedures_' + index++;
                        var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                        $(percentByDataPartnerContainer).append(d2);
                        id = 'procedures_' + index++;
                        var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                        $(percentByDataPartnerContainerPie).append(p);
                        var s2 = new DataChecker.ChartSource($.Enumerable.From(item.Codes)
                            .Where(function (x) { return $.inArray(x.Code, rxAmts) > -1; })
                            .Select(function (x) { return [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.Partner);
                        s2.xaxis_label = 'RxAmt';
                        s2.yaxis_label = '%';
                        s2.pointLabelFormatString = '%.2f';
                        s2.isPercentage = true;
                        s2.title = 'RxAmt Distribution within ' + item.Partner;
                        plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));
                        var s3 = new DataChecker.ChartSource($.Enumerable.From(item.Codes)
                            .Where(function (x) { return $.inArray(x.Code, rxAmts) > -1; })
                            .Select(function (x) { return [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]; }).ToArray(), item.Partner);
                        s3.title = 'RxAmt Distribution within ' + item.Partner;
                        plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                    });
                    var contributionContainerBar = $('#PercentDataPartnerContribution');
                    var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                    self.PartnersByCode.forEach(function (item) {
                        var id = 'contrib_percent_' + index++;
                        var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                        $(contributionContainerBar).append(d);
                        id = 'contrib_percent_' + index++;
                        var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                        $(contributionContainerPie).append(p);
                        var s4 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.RxAmt_Display);
                        s4.xaxis_label = 'Data Partner';
                        s4.yaxis_label = '%';
                        s4.pointLabelFormatString = '%.2f';
                        s4.isPercentage = true;
                        s4.title = 'Data Partner Contribution to RxAmt: ' + item.RxAmt_Display;
                        plotArr.push(DataChecker.Charting.plotBarChart(d, s4));
                        var s5 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]; }).ToArray(), item.RxAmt_Display);
                        s5.title = 'Data Partner Contribution to RxAmt: ' + item.RxAmt_Display;
                        plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                    });
                    return plotArr;
                };
                self.responseID(parameters.ResponseID());
                self.requestID(parameters.RequestID());
                $.when($.get('/DataChecker/RxAmt/GetRxAmounts?requestID=' + self.requestID().toString()), $.get('/DataChecker/RxAmt/ProcessMetricsByResponse?responseID=' + self.responseID().toString())).then(function (arrAmounts, metricResult) {
                    var rxAmounts = arrAmounts[0];
                    var result = metricResult[0];
                    self.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Percent within Data Partner", 1), new DataChecker.ResponseMetricsItem("Data Partner Contribution", 2)]);
                    self.SelectedMetric = ko.observable(self.Metrics()[0]);
                    self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where(function (x) { return $.inArray(x.RxAmt, self.ToRxAmounts(rxAmounts)) > -1; }).ToArray();
                    self.CodesByPartner = result.CodesByPartner || [];
                    self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where(function (x) { return $.inArray(x.RxAmt, self.ToRxAmounts(rxAmounts)) > -1; }).ToArray();
                    self.DataPartners = result.DataPartners || [];
                    self.HasResults = (self.OverallMetrics.length > 0 && self.DataPartners.length > 0 && self.PartnersByCode.length > 0);
                    self.isLoaded(true);
                    self.SelectedMetric.subscribe(function () {
                        self.chartPlots.forEach(function (chart) {
                            chart.replot({ resetAxes: true });
                        });
                        $(window.frameElement).height($('html').height() + 70);
                    });
                    if (self.HasResults) {
                        self.chartPlots = self.buildCharts(self.ToRxAmounts(rxAmounts));
                        $(window.frameElement).height($('html').height() + 70);
                    }
                }).fail(function (error) {
                    alert(error);
                    return;
                });
            }
            ViewModel.prototype.ToRxAmounts = function (rxAmtTypes) {
                var rxAmts = [];
                rxAmtTypes.forEach(function (amtType) {
                    switch (amtType) {
                        case 0:
                            rxAmts.push("-1");
                            break;
                        case 1:
                            rxAmts.push("0");
                            break;
                        case 2:
                            rxAmts.push("30");
                            break;
                        case 3:
                            rxAmts.push("60");
                            break;
                        case 4:
                            rxAmts.push("90");
                            break;
                        case 5:
                            rxAmts.push("120");
                            break;
                        case 6:
                            rxAmts.push("180");
                            break;
                        case 7:
                            rxAmts.push("181");
                            break;
                        case 9:
                            rxAmts.push("MISSING");
                            break;
                    }
                });
                rxAmts.push("OTHER");
                return rxAmts;
            };
            return ViewModel;
        }());
        RxAmt.ViewModel = ViewModel;
    })(RxAmt = DataChecker.RxAmt || (DataChecker.RxAmt = {}));
})(DataChecker || (DataChecker = {}));
