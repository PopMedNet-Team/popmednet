/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
module DataChecker.Procedures {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public _model: IProcedureResultsModelData;
        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;

        public DataPartners: Array<any> = [];
        public OverallMetrics: Array<any> = [];
        public CountByDataPartner: Array<any> = [];
        public PercentByDataPartner: Array<any> = [];
        public Label: KnockoutObservable<string>;
        public HasResults: boolean = false;

        constructor(model: IProcedureResultsModelData) {
            this._model = model;

            this.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Count by Data Partner", 1), new ResponseMetricsItem("Data Partner Contribution", 2)]);
            this.SelectedMetric = ko.observable(this.Metrics()[0]);


            var table = this._model.RawData.Table;
            var codes = $.Enumerable.From(table).GroupBy((x: IProcedureItemData) => <any>{ Code: x.PX, CodeType: x.Px_Codetype }, (x: IProcedureItemData) => x, (key, group) => key, (key) => { return key.Code.toString() + key.CodeType.toString() }).ToArray();
            var total_n = $.Enumerable.From(table).Sum((x: IProcedureItemData) => x.n);
            this.DataPartners = $.Enumerable.From(table).Distinct((item: IProcedureItemData) => item.DP).Select((item: IProcedureItemData) => item.DP).OrderBy(x => x).ToArray();
            this.Label = ko.observable('*Selected data partners include: ' + this.DataPartners.toString());
            this.OverallMetrics = $.Enumerable.From(table)
                .GroupBy((x: IProcedureItemData) => <any>{ Code: x.PX, CodeType: x.Px_Codetype },
                (x: IProcedureItemData) => x,
                (key, group) => <any>{
                    Code: key.Code,
                    CodeType: key.CodeType,
                    Count: $.Enumerable.From(group.source).Sum((x: IProcedureItemData) => x.n),
                    Percent: this.toPercent($.Enumerable.From(group.source).Sum((x: IProcedureItemData) => x.n), total_n)
                }, (key) => { return key.Code.toString() + key.CodeType.toString() }).OrderBy(c => c.Code).ToArray();


            this.HasResults = this.OverallMetrics.length > 0;
            if (!this.HasResults) {
                return;
            }
            
            $('#OverallMetricsChart').addClass(codes.length > 11 ? "overallmetric_barchart_fullwidth" : "overallmetric_barchart");
            $('#OverallMetricsChart').width(Math.max($('#OverallMetricsChart').width(), codes.length * 55));

            var chartSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select(x => [x.Code + ' (' + x.CodeType + ') ', x.Count]).ToArray());
            chartSource.title = this.formatCodeType(model.CodeType) + ' Procedure Code Distribution among Selected Data Partners*';
            chartSource.yaxis_label = 'n';
            chartSource.xaxis_label = 'Procedure Code';
            chartSource.pointLabelFormatString = '%d';
            DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), chartSource);

            var overallPercentSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select(x => [x.Code + ' (' + x.CodeType + ') ', x.Percent]).ToArray());
            overallPercentSource.title = this.formatCodeType(model.CodeType) + ' Procedure Code Distribution among Selected Data Partners*';
            overallPercentSource.yaxis_label = '%';
            overallPercentSource.xaxis_label = 'Procedure Code';
            overallPercentSource.pointLabelFormatString = '%.2f';
            overallPercentSource.isPercentage = true;
            DataChecker.Charting.plotBarChart($('#OverallMetricsPercentageBarChart'), overallPercentSource);

            var overallPieSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select(x => [x.Code + ' (' + x.CodeType + ') ' + x.Percent + '%', x.Percent / 100]).ToArray());
            overallPieSource.title = this.formatCodeType(model.CodeType) + ' Procedure Code Distribution among Selected Data Partners*';
            DataChecker.Charting.plotPieChart($('#OverallMetricsPieChart'), overallPieSource);


            var total_n_byCode = $.Enumerable.From(table).GroupBy((x: IProcedureItemData) => <any>{ Code: x.PX, CodeType: x.Px_Codetype }, (x: IProcedureItemData) => x, (key, group) => <any>{ Code: key.Code, CodeType: key.CodeType, Total: $.Enumerable.From(group.source).Sum((x: IProcedureItemData) => x.n) }, (key) => { return key.Code.toString() + key.CodeType.toString() }).ToArray();
            var total_n_byPartner = $.Enumerable.From(table).GroupBy((x: IProcedureItemData) => x.DP, (x: IProcedureItemData) => x, (key, group) => <any>{ DataPartner: key, Total: $.Enumerable.From(group.source).Sum((x: IProcedureItemData) => x.n) }).ToArray();

            this.CountByDataPartner = $.Enumerable.From(table)
                .GroupBy((x: IProcedureItemData) => <any>{ Code: x.PX, CodeType: x.Px_Codetype }, (y: IProcedureItemData) => y, (key, group) => <any>{
                    Code: key.Code,
                    CodeType: key.CodeType,
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{ Partner: dp, Count: $.Enumerable.From(group.source).Where((x: IProcedureItemData) => x.PX == key.Code && x.Px_Codetype == key.CodeType && x.DP == dp).Sum((x: IProcedureItemData) => x.n) }).ToArray()
                }, (key) => { return key.Code.toString() + key.CodeType.toString() }).OrderBy(c => c.Code).ToArray();

            var countByCode = $.Enumerable.From(table)
                .GroupBy((x: IProcedureItemData) => x.DP, (x: IProcedureItemData) => x,
                (key, group) => <any>{
                    Partner: key,
                    Total: $.Enumerable.From(total_n_byPartner).Where(x => key == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                    Codes: $.Enumerable.From(codes).Select(c => <any>{
                        Code: c.Code,
                        CodeType: c.CodeType,
                        Count: $.Enumerable.From(group.source).Where((x: IProcedureItemData) => x.DP == key && x.PX == c.Code && x.Px_Codetype == c.CodeType).Sum((x: IProcedureItemData) => x.n)
                    }).OrderBy(c => c.Code).ToArray()
                }).ToArray();


            this.PercentByDataPartner = $.Enumerable.From(table)
                .GroupBy((x: IProcedureItemData) => <any>{ Code: x.PX, CodeType: x.Px_Codetype },
                (x: IProcedureItemData) => x,
                (k, group) => <any>{
                    Code: k.Code,
                    CodeType: k.CodeType,
                    Total: $.Enumerable.From(total_n_byCode).Where(x => k.Code == x.Code && k.CodeType == x.CodeType).Select(x => x.Total).FirstOrDefault(0),
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{
                        Partner: dp,
                        Total: $.Enumerable.From(total_n_byPartner).Where(x => dp == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where((x: IProcedureItemData) => x.DP == dp && x.PX == k.Code && x.Px_Codetype == k.CodeType).Sum((x: IProcedureItemData) => x.n)
                    }).ToArray()
                }, (key) => { return key.Code.toString() + key.CodeType.toString() }).OrderBy(c => c.Code).ToArray();

            var index = 1;

            var chartContainer = $('#DataPartnerMetrics');
            var percentByDataPartnerContainer = $('#DataPartnerMetricsPercent');
            var percentByDataPartnerContainerPie = $('#DataPartnersMetricsPie');            
            countByCode.forEach((item: any) => {
                var id = 'procedures_' + index++;
                var d = $('<div>').attr('id', id).addClass(codes.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");                
                //$(d).width(Math.max($(d).width(), codes.length * 55));
                $(chartContainer).append(d);

                id = 'procedures_' + index++;
                var d2 = $('<div>').attr('id', id).addClass(codes.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                //$(d2).width(Math.max($(d2).width(), codes.length * 55));
                $(percentByDataPartnerContainer).append(d2);

                id = 'procedures_' + index++;
                var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                $(percentByDataPartnerContainerPie).append(p);

                var source = new ChartSource($.Enumerable.From(item.Codes).Select((x:any) => [x.Code + ' (' + x.CodeType + ') ', x.Count]).OrderBy((c:any) => c.Code).ToArray(), this.formatCodeType(model.CodeType) + ' Procedure Code Distribution within ' + item.Partner);
                source.xaxis_label = 'Procedure Code';
                source.yaxis_label = 'n';
                source.pointLabelFormatString = '%d';
                DataChecker.Charting.plotBarChart(d, source);

                var s2 = new ChartSource($.Enumerable.From(item.Codes).Select((x:any) => [x.Code + ' (' + x.CodeType + ') ', this.toPercent(x.Count, item.Total)]).OrderBy((c:any) => c.Code).ToArray(), this.formatCodeType(model.CodeType) + ' Procedure Code Distribution within ' + item.Partner);
                s2.xaxis_label = 'Procedure Code';
                s2.yaxis_label = '%';
                s2.pointLabelFormatString = '%.2f';
                s2.isPercentage = true;
                DataChecker.Charting.plotBarChart(d2, s2);

                var s3 = new ChartSource($.Enumerable.From(item.Codes).Select((x:any) => [x.Code + ' (' + x.CodeType + ') ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total) / 100]).ToArray(), this.formatCodeType(model.CodeType) + ' Procedure Code Distribution within ' + item.Partner);
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

                var source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner, this.toPercent(x.Count, item.Total)]).ToArray(), 'Data Partner Contribution to ' + this.formatCodeType(item.CodeType) + ' Procedure Code: ' + item.Code);
                source.xaxis_label = 'Data Partner';
                source.yaxis_label = '%';
                source.pointLabelFormatString = '%.2f';
                source.isPercentage = true;
                DataChecker.Charting.plotBarChart(d, source);

                source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner + ' ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total) / 100]).ToArray(), 'Data Partner Contribution to ' + this.formatCodeType(item.CodeType) + ' Procedure Code: ' + item.Code);
                DataChecker.Charting.plotPieChart(p, source);
            });


        }

        public formatCodeType(codeType: string) {
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

            return "<unknown>";
        }

        public toPercent(count: number, total: number) : number {
            return parseFloat((count / total * 100).toFixed(2));
        }

    }

    export function init(model: IProcedureResultsModelData, bindingControl: JQuery) {
        _bindingControl = bindingControl;
        vm = new ViewModel(model);
        ko.applyBindings(vm, bindingControl[0]);
    }

    export interface IProcedureResultsModelData extends IResultsModelData {
        CodeType: string;
    }
}