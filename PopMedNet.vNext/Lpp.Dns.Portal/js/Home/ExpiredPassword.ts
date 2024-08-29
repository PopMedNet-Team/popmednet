/// <reference path="../_rootlayout.ts" />

module Home.ExpiredPassword {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ExpiredPassword: Dns.ViewModels.UpdateUserPasswordViewModel;
        public ConfirmPassword: KnockoutObservable<string>;
        public PasswordScore: KnockoutObservable<number>;
        constructor(bindingControl: JQuery) {
            super(bindingControl);
            this.ExpiredPassword = new Dns.ViewModels.UpdateUserPasswordViewModel({UserID: User.ID, Password: ''});
            this.ConfirmPassword = ko.observable("");
            this.PasswordScore = ko.observable(0);
            this.ExpiredPassword.Password.subscribe((value) => {
                this.PasswordScore(Global.Helpers.TestPasswordStrength(value));
            });
        }

        public onSubmit(formElement): boolean {            
            if (!this.Validate())
                return false;
                
            if (this.ConfirmPassword() != this.ExpiredPassword.Password()) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Passwords do not match.</p>");
                return false;
            }
            if (this.PasswordScore() < 5) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that your password contains at least one capital letter, number and symbol.</p>");
                return false;
            }

            return true;
        }
    }


    function init() {
        $.when<any>(
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            })
        );
    }

    init();
}  