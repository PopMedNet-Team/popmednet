/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
var DataChecker;
(function (DataChecker) {
    var RxAmt;
    (function (RxAmt) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
            function ViewModel(model, result) {
                var _this = this;
                this.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Percent within Data Partner", 1), new DataChecker.ResponseMetricsItem("Data Partner Contribution", 2)]);
                this.SelectedMetric = ko.observable(this.Metrics()[0]);
                this._documentID = model.ResponseDocumentIDs[0];
                this.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where(function (x) { return $.inArray(x.RxAmt, _this.ToRxAmounts(model.RxAmounts)) > -1; }).ToArray();
                this.CodesByPartner = result.CodesByPartner || [];
                this.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where(function (x) { return $.inArray(x.RxAmt, _this.ToRxAmounts(model.RxAmounts)) > -1; }).ToArray();
                this.DataPartners = result.DataPartners || [];
                this.HasResults = (this.OverallMetrics.length > 0 && this.DataPartners.length > 0 && this.PartnersByCode.length > 0);
                if (this.HasResults) {
                    this.buildCharts(this.ToRxAmounts(model.RxAmounts));
                }
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
                        ////////case 8:
                        //    rxAmts.push("OTHER");
                        //    break;
                        case 9:
                            rxAmts.push("MISSING");
                            break;
                    }
                });
                rxAmts.push("OTHER");
                return rxAmts;
            };
            ViewModel.prototype.buildCharts = function (rxAmts) {
                var _this = this;
                //overall metrics charts
                var overallPercentBarSrc = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).Select(function (x) { return [x.RxAmt_Display, x.Percent]; }).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'RxAmt';
                overallPercentBarSrc.title = 'RxAmt Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc);
                var overallPercentPieSrc = new DataChecker.ChartSource($.Enumerable.From(this.OverallMetrics).Select(function (x) { return [x.RxAmt_Display + ' ' + x.Percent + '%', x.Percent / 100]; }).ToArray());
                overallPercentPieSrc.title = 'RxAmt Distribution among Selected Data Partners*';
                DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc);
                var index = 1;
                //percent within data partners charts
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                this.CodesByPartner.forEach(function (item) {
                    var id = 'procedures_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(_this.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);
                    id = 'procedures_' + index++;
                    var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    $(percentByDataPartnerContainerPie).append(p);
                    var s2 = new DataChecker.ChartSource($.Enumerable.From(item.Codes)
                        .Where(function (x) { return $.inArray(x.Code, rxAmts) > -1; })
                        .Select(function (x) { return [x.Code_Display, _this.toPercent(x.Count, item.Total)]; }).ToArray(), item.Partner);
                    s2.xaxis_label = 'RxAmt';
                    s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'RxAmt Distribution within ' + item.Partner;
                    DataChecker.Charting.plotBarChart(d2, s2);
                    var s3 = new DataChecker.ChartSource($.Enumerable.From(item.Codes)
                        .Where(function (x) { return $.inArray(x.Code, rxAmts) > -1; })
                        .Select(function (x) { return [x.Code_Display + ' ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total) / 100]; }).ToArray(), item.Partner);
                    s3.title = 'RxAmt Distribution within ' + item.Partner;
                    DataChecker.Charting.plotPieChart(p, s3);
                });
                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                this.PartnersByCode.forEach(function (item) {
                    var id = 'contrib_percent_' + index++;
                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    //$(d).width(Math.max((this.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);
                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(_this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);
                    var s4 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, _this.toPercent(x.Count, item.Total)]; }).ToArray(), item.RxAmt_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to RxAmt: ' + item.RxAmt_Display;
                    DataChecker.Charting.plotBarChart(d, s4);
                    var s5 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner + ' ' + _this.toPercent(x.Count, item.Total) + '%', _this.toPercent(x.Count, item.Total) / 100]; }).ToArray(), item.RxAmt_Display);
                    s5.title = 'Data Partner Contribution to RxAmt: ' + item.RxAmt_Display;
                    DataChecker.Charting.plotPieChart(p, s5);
                });
            };
            ViewModel.prototype.toPercent = function (count, total) {
                return total <= 0 ? 0 : parseFloat((count / total * 100).toFixed(2));
            };
            return ViewModel;
        }());
        RxAmt.ViewModel = ViewModel;
        function init(model, bindingControl) {
            var documentID = model.ResponseDocumentIDs[0];
            $.get('/DataChecker/RxAmt/ProcessMetrics?documentID=' + documentID).done(function (result) {
                _bindingControl = bindingControl;
                vm = new ViewModel(model, result);
                ko.applyBindings(vm, bindingControl[0]);
            })
                .fail(function (error) {
                alert(error);
            });
        }
        RxAmt.init = init;
    })(RxAmt = DataChecker.RxAmt || (DataChecker.RxAmt = {}));
})(DataChecker || (DataChecker = {}));
