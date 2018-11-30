/// <reference path="../../Scripts/page/Page.ts" />

module Dialog.Info {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public Info: KnockoutObservable<string>;

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            var self = this;
            this.Info = ko.observable(this.Parameters)

        }

    }

    function init() {
        $(() => {
            var bindingControl = $("body");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 