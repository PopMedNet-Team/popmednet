/// <reference path="../../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="listroutings.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
//# sourceMappingURL=AddDataMartDialog.js.map