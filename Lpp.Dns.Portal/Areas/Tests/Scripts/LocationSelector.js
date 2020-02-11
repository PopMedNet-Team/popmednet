/// <reference path="../../../scripts/common.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Tests;
(function (Tests) {
    var LocationSelector;
    (function (LocationSelector) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel() {
                var _this = _super.call(this, null) || this;
                _this.State = ko.observable();
                _this.Location = ko.observable();
                _this.Regions = ko.observable();
                _this.Towns = ko.observable();
                _this.CensusData = ko.observableArray();
                _this.State.subscribe(_this.UpdateTownsAndRegions);
                return _this;
            }
            ViewModel.prototype.LocationChanged = function (data, event) {
                vm.UpdateCensus();
            };
            ViewModel.prototype.UpdateTownsAndRegions = function (state) {
                if (state) {
                    $.getJSON("/api/demographics/GetRegionsAndTowns?country=us&state=" + encodeURIComponent(state)).done(function (results) {
                        vm.Regions(results.Regions);
                        vm.Towns(results.Towns);
                    }).fail(function (error) {
                    });
                    //Get the census data here
                    vm.UpdateCensus();
                }
                else {
                    vm.Regions([]);
                    vm.Towns([]);
                }
            };
            ViewModel.prototype.UpdateCensus = function () {
                if (vm.Location()) {
                    //Selected, go look it up, first determine if it's a region or a town.
                    var selected = $(":selected", $("#cboLocation"));
                    var optGroup = selected.closest("optgroup").attr("label");
                    switch (optGroup) {
                        case "Regions":
                            $.getJSON("/api/demographics/GetCensusDataByRegion?country=us&state=" + encodeURIComponent(vm.State()) + "&region=" + encodeURIComponent(this.Location()) + "&stratification=1").done(function (results) {
                            });
                            break;
                        default:
                            $.getJSON("/api/demographics/GetCensusDataByTown?country=us&state=" + encodeURIComponent(vm.State()) + "&town=" + encodeURIComponent(this.Location()) + "&stratification=1").done(function (results) {
                            });
                            break;
                    }
                }
                else if (vm.State()) {
                    $.getJSON("/api/demographics/GetCensusDataByState?country=us&state=" + encodeURIComponent(vm.State()) + "&stratification=1").done(function (results) {
                    });
                }
                else {
                    //Nothing selected, hide projection data.
                    vm.CensusData.removeAll();
                }
            };
            return ViewModel;
        }(Dns.PageViewModel));
        LocationSelector.ViewModel = ViewModel;
        function init() {
            $(function () {
                vm = new ViewModel();
                ko.applyBindings(vm, $("#container")[0]);
                vm.State("MA");
            });
        }
        init();
    })(LocationSelector = Tests.LocationSelector || (Tests.LocationSelector = {}));
})(Tests || (Tests = {}));
