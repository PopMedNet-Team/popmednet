/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />
var RequestCriteriaModels;
(function (RequestCriteriaModels) {
    var AgeStratifierTypes;
    (function (AgeStratifierTypes) {
        AgeStratifierTypes[AgeStratifierTypes["NotSpecified"] = -1] = "NotSpecified";
        AgeStratifierTypes[AgeStratifierTypes["None"] = 0] = "None";
        AgeStratifierTypes[AgeStratifierTypes["Ten"] = 1] = "Ten";
        AgeStratifierTypes[AgeStratifierTypes["Seven"] = 2] = "Seven";
        AgeStratifierTypes[AgeStratifierTypes["Four"] = 3] = "Four";
        AgeStratifierTypes[AgeStratifierTypes["Two"] = 4] = "Two";
    })(AgeStratifierTypes = RequestCriteriaModels.AgeStratifierTypes || (RequestCriteriaModels.AgeStratifierTypes = {}));
})(RequestCriteriaModels || (RequestCriteriaModels = {}));
