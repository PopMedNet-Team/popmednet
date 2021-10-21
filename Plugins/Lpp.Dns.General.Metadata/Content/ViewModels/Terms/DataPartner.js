/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/DataPartner.ts" />
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
    var DataPartnerTerm = /** @class */ (function (_super) {
        __extends(DataPartnerTerm, _super);
        function DataPartnerTerm(dataPartnersData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.DataPartnerTerm) || this;
            var dummy = [];
            _this.DataPartners = ko.observableArray(dataPartnersData ? dataPartnersData.DataPartners : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        DataPartnerTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var dataPartnersData = {
                TermType: superdata.TermType,
                DataPartners: this.DataPartners()
            };
            //console.log('Data Partners: ' + JSON.stringify(dataPartnersData));
            return dataPartnersData;
        };
        return DataPartnerTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.DataPartnerTerm = DataPartnerTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
