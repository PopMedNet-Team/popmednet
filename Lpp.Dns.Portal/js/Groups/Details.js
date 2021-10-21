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
var Groups;
(function (Groups) {
    var Details;
    (function (Details) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, group, allOrganizations, organizationGroups, groupProjects, events, permissionList, groupPermissions, securityGroupTree, eventList, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                _this.Group = new Dns.ViewModels.GroupViewModel(group);
                _this.AllOrganizations = ko.observableArray(allOrganizations.map(function (item) {
                    return new Dns.ViewModels.OrganizationViewModel(item);
                }));
                //this.OrganizationGroups = ko.observableArray(organizationGroups.map((item) => {
                //    return new Dns.ViewModels.OrganizationGroupViewModel(item);
                //}));
                _this.OrganizationGroups = ko.observableArray(ko.utils.arrayMap(organizationGroups, function (item) { return new Dns.ViewModels.OrganizationGroupViewModel(item); }));
                _this.AddableOrganizationList = ko.computed(function () {
                    var results = self.AllOrganizations().filter(function (o) {
                        var exists = false;
                        self.OrganizationGroups().forEach(function (og) {
                            if (og.OrganizationID() == o.ID()) {
                                exists = true;
                                return;
                            }
                        });
                        return !exists;
                    });
                    return results;
                });
                _this.GroupProjects = ko.observableArray(ko.utils.arrayMap(groupProjects, function (item) { return new Dns.ViewModels.ProjectViewModel(item); }));
                _this.GroupAcls = ko.observableArray(ko.utils.arrayMap(groupPermissions, function (item) { return new Dns.ViewModels.AclGroupViewModel(item); }));
                _this.GroupSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, _this.GroupAcls, [
                    {
                        Field: "GroupID",
                        Value: _this.Group.ID()
                    }
                ], Dns.ViewModels.AclGroupViewModel);
                _this.WatchTitle(_this.Group.Name, "Group: ");
                //Events
                _this.GroupEvents = ko.observableArray(events != null ? events.map(function (e) {
                    return new Dns.ViewModels.GroupEventViewModel(e);
                }) : null);
                _this.Events = new Events.Acl.EventAclEditViewModel(eventList, securityGroupTree, _this.GroupEvents, [{ Field: "GroupID", Value: _this.Group.ID() }], Dns.ViewModels.GroupEventViewModel);
                return _this;
            }
            ViewModel.prototype.AddOrganization = function (o) {
                vm.OrganizationGroups.push(new Dns.ViewModels.OrganizationGroupViewModel({
                    Organization: o.Name(),
                    OrganizationID: o.ID(),
                    Group: vm.Group.Name(),
                    GroupID: vm.Group.ID()
                }));
            };
            ViewModel.prototype.RemoveOrganization = function (o) {
                Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the group?</p>").done(function () {
                    if (o.GroupID()) {
                        Dns.WebApi.OrganizationGroups.Remove([o.toData()]).done(function () {
                            vm.OrganizationGroups.remove(o);
                        });
                    }
                    else {
                        vm.OrganizationGroups.remove(o);
                    }
                });
            };
            ViewModel.prototype.CreateProject = function () {
                window.location.href = "/projects/details?GroupID=" + this.Group.ID();
            };
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!_super.prototype.Validate.call(this))
                    return;
                Dns.WebApi.Groups.InsertOrUpdate([this.Group.toData()]).done(function (group) {
                    //Update the values for the ID and timestamp as necessary.
                    _this.Group.ID(group[0].ID);
                    _this.Group.Timestamp(group[0].Timestamp);
                    // Save everything else
                    var organizations = _this.OrganizationGroups().map(function (o) {
                        o.GroupID(_this.Group.ID());
                        return o.toData();
                    });
                    var groupAcls = _this.GroupAcls().map(function (a) {
                        a.GroupID(_this.Group.ID());
                        return a.toData();
                    });
                    var groupEvents = _this.GroupEvents().map(function (e) {
                        e.GroupID(_this.Group.ID());
                        return e.toData();
                    });
                    $.when(Dns.WebApi.Security.UpdateGroupPermissions(groupAcls), Dns.WebApi.Events.UpdateGroupEventPermissions(groupEvents), Dns.WebApi.OrganizationGroups.InsertOrUpdate(organizations)).done(function () {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Group?</p>").done(function () {
                    Dns.WebApi.Groups.Delete([vm.Group.ID()]).done(function () {
                        window.history.back();
                    });
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            $.when(id == null ? null : Dns.WebApi.Groups.GetPermissions([id], [PMNPermissions.Group.Delete, PMNPermissions.Group.Edit, PMNPermissions.Group.ManageSecurity, PMNPermissions.Group.CreateProject]), id == null ? null : Dns.WebApi.Groups.Get(id), Dns.WebApi.Organizations.List(null, null, "Name"), id == null ? null : Dns.WebApi.OrganizationGroups.List("GroupID eq " + id), id == null ? null : Dns.WebApi.Projects.List("GroupID eq " + id), id == null ? null : Dns.WebApi.Events.GetGroupEventPermissions(id), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Groups]), Dns.WebApi.Security.GetGroupPermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Groups])).done(function (screenPermissions, groups, allOrganizations, organizationGroups, groupProjects, events, permissionList, groupPermission, securityGroupTree, eventList) {
                var group = groups == null ? null : groups[0];
                screenPermissions = id == null ? [PMNPermissions.Group.Delete, PMNPermissions.Group.Edit, PMNPermissions.Group.ManageSecurity, PMNPermissions.Group.CreateProject] : screenPermissions;
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions, group, allOrganizations, organizationGroups, groupProjects, events, permissionList, groupPermission, securityGroupTree, eventList, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Details = Groups.Details || (Groups.Details = {}));
})(Groups || (Groups = {}));
