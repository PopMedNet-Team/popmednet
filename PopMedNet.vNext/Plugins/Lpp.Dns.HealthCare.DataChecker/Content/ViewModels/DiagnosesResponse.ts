/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />

module DataChecker.Diagnoses {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public _model: IDiagnosesResultsModelData;
        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;

        public OverallMetrics: Array<any> = [];
        public CountByDataPartner: Array<any> = [];
        public DataPartners: Array<any> = [];
        public PercentByDataPartner: Array<any> = [];
        public Label: KnockoutObservable<string>;
        public HasResults: boolean = false;

        constructor(model: IDiagnosesResultsModelData) {
            this._model = model;
            
            this.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics",0 ), new ResponseMetricsItem("Count by Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2) ]);
            this.SelectedMetric = ko.observable(this.Metrics()[0]);
            
            var table: any = this._model.RawData.Table;
            var codes: Array<ICodeTypeKey> = $.Enumerable.From(table).GroupBy((x: IDiagnosesItemData) => <ICodeTypeKey>{ Code: x.DX, CodeType: x.Dx_Codetype }, (x: IDiagnosesItemData) => x, (key: ICodeTypeKey, group) => key, (key: ICodeTypeKey) => { return key.Code.toString() + key.CodeType.toString(); }).ToArray();
            var total_n = $.Enumerable.From(table).Sum((x: IDiagnosesItemData) => x.n);
            this.DataPartners = $.Enumerable.From(table).Distinct((item: IDiagnosesItemData) => item.DP).Select((item: IDiagnosesItemData) => item.DP).OrderBy(x => x).ToArray();
            this.Label = ko.observable('*Selected data partners include: ' + this.DataPartners.toString());
            this.OverallMetrics = $.Enumerable.From(table)
                .GroupBy((x: IDiagnosesItemData) => <any>{ Code: x.DX, CodeType: x.Dx_Codetype }, (y: IDiagnosesItemData) => y,
                (k, group) => <any>{
                    Code: k.Code,
                    CodeType: k.CodeType,
                    Count: $.Enumerable.From(group.source).Sum((x: IDiagnosesItemData) => x.n),
                    Percent: this.toPercent($.Enumerable.From(group.source).Sum((x: IDiagnosesItemData) => x.n), total_n)
                }, (key) => { return key.Code.toString() + key.CodeType.toString() }).ToArray();

            this.HasResults = this.OverallMetrics.length > 0;
            if (this.HasResults == false) {

                return;
            }

            $('#OverallMetricsChart').addClass(codes.length > 11 ? "overallmetric_barchart_fullwidth" : "overallmetric_barchart");
            $('#OverallMetricsChart').width(Math.max($('#OverallMetricsChart').width(), codes.length * 55));


            var chartSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select(x => [x.Code + ' (' + x.CodeType + ') ', x.Count]).ToArray());
            chartSource.yaxis_label = 'n';
            chartSource.xaxis_label = 'Diagnosis Code';
            chartSource.pointLabelFormatString = '%d';
            chartSource.title = this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution among Selected Data Partners*';
            DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), chartSource);

            var overallPercentSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select(x => [x.Code + ' (' + x.CodeType + ') ', x.Percent]).ToArray());
            overallPercentSource.yaxis_label = '%';
            overallPercentSource.xaxis_label = 'Diagnosis Code';
            overallPercentSource.pointLabelFormatString = '%.2f';
            overallPercentSource.isPercentage = true;
            overallPercentSource.title = this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution among Selected Data Partners*';
            DataChecker.Charting.plotBarChart($('#OverallMetricsPercentageBarChart'), overallPercentSource);

            var overallPieSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select(x => [x.Code + ' (' + x.CodeType + ') ' + x.Percent + '%', x.Percent / 100]).ToArray());
            overallPieSource.title = this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution among Selected Data Partners *';
            DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPieSource);

            var total_n_byCode = $.Enumerable.From(table).GroupBy((x: IDiagnosesItemData) => <ICodeTypeKey>{ Code: x.DX, CodeType: x.Dx_Codetype }, (x: IDiagnosesItemData) => x, (key: ICodeTypeKey, group) => <any>{ Code: key.Code, CodeType: key.CodeType, Total: $.Enumerable.From(group.source).Sum((x: IDiagnosesItemData) => x.n) }, (key: ICodeTypeKey) => { return key.Code.toString() + key.CodeType.toString() }).ToArray();
            var total_n_byPartner = $.Enumerable.From(table).GroupBy((x: IDiagnosesItemData) => x.DP, (x: IDiagnosesItemData) => x, (key, group) => <any>{ DataPartner: key, Total: $.Enumerable.From(group.source).Sum((x: IDiagnosesItemData) => x.n) }).ToArray();

            this.CountByDataPartner = $.Enumerable.From(table)
                .GroupBy((x: IDiagnosesItemData) => <ICodeTypeKey>{ Code: x.DX, CodeType: x.Dx_Codetype }, (y: IDiagnosesItemData) => y,
                (key: ICodeTypeKey, group) => <any>{
                    Code: key.Code,
                    CodeType: key.CodeType,
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{
                        Partner: dp,
                        Count: $.Enumerable.From(group.source).Where((x: IDiagnosesItemData) => x.DP == dp && x.Dx_Codetype == key.CodeType && x.DX == key.Code).Sum((x: IDiagnosesItemData) => x.n)
                    }).ToArray()
                }, (key: ICodeTypeKey) => { return key.Code.toString() + key.CodeType.toString() }).ToArray();

            //for each partner a single chart: code on x axis and counts on the y-axis
            var countByCode = $.Enumerable.From(table)
                .GroupBy((x: IDiagnosesItemData) => x.DP, (x: IDiagnosesItemData) => x,
                (key, group) => <any>{
                    Partner: key,
                    Total: $.Enumerable.From(total_n_byPartner).Where(x => key == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                    Codes: $.Enumerable.From(codes).Select((c: ICodeTypeKey) => <any>{
                        Code: c.Code,
                        CodeType: c.CodeType,
                        Total: $.Enumerable.From(total_n_byCode).Where(x => c.Code == x.Code && c.CodeType == x.CodeType).Select(x => x.Total).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where((x: IDiagnosesItemData) => x.DP == key && x.Dx_Codetype == c.CodeType && x.DX == c.Code).Sum((x: IDiagnosesItemData) => x.n)
                    }).ToArray()
                }).ToArray();

            this.PercentByDataPartner = $.Enumerable.From(table)
                .GroupBy((x: IDiagnosesItemData) => <ICodeTypeKey>{ Code: x.DX, CodeType: x.Dx_Codetype },
                (x: IDiagnosesItemData) => x,
                (k: ICodeTypeKey, group) => <any>{
                    Code: k.Code,
                    CodeType: k.CodeType,
                    Total: $.Enumerable.From(total_n_byCode).Where(x => k.Code == x.Code && k.CodeType == x.CodeType).Select(x => x.Total).FirstOrDefault(0),
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{
                        Partner: dp,
                        Total: $.Enumerable.From(total_n_byPartner).Where(x => dp == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where((x: IDiagnosesItemData) => x.DP == dp && x.DX == k.Code && x.Dx_Codetype == k.CodeType).Sum((x: IDiagnosesItemData) => x.n)
                    }).ToArray()
                }, (key: ICodeTypeKey) => { return key.Code.toString() + key.CodeType.toString() }).ToArray();


            var index = 1;

            var chartContainer = $('#DataPartnerMetrics');
            var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
            var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');            
            countByCode.forEach((item: any) => {
                var id = 'diagnosis_' + index++;
                var d = $('<div>').attr('id', id).addClass(codes.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                //$(d).width(Math.max($(d).width(), codes.length * 55));
                $(chartContainer).append(d);

                id = 'diagnosis_' + index++;
                var d2 = $('<div>').attr('id', id).addClass(codes.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                $(percentByDataPartnerContainer).append(d2);

                id = 'diagnosis_' + index++;
                var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                $(percentByDataPartnerContainerPie).append(p);

                var source = new ChartSource($.Enumerable.From(item.Codes).Select((x:any) => [x.Code + ' (' + x.CodeType + ') ', x.Count]).ToArray(), this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution within ' + item.Partner);
                source.xaxis_label = 'Diagnosis Code';
                source.yaxis_label = 'n';
                source.pointLabelFormatString = '%d';
                DataChecker.Charting.plotBarChart(d, source);

                var s2 = new ChartSource($.Enumerable.From(item.Codes).Select((x:any) => [x.Code + ' (' + x.CodeType + ') ', this.toPercent(x.Count, item.Total)]).ToArray(), this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution within ' + item.Partner);
                s2.xaxis_label = 'Diagnosis Code';
                s2.yaxis_label = '%';
                s2.pointLabelFormatString = '%.2f';
                s2.isPercentage = true;
                DataChecker.Charting.plotBarChart(d2, s2);

                var s3 = new ChartSource($.Enumerable.From(item.Codes).Select((x:any) => [x.Code + ' (' + x.CodeType + ') ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total) / 100]).ToArray(), this.formatCodeType(model.CodeType) + ' Diagnosis Code Distribution within ' + item.Partner);
                DataChecker.Charting.plotPieChart(p, s3);
            });

            var contributionContainerBar = $('#PercentDataPartnerContribution');
            var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');

            this.PercentByDataPartner.forEach((item: any) => {
                var id = 'contrib_percent_' + index++;

                var d = $('<div>').attr('id', id).addClass(this.DataPartners.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                $(d).width(Math.max((this.DataPartners.length * 80), 450));
                $(contributionContainerBar).append(d);

                id = 'contrib_percent_' + index++;
                var p = $('<div>').attr('id', id).addClass(this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                $(contributionContainerPie).append(p);

                var source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner, this.toPercent(x.Count, item.Total)]).ToArray(), 'Data Partner Contribution to ' + this.formatCodeType(item.CodeType) + ' Diagnosis code: ' + item.Code);
                source.xaxis_label = 'Data Partner';
                source.yaxis_label = '%';
                source.pointLabelFormatString = '%.2f';
                source.isPercentage = true;
                DataChecker.Charting.plotBarChart(d, source);

                source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner + ' ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total) / 100]).ToArray(), 'Data Partner Contribution to ' + this.formatCodeType(item.CodeType) + ' Diagnosis code: ' + item.Code);
                DataChecker.Charting.plotPieChart(p, source);
            });
        }

        public formatCodeType(codeType: string): string {

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
        }

        public toPercent(count: number, total: number): number {
            return parseFloat((count / total * 100).toFixed(2));
        }

    }

    export function init(model: IDiagnosesResultsModelData, bindingControl: JQuery) {
        _bindingControl = bindingControl;
        vm = new ViewModel(model);
        ko.applyBindings(vm, bindingControl[0]);
    }


    export interface IDiagnosesResultsModelData extends IResultsModelData {
        CodeType: string;
    }
}
