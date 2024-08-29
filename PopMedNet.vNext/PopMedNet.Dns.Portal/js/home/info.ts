import * as Global from "../../scripts/page/global.js";
class ViewModel extends Global.DialogViewModel {
    Info: KnockoutObservable<string>;

    constructor(bindingControl: JQuery) {
        super(bindingControl);
        let self = this;
        this.Info = ko.observable(this.Parameters);
    }
}

$(function () {
    let bindingControl = $("body");
    let vm = new ViewModel(bindingControl);
    ko.applyBindings(vm, bindingControl[0]);
});