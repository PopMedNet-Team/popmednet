module Templates.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;
        public dsSetting: Dns.Interfaces.IUserSettingDTO;
        constructor(gTemplatesSetting: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;
            let dsgroupSettings = gTemplatesSetting.filter((item) => { return item.Key === "Templates.Index.gTemplates.User:" + User.ID });
            this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null
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
        $.when<any>(Users.GetSettings(["Templates.Index.gTemplates.User:" + User.ID]),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateTemplate)).done((gTemplatesSetting, canAdd) => {
            $(() => {
                var bindingControl = $('#Content');
                vm = new ViewModel(gTemplatesSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateTemplate] : []);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
    }

    init();
}