/// <reference path="../../../Lpp.Dns.Portal/Scripts/Page/Page.ts" />

module Account.Login {
    var vm: ViewModel;
    

    export class ViewModel extends Global.PageViewModel {
        public Login: Dns.ViewModels.LoginViewModel;
        public Terms: KnockoutObservable<string>;

        constructor(theme: Dns.Interfaces.IThemeDTO, bindingControl: JQuery, loginModel: Dns.Interfaces.ILoginDTO) {
            super(bindingControl);
            this.Login = new Dns.ViewModels.LoginViewModel(loginModel);
            this.Terms = ko.observable(theme.Terms);
        }

        public ShowTerms() {
            var self = this;
            Global.ShowTerms(self.Terms());
        }

        public ForgotPassword() {
            Global.Helpers.ShowDialog("Forgot Password?", "/account/forgotpassword", ["Close"], 640, 400).done(() => { });
        }

        public Register() {
            //Hacked for now.
            Global.Helpers.ShowDialog("User Registration", "/account/register", ["Close"], 900, 500);
        }

        public Submit(formElement) {
            if (!this.Validate())
                return false;

            return true;
        }
    }

    export function init(loginModel: Dns.Interfaces.ILoginDTO, bindingControl: JQuery) {       
        Dns.WebApi.Theme.GetText(["Terms"]).done((results: Dns.Interfaces.IThemeDTO[]) => {
            vm = new ViewModel(results[0], bindingControl, loginModel);
            ko.applyBindings(vm, bindingControl[0]);
        })        
    }
}