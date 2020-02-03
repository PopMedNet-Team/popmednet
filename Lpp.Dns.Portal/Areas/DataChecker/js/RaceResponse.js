/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
var DataChecker;
(function (DataChecker) {
    var Race;
    (function (Race) {
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
                self.buildCharts = function (rxRaces) {
                    var plotArr = [];
                    //overall metrics charts
                    var overallPercentBarSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.Race_Display, x.Percent]; }).ToArray());
                    overallPercentBarSrc.yaxis_label = '%';
                    overallPercentBarSrc.xaxis_label = 'Race';
                    overallPercentBarSrc.title = 'Race Distribution among Selected Data Partners*';
                    overallPercentBarSrc.pointLabelFormatString = '%.2f';
                    overallPercentBarSrc.isPercentage = true;
                    plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));
                    var overallPercentPieSrc = new DataChecker.ChartSource($.Enumerable.From(self.OverallMetrics).Select(function (x) { return [x.Race_Display + ' ' + x.Percent.toFixed(2) + '%', x.Percent / 100]; }).ToArray());
                    overallPercentPieSrc.title = 'Race Distribution among Selected Data Partners*';
                    plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));
                    var index = 1;
                    //percent within data partners charts
                    var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                    var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                    self.CodesByPartner.forEach(function (item) {
                        var id = 'procedures_' + index++;
                        var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                        //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                        $(percentByDataPartnerContainer).append(d2);
                        id = 'procedures_' + index++;
                        var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                        $(percentByDataPartnerContainerPie).append(p);
                        var s2 = new DataChecker.ChartSource($.Enumerable.From(item.Codes)
                            .Where(function (x) { return $.inArray(x.Code_Display, rxRaces) > -1; })
                            .Select(function (x) { return [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.Partner);
                        s2.xaxis_label = 'Race';
                        s2.yaxis_label = '%';
                        s2.pointLabelFormatString = '%.2f';
                        s2.isPercentage = true;
                        s2.title = 'Race Distribution within ' + item.Partner;
                        plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));
                        var s3 = new DataChecker.ChartSource($.Enumerable.From(item.Codes)
                            .Where(function (x) { return $.inArray(x.Code_Display, rxRaces) > -1; })
                            .Select(function (x) { return [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]; }).ToArray(), item.Partner);
                        s3.title = 'Race Distribution within ' + item.Partner;
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
                        var s4 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]; }).ToArray(), item.Race_Display);
                        s4.xaxis_label = 'Data Partner';
                        s4.yaxis_label = '%';
                        s4.pointLabelFormatString = '%.2f';
                        s4.isPercentage = true;
                        s4.title = 'Data Partner Contribution to Race: ' + item.Race_Display;
                        plotArr.push(DataChecker.Charting.plotBarChart(d, s4));
                        var s5 = new DataChecker.ChartSource($.Enumerable.From(item.Partners).Select(function (x) { return [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]; }).ToArray(), item.Race_Display);
                        s5.title = 'Data Partner Contribution to Race: ' + item.Race_Display;
                        plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                    });
                    return plotArr;
                };
                self.responseID(parameters.ResponseID());
                self.requestID(parameters.RequestID());
                $.when($.get('/DataChecker/Race/GetTermValues?requestID=' + self.requestID().toString(), null, "script"), $.get('/DataChecker/Race/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")).then(function (termValues, metricResult) {
                    var rxRaces = termValues[0];
                    var result = metricResult[0];
                    self.Metrics = ko.observableArray([new DataChecker.ResponseMetricsItem("Overall Metrics", 0), new DataChecker.ResponseMetricsItem("Percent within Data Partner", 1), new DataChecker.ResponseMetricsItem("Data Partner Contribution", 2)]);
                    self.SelectedMetric = ko.observable(self.Metrics()[0]);
                    self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where(function (x) { return $.inArray(x.Race_Display, self.ToRaceValues(rxRaces)) > -1; }).ToArray();
                    self.CodesByPartner = result.CodesByPartner || [];
                    self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where(function (x) { return $.inArray(x.Race_Display, self.ToRaceValues(rxRaces)) > -1; }).ToArray();
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
                        self.chartPlots = self.buildCharts(self.ToRaceValues(rxRaces));
                        //resize the iframe to the contents plus padding for the export dropdown menu
                        $(window.frameElement).height($('html').height() + 70);
                    }
                }).fail(function (error) {
                    alert(error.statusText);
                    return;
                });
            }
            ViewModel.prototype.ToRaceValues = function (rxRacesTypes) {
                var rxValues = [];
                rxRacesTypes.forEach(function (amtType) {
                    switch (amtType) {
                        case -1:
                            rxValues.push("Other");
                            break;
                        case 0:
                            rxValues.push("Unknown");
                            break;
                        case 1:
                            rxValues.push("American Indian or Alaska Native");
                            break;
                        case 2:
                            rxValues.push("Asian");
                            break;
                        case 3:
                            rxValues.push("Black or African American");
                            break;
                        case 4:
                            rxValues.push("Native Hawaiian or Other Pacific Islander");
                            break;
                        case 5:
                            rxValues.push("White");
                            break;
                        case 6:
                            rxValues.push("Missing");
                            break;
                    }
                });
                rxValues.push("Other");
                return rxValues;
            };
            ViewModel.prototype.determineRaceTitle = function (raceID) {
                switch (raceID) {
                    case "-1": return "Other";
                    case "0": return "Unknown";
                    case "1": return "American Indian or Alaska Native";
                    case "2": return "Asian";
                    case "3": return "Black or African American";
                    case "4": return "Native Hawaiian or Other Pacific Islander";
                    case "5": return "White";
                    case "6": return "Missing";
                }
                return "missing definition: " + raceID;
            };
            return ViewModel;
        }());
        Race.ViewModel = ViewModel;
    })(Race = DataChecker.Race || (DataChecker.Race = {}));
})(DataChecker || (DataChecker = {}));
//# sourceMappingURL=RaceResponse.js.map