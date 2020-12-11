/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Home;
(function (Home) {
    var UserRegistration;
    (function (UserRegistration) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Registration = new Dns.ViewModels.UserRegistrationViewModel();
                _this.ConfirmPassword = ko.observable("");
                _this.PasswordScore = ko.observable(0);
                _this.Registration.Password.subscribe(function (value) {
                    vm.PasswordScore(Global.Helpers.TestPasswordStrength(value));
                });
                return _this;
            }
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!this.Validate())
                    return;
                if (this.ConfirmPassword() != this.Registration.Password()) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that the passwords enter match.</p>");
                    return;
                }
                var registration = this.Registration.toData();
                //This is in relation to PMNMAINT-1220.
                //We're basically checking to make sure we're not passing in an empty string for phone/fax values.
                //If it's an empty string, we replace it with null. Otherwise entity validation throws an exception as detailed in PMNMAINT-1220.
                if (registration.Phone != null && registration.Phone.trim().length == 0)
                    registration.Phone = null;
                if (registration.Fax != null && registration.Fax.trim().length == 0)
                    registration.Fax = null;
                Dns.WebApi.Users.UserRegistration(registration, true).done(function () {
                    Global.Helpers.ShowAlert("Request Submitted", "<p>Your User Registration Has been submitted</p>").done(function () {
                        _this.Close(null);
                    });
                }).fail(function (e) {
                    var msg = e.responseJSON.errors != null ? e.responseJSON.errors[0].Description : e.responseJSON.results[0];
                    //This is in relation to PMNMAINT-1220.
                    //In case of validation errors at the API/Entity level, the error is stored in the errors collection on the results object.
                    if (typeof msg === 'object' && msg != null)
                        msg = e.responseJSON.results[0].errors[0].Description;
                    Global.Helpers.ShowAlert("Registration Error", msg).done(function () {
                        return;
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                this.Close(null);
            };
            return ViewModel;
        }(Global.DialogViewModel));
        UserRegistration.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = jQuery("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(UserRegistration = Home.UserRegistration || (Home.UserRegistration = {}));
})(Home || (Home = {}));
