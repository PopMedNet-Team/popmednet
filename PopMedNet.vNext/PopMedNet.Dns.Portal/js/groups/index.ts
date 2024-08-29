import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import { IUserSettingDTO, IGroupDTO } from "../Dns.Interfaces.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export default class ViewModel extends Global.PageViewModel {
    public ds: kendo.data.DataSource;
    public dsSetting: Dns.Interfaces.IUserSettingDTO;
    public readonly CanCreateNew: boolean;

    constructor(gGroupsSetting: IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);
        let dsgroupSettings = gGroupsSetting.filter((item) => { return item.Key === "Groups.Index.gGroups.User:" + Global.User.ID });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;

        this.ds = new PMNGridDataSource<typeof kendoModelDTO>(Global.Helpers.GetServiceUrl("/groups/list?$select=ID,Name"), { field: "Name", dir: "asc" }, null, kendoModelDTO);

        this.CanCreateNew = this.HasPermission(PMNPermissions.Portal.CreateGroup);

    }

    public btnNewGroup_Click() {
        window.location.href = '/groups/details';
    }

    public GroupsGrid(): kendo.ui.Grid {
        return $("#gGroups").data("kendoGrid");
    }

    public NameAnchor(dataItem: IGroupDTO): string {
        return "<a href=\"/groups/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }
}

var kendoModelDTO: any = {
    id: 'ID',
    fields: {
        'ID': { type: 'default', nullable: true },
        'Timestamp': { type: 'default', nullable: false },
        'Name': { type: 'string', nullable: false }        
    }
};

$.when<any>(UserSettingHelper.GetSettings(["Groups.Index.gGroups.User:" + Global.User.ID]),
    Users.GetGlobalPermission(PMNPermissions.Portal.CreateGroup)).done((gGroupsSetting, canAdd) => {
        $(() => {
            var bindingControl = $("#Content");
            let vm = new ViewModel(gGroupsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateGroup] : []);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });