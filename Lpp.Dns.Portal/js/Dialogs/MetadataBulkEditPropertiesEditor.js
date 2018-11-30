var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../Scripts/page/Page.ts" />
var Dialog;
(function (Dialog) {
    var MetadataBulkEditPropertiesEditor;
    (function (MetadataBulkEditPropertiesEditor) {
        var vm;
        var dvm;
        var DataMartsViewModel = /** @class */ (function () {
            function DataMartsViewModel(routing) {
                var self = this;
                self.DataMartID = routing.DataMartID;
                self.RequestDataMartID = routing.ID;
                self.DataMartName = routing.DataMart;
                self.Selected = ko.observable(false);
            }
            return DataMartsViewModel;
        }());
        MetadataBulkEditPropertiesEditor.DataMartsViewModel = DataMartsViewModel;
        var MetadataBulkEditPropertiesEditorViewModel = /** @class */ (function (_super) {
            __extends(MetadataBulkEditPropertiesEditorViewModel, _super);
            function MetadataBulkEditPropertiesEditorViewModel(bindingControl, defaultPriority, defaultDueDate, isRequestLevel) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                self.IsRequestLevel = isRequestLevel;
                _this.InfoText = isRequestLevel ? 'The following values will be applied to the selected requests.' : 'The following values will be applied to the selected datamarts.';
                self.Priority = ko.observable(defaultPriority);
                self.DueDate = ko.observable(defaultDueDate);
                self.PrioritySelected = ko.observable(false);
                self.DueDateSelected = ko.observable(false);
                self.ApplyToRoutings = ko.observable(false);
                self.Priority.subscribe(function (v) { return self.PrioritySelected(true); });
                self.DueDate.subscribe(function (v) { return self.DueDateSelected(true); });
                self.onApply = function () {
                    var stringDate = "";
                    if (self.DueDate() != null) {
                        stringDate = self.DueDate().toDateString();
                    }
                    var results = {
                        UpdatePriority: self.PrioritySelected(),
                        UpdateDueDate: self.DueDateSelected(),
                        PriorityValue: self.Priority(),
                        DueDateValue: self.DueDate(),
                        ApplyToRoutings: self.ApplyToRoutings(),
                        stringDate: stringDate
                    };
                    self.Close(results);
                };
                self.onCancel = function () {
                    self.Close(null);
                };
                return _this;
            }
            return MetadataBulkEditPropertiesEditorViewModel;
        }(Global.DialogViewModel));
        MetadataBulkEditPropertiesEditor.MetadataBulkEditPropertiesEditorViewModel = MetadataBulkEditPropertiesEditorViewModel;
        function init() {
            var window = Global.Helpers.GetDialogWindow();
            var parameters = (window.options).parameters;
            var defaultPriority = (parameters.defaultPriority);
            var defaultDueDate = (parameters.defaultDueDate);
            var isRequestLevel = (parameters.isRequestLevel || false);
            $(function () {
                var bindingControl = $("MetadataBulkEditPropertiesEditor");
                vm = new MetadataBulkEditPropertiesEditorViewModel(bindingControl, defaultPriority, defaultDueDate, isRequestLevel);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(MetadataBulkEditPropertiesEditor = Dialog.MetadataBulkEditPropertiesEditor || (Dialog.MetadataBulkEditPropertiesEditor = {}));
})(Dialog || (Dialog = {}));
