﻿/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.RxSup {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public _documentID: number;
        public OverallMetrics: Array<IOverallMetricItem>;
        public DataPartners: Array<string>;
        public CodesByPartner: Array<IPartnerGroupingData>;
        public PartnersByCode: Array<ICodeGroupingData>;
        public HasResults: boolean;

        public chartPlots = [];

        buildCharts: (rxSups: string[]) => any[];

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

            self.buildCharts = (rxSups: string[]) => {
                var plotArr = [];
                //overall metrics charts
                var overallPercentBarSrc = new ChartSource($.Enumerable.From(this.OverallMetrics).Select((x: IOverallMetricItem) => [x.RxSup_Display, x.Percent]).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'RxSup';
                overallPercentBarSrc.title = 'RxSup Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));

                var overallPercentPieSrc = new ChartSource($.Enumerable.From(this.OverallMetrics).Select((x: IOverallMetricItem) => [x.RxSup_Display + ' ' + x.Percent.toFixed(2) + '%', x.Percent / 100]).ToArray());
                overallPercentPieSrc.title = 'RxSup Distribution among Selected Data Partners*';
                plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));

                var index = 1;

                //percent within data partners charts
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                this.CodesByPartner.forEach((item: IPartnerGroupingData) => {
                    var id = 'procedures_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(this.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);

                    id = 'procedures_' + index++;
                    var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    $(percentByDataPartnerContainerPie).append(p);

                    var s2 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: ICodeTotalData) => $.inArray(x.Code, rxSups) > -1)
                        .Select((x: ICodeTotalData) => [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Partner);
                    s2.xaxis_label = 'RxSup';
                    s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'RxSup Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));

                    var s3 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: ICodeTotalData) => $.inArray(x.Code, rxSups) > -1)
                        .Select((x: ICodeTotalData) => [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Partner);
                    s3.title = 'RxSup Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                });

                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                this.PartnersByCode.forEach((item: ICodeGroupingData) => {
                    var id = 'contrib_percent_' + index++;

                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    //$(d).width(Math.max((this.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);

                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);

                    var s4 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IPartnerTotalData) => [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.RxSup_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to RxSup: ' + item.RxSup_Display;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, s4));

                    var s5 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IPartnerTotalData) => [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.RxSup_Display);
                    s5.title = 'Data Partner Contribution to RxSup: ' + item.RxSup_Display;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                });

                return plotArr;
            };

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/RxSup/GetRxSupplies?requestID=' + self.requestID().toString()),
                $.get('/DataChecker/RxSup/ProcessMetricsByResponse?responseID=' + self.responseID().toString())
                ).then((arrAmounts: any[], metricResult: IRxSupItemResultsData[]) => {

                var rxSups: number[] = arrAmounts[0];

                var result: IRxSupItemResultsData = metricResult[0];

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where((x: IOverallMetricItem) => $.inArray(x.RxSup, self.ToRxSups(rxSups)) > -1).ToArray();
                self.CodesByPartner = result.CodesByPartner || [];
                self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where((x: ICodeGroupingData) => $.inArray(x.RxSup, self.ToRxSups(rxSups)) > -1).ToArray();
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
                    self.chartPlots = self.buildCharts(self.ToRxSups(rxSups));

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }

            }).fail((error) => {
                alert(error);
                return;
            });
        }

        private ToRxSups(rxAmtTypes: number[]): string[] {
            var rxSups: string[] = [];
            rxAmtTypes.forEach(amtType => {
                switch (amtType) {
                    case 0:
                        rxSups.push("-1");
                        break;
                    case 1:
                        rxSups.push("0");
                        break;
                    case 2:
                        rxSups.push("2");
                        break;
                    case 3:
                        rxSups.push("30");
                        break;
                    case 4:
                        rxSups.push("60");
                        break;
                    case 5:
                        rxSups.push("90");
                        break;
                    //case 6:
                    //    rxSups.push("OTHER");
                    //    break;
                    case 7:
                        rxSups.push("MISSING");
                        break;
                }
            });

            // Always include OTHER.
            rxSups.push("OTHER");
            return rxSups;
        }
    }

    export interface IRxSupResultsModelData extends IResultsModelData {
        RxSups: number[];
    }

    export interface IRxSupItemResultsData {
        DataPartners: string[];
        OverallMetrics: IOverallMetricItem[];
        CodesByPartner: IPartnerGroupingData[];
        PartnersByCode: ICodeGroupingData[];
    }

    export interface IOverallMetricItem {
        RxSup: string;
        RxSup_Display: string;
        n: number;
        Percent: number;
    }

    export interface ICodeTotalData {
        Code: string;
        Code_Display: string;
        Count: number;
    }

    export interface IPartnerGroupingData {
        Partner: string;
        Total: number;
        Codes: ICodeTotalData[];
    }

    export interface IPartnerTotalData {
        Partner: string;
        Total: number;
        Count: number;
    }

    export interface ICodeGroupingData {
        RxSup: string;
        RxSup_Display: string;
        Total: number;
        Partners: IPartnerTotalData[];
    }
} 