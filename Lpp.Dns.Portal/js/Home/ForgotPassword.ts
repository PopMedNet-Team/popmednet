/// <reference path="../_rootlayout.ts" />

module Home.ForgotPassword {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public Email: KnockoutObservable<string>;
        public UserName: KnockoutObservable<string>;

        constructor(bindingControl: JQuery) {
            super(bindingControl);

            this.Email = ko.observable(null);
            this.UserName = ko.observable(this.Parameters.Username);
        }

        public Save() {
            if (!this.Validate())
                return;

            if (!this.Email() && !this.UserName()) {
                Global.Helpers.ShowAlert("Validation Error", "Please enter your email address or user name");
                return;
            }

            Dns.WebApi.Users.ForgotPassword({ UserName: this.UserName(), Email: this.Email() }, true).done(() => {
                Global.Helpers.ShowAlert("Request Submitted", "<p>An email has been sent to you that includes the link that you will need to visit to update your information. Please follow the instructions included. Thank you!</p>").done(() => {
                    this.Close(null);
                });
            }).fail((e: any) => {
                var msg = e.responseJSON.errors != null ? e.responseJSON.errors[0].Description : e.responseJSON.results[0];
                Global.Helpers.ShowAlert("Password Reset Error", msg).done(() => {
                    this.Close(null);
                });
            });
        }

        public Cancel() {
            this.Close(null);
        }
    }

    function init() {
        $(() => {
            var bindingControl = jQuery("#Content");

            vm = new ViewModel(bindingControl);

            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}