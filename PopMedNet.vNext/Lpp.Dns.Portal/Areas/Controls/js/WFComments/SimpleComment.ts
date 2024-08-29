/// <reference path="../../../../js/_layout.ts" />
module Controls.WFComments.SimpleComment {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public Comment: KnockoutObservable<string>;

        public onCancel: () => void;
        public onSave: () => void;

        constructor(bindingControl: JQuery) {
            super(bindingControl);

            this.Comment = ko.observable('');

            var self = this;
            self.onCancel = () => {
                self.Close();
            };

            self.onSave = () => {

                if (!self.Validate())
                    return;

                self.Close(self.Comment());
            };

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