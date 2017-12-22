/// <reference path="../_rootlayout.ts" />
/// <reference path="../security/aclviewmodel.ts" />
/// <reference path="../events/editeventpermissions.ts" />


module Home.DefaultSecurityPermissions {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public Global: Security.Acl.AclEditViewModel<Dns.ViewModels.AclViewModel>;        
        public Organization: Security.Acl.AclEditViewModel<Dns.ViewModels.AclViewModel>;
        public Group: Security.Acl.AclEditViewModel<Dns.ViewModels.AclViewModel>;
        public Project: Security.Acl.AclEditViewModel<Dns.ViewModels.AclViewModel>;
        public Registry: Security.Acl.AclEditViewModel<Dns.ViewModels.AclViewModel>;
        public User: Security.Acl.AclEditViewModel<Dns.ViewModels.AclViewModel>;
        public FieldOptions: Security.Acl.FieldOption.AclFieldOptionEditViewModel<Dns.ViewModels.BaseFieldOptionAclViewModel>;

        public GlobalEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.BaseEventPermissionViewModel>;
        public OrganizationEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.BaseEventPermissionViewModel>;
        public GroupEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.BaseEventPermissionViewModel>;
        public ProjectEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.BaseEventPermissionViewModel>;
        public RegistryEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.BaseEventPermissionViewModel>;
        public UserEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.BaseEventPermissionViewModel>;

        public Acls: KnockoutObservableArray<Dns.ViewModels.AclViewModel>;
        public EventAcls: KnockoutObservableArray<Dns.ViewModels.BaseEventPermissionViewModel>;
        public FieldOptionAcl: KnockoutObservableArray<Dns.ViewModels.AclGlobalFieldOptionViewModel>;
         
        constructor(permissions: Dns.Interfaces.IPermissionDTO[],
            acls: Dns.Interfaces.IAclDTO[],
            securityGroups: Dns.Interfaces.ITreeItemDTO[],
            events: Dns.Interfaces.IEventDTO[],
            eventAcls: Dns.Interfaces.IBaseEventPermissionDTO[],
            fieldOptions: Dns.Interfaces.IAclGlobalFieldOptionDTO[],
            bindingControl: JQuery) {
            super(bindingControl);

            this.Acls = ko.observableArray(acls.map((a) => {
                return new Dns.ViewModels.AclViewModel(a);
            }));

            this.EventAcls = ko.observableArray(eventAcls.map((e) => {
                return new Dns.ViewModels.BaseEventPermissionViewModel(e);
            }));

            //Permissions
            this.Global = new Security.Acl.AclEditViewModel(permissions.filter((p) => {
                return (p.Locations.length == 1 || p.Locations.indexOf(Dns.Enums.PermissionAclTypes.RequestTypes) > -1 || p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Templates) > -1) && p.Locations[0] == Dns.Enums.PermissionAclTypes.Global;
            }), securityGroups, this.Acls, [], Dns.ViewModels.AclViewModel, "Global");

            this.Organization = new Security.Acl.AclEditViewModel(permissions.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Organizations) > -1;
            }), securityGroups, this.Acls, [], Dns.ViewModels.AclViewModel, "Organization");

            this.Group = new Security.Acl.AclEditViewModel(permissions.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Groups) > -1;
            }), securityGroups, this.Acls, [], Dns.ViewModels.AclViewModel, "Group");

            this.Project = new Security.Acl.AclEditViewModel(permissions.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
            }), securityGroups, this.Acls, [], Dns.ViewModels.AclViewModel, "Project");

            this.Registry = new Security.Acl.AclEditViewModel(permissions.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Registries) > -1;
            }), securityGroups, this.Acls, [], Dns.ViewModels.AclViewModel, "Registry");

            this.User = new Security.Acl.AclEditViewModel(permissions.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Users) > -1;
            }), securityGroups, this.Acls, [], Dns.ViewModels.AclViewModel, "User");

            this.FieldOptionAcl = ko.observableArray(fieldOptions.map((e) => {
                return new Dns.ViewModels.AclGlobalFieldOptionViewModel(e);
            }));

            this.FieldOptions = new Security.Acl.FieldOption.AclFieldOptionEditViewModel(fieldOptions, securityGroups, this.FieldOptionAcl,[], Dns.ViewModels.AclGlobalFieldOptionViewModel);

            //Events
            this.GlobalEvents = new Events.Acl.EventAclEditViewModel(events.filter((p) => {
                return p.Locations.length == 1 && p.Locations[0] == Dns.Enums.PermissionAclTypes.Global;
            }), securityGroups, this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "GlobalEvents"); 

            this.OrganizationEvents = new Events.Acl.EventAclEditViewModel(events.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Organizations) > -1;
            }), securityGroups, this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "OrganizationEvents"); 

            this.GroupEvents = new Events.Acl.EventAclEditViewModel(events.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Groups) > -1;
            }), securityGroups, this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "GroupEvents"); 

            this.ProjectEvents = new Events.Acl.EventAclEditViewModel(events.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
            }), securityGroups, this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "ProjectEvents"); 

            this.RegistryEvents = new Events.Acl.EventAclEditViewModel(events.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Registries) > -1;
            }), securityGroups, this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "RegistryEvents"); 

            this.UserEvents = new Events.Acl.EventAclEditViewModel(events.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Users) > -1;
            }), securityGroups, this.EventAcls, [], Dns.ViewModels.BaseEventPermissionViewModel, "UserEvents"); 
        }

        public Cancel() {
            window.history.back();
        }

        public Save() {
            var globalAcls = this.Acls().map((a) => {
                return a.toData();
            });

            var globalEventAcls = this.EventAcls().map((e) => {
                return e.toData();
            });

            var fieldOptionAcl = this.FieldOptionAcl().map((e) => {
                return e.toData();
            });
            
            $.when<any>(Dns.WebApi.Security.UpdatePermissions(globalAcls),
                Dns.WebApi.Events.UpdateGlobalEventPermissions(globalEventAcls),
                Dns.WebApi.Security.UpdateFieldOptionPermissions(fieldOptionAcl)
                ).done(() => {
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
            });
        }
    }
    function init() {
        $.when<any>(
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Global]),
            Dns.WebApi.Security.GetGlobalPermissions(),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Global]),
            Dns.WebApi.Events.GetGlobalEventPermissions(),
            Dns.WebApi.Security.GetGlobalFieldOptionPermissions()
            )
            .done((permissions, acls, securityGroups, events, eventAcls, fieldOptions) => {

                $(() => {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(permissions, acls, securityGroups, events, eventAcls, fieldOptions, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }
    init();
} 
