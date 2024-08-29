/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../responses.common.ts" />
module DataChecker.DiagnosesPDX {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public _model: IResultsModelData;
        public _documentID: number;
        public EncounterTypes: IEncounterType[];
        public DataPartners: string[];
        public OverallMetrics: IPDXGroupingData[]; 
        public PercentWithinDataPartner: IDataPartnerGroupingData[];
        public OverallTicks: string[];
        public OverallSeriesLabels: any;
        public JQCharts: any[];
        public SelectedMetricChangedSubscription: any;
        public HasResults: boolean;
       

        constructor(model: IResultsModelData, result: IPDXResultsData) {
            this._model = model;

            this._documentID = (<any>this._model).ResponseDocumentIDs[0];

            this.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Percent within Data Partner", 1)]);
            this.SelectedMetric = ko.observable(this.Metrics()[0]);

            this.DataPartners = result.DataPartners;
            this.EncounterTypes = result.EncounterTypes;
            this.OverallMetrics = result.OverallMetrics;
            this.PercentWithinDataPartner = result.PercentWithinDataPartner;
            this.HasResults = (this.DataPartners.length > 0 && this.OverallMetrics.length > 0 && this.PercentWithinDataPartner.length > 0);

            this.OverallTicks = $.Enumerable.From(this.EncounterTypes).Select((x: IEncounterType) => x.EncType_Display).ToArray();
            this.OverallSeriesLabels = $.Enumerable.From(this.OverallMetrics).Select((x: IPDXGroupingData) => { return { 'label': x.PDX_Display }; }).ToArray(); 

            this.JQCharts = [];
            this.SelectedMetricChangedSubscription = this.SelectedMetric.subscribe((value: ResponseMetricsItem) => {
                //html canvas will not render if the container is not visible by display:none
                //since the binding of the charts is done at the same time the visibility of the metrics container is bound the charts are not actually rendered.
                //The subscription event fires just before the actual visibility happens, so need to put a short timeout to wait before trying to replot the graph.
                //Only needs to be done once, after the first time it does not need to be replotted unless the data has changed.
                if (value.value == 1) {
                    this.SelectedMetricChangedSubscription.dispose();
                    setTimeout(() => {
                        vm.JQCharts.forEach(chart => {
                            if (chart._drawCount === 0) {
                                Charting.replot(chart);
                            }
                        });
                    }, 100);                    

                }
            });            
        }

        public toPercent(count: number, total: number): number {
            return parseFloat((count / total * 100).toFixed(2));
        }
    }

    export function init(model: IResultsModelData, bindingControl: JQuery) {
        var documentID = (<any>model).ResponseDocumentIDs[0];
        $.get('/DataChecker/DiagnosisPDX/ProcessMetrics?documentID=' + documentID).done((result: IPDXResultsData) => {
            _bindingControl = bindingControl;
            vm = new ViewModel(model, result);
            ko.applyBindings(vm, bindingControl[0]);
        }).fail(error => {
            alert(error);
        });
    }

    export interface IPDXResultsData {
        DataPartners: string[];
        EncounterTypes: IEncounterType[]
        OverallMetrics: IPDXGroupingData[];
        PercentWithinDataPartner: IDataPartnerGroupingData[];
    }

    export interface IEncounterType {
        EncType: string;
        EncType_Display: string;
    }

    export interface IPDXType {
        PDX: string;
        PDX_Display: string;
    }

    export interface IEncounterData {
        EncType: string;
        EncType_Display: string;
        Total: number;
        Count: number;
    }

    export interface IPDXGroupingData {
        PDX: string;
        PDX_Display: string;
        Encounters: IEncounterData[];
    }

    export interface IDataPartnerGroupingData {
        DP: string;
        PDX: IPDXGroupingData[];
    }    

    ko.bindingHandlers.pdxPercentChart = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var source = valueAccessor();           

            var overallSeriesData = [];
            vm.EncounterTypes.forEach((encounterType: IEncounterType, i: number) => {
                var v = $.Enumerable.From(source.data).SelectMany((x: IPDXGroupingData) => $.Enumerable.From(x.Encounters).Where((ec: IEncounterData) => ec.EncType == encounterType.EncType).Select((ec: IEncounterData) => vm.toPercent(ec.Count, ec.Total))).ToArray();
                v.forEach((x: number, k: number) => {
                    if (typeof overallSeriesData[k] == 'undefined')
                        overallSeriesData[k] = [];

                    overallSeriesData[k].push(x);
                });
            });

            if (overallSeriesData.length > 0) {
                var overallPercentBarSrc = new ChartSource(overallSeriesData);
                overallPercentBarSrc.isPercentage = true;
                overallPercentBarSrc.yaxis_label = '%';
                overallPercentBarSrc.xaxis_label = 'Encounter Type';
                overallPercentBarSrc.title = source.title;
                overallPercentBarSrc.pointLabelFormatString = '%.2f';
                overallPercentBarSrc.multiSeriesData = { series_labels: vm.OverallSeriesLabels, ticks: vm.OverallTicks };

                var chart = DataChecker.Charting.plotBarChart($(element), overallPercentBarSrc);
                vm.JQCharts.push(chart);
            }

        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        }
    };    
} 

interface KnockoutBindingHandlers {
    pdxPercentChart: {};
}