/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
module DataChecker.Ethnicity {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public _model: IResultsModelData;
        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;

        public DataPartners: Array<string> = [];
        public OverallMetrics: Array<any> = [];
        public Total_N_DataPartner: Array<any> = [];
        public Total_N_DataPartnerFiltered: Array<any> = [];
        public Total_N_Ethnicity: Array<any> = [];
        public PercentWithinDataPartner: Array<any> = [];
        public PercentWithinDataPartnerFiltered: Array<any> = [];
        public Label: KnockoutObservable<string>;
        public HasResults: boolean = false;

        constructor(model: IResultsModelData) {
            this._model = model;

            this.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Percent Data Partner Contribution", 2)]);
            this.SelectedMetric = ko.observable(this.Metrics()[0]);

            var table: any = this._model.RawData.Table;

            var total_n: number = $.Enumerable.From(table).Sum((item: IEthnicityItemData) => item.Total);
            this.DataPartners = $.Enumerable.From(table).Distinct((x: IEthnicityItemData) => x.DP).Select((x: IEthnicityItemData) => x.DP).OrderBy(x => x).ToArray();
            this.Label = ko.observable('*Selected data partners include: ' + this.DataPartners.toString());

            var q = $.Enumerable.From(table).GroupBy((i: IEthnicityItemData) => i.HISPANIC,
                (i: IEthnicityItemData) => i, (key: string, group) => <any>{
                    EthnicityID: key,
                    Ethnicity: this.formatEthnicity(key),
                    Count: $.Enumerable.From(group.source).Sum((x: IEthnicityItemData) => x.Total)
                }).OrderBy(c => c.Ethnicity);

            this.OverallMetrics = $.Enumerable.From(q).Select((x: any) => <any>{ EthnicityID: x.EthnicityID, Ethnicity: x.Ethnicity, Count: x.Count, Percent: Math.round((x.Count / total_n) * 10000) / 100 }).ToArray();

            this.HasResults = this.OverallMetrics.length > 0;
            if (!this.HasResults) {
                return;
            }

            var chartSource = new ChartSource($.Enumerable.From(this.OverallMetrics).OrderBy(x => x.Ethnicity).Select((x: any) => <any>[x.Ethnicity, x.Percent]).ToArray());
            chartSource.title = 'Ethnicity Distribution Among Selected Data Partners*';
            chartSource.setXAxisLabelRotation(true);
            chartSource.xaxis_label = 'Ethnicity';
            chartSource.yaxis_label = '%';
            chartSource.pointLabelFormatString = '%.2f';
            chartSource.isPercentage = true;
            DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), chartSource);

            chartSource = new ChartSource($.Enumerable.From(this.OverallMetrics).OrderBy(x => x.Ethnicity).Select((x: any) => <any>[x.Ethnicity + ' ' + x.Percent + '%', x.Percent]).ToArray());
            chartSource.title = 'Ethnicity Distribution Among Selected Data Partners*';
            DataChecker.Charting.plotPieChart($('#OverallMetricsPie'), chartSource);

            this.Total_N_DataPartner = $.Enumerable.From(table).GroupBy((x: IEthnicityItemData) => x.DP, (x: IEthnicityItemData) => x, (key, group) => <any>{ DataPartner: key, Total: $.Enumerable.From(group.source).Sum((x: IEthnicityItemData) => x.Total) }).ToArray();
            
            var index = 1;

            //foreach data partner, percent per ethnicity in the result set
            var ethnicities = $.Enumerable.From(table).Distinct((x: IEthnicityItemData) => x.HISPANIC).Select(x => x.HISPANIC).ToArray();

            var countByEthnicity = $.Enumerable.From(table)
                .GroupBy((x: IEthnicityItemData) => x.DP, (x: IEthnicityItemData) => x,
                (key, group) => <any>{
                    Partner: key,
                    Total: $.Enumerable.From(this.Total_N_DataPartner).Where(x => key == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                    Ethnicities: $.Enumerable.From(ethnicities).Select(e => <any>{
                        EthnicityID: e,
                        Ethnicity: this.formatEthnicity(e),
                        Count: $.Enumerable.From(group.source).Where((x: IEthnicityItemData) => x.DP == key && x.HISPANIC == e).Sum((x: IEthnicityItemData) => x.Total)
                    }).ToArray()
                }).ToArray();

            this.PercentWithinDataPartner = $.Enumerable.From(table)
                .GroupBy((x: IEthnicityItemData) => x.HISPANIC,
                (x: IEthnicityItemData) => x,
                (k, group) => <any>{
                    EthnicityID: k,
                    Ethnicity: this.formatEthnicity(k),
                    Total: $.Enumerable.From(this.Total_N_Ethnicity).Where(x => k == x.EthnicityID).Select(x => x.Total).FirstOrDefault(0),
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{
                        Partner: dp,
                        Total: $.Enumerable.From(this.Total_N_DataPartner).Where(x => dp == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where((x: IEthnicityItemData) => x.DP == dp && x.HISPANIC == k).Sum((x: IEthnicityItemData) => x.Total)
                    }).ToArray()
                }).OrderBy(c => c.Ethnicity).ToArray();

            //filter out the "Other" items so that the numbers are based on the requests criteria
            this.Total_N_Ethnicity = $.Enumerable.From(table).Where((x: IEthnicityItemData) => x.HISPANIC != "OTHER").GroupBy((x: IEthnicityItemData) => x.HISPANIC, (x: IEthnicityItemData) => x, (key, group) => <any>{ EthnicityID: key, Total: $.Enumerable.From(group.source).Sum((x: IEthnicityItemData) => x.Total) }).ToArray();
            this.Total_N_DataPartnerFiltered = $.Enumerable.From(table).Where((x: IEthnicityItemData) => x.HISPANIC != "OTHER").GroupBy((x: IEthnicityItemData) => x.DP, (x: IEthnicityItemData) => x, (key, group) => <any>{ DataPartner: key, Total: $.Enumerable.From(group.source).Sum((x: IEthnicityItemData) => x.Total) }).ToArray();
            this.PercentWithinDataPartnerFiltered = $.Enumerable.From(table).Where((x: IEthnicityItemData) => x.HISPANIC != "OTHER")
                .GroupBy((x: IEthnicityItemData) => x.HISPANIC,
                (x: IEthnicityItemData) => x,
                (k, group) => <any>{
                    EthnicityID: k,
                    Ethnicity: this.formatEthnicity(k),
                    Total: $.Enumerable.From(this.Total_N_Ethnicity).Where(x => k == x.EthnicityID).Select(x => x.Total).FirstOrDefault(0),
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{
                        Partner: dp,
                        Total: $.Enumerable.From(this.Total_N_DataPartnerFiltered).Where(x => dp == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where((x: IEthnicityItemData) => x.DP == dp && x.HISPANIC == k).Sum((x: IEthnicityItemData) => x.Total)
                    }).ToArray()
                }).OrderBy(c => c.Ethnicity).ToArray();

            var percentByDataPartnerContainer = $('#PercentWithinDataPartners');
            var percentByDataPartnerContainerPie = $('#PercentWithinDataPartners_Pie');
            countByEthnicity.forEach((item: any) => {
                var id = 'dp_percent_' + index++;
                var d = $('<div class="halfwidth-barchart-dpc">').attr('id', id);
                $(percentByDataPartnerContainer).append(d);
                $(d).width(Math.max(ethnicities.length * 80, 450));

                id = 'dp_percent_' + index++;
                var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                $(percentByDataPartnerContainerPie).append(p);

                var source = new ChartSource($.Enumerable.From(item.Ethnicities).OrderBy((c:any) => c.Ethnicity).Select((x:any) => [x.Ethnicity, this.toPercent(x.Count, item.Total)]).ToArray(), 'Ethnicity Distribution within ' + item.Partner);
                source.xaxis_label = 'Ethnicity';
                source.yaxis_label = '%';
                source.pointLabelFormatString = '%.2f';
                source.isPercentage = true;
                DataChecker.Charting.plotBarChart(d, source);


                source = new ChartSource($.Enumerable.From(item.Ethnicities).OrderBy((c:any) => c.Ethnicity).Select((x:any) => [x.Ethnicity + ' ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total)]).ToArray(), 'Ethnicity Distribution within ' + item.Partner);
                DataChecker.Charting.plotPieChart(p, source);
            });
            
            var contributionContainer = $('#PercentDataPartnerContribution');
            var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');
            this.PercentWithinDataPartnerFiltered.forEach((item: any) => {
                var id = 'contrib_percent_' + index++;
                var d = $('<div>').attr('id', id).addClass(this.DataPartners.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");                
                $(d).width(Math.max((this.DataPartners.length * 80), 450));
                
                $(contributionContainer).append(d);

                id = 'contrib_percent_' + index++;
                var p = $('<div>').attr('id', id).addClass(this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                $(contributionContainerPie).append(p);


                var source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner, this.toPercent(x.Count, item.Total)]).ToArray(), 'Data Partner Contribution to Ethnicity: ' + item.Ethnicity);
                source.xaxis_label = 'Data Partner';
                source.yaxis_label = '%';
                source.pointLabelFormatString = '%.2f';
                source.isPercentage = true;
                DataChecker.Charting.plotBarChart(d, source);

                source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner + ' ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total)]).ToArray(), 'Data Partner Contribution to Ethnicity: ' + item.Ethnicity);
                DataChecker.Charting.plotPieChart(p, source);
            });
        }

        public formatEthnicity(id: string) {
            switch (id.toUpperCase()) {
                case "N": return "Not Hispanic";
                case "U": return "Unknown";
                case "Y": return "Hispanic";
                case "OTHER": return "Other";
                case "MISSING": return "Missing";
            }

            return "<unknown>";
        }

        public toPercent(count: number, total: number): number {
            return Math.round((count / total) * 10000) / 100;
        }
    }

    export function init(model: IResultsModelData, bindingControl: JQuery) {
        _bindingControl = bindingControl;
        vm = new ViewModel(model);
        ko.applyBindings(vm, bindingControl[0]);
    }
}