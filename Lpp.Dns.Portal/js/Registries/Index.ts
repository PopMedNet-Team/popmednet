/// <reference path="../_rootlayout.ts" />
module Registries.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;
        public dsSetting: Dns.Interfaces.IUserSettingDTO;
        constructor(gRegistriesSettings: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);    
            var self = this;
            let dsgroupSettings = gRegistriesSettings.filter((item) => { return item.Key === "Registries.Index.gRegistries.User:" + User.ID });
            this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null
            this.ds = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/registries/list"),
                    }
                    ,
                    parameterMap: (options, transportType) => {
                        //override the parameterMap implementation to update the odata filter value for the enum type RegistryTypes. 
                        //By default the webapi paramter mapper does not know to include the enum typename in the filter value.
                        var opt = Global.Helpers.UpdateKendoGridFilterOptions(options, [{ field: 'Type', format: "Lpp.Dns.DTO.Enums.RegistryTypes'{0}'" }]);
                        var map = new (<any>kendo.data.transports).webapi.parameterMap(opt);
                        return map;
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelRegistryDTO)
                },
                sort: { field: "Name", dir: "asc" },

            });
        }

        public btnNewRegistry_Click() {
            window.location.href = "/registries/details";
        }

        public RegistriesGrid(): kendo.ui.Grid {
            return $("#gRegistries").data("kendoGrid");
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IRegistryDTO): string {
        return "<a href=\"/registries/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    export function typeFilterUI(element: any) {
        element.kendoDropDownList({
            dataSource: Dns.Enums.RegistryTypesTranslation,
            optionLabel: '--Select Value--',
            dataTextField: 'text',
            dataValueField: 'value'
        });
    }

    function init() {
        $.when<any>(Users.GetSettings(["Registries.Index.gRegistries.User:" + User.ID]),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateRegistry)).done((gRegistriesSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gRegistriesSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateRegistry] : []);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
    }

    init();
}