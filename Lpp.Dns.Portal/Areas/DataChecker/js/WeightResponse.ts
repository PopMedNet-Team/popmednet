/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.Weight {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public OverallMetrics: Array<IWeightOverallMetricItem>;
        public DataPartners: Array<string>;
        public CodesByPartner: Array<IWeightPartnerGroupingData>;
        public PartnersByCode: Array<IWeightCodeGroupingData>;
        public HasResults: boolean;

        public chartPlots = [];

        buildCharts: (rxWeightes: string[]) => any[];

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

            self.buildCharts = (rxWeightes: string[]) => {
                var plotArr = [];

                //overall metrics charts
                var overallPercentBarSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IWeightOverallMetricItem) => [x.Weight_Display, x.Percent]).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'Weight';
                overallPercentBarSrc.title = 'Weight Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));

                //var overallPercentPieSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IWeightOverallMetricItem) => [x.Weight_Display + ' ' + x.Percent + '%', x.Percent / 100]).ToArray());
                //overallPercentPieSrc.title = 'Weight Distribution among Selected Data Partners*';
                //plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));

                var index = 1;

                //percent within data partners charts
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                //var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                self.CodesByPartner.forEach((item: IWeightPartnerGroupingData) => {

                    var id = 'procedures_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);

                    //id = 'procedures_' + index++;
                    //var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                    //$(percentByDataPartnerContainerPie).append(p);

                    var s2 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: IWeightCodeTotalData) => $.inArray(x.Code_Display, rxWeightes) > -1)
                        .Select((x: IWeightCodeTotalData) => [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Partner);
                    s2.xaxis_label = 'Weight'; s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'Weight Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));

                    //var s3 = new ChartSource($.Enumerable.From(item.Codes)
                    //    .Where((x: IWeightCodeTotalData) => $.inArray(x.Code_Display, rxWeightes) > -1)
                    //    .Select((x: IWeightCodeTotalData) => [x.Code_Display + ' ' + Global.Helpers.ToPercent(x.Count, item.Total) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Partner);
                    //s3.title = 'Weight Distribution within ' + item.Partner;
                    //plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                });

                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                //var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                self.PartnersByCode.forEach((item: IWeightCodeGroupingData) => {
                    var id = 'contrib_percent_' + index++;

                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    //$(d).width(Math.max((self.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);

                    //id = 'contrib_percent_' + index++;
                    //var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    //$(contributionContainerPie).append(p);

                    var s4 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IWeightPartnerTotalData) => [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Weight_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to Weight: ' + item.Weight_Display;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, s4));

                    //var s5 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IWeightPartnerTotalData) => [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Weight_Display);
                    //s5.title = 'Data Partner Contribution to Weight: ' + item.Weight_Display;
                    //plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                });

                return plotArr;
            };

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/Weight/GetTermValues?requestID=' + self.requestID().toString(), null, "script"),
                $.get('/DataChecker/Weight/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")
                ).then((termValues: any[], metricResult: IWeightItemResultsData[]) => {

                var rxWeightes: string[] = termValues[0];

                var result: IWeightItemResultsData = metricResult[0];

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where((x: IWeightOverallMetricItem) => $.inArray(x.Weight_Display, self.ToWeightValues(rxWeightes)) > -1).ToArray();
                self.CodesByPartner = result.CodesByPartner || [];
                self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where((x: IWeightCodeGroupingData) => $.inArray(x.Weight_Display, self.ToWeightValues(rxWeightes)) > -1).ToArray();
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
                    self.chartPlots = self.buildCharts(self.ToWeightValues(rxWeightes));

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }

            }).fail((error: JQueryXHR) => {

                alert(error.statusText);
                return;
            });
        }

        private ToWeightValues(rxWeightTypes: string[]) {
            var rxValues = [];
            rxWeightTypes.forEach(amtType => {
                switch (amtType) {
                    case "0":
                        rxValues.push("<0 lbs");
                        break;
                    case "1":
                        rxValues.push("0-1 lbs");
                        break;
                    case "2":
                        rxValues.push("2-6 lbs");
                        break;
                    case "3":
                        rxValues.push("7-12 lbs");
                        break;
                    case "4":
                        rxValues.push("13-20 lbs");
                        break;
                    case "5":
                        rxValues.push("21-35 lbs");
                        break;
                    case "6":
                        rxValues.push("36-50 lbs");
                        break;
                    case "7":
                        rxValues.push("51-75 lbs");
                        break;
                    case "8":
                        rxValues.push("75-100 lbs");
                        break;
                    case "9":
                        rxValues.push("101-125 lbs");
                        break;
                    case "10":
                        rxValues.push("126-150 lbs");
                        break;
                    case "11":
                        rxValues.push("151-175 lbs");
                        break;
                    case "12":
                        rxValues.push("176-200 lbs");
                        break;
                    case "13":
                        rxValues.push("201-225 lbs");
                        break;
                    case "14":
                        rxValues.push("226-250 lbs");
                        break;
                    case "15":
                        rxValues.push("251-275 lbs");
                        break;
                    case "16":
                        rxValues.push("276-300 lbs");
                        break;
                    case "17":
                        rxValues.push("301-350 lbs");
                        break;
                    case "18":
                        rxValues.push("350+ lbs");
                        break;
                    case "19":
                        rxValues.push("NULL or Missing");
                        break;
                   
                }
            });

            rxValues.push("Other");
            return rxValues;
        }

        public determineWeightTitle(weightID: string): string {
            switch (weightID) {
                case "<0": return "<0 lbs";
                case "0-1": return "0-1 lbs";
                case "2-6": return "2-6 lbs";
                case "7-12": return "7-12 lbs";
                case "13-20": return "13-20 lbs";
                case "21-35": return "21-35 lbs";
                case "36-50": return "36-50 lbs";
                case "51-75": return "51-75 lbs";
                case "76-100": return "76-100 lbs";
                case "101-125": return "101-125 lbs";
                case "126-150": return "126-150 lbs";
                case "151-175": return "151-175 lbs";
                case "176-200": return "176-200 lbs";
                case "201-225": return "201-225 lbs";
                case "226-250": return "226-250 lbs";
                case "251-275": return "251-275 lbs";
                case "276-300": return "276-300 lbs";
                case "301-350": return "301-350 lbs";
                case "350+": return "350+ lbs";
                case "NULL or Missing": return "NULL or Missing";
                case "OTHER": return "OTHER";
            }

            return "missing definition: " + weightID;
        }
    }

    export interface IWeightResultsModelData extends IResultsModelData {
        RxValues: string[];
    }

    export interface IWeightItemResultsData {
        DataPartners: string[];
        OverallMetrics: IWeightOverallMetricItem[];
        CodesByPartner: IWeightPartnerGroupingData[];
        PartnersByCode: IWeightCodeGroupingData[];
    }

    export interface IWeightOverallMetricItem {
        Weight: string;
        Weight_Display: string;
        n: number;
        Percent: number;
    }

    export interface IWeightCodeTotalData {
        Code: string;
        Code_Display: string;
        Count: number;
    }

    export interface IWeightPartnerGroupingData {
        Partner: string;
        Total: number;
        Codes: IWeightCodeTotalData[];
    }

    export interface IWeightPartnerTotalData {
        Partner: string;
        Total: number;
        Count: number;
    }

    export interface IWeightCodeGroupingData {
        Weight: string;
        Weight_Display: string;
        Total: number;
        Partners: IWeightPartnerTotalData[];
    }
}  