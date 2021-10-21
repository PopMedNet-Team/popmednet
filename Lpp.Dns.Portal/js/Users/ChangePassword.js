/// <reference path="../_rootlayout.ts" />
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
var Users;
(function (Users) {
    var ChangePassword;
    (function (ChangePassword) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(id, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.ID = id;
                _this.Password = ko.observable("");
                _this.ConfirmPassword = ko.observable("");
                _this.PasswordScore = ko.observable(0);
                _this.Password.subscribe(function (value) {
                    _this.PasswordScore(Global.Helpers.TestPasswordStrength(_this.Password()));
                });
                return _this;
            }
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!vm.Validate())
                    return;
                if (vm.Password().length > 50) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Maximum password length is 50.</p>");
                    return;
                }
                if (vm.Password() != vm.ConfirmPassword()) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that the passwords match.</p>");
                    return;
                }
                if (this.PasswordScore() < 5) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Your password is not strong enough. Please ensure that your password is at least 8 characters long, contains at least one capital letter, one number and one symbol.</p>", 300);
                    return;
                }
                Dns.WebApi.Users.ChangePassword({
                    Password: this.Password(),
                    UserID: this.ID || User.ID
                }).done(function () {
                    _this.Close(true);
                });
            };
            ViewModel.prototype.Cancel = function () {
                this.Close(false);
            };
            return ViewModel;
        }(Global.DialogViewModel));
        ChangePassword.ViewModel = ViewModel;
        function init() {
            $(function () {
                var id = $.url().param("ID");
                var bindingControl = $("#Content");
                vm = new ViewModel(id, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(ChangePassword = Users.ChangePassword || (Users.ChangePassword = {}));
})(Users || (Users = {}));
