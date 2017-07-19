/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />
var DataCheckerModels;
(function (DataCheckerModels) {
    var MetricsTermTypes;
    (function (MetricsTermTypes) {
        MetricsTermTypes[MetricsTermTypes["Race"] = 0] = "Race";
        MetricsTermTypes[MetricsTermTypes["Ethnicity"] = 1] = "Ethnicity";
        MetricsTermTypes[MetricsTermTypes["Diagnoses"] = 2] = "Diagnoses";
        MetricsTermTypes[MetricsTermTypes["Procedures"] = 3] = "Procedures";
        MetricsTermTypes[MetricsTermTypes["NDC"] = 4] = "NDC";
    })(MetricsTermTypes = DataCheckerModels.MetricsTermTypes || (DataCheckerModels.MetricsTermTypes = {}));
    var MetricsTypes;
    (function (MetricsTypes) {
        MetricsTypes[MetricsTypes["DataPartnerCount"] = 0] = "DataPartnerCount";
        MetricsTypes[MetricsTypes["DataPartnerPercent"] = 1] = "DataPartnerPercent";
        MetricsTypes[MetricsTypes["DataPartnerPercentContribution"] = 2] = "DataPartnerPercentContribution";
        MetricsTypes[MetricsTypes["DataPartnerPresence"] = 3] = "DataPartnerPresence";
        MetricsTypes[MetricsTypes["Overall"] = 4] = "Overall";
        MetricsTypes[MetricsTypes["OverallCount"] = 5] = "OverallCount";
        MetricsTypes[MetricsTypes["OverallPresence"] = 6] = "OverallPresence";
    })(MetricsTypes = DataCheckerModels.MetricsTypes || (DataCheckerModels.MetricsTypes = {}));
})(DataCheckerModels || (DataCheckerModels = {}));
