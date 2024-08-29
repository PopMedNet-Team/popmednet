var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../models/datacheckermodels.ts" />
/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="requestcriteriaviewmodels.ts" />
/// <reference path="../../../../Lpp.Dns.Api/Scripts/Lpp.Dns.Interfaces.ts" />
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
    var MetaDataTableTerm = /** @class */ (function (_super) {
        __extends(MetaDataTableTerm, _super);
        function MetaDataTableTerm(tableData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.MetaDataTableTerm) || this;
            var dummy = [];
            _this.Tables = ko.observableArray(tableData ? tableData.Tables : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        MetaDataTableTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var pdxData = {
                TermType: superdata.TermType,
                Tables: this.Tables()
            };
            return pdxData;
        };
        return MetaDataTableTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.MetaDataTableTerm = MetaDataTableTerm;
    var MetricTerm = /** @class */ (function (_super) {
        __extends(MetricTerm, _super);
        function MetricTerm(metricData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.MetricTerm) || this;
            _this.MetricsTermType = ko.observable(metricData.MetricsTermType);
            _this.Metrics = ko.observableArray(metricData.Metrics);
            _this.MetricsList = ko.observableArray();
            switch (_this.MetricsTermType()) {
                case DataCheckerModels.MetricsTermTypes.Race:
                case DataCheckerModels.MetricsTermTypes.Ethnicity:
                    _this.MetricsList.push(new Dns.KeyValuePairData('Overall', DataCheckerModels.MetricsTypes.Overall));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Percent within Data Partner', DataCheckerModels.MetricsTypes.DataPartnerPercent));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Percent of Data Partner Contribution', DataCheckerModels.MetricsTypes.DataPartnerPercentContribution));
                    break;
                case DataCheckerModels.MetricsTermTypes.Diagnoses:
                case DataCheckerModels.MetricsTermTypes.Procedures:
                    _this.MetricsList.push(new Dns.KeyValuePairData('Overall Count', DataCheckerModels.MetricsTypes.OverallCount));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Count by Data Partner', DataCheckerModels.MetricsTypes.DataPartnerCount));
                    break;
                case DataCheckerModels.MetricsTermTypes.NDC:
                    _this.MetricsList.push(new Dns.KeyValuePairData('Overall Presence', DataCheckerModels.MetricsTypes.OverallPresence));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Presence by Data Partner', DataCheckerModels.MetricsTypes.DataPartnerPresence));
                    break;
            }
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        MetricTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var metricData = {
                TermType: superdata.TermType,
                Metrics: this.Metrics(),
                MetricsTermType: this.MetricsTermType(),
            };
            //console.log('Race: ' + JSON.stringify(metricData));
            return metricData;
        };
        return MetricTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.MetricTerm = MetricTerm;
    var PDXTerm = /** @class */ (function (_super) {
        __extends(PDXTerm, _super);
        function PDXTerm(pdxData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.PDXTerm) || this;
            var dummy = [];
            _this.PDXes = ko.observableArray(pdxData ? pdxData.PDXes : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        PDXTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var pdxData = {
                TermType: superdata.TermType,
                PDXes: this.PDXes()
            };
            return pdxData;
        };
        return PDXTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.PDXTerm = PDXTerm;
    var RaceTerm = /** @class */ (function (_super) {
        __extends(RaceTerm, _super);
        function RaceTerm(raceData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.RaceTerm) || this;
            var dummy = [];
            _this.Races = ko.observableArray(raceData ? raceData.Races : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RaceTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var raceData = {
                TermType: superdata.TermType,
                Races: this.Races()
            };
            return raceData;
        };
        RaceTerm.RacesList = [
            new Dns.KeyValuePairData('Unknown', DataCheckerModels.RaceTypes.Unknown),
            new Dns.KeyValuePairData('American Indian/Alaska Native', DataCheckerModels.RaceTypes.AmericanIndianOrAlaskaNative),
            new Dns.KeyValuePairData('Asian', DataCheckerModels.RaceTypes.Asian),
            new Dns.KeyValuePairData('Black/African American', DataCheckerModels.RaceTypes.BlackOrAfricanAmerican),
            new Dns.KeyValuePairData('Native Hawaiian/Pacific Islander', DataCheckerModels.RaceTypes.NativeHawaiianOrOtherPacificIslander),
            new Dns.KeyValuePairData('White', DataCheckerModels.RaceTypes.White),
            new Dns.KeyValuePairData('Missing', DataCheckerModels.RaceTypes.Missing)
        ];
        return RaceTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.RaceTerm = RaceTerm;
    var RxAmtTerm = /** @class */ (function (_super) {
        __extends(RxAmtTerm, _super);
        function RxAmtTerm(amtData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.RxAmtTerm) || this;
            var dummy = [];
            _this.RxAmounts = ko.observableArray(amtData ? amtData.RxAmounts : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RxAmtTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var encounterData = {
                TermType: superdata.TermType,
                RxAmounts: this.RxAmounts()
            };
            return encounterData;
        };
        return RxAmtTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.RxAmtTerm = RxAmtTerm;
    var RxSupTerm = /** @class */ (function (_super) {
        __extends(RxSupTerm, _super);
        function RxSupTerm(supData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.RxSupTerm) || this;
            var dummy = [];
            _this.RxSups = ko.observableArray(supData ? supData.RxSups : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RxSupTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var encounterData = {
                TermType: superdata.TermType,
                RxSups: this.RxSups()
            };
            return encounterData;
        };
        return RxSupTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.RxSupTerm = RxSupTerm;
    var EncounterTerm = /** @class */ (function (_super) {
        __extends(EncounterTerm, _super);
        function EncounterTerm(encounterData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.EncounterTypeTerm) || this;
            var dummy = [];
            _this.Encounters = ko.observableArray(encounterData ? encounterData.Encounters : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        EncounterTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var encounterData = {
                TermType: superdata.TermType,
                Encounters: this.Encounters()
            };
            return encounterData;
        };
        return EncounterTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.EncounterTerm = EncounterTerm;
    var EthnicityTerm = /** @class */ (function (_super) {
        __extends(EthnicityTerm, _super);
        function EthnicityTerm(ethnicityData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.EthnicityTerm) || this;
            var dummy = [];
            _this.Ethnicities = ko.observableArray(ethnicityData ? ethnicityData.Ethnicities : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        EthnicityTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var ethnicityData = {
                TermType: superdata.TermType,
                Ethnicities: this.Ethnicities()
            };
            //console.log('Race: ' + JSON.stringify(ethnicityData));
            return ethnicityData;
        };
        EthnicityTerm.EthnicitiesList = [
            new Dns.KeyValuePairData('Unknown', DataCheckerModels.EthnicityTypes.Unknown),
            new Dns.KeyValuePairData('Hispanic', DataCheckerModels.EthnicityTypes.Hispanic),
            new Dns.KeyValuePairData('Not Hispanic', DataCheckerModels.EthnicityTypes.NotHispanic),
            new Dns.KeyValuePairData('Missing', DataCheckerModels.EthnicityTypes.Missing)
        ];
        return EthnicityTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.EthnicityTerm = EthnicityTerm;
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
