/// <reference path="../_rootlayout.ts" />

module Users.Deactivate {
    var vm: ViewModel;
    export class ViewModel extends Global.DialogViewModel {
        public Reason: KnockoutObservable<string>;

        constructor(bindingControl: JQuery) {
            super(bindingControl);

            this.Reason = ko.observable("");
        }

        public Save() {
            if (!this.Validate())
                return;

            vm.Close(vm.Reason());
        }

        public Cancel() {
            vm.Close();
        }
    }

    function init() {
        $(() => {
            var bindingControl = $("#Content");
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 