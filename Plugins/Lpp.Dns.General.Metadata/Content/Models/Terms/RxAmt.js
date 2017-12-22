/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />
var DataCheckerModels;
(function (DataCheckerModels) {
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
})(DataCheckerModels || (DataCheckerModels = {}));
