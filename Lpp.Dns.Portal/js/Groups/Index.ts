/// <reference path="../_rootlayout.ts" />
module Groups.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;
        public dsSetting: Dns.Interfaces.IUserSettingDTO;

        constructor(gGroupsSetting: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;
            let dsgroupSettings = gGroupsSetting.filter((item) => { return item.Key === "Groups.Index.gResults.User:" + User.ID });
            this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null
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

        }

        public btnNewGroup_Click() {
            window.location.href = '/groups/details';
        }

        public GroupsGrid(): kendo.ui.Grid {
            return $("#gGroups").data("kendoGrid");
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IGroupDTO): string {
        return "<a href=\"/groups/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    function init() {
        $.when<any>(Users.GetSettings(["Groups.Index.gGroups.User:" + User.ID]),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateGroup)).done((gGroupsSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gGroupsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateGroup] : []);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
    }

    init();
}