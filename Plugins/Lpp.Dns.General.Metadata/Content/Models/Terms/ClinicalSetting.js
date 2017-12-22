/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />
var RequestCriteriaModels;
(function (RequestCriteriaModels) {
    var ClinicalSettingTypes;
    (function (ClinicalSettingTypes) {
        ClinicalSettingTypes[ClinicalSettingTypes["NotSpecified"] = -1] = "NotSpecified";
        ClinicalSettingTypes[ClinicalSettingTypes["Any"] = 0] = "Any";
        ClinicalSettingTypes[ClinicalSettingTypes["InPatient"] = 1] = "InPatient";
        ClinicalSettingTypes[ClinicalSettingTypes["OutPatient"] = 2] = "OutPatient";
        ClinicalSettingTypes[ClinicalSettingTypes["Emergency"] = 3] = "Emergency";
    })(ClinicalSettingTypes = RequestCriteriaModels.ClinicalSettingTypes || (RequestCriteriaModels.ClinicalSettingTypes = {}));
})(RequestCriteriaModels || (RequestCriteriaModels = {}));
