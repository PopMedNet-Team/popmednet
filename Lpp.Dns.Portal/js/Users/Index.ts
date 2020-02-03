/// <reference path="../_rootlayout.ts" />

module Users.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;

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
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelUserDTO)
                },
                sort: { field: "UserName", dir: "asc" },

            });
            Global.Helpers.SetDataSourceFromSettings(this.ds, gUsersSetting);
        }

        public btnNewUser_Click() {
            window.location.href = "/users/details?OrganizationID=" + User.EmployerID;
        }

        public UsersGrid(): kendo.ui.Grid {
            return $("#gUsers").data("kendoGrid");
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
                Global.Helpers.SetGridFromSettings(vm.UsersGrid(), gUsersSetting);
                vm.UsersGrid().bind("dataBound", function (e) {
                  Users.SetSetting("Users.Index.gUsers.User:" + User.ID, Global.Helpers.GetGridSettings(vm.UsersGrid()));
                });
                vm.UsersGrid().bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
            });
        });
    }

    init();
} 