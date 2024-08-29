/// <reference path="../_rootlayout.ts" />

module Organizations.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;
        public dsSetting: Dns.Interfaces.IUserSettingDTO;
        constructor(gOrganizationsSetting: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;
            let dsgroupSettings = gOrganizationsSetting.filter((item) => { return item.Key === "Organizations.Index.gOrganizations.User:" + User.ID });
            this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null
            this.ds = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/organizations/list"),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                },
                sort: { field: "Name", dir: "asc" },

            });
        }
            
        public btnNewOrganization_Click() {
            window.location.href = '/organizations/details'
        }

        public OrganizationsGrid(): kendo.ui.Grid {
            return $("#gOrganizations").data("kendoGrid");
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IOrganizationDTO): string {
        return "<a href=\"/organizations/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }
    export function ParentNameAchor(dataItem: Dns.Interfaces.IOrganizationDTO): string {
        if (dataItem.ParentOrganization != null) {
            return "<a href=\"/organizations/details?ID=" + dataItem.ParentOrganizationID + "\">" + dataItem.ParentOrganization + "</a>";
        }
        else {
            return ''
        }
        
    }

    function init() {
        $.when<any>(Users.GetSettings(["Organizations.Index.gOrganizations.User:" + User.ID]),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateOrganization)).done((gOrganizationsSetting, canAdd) => {
            $(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(gOrganizationsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateOrganization] : []);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
    }

    init();
} 