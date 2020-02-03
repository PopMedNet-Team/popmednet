/// <reference path="../_rootlayout.ts" />
/// <reference path="../security/aclviewmodel.ts" />
/// <reference path="../events/editeventpermissions.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Home;
(function (Home) {
    var DefaultSecurityPermissions;
    (function (DefaultSecurityPermissions) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(permissions, acls, securityGroups, events, eventAcls, fieldOptions, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Acls = ko.observableArray(acls.map(function (a) {
                    return new Dns.ViewModels.AclViewModel(a);
                }));
                _this.EventAcls = ko.observableArray(eventAcls.map(function (e) {
                    return new Dns.ViewModels.BaseEventPermissionViewModel(e);
                }));
                //Permissions
                _this.Global = new Security.Acl.AclEditViewModel(permissions.filter(function (p) {
                    return (p.Locations.length == 1 || p.Locations.indexOf(Dns.Enums.PermissionAclTypes.RequestTypes) > -1 || p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Templates) > -1) && p.Locations[0] == Dns.Enums.PermissionAclTypes.Global;
                }), securityGroups, _this.Acls, [], Dns.ViewModels.AclViewModel, "Global");
                _this.Organization = new Security.Acl.AclEditViewModel(permissions.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Organizations) > -1;
                }), securityGroups, _this.Acls, [], Dns.ViewModels.AclViewModel, "Organization");
                _this.Group = new Security.Acl.AclEditViewModel(permissions.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Groups) > -1;
                }), securityGroups, _this.Acls, [], Dns.ViewModels.AclViewModel, "Group");
                _this.Project = new Security.Acl.AclEditViewModel(permissions.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
                }), securityGroups, _this.Acls, [], Dns.ViewModels.AclViewModel, "Project");
                _this.Registry = new Security.Acl.AclEditViewModel(permissions.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Registries) > -1;
                }), securityGroups, _this.Acls, [], Dns.ViewModels.AclViewModel, "Registry");
                _this.User = new Security.Acl.AclEditViewModel(permissions.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Users) > -1;
                }), securityGroups, _this.Acls, [], Dns.ViewModels.AclViewModel, "User");
                _this.FieldOptionAcl = ko.observableArray(fieldOptions.map(function (e) {
                    return new Dns.ViewModels.AclGlobalFieldOptionViewModel(e);
                }));
                _this.FieldOptions = new Security.Acl.FieldOption.AclFieldOptionEditViewModel(fieldOptions, securityGroups, _this.FieldOptionAcl, [], Dns.ViewModels.AclGlobalFieldOptionViewModel);
                //Events
                _this.GlobalEvents = new Events.Acl.EventAclEditViewModel(events.filter(function (p) {
                    return p.Locations.length == 1 && p.Locations[0] == Dns.Enums.PermissionAclTypes.Global;
                }), securityGroups, _this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "GlobalEvents");
                _this.OrganizationEvents = new Events.Acl.EventAclEditViewModel(events.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Organizations) > -1;
                }), securityGroups, _this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "OrganizationEvents");
                _this.GroupEvents = new Events.Acl.EventAclEditViewModel(events.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Groups) > -1;
                }), securityGroups, _this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "GroupEvents");
                _this.ProjectEvents = new Events.Acl.EventAclEditViewModel(events.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
                }), securityGroups, _this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "ProjectEvents");
                _this.RegistryEvents = new Events.Acl.EventAclEditViewModel(events.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Registries) > -1;
                }), securityGroups, _this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "RegistryEvents");
                _this.UserEvents = new Events.Acl.EventAclEditViewModel(events.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Users) > -1;
                }), securityGroups, _this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "UserEvents");
                return _this;
            }
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Save = function () {
                var globalAcls = this.Acls().map(function (a) {
                    return a.toData();
                });
                var globalEventAcls = this.EventAcls().map(function (e) {
                    return e.toData();
                });
                var fieldOptionAcl = this.FieldOptionAcl().map(function (e) {
                    return e.toData();
                });
                $.when(Dns.WebApi.Security.UpdatePermissions(globalAcls), Dns.WebApi.Events.UpdateGlobalEventPermissions(globalEventAcls), Dns.WebApi.Security.UpdateFieldOptionPermissions(fieldOptionAcl)).done(function () {
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        DefaultSecurityPermissions.ViewModel = ViewModel;
        function init() {
            $.when(Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Global]), Dns.WebApi.Security.GetGlobalPermissions(), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Global]), Dns.WebApi.Events.GetGlobalEventPermissions(), Dns.WebApi.Security.GetGlobalFieldOptionPermissions())
                .done(function (permissions, acls, securityGroups, events, eventAcls, fieldOptions) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(permissions, acls, securityGroups, events, eventAcls, fieldOptions, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(DefaultSecurityPermissions = Home.DefaultSecurityPermissions || (Home.DefaultSecurityPermissions = {}));
})(Home || (Home = {}));
//# sourceMappingURL=DefaultSecurityPermissions.js.map