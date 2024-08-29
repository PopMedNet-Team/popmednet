import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { IUserSettingDTO, IProjectDTO } from "../Dns.Interfaces.js";
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export class ViewModel extends Global.PageViewModel {
    public ds: kendo.data.DataSource;
    public dsSetting: IUserSettingDTO;
    public gHeight: KnockoutObservable<string> = ko.observable("400px");
    public readonly CanCreateProject: boolean;

    constructor(gProjectsSetting: IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);

        this.CanCreateProject = this.HasPermission(PMNPermissions.Group.CreateProject);
        let dsgroupSettings = gProjectsSetting.filter((item) => { return item.Key === "Projects.Index.gResults.User:" + Global.User.ID });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;

        this.ds = new PMNGridDataSource<typeof kendoModelDTO>(Global.Helpers.GetServiceUrl("/projects/list?$select=ID,Name,Group,GroupID,Description"), { field: "Name", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 500) + "px");
        }, kendoModelDTO);
    }

    public btnNewProject_Click() {
        window.location.href = "/projects/details";
    }

    public ProjectsGrid(): kendo.ui.Grid {
        return $("#gProjects").data("kendoGrid");
    }

    public Save() {
    }

    public NameAnchor(dataItem: IProjectDTO): string {
        return "<a href=\"/projects/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    public GroupAnchor(dataItem: IProjectDTO): string {
        return "<a href=\"/groups/details?ID=" + dataItem.GroupID + "\">" + dataItem.Group + "</a>";
    }
}

var kendoModelDTO: any = {
    id: 'ID',
    fields: {
        'ID': { type: 'default', nullable: true },
        'Timestamp': { type: 'default', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Group': { type: 'string', nullable: false },
        'GroupID': { type: 'default', nullable: true }
    }
};

$.when<any>(UserSettingHelper.GetSettings(["Projects.Index.gProjects.User:" + Global.User.ID]),
    Users.GetGlobalPermission(PMNPermissions.Group.CreateProject)
).done((gProjectsSetting, canAdd) => {
    $(() => {
        let bindingControl = $("#Content");
        let vm = new ViewModel(gProjectsSetting, bindingControl, canAdd ? [PMNPermissions.Group.CreateProject] : []);
        ko.applyBindings(vm, bindingControl[0]);
    });
});