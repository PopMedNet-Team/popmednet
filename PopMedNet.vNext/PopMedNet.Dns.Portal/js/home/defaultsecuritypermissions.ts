import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import { vmHeader } from '../_Layout.js';
import * as Enums from '../Dns.Enums.js';
import * as SecurityViewModels from '../security/AclViewModel.js';
import * as EventsAcl from "../events/EditEventPermissions.js";
import * as SecurityAclFieldOption from "../security/AclFieldOptionViewModel.js";

export default class ViewModel extends Global.PageViewModel {
    public Global: SecurityViewModels.AclEditViewModel<ViewModels.AclViewModel>;
    public Organization: SecurityViewModels.AclEditViewModel<ViewModels.AclViewModel>;
    public Group: SecurityViewModels.AclEditViewModel<ViewModels.AclViewModel>;
    public Project: SecurityViewModels.AclEditViewModel<ViewModels.AclViewModel>;
    public Registry: SecurityViewModels.AclEditViewModel<ViewModels.AclViewModel>;
    public User: SecurityViewModels.AclEditViewModel<ViewModels.AclViewModel>;
    public FieldOptions: SecurityAclFieldOption.AclFieldOptionEditViewModel<ViewModels.BaseFieldOptionAclViewModel>;

    public GlobalEvents: EventsAcl.EventAclEditViewModel<ViewModels.BaseEventPermissionViewModel>;
    public OrganizationEvents: EventsAcl.EventAclEditViewModel<ViewModels.BaseEventPermissionViewModel>;
    public GroupEvents: EventsAcl.EventAclEditViewModel<ViewModels.BaseEventPermissionViewModel>;
    public ProjectEvents: EventsAcl.EventAclEditViewModel<ViewModels.BaseEventPermissionViewModel>;
    public RegistryEvents: EventsAcl.EventAclEditViewModel<ViewModels.BaseEventPermissionViewModel>;
    public UserEvents: EventsAcl.EventAclEditViewModel<ViewModels.BaseEventPermissionViewModel>;

    public Acls: KnockoutObservableArray<ViewModels.AclViewModel>;
    public EventAcls: KnockoutObservableArray<ViewModels.BaseEventPermissionViewModel>;
    public FieldOptionAcl: KnockoutObservableArray<ViewModels.AclGlobalFieldOptionViewModel>;

    constructor(permissions: Interfaces.IPermissionDTO[],
        acls: Interfaces.IAclDTO[],
        securityGroups: Interfaces.ITreeItemDTO[],
        events: Interfaces.IEventDTO[],
        eventAcls: Interfaces.IBaseEventPermissionDTO[],
        fieldOptions: Interfaces.IAclGlobalFieldOptionDTO[],
        bindingControl: JQuery) {
        super(bindingControl);

        this.Acls = ko.observableArray(acls.map((a) => {
            return new ViewModels.AclViewModel(a);
        }));

        this.EventAcls = ko.observableArray(eventAcls.map((e) => {
            return new ViewModels.BaseEventPermissionViewModel(e);
        }));

        //Permissions
        this.Global = new SecurityViewModels.AclEditViewModel(permissions.filter((p) => {
            return (p.Locations.length == 1 || p.Locations.indexOf(Enums.PermissionAclTypes.RequestTypes) > -1 || p.Locations.indexOf(Enums.PermissionAclTypes.Templates) > -1) && p.Locations[0] == Enums.PermissionAclTypes.Global;
        }), securityGroups, this.Acls, [], ViewModels.AclViewModel, "Global");

        this.Organization = new SecurityViewModels.AclEditViewModel(permissions.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Organizations) > -1;
        }), securityGroups, this.Acls, [], ViewModels.AclViewModel, "Organization");

        this.Group = new SecurityViewModels.AclEditViewModel(permissions.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Groups) > -1;
        }), securityGroups, this.Acls, [], ViewModels.AclViewModel, "Group");

        this.Project = new SecurityViewModels.AclEditViewModel(permissions.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Projects) > -1;
        }), securityGroups, this.Acls, [], ViewModels.AclViewModel, "Project");

        this.Registry = new SecurityViewModels.AclEditViewModel(permissions.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Registries) > -1;
        }), securityGroups, this.Acls, [], ViewModels.AclViewModel, "Registry");

        this.User = new SecurityViewModels.AclEditViewModel(permissions.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Users) > -1;
        }), securityGroups, this.Acls, [], ViewModels.AclViewModel, "User");

        this.FieldOptionAcl = ko.observableArray(fieldOptions.map((e) => {
            return new ViewModels.AclGlobalFieldOptionViewModel(e);
        }));

        this.FieldOptions = new SecurityAclFieldOption.AclFieldOptionEditViewModel(fieldOptions, securityGroups, this.FieldOptionAcl, [], ViewModels.AclGlobalFieldOptionViewModel);

        //Events
        this.GlobalEvents = new EventsAcl.EventAclEditViewModel(events.filter((p) => {
            return p.Locations.length == 1 && p.Locations[0] == Enums.PermissionAclTypes.Global;
        }), securityGroups, this.EventAcls, [], ViewModels.BaseEventPermissionViewModel, "GlobalEvents");

        this.OrganizationEvents = new EventsAcl.EventAclEditViewModel(events.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Organizations) > -1;
        }), securityGroups, this.EventAcls, [], ViewModels.BaseEventPermissionViewModel, "OrganizationEvents");

        this.GroupEvents = new EventsAcl.EventAclEditViewModel(events.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Groups) > -1;
        }), securityGroups, this.EventAcls, [], ViewModels.BaseEventPermissionViewModel, "GroupEvents");

        this.ProjectEvents = new EventsAcl.EventAclEditViewModel(events.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Projects) > -1;
        }), securityGroups, this.EventAcls, [], ViewModels.BaseEventPermissionViewModel, "ProjectEvents");

        this.RegistryEvents = new EventsAcl.EventAclEditViewModel(events.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Registries) > -1;
        }), securityGroups, this.EventAcls, [], ViewModels.BaseEventPermissionViewModel, "RegistryEvents");

        this.UserEvents = new EventsAcl.EventAclEditViewModel(events.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Users) > -1;
        }), securityGroups, this.EventAcls, [], ViewModels.BaseEventPermissionViewModel, "UserEvents");
    }

    public Cancel() {
        window.location.href = "/";
    }

    public Save() {
        let globalAcls = this.Acls().map((a) => {
            return a.toData();
        });

        let globalEventAcls = this.EventAcls().map((e) => {
            return e.toData();
        });

        let fieldOptionAcl = this.FieldOptionAcl().map((e) => {
            return e.toData();
        });

        $.when<any>(WebApi.Security.UpdatePermissions(globalAcls),
            WebApi.Events.UpdateGlobalEventPermissions(globalEventAcls),
            WebApi.Security.UpdateFieldOptionPermissions(fieldOptionAcl)
        ).done(() => {
            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
        });
    }
}

$.when<any>(
    WebApi.Security.GetPermissionsByLocation([Enums.PermissionAclTypes.Global]),
    WebApi.Security.GetGlobalPermissions(),
    WebApi.Security.GetAvailableSecurityGroupTree(),
    WebApi.Events.GetEventsByLocation([Enums.PermissionAclTypes.Global]),
    WebApi.Events.GetGlobalEventPermissions(),
    WebApi.Security.GetGlobalFieldOptionPermissions()
)
    .done((permissions, acls, securityGroups, events, eventAcls, fieldOptions) => {

        $(() => {
            let bindingControl = $("#Content");
            let vm = new ViewModel(permissions, acls, securityGroups, events, eventAcls, fieldOptions, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    });