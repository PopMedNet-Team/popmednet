/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />
module Request.Utility.SelectDataMarts {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {

        public DataMarts: Dns.Interfaces.IDataMartListDTO[];
        public SelectedDataMartIDs: KnockoutObservableArray<string>;

        constructor(datamarts: Dns.Interfaces.IDataMartListDTO[], bindingControl: JQuery) {
            super(bindingControl);

            this.DataMarts = datamarts;
            this.SelectedDataMartIDs = ko.observableArray([]);
        }

        public onContinue() {
            vm.Close(this.SelectedDataMartIDs());
        }

        public onCancel() {
            vm.Close();
        }
    }

    export function init() {
        $(() => {
            var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
            var datamarts = (<any>(window.options)).parameters.DataMarts;
            var bindingControl = $("#Content");
            vm = new ViewModel(datamarts, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}