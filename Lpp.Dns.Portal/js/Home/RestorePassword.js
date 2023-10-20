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
var Home;
(function (Home) {
    var RestorePassword;
    (function (RestorePassword) {
        var vm;
        var ViewModel = (function (_super) {
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
                    Global.Helpers.ShowAlert("Validation Error", "<p>Passwords do not match.</p>");
                    return;
                }
                if (this.PasswordScore() < 5) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Your password is not strong enough. The password must be 15 characters minimum. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':&semi;&lt;'.</p>");
                    return;
                }
                var actions = [];
                var dmcsUrl = event.target.dataset.dmcs;
                if (dmcsUrl) {
                    actions.push({
                        text: "DataMart Client Server",
                        action: function (e) {
                            var dmcsUrl = $("#btnSubmit").attr("data-DMCS");
                            window.location.href = dmcsUrl;
                            return true;
                        }
                    });
                    actions.push({
                        text: "PopMedNet",
                        primary: true,
                        action: function (e) {
                            window.location.href = "/";
                            return true;
                        }
                    });
                }
                else {
                    actions.push({
                        text: "OK",
                        primary: true,
                        action: function (e) {
                            window.location.href = "/";
                            return true;
                        }
                    });
                }
                Dns.WebApi.Users.RestorePassword({
                    Password: this.Password(),
                    PasswordRestoreToken: $.url().param("token")
                }).done(function () {
                    var kendoDialog = $("<div />").kendoDialog({
                        title: "Successful!",
                        resizable: false,
                        modal: true,
                        buttonLayout: "normal",
                        closable: false,
                        content: "<p class='text-center' style='margin-top:1em;margin-bottom:1em;' >You have successfully updated your password. Please login using this new password.",
                        actions: actions,
                        width: 600,
                    });
                    kendoDialog.data("kendoDialog").open();
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
