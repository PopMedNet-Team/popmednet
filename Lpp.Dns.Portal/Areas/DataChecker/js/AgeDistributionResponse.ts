/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.AgeDistribution {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public OverallMetrics: Array<IAgeOverallMetricItem>;
        public DataPartners: Array<string>;
        public CodesByPartner: Array<IAgePartnerGroupingData>;
        public PartnersByCode: Array<IAgeCodeGroupingData>;
        public HasResults: boolean;

        public chartPlots = [];

        buildCharts: (rxAgees: string[]) => any[];

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

            self.buildCharts = (rxAgees: string[]) => {
                var plotArr = [];

                //overall metrics charts
                var overallPercentBarSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IAgeOverallMetricItem) => [x.Age_Display, x.Percent]).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'Age';
                overallPercentBarSrc.title = 'Age Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));

                //var overallPercentPieSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IAgeOverallMetricItem) => [x.Age_Display + ' ' + x.Percent + '%', x.Percent / 100]).ToArray());
                //overallPercentPieSrc.title = 'Age Distribution among Selected Data Partners*';
                //plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));

                var index = 1;

                //percent within data partners charts
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                self.CodesByPartner.forEach((item: IAgePartnerGroupingData) => {

                    var id = 'procedures_' + index++;
                    var d2 = $('<div>').attr('id', id).addClass(self.CodesByPartner.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                    //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                    $(percentByDataPartnerContainer).append(d2);

                    var s2 = new ChartSource($.Enumerable.From(item.Codes)
                        .Where((x: IAgeCodeTotalData) => $.inArray(x.Code_Display, rxAgees) > -1)
                        .Select((x: IAgeCodeTotalData) => [x.Code_Display, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Partner);
                    s2.xaxis_label = 'Age'; s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'Age Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));
                });

                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                self.PartnersByCode.forEach((item: IAgeCodeGroupingData) => {
                    var id = 'contrib_percent_' + index++;

                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    $(contributionContainerBar).append(d);

                    var s4 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IAgePartnerTotalData) => [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Age_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to Age: ' + item.Age_Display;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, s4));
                });

                return plotArr;
            };
            
            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());
            $.when<any>(
                $.get('/DataChecker/AgeDistribution/GetTermValues?requestID=' + self.requestID().toString(), null, "script"),
                $.get('/DataChecker/AgeDistribution/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")
                ).then((termValues: any[], metricResult: IAgeItemResultsData[]) => {
                var rxAgees: string[] = termValues[0];

                var result: IAgeItemResultsData = metricResult[0];

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.OverallMetrics = $.Enumerable.From(result.OverallMetrics).Where((x: IAgeOverallMetricItem) => $.inArray(x.Age_Display, self.ToAgeValues(rxAgees)) > -1).ToArray();
                self.CodesByPartner = result.CodesByPartner || [];
                self.PartnersByCode = $.Enumerable.From(result.PartnersByCode).Where((x: IAgeCodeGroupingData) => $.inArray(x.Age_Display, self.ToAgeValues(rxAgees)) > -1).ToArray();
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
                    self.chartPlots = self.buildCharts(self.ToAgeValues(rxAgees));

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }

            }).fail((error: JQueryXHR) => {
                alert(error.statusText);
                return;
                });

            
        }

        private ToAgeValues(rxAgeTypes: string[]) {
            var rxValues = [];
            rxAgeTypes.forEach(amtType => {
                switch (amtType) {
                    case "0":
                        rxValues.push("<0 yrs");
                        break;
                    case "1":
                        rxValues.push("0-1 yrs");
                        break;
                    case "2":
                        rxValues.push("2-4 yrs");
                        break;
                    case "3":
                        rxValues.push("5-9 yrs");
                        break;
                    case "4":
                        rxValues.push("10-14 yrs");
                        break;
                    case "5":
                        rxValues.push("15-18 yrs");
                        break;
                    case "6":
                        rxValues.push("19-21 yrs");
                        break;
                    case "7":
                        rxValues.push("22-44 yrs");
                        break;
                    case "8":
                        rxValues.push("45-64 yrs");
                        break;
                    case "9":
                        rxValues.push("65-74 yrs");
                        break;
                    case "10":
                        rxValues.push("75-110 yrs");
                        break;
                    case "11":
                        rxValues.push(">110 yrs");
                        break;
                    case "12":
                        rxValues.push("NULL or Missing");
                        break;
                }
            });

            rxValues.push("Other");
            return rxValues;
        }

        public determineAgeTitle(ageID: string): string {
            switch (ageID) {
                case "<0": return "<0 years";
                case "0-1": return "0-1 years";
                case "2-4": return "2-4 years";
                case "5-6": return "5-9 years";
                case "10-14": return "10-14 years";
                case "15-18": return "15-18 years";
                case "19-21": return "19-21 years";
                case "22-44": return "22-44 years";
                case "45-64": return "45-64 years";
                case "65-74": return "65-74 years";
                case "75-110": return "75-110 years";
                case ">110": return ">110 years";
                case "NULL or Missing": return "NULL or Missing";
                case "Other": return "Other";
            }

            return "missing definition: " + ageID;
        }
    }

    export interface IAgeResultsModelData extends IResultsModelData {
        RxValues: string[];
    }

    export interface IAgeItemResultsData {
        DataPartners: string[];
        OverallMetrics: IAgeOverallMetricItem[];
        CodesByPartner: IAgePartnerGroupingData[];
        PartnersByCode: IAgeCodeGroupingData[];
    }

    export interface IAgeOverallMetricItem {
        Age: string;
        Age_Display: string;
        n: number;
        Percent: number;
    }

    export interface IAgeCodeTotalData {
        Code: string;
        Code_Display: string;
        Count: number;
    }

    export interface IAgePartnerGroupingData {
        Partner: string;
        Total: number;
        Codes: IAgeCodeTotalData[];
    }

    export interface IAgePartnerTotalData {
        Partner: string;
        Total: number;
        Count: number;
    }

    export interface IAgeCodeGroupingData {
        Age: string;
        Age_Display: string;
        Total: number;
        Partners: IAgePartnerTotalData[];
    }
}  