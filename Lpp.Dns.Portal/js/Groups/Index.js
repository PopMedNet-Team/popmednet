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
/// <reference path="../_rootlayout.ts" />
var Groups;
(function (Groups) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gGroupsSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                var dsgroupSettings = gGroupsSetting.filter(function (item) { return item.Key === "Groups.Index.gResults.User:" + User.ID; });
                _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
                _this.ds = new kendo.data.DataSource({
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
                return _this;
            }
            ViewModel.prototype.btnNewGroup_Click = function () {
                window.location.href = '/groups/details';
            };
            ViewModel.prototype.GroupsGrid = function () {
                return $("#gGroups").data("kendoGrid");
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            return "<a href=\"/groups/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAchor = NameAchor;
        function init() {
            $.when(Users.GetSettings(["Groups.Index.gGroups.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateGroup)).done(function (gGroupsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gGroupsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateGroup] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = Groups.Index || (Groups.Index = {}));
})(Groups || (Groups = {}));
