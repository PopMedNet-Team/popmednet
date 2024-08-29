import * as Global from "../../scripts/page/global.js";
class ViewModel extends Global.DialogViewModel {
    Terms: KnockoutObservable<string>;

    constructor(bindingControl: JQuery) {
        super(bindingControl);
        this.Terms = ko.observable(this.Parameters)
    }
}

$(() => {
    var bindingControl = $("body");
    let vm = new ViewModel(bindingControl);
    ko.applyBindings(vm, bindingControl[0]);
});