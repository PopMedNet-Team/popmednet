/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />
var DataCheckerModels;
(function (DataCheckerModels) {
    var MetaDataTableTypes;
    (function (MetaDataTableTypes) {
        MetaDataTableTypes[MetaDataTableTypes["Diagnosis"] = 0] = "Diagnosis";
        MetaDataTableTypes[MetaDataTableTypes["Dispensing"] = 1] = "Dispensing";
        MetaDataTableTypes[MetaDataTableTypes["Encounter"] = 2] = "Encounter";
        MetaDataTableTypes[MetaDataTableTypes["Enrollment"] = 3] = "Enrollment";
        MetaDataTableTypes[MetaDataTableTypes["Procedure"] = 4] = "Procedure";
    })(MetaDataTableTypes = DataCheckerModels.MetaDataTableTypes || (DataCheckerModels.MetaDataTableTypes = {}));
})(DataCheckerModels || (DataCheckerModels = {}));
