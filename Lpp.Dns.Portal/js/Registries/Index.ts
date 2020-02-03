/// <reference path="../_rootlayout.ts" />
module Registries.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;

        constructor(gRegistriesSettings: string, bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);    
            var self = this;
                    
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
            Global.Helpers.SetDataSourceFromSettings(this.ds, gRegistriesSettings);
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
        $.when<any>(Users.GetSetting("Registries.Index.gRegistries.User:" + User.ID),
        Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateRegistry)).done((gRegistriesSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gRegistriesSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateRegistry] : []);
                ko.applyBindings(vm, bindingControl[0]);
                Global.Helpers.SetGridFromSettings(vm.RegistriesGrid(), gRegistriesSetting);
                vm.RegistriesGrid().bind("dataBound", function (e) {
                  Users.SetSetting("Registries.Index.gRegistries.User:" + User.ID, Global.Helpers.GetGridSettings(vm.RegistriesGrid()));
                });
                vm.RegistriesGrid().bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
            });
        });
    }

    init();
}