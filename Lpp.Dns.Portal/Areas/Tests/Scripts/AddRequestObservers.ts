/// <reference path="../../../js/_layout.ts" />
module Tests.AddRequestObserver {

    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {

        constructor(bindingControl: JQuery) {
            super(bindingControl);
        }

    }

    export function init() {
        $(() => {
            var bindingControl = $('#Content');
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 