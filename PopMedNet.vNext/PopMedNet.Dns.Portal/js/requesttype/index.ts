import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import { IUserSettingDTO, IRequestTypeDTO, KendoModelRequestTypeDTO } from "../Dns.Interfaces.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export class ViewModel extends Global.PageViewModel {
    public ds: PMNGridDataSource<typeof KendoModelRequestTypeDTO>;
    public dsSetting: Dns.Interfaces.IUserSettingDTO;
    public gHeight: KnockoutObservable<string> = ko.observable("400px");
    public CanCreateRequestType: boolean;

    constructor(gRequestTypesSetting: IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);

        this.CanCreateRequestType = this.HasPermission(PMNPermissions.Portal.CreateRequestType);
        let dsgroupSettings = gRequestTypesSetting.filter((item) => { return item.Key === "RequestType.Index.gRequestTypes.User:" + Global.User.ID });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;

        this.ds = new PMNGridDataSource<typeof KendoModelRequestTypeDTO>(Global.Helpers.GetServiceUrl("/requesttypes/list"), { field: "Name", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 400) + "px");
        }, KendoModelRequestTypeDTO);
    }

    public onNewRequestType() {
        window.location.href = '/requesttype/details';
    }

    public RequestTypesGrid(): kendo.ui.Grid {
        return $("#gRequestTypes").data("kendoGrid");
    }

    public Save() {
        UserSettingHelper.SetSetting("RequestType.Index.gRequestTypes.User:" + Global.User.ID, Global.Helpers.GetGridSettings(this.RequestTypesGrid()));
    }

    public NameAchor(dataItem: IRequestTypeDTO): string {
        return "<a href=\"/requesttype/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

}

$.when<any>(UserSettingHelper.GetSettings(["RequestType.Index.gRequestTypes.User:" + Global.User.ID]),
    Users.GetGlobalPermission(PMNPermissions.Portal.CreateRequestType)).done((gRequestTypesSetting, canAdd) => {
        $(() => {
            let bindingControl = $('#Content');
            let vm = new ViewModel(gRequestTypesSetting, bindingControl, canAdd ? [PMNPermissions.Portal.CreateRequestType] : []);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });