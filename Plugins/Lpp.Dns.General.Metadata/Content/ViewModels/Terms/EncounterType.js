/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/encountertype.ts" />
///// <reference path="../Terms.ts" />
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
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
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
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
