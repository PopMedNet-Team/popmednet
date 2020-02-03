/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />

module Requests.Create {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {

        public RequestTypes: Dns.Interfaces.IRequestTypeDTO[];

        public onSelectRequestType : (requestType: Dns.Interfaces.IRequestTypeDTO) => void;

        constructor(projectID: any, requestTypes: Dns.Interfaces.IRequestTypeDTO[], bindingControl: JQuery) {
            super(bindingControl);   

            var self = this;
            self.RequestTypes = requestTypes;
            this.onSelectRequestType = (requestType: Dns.Interfaces.IRequestTypeDTO) => {
                self.Close(requestType);
            };
        }

    }

    export function init() {

        var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        var projectID = (<any>(window.options)).parameters.ProjectID || null;
        if (projectID === undefined || projectID == '')
            projectID = Constants.GuidEmpty;

        
        Dns.WebApi.Projects.GetAvailableRequestTypeForNewRequest(projectID, null, null, "Name").done((results) => {
            var bindingControl = $('#Content');
            vm = new ViewModel(projectID, results, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();

    export interface IRequestTypeData {
        ID: string;
        Name: string;
        Description: string;
    }
} 