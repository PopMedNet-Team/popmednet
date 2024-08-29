import * as Global from "../../scripts/page/global.js";
import * as WebApi from "../Lpp.Dns.WebApi.js";
export class ViewModel extends Global.DialogViewModel {
    Password;
    ConfirmPassword;
    PasswordScore;
    ID;
    constructor(id, bindingControl) {
        super(bindingControl);
        this.ID = id;
        this.Password = ko.observable("");
        this.ConfirmPassword = ko.observable("");
        this.PasswordScore = ko.observable(0);
        this.Password.subscribe((value) => {
            this.PasswordScore(Global.Helpers.TestPasswordStrength(this.Password()));
        });
    }
    Save() {
        if (!this.Validate())
            return;
        if (this.Password().length > 50) {
            Global.Helpers.ShowAlert("Validation Error", "<p>Maximum password length is 50.</p>");
            return;
        }
        if (this.Password() != this.ConfirmPassword()) {
            Global.Helpers.ShowAlert("Validation Error", "<p>Passwords do not match.</p>");
            return;
        }
        if (this.PasswordScore() < 5) {
            Global.Helpers.ShowAlert("Validation Error", "<p>Your password is not strong enough. Please ensure that your password is at least 8 characters long, contains at least one capital letter, one number and one symbol.</p>", 300);
            return;
        }
        WebApi.Users.ChangePassword({
            Password: this.Password(),
            UserID: this.ID || Global.User.ID
        }).done(() => {
            this.Close(true);
        });
    }
    Cancel() {
        this.Close(false);
    }
}
$(() => {
    let id = Global.GetQueryParam("ID");
    let bindingControl = $("#Content");
    let vm = new ViewModel(id, bindingControl);
    ko.applyBindings(vm, bindingControl[0]);
});
//# sourceMappingURL=changepassword.js.map