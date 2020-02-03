/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../plugins/lpp.dns.healthcare.datachecker/content/responses.common.ts" />
var DataChecker;
(function (DataChecker) {
    var MetaData;
    (function (MetaData) {
        var vm;
        var _bindingControl;
        var ViewModel = /** @class */ (function () {
            function ViewModel(model) {
                this._model = model;
                this.hasDiagnosis = ko.observable($.Enumerable.From(model.MetadataTables).Where(function (t) { return t == MetadataTableTypes.Diagnosis; }).Count() > 0);
                this.hasDispensing = ko.observable($.Enumerable.From(model.MetadataTables).Where(function (t) { return t == MetadataTableTypes.Dispensing; }).Count() > 0);
                this.hasEncounter = ko.observable($.Enumerable.From(model.MetadataTables).Where(function (t) { return t == MetadataTableTypes.Encounter; }).Count() > 0);
                this.hasEnrollment = ko.observable($.Enumerable.From(model.MetadataTables).Where(function (t) { return t == MetadataTableTypes.Enrollment; }).Count() > 0);
                this.hasProcedure = ko.observable($.Enumerable.From(model.MetadataTables).Where(function (t) { return t == MetadataTableTypes.Procedure; }).Count() > 0);
                this._documentID = this._model.ResponseDocumentIDs[0];
                this.Results = ko.observableArray([]);
            }
            ViewModel.prototype.loadData = function () {
                var _this = this;
                $.get('/DataChecker/MetaData/ProcessMetrics?documentID=' + this._documentID).done(function (result) {
                    result.Results.forEach(function (item) {
                        _this.Results.push(item);
                    });
                })
                    .fail(function (error) {
                    alert(error);
                });
            };
            return ViewModel;
        }());
        MetaData.ViewModel = ViewModel;
        function init(model, bindingControl) {
            _bindingControl = bindingControl;
            vm = new ViewModel(model);
            ko.applyBindings(vm, bindingControl[0]);
            vm.loadData();
        }
        MetaData.init = init;
        var MetadataTableTypes;
        (function (MetadataTableTypes) {
            MetadataTableTypes[MetadataTableTypes["Diagnosis"] = 0] = "Diagnosis";
            MetadataTableTypes[MetadataTableTypes["Procedure"] = 4] = "Procedure";
            MetadataTableTypes[MetadataTableTypes["Dispensing"] = 1] = "Dispensing";
            MetadataTableTypes[MetadataTableTypes["Encounter"] = 2] = "Encounter";
            MetadataTableTypes[MetadataTableTypes["Enrollment"] = 3] = "Enrollment";
        })(MetadataTableTypes = MetaData.MetadataTableTypes || (MetaData.MetadataTableTypes = {}));
    })(MetaData = DataChecker.MetaData || (DataChecker.MetaData = {}));
})(DataChecker || (DataChecker = {}));
