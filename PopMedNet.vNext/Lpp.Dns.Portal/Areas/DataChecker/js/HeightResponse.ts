﻿/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.Height {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public OverallMetrics: Array<IHeightOverallMetricItem>;
        public DataPartners: Array<string>;
        public CodesByPartner: Array<IHeightPartnerGroupingData>;
        public PartnersByCode: Array<IHeightCodeGroupingData>;
        public HasResults: boolean;

        public chartPlots = [];

        buildCharts: (rxHeightes: string[]) => any[];

        constructor(parameters: any) {
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

            self.buildCharts = (rxHeightes: string[]) => {
                var plotArr = [];

                //overall metrics charts
                var overallPercentBarSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IHeightOverallMetricItem) => [x.Height_Display, x.Percent]).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'Height';
                overallPercentBarSrc.title = 'Height Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));

                //var overallPercentPieSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IHeightOverallMetricItem) => [x.Height_Display + ' ' + x.Percent + '%', x.Percent / 100]).ToArray());
                //overallPercentPieSrc.title = 'Height Distribution among Selected Data Partners*';
                //plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));

                var index = 1;

                //percent within data partners charts
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                //var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                self.CodesByPartner.forEach((item: IHeightPartnerGroupingData) => {

                    var id = 'procedures_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);

                    //id = 'procedures_' + index++;
                    //var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    //$(percentByDataPartnerContainerPie).append(p);

                    var s2 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: IHeightCodeTotalData) => $.inArray(x.Code_Display, rxHeightes) > -1)
                        .Select((x: IHeightCodeTotalData) => [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Partner);
                    s2.xaxis_label = 'Height'; s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'Height Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));

                    //var s3 = new ChartSource($.Enumerable.From(item.Codes)
                    //    .Where((x: IHeightCodeTotalData) => $.inArray(x.Code_Display, rxHeightes) > -1)
                    //    .Select((x: IHeightCodeTotalData) => [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Partner);
                    //s3.title = 'Height Distribution within ' + item.Partner;
                    //plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                });

                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                //var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                self.PartnersByCode.forEach((item: IHeightCodeGroupingData) => {
                    var id = 'contrib_percent_' + index++;

                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    //$(d).width(Math.max((self.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);

                    //id = 'contrib_percent_' + index++;
                    //var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    //$(contributionContainerPie).append(p);

                    var s4 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IHeightPartnerTotalData) => [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Height_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to Height: ' + item.Height_Display;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, s4));

                    //var s5 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IHeightPartnerTotalData) => [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Height_Display);
                    //s5.title = 'Data Partner Contribution to Height: ' + item.Height_Display;
                    //plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                });

                return plotArr;
            };

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/Height/GetTermValues?requestID=' + self.requestID().toString(), null, "script"),
                $.get('/DataChecker/Height/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")
                ).then((termValues: any[], metricResult: IHeightItemResultsData[]) => {

                var rxHeightes: string[] = termValues[0];

                var result: IHeightItemResultsData = metricResult[0];

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where((x: IHeightOverallMetricItem) => $.inArray(x.Height_Display, self.ToHeightValues(rxHeightes)) > -1).ToArray();
                self.CodesByPartner = result.CodesByPartner || [];
                self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where((x: IHeightCodeGroupingData) => $.inArray(x.Height_Display, self.ToHeightValues(rxHeightes)) > -1).ToArray();
                self.DataPartners = result.DataPartners || [];

                self.HasResults = (self.OverallMetrics.length > 0 && self.DataPartners.length > 0 && self.PartnersByCode.length > 0);
                self.isLoaded(true);

                self.SelectedMetric.subscribe(function () {
                    self.chartPlots.forEach((chart) => {
                        chart.replot({ resetAxes: true });
                    });

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                });

                if (self.HasResults) {
                    self.chartPlots = self.buildCharts(self.ToHeightValues(rxHeightes));

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }

            }).fail((error: JQueryXHR) => {

                alert(error.statusText);
                return;
            });
        }

        private ToHeightValues(rxHeightTypes: string[]) {
            var rxValues = [];
            rxHeightTypes.forEach(amtType => {
                switch (amtType) {
                    case "0":
                        rxValues.push("0-10 in");
                        break;
                    case "1":
                        rxValues.push("11-20 in");
                        break;
                    case "2":
                        rxValues.push("21-45 in");
                        break;
                    case "3":
                        rxValues.push("46-52 in");
                        break;
                    case "4":
                        rxValues.push("53-58 in");
                        break;
                    case "5":
                        rxValues.push("59-64 in");
                        break;
                    case "6":
                        rxValues.push("65-70 in");
                        break;
                    case "7":
                        rxValues.push("71-76 in");
                        break;
                    case "8":
                        rxValues.push("77-82 in");
                        break;
                    case "9":
                        rxValues.push("83-88 in");
                        break;
                    case "10":
                        rxValues.push("89-94 in");
                        break;
                    case "11":
                        rxValues.push("95+ in");
                        break;
                    case "12":
                        rxValues.push("<0 in");
                        break;
                    case "13":
                        rxValues.push("NULL or Missing");
                        break;
                }
            });

            rxValues.push("Other");
            return rxValues;
        }

        public determineHeightTitle(heightID: string): string {
            switch (heightID) {
                case "<0": return "<0 in";
                case "0-10": return "0-10 in";
                case "11-20": return "11-20 in";
                case "21-45": return "21-45 in";
                case "46-52": return "46-52 in";
                case "53-58": return "53-58 in";
                case "59-64": return "59-64 in";
                case "65-70": return "65-70 in";
                case "71-76": return "71-76 in";
                case "77-82": return "77-82 in";
                case "83-88": return "83-88 in";
                case "89-94": return "89-94 in";
                case "95+": return "95+ in";
                case "NULL or Missing": return "NULL or Missing";
                case "OTHER": return "OTHER";
            }

            return "missing definition: " + heightID;
        }
    }

    export interface IHeightResultsModelData extends IResultsModelData {
        RxValues: string[];
    }

    export interface IHeightItemResultsData {
        DataPartners: string[];
        OverallMetrics: IHeightOverallMetricItem[];
        CodesByPartner: IHeightPartnerGroupingData[];
        PartnersByCode: IHeightCodeGroupingData[];
    }

    export interface IHeightOverallMetricItem {
        Height: string;
        Height_Display: string;
        n: number;
        Percent: number;
    }

    export interface IHeightCodeTotalData {
        Code: string;
        Code_Display: string;
        Count: number;
    }

    export interface IHeightPartnerGroupingData {
        Partner: string;
        Total: number;
        Codes: IHeightCodeTotalData[];
    }

    export interface IHeightPartnerTotalData {
        Partner: string;
        Total: number;
        Count: number;
    }

    export interface IHeightCodeGroupingData {
        Height: string;
        Height_Display: string;
        Total: number;
        Partners: IHeightPartnerTotalData[];
    }
}   