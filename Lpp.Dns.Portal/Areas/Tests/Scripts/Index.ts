/// <reference path="../../../js/_layout.ts" />
module Tests.Index {

    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {

        constructor(bindingControl: JQuery) {
            super(bindingControl);
        }

        public onAddRequestObserver() {            
            Global.Helpers.ShowDialog("Add Users", "/tests/test/addrequestobservers", ['Close'], 640, 630, {});
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