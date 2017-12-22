/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Race.ts" />
///// <reference path="../Terms.ts" />
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
    var RaceTerm = (function (_super) {
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
        return RaceTerm;
    }(RequestCriteriaViewModels.Term));
    RaceTerm.RacesList = [
        new Dns.KeyValuePairData('Unknown', DataCheckerModels.RaceTypes.Unknown),
        new Dns.KeyValuePairData('American Indian/Alaska Native', DataCheckerModels.RaceTypes.AmericanIndianOrAlaskaNative),
        new Dns.KeyValuePairData('Asian', DataCheckerModels.RaceTypes.Asian),
        new Dns.KeyValuePairData('Black/African American', DataCheckerModels.RaceTypes.BlackOrAfricanAmerican),
        new Dns.KeyValuePairData('Native Hawaiian/Pacific Islander', DataCheckerModels.RaceTypes.NativeHawaiianOrOtherPacificIslander),
        new Dns.KeyValuePairData('White', DataCheckerModels.RaceTypes.White),
        new Dns.KeyValuePairData('Missing', DataCheckerModels.RaceTypes.Missing)
    ];
    DataCheckerViewModels.RaceTerm = RaceTerm;
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
