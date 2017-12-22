var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../_rootlayout.ts" />
var Groups;
(function (Groups) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gGroupsSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
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
                Global.Helpers.SetDataSourceFromSettings(_this.ds, gGroupsSetting);
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.Save();
                    });
                };
                return _this;
            }
            ViewModel.prototype.btnNewGroup_Click = function () {
                window.location.href = '/groups/details';
            };
            ViewModel.prototype.GroupsGrid = function () {
                return $("#gGroups").data("kendoGrid");
            };
            ViewModel.prototype.Save = function () {
                Users.SetSetting("Groups.Index.gGroups.User:" + User.ID, Global.Helpers.GetGridSettings(this.GroupsGrid()));
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            return "<a href=\"/groups/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAchor = NameAchor;
        function init() {
            $.when(Users.GetSetting("Groups.Index.gGroups.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateGroup)).done(function (gGroupsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gGroupsSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateGroup] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () { return vm.Save(); });
                    Global.Helpers.SetGridFromSettings(vm.GroupsGrid(), gGroupsSetting);
                });
            });
        }
        init();
    })(Index = Groups.Index || (Groups.Index = {}));
})(Groups || (Groups = {}));
//# sourceMappingURL=Index.js.map