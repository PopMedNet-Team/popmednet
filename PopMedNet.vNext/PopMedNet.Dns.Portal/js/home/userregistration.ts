﻿import * as Global from "../../scripts/page/global.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";

export class ViewModel extends Global.DialogViewModel {
    public Registration: ViewModels.UserRegistrationViewModel;

    public ConfirmPassword: KnockoutObservable<string>;
    public PasswordScore: KnockoutObservable<number>;

    constructor(bindingControl: JQuery) {
        super(bindingControl);

        this.Registration = new ViewModels.UserRegistrationViewModel();
        this.ConfirmPassword = ko.observable("");
        this.PasswordScore = ko.observable(0);

        let self = this;
        this.Registration.Password.subscribe((value) => {
            self.PasswordScore(Global.Helpers.TestPasswordStrength(value));
        });
    }

    public Save() {
        if (!this.Validate())
            return;

        if (this.ConfirmPassword() != this.Registration.Password()) {
            Global.Helpers.ShowAlert("Validation Error", "<p>Passwords do not match.</p>");
            return;
        }

        let registration = this.Registration.toData();

        //This is in relation to PMNMAINT-1220.
        //We're basically checking to make sure we're not passing in an empty string for phone/fax values.
        //If it's an empty string, we replace it with null. Otherwise entity validation throws an exception as detailed in PMNMAINT-1220.
        if (registration.Phone != null && registration.Phone.trim().length == 0)
            registration.Phone = null;
        if (registration.Fax != null && registration.Fax.trim().length == 0)
            registration.Fax = null;

        WebApi.Users.UserRegistration(registration, true).done(() => {
            Global.Helpers.ShowAlert("Request Submitted", "<p>Your User Registration Has been submitted</p>").done(() => {
                this.Close(null);
            });
        }).fail((e: any) => {
            let msg = e.responseJSON.errors != null ? e.responseJSON.errors[0].Description : e.responseJSON.results[0];

            //This is in relation to PMNMAINT-1220.
            //In case of validation errors at the API/Entity level, the error is stored in the errors collection on the results object.
            if (typeof msg === 'object' && msg != null)
                msg = e.responseJSON.results[0].errors[0].Description;

            Global.Helpers.ShowAlert("Registration Error", msg).done(() => {
                return;
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