import * as Global from "../../scripts/page/global.js";
import { IGroupDTO, IOrganizationDTO, IOrganizationGroupDTO, IProjectDTO, IGroupEventDTO, IAclGroupDTO, ITreeItemDTO, IEventDTO, IPermissionDTO } from "../Dns.Interfaces";
import * as Enums from "../Dns.Enums.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import { Groups, Security, Organizations, OrganizationGroups, Projects, Events } from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import * as SecurityViewModels from '../security/AclViewModel.js';
import * as EventsAcl from "../events/EditEventPermissions.js";

export class ViewModel extends Global.PageViewModel {
    public Group: ViewModels.GroupViewModel;
    public AllOrganizations: KnockoutObservableArray<ViewModels.OrganizationViewModel>;
    public OrganizationGroups: KnockoutObservableArray<ViewModels.OrganizationGroupViewModel>;
    public AddableOrganizationList: KnockoutComputed<ViewModels.OrganizationViewModel[]>;
    public GroupProjects: KnockoutObservableArray<ViewModels.ProjectViewModel>;
    public GroupAcls: KnockoutObservableArray<ViewModels.AclGroupViewModel>;
    public GroupSecurity: SecurityViewModels.AclEditViewModel<ViewModels.AclGroupViewModel>;

    //Events
    public Events: EventsAcl.EventAclEditViewModel<ViewModels.GroupEventViewModel>;
    public GroupEvents: KnockoutObservableArray<ViewModels.GroupEventViewModel>;

    public CanManageSecurity: KnockoutComputed<boolean>;
    public CanCreateProject: KnockoutComputed<boolean>;
    public CanDeleteGroup: KnockoutComputed<boolean>;
    public CanEditGroup: KnockoutComputed<boolean>;

    constructor(
        screenPermissions: any[],
        group: IGroupDTO,
        allOrganizations: IOrganizationDTO[],
        organizationGroups: IOrganizationGroupDTO[],
        groupProjects: IProjectDTO[],
        events: IGroupEventDTO[],
        permissionList: IPermissionDTO[],
        groupPermissions: IAclGroupDTO[],
        securityGroupTree: ITreeItemDTO[],
        eventList: IEventDTO[],
        bindingControl: JQuery) {

        super(bindingControl, screenPermissions);

        var self = this;

        this.Group = new ViewModels.GroupViewModel(group);

        this.AllOrganizations = ko.observableArray(allOrganizations.map((item) => {
            return new ViewModels.OrganizationViewModel(item);
        }));

        this.OrganizationGroups = ko.observableArray(ko.utils.arrayMap(organizationGroups, (item) => { return new ViewModels.OrganizationGroupViewModel(item) }));

        this.AddableOrganizationList = ko.computed<ViewModels.OrganizationViewModel[]>(() => {
            var results = self.AllOrganizations().filter((o) => {
                var exists = false;
                self.OrganizationGroups().forEach((og) => {
                    if (og.OrganizationID() == o.ID()) {
                        exists = true;
                        return;
                    }
                });

                return !exists;
            });

            return results;
        });

        this.GroupProjects = ko.observableArray(ko.utils.arrayMap(groupProjects, (item) => { return new ViewModels.ProjectViewModel(item) }));


        this.GroupAcls = ko.observableArray(ko.utils.arrayMap(groupPermissions, (item) => { return new ViewModels.AclGroupViewModel(item) }));

        this.GroupSecurity = new SecurityViewModels.AclEditViewModel(permissionList, securityGroupTree, this.GroupAcls, [
            {
                Field: "GroupID",
                Value: this.Group.ID()
            }
        ], ViewModels.AclGroupViewModel);

        this.WatchTitle(this.Group.Name, "Group: ");

        this.CanManageSecurity = ko.computed(() => this.HasPermission(PMNPermissions.Group.ManageSecurity), this);
        this.CanCreateProject = ko.computed(() => this.HasPermission(PMNPermissions.Group.CreateProject), this);
        this.CanDeleteGroup = ko.computed(() => this.HasPermission(PMNPermissions.Group.Delete), this);
        this.CanEditGroup = ko.computed(() => this.HasPermission(PMNPermissions.Group.Edit), this);

        //Events
        this.GroupEvents = ko.observableArray(events != null ? events.map((e) => {
            return new ViewModels.GroupEventViewModel(e);
        }) : null);

        this.Events = new EventsAcl.EventAclEditViewModel<ViewModels.GroupEventViewModel>(eventList, securityGroupTree, this.GroupEvents, [{ Field: "GroupID", Value: this.Group.ID() }], ViewModels.GroupEventViewModel);

    }

    public AddOrganization(o: ViewModels.OrganizationViewModel) {
        this.OrganizationGroups.push(new ViewModels.OrganizationGroupViewModel({
            Organization: o.Name(),
            OrganizationID: o.ID(),
            Group: this.Group.Name(),
            GroupID: this.Group.ID()
        }));
    }

    public RemoveOrganization(o: ViewModels.OrganizationGroupViewModel) {
        let self = this;
        Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the group?</p>").done(() => {

            if (o.GroupID()) {
                OrganizationGroups.Remove([o.toData()]).done(() => {
                    self.OrganizationGroups.remove(o);
                });
            } else {
                self.OrganizationGroups.remove(o);
            }
        });
    }

    public CreateProject() {
        window.location.href = "/projects/details?GroupID=" + this.Group.ID();
    }

    public Save() {
        if (!super.Validate())
            return;

        Groups.InsertOrUpdate([this.Group.toData()]).done((groups) => {
            //Update the values for the ID and timestamp as necessary.
            this.Group.ID(groups[0].ID);
            this.Group.Timestamp(groups[0].Timestamp);

            // Save everything else
            let organizations = this.OrganizationGroups().map((o) => {
                o.GroupID(this.Group.ID());
                return o.toData();
            });

            let groupAcls = this.GroupAcls().map((a) => {
                a.GroupID(this.Group.ID());
                return a.toData();
            });

            let groupEvents = this.GroupEvents().map((e) => {
                e.GroupID(this.Group.ID());
                return e.toData();
            });


            $.when<any>(
                Security.UpdateGroupPermissions(groupAcls),
                Events.UpdateGroupEventPermissions(groupEvents),
                OrganizationGroups.InsertOrUpdate(organizations)
            ).done(() => {
                Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
            });
        });
    }

    public Cancel() {
        window.location.href = "/groups";
    }

    public Delete() {
        let self = this;
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Group?</p>").done(() => {
            Groups.Delete([self.Group.ID()]).done(() => {
                window.location.href = "/groups";
            });
        });
    }
}

let id = Global.GetQueryParam("ID");
$.when<any>(
    id == null ? null : Groups.GetPermissions([id], [PMNPermissions.Group.Delete, PMNPermissions.Group.Edit, PMNPermissions.Group.ManageSecurity, PMNPermissions.Group.CreateProject]),
    id == null ? null : Groups.Get(id),
    Organizations.List(null, null, "Name"),
    id == null ? null : OrganizationGroups.List("GroupID eq " + id),
    id == null ? null : Projects.List("GroupID eq " + id),
    id == null ? null : Events.GetGroupEventPermissions(id),
    Security.GetPermissionsByLocation([Enums.PermissionAclTypes.Groups]),
    Security.GetGroupPermissions(id ? id : Constants.GuidEmpty),
    Security.GetAvailableSecurityGroupTree(),
    Events.GetEventsByLocation([Enums.PermissionAclTypes.Groups])
).done((
    screenPermissions: any[],
    groups: IGroupDTO,
    allOrganizations: IOrganizationDTO[],
    organizationGroups: IOrganizationGroupDTO[],
    groupProjects: IProjectDTO[],
    events: IGroupEventDTO[],
    permissionList,
    groupPermission,
    securityGroupTree,
    eventList
) => {

    let group: IGroupDTO;
    if (Array.isArray(groups)) {
        group = groups[0];
    } else if (groups) {
        group = groups;
    } else {
        group = null;
    }

    screenPermissions = id == null ? [PMNPermissions.Group.Delete, PMNPermissions.Group.Edit, PMNPermissions.Group.ManageSecurity, PMNPermissions.Group.CreateProject] : screenPermissions;

    $(() => {
        let bindingControl = $("#Content");
        let vm = new ViewModel(screenPermissions, group, allOrganizations, organizationGroups, groupProjects, events, permissionList, groupPermission, securityGroupTree, eventList, bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
    });
});
