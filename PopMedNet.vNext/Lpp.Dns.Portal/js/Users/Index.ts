/// <reference path="../_rootlayout.ts" />

module Users.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;
        public dsSetting: Dns.Interfaces.IUserSettingDTO;
        constructor(gUsersSetting: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;
            let dsgroupSettings = gUsersSetting.filter((item) => { return item.Key === "Users.Index.gUsers.User:" + User.ID });
            this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null
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
        $.when<any>(Users.GetSettings(["Users.Index.gUsers.User:" + User.ID]),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.CreateUsers)).done((gUsersSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gUsersSetting, bindingControl, canAdd[0] ? [PMNPermissions.Organization.CreateUsers] : []);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
    }

    init();
} 