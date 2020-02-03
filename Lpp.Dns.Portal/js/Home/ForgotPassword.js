/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Home;
(function (Home) {
    var ForgotPassword;
    (function (ForgotPassword) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Email = ko.observable(null);
                _this.UserName = ko.observable(_this.Parameters.Username);
                return _this;
            }
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!this.Validate())
                    return;
                if (!this.Email() && !this.UserName()) {
                    Global.Helpers.ShowAlert("Validation Error", "Please enter your email address or user name");
                    return;
                }
                Dns.WebApi.Users.ForgotPassword({ UserName: this.UserName(), Email: this.Email() }, true).done(function () {
                    Global.Helpers.ShowAlert("Request Submitted", "<p>An email has been sent to you that includes the link that you will need to visit to update your information. Please follow the instructions included. Thank you!</p>").done(function () {
                        _this.Close(null);
                    });
                }).fail(function (e) {
                    var msg = e.responseJSON.errors != null ? e.responseJSON.errors[0].Description : e.responseJSON.results[0];
                    Global.Helpers.ShowAlert("Password Reset Error", msg).done(function () {
                        _this.Close(null);
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                this.Close(null);
            };
            return ViewModel;
        }(Global.DialogViewModel));
        ForgotPassword.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = jQuery("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(ForgotPassword = Home.ForgotPassword || (Home.ForgotPassword = {}));
})(Home || (Home = {}));
//# sourceMappingURL=ForgotPassword.js.map