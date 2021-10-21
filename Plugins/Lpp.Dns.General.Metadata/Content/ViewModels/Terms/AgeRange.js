/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/AgeRange.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
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
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var AgeRangeTerm = /** @class */ (function (_super) {
        __extends(AgeRangeTerm, _super);
        function AgeRangeTerm(ageRangeData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.AgeRangeTerm) || this;
            _this.MinAge = ko.observable(ageRangeData ? ageRangeData.MinAge : 0);
            _this.MaxAge = ko.observable(ageRangeData ? ageRangeData.MaxAge : 0);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        AgeRangeTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var data = {
                TermType: superdata.TermType,
                MinAge: this.MinAge(),
                MaxAge: this.MaxAge()
            };
            return data;
        };
        return AgeRangeTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.AgeRangeTerm = AgeRangeTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
