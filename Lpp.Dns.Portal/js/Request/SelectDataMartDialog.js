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
var Requests;
(function (Requests) {
    var Utility;
    (function (Utility) {
        var SelectDataMarts;
        (function (SelectDataMarts) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(datamarts, bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.DataMarts = datamarts;
                    _this.SelectedDataMartIDs = ko.observableArray([]);
                    return _this;
                }
                ViewModel.prototype.onContinue = function () {
                    vm.Close(this.SelectedDataMartIDs());
                };
                ViewModel.prototype.onCancel = function () {
                    vm.Close();
                };
                return ViewModel;
            }(Global.DialogViewModel));
            SelectDataMarts.ViewModel = ViewModel;
            function init() {
                $(function () {
                    var window = Global.Helpers.GetDialogWindow();
                    var datamarts = (window.options).parameters.DataMarts;
                    var bindingControl = $("#Content");
                    vm = new ViewModel(datamarts, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            SelectDataMarts.init = init;
            init();
        })(SelectDataMarts = Utility.SelectDataMarts || (Utility.SelectDataMarts = {}));
    })(Utility = Requests.Utility || (Requests.Utility = {}));
})(Requests || (Requests = {}));
