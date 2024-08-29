import * as Global from "../../scripts/page/global.js";

export class ViewModel extends Global.DialogViewModel {
    public Reason: KnockoutObservable<string>;

    constructor(bindingControl: JQuery) {
        super(bindingControl);

        this.Reason = ko.observable("");
    }

    public Save() {
        if (!this.Validate())
            return;

        this.Close(this.Reason());
    }

    public Cancel() {
        this.Close();
    }
}

$(() => {
    let bindingControl = $("#Content");
    let vm = new ViewModel(bindingControl);
    ko.applyBindings(vm, bindingControl[0]);
});