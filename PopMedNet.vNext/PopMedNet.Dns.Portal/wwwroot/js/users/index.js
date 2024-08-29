import * as Global from "../../scripts/page/global.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';
import { KendoModelUserDTO } from "../Dns.Interfaces.js";
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";
export default class ViewModel extends Global.PageViewModel {
    ds;
    dsSetting;
    gHeight = ko.observable("400px");
    UserTypesTranslation = [
        { value: true, text: 'Active' },
        { value: false, text: 'Inactive' },
    ];
    CanCreateUser;
    constructor(gUsersSetting, bindingControl, screenPermissions) {
        super(bindingControl, screenPermissions);
        this.CanCreateUser = this.HasPermission(PMNPermissions.Organization.CreateUsers);
        let dsgroupSettings = gUsersSetting.filter((item) => { return item.Key === "Users.Index.gUsers.User:" + Global.User.ID; });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
        this.ds = new PMNGridDataSource(Global.Helpers.GetServiceUrl("/users/list"), { field: "UserName", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 400) + "px");
        }, KendoModelUserDTO);
    }
    btnNewUser_Click() {
        window.location.href = "/users/details?OrganizationID=" + Global.User.EmployerID;
    }
    UsersGrid() {
        return $("#gUsers").data("kendoGrid");
    }
    NameAchor(dataItem) {
        return "<a href=\"/users/details?ID=" + dataItem.ID + "\">" + dataItem.UserName + "</a>";
    }
    typeFilterUI(element) {
        element.kendoDropDownList({
            dataSource: this.UserTypesTranslation,
            optionLabel: '--Select Value--',
            dataTextField: 'text',
            dataValueField: 'value'
        });
    }
    ActiveTemplate(dataItem) {
        return dataItem.Active ? "Active" : "Inactive";
    }
}
$.when(UserSettingHelper.GetSettings(["Users.Index.gUsers.User:" + Global.User.ID]), WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.CreateUsers)).done((gUsersSetting, canAdd) => {
    $(() => {
        let bindingControl = $("#Content");
        let vm = new ViewModel(gUsersSetting, bindingControl, canAdd ? [PMNPermissions.Organization.CreateUsers] : []);
        ko.applyBindings(vm, bindingControl[0]);
    });
});
//# sourceMappingURL=index.js.map