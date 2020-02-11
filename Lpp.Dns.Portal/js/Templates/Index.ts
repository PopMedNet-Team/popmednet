module Templates.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;

        constructor(gTemplatesSetting: string, bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;

            this.ds = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/templates/criteriagroups"),
                    },
                    parameterMap: (options, transportType) => {
                        //override the parameterMap implementation to update the odata filter value for the enum type TemplateTypes. 
                        //By default the webapi paramter mapper does not know to include the enum typename in the filter value.
                        var opt = Global.Helpers.UpdateKendoGridFilterOptions(options, [{ field: 'Type', format: "Lpp.Dns.DTO.Enums.TemplateTypes'{0}'" }]);
                        var map = new (<any>kendo.data.transports).webapi.parameterMap(opt);
                        return map;
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelTemplateDTO)
                },
                sort: { field: "Name", dir: "asc" },

            });
            Global.Helpers.SetDataSourceFromSettings(this.ds, gTemplatesSetting);
        }

        public TemplatesGrid(): kendo.ui.Grid {
            return $("#gTemplates").data("kendoGrid");
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IRegistryDTO): string {
        return "<a href=\"/templates/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    export function typeFilterUI(element: any) {
        element.kendoDropDownList({
            dataSource: Dns.Enums.TemplateTypesTranslation,
            optionLabel: '--Select Value--',
            dataTextField: 'text',
            dataValueField: 'value'
        });
    }

    function init() {
        //TODO: get the screen permissions and then bind the screen
        $.when<any>(Users.GetSetting("Templates.Index.gTemplates.User:" + User.ID),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateTemplate)).done((gTemplatesSetting, canAdd) => {
            $(() => {
                var bindingControl = $('#Content');
                vm = new ViewModel(gTemplatesSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateTemplate] : []);
                ko.applyBindings(vm, bindingControl[0]);
                Global.Helpers.SetGridFromSettings(vm.TemplatesGrid(), gTemplatesSetting);
                vm.TemplatesGrid().bind("dataBound", function (e) {
                  Users.SetSetting("Templates.Index.gTemplates.User:" + User.ID, Global.Helpers.GetGridSettings(vm.TemplatesGrid()));
                });
                vm.TemplatesGrid().bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
            });
        });
    }

    init();
}