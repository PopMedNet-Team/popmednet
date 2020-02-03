/// <reference path="../../../scripts/common.ts" />


module Tests.LocationSelector {
    var vm: ViewModel;

    export class ViewModel extends Dns.PageViewModel {
        public Regions: KnockoutObservable<string[]>;
        public Towns: KnockoutObservable<string[]>;
        public CensusData: KnockoutObservableArray<any>;

        public Location: KnockoutObservable<string>;
        public State: KnockoutObservable<string>;



        constructor() { 
            super(null);

            this.State = ko.observable<string>();
            this.Location = ko.observable<string>();

            this.Regions = ko.observable<string[]>();
            this.Towns = ko.observable<string[]>();
            this.CensusData = ko.observableArray<any>();

            this.State.subscribe(this.UpdateTownsAndRegions);            
        }

        public LocationChanged(data: string, event: JQueryEventObject) {
            vm.UpdateCensus();
        }

        private UpdateTownsAndRegions(state: string) {
            if (state) {
                $.getJSON("/api/demographics/GetRegionsAndTowns?country=us&state=" + encodeURIComponent(state)).done((results) => {
                    vm.Regions(results.Regions);
                    vm.Towns(results.Towns);
                }).fail((error) => {
                    debugger;
                    });

                //Get the census data here
                vm.UpdateCensus();
            } else {
                vm.Regions([]);
                vm.Towns([]);
            }
        }

        private UpdateCensus() {
            if (vm.Location()) {
                //Selected, go look it up, first determine if it's a region or a town.
                var selected = $(":selected", $("#cboLocation"));
                var optGroup = selected.closest("optgroup").attr("label");
                switch (optGroup) {
                    case "Regions":
                        $.getJSON("/api/demographics/GetCensusDataByRegion?country=us&state=" + encodeURIComponent(vm.State()) + "&region=" + encodeURIComponent(this.Location()) + "&stratification=1").done((results) => {
                            debugger;
                        });
                        break;
                    default:
                        $.getJSON("/api/demographics/GetCensusDataByTown?country=us&state=" + encodeURIComponent(vm.State()) + "&town=" + encodeURIComponent(this.Location()) + "&stratification=1").done((results) => {
                            debugger;
                        });
                        break;
                }
            } else if (vm.State()) {
                $.getJSON("/api/demographics/GetCensusDataByState?country=us&state=" + encodeURIComponent(vm.State()) + "&stratification=1").done((results) => {
                    debugger;
                });
            } else {
                //Nothing selected, hide projection data.
                vm.CensusData.removeAll();
            }

        }
    }

    function init() {
        $(() => {
            vm = new ViewModel();

            ko.applyBindings(vm, $("#container")[0]);

            vm.State("MA");
        });
    }

    init();
} 