/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Users;
(function (Users) {
    var Deactivate;
    (function (Deactivate) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Reason = ko.observable("");
                return _this;
            }
            ViewModel.prototype.Save = function () {
                if (!this.Validate())
                    return;
                vm.Close(vm.Reason());
            };
            ViewModel.prototype.Cancel = function () {
                vm.Close();
            };
            return ViewModel;
        }(Global.DialogViewModel));
        Deactivate.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(Deactivate = Users.Deactivate || (Users.Deactivate = {}));
})(Users || (Users = {}));
//# sourceMappingURL=Deactivate.js.map