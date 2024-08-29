import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import { IUserSettingDTO, IRegistryDTO } from "../Dns.Interfaces.js";
import * as Enums from "../Dns.Enums.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";

export class ViewModel extends Global.PageViewModel {
    public ds: PMNGridDataSource<typeof kendoModelDTO>;
    public dsSetting: Dns.Interfaces.IUserSettingDTO;
    public gHeight: KnockoutObservable<string> = ko.observable("400px");
    public readonly CanCreateNew: boolean;
    public DataUpdateFrequenciesTranslation = () => Enums.RegistryTypesTranslation;

    constructor(gRegistriesSettings: IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
        super(bindingControl, screenPermissions);

        let dsgroupSettings = gRegistriesSettings.filter((item) => { return item.Key === "Registries.Index.gRegistries.User:" + Global.User.ID });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;

        this.ds = new PMNGridDataSource<typeof kendoModelDTO>(Global.Helpers.GetServiceUrl("/registries/list?$select=ID,Name,Type"), { field: "Name", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 400) + "px");
        }, kendoModelDTO);

        this.CanCreateNew = this.HasPermission(PMNPermissions.Portal.CreateRegistry);
    }

    public btnNewRegistry_Click() {
        window.location.href = '/registries/details';
    }

    public RegistriesGrid(): kendo.ui.Grid {
        return $("#gRegistries").data("kendoGrid");
    }

    public NameAnchor(dataItem: IRegistryDTO): string {
        return "<a href=\"/registries/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    public typeFilterUI(element: any) {
        element.kendoDropDownList({
            dataSource: Dns.Enums.RegistryTypesTranslation,
            optionLabel: '--Select Value--',
            dataTextField: 'text',
            dataValueField: 'value'
        });
    }
}

var kendoModelDTO: any = {
    id: 'ID',
    fields: {
        'ID': { type: 'default', nullable: true },
        'Timestamp': { type: 'default', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Type': { type: 'number', nullable: false}
    }
};

$.when<any>(UserSettingHelper.GetSettings(["Registries.Index.gRegistries.User:" + Global.User.ID]),
    Users.GetGlobalPermission(PMNPermissions.Portal.CreateRegistry)).done((gRegistriesSetting, canAdd) => {
        $(() => {
            var bindingControl = $("#Content");
            let vm = new ViewModel(gRegistriesSetting, bindingControl, canAdd ? [PMNPermissions.Portal.CreateRegistry] : []);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });