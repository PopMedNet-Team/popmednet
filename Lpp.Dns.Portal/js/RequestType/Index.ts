/// <reference path="../_rootlayout.ts" />
module RequestType.Index {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ds: kendo.data.DataSource;
        public dsSetting: Dns.Interfaces.IUserSettingDTO;
        constructor(gRequestTypesSetting: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery, screenPermissions: any[]) {
            super(bindingControl, screenPermissions);
            var self = this;
            let dsgroupSettings = gRequestTypesSetting.filter((item) => { return item.Key === "RequestType.Index.gRequestTypes.User:" + User.ID });
            this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null
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
        return "<a href=\"/requesttype/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    }

    function init() {
        $.when<any>(Users.GetSettings(["RequestType.Index.gRequestTypes.User:" + User.ID]),
        Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateRequestType)).done((gRequestTypesSetting, canAdd) => {
            $(() => {
                var bindingControl = $('#Content');
                vm = new ViewModel(gRequestTypesSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateRequestType] : []);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
    }

    init();
}