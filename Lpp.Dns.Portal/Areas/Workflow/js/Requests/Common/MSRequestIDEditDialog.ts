

module Workflow.Common.MSRequestID {
    var vm: ViewModel;
    export class ViewModel extends Global.DialogViewModel {

        private MSRequestID: KnockoutObservable<string>;

        private onSave: () => void;
        private onCancel: () => void;

        constructor(bindingControl: JQuery, requestID: string) {
            super(bindingControl);
            var self = this;

            this.MSRequestID = ko.observable(requestID);

            self.onSave = () => {
                self.Close({ MSRequestID: self.MSRequestID() });
            };

            self.onCancel = () => {
                self.Close(null);
            };

        }
    }

    export function init() {
        var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        var parameters = (<any>(window.options)).parameters;
        var requestID = <string>parameters.MSRequestID || null;
        var bindingControl = $('#EditMSRequestIDDialog');
        vm = new ViewModel(bindingControl, requestID);
        $(() => {
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();

}