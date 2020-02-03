/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/PDX.ts" />
///// <reference path="../Terms.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
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
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
