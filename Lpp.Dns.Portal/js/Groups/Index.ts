/// <reference path="../_rootlayout.ts" />
module Groups.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;

        public onColumnMenuInit: (e: any) => void;

        constructor(gGroupsSetting: string, bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;

            this.ds = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/groups/list?$select=ID,Name"),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                },
                sort: { field: "Name", dir: "asc" },

            });
            Global.Helpers.SetDataSourceFromSettings(this.ds, gGroupsSetting);  


            this.onColumnMenuInit = (e) => {
                var menu = e.container.find(".k-menu").data("kendoMenu");
                menu.bind("close",(e) => {

                    self.Save();
                });
            };
        }

        public btnNewGroup_Click() {
            window.location.href = '/groups/details';
        }

        public GroupsGrid(): kendo.ui.Grid {
            return $("#gGroups").data("kendoGrid");
        }

        public Save() {
            Users.SetSetting("Groups.Index.gGroups.User:" + User.ID, Global.Helpers.GetGridSettings(this.GroupsGrid()));

        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IGroupDTO): string {
        return "<a href=\"/groups/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    function init() {
        $.when<any>(Users.GetSetting("Groups.Index.gGroups.User:" + User.ID),
        Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateGroup)).done((gGroupsSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gGroupsSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateGroup] : []);
                ko.applyBindings(vm, bindingControl[0]);
                $(window).unload(() => vm.Save());
                Global.Helpers.SetGridFromSettings(vm.GroupsGrid(), gGroupsSetting);
            });
        });
    }

    init();
}