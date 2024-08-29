import * as Global from "../../scripts/page/global.js";
import * as Layout from "../_Layout.js";

class ViewModel extends Global.PageViewModel {

    constructor(bindingControl: JQuery) {
        super(bindingControl);
    }

    public ShowTerms() {
        Global.ShowTerms(Layout.vmFooter.Theme.Terms);
    }

    public ShowInfo() {
        Global.ShowInfo(Layout.vmFooter.Theme.Info);
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

$(() => {
    //reset the cached main menu
    Global.Session("MainMenu", null);

    let bindingControl = $("#fLogin");
    ko.applyBindings(new ViewModel(bindingControl), bindingControl[0]);
});