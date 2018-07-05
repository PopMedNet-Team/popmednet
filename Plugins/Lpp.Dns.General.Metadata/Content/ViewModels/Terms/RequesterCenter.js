/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/RequesterCenter.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
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
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var RequesterCenterTerm = /** @class */ (function (_super) {
        __extends(RequesterCenterTerm, _super);
        function RequesterCenterTerm(requesterCenterData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.RequesterCenterTerm) || this;
            _this.RequesterCenter = ko.observable(requesterCenterData == undefined ? "00000000-0000-0000-0000-000000000000" : requesterCenterData.RequesterCenterID);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RequesterCenterTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var requesterCenterData = {
                TermType: superdata.TermType,
                RequesterCenterID: this.RequesterCenter()
            };
            return requesterCenterData;
        };
        return RequesterCenterTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.RequesterCenterTerm = RequesterCenterTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
