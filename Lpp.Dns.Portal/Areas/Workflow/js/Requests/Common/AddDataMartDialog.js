var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var AddDataMartDialog;
        (function (AddDataMartDialog) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, currentRoutings, allDataMarts, projectID) {
                    var _this = _super.call(this, bindingControl) || this;
                    var self = _this;
                    _this.RecievedDataMarts = currentRoutings;
                    _this.AllUnfilteredDataMarts = allDataMarts;
                    _this.SelectedDataMartIDs = ko.observableArray([]);
                    self.RoutesSelectAll = ko.pureComputed({
                        read: function () {
                            return self.AllUnfilteredDataMarts.length > 0 && self.SelectedDataMartIDs().length === self.AllUnfilteredDataMarts.length;
                        },
                        write: function (value) {
                            if (value) {
                                var allID = ko.utils.arrayMap(self.AllUnfilteredDataMarts, function (i) { return i.ID; });
                                self.SelectedDataMartIDs(allID);
                            }
                            else {
                                self.SelectedDataMartIDs([]);
                            }
                        }
                    });
                    return _this;
                }
                ViewModel.prototype.onContinue = function () {
                    vm.Close(this.SelectedDataMartIDs());
                };
                ViewModel.prototype.onCancel = function () {
                    vm.Close(null);
                };
                return ViewModel;
            }(Global.DialogViewModel));
            AddDataMartDialog.ViewModel = ViewModel;
            function init() {
                $(function () {
                    var projectID = Global.GetQueryParam("projectID");
                    var window = Global.Helpers.GetDialogWindow();
                    var parameters = (window.options).parameters;
                    var currentRoutings = (parameters.CurrentRoutings || null);
                    var allDataMarts = (parameters.AllDataMarts || null);
                    $.when()
                        .done(function (datamarts) {
                        var bindingControl = $("#AddDataMartDialog");
                        vm = new ViewModel(bindingControl, currentRoutings, allDataMarts, projectID);
                        $(function () {
                            ko.applyBindings(vm, bindingControl[0]);
                        });
                    });
                });
            }
            AddDataMartDialog.init = init;
            init();
        })(AddDataMartDialog = Common.AddDataMartDialog || (Common.AddDataMartDialog = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
