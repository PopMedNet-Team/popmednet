/// <reference path="../_rootlayout.ts" />

module Users.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;

        public onColumnMenuInit: (e: any) => void;

        constructor(gUsersSetting: string, bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;

            this.ds = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 50,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/users/list"),
                    }
                    ,
                    parameterMap: (options, transportType) => {
                        //override the parameterMap implementation to update the odata filter value for the enum type RegistryTypes. 
                        //By default the webapi paramter mapper does not know to include the enum typename in the filter value.
                        var opt = Global.Helpers.UpdateKendoGridFilterOptions(options, [{ field: 'Active', format: "{0}" }]);
                        var map = new (<any>kendo.data.transports).webapi.parameterMap(opt);
                        return map;
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                },
                sort: { field: "UserName", dir: "asc" },

            });
            Global.Helpers.SetDataSourceFromSettings(this.ds, gUsersSetting);

            this.onColumnMenuInit = (e) => {
                var menu = e.container.find(".k-menu").data("kendoMenu");
                menu.bind("close",(e) => {

                    self.Save();
                });
            };
        }

        public btnNewUser_Click() {
            window.location.href = "/users/details?OrganizationID=" + User.EmployerID;
        }

        public UsersGrid(): kendo.ui.Grid {
            return $("#gUsers").data("kendoGrid");
        }

        public Save() {
            Users.SetSetting("Users.Index.gUsers.User:" + User.ID, Global.Helpers.GetGridSettings(this.UsersGrid()));
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IUserDTO): string {
        return "<a href=\"/users/details?ID=" + dataItem.ID + "\">" + dataItem.UserName + "</a>";
    }

    export var UserTypesTranslation: Dns.Structures.KeyValuePair[] = [
        { value: true, text: 'Active' },
        { value: false, text: 'Inactive' },
    ];

    export function typeFilterUI(element: any) {
        element.kendoDropDownList({
            dataSource: UserTypesTranslation,
            optionLabel: '--Select Value--',
            dataTextField: 'text',
            dataValueField: 'value'
        });
    }

    export function ActiveTemplate(dataItem: Dns.Interfaces.IUserDTO): string {
        return dataItem.Active ? "Active" : "Inactive";
    }

    function init() {
        $.when<any>(Users.GetSetting("Users.Index.gUsers.User:" + User.ID),
        Dns.WebApi.Users.GetGlobalPermission(Permissions.Organization.CreateUsers)).done((gUsersSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gUsersSetting, bindingControl, canAdd[0] ? [Permissions.Organization.CreateUsers] : []);
                ko.applyBindings(vm, bindingControl[0]);
                $(window).unload(() => vm.Save());
                Global.Helpers.SetGridFromSettings(vm.UsersGrid(), gUsersSetting);

            });
        });
    }

    init();
} 