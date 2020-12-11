/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Organizations;
(function (Organizations) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gOrganizationsSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                var dsgroupSettings = gOrganizationsSetting.filter(function (item) { return item.Key === "Organizations.Index.gOrganizations.User:" + User.ID; });
                _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
                _this.ds = new kendo.data.DataSource({
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
                return _this;
            }
            ViewModel.prototype.btnNewOrganization_Click = function () {
                window.location.href = '/organizations/details';
            };
            ViewModel.prototype.OrganizationsGrid = function () {
                return $("#gOrganizations").data("kendoGrid");
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            return "<a href=\"/organizations/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAchor = NameAchor;
        function ParentNameAchor(dataItem) {
            if (dataItem.ParentOrganization != null) {
                return "<a href=\"/organizations/details?ID=" + dataItem.ParentOrganizationID + "\">" + dataItem.ParentOrganization + "</a>";
            }
            else {
                return '';
            }
        }
        Index.ParentNameAchor = ParentNameAchor;
        function init() {
            $.when(Users.GetSettings(["Organizations.Index.gOrganizations.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateOrganization)).done(function (gOrganizationsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gOrganizationsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateOrganization] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = Organizations.Index || (Organizations.Index = {}));
})(Organizations || (Organizations = {}));
