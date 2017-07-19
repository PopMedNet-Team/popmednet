/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />
var DataCheckerModels;
(function (DataCheckerModels) {
    var EncounterTypes;
    (function (EncounterTypes) {
        EncounterTypes[EncounterTypes["All"] = 0] = "All";
        EncounterTypes[EncounterTypes["AmbulatoryVisit"] = 1] = "AmbulatoryVisit";
        EncounterTypes[EncounterTypes["EmergencyDepartment"] = 2] = "EmergencyDepartment";
        EncounterTypes[EncounterTypes["InpatientHospitalStay"] = 3] = "InpatientHospitalStay";
        EncounterTypes[EncounterTypes["NonAcuteInstitutionalStay"] = 4] = "NonAcuteInstitutionalStay";
        EncounterTypes[EncounterTypes["OtherAmbulatoryVisit"] = 5] = "OtherAmbulatoryVisit";
        EncounterTypes[EncounterTypes["Missing"] = 6] = "Missing";
    })(EncounterTypes = DataCheckerModels.EncounterTypes || (DataCheckerModels.EncounterTypes = {}));
})(DataCheckerModels || (DataCheckerModels = {}));
