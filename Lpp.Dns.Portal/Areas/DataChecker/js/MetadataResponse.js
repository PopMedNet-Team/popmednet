/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
var DataChecker;
(function (DataChecker) {
    var Metadata;
    (function (Metadata) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
            function ViewModel(parameters) {
                var _this = this;
                this.requestID = ko.observable(null);
                this.responseID = ko.observable(null);
                this.isLoaded = ko.observable(false);
                this.Results = ko.observableArray([]);
                var self = this;
                if (parameters == null) {
                    return;
                }
                else if (parameters.ResponseID == null || parameters.ResponseID() == null) {
                    return;
                }
                else if (parameters.RequestID == null || parameters.RequestID() == null) {
                    return;
                }
                self.responseID(parameters.ResponseID());
                self.requestID(parameters.RequestID());
                $.when($.get('/DataChecker/MetaData/GetTermValues?requestID=' + self.requestID().toString()), $.get('/DataChecker/MetaData/ProcessMetricsByResponse?responseID=' + self.responseID().toString())).then(function (arrTable, result) {
                    result[0].Results.forEach(function (item) {
                        self.Results.push(item);
                    });
                    var metadataTables = arrTable[0];
                    _this.hasDiagnosis = ko.observable($.Enumerable.From(metadataTables).Where(function (t) { return t == MetadataTableTypes.Diagnosis; }).Count() > 0);
                    _this.hasDispensing = ko.observable($.Enumerable.From(metadataTables).Where(function (t) { return t == MetadataTableTypes.Dispensing; }).Count() > 0);
                    _this.hasEncounter = ko.observable($.Enumerable.From(metadataTables).Where(function (t) { return t == MetadataTableTypes.Encounter; }).Count() > 0);
                    _this.hasEnrollment = ko.observable($.Enumerable.From(metadataTables).Where(function (t) { return t == MetadataTableTypes.Enrollment; }).Count() > 0);
                    _this.hasProcedure = ko.observable($.Enumerable.From(metadataTables).Where(function (t) { return t == MetadataTableTypes.Procedure; }).Count() > 0);
                    self.isLoaded(true);
                    //resize the iframe to the contents plus padding for the export dropdown menu
                    $(window.frameElement).height($('html').height() + 70);
                }).fail(function (error) {
                    alert(error);
                    return;
                });
            }
            return ViewModel;
        }());
        Metadata.ViewModel = ViewModel;
        var MetadataTableTypes;
        (function (MetadataTableTypes) {
            MetadataTableTypes[MetadataTableTypes["Diagnosis"] = 0] = "Diagnosis";
            MetadataTableTypes[MetadataTableTypes["Procedure"] = 4] = "Procedure";
            MetadataTableTypes[MetadataTableTypes["Dispensing"] = 1] = "Dispensing";
            MetadataTableTypes[MetadataTableTypes["Encounter"] = 2] = "Encounter";
            MetadataTableTypes[MetadataTableTypes["Enrollment"] = 3] = "Enrollment";
        })(MetadataTableTypes = Metadata.MetadataTableTypes || (Metadata.MetadataTableTypes = {}));
    })(Metadata = DataChecker.Metadata || (DataChecker.Metadata = {}));
})(DataChecker || (DataChecker = {}));
