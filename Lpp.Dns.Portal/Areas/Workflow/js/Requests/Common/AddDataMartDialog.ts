/// <reference path="../../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="listroutings.ts" />



module Workflow.Common.AddDataMartDialog {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {

        public RecievedDataMarts: Dns.Interfaces.IRequestDataMartDTO[];//current routings

        public AllUnfilteredDataMarts: Dns.Interfaces.IDataMartListDTO[];//passed filtered DMs

        //public DataMarts: KnockoutObservableArray<Dns.Interfaces.IDataMartListDTO[]>;

        public SelectedDataMartIDs: KnockoutObservableArray<string>;

        constructor(
            bindingControl: JQuery,
            currentRoutings: Dns.Interfaces.IRequestDataMartDTO[],
            allDataMarts: Dns.Interfaces.IDataMartListDTO[],
            projectID: any
            ) {
            super(bindingControl);
            var self = this;

            this.RecievedDataMarts = currentRoutings;
            this.AllUnfilteredDataMarts = allDataMarts;

            this.SelectedDataMartIDs = ko.observableArray([]);

        }

        public onContinue() {
            vm.Close(this.SelectedDataMartIDs());
        }

        public onCancel() {
            vm.Close(null);
        }
    }

    export function init() {
        $(() => {
            var projectID: any = Global.GetQueryParam("projectID")
            var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
            var parameters = (<any>(window.options)).parameters;        
            var currentRoutings = <Dns.Interfaces.IRequestDataMartDTO[]>(parameters.CurrentRoutings || null);
            var allDataMarts = <Dns.Interfaces.IDataMartListDTO[]>(parameters.AllDataMarts || null);
            $.when<any>(

                )
                .done((datamarts: Dns.Interfaces.IDataMartListDTO[]) => {

                    var bindingControl = $("#AddDataMartDialog");
                    vm = new ViewModel(bindingControl, currentRoutings, allDataMarts, projectID);
                $(() => {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        });
    }

    init();
} 