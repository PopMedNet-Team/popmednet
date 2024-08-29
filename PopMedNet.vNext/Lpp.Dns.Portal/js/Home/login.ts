/// <reference path="../_rootlayout.ts" />

module Home.Login {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {

        constructor(bindingControl: JQuery) {
            super(bindingControl);
           
        }

        public ShowTerms() {
            Global.ShowTerms(Layout.vmFooter.Theme.Terms());
        }

        public ShowInfo() {
            Global.ShowInfo(Layout.vmFooter.Theme.Info());
        }

        public ForgotPassword() {
            Global.Helpers.ShowDialog("Forgot Password", "/home/forgotpassword", ["close"], 800, 350, { Username: $('#txtUserName').val() });
        }

        public Registration() {
            Global.Helpers.ShowDialog("User Registration", "/home/userregistration", ["close"], 900, 550);
        }

        public Submit() {
            var formElement: JQuery = $("#fLogin");
            if (!this.Validate())
                return;
            formElement.submit();
        }
    }

    function init() {
        $(() => {
            var bindingControl = $("#fLogin");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}