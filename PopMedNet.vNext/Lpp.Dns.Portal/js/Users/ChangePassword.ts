/// <reference path="../_rootlayout.ts" />

module Users.ChangePassword {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public Password: KnockoutObservable<string>;
        public ConfirmPassword: KnockoutObservable<string>;
        public PasswordScore: KnockoutObservable<number>;
        private ID: any;

        constructor(id: any, bindingControl: JQuery) {
            super(bindingControl);

            this.ID = id;
            this.Password = ko.observable("");
            this.ConfirmPassword = ko.observable("");
            this.PasswordScore = ko.observable(0);

            this.Password.subscribe((value) => {
                this.PasswordScore(Global.Helpers.TestPasswordStrength(this.Password()));
            });
        }

        public Save() {
            if (!vm.Validate())
                return;

            if (vm.Password().length > 50) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Maximum password length is 50.</p>");
                return;
            }

            if (vm.Password() != vm.ConfirmPassword()) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Passwords do not match.</p>");
                return;
            }

            if (this.PasswordScore() < 5) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Your password is not strong enough. Please ensure that your password is at least 8 characters long, contains at least one capital letter, one number and one symbol.</p>", 300);
                return;
            }    

            Dns.WebApi.Users.ChangePassword({
                Password: this.Password(),
                UserID: this.ID || User.ID
            }).done(() => {
                    this.Close(true);
                });
        }

        public Cancel() {
            this.Close(false);
        }
    }

    function init() {
        $(() => {
            var id: any = $.url().param("ID");
            var bindingControl = $("#Content");

            vm = new ViewModel(id, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 