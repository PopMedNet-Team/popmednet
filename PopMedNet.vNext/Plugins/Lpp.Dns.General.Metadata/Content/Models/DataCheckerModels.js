/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="requestcriteriamodels.ts" />
var DataCheckerModels;
(function (DataCheckerModels) {
    var RxSupTypes;
    (function (RxSupTypes) {
        RxSupTypes[RxSupTypes["LessThanZero"] = 0] = "LessThanZero";
        RxSupTypes[RxSupTypes["Zero"] = 1] = "Zero";
        RxSupTypes[RxSupTypes["TwoThroughThirty"] = 2] = "TwoThroughThirty";
        RxSupTypes[RxSupTypes["Thirty"] = 3] = "Thirty";
        RxSupTypes[RxSupTypes["Sixty"] = 4] = "Sixty";
        RxSupTypes[RxSupTypes["Ninety"] = 5] = "Ninety";
        RxSupTypes[RxSupTypes["Other"] = 6] = "Other";
        RxSupTypes[RxSupTypes["Missing"] = 7] = "Missing";
    })(RxSupTypes = DataCheckerModels.RxSupTypes || (DataCheckerModels.RxSupTypes = {}));
    var RxAmtTypes;
    (function (RxAmtTypes) {
        RxAmtTypes[RxAmtTypes["LessThanZero"] = 0] = "LessThanZero";
        RxAmtTypes[RxAmtTypes["Zero"] = 1] = "Zero";
        RxAmtTypes[RxAmtTypes["TwoThroughThirty"] = 2] = "TwoThroughThirty";
        RxAmtTypes[RxAmtTypes["Thirty"] = 3] = "Thirty";
        RxAmtTypes[RxAmtTypes["Sixty"] = 4] = "Sixty";
        RxAmtTypes[RxAmtTypes["Ninety"] = 5] = "Ninety";
        RxAmtTypes[RxAmtTypes["OneHundredTwenty"] = 6] = "OneHundredTwenty";
        RxAmtTypes[RxAmtTypes["OneHundredEighty"] = 7] = "OneHundredEighty";
        RxAmtTypes[RxAmtTypes["Other"] = 8] = "Other";
        RxAmtTypes[RxAmtTypes["Missing"] = 9] = "Missing";
    })(RxAmtTypes = DataCheckerModels.RxAmtTypes || (DataCheckerModels.RxAmtTypes = {}));
    var RaceTypes;
    (function (RaceTypes) {
        RaceTypes[RaceTypes["Unknown"] = 0] = "Unknown";
        RaceTypes[RaceTypes["AmericanIndianOrAlaskaNative"] = 1] = "AmericanIndianOrAlaskaNative";
        RaceTypes[RaceTypes["Asian"] = 2] = "Asian";
        RaceTypes[RaceTypes["BlackOrAfricanAmerican"] = 3] = "BlackOrAfricanAmerican";
        RaceTypes[RaceTypes["NativeHawaiianOrOtherPacificIslander"] = 4] = "NativeHawaiianOrOtherPacificIslander";
        RaceTypes[RaceTypes["White"] = 5] = "White";
        RaceTypes[RaceTypes["Missing"] = 6] = "Missing";
    })(RaceTypes = DataCheckerModels.RaceTypes || (DataCheckerModels.RaceTypes = {}));
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
    var MetaDataTableTypes;
    (function (MetaDataTableTypes) {
        MetaDataTableTypes[MetaDataTableTypes["Diagnosis"] = 0] = "Diagnosis";
        MetaDataTableTypes[MetaDataTableTypes["Dispensing"] = 1] = "Dispensing";
        MetaDataTableTypes[MetaDataTableTypes["Encounter"] = 2] = "Encounter";
        MetaDataTableTypes[MetaDataTableTypes["Enrollment"] = 3] = "Enrollment";
        MetaDataTableTypes[MetaDataTableTypes["Procedure"] = 4] = "Procedure";
    })(MetaDataTableTypes = DataCheckerModels.MetaDataTableTypes || (DataCheckerModels.MetaDataTableTypes = {}));
    var EthnicityTypes;
    (function (EthnicityTypes) {
        EthnicityTypes[EthnicityTypes["Hispanic"] = 0] = "Hispanic";
        EthnicityTypes[EthnicityTypes["NotHispanic"] = 1] = "NotHispanic";
        EthnicityTypes[EthnicityTypes["Unknown"] = 2] = "Unknown";
        EthnicityTypes[EthnicityTypes["Missing"] = 3] = "Missing";
    })(EthnicityTypes = DataCheckerModels.EthnicityTypes || (DataCheckerModels.EthnicityTypes = {}));
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
    var PDXTypes;
    (function (PDXTypes) {
        PDXTypes[PDXTypes["Principle"] = 0] = "Principle";
        PDXTypes[PDXTypes["Secondary"] = 1] = "Secondary";
        PDXTypes[PDXTypes["Other"] = 2] = "Other";
        PDXTypes[PDXTypes["Missing"] = 3] = "Missing";
    })(PDXTypes = DataCheckerModels.PDXTypes || (DataCheckerModels.PDXTypes = {}));
})(DataCheckerModels || (DataCheckerModels = {}));
