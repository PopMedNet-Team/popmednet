import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import { IUserSettingDTO } from "../Dns.Interfaces.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export class ViewModel extends Global.PageViewModel {
    public ds: kendo.data.DataSource;
    public dsSetting: IUserSettingDTO;
    public gHeight: KnockoutObservable<string> = ko.observable<string>("400px");
	public readonly CanCreateNew: boolean;

    constructor(gDataMartsSetting: IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);

        let dsDataMartSettings = gDataMartsSetting.filter((item) => { return item.Key === "DataMarts.Index.gDataMarts.User:" + Global.User.ID });
        this.dsSetting = (dsDataMartSettings.length > 0 && dsDataMartSettings[0] !== null) ? dsDataMartSettings[0] : null;

        this.ds = new PMNGridDataSource<typeof kendoModelDTO>(Global.Helpers.GetServiceUrl("/datamarts/list?$select=ID,Name,Timestamp,Description,Acronym,OrganizationID,Organization,AdapterID,Adapter"), { field: "Name", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 400) + "px");
        }, kendoModelDTO);

		this.CanCreateNew = this.HasPermission(PMNPermissions.Organization.CreateDataMarts);
    }

    public btnNewDataMart_Click() {
        window.location.href = "/datamarts/details";
    }

    public DataMartsGrid(): kendo.ui.Grid {
        return $("#gDataMarts").data("kendoGrid");
	}

	public NameAnchor(dataItem: any): string {
		return "<a href=\"/datamarts/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }
}

var kendoModelDTO: any = {
	id: 'ID',
	fields: {
		'ID': { type: 'default', nullable: true },
		'Timestamp': { type: 'default', nullable: false },
		'Name': { type: 'string', nullable: false },
		'Description': { type: 'string', nullable: false },
		'Acronym': { type: 'string', nullable: false },
		'OrganizationID': { type: 'default', nullable: true },
		'Organization': { type: 'string', nullable: false },
		'AdapterID': { type: 'default', nullable: true },
		'Adapter': { type: 'string', nullable: false }
	}
};

$(() => {
    $.when<any>(UserSettingHelper.GetSettings(["DataMarts.Index.gDataMarts.User:" + Global.User.ID]),
        Users.GetGlobalPermission(PMNPermissions.Organization.CreateDataMarts))
        .done((gDataMartsSetting, canAdd) => {

            var bindingControl = $("#Content");
            let vm = new ViewModel(gDataMartsSetting, bindingControl, canAdd ? [PMNPermissions.Organization.CreateDataMarts] : []);
            ko.applyBindings(vm, bindingControl[0]);
        });
}); 