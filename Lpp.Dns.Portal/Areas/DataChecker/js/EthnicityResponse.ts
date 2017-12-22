/// <reference path="rxamtresponse.ts" />
/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.Ethnicity {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public OverallMetrics: Array<IEthnicityOverallMetricItem>;
        public DataPartners: Array<string>;
        public CodesByPartner: Array<IEthnicityPartnerGroupingData>;
        public PartnersByCode: Array<IEthnicityCodeGroupingData>;
        public HasResults: boolean;

        public chartPlots = [];

        buildCharts: (rxData: string[]) => any[];

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

            self.buildCharts = (rxData: string[]) => {
                var plotArr = [];

                //overall metrics charts
                var overallPercentBarSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IEthnicityOverallMetricItem) => [x.Hispanic_Display, x.Percent]).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'Ethnicity';
                overallPercentBarSrc.title = 'Ethnicity Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));

                var overallPercentPieSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IEthnicityOverallMetricItem) => [x.Hispanic_Display + ' ' + x.Percent.toFixed(2) + '%', x.Percent / 100]).ToArray());
                overallPercentPieSrc.title = 'Ethnicity Distribution among Selected Data Partners*';
                plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));

                var index = 1;

                //percent within data partners charts
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                self.CodesByPartner.forEach((item: IEthnicityPartnerGroupingData) => {

                    var id = 'procedures_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);

                    id = 'procedures_' + index++;
                    var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    $(percentByDataPartnerContainerPie).append(p);

                    var s2 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: IEthnicityCodeTotalData) => $.inArray(x.Code_Display, rxData) > -1)
                        .Select((x: IEthnicityCodeTotalData) => [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Partner);
                    s2.xaxis_label = 'Ethnicity'; s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'Ethnicity Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));

                    var s3 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: IEthnicityCodeTotalData) => $.inArray(x.Code_Display, rxData) > -1)
                        .Select((x: IEthnicityCodeTotalData) => [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Partner);
                    s3.title = 'Ethnicity Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                });

                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                self.PartnersByCode.forEach((item: IEthnicityCodeGroupingData) => {
                    var id = 'contrib_percent_' + index++;

                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    //$(d).width(Math.max((self.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);

                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);

                    var s4 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IEthnicityPartnerTotalData) => [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Hispanic_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to Ethnicity: ' + item.Hispanic_Display;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, s4));

                    var s5 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IEthnicityPartnerTotalData) => [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Hispanic_Display);
                    s5.title = 'Data Partner Contribution to Ethnicity: ' + item.Hispanic_Display;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                });

                return plotArr;
            };

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/Ethnicity/GetTermValues?requestID=' + self.requestID().toString(), null, "script"),
                $.get('/DataChecker/Ethnicity/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")
                ).then((termValues: any[], metricResult: IEthnicityItemResultsData[]) => {

                var rxHispanics: number[] = termValues[0];

                var result: IEthnicityItemResultsData = metricResult[0];

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where((x: IEthnicityOverallMetricItem) => $.inArray(x.Hispanic_Display, self.ToHispanicValues(rxHispanics)) > -1).ToArray();
                self.CodesByPartner = result.CodesByPartner || [];
                self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where((x: IEthnicityCodeGroupingData) => $.inArray(x.Hispanic_Display, self.ToHispanicValues(rxHispanics)) > -1).ToArray();
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
                    self.chartPlots = self.buildCharts(self.ToHispanicValues(rxHispanics));

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }

            }).fail((error: JQueryXHR) => {

                alert(error.statusText);
                return;
            });

        }

        private ToHispanicValues(rxTypes: number[]) {
            var rxValues = [];
            rxTypes.forEach(amtType => {
                switch (amtType) {
                    case 0:
                        rxValues.push("Unknown");
                        break;
                    case 1:
                        rxValues.push("Hispanic");
                        break;
                    case 2:
                        rxValues.push("Not Hispanic");
                        break;
                    case 3:
                        rxValues.push("Missing");
                        break;
                }
            });

            rxValues.push("Other");
            return rxValues;
        }
    }

    export interface IEthnicityResultsModelData extends IResultsModelData {
        RxValues: number[];
    }

    export interface IEthnicityItemResultsData {
        DataPartners: string[];
        OverallMetrics: IEthnicityOverallMetricItem[];
        CodesByPartner: IEthnicityPartnerGroupingData[];
        PartnersByCode: IEthnicityCodeGroupingData[];
    }

    export interface IEthnicityOverallMetricItem {
        Hispanic: string;
        Hispanic_Display: string;
        n: number;
        Percent: number;
    }

    export interface IEthnicityCodeTotalData {
        Code: string;
        Code_Display: string;
        Count: number;
    }

    export interface IEthnicityPartnerGroupingData {
        Partner: string;
        Total: number;
        Codes: IEthnicityCodeTotalData[];
    }

    export interface IEthnicityPartnerTotalData {
        Partner: string;
        Total: number;
        Count: number;
    }

    export interface IEthnicityCodeGroupingData {
        Hispanic: string;
        Hispanic_Display: string;
        Total: number;
        Partners: IEthnicityPartnerTotalData[];
    }
}