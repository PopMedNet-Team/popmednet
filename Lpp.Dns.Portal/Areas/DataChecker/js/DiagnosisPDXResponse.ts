/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.DiagnosesPDX {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public Metrics: KnockoutObservableArray<ResponseMetricsItem>;
        public SelectedMetric: KnockoutObservable<ResponseMetricsItem>;
        public EncounterTypes: IEncounterType[];
        public DataPartners: string[];
        public OverallMetrics: IPDXGroupingData[];
        public PercentWithinDataPartner: IDataPartnerGroupingData[];
        public OverallTicks: string[];
        public OverallSeriesLabels: any;
        public JQCharts: any[] = [];
        public SelectedMetricChangedSubscription: any;
        public HasResults: boolean;

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

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/DiagnosisPDX/ProcessMetricsByResponse?responseID=' + self.responseID().toString())
                ).then((result: IPDXResultsData) => {

                self.Metrics = ko.observableArray([new ResponseMetricsItem("Overall Metrics", 0), new ResponseMetricsItem("Count by DataPartner", 1)]);
                self.SelectedMetric = ko.observable(self.Metrics()[0]);

                self.DataPartners = result.DataPartners;
                self.EncounterTypes = result.EncounterTypes;
                self.OverallMetrics = result.OverallMetrics;
                self.PercentWithinDataPartner = result.PercentWithinDataPartner;
                self.HasResults = (self.DataPartners.length > 0 && self.OverallMetrics.length > 0 && self.PercentWithinDataPartner.length > 0);

                self.OverallTicks = $.Enumerable.From(self.EncounterTypes).Select((x: IEncounterType) => x.EncType_Display).ToArray();
                self.OverallSeriesLabels = $.Enumerable.From(self.OverallMetrics).Select((x: IPDXGroupingData) => { return { 'label': x.PDX_Display }; }).ToArray();

                self.SelectedMetricChangedSubscription = self.SelectedMetric.subscribe((value: ResponseMetricsItem) => {
                    setTimeout(() => {
                        self.JQCharts.forEach(chart => {
                            if (chart._drawCount === 0) {
                                Charting.replot(chart);
                            }
                        });
                    }, 100);
                });

                self.isLoaded(true);

                //resize the iframe to the contents plus padding for the export dropdown menu
                $(window.frameElement).height($('html').height() + 70);

            }).fail((error) => {
                alert(error);
                return;
            });
        }
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
            source.encounterTypes.forEach((encounterType: IEncounterType, i: number) => {
                var v = $.Enumerable.From(source.data).SelectMany((x: IPDXGroupingData) => $.Enumerable.From(x.Encounters).Where((ec: IEncounterData) => ec.EncType == encounterType.EncType).Select((ec: IEncounterData) => Global.Helpers.ToPercent(ec.Count, ec.Total))).ToArray();
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
                overallPercentBarSrc.multiSeriesData = { series_labels: source.overallSeriesLabels, ticks: source.overallTicks };

                var chart = DataChecker.Charting.plotBarChart($(element), overallPercentBarSrc);
                source.jqCharts.push(chart);
            }

        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        }
    };
}

interface KnockoutBindingHandlers {
    pdxPercentChart: {};
}