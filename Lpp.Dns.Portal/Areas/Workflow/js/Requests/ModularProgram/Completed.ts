/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.ModularProgram.Completed{
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        constructor(bindingControl: JQuery) {
            super(bindingControl)

        }
    }

    $(() => {
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        var bindingControl = $('#CompletedDetail');
        vm = new ViewModel(bindingControl);
        $(() => {
            ko.applyBindings(vm, bindingControl[0]);
        });
    });
}