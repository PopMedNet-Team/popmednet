/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
var DataChecker;
(function (DataChecker) {
    var Sex;
    (function (Sex) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
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
                self.buildCharts = function (rxSexes) {
                    var plotArr = [];
                    //overall metrics charts
                    var overallPercentBarSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.Sex_Display, x.Percent]; }).ToArray());
                    overallPercentBarSrc.yaxis_label = '%';
                    overallPercentBarSrc.xaxis_label = 'Sex';
                    overallPercentBarSrc.title = 'Sex Distribution among Selected Data Partners*';
                    overallPercentBarSrc.pointLabelFormatString = '%.2f';
                    overallPercentBarSrc.isPercentage = true;
                    plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));
                    //var overallPercentPieSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: ISexOverallMetricItem) => [x.Sex_Display + ' ' + x.Percent + '%', x.Percent / 100]).ToArray());
                    //overallPercentPieSrc.title = 'Sex Distribution among Selected Data Partners*';
                    //plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));
                    var index = 1;
                    //percent within data partners charts
                    var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                    //var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                    self.CodesByPartner.forEach(function (item) {
                        var id = 'procedures_' + index++;
                        var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                        //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                        $(percentByDataPartnerContainer).append(d2);
                        //id = 'procedures_' + index++;
                        //var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                        //$(percentByDataPartnerContainerPie).append(p);
                        var s2 = new DataChecker.ChartSource($.Enumerable.From(item.Codes)
                            .Where(function (x) { return $.inArray(x.Code_Display, rxSexes) > -1; })
                            .Select(function (x) { return [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.Partner);
                        s2.xaxis_label = 'Sex';
                        s2.yaxis_label = '%';
                        s2.pointLabelFormatString = '%.2f';
                        s2.isPercentage = true;
                        s2.title = 'Sex Distribution within ' + item.Partner;
                        plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));
                        //var s3 = new ChartSource($.Enumerable.From(item.Codes)
                        //    .Where((x: ISexCodeTotalData) => $.inArray(x.Code_Display, rxSexes) > -1)
                        //    .Select((x: ISexCodeTotalData) => [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Partner);
                        //s3.title = 'Sex Distribution within ' + item.Partner;
                        //plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                    });
                    //percent data partner contribution charts
                    var contributionContainerBar = $('#PercentDataPartnerContribution');
                    //var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                    self.PartnersByCode.forEach(function (item) {
                        var id = 'contrib_percent_' + index++;
                        var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                        //$(d).width(Math.max((self.DataPartners.length * 80), 450));
                        $(contributionContainerBar).append(d);
                        //id = 'contrib_percent_' + index++;
                        //var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                        //$(contributionContainerPie).append(p);
                        var s4 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.Sex_Display);
                        s4.xaxis_label = 'Data Partner';
                        s4.yaxis_label = '%';
                        s4.pointLabelFormatString = '%.2f';
                        s4.isPercentage = true;
                        s4.title = 'Data Partner Contribution to Sex: ' + item.Sex_Display;
                        plotArr.push(DataChecker.Charting.plotBarChart(d, s4));
                        //var s5 = new ChartSource($.Enumerable.From(item.Partners).Select((x: ISexPartnerTotalData) => [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Sex_Display);
                        //s5.title = 'Data Partner Contribution to Sex: ' + item.Sex_Display;
                        //plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                    });
                    return plotArr;
                };
                self.responseID(parameters.ResponseID());
                self.requestID(parameters.RequestID());
                $.when($.get('/DataChecker/Sex/GetTermValues?requestID=' + self.requestID().toString(), null, "script"), $.get('/DataChecker/Sex/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")).then(function (termValues, metricResult) {
                    var rxSexes = termValues[0];
                    var result = metricResult[0];
                    self.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Percent within Data Partner", 1), new DataChecker.ResponseMetricsItem("Data Partner Contribution", 2)]);
                    self.SelectedMetric = ko.observable(self.Metrics()[0]);
                    self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where(function (x) { return $.inArray(x.Sex_Display, self.ToSexValues(rxSexes)) > -1; }).ToArray();
                    self.CodesByPartner = result.CodesByPartner || [];
                    self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where(function (x) { return $.inArray(x.Sex_Display, self.ToSexValues(rxSexes)) > -1; }).ToArray();
                    self.DataPartners = result.DataPartners || [];
                    self.HasResults = (self.OverallMetrics.length > 0 && self.DataPartners.length > 0 && self.PartnersByCode.length > 0);
                    self.isLoaded(true);
                    self.SelectedMetric.subscribe(function () {
                        self.chartPlots.forEach(function (chart) {
                            chart.replot({ resetAxes: true });
                        });
                        //resize the iframe to the contents plus padding for the export dropdown menu
                        $(window.frameElement).height($('html').height() + 70);
                    });
                    if (self.HasResults) {
                        self.chartPlots = self.buildCharts(self.ToSexValues(rxSexes));
                        //resize the iframe to the contents plus padding for the export dropdown menu
                        $(window.frameElement).height($('html').height() + 70);
                    }
                }).fail(function (error) {
                    alert(error.statusText);
                    return;
                });
            }
            ViewModel.prototype.ToSexValues = function (rxSexTypes) {
                var rxValues = [];
                rxSexTypes.forEach(function (amtType) {
                    switch (amtType) {
                        case "0":
                            rxValues.push("Ambiguous");
                            break;
                        case "1":
                            rxValues.push("Female");
                            break;
                        case "2":
                            rxValues.push("Male");
                            break;
                        case "3":
                            rxValues.push("No Information");
                            break;
                        case "4":
                            rxValues.push("NULL or Missing");
                            break;
                        case "5":
                            rxValues.push("Other");
                            break;
                        case "6":
                            rxValues.push("Unknown");
                            break;
                        case "7":
                            rxValues.push("Values outside of CDM specifications");
                            break;
                        case "OTHER":
                            rxValues.push("Unselected Categories");
                            break;
                    }
                });
                rxValues.push("Unselected Categories");
                return rxValues;
            };
            ViewModel.prototype.determineSexTitle = function (sexID) {
                switch (sexID) {
                    case "OT": return "Other";
                    case "UN": return "Unknown";
                    case "A": return "Ambiguous";
                    case "F": return "Female";
                    case "M": return "Male";
                    case "NI": return "No Information";
                    case "NULL or Missing": return "NULL or Missing";
                    case "Values outside of CDM specifications": return "Values outside of CDM specifications";
                    case "OTHER": return "Unselected Categories";
                }
                return "missing definition: " + sexID;
            };
            return ViewModel;
        }());
        Sex.ViewModel = ViewModel;
    })(Sex = DataChecker.Sex || (DataChecker.Sex = {}));
})(DataChecker || (DataChecker = {}));
//# sourceMappingURL=SexResponse.js.map