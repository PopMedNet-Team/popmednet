/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
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
                Global.Helpers.SetDataSourceFromSettings(_this.ds, gOrganizationsSetting);
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
            $.when(Users.GetSetting("Organizations.Index.gOrganizations.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateOrganization)).done(function (gOrganizationsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gOrganizationsSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateOrganization] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                    Global.Helpers.SetGridFromSettings(vm.OrganizationsGrid(), gOrganizationsSetting);
                    vm.OrganizationsGrid().bind("dataBound", function (e) {
                        Users.SetSetting("Organizations.Index.gOrganizations.User:" + User.ID, Global.Helpers.GetGridSettings(vm.OrganizationsGrid()));
                    });
                    vm.OrganizationsGrid().bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
                });
            });
        }
        init();
    })(Index = Organizations.Index || (Organizations.Index = {}));
})(Organizations || (Organizations = {}));
