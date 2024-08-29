import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import * as Interfaces from "../Dns.Interfaces.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export class ViewModel extends Global.PageViewModel {
    public ds: PMNGridDataSource<typeof Interfaces.KendoModelTemplateDTO>;
    public dsSetting: Interfaces.IUserSettingDTO;
    public gHeight: KnockoutObservable<string> = ko.observable("400px");

    constructor(gTemplatesSetting: Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);
        
        let dsgroupSettings = gTemplatesSetting.filter((item) => { return item.Key === "Templates.Index.gTemplates.User:" + Global.User.ID });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;

        this.ds = new PMNGridDataSource<typeof Interfaces.KendoModelTemplateDTO>(Global.Helpers.GetServiceUrl("/templates/criteriagroups?$select=ID,Name"), { field: "Name", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 630) + "px");
        }, Interfaces.KendoModelTemplateDTO);

    }

    public TemplatesGrid(): kendo.ui.Grid {
        return $("#gTemplates").data("kendoGrid");
    }

    public NameAchor(dataItem: Interfaces.IRegistryDTO): string {
        return "<a href=\"/templates/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }
}

$.when<any>(UserSettingHelper.GetSettings(["Templates.Index.gTemplates.User:" + Global.User.ID]),
    Users.GetGlobalPermission(PMNPermissions.Portal.CreateTemplate)).done((gTemplatesSetting, canAdd) => {
        $(() => {
            let bindingControl = $('#Content');
            let vm = new ViewModel(gTemplatesSetting, bindingControl, canAdd ? [PMNPermissions.Portal.CreateTemplate] : []);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });