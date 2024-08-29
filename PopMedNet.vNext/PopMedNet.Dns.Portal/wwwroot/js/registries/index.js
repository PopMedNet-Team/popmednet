import * as Global from "../../scripts/page/global.js";
import { Users } from '../Lpp.Dns.WebApi.js';
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import * as Enums from "../Dns.Enums.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";
export class ViewModel extends Global.PageViewModel {
    ds;
    dsSetting;
    gHeight = ko.observable("400px");
    CanCreateNew;
    DataUpdateFrequenciesTranslation = () => Enums.RegistryTypesTranslation;
    constructor(gRegistriesSettings, bindingControl, screenPermissions) {
        super(bindingControl, screenPermissions);
        let dsgroupSettings = gRegistriesSettings.filter((item) => { return item.Key === "Registries.Index.gRegistries.User:" + Global.User.ID; });
        this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
        this.ds = new PMNGridDataSource(Global.Helpers.GetServiceUrl("/registries/list?$select=ID,Name,Type"), { field: "Name", dir: "asc" }, (e) => {
            this.gHeight(PMNGridDataSource.ResizeGridFromResults(e, 1100, 400) + "px");
        }, kendoModelDTO);
        this.CanCreateNew = this.HasPermission(PMNPermissions.Portal.CreateRegistry);
    }
    btnNewRegistry_Click() {
        window.location.href = '/registries/details';
    }
    RegistriesGrid() {
        return $("#gRegistries").data("kendoGrid");
    }
    NameAnchor(dataItem) {
        return "<a href=\"/registries/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }
    typeFilterUI(element) {
        element.kendoDropDownList({
            dataSource: Dns.Enums.RegistryTypesTranslation,
            optionLabel: '--Select Value--',
            dataTextField: 'text',
            dataValueField: 'value'
        });
    }
}
var kendoModelDTO = {
    id: 'ID',
    fields: {
        'ID': { type: 'default', nullable: true },
        'Timestamp': { type: 'default', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Type': { type: 'number', nullable: false }
    }
};
$.when(UserSettingHelper.GetSettings(["Registries.Index.gRegistries.User:" + Global.User.ID]), Users.GetGlobalPermission(PMNPermissions.Portal.CreateRegistry)).done((gRegistriesSetting, canAdd) => {
    $(() => {
        var bindingControl = $("#Content");
        let vm = new ViewModel(gRegistriesSetting, bindingControl, canAdd ? [PMNPermissions.Portal.CreateRegistry] : []);
        ko.applyBindings(vm, bindingControl[0]);
    });
});
//# sourceMappingURL=index.js.map