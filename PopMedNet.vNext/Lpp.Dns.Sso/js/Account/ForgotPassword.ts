/// <reference path="../../../Lpp.Dns.Portal/Scripts/Page/Page.ts" />

module Account.ForgotPassword {
    var vm: ViewModel;
    export class ViewModel extends Global.DialogViewModel {
        public Request: Dns.ViewModels.ForgotPasswordViewModel;
        constructor(bindingControl: JQuery) {
            super(bindingControl);
            this.Request = new Dns.ViewModels.ForgotPasswordViewModel(); 
        }

        public Send() {
            var data = this.Request.toData();

            Dns.WebApi.Users.ForgotPassword(data).done((results) : void => {
                this.Close();
            });
        }
    }

    export function init(bindingControl: JQuery) {
        vm = new ViewModel(bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    }
}