import * as Global from "../../scripts/page/global.js";
import * as WebApi from "../Lpp.Dns.WebApi.js";

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

        Global.Helpers.ShowExecuting();

        WebApi.Users.RestorePassword({
            Password: this.Password(),
            PasswordRestoreToken: Global.GetQueryParam("token")
        })
            .always(() => { Global.Helpers.HideExecuting(); })
            .done(() => {
            Global.Helpers.ShowAlert("Successful!", "<p>You have successfully updated your password. Please login with this new password.</p>").done(() => {
                window.location.href = "/";
            });
        });
    }

    public Cancel(data: ViewModel, event) {
        window.location.href = "/";
    }
}

$(() => {
    let bindingControl = $("#Content");
    let vm = new ViewModel(bindingControl);
    ko.applyBindings(vm, bindingControl[0]);
});