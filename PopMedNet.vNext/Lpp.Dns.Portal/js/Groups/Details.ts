/// <reference path="../_rootlayout.ts" />

module Groups.Details {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public Group: Dns.ViewModels.GroupViewModel;
        public AllOrganizations: KnockoutObservableArray<Dns.ViewModels.OrganizationViewModel>;
        public OrganizationGroups: KnockoutObservableArray<Dns.ViewModels.OrganizationGroupViewModel>;
        public AddableOrganizationList: KnockoutComputed<Dns.ViewModels.OrganizationViewModel[]>;
        public GroupProjects: KnockoutObservableArray<Dns.ViewModels.ProjectViewModel>;
        public GroupAcls: KnockoutObservableArray<Dns.ViewModels.AclGroupViewModel>;
        public GroupSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclGroupViewModel>;

        //Events
        public Events: Events.Acl.EventAclEditViewModel<Dns.ViewModels.GroupEventViewModel>;
        public GroupEvents: KnockoutObservableArray<Dns.ViewModels.GroupEventViewModel>;
        
        constructor(
            screenPermissions: any[],
            group: Dns.Interfaces.IGroupDTO,
            allOrganizations: Dns.Interfaces.IOrganizationDTO[],
            organizationGroups: Dns.Interfaces.IOrganizationGroupDTO[],
            groupProjects: Dns.Interfaces.IProjectDTO[],
            events: Dns.Interfaces.IGroupEventDTO[],
            permissionList: Dns.Interfaces.IPermissionDTO[],
            groupPermissions: Dns.Interfaces.IAclGroupDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            eventList: Dns.Interfaces.IEventDTO[],
            bindingControl: JQuery) {

            super(bindingControl, screenPermissions);

            var self = this;            

            this.Group = new Dns.ViewModels.GroupViewModel(group);

            this.AllOrganizations = ko.observableArray(allOrganizations.map((item) => {
                return new Dns.ViewModels.OrganizationViewModel(item);
            }));

            //this.OrganizationGroups = ko.observableArray(organizationGroups.map((item) => {
            //    return new Dns.ViewModels.OrganizationGroupViewModel(item);
            //}));

            this.OrganizationGroups = ko.observableArray(ko.utils.arrayMap(organizationGroups, (item) => { return new Dns.ViewModels.OrganizationGroupViewModel(item) }));

            this.AddableOrganizationList = ko.computed<Dns.ViewModels.OrganizationViewModel[]>(() => {
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

            this.GroupProjects = ko.observableArray(ko.utils.arrayMap(groupProjects, (item) => { return new Dns.ViewModels.ProjectViewModel(item) }));


            this.GroupAcls = ko.observableArray(ko.utils.arrayMap(groupPermissions, (item) => { return new Dns.ViewModels.AclGroupViewModel(item) }));
            
            this.GroupSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, this.GroupAcls, [
                {
                    Field: "GroupID",
                    Value: this.Group.ID()
                }
            ], Dns.ViewModels.AclGroupViewModel);

            this.WatchTitle(this.Group.Name, "Group: ");

            //Events
            this.GroupEvents = ko.observableArray(events != null ? events.map((e) => {
                return new Dns.ViewModels.GroupEventViewModel(e);
            }) : null);

            this.Events = new Events.Acl.EventAclEditViewModel<Dns.ViewModels.GroupEventViewModel>(eventList, securityGroupTree, this.GroupEvents, [{ Field: "GroupID", Value: this.Group.ID() }], Dns.ViewModels.GroupEventViewModel);

        }

        public AddOrganization(o: Dns.ViewModels.OrganizationViewModel) {
            vm.OrganizationGroups.push(new Dns.ViewModels.OrganizationGroupViewModel({
                Organization: o.Name(),
                OrganizationID: o.ID(),
                Group: vm.Group.Name(),
                GroupID: vm.Group.ID()
            }));
        }

        public RemoveOrganization(o: Dns.ViewModels.OrganizationGroupViewModel) {
            Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the group?</p>").done(() => {

                if (o.GroupID()) {
                    Dns.WebApi.OrganizationGroups.Remove([o.toData()]).done(() => {
                        vm.OrganizationGroups.remove(o);
                    });
                } else {
                    vm.OrganizationGroups.remove(o);
                }
            });
        }

        public CreateProject() {
            window.location.href = "/projects/details?GroupID=" + this.Group.ID();
        }

        public Save() {
            if (!super.Validate())
                return;

            Dns.WebApi.Groups.InsertOrUpdate([this.Group.toData()]).done((group) => {
                //Update the values for the ID and timestamp as necessary.
                this.Group.ID(group[0].ID);
                this.Group.Timestamp(group[0].Timestamp);

                // Save everything else
                var organizations = this.OrganizationGroups().map((o) => {
                    o.GroupID(this.Group.ID());
                    return o.toData();
                });

                var groupAcls = this.GroupAcls().map((a) => {
                    a.GroupID(this.Group.ID());
                    return a.toData();
                });

                var groupEvents = this.GroupEvents().map((e) => {
                    e.GroupID(this.Group.ID());
                    return e.toData();
                });


                $.when<any>(
                    Dns.WebApi.Security.UpdateGroupPermissions(groupAcls),
                    Dns.WebApi.Events.UpdateGroupEventPermissions(groupEvents),
                    Dns.WebApi.OrganizationGroups.InsertOrUpdate(organizations)
                    ).done(() => {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
            });
        }

        public Cancel() {
            window.history.back();
        }

        public Delete() {
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Group?</p>").done(() => {
                Dns.WebApi.Groups.Delete([vm.Group.ID()]).done(() => {
                    window.history.back();
                });
            });
        }
    }

    function init() {
        var id: any = $.url().param("ID");
        $.when<any>(
            id == null ? null : Dns.WebApi.Groups.GetPermissions([id], [PMNPermissions.Group.Delete, PMNPermissions.Group.Edit, PMNPermissions.Group.ManageSecurity, PMNPermissions.Group.CreateProject]),
            id == null ? null : Dns.WebApi.Groups.Get(id),
            Dns.WebApi.Organizations.List(null, null, "Name"),
            id == null ? null : Dns.WebApi.OrganizationGroups.List("GroupID eq " + id ),
            id == null ? null : Dns.WebApi.Projects.List("GroupID eq " + id),
            id == null ? null : Dns.WebApi.Events.GetGroupEventPermissions(id),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Groups]),
            Dns.WebApi.Security.GetGroupPermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Groups])
            ).done((
                screenPermissions: any[],
                groups: Dns.Interfaces.IGroupDTO,
                allOrganizations: Dns.Interfaces.IOrganizationDTO[],
                organizationGroups: Dns.Interfaces.IOrganizationGroupDTO[],
                groupProjects: Dns.Interfaces.IProjectDTO[],
                events: Dns.Interfaces.IGroupEventDTO[],
                permissionList,
                groupPermission,
                securityGroupTree,
                eventList
                ) => {
                var group: Dns.Interfaces.IGroupDTO = groups == null ? null : groups[0];

                screenPermissions = id == null ? [PMNPermissions.Group.Delete, PMNPermissions.Group.Edit, PMNPermissions.Group.ManageSecurity, PMNPermissions.Group.CreateProject] : screenPermissions;

                $(() => {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions, group, allOrganizations, organizationGroups, groupProjects, events, permissionList, groupPermission, securityGroupTree, eventList, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }

    init();
}

 