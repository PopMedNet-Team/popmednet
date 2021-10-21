/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />
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
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Sex.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var SexTerm = /** @class */ (function (_super) {
        __extends(SexTerm, _super);
        function SexTerm(sexData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.SexTerm) || this;
            _this.Sex = ko.observable(sexData ? sexData.Sex : RequestCriteriaModels.SexTypes.NotSpecified);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        SexTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var sexData = {
                TermType: superdata.TermType,
                Sex: this.Sex()
            };
            //console.log('Sex: ' + JSON.stringify(sexData));
            return sexData;
        };
        SexTerm.SexesList = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.SexTypes.NotSpecified),
            new Dns.KeyValuePairData('Male', RequestCriteriaModels.SexTypes.Male),
            new Dns.KeyValuePairData('Female', RequestCriteriaModels.SexTypes.Female),
            new Dns.KeyValuePairData('Both', RequestCriteriaModels.SexTypes.Both),
            new Dns.KeyValuePairData('Both Aggregated', RequestCriteriaModels.SexTypes.Aggregated)
        ];
        return SexTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.SexTerm = SexTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
