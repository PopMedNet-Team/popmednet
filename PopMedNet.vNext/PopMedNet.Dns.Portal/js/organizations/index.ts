import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import * as Interfaces from "../Dns.Interfaces.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export class ViewModel extends Global.PageViewModel {
    public ds: PMNGridDataSource<typeof kendoModelDTO>;
    public dsSetting: Interfaces.IUserSettingDTO;
    public gHeight: KnockoutObservable<string> = ko.observable("400px");
    readonly CanCreateOrganization: boolean;

    constructor(gOrganizationsSetting: Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);
        
        let dsgroupSettings = gOrganizationsSetting.filter((item) => { return item.Key === "Organizations.Index.gOrganizations.User:" + Global.User.ID });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;

        this.ds = new PMNGridDataSource<typeof kendoModelDTO>(Global.Helpers.GetServiceUrl("/organizations/list?$select=ID,Name,Acronym,ParentOrganization"), { field: "Name", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 400) + "px");
        }, kendoModelDTO);

        this.CanCreateOrganization = this.HasPermission(PMNPermissions.Portal.CreateOrganization);
    }

    public btnNewOrganization_Click() {
        window.location.href = '/organizations/details'
    }

    public OrganizationsGrid(): kendo.ui.Grid {
        return $("#gOrganizations").data("kendoGrid");
    }

    public NameAchor(dataItem: Interfaces.IOrganizationDTO): string {
        return "<a href=\"/organizations/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    public ParentNameAchor(dataItem: Interfaces.IOrganizationDTO): string {
        if (dataItem.ParentOrganization != null) {
            return "<a href=\"/organizations/details?ID=" + dataItem.ParentOrganizationID + "\">" + dataItem.ParentOrganization + "</a>";
        }
        else {
            return '';
        }

    }
}

let kendoModelDTO: any = {
    id: 'ID',
    fields: {
        'ID': { type: 'default', nullable: true },
        'Name': { type: 'string', nullable: false },
        'Acronym': { type: 'string', nullable: true },
        'ParentOrganization': { type: 'string', nullable: true }
    }
};

$.when<any>(UserSettingHelper.GetSettings(["Organizations.Index.gOrganizations.User:" + Global.User.ID]),
    Users.GetGlobalPermission(PMNPermissions.Portal.CreateOrganization)).done((gOrganizationsSetting, canAdd) => {
        $(() => {
            let bindingControl = $("#Content");
            let vm = new ViewModel(gOrganizationsSetting, bindingControl, canAdd ? [PMNPermissions.Portal.CreateOrganization] : []);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });

