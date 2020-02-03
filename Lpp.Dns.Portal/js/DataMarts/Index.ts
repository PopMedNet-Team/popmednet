/// <reference path="../_rootlayout.ts" />

module DataMarts.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;

        public onColumnMenuInit: (e: any) => void;

        constructor(gDataMartsSetting: string, bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;

            this.ds = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/datamarts/list"),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                },
                sort: { field: "Name", dir: "asc" },

            });
            Global.Helpers.SetDataSourceFromSettings(this.ds, gDataMartsSetting);       
            
            this.onColumnMenuInit = (e) => {
                var menu = e.container.find(".k-menu").data("kendoMenu");
                menu.bind("close",(e) => {

                    self.Save();
                });
            };
        }

        public btnNewDataMart_Click() {            
            window.location.href = "/datamarts/details";
        }

        public DataMartsGrid(): kendo.ui.Grid {
            return $("#gDataMarts").data("kendoGrid");
        }

        public Save() {
            Users.SetSetting("DataMarts.Index.gDataMarts.User:" + User.ID, Global.Helpers.GetGridSettings(this.DataMartsGrid()));
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IRequestDTO): string {
        return "<a href=\"/datamarts/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    function init() {
        $.when<any>(Users.GetSetting("DataMarts.Index.gDataMarts.User:" + User.ID),
        Dns.WebApi.Users.GetGlobalPermission(Permissions.Organization.CreateDataMarts)).done((gDataMartsSetting, canAdd) => {
            $(() => {

                var bindingControl = $("#Content");
                vm = new ViewModel(gDataMartsSetting, bindingControl, canAdd[0] ? [Permissions.Organization.CreateDataMarts] : []);
                $(window).unload(() => vm.Save());
                ko.applyBindings(vm, bindingControl[0]);
                Global.Helpers.SetGridFromSettings(vm.DataMartsGrid(), gDataMartsSetting);
            });
        });        
    }

    init();
}