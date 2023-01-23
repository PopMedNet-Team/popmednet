/// <reference path="../_rootlayout.ts" />

module DataMarts.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;
        public dsSetting: Dns.Interfaces.IUserSettingDTO;

        constructor(gDataMartsSetting: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;

            let dsDataMartSettings = gDataMartsSetting.filter((item) => { return item.Key === "DataMarts.Index.gResults.User:" + User.ID });
            this.dsSetting = (dsDataMartSettings.length > 0 && dsDataMartSettings[0] !== null) ? dsDataMartSettings[0] : null
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

        }

        public btnNewDataMart_Click() {            
            window.location.href = "/datamarts/details";
        }

        public DataMartsGrid(): kendo.ui.Grid {
            return $("#gDataMarts").data("kendoGrid");
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IRequestDTO): string {
        return "<a href=\"/datamarts/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    export function OrgAnchor(dataItem: Dns.Interfaces.IRequestDTO): string {
        return "<a href=\"/organizations/details?ID=" + dataItem.OrganizationID + "\">" + dataItem.Organization + "</a>";
    }

    function init() {
        $.when<any>(Users.GetSettings(["DataMarts.Index.gDataMarts.User:" + User.ID]),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.CreateDataMarts)).done((gDataMartsSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gDataMartsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Organization.CreateDataMarts] : []);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });        
    }

    init();
}