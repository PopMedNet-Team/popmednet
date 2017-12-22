/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Projects;
(function (Projects) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gProjectsSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                _this.ds = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/projects/list?$select=ID,Name,Group, GroupID,Description"),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                    },
                    sort: { field: "Name", dir: "asc" },
                });
                Global.Helpers.SetDataSourceFromSettings(_this.ds, gProjectsSetting);
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.Save();
                    });
                };
                return _this;
            }
            ViewModel.prototype.btnNewProject_Click = function () {
                window.location.href = "/projects/details";
            };
            ViewModel.prototype.ProjectsGrid = function () {
                return $("#gProjects").data("kendoGrid");
            };
            ViewModel.prototype.Save = function () {
                Users.SetSetting("Projects.Index.gProjects.User:" + User.ID, Global.Helpers.GetGridSettings(this.ProjectsGrid()));
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAnchor(dataItem) {
            return "<a href=\"/projects/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAnchor = NameAnchor;
        function GroupAnchor(dataItem) {
            return "<a href=\"/groups/details?ID=" + dataItem.GroupID + "\">" + dataItem.Group + "</a>";
        }
        Index.GroupAnchor = GroupAnchor;
        function init() {
            $.when(Users.GetSetting("Projects.Index.gProjects.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(Permissions.Group.CreateProject)).done(function (gProjectsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gProjectsSetting, bindingControl, canAdd[0] ? [Permissions.Group.CreateProject] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () { return vm.Save(); });
                    Global.Helpers.SetGridFromSettings(vm.ProjectsGrid(), gProjectsSetting);
                });
            });
        }
        init();
    })(Index = Projects.Index || (Projects.Index = {}));
})(Projects || (Projects = {}));
//# sourceMappingURL=Index.js.map