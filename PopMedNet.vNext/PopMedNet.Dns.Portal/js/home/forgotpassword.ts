import * as Global from "../../scripts/page/global.js";
import * as WebApi from "../Lpp.Dns.WebApi.js";

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

        Global.Helpers.ShowExecuting();
        WebApi.Users.ForgotPassword({ UserName: this.UserName(), Email: this.Email() }, true)
            .always(() => { Global.Helpers.HideExecuting(); })
            .done(() => {
            Global.Helpers.ShowAlert("Request Submitted", "<p>An email has been sent to you that includes the link that you will need to visit to update your information. Please follow the instructions included. Thank you!</p>").done(() => {
                this.Close(null);
            });
        }).fail((e: any) => {
            //let msg = e.responseJSON.errors != null ? e.responseJSON.errors[0].Description : e.responseJSON.results[0];
            let msg = Global.Helpers.ProcessAjaxError(e);
            Global.Helpers.ShowAlert("Password Reset Error", msg).done(() => {
                this.Close(null);
            });
        });
    }

    public Cancel() {
        this.Close(null);
    }
}

$(() => {
    let bindingControl = jQuery("#Content");

    let vm = new ViewModel(bindingControl);

    ko.applyBindings(vm, bindingControl[0]);
});