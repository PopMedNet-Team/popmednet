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
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/ClinicalSetting.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var ClinicalSettingTerm = (function (_super) {
        __extends(ClinicalSettingTerm, _super);
        function ClinicalSettingTerm(clinicalSettingData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.ClinicalSettingTerm) || this;
            _this.ClinicalSetting = ko.observable(clinicalSettingData ? clinicalSettingData.ClinicalSetting : RequestCriteriaModels.ClinicalSettingTypes.NotSpecified);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        ClinicalSettingTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var clinicalSettingData = {
                TermType: superdata.TermType,
                ClinicalSetting: this.ClinicalSetting()
            };
            //console.log('Clinical Setting: ' + JSON.stringify(clinicalSettingData));
            return clinicalSettingData;
        };
        return ClinicalSettingTerm;
    }(RequestCriteriaViewModels.Term));
    ClinicalSettingTerm.ClinicalSettingsList = [
        new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.ClinicalSettingTypes.NotSpecified),
        new Dns.KeyValuePairData('Any', RequestCriteriaModels.ClinicalSettingTypes.Any),
        new Dns.KeyValuePairData('In-patient', RequestCriteriaModels.ClinicalSettingTypes.InPatient),
        new Dns.KeyValuePairData('Out-patient', RequestCriteriaModels.ClinicalSettingTypes.OutPatient),
        new Dns.KeyValuePairData('Emergency', RequestCriteriaModels.ClinicalSettingTypes.Emergency),
    ];
    RequestCriteriaViewModels.ClinicalSettingTerm = ClinicalSettingTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
