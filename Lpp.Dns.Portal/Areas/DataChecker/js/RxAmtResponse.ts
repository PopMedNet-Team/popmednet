/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.RxAmt {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public OverallMetrics: Array<IOverallMetricItem>;
        public DataPartners: Array<string>;
        public CodesByPartner: Array<IPartnerGroupingData>;
        public PartnersByCode: Array<ICodeGroupingData>;
        public HasResults: boolean;

        public chartPlots = [];

        buildCharts: (rxAmts: string[]) => any[];

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

            self.buildCharts = (rxAmts: string[]) => {
                var plotArr = [];

                //overall metrics charts
                var overallPercentBarSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IOverallMetricItem) => [x.RxAmt_Display, x.Percent]).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'RxAmt';
                overallPercentBarSrc.title = 'RxAmt Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));

                var overallPercentPieSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IOverallMetricItem) => [x.RxAmt_Display + ' ' + x.Percent.toFixed(2) + '%', x.Percent / 100]).ToArray());
                overallPercentPieSrc.title = 'RxAmt Distribution among Selected Data Partners*';
                plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));

                var index = 1;

                //percent within data partners charts
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                self.CodesByPartner.forEach((item: IPartnerGroupingData) => {

                    var id = 'procedures_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);

                    id = 'procedures_' + index++;
                    var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    $(percentByDataPartnerContainerPie).append(p);

                    var s2 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: ICodeTotalData) => $.inArray(x.Code, rxAmts) > -1)
                        .Select((x: ICodeTotalData) => [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Partner);
                    s2.xaxis_label = 'RxAmt'; s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'RxAmt Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));

                    var s3 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: ICodeTotalData) => $.inArray(x.Code, rxAmts) > -1)
                        .Select((x: ICodeTotalData) => [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Partner);
                    s3.title = 'RxAmt Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                });

                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                self.PartnersByCode.forEach((item: ICodeGroupingData) => {
                    var id = 'contrib_percent_' + index++;

                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    //$(d).width(Math.max((self.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);

                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);

                    var s4 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IPartnerTotalData) => [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.RxAmt_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to RxAmt: ' + item.RxAmt_Display;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, s4));

                    var s5 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IPartnerTotalData) => [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.RxAmt_Display);
                    s5.title = 'Data Partner Contribution to RxAmt: ' + item.RxAmt_Display;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                });

                return plotArr;
            };

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/RxAmt/GetRxAmounts?requestID=' + self.requestID().toString()),
                $.get('/DataChecker/RxAmt/ProcessMetricsByResponse?responseID=' + self.responseID().toString())
                ).then((arrAmounts: any[], metricResult: IRxAmtItemResultsData[]) => {

                var rxAmounts: number[] = arrAmounts[0];

                var result: IRxAmtItemResultsData = metricResult[0];

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where((x: IOverallMetricItem) => $.inArray(x.RxAmt, self.ToRxAmounts(rxAmounts)) > -1).ToArray();
                self.CodesByPartner = result.CodesByPartner || [];
                self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where((x: ICodeGroupingData) => $.inArray(x.RxAmt, self.ToRxAmounts(rxAmounts)) > -1).ToArray();
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
                    self.chartPlots = self.buildCharts(self.ToRxAmounts(rxAmounts));

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }

            }).fail((error) => {
                alert(error);
                return;
            });
        }

        private ToRxAmounts(rxAmtTypes: number[]) {
            //alert(rxAmtTypes.length);
            var rxAmts = [];
            rxAmtTypes.forEach(amtType => {
                //alert(amtType);

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
        }
    }

    export interface IRxAmtResultsModelData extends IResultsModelData {
        RxAmounts: number[];
    }

    export interface IRxAmtItemResultsData {
        DataPartners: string[];
        OverallMetrics: IOverallMetricItem[];
        CodesByPartner: IPartnerGroupingData[];
        PartnersByCode: ICodeGroupingData[];
    }

    export interface IOverallMetricItem {
        RxAmt: string;
        RxAmt_Display: string;
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
        RxAmt: string;
        RxAmt_Display: string;
        Total: number;
        Partners: IPartnerTotalData[];
    }
} 