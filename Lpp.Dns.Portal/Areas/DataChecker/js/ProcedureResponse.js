/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
var DataChecker;
(function (DataChecker) {
    var DCProcedure;
    (function (DCProcedure) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
            function ViewModel(parameters) {
                this.requestID = ko.observable(null);
                this.responseID = ko.observable(null);
                this.isLoaded = ko.observable(false);
                this.codeType = ko.observable(null);
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
                self.formatCodeType = function (codeType) {
                    switch (codeType) {
                        case "09":
                            return "ICD-9-CM";
                        case "10":
                            return "ICD-10-CM";
                        case "11":
                            return "ICD-11-CM";
                        case "C2":
                            return "CPT Category II";
                        case "C3":
                            return "CPT Category III";
                        case "C4":
                            return "CPT-4 (i.e., HCPCS Level I)";
                        case "HC":
                            return "HCPCS (i.e., HCPCS Level II)";
                        case "H3":
                            return "HCPCS Level III";
                        case "LC":
                            return "LOINC";
                        case "LO":
                            return "Local homegrown";
                        case "ND":
                            return "NDC";
                        case "RE":
                            return "Revenue";
                        case "OT":
                            return "Other";
                    }
                    return codeType;
                };
                self.buildCharts = function (rxProcedurees) {
                    var plotArr = [];
                    $('#OverallMetricsCountChart').addClass(self.OverallMetrics.length > 11 ? "overallmetric_barchart_fullwidth" : "overallmetric_barchart");
                    $('#OverallMetricsCountChart').width(Math.max($('#OverallMetricsChart').width(), self.OverallMetrics.length * 55));
                    //overall metrics charts
                    var overallCountBarSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.Procedure_Display + ' (' + x.Code_Type + ') ', x.n]; }).ToArray());
                    overallCountBarSrc.title = ' Procedure Code Distribution among Selected Data Partners*';
                    overallCountBarSrc.yaxis_label = 'n';
                    overallCountBarSrc.xaxis_label = 'Procedure Code';
                    overallCountBarSrc.pointLabelFormatString = '%d';
                    plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsCountChart'), overallCountBarSrc));
                    var overallPercentBarSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.Procedure_Display + ' (' + x.Code_Type + ') ', x.Percent]; }).ToArray());
                    overallPercentBarSrc.yaxis_label = '%';
                    overallPercentBarSrc.xaxis_label = 'Procedure Code';
                    overallPercentBarSrc.title = 'Procedure Code Distribution among Selected Data Partners*';
                    overallPercentBarSrc.pointLabelFormatString = '%.2f';
                    overallPercentBarSrc.isPercentage = true;
                    plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));
                    var overallPercentPieSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.Procedure_Display + '(' + x.Code_Type + ')' + x.Percent.toFixed(2) + '%', x.Percent / 100]; }).ToArray());
                    overallPercentPieSrc.title = 'Procedure Code Distribution among Selected Data Partners*';
                    plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));
                    var index = 1;
                    //percent within data partners charts
                    var chartContainer = $('#DataPartnerMetrics');
                    var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                    var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                    self.CodesByPartner.forEach(function (item) {
                        var id = 'procedures_' + index++;
                        var d = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                        //$(d).width(Math.max($(d).width(), codes.length * 55));
                        $(chartContainer).append(d);
                        var id = 'procedures_' + index++;
                        var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                        //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                        $(percentByDataPartnerContainer).append(d2);
                        id = 'procedures_' + index++;
                        var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                        $(percentByDataPartnerContainerPie).append(p);
                        var source = new DataChecker.ChartSource($.Enumerable.From(item.Codes).Select(function (x) { return [x.Code_Display + ' (' + x.Code_Type + ') ', x.Count]; }).ToArray(), item.Partner);
                        source.xaxis_label = 'Procedure Code';
                        source.yaxis_label = 'n';
                        source.pointLabelFormatString = '%d';
                        source.title = 'Procedure Code Distribution within ' + item.Partner;
                        plotArr.push(DataChecker.Charting.plotBarChart(d, source));
                        var s2 = new DataChecker.ChartSource($.Enumerable.From(item.Codes).Select(function (x) { return [x.Code_Display + ' (' + x.Code_Type + ') ', Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.Partner);
                        s2.xaxis_label = 'Procedure Code';
                        s2.yaxis_label = '%';
                        s2.pointLabelFormatString = '%.2f';
                        s2.isPercentage = true;
                        s2.title = 'Procedure Code Distribution within ' + item.Partner;
                        plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));
                        var s3 = new DataChecker.ChartSource($.Enumerable.From(item.Codes).Select(function (x) { return [x.Code_Display + '(' + x.Code_Type + ') ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]; }).ToArray(), item.Partner);
                        s3.title = 'Procedure Code Distribution within ' + item.Partner;
                        plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                    });
                    //percent data partner contribution charts
                    var contributionContainerBar = $('#PercentDataPartnerContribution');
                    var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                    self.PartnersByCode.forEach(function (item) {
                        var id = 'contrib_percent_' + index++;
                        var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                        //$(d).width(Math.max((self.DataPartners.length * 80), 450));
                        $(contributionContainerBar).append(d);
                        id = 'contrib_percent_' + index++;
                        var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                        $(contributionContainerPie).append(p);
                        var s4 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.Procedure_Display);
                        s4.xaxis_label = 'Data Partner';
                        s4.yaxis_label = '%';
                        s4.pointLabelFormatString = '%.2f';
                        s4.isPercentage = true;
                        s4.title = 'Data Partner Contribution to ' + self.formatCodeType(item.Code_Type) + ' Procedure: ' + item.Procedure_Display;
                        plotArr.push(DataChecker.Charting.plotBarChart(d, s4));
                        var s5 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]; }).ToArray(), item.Procedure_Display);
                        s5.title = 'Data Partner Contribution to ' + self.formatCodeType(item.Code_Type) + ' Procedure: ' + item.Procedure_Display;
                        plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                    });
                    return plotArr;
                };
                self.responseID(parameters.ResponseID());
                self.requestID(parameters.RequestID());
                $.when($.get('/DataChecker/Procedure/GetTermValues?requestID=' + self.requestID().toString(), null, "script"), $.get('/DataChecker/Procedure/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")).then(function (termValues, metricResult) {
                    var rxProcedurees = termValues[0];
                    var result = metricResult[0];
                    self.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Count by DataPartner", 1), new DataChecker.ResponseMetricsItem("Data Partner Contribution", 2)]);
                    self.SelectedMetric = ko.observable(self.Metrics()[0]);
                    self.OverallMetrics = result.OverallMetrics;
                    self.CodesByPartner = result.CodesByPartner || [];
                    self.PartnersByCode = result.PartnersByCode || [];
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
                        self.chartPlots = self.buildCharts(rxProcedurees);
                        //resize the iframe to the contents plus padding for the export dropdown menu
                        $(window.frameElement).height($('html').height() + 70);
                    }
                }).fail(function (error) {
                    alert(error.statusText);
                    return;
                });
            }
            return ViewModel;
        }());
        DCProcedure.ViewModel = ViewModel;
    })(DCProcedure = DataChecker.DCProcedure || (DataChecker.DCProcedure = {}));
})(DataChecker || (DataChecker = {}));
