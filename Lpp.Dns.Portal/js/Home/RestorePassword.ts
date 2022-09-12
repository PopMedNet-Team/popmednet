/// <reference path="../_rootlayout.ts" />

module Home.RestorePassword {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public Password: KnockoutObservable<string>;
        public ConfirmPassword: KnockoutObservable<string>;
        public PasswordScore: KnockoutObservable<number>;

        constructor(bindingControl: JQuery) {
            super(bindingControl);

            this.Password = ko.observable("");
            this.ConfirmPassword = ko.observable("");
            this.PasswordScore = ko.observable(0);

            this.Password.subscribe((value) => {
                this.PasswordScore(Global.Helpers.TestPasswordStrength(this.Password()));
            });
        }

        public Save(data: ViewModel, event) {
            if (!this.Validate())
                return;     

            if (this.Password() != this.ConfirmPassword()) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Passwords do not match.</p>");
                return;
            }      

            if (this.PasswordScore() < 5) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Your password is not strong enough. Please ensure that your password contains at least one capital letter, one number and one symbol.</p>");
                return;
            } 

            let actions = [];
            let dmcsUrl = event.target.dataset.dmcs;
            if (dmcsUrl) {
                actions.push({
                    text: "DataMart Client Server",
                    action: function (e) {
                        let dmcsUrl = $("#btnSubmit").attr("data-DMCS");
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
            } else {
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
            }).done(() => {

                var kendoDialog = <any>$("<div />").kendoDialog(<kendo.ui.DialogOptions>{
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
        }

        public Cancel(data: ViewModel, event) {
            window.location.href = "/";
        }
    }

    function init() {
        $(() => {
            var bindingControl = $("#Content");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}