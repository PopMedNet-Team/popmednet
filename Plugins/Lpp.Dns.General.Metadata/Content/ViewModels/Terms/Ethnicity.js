/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />
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
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Ethnicity.ts" />
///// <reference path="../Terms.ts" />
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
    var EthnicityTerm = /** @class */ (function (_super) {
        __extends(EthnicityTerm, _super);
        function EthnicityTerm(ethnicityData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.EthnicityTerm) || this;
            var dummy = [];
            _this.Ethnicities = ko.observableArray(ethnicityData ? ethnicityData.Ethnicities : dummy);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        EthnicityTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var ethnicityData = {
                TermType: superdata.TermType,
                Ethnicities: this.Ethnicities()
            };
            //console.log('Race: ' + JSON.stringify(ethnicityData));
            return ethnicityData;
        };
        EthnicityTerm.EthnicitiesList = [
            new Dns.KeyValuePairData('Unknown', DataCheckerModels.EthnicityTypes.Unknown),
            new Dns.KeyValuePairData('Hispanic', DataCheckerModels.EthnicityTypes.Hispanic),
            new Dns.KeyValuePairData('Not Hispanic', DataCheckerModels.EthnicityTypes.NotHispanic),
            new Dns.KeyValuePairData('Missing', DataCheckerModels.EthnicityTypes.Missing)
        ];
        return EthnicityTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.EthnicityTerm = EthnicityTerm;
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
