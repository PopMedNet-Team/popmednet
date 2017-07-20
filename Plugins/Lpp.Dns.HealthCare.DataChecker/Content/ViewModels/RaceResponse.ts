/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
module DataChecker.Race {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public _model: IResultsModelData;
        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;

        public DataPartners: Array<string> = [];
        public OverallMetrics: Array<any> = [];
        public PercentWithinDataPartner: Array<any> = [];
        public PercentWithinDataPartnerFiltered: Array<any> = [];
        public Total_N_DataPartner: Array<any> = [];
        public Total_N_DataPartnerFiltered: Array<any> = [];
        public Total_N_Race: Array<any> = [];
        public HasResults: boolean = false;

        constructor(model: IResultsModelData) {
            this._model = model;
            
            this.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1), new ResponseMetricsItem("Percent Data Partner Contribution", 2)]);
            this.SelectedMetric = ko.observable(this.Metrics()[0]);

            var table: any = this._model.RawData.Table;

            var total_n: number = $.Enumerable.From(table).Sum((item: IRaceItemData) => item.Total);
            this.DataPartners = $.Enumerable.From(table).Distinct((x: IRaceItemData) => x.DP).Select((x: IRaceItemData) => x.DP).OrderBy(x => x).ToArray();

            var q = $.Enumerable.From(table).GroupBy((i: IRaceItemData) => i.RACE,
                (i: IRaceItemData) => i, (key: string, group) => <any>{
                    RaceID: key,
                    RaceTitle: this.determineRaceTitle(key),
                    Count: $.Enumerable.From(group.source).Sum((x: IRaceItemData) => x.Total)
                });

            this.OverallMetrics = $.Enumerable.From(q).Select((x: any) => <any>{ RaceID: x.RaceID, RaceTitle: x.RaceTitle, Count: x.Count, Percent: Math.round((x.Count / total_n) * 10000) / 100 }).ToArray();

            this.HasResults = this.OverallMetrics.length > 0;
            if (!this.HasResults) {
                return;
            }

            var chartSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select((x: any) => <any>[x.RaceTitle, x.Percent]).ToArray());
            chartSource.title = 'Race Distribution among Selected Data Partners*';
            
            chartSource.setXAxisLabelRotation(true);
            chartSource.xaxis_label = 'Race';
            chartSource.yaxis_label = '%';
            chartSource.isPercentage = true;
            DataChecker.Charting.plotBarChart($('#OverallMetricsChart'), chartSource);

            chartSource = new ChartSource($.Enumerable.From(this.OverallMetrics).Select((x: any) => <any>[x.RaceTitle + ' ' + x.Percent + '%', x.Percent]).ToArray());
            chartSource.title = 'Race Distribution among Selected Data Partners*';

            DataChecker.Charting.plotPieChart($('#OverallMetricsPie'), chartSource);

            var index = 1;
            this.Total_N_DataPartner = $.Enumerable.From(table).GroupBy((x: IRaceItemData) => x.DP, (x: IRaceItemData) => x, (key, group) => <any>{ DataPartner: key, Total: $.Enumerable.From(group.source).Sum((x: IRaceItemData) => x.Total) }).ToArray();

            var raceIdentifiers = $.Enumerable.From(table).Distinct((x: IRaceItemData) => x.RACE).Select((x: IRaceItemData) => x.RACE).ToArray();
            var countByRace = $.Enumerable.From(table)
                .GroupBy((x: IRaceItemData) => x.DP, (x: IRaceItemData) => x,
                (key, group) => <any>{
                    Partner: key,
                    Total: $.Enumerable.From(this.Total_N_DataPartner).Where(x => key == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                    Races: $.Enumerable.From(raceIdentifiers).Select(r => <any>{
                        RaceID: r,
                        RaceTitle: this.determineRaceTitle(r.toString()),
                        Count: $.Enumerable.From(group.source).Where((x: IRaceItemData) => x.DP == key && x.RACE == r).Sum((x: IRaceItemData) => x.Total)
                    }).ToArray()
                }).ToArray();


            this.PercentWithinDataPartner = $.Enumerable.From(table)
                .GroupBy((x: IRaceItemData) => x.RACE,
                (x: IRaceItemData) => x,
                (k, group) => <any>{
                    RaceID: k,
                    RaceTitle: this.determineRaceTitle(k),
                    Total: $.Enumerable.From(this.Total_N_Race).Where(x => k == x.RaceID).Select(x => x.Total).FirstOrDefault(0),
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{
                        Partner: dp,
                        Total: $.Enumerable.From(this.Total_N_DataPartner).Where(x => dp == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where((x: IRaceItemData) => x.DP == dp && x.RACE == k).Sum((x: IRaceItemData) => x.Total)
                    }).ToArray()
                }).ToArray();


            this.Total_N_DataPartnerFiltered = $.Enumerable.From(table).Where((x: IRaceItemData) => x.RACE >= 0).GroupBy((x: IRaceItemData) => x.DP, (x: IRaceItemData) => x, (key, group) => <any>{ DataPartner: key, Total: $.Enumerable.From(group.source).Sum((x: IRaceItemData) => x.Total) }).ToArray();
            this.Total_N_Race = $.Enumerable.From(table).Where((x: IRaceItemData) => x.RACE >= 0).GroupBy((x: IRaceItemData) => x.RACE, (x: IRaceItemData) => x, (key, group) => <any>{ RaceID: key, Total: $.Enumerable.From(group.source).Sum((x: IRaceItemData) => x.Total) }).ToArray();

            this.PercentWithinDataPartnerFiltered = $.Enumerable.From(table)
                .Where((x: IRaceItemData) => x.RACE >= 0)
                .GroupBy((x: IRaceItemData) => x.RACE,
                (x: IRaceItemData) => x,
                (k, group) => <any>{
                    RaceID: k,
                    RaceTitle: this.determineRaceTitle(k),
                    Total: $.Enumerable.From(this.Total_N_Race).Where(x => k == x.RaceID).Select(x => x.Total).FirstOrDefault(0),
                    Partners: $.Enumerable.From(this.DataPartners).Select(dp => <any>{
                        Partner: dp,
                        Total: $.Enumerable.From(this.Total_N_DataPartnerFiltered).Where(x => dp == x.DataPartner).Select(x => x.Total).FirstOrDefault(0),
                        Count: $.Enumerable.From(group.source).Where((x: IRaceItemData) => x.DP == dp && x.RACE == k).Sum((x: IRaceItemData) => x.Total)
                    }).ToArray()
                }).ToArray();


            var percentByDataPartnerContainer = $('#PercentWithinDataPartners');
            var percentByDataPartnerContainerPie = $('#PercentWithinDataPartners_Pie');
            countByRace.forEach((item: any) => {
                var id = 'dp_percent_' + index++;
                var d = $('<div class="halfwidth-barchart-dpp">').attr('id', id);                
                $(d).width(Math.max(raceIdentifiers.length * 44, 450));

                $(percentByDataPartnerContainer).append(d);

                id = 'dp_percent_' + index++;
                var p = $('<div class="halfwidth-piechart-dpp">').attr('id', id);
                $(p).height($(p).width() * 0.80);
                $(percentByDataPartnerContainerPie).append(p);

                var source = new ChartSource($.Enumerable.From(item.Races).Select((x: any) => [x.RaceTitle, this.toPercent(x.Count, item.Total)]).ToArray(),'Race Distribution within ' + item.Partner);
                source.setXAxisLabelRotation(true);
                source.xaxis_label = 'Race';
                source.yaxis_label = '%';
                source.isPercentage = true;
                DataChecker.Charting.plotBarChart(d, source);

                source = new ChartSource($.Enumerable.From(item.Races).Select((x: any) => [x.RaceTitle + ' ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total)]).ToArray(), 'Race Distribution within '+ item.Partner);
                var plot = DataChecker.Charting.plotPieChart(p, source);
                
                $("#ResponseContainer").resize(() => {                    
                    plot.replot({ resetAxes:true });
                });
            });

            var contributionContainer = $('#PercentDataPartnerContribution');
            var contributionContainerPie = $('#PercentDataPartnerContribution_Pie');

            this.PercentWithinDataPartnerFiltered.forEach((item: any) => {
                var id = 'contrib_percent_' + index++;
                var d = $('<div>').attr('id', id).addClass(this.DataPartners.length > 11 ? "fullwidth-barchart-dpc" : "halfwidth-barchart-dpc");
                $(d).width(Math.max(this.DataPartners.length * 44, 450));

                $(contributionContainer).append(d);

                id = 'contrib_percent_' + index++;
                var p = $('<div>').attr('id', id).addClass(this.DataPartners.length > 11 ? "fullwidth-piechart-dpc" : "halfwidth-piechart-dpc");
                $(contributionContainerPie).append(p);

                var source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner, this.toPercent(x.Count, item.Total)]).ToArray(),'Data Partner Contribution to Race:  ' + item.RaceTitle);
                source.xaxis_label = 'Data Partner';
                source.yaxis_label = '%';
                source.isPercentage = true;
                DataChecker.Charting.plotBarChart(d, source);

                source = new ChartSource($.Enumerable.From(item.Partners).Select((x:any) => [x.Partner + ' ' + this.toPercent(x.Count, item.Total) + '%', this.toPercent(x.Count, item.Total)]).ToArray(), 'Data Partner Contribution to Race: ' + item.RaceTitle);
                DataChecker.Charting.plotPieChart(p, source);
            });

        }

        public determineRaceTitle(raceID: string): string {
            switch (raceID) {
                case "-1": return "Other";
                case "0": return "Unknown";
                case "1": return "American Indian or Alaska Native";
                case "2": return "Asian";
                case "3": return "Black or African American";
                case "4": return "Native Hawaiian or Other Pacific Islander";
                case "5": return "White";
                case "6": return "Missing";
            }

            return "missing definition: " + raceID;
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