/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/rxamt.ts" />
///// <reference path="../Terms.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
    var RxSupTerm = (function (_super) {
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
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
