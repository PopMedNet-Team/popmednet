import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as Constants from "../../scripts/page/constants.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';

export class ViewModel extends Global.DialogViewModel {

    public RequestTypes: Interfaces.IRequestTypeDTO[];

    public onSelectRequestType: (requestType: Interfaces.IRequestTypeDTO) => void;

    constructor(projectID: any, requestTypes: Interfaces.IRequestTypeDTO[], bindingControl: JQuery) {
        super(bindingControl);

        let self = this;
        self.RequestTypes = requestTypes;
        this.onSelectRequestType = (requestType: Interfaces.IRequestTypeDTO) => {
            self.Close(requestType);
        };
    }

}

function init() {

    let window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
    let projectID = (<any>(window.options)).parameters.ProjectID || null;
    if (projectID === undefined || projectID == '')
        projectID = Constants.GuidEmpty;

    WebApi.Projects.GetAvailableRequestTypeForNewRequest(projectID, null, null, "Name").done((results) => {
        let bindingControl = $('#Content');
        let vm = new ViewModel(projectID, results, bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    });
}

init();

export interface IRequestTypeData {
    ID: string;
    Name: string;
    Description: string;
}