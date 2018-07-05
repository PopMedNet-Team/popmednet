/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/rxamt.ts" />
///// <reference path="../Terms.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
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
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
