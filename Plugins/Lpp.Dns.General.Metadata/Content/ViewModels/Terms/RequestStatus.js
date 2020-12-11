/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/RequestStatus.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var RequestStatusTerm = /** @class */ (function (_super) {
        __extends(RequestStatusTerm, _super);
        function RequestStatusTerm(requestStatusData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.RequestStatusTerm) || this;
            _this.RequestStatus = ko.observable(requestStatusData ? requestStatusData.RequestStatus : null);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RequestStatusTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var requestStatusData = {
                TermType: superdata.TermType,
                RequestStatus: this.RequestStatus()
            };
            //console.log('Request Status: ' + JSON.stringify(requestStatusData));
            return requestStatusData;
        };
        return RequestStatusTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.RequestStatusTerm = RequestStatusTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
