/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />
var RequestCriteriaModels;
(function (RequestCriteriaModels) {
    var SexTypes;
    (function (SexTypes) {
        SexTypes[SexTypes["NotSpecified"] = -1] = "NotSpecified";
        SexTypes[SexTypes["Male"] = 1] = "Male";
        SexTypes[SexTypes["Female"] = 2] = "Female";
        SexTypes[SexTypes["Both"] = 3] = "Both";
        SexTypes[SexTypes["Aggregated"] = 4] = "Aggregated";
    })(SexTypes = RequestCriteriaModels.SexTypes || (RequestCriteriaModels.SexTypes = {}));
})(RequestCriteriaModels || (RequestCriteriaModels = {}));
