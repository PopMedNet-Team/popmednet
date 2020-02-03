/// <reference path="../_rootlayout.ts" />
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
var Home;
(function (Home) {
    var ExpiredPassword;
    (function (ExpiredPassword) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.ExpiredPassword = new Dns.ViewModels.UpdateUserPasswordViewModel({ UserID: User.ID, Password: '' });
                _this.ConfirmPassword = ko.observable("");
                _this.PasswordScore = ko.observable(0);
                _this.ExpiredPassword.Password.subscribe(function (value) {
                    _this.PasswordScore(Global.Helpers.TestPasswordStrength(value));
                });
                return _this;
            }
            ViewModel.prototype.onSubmit = function (formElement) {
                if (!this.Validate())
                    return false;
                if (this.ConfirmPassword() != this.ExpiredPassword.Password()) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that the passwords enter match.</p>");
                    return false;
                }
                if (this.PasswordScore() < 5) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that your password contains at least one capital letter, number and symbol.</p>");
                    return false;
                }
                return true;
            };
            return ViewModel;
        }(Global.PageViewModel));
        ExpiredPassword.ViewModel = ViewModel;
        function init() {
            $.when($(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            }));
        }
        init();
    })(ExpiredPassword = Home.ExpiredPassword || (Home.ExpiredPassword = {}));
})(Home || (Home = {}));
