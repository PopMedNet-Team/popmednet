/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.DCDiagnosis {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);
        public codeType: KnockoutObservable<string> = ko.observable(null);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public OverallMetrics: Array<IDiagnosisOverallMetricItem>;
        public DataPartners: Array<string>;
        public CodesByPartner: Array<IDiagnosisPartnerGroupingData>;
        public PartnersByCode: Array<IDiagnosisCodeGroupingData>;
        public HasResults: boolean;

        public chartPlots = [];

        formatCodeType: (codeType: string) => string;
        buildCharts: (rxDiagnosises: string[]) => any[];

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

            self.formatCodeType = (codeType: string) => {

            switch (codeType) {
                case "09":
                    return 'ICD-9-CM';
                case "10":
                    return 'ICD-10-CM';
                case "11":
                    return 'ICD-11-CM';
                case "SM":
                    return 'SNOMED CT';
                case "OT":
                    return 'Other';
            }

            return codeType;
            };

            self.buildCharts = (rxDiagnosises: string[]) => {
                var plotArr = [];

                $('#OverallMetricsCountChart').addClass(self.OverallMetrics.length > 11 ? "overallmetric_barchart_fullwidth" : "overallmetric_barchart");
                $('#OverallMetricsCountChart').width(Math.max($('#OverallMetricsCountChart').width(), self.OverallMetrics.length * 55));

                var codeTypeChart = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IDiagnosisOverallMetricItem) => [x.Code_Type]).ToArray());
                self.codeType = codeTypeChart.data[0][0];

                //overall metrics charts
                var overallCountBarSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IDiagnosisOverallMetricItem) => [x.Diagnosis_Display + ' (' + x.Code_Type + ')  ', x.n]).ToArray());
                overallCountBarSrc.yaxis_label = 'n';
                overallCountBarSrc.xaxis_label = 'Diagnosis Code';
                overallCountBarSrc.pointLabelFormatString = '%d';
                overallCountBarSrc.title = ' Diagnosis Code Distribution among Selected Data Partners*';
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsCountChart'), overallCountBarSrc));

                var overallPercentBarSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IDiagnosisOverallMetricItem) => [x.Diagnosis_Display + ' (' + x.Code_Type + ') ', x.Percent]).ToArray());
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'Diagnosis Code';
                overallPercentBarSrc.title = 'Diagnosis Code Distribution among Selected Data Partners*';
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.isPercentage = true;
                plotArr.push(DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), overallPercentBarSrc));

                var overallPercentPieSrc = new ChartSource($.Enumerable.From(self.OverallMetrics).Select((x: IDiagnosisOverallMetricItem) => [x.Diagnosis_Display + ' (' + x.Code_Type + ') ' + x.Percent.toFixed(2) + '%', x.Percent / 100]).ToArray());
                overallPercentPieSrc.title = 'Diagnosis Code Distribution among Selected Data Partners*';
                plotArr.push(DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPercentPieSrc));

                var index = 1;

                //percent within data partners charts
                var chartContainer = $('#DataPartnerMetrics');
                var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
                var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');
                self.CodesByPartner.forEach((item: IDiagnosisPartnerGroupingData) => {
                    
                    var id = 'diagnosis_' + index++;
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

                    //
                    var source = new ChartSource($.Enumerable.From(item.Codes)
                        .Select((x: IDiagnosisCodeTotalData) => [x.Code_Display + ' (' + x.Code_Type + ') ', x.Count]).ToArray(), item.Partner);
                    source.xaxis_label = 'Diagnosis Code';
                    source.yaxis_label = 'n';
                    source.pointLabelFormatString = '%d';
                    source.title = 'Diagnosis Code Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, source));

                    var s2 = new ChartSource($.Enumerable.From(item.Codes)
                        .Select((x: IDiagnosisCodeTotalData) => [x.Code_Display + ' (' + x.Code_Type + ') ', Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Partner);
                    s2.xaxis_label = 'Diagnosis Code'; s2.yaxis_label = '%';
                    s2.pointLabelFormatString = '%.2f';
                    s2.isPercentage = true;
                    s2.title = 'Diagnosis Code Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotBarChart(d2, s2));

                    var s3 = new ChartSource($.Enumerable.From(item.Codes)
                        .Select((x: IDiagnosisCodeTotalData) => [x.Code_Display + '(' + x.Code_Type + ')  ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Partner);
                    s3.title = 'Diagnosis Code Distribution within ' + item.Partner;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s3));
                });

                //percent data partner contribution charts
                var contributionContainerBar = $('#PercentDataPartnerContribution');
                var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
                self.PartnersByCode.forEach((item: IDiagnosisCodeGroupingData) => {
                    var id = 'contrib_percent_' + index++;

                    var d = $('<div>').attr('id', id).addClass("fullwidth-barchart-dpc");
                    //$(d).width(Math.max((self.DataPartners.length * 80), 450));
                    $(contributionContainerBar).append(d);

                    id = 'contrib_percent_' + index++;
                    var p = $('<div>').attr('id', id).addClass(self.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                    $(contributionContainerPie).append(p);

                    var s4 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IDiagnosisPartnerTotalData) => [x.Partner, Global.Helpers.ToPercent(x.Count, item.Total)]).ToArray(), item.Diagnosis_Display);
                    s4.xaxis_label = 'Data Partner';
                    s4.yaxis_label = '%';
                    s4.pointLabelFormatString = '%.2f';
                    s4.isPercentage = true;
                    s4.title = 'Data Partner Contribution to ' + self.formatCodeType(item.Code_Type) + ' Diagnosis code: ' + item.Diagnosis_Display;
                    plotArr.push(DataChecker.Charting.plotBarChart(d, s4));

                    var s5 = new ChartSource($.Enumerable.From(item.Partners).Select((x: IDiagnosisPartnerTotalData) => [x.Partner + ' ' + Global.Helpers.ToPercent(x.Count, item.Total).toFixed(2) + '%', Global.Helpers.ToPercent(x.Count, item.Total) / 100]).ToArray(), item.Diagnosis_Display);
                    s5.title = 'Data Partner Contribution to ' + self.formatCodeType(item.Code_Type) + ' Diagnosis code: ' + item.Diagnosis_Display;
                    plotArr.push(DataChecker.Charting.plotPieChart(p, s5));
                });

                return plotArr;
            };

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/Diagnosis/GetTermValues?requestID=' + self.requestID().toString(), null, "script"),
                $.get('/DataChecker/Diagnosis/ProcessMetricsByResponse?responseID=' + self.responseID().toString(), null, "script")
                ).then((termValues: any[], metricResult: IDiagnosisItemResultsData[]) => {

                var rxDiagnosises: string[] = termValues[0];

                var result: IDiagnosisItemResultsData = metricResult[0];

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Count by DataPartner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.OverallMetrics = result.OverallMetrics;
                self.CodesByPartner = result.CodesByPartner || [];
                self.PartnersByCode = result.PartnersByCode || [];
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
                    self.chartPlots = self.buildCharts(rxDiagnosises);

                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }

            }).fail((error: JQueryXHR) => {

                alert(error.statusText);
                return;
            });
        }

        //public formatCodeType(codeType: string): string {
        //    switch (codeType) {
        //        case "09":
        //            return 'ICD-9-CM';
        //        case "10":
        //            return 'ICD-10-CM';
        //        case "11":
        //            return 'ICD-11-CM';
        //        case "SM":
        //            return 'SNOMED CT';
        //        case "OT":
        //            return 'Other';
        //    }
        //    return codeType;
        //}
    }

    export interface IDiagnosisResultsModelData extends IResultsModelData {
        RxValues: string[];
    }

    export interface IDiagnosisItemResultsData {
        DataPartners: string[];
        OverallMetrics: IDiagnosisOverallMetricItem[];
        CodesByPartner: IDiagnosisPartnerGroupingData[];
        PartnersByCode: IDiagnosisCodeGroupingData[];
    }

    export interface IDiagnosisOverallMetricItem {
        Diagnosis: string;
        Diagnosis_Display: string;
        Code_Type: string;
        n: number;
        Percent: number;
    }

    export interface IDiagnosisCodeTotalData {
        Code: string;
        Code_Display: string;
        Code_Type: string;
        Count: number;
    }

    export interface IDiagnosisPartnerGroupingData {
        Partner: string;
        Total: number;
        Codes: IDiagnosisCodeTotalData[];
    }

    export interface IDiagnosisPartnerTotalData {
        Partner: string;
        Total: number;
        Count: number;
    }

    export interface IDiagnosisCodeGroupingData {
        Diagnosis: string;
        Diagnosis_Display: string;
        Code_Type: string;
        Total: number;
        Partners: IDiagnosisPartnerTotalData[];
    }
}  