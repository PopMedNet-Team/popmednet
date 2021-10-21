/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Projects;
(function (Projects) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gProjectsSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                var dsgroupSettings = gProjectsSetting.filter(function (item) { return item.Key === "Projects.Index.gResults.User:" + User.ID; });
                _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
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
                return _this;
            }
            ViewModel.prototype.btnNewProject_Click = function () {
                window.location.href = "/projects/details";
            };
            ViewModel.prototype.ProjectsGrid = function () {
                return $("#gProjects").data("kendoGrid");
            };
            ViewModel.prototype.Save = function () {
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
            $.when(Users.GetSettings(["Projects.Index.gProjects.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Group.CreateProject)).done(function (gProjectsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gProjectsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Group.CreateProject] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = Projects.Index || (Projects.Index = {}));
})(Projects || (Projects = {}));
