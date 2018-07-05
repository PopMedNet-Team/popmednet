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
var Home;
(function (Home) {
    var RestorePassword;
    (function (RestorePassword) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Password = ko.observable("");
                _this.ConfirmPassword = ko.observable("");
                _this.PasswordScore = ko.observable(0);
                _this.Password.subscribe(function (value) {
                    _this.PasswordScore(Global.Helpers.TestPasswordStrength(_this.Password()));
                });
                return _this;
            }
            ViewModel.prototype.Save = function (data, event) {
                if (!this.Validate())
                    return;
                if (this.Password() != this.ConfirmPassword()) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that the passwords match</p>");
                    return;
                }
                if (this.PasswordScore() < 5) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Your password is not strong enough. Please ensure that your password contains at least one capital letter, one number and one symbol.</p>");
                    return;
                }
                Dns.WebApi.Users.RestorePassword({
                    Password: this.Password(),
                    PasswordRestoreToken: $.url().param("token")
                }).done(function () {
                    Global.Helpers.ShowAlert("Successful!", "<p>You have successfully updated your password. Please login with this new password.</p>").done(function () {
                        window.location.href = "/";
                    });
                });
            };
            ViewModel.prototype.Cancel = function (data, event) {
                window.location.href = "/";
            };
            return ViewModel;
        }(Global.PageViewModel));
        RestorePassword.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(RestorePassword = Home.RestorePassword || (Home.RestorePassword = {}));
})(Home || (Home = {}));
//# sourceMappingURL=RestorePassword.js.map