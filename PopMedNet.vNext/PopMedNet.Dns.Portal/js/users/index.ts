import * as Global from "../../scripts/page/global.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';
import { IUserSettingDTO, IUserDTO, KendoModelUserDTO } from "../Dns.Interfaces.js";
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import KeyValuePair from "../Dns.Structures.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export default class ViewModel extends Global.PageViewModel {
    public ds: kendo.data.DataSource;
    public dsSetting: IUserSettingDTO;
    public gHeight: KnockoutObservable<string> = ko.observable("400px");

    public UserTypesTranslation: KeyValuePair[] = [
        { value: true, text: 'Active' },
        { value: false, text: 'Inactive' },
    ];

    readonly CanCreateUser: boolean;

    constructor(gUsersSetting: IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);

        this.CanCreateUser = this.HasPermission(PMNPermissions.Organization.CreateUsers);

        let dsgroupSettings = gUsersSetting.filter((item) => { return item.Key === "Users.Index.gUsers.User:" + Global.User.ID });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;       

        this.ds = new PMNGridDataSource<typeof KendoModelUserDTO>(Global.Helpers.GetServiceUrl("/users/list"), { field: "UserName", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 400) + "px");
        }, KendoModelUserDTO);
    }

    public btnNewUser_Click() {
        window.location.href = "/users/details?OrganizationID=" + Global.User.EmployerID;
    }

    public UsersGrid(): kendo.ui.Grid {
        return $("#gUsers").data("kendoGrid");
    }

    public NameAchor(dataItem: IUserDTO): string {
        return "<a href=\"/users/details?ID=" + dataItem.ID + "\">" + dataItem.UserName + "</a>";
    }

    public typeFilterUI(element: any) {
        element.kendoDropDownList({
            dataSource: this.UserTypesTranslation,
            optionLabel: '--Select Value--',
            dataTextField: 'text',
            dataValueField: 'value'
        });
    }

    public ActiveTemplate(dataItem: IUserDTO): string {
        return dataItem.Active ? "Active" : "Inactive";
    }
}

$.when<any>(UserSettingHelper.GetSettings(["Users.Index.gUsers.User:" + Global.User.ID]),
    WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.CreateUsers)).done((gUsersSetting, canAdd) => {
        $(() => {
            let bindingControl = $("#Content");
            let vm = new ViewModel(gUsersSetting, bindingControl, canAdd ? [PMNPermissions.Organization.CreateUsers] : []);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });