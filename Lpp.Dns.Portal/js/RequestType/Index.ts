/// <reference path="../_rootlayout.ts" />
module RequestType.Index {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;

        constructor(gRequestTypesSetting: string, bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;

            this.ds = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/requesttypes/list"),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelRequestTypeDTO)
                },
                sort: { field: "Name", dir: "asc" },

            });
            Global.Helpers.SetDataSourceFromSettings(this.ds, gRequestTypesSetting);
        }
        
        public onNewRequestType() {
            window.location.href = '/requesttype/details';
        }

        public RequestTypesGrid(): kendo.ui.Grid {
            return $("#gRequestTypes").data("kendoGrid");
        }

        public Save() {
            Users.SetSetting("RequestType.Index.gRequestTypes.User:" + User.ID, Global.Helpers.GetGridSettings(this.RequestTypesGrid()));
        }

    }

    export function NameAchor(dataItem: Dns.Interfaces.IRequestTypeDTO): string {
        if (!dataItem.TemplateID || dataItem.TemplateID == '00000000-0000-0000-0000-000000000000') {
            return dataItem.Name;
        } else {
            return "<a href=\"/requesttype/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
    }

    function init() {
        $.when<any>(Users.GetSetting("RequestType.Index.gRequestTypes.User:" + User.ID),
        Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateRequestType)).done((gRequestTypesSetting, canAdd) => {
            $(() => {
                var bindingControl = $('#Content');
                vm = new ViewModel(gRequestTypesSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateRequestType] : []);
                ko.applyBindings(vm, bindingControl[0]);
                Global.Helpers.SetGridFromSettings(vm.RequestTypesGrid(), gRequestTypesSetting);
                vm.RequestTypesGrid().bind("dataBound", function (e) {
                  Users.SetSetting("RequestType.Index.gRequestTypes.User:" + User.ID, Global.Helpers.GetGridSettings(vm.RequestTypesGrid()));
                });
                vm.RequestTypesGrid().bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
            });
        });
    }

    init();
}