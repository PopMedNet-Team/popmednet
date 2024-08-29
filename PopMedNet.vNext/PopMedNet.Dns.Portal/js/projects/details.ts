import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import { Projects, ProjectDataMarts, ProjectOrganizations, Security, Events, Organizations, DataMarts, Groups, SecurityGroups, RequestTypes, Workflow } from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import { vmHeader } from '../_Layout.js';
import * as Enums from '../Dns.Enums.js';
import * as SecurityViewModels from '../security/AclViewModel.js';
import * as SecurityAclRequestTypes from '../security/AclRequestTypeViewModel.js';
import * as EventsAcl from "../events/EditEventPermissions.js";
import * as SecurityAclFieldOption from "../security/AclFieldOptionViewModel.js";

export default class ViewModel extends Global.PageViewModel {

    public Project: ViewModels.ProjectViewModel;
    public ProjectDataMarts: KnockoutObservableArray<ProjectDataMartViewModel>;
    public ProjectRequestTypes: KnockoutObservableArray<ProjectRequestTypeViewModel>;

    public SecurityGroups: KnockoutObservableArray<ViewModels.SecurityGroupViewModel>;
    public Organizations: KnockoutObservableArray<OrganizationViewModel>;
    public DataMartList: Interfaces.IDataMartDTO[];
    public OrganizationList: KnockoutObservableArray<ViewModels.OrganizationViewModel>;
    public PermissionList: Interfaces.IPermissionDTO[];
    public EventList: Interfaces.IEventDTO[];
    public ProjectDataMartEventList: Interfaces.IEventDTO[];
    public SecurityGroupTree: Interfaces.ITreeItemDTO[];
    public RequestTypes: KnockoutObservableArray<ViewModels.RequestTypeViewModel>;
    public AddableDataMartList: KnockoutComputed<Interfaces.IDataMartDTO[]>;
    public AddableOrganizationList: KnockoutComputed<ViewModels.OrganizationViewModel[]>;

    public AddableRequestTypeList: KnockoutComputed<Interfaces.IRequestTypeDTO[]>;
    public RequestTypeList: Interfaces.IRequestTypeDTO[];

    public Groups: Interfaces.IGroupDTO[];

    public ProjectAcls: KnockoutObservableArray<ViewModels.AclProjectViewModel>;
    public ProjectRequestTypeAcls: KnockoutObservableArray<ViewModels.AclProjectRequestTypeViewModel>;
    public ProjectEvents: KnockoutObservableArray<ViewModels.ProjectEventViewModel>;
    public DataMartAcls: KnockoutObservableArray<ViewModels.AclProjectDataMartViewModel>;
    public ProjectDataMartEvents: KnockoutObservableArray<ViewModels.ProjectDataMartEventViewModel>;
    public DataMartRequestTypeAcls: KnockoutObservableArray<ViewModels.AclProjectDataMartRequestTypeViewModel>;
    public OrganizationAcls: KnockoutObservableArray<ViewModels.AclProjectOrganizationViewModel>;
    public OrganizationEvents: KnockoutObservableArray<ViewModels.ProjectOrganizationEventViewModel>;
    public ProjectRequestTypeWorkflowActivityAcls: KnockoutObservableArray<ViewModels.AclProjectRequestTypeWorkflowActivityViewModel>;

    public ProjectSecurity: SecurityViewModels.AclEditViewModel<ViewModels.AclProjectViewModel>;
    public ProjectRequestTypesSecurity: SecurityAclRequestTypes.AclRequestTypeEditViewModel<ViewModels.AclProjectRequestTypeViewModel>;
    public ProjectEventSecurity: EventsAcl.EventAclEditViewModel<ViewModels.ProjectEventViewModel>;

    public FieldOptions: SecurityAclFieldOption.AclFieldOptionEditViewModel<ViewModels.BaseFieldOptionAclViewModel>;
    public FieldOptionAcl: KnockoutObservableArray<ViewModels.AclProjectFieldOptionViewModel>;

    public CanManageSecurityTypes: KnockoutObservable<boolean>;
    public CanManageProjectSecurity: KnockoutObservable<boolean>;
    public CanDeleteProject: KnockoutObservable<boolean>;
    public CanCopyProject: KnockoutObservable<boolean>;
    public CanEditProject: KnockoutObservable<boolean>;

    public Save: (data, event: JQueryEventObject, showPrompt: boolean) => JQueryDeferred<void>;

    public DeletedDataMarts: ProjectDataMartViewModel[] = [];
    public DeletedOrganizations: OrganizationViewModel[] = [];

    constructor(
        screenPermissions: any[],
        project: Interfaces.IProjectDTO,
        projectRequestTypes: Interfaces.IProjectRequestTypeDTO[],
        projectRequestTypeWorkflowActivityAcls,
        groups: Interfaces.IGroupDTO[],
        dataMartList: Interfaces.IDataMartDTO[],
        organizationList: Interfaces.IOrganizationDTO[],
        permissionList: Interfaces.IPermissionDTO[],
        requestTypes: Interfaces.IRequestTypeDTO[],
        projectDataMarts: Interfaces.IProjectDataMartWithRequestTypesDTO[],
        projectDataMartPermissions: Interfaces.IAclProjectDataMartDTO[],
        projectPermissions: Interfaces.IAclProjectDTO[],
        projectRequestTypePermissions: Interfaces.IAclProjectRequestTypeDTO[],
        projectDataMartRequestTypePermissions: Interfaces.IAclProjectDataMartRequestTypeDTO[],
        projectEventPermissions: Interfaces.IProjectEventDTO[],
        projectDataMartEventPermissions: Interfaces.IProjectDataMartEventDTO[],
        projectOrganizationPermissions: Interfaces.IAclProjectOrganizationDTO[],
        projectOrganizationEventPermissions: Interfaces.IProjectOrganizationEventDTO[],
        securityGroupTree: Interfaces.ITreeItemDTO[],
        projectSecurityGroups: Interfaces.ISecurityGroupDTO[],
        projectOrganizations: Interfaces.IProjectOrganizationDTO[],
        eventList: Interfaces.IEventDTO[],
        projectDataMartEventsList: Interfaces.IEventDTO[],
        groupid: any,
        requestTypeList: Interfaces.IRequestTypeDTO[],
        fieldOptions: Interfaces.IAclProjectFieldOptionDTO[],
        bindingControl: JQuery) {

        super(bindingControl, screenPermissions, $('#frmProjectDetails'));

        var self = this;

        this.CanManageSecurityTypes = ko.observable(this.HasPermission(PMNPermissions.Project.ManageRequestTypes));
        this.CanManageProjectSecurity = ko.observable(this.HasPermission(PMNPermissions.Project.ManageSecurity));
        this.CanDeleteProject = ko.observable(this.HasPermission(PMNPermissions.Project.Delete));
        this.CanCopyProject = ko.observable(this.HasPermission(PMNPermissions.Project.Copy));
        this.CanEditProject = ko.observable(this.HasPermission(PMNPermissions.Project.Edit));

        //Get lists
        this.DataMartList = dataMartList;
        this.OrganizationList = ko.observableArray(organizationList.map((item) => {
            return new ViewModels.OrganizationViewModel(item);
        }));
        this.PermissionList = permissionList;
        this.EventList = eventList;
        this.ProjectDataMartEventList = projectDataMartEventsList;
        this.SecurityGroupTree = securityGroupTree;
        this.RequestTypes = ko.observableArray<ViewModels.RequestTypeViewModel>(requestTypes.map((item) => {
            return new ViewModels.RequestTypeViewModel(item);
        }));
        this.RequestTypeList = requestTypeList;
        this.Groups = groups;
        this.Project = new ViewModels.ProjectViewModel(project);
        this.Project.StartDate = ko.observable((project == null || project.StartDate == null) ? null : project.StartDate);
        this.Project.EndDate = ko.observable((project == null || project.EndDate == null) ? null : project.EndDate);
        this.SecurityGroups = ko.observableArray(projectSecurityGroups.map((sg) => {
            return new ViewModels.SecurityGroupViewModel(sg);
        }));

        this.ProjectRequestTypes = ko.observableArray(projectRequestTypes.map((item) => {
            return new ProjectRequestTypeViewModel(item, this);
        }));

        //Acls
        this.ProjectAcls = ko.observableArray(projectPermissions.map((item) => {
            return new ViewModels.AclProjectViewModel(item);
        }));
        this.ProjectRequestTypeAcls = ko.observableArray(projectRequestTypePermissions.map((item) => {
            return new ViewModels.AclProjectRequestTypeViewModel(item);
        }));

        this.ProjectRequestTypeWorkflowActivityAcls = ko.observableArray<ViewModels.AclProjectRequestTypeWorkflowActivityViewModel>(projectRequestTypeWorkflowActivityAcls.map((item) => {
            return new ViewModels.AclProjectRequestTypeWorkflowActivityViewModel(item);
        }));

        this.ProjectEvents = ko.observableArray(projectEventPermissions.map((item) => {
            return new ViewModels.ProjectEventViewModel(item);
        }));
        this.ProjectDataMartEvents = ko.observableArray(projectDataMartEventPermissions.map((item) => {
            return new ViewModels.ProjectDataMartEventViewModel(item);
        }));
        this.DataMartAcls = ko.observableArray(projectDataMartPermissions.map((item) => {
            return new ViewModels.AclProjectDataMartViewModel(item);
        }));
        this.DataMartRequestTypeAcls = ko.observableArray(projectDataMartRequestTypePermissions.map((item) => {
            return new ViewModels.AclProjectDataMartRequestTypeViewModel(item);
        }));
        this.OrganizationAcls = ko.observableArray(projectOrganizationPermissions.map((item) => {
            return new ViewModels.AclProjectOrganizationViewModel(item);
        }));
        this.OrganizationEvents = ko.observableArray(projectOrganizationEventPermissions.map((item) => {
            return new ViewModels.ProjectOrganizationEventViewModel(item);
        }));

        this.ProjectSecurity = new SecurityViewModels.AclEditViewModel(permissionList.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.Projects) > -1;
        }), securityGroupTree, this.ProjectAcls, [
            {
                Field: "ProjectID",
                Value: this.Project.ID()
            }
        ], ViewModels.AclProjectViewModel);

        this.ProjectRequestTypesSecurity = new SecurityAclRequestTypes.AclRequestTypeEditViewModel(this.RequestTypes, securityGroupTree, this.ProjectRequestTypeAcls, [
            {
                Field: "ProjectID",
                Value: this.Project.ID()
            }
        ], ViewModels.AclProjectRequestTypeViewModel);

        this.ProjectEventSecurity = new EventsAcl.EventAclEditViewModel(eventList.filter((e) => {
            return e.Locations.indexOf(Enums.PermissionAclTypes.Projects) > -1;
        }), securityGroupTree, this.ProjectEvents, [
            {
                Field: "ProjectID",
                Value: this.Project.ID()
            }
        ], ViewModels.ProjectEventViewModel);

        this.FieldOptionAcl = ko.observableArray(fieldOptions.map((e) => {
            if (e.ProjectID != self.Project.ID())
                e.ProjectID = self.Project.ID();
            return new ViewModels.AclProjectFieldOptionViewModel(e);
        }));

        this.FieldOptions = new SecurityAclFieldOption.AclFieldOptionEditViewModel(fieldOptions, securityGroupTree, this.FieldOptionAcl, [
            {
                Field: "ProjectID",
                Value: self.Project.ID()
            }], ViewModels.AclProjectFieldOptionViewModel);

        this.ProjectDataMarts = ko.observableArray(projectDataMarts.map((item) => { return new ProjectDataMartViewModel(item, this.PermissionList, this.EventList, this.ProjectDataMartEventList, this.SecurityGroupTree, this.DataMartAcls, this.DataMartRequestTypeAcls, this.ProjectDataMartEvents); }));

        this.Organizations = ko.observableArray(projectOrganizations.map((o) => {
            return new OrganizationViewModel(o, this.PermissionList, this.EventList, this.SecurityGroupTree, this.OrganizationAcls, this.OrganizationEvents);
        }));


        //Set the Title. Every page should have this.
        this.WatchTitle(this.Project.Name, "Project: ");

        //List of datamarts that can be added to the project
        this.AddableDataMartList = ko.computed<Interfaces.IDataMartDTO[]>(() => {
            return self.DataMartList.filter((dm) => {
                var exists = false;
                self.ProjectDataMarts().forEach((pdm) => {
                    if (pdm.DataMartID() == dm.ID) {
                        exists = true;
                        return;
                    }
                });

                return !exists;
            });
        });

        this.AddableOrganizationList = ko.computed<ViewModels.OrganizationViewModel[]>(() => {
            var results = self.OrganizationList().filter((o) => {
                var exists = false;
                self.Organizations().forEach((po) => {
                    if (po.OrganizationID() == o.ID()) {
                        exists = true;
                        return;
                    }
                });

                return !exists;
            });

            return results;
        });

        this.AddableRequestTypeList = ko.computed<Interfaces.IRequestTypeDTO[]>(() => {
            var results = self.RequestTypeList.filter((rt) => {
                var exists = false;
                self.ProjectRequestTypes().forEach((prt) => {
                    if (prt.RequestTypeID() == rt.ID) {
                        exists = true;
                        return;
                    }
                });

                return !exists;
            });

            return results;

        });


        this.Project.GroupID.subscribe((value) => {
            if (value == this.Project.GroupID())
                return;

            (value == null ? null : Organizations.ListByGroupMembership(value)).done((organizationList) => {
                this.OrganizationList = ko.observableArray(organizationList.map((item) => { return new ViewModels.OrganizationViewModel(item); }) || []);
            });
        });

        if (groupid != null)
            this.Project.GroupID(groupid);

        self.Save = (data, event: JQueryEventObject, showPrompt: boolean = true) => {

            Global.Helpers.ShowExecuting();

            var deferred = $.Deferred<void>();

            if (!self.Validate()) {
                deferred.reject();
                if (event != null)
                    event.preventDefault();

                return;
            }

            //Only save if the root project saved.
            Projects.InsertOrUpdate([self.Project.toData()]).done((project) => {

                let projectID = project[0].ID;

                //Update the values for the ID and timestamp as necessary.
                self.Project.ID(projectID);
                self.Project.Timestamp(project[0].Timestamp);
                window.history.replaceState(null, window.document.title, "/projects/details?ID=" + projectID);

                
                vmHeader.ReloadMenu();

                //Save everything else
                let dataMarts = self.ProjectDataMarts().map((dm) => {
                    dm.ProjectID(projectID);
                    return dm.toData();
                });

                let organizations = self.Organizations().map((o) => {
                    o.ProjectID(projectID);
                    return o.toData();
                });

                let deletedDataMarts = self.DeletedDataMarts.map(dm => {
                    return dm.toData();
                });

                let deletedOrgs = self.DeletedOrganizations.map(o => {
                    return o.toData();
                });

                let dataMartAcls: Interfaces.IAclProjectDataMartDTO[] = null;
                let projectAcls: Interfaces.IAclProjectDTO[] = null;
                let projectEventAcls: Interfaces.IProjectEventDTO[] = null;
                let projectRequestTypeAcls: Interfaces.IAclProjectRequestTypeDTO[] = null;
                let projectDataMartEventAcls: Interfaces.IProjectDataMartEventDTO[] = null;
                let projectDataMartRequestTypeAcls: Interfaces.IAclProjectDataMartRequestTypeDTO[] = null;
                let organizationAcls: Interfaces.IAclProjectOrganizationDTO[] = null;
                let organizationEvents: Interfaces.IProjectOrganizationEventDTO[] = null;
                let projectRequestTypeWorkFlowActivityAcls: Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[] = null;
                let projectFieldOptionsAcls: Interfaces.IAclProjectFieldOptionDTO[] = null;

                if (self.HasPermission(PMNPermissions.Project.ManageSecurity)) {
                    dataMartAcls = self.DataMartAcls().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    projectAcls = self.ProjectAcls().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    projectEventAcls = self.ProjectEvents().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    projectRequestTypeAcls = self.ProjectRequestTypeAcls().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    projectDataMartEventAcls = self.ProjectDataMartEvents().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    projectDataMartRequestTypeAcls = self.DataMartRequestTypeAcls().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    organizationAcls = self.OrganizationAcls().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    organizationEvents = self.OrganizationEvents().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    projectRequestTypeWorkFlowActivityAcls = self.ProjectRequestTypeWorkflowActivityAcls().map((a) => {
                        a.ProjectID(projectID);
                        return a.toData();
                    });

                    projectFieldOptionsAcls = self.FieldOptions.Acls().map((a) => <Interfaces.IAclProjectFieldOptionDTO>a.toData());
                }

                let projectRequestTypes: Interfaces.IUpdateProjectRequestTypesDTO = null;
                let canManageRequestTypes = self.HasPermission(PMNPermissions.Project.ManageRequestTypes);
                if (canManageRequestTypes) {
                    projectRequestTypes = {
                        ProjectID: projectID,
                        RequestTypes: self.ProjectRequestTypes().map((rt) => {
                            rt.ProjectID(projectID);
                            return rt.toData();
                        })
                    };
                }

                let canManageSecurity = self.HasPermission(PMNPermissions.Project.ManageSecurity);
                let originalManageRequestTypes = canManageRequestTypes;

                $.when<any>(
                    canManageSecurity ? Security.UpdateProjectPermissions(projectAcls) : null,
                    canManageSecurity ? Events.UpdateProjectEventPermissions(projectEventAcls) : null,
                    canManageSecurity ? Events.UpdateProjectDataMartEventPermissions(projectDataMartEventAcls) : null,
                    canManageSecurity ? Events.UpdateProjectOrganizationEventPermissions(organizationEvents) : null,
                    canManageSecurity ? Security.UpdateProjectOrganizationPermissions(organizationAcls) : null,
                    canManageSecurity ? Security.UpdateProjectDataMartPermissions(dataMartAcls) : null,
                    canManageSecurity && (projectFieldOptionsAcls != null && projectFieldOptionsAcls.length > 0) ? Security.UpdateProjectFieldOptionPermissions(projectFieldOptionsAcls) : null
                ).done(() => {

                    //update permissions
                    Projects.GetPermissions([projectID], [
                        PMNPermissions.Project.Copy,
                        PMNPermissions.Project.Delete,
                        PMNPermissions.Project.Edit,
                        PMNPermissions.Project.ManageSecurity,
                        PMNPermissions.Project.ManageRequestTypes]).done((perms) => {

                            //update the screen permissions with the newly set permissions
                            self.ScreenPermissions = perms.map((sp: string) => {
                                return sp.toLowerCase();
                            });

                            //now update the rest of the stuff using the new permissions
                            canManageRequestTypes = self.HasPermission(PMNPermissions.Project.ManageRequestTypes);
                            self.CanManageSecurityTypes(canManageRequestTypes);

                            $.when<any>(
                                canManageRequestTypes ? Security.UpdateProjectRequestTypePermissions(projectRequestTypeAcls) : null,
                                canManageRequestTypes ? Security.UpdateProjectDataMartRequestTypePermissions(projectDataMartRequestTypeAcls) : null
                            ).done(() => {

                                $.when<any>(
                                    self.DeletedDataMarts.length > 0 ? ProjectDataMarts.Remove(deletedDataMarts) : null,
                                    self.DeletedOrganizations.length > 0 ? ProjectOrganizations.Remove(deletedOrgs) : null
                                )
                                    .then<any>(() => {
                                    ProjectDataMarts.InsertOrUpdate({ ProjectID: projectID, DataMarts: dataMarts }),
                                        ProjectOrganizations.InsertOrUpdate({ ProjectID: projectID, Organizations: organizations }),
                                        (canManageRequestTypes && projectRequestTypes != null) ? Projects.UpdateProjectRequestTypes(projectRequestTypes) : null,
                                        (canManageRequestTypes && projectRequestTypeWorkFlowActivityAcls != null) ? Security.UpdateProjectRequestTypeWorkflowActivityPermissions(projectRequestTypeWorkFlowActivityAcls) : null
                                    ////Add others here.
                                    })
                                    .done(() => {

                                    if (showPrompt) {
                                        Global.Helpers.ShowAlert("Save", '<p style="text-align:center;">Save completed successfully!</p>')
                                            .done(() => {

                                                if (canManageRequestTypes != originalManageRequestTypes && canManageRequestTypes) {
                                                    //if the permission to edit request types changes from denied to allowed, reload after save to make sure that all the requesttype collections are properly loaded back up.
                                                    window.location.reload();
                                                }
                                            });
                                    };

                                    deferred.resolve();

                                }).fail((error) => {
                                    deferred.reject();
                                });

                            }).fail((error) => {
                                deferred.reject();
                            });



                        });
                })
                    .fail((error) => {
                        deferred.reject();
                    });

            }).fail((error) => {
                //fail of initial save
                deferred.reject();
            }).always(() => {
                Global.Helpers.HideExecuting();
            });

            return deferred;

        };
    }

    public AddDataMart = (dm: Interfaces.IDataMartDTO) => {
        let vm = this;
        this.DeletedDataMarts = ko.utils.arrayFilter(this.DeletedDataMarts, (item) => { return item.DataMartID() !== dm.ID });
        //Get the request types for the datamart

        //Add them to the list of request types
        DataMarts.GetRequestTypesByDataMarts([dm.ID]).done((results) => {
            this.AddRequestTypes(results);
            vm.ProjectDataMarts.push(new ProjectDataMartViewModel({
                DataMartID: dm.ID,
                DataMart: dm.Name,
                Organization: dm.Organization,
                ProjectID: this.Project.ID(),
                RequestTypes: results,
                Project: this.Project.Name(),
                ProjectAcronym: this.Project.Acronym()
            }, vm.PermissionList, vm.EventList, vm.ProjectDataMartEventList, vm.SecurityGroupTree, vm.DataMartAcls, vm.DataMartRequestTypeAcls, vm.ProjectDataMartEvents));
        });
    }

    public RemoveDataMart = (pdm: ProjectDataMartViewModel) => {
        let vm = this;
        Global.Helpers.ShowConfirm("Confirm DataMart Removal", "<p>Are you sure that you wish to remove " + pdm.DataMart() + " from the project?</p>").done(() => {

            if (pdm.ProjectID())
                vm.DeletedDataMarts.push(pdm);

            pdm.DataMartSecurity.ClearAllGroups();
            pdm.DataMartRequestTypeSecurity.ClearAllGroups();
            pdm.DataMartEvents.ClearAllGroups();

            $('#acl' + pdm.DataMartID()).remove();
            vm.ProjectDataMarts.remove(pdm); //Remove from the list.
            vm.UpdateRequestTypes(); //Update the request types that are availble.
        });
    }

    public AddRequestTypes(requestTypes: Interfaces.IRequestTypeDTO[]) {
        requestTypes.forEach((newrt) => {
            var exists = false;
            this.RequestTypes().forEach((oldrt) => {
                if (oldrt.ID() == newrt.ID) {
                    exists = true;
                    return;
                }
            });

            if (!exists) {
                this.RequestTypes.push(new ViewModels.RequestTypeViewModel(newrt));
            }
        });
    }

    public AddProjectRequestType(requestType: Interfaces.IRequestTypeDTO) {
        this.ProjectRequestTypes.push(new ProjectRequestTypeViewModel({
            ProjectID: this.Project.ID(),
            RequestType: requestType.Name,
            RequestTypeID: requestType.ID,
            Workflow: requestType.Workflow,
            WorkflowID: requestType.WorkflowID
        }, this));

        this.RequestTypes.push(new ViewModels.RequestTypeViewModel(requestType));
    }

    public DeleteProjectRequestType(requestType: ProjectRequestTypeViewModel) {
        this.ProjectRequestTypes.remove(requestType);
        this.RequestTypes.remove((item) => {
            return item.ID() == requestType.RequestTypeID();
        });
    }

    public AddSecurityGroup() {
        let vm = this;
        vm.Save(null, null, false).done(() => {
            window.location.href = '/securitygroups/details?OwnerID=' + vm.Project.ID();
        })
            .fail((err) => {
                debugger;
            });
    }

    public UpdateRequestTypes(): JQueryDeferred<void> {
        let self = this;
        let deferred = $.Deferred<void>();
        let dataMartIDs = this.ProjectDataMarts().map((dm) => { return dm.DataMartID(); });
        DataMarts.GetRequestTypesByDataMarts(dataMartIDs).done((results) => {
            //Remove ones that are there
            for (let i = self.RequestTypes().length - 1; i >= 0; i--) {
                let oldrt = this.RequestTypes()[i];

                let exists = false;
                results.forEach((newrt) => {
                    if (oldrt.ID() == newrt) {
                        exists = true;
                        return;
                    }
                });

                if (exists)
                    continue;

                self.RequestTypes.remove(oldrt);
            }

            //Add ones that aren't there
            results.forEach((newrt) => {
                var exists = false;
                self.RequestTypes().forEach((oldrt) => {
                    if (oldrt.ID() == newrt.ID) {
                        exists = true;
                        return;
                    }
                });

                if (!exists)
                    self.RequestTypes.push(new ViewModels.RequestTypeViewModel(newrt));
            });

            deferred.resolve();
        });

        return deferred;
    }

    public AddOrganization(o: ViewModels.OrganizationViewModel) {
        let vm = this;
        vm.Organizations.push(new OrganizationViewModel({
            Organization: o.Name(),
            OrganizationID: o.ID(),
            Project: vm.Project.Name(),
            ProjectID: vm.Project.ID()
        }, vm.PermissionList, vm.EventList, vm.SecurityGroupTree, vm.OrganizationAcls, vm.OrganizationEvents));
    }

    public RemoveOrganization(o: OrganizationViewModel) {
        let vm = this;
        Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the project?</p>").done(() => {

            if (o.ProjectID())
                vm.DeletedOrganizations.push(o);

            o.OrganizationSecurity.ClearAllGroups();
            o.OrganizationEvents.ClearAllGroups();

            $('#acl' + o.OrganizationID()).remove();
            vm.Organizations.remove(o);
        });
    }

    public Cancel() {
        window.location.href = "/projects";
    }

    public Delete() {
        let vm = this;
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this project?</p>").done(() => {
            Projects.Delete([vm.Project.ID()]).done(() => {
                window.location.href = document.referrer;
            });
        });
    }

    public Copy() {
        Projects.Copy(this.Project.ID()).done((results) => {
            var newProjectID = results[0];

            window.location.href = "/projects/details?ID=" + newProjectID;
        });
    }

    public static TranslateSecurityGroupKind(kind: Enums.SecurityGroupKinds) : string {
        return Global.Helpers.GetEnumString(Enums.SecurityGroupKindsTranslation, kind);
    }
}

//First we get the ID that was passed as a param to the page. (this will be null for a new project)
let id = Global.GetQueryParam("ID");

//Get the GroupID that was passed as a param to the page, if we got here from the Group Detail screen.
let groupid = Global.GetQueryParam("GroupID");

//Then we call all of the database calls that are necessary. By putting them in a $.when they all execute simultaniously and will complete at the length of the longest running request.
$.when<any>(
    id == null ? null : Projects.GetPermissions([id], [
        PMNPermissions.Project.Copy,
        PMNPermissions.Project.Delete,
        PMNPermissions.Project.Edit,
        PMNPermissions.Project.ManageSecurity,
        PMNPermissions.Project.ManageRequestTypes]),
    id == null ? null : Projects.Get(id),
    Groups.List(null, "ID,Name"),
    DataMarts.List(null, "ID,Name,Organization", "Name"),
    id == null ? null : Security.GetProjectRequestTypeWorkflowActivityPermissions(id),
    Security.GetPermissionsByLocation([Enums.PermissionAclTypes.Projects,
    Enums.PermissionAclTypes.ProjectDataMarts,
    Enums.PermissionAclTypes.ProjectDataMartRequestTypes,
    Enums.PermissionAclTypes.ProjectOrganizations,
    Enums.PermissionAclTypes.ProjectRequestTypeWorkflowActivity
    ]),
    id == null ? null : ProjectDataMarts.ListWithRequestTypes("ProjectID eq " + id),
    Security.GetProjectDataMartPermissions(id ? id : Constants.GuidEmpty, null),
    Security.GetProjectPermissions(id ? id : Constants.GuidEmpty),
    Security.GetProjectRequestTypePermissions(id ? id : Constants.GuidEmpty),
    Security.GetProjectDataMartRequestTypePermissions(id ? id : Constants.GuidEmpty, null),
    Events.GetProjectEventPermissions(id ? id : Constants.GuidEmpty),
    Events.GetProjectDataMartEventPermissions(id ? id : Constants.GuidEmpty, null),
    Security.GetProjectOrganizationPermissions(id ? id : Constants.GuidEmpty, null),
    Events.GetProjectOrganizationEventPermissions(id ? id : Constants.GuidEmpty, null),
    Security.GetAvailableSecurityGroupTree(),
    id == null ? null : SecurityGroups.List("OwnerID eq " + id),
    id == null ? null : ProjectOrganizations.List("ProjectID eq " + id),
    Events.GetEventsByLocation([Enums.PermissionAclTypes.Projects, Enums.PermissionAclTypes.ProjectDataMarts]),
    RequestTypes.ListAvailableRequestTypes(null, null, "Name"),
    id == null ? [] : Security.GetProjectFieldOptionPermissions(id)
).done(( //This is the results for each of the calls. These are not typed in a when and have to be manually typed.
    screenPermissions: any[],
    projects: Interfaces.IProjectDTO,
    groups,
    dataMartList,
    projectRequestTypeWorkflowActivityAcls: Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[],
    permissionList,
    projectDataMarts,
    projectDataMartPermissions,
    projectPermissions,
    projectRequestTypePermissions,
    projectDataMartRequestTypePermissions,
    projectEventPermissions,
    projectDataMartEventPermissions,
    projectOrganizationPermissions,
    projectOrganizationEvents,
    securityGroupTree,
    projectSecurityGroups,
    projectOrganizations,
    eventList,
    requestTypeList,
    fieldOptions) => {

    var project: Interfaces.IProjectDTO = projects == null ? null : (Array.isArray(projects) ? projects[0]: projects);
    //Now have our conditional queries that need to be executed. These should be items that depend on other items that you just got back to be queried.
    if (project != null && project.GroupID) {

        $.when<any>(
            screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Projects.GetProjectRequestTypes(id) : null,
            screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Projects.GetRequestTypes(id) : null,
            Organizations.ListByGroupMembership(project.GroupID))
            .done((
                projectRequestTypes: Interfaces.IProjectRequestTypeDTO[],
                requestTypes: Interfaces.IRequestTypeDTO[],
                organizationList) => {
                $(() => {
                    //We get our binding control here because it's inside the document.ready. It cannot be assured anywhere else.
                    var bindingControl = $("#Content");
                    //Pass everything in to the view model here.

                    let vm = new ViewModel(
                        screenPermissions || [PMNPermissions.Project.Edit, PMNPermissions.Project.ManageSecurity],
                        project,
                        projectRequestTypes || [],
                        projectRequestTypeWorkflowActivityAcls || [],
                        groups,
                        dataMartList || [],
                        organizationList || [],
                        permissionList,
                        requestTypes || [],
                        projectDataMarts || [],
                        projectDataMartPermissions,
                        projectPermissions,
                        projectRequestTypePermissions,
                        projectDataMartRequestTypePermissions,
                        projectEventPermissions,
                        projectDataMartEventPermissions,
                        projectOrganizationPermissions,
                        projectOrganizationEvents,
                        securityGroupTree,
                        projectSecurityGroups || [],
                        projectOrganizations || [],
                        ko.utils.arrayFilter(eventList || [], function (evt) { return evt.Locations.indexOf(Enums.PermissionAclTypes.Projects) >= 0; }),
                        ko.utils.arrayFilter(eventList || [], function (evt) { return evt.Locations.indexOf(Enums.PermissionAclTypes.ProjectDataMarts) >= 0; }),
                        groupid,
                        requestTypeList,
                        fieldOptions,
                        bindingControl);

                    //Apply your bindings.
                    ko.applyBindings(vm, bindingControl[0]);
                    $('#PageLoadingMessage').remove();
                });
            });
    } else {

        $.when<any>(
            id != null ? screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Projects.GetProjectRequestTypes(id) : [] : [],
            id != null ? screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Projects.GetRequestTypes(id) : [] : []
        ).done((
            projectRequestTypes: Interfaces.IProjectRequestTypeDTO[],
            requestTypes: Interfaces.IRequestTypeDTO[]) => {


            $(() => {
                var bindingControl = $("#Content");

                let vm = new ViewModel(screenPermissions || [PMNPermissions.Project.Edit,
                PMNPermissions.Project.ManageSecurity,
                PMNPermissions.Project.ManageRequestTypes],
                    project,
                    projectRequestTypes,
                    projectRequestTypeWorkflowActivityAcls || [],
                    groups, dataMartList || [],
                    [],
                    permissionList,
                    requestTypes,
                    projectDataMarts || [],
                    projectDataMartPermissions,
                    projectPermissions,
                    projectRequestTypePermissions,
                    projectDataMartRequestTypePermissions,
                    projectEventPermissions,
                    projectDataMartEventPermissions,
                    projectOrganizationPermissions,
                    projectOrganizationEvents,
                    securityGroupTree,
                    projectSecurityGroups || [],
                    projectOrganizations || [],
                    ko.utils.arrayFilter(eventList || [], function (evt) { return evt.Locations.indexOf(Enums.PermissionAclTypes.Projects) >= 0; }),
                    ko.utils.arrayFilter(eventList || [], function (evt) { return evt.Locations.indexOf(Enums.PermissionAclTypes.ProjectDataMarts) >= 0; }),
                    groupid,
                    requestTypeList,
                    fieldOptions,
                    bindingControl);

                ko.applyBindings(vm, bindingControl[0]);
                $('#PageLoadingMessage').remove();
            });
        });

    }


});

export class ProjectDataMartViewModel extends ViewModels.ProjectDataMartWithRequestTypesViewModel {
    //This references the ACL editor (which must be added to the pages' script section) and passes everything it needs to work
    public DataMartSecurity: SecurityViewModels.AclEditViewModel<ViewModels.AclProjectDataMartViewModel>;
    ////This is the Request Type ACL Editor. Same applies here.
    public DataMartRequestTypeSecurity: SecurityAclRequestTypes.AclRequestTypeEditViewModel<ViewModels.AclProjectDataMartRequestTypeViewModel>;
    //this is the Event editor. Same applies here.
    public DataMartEvents: EventsAcl.EventAclEditViewModel<ViewModels.DataMartEventViewModel>;
    //This is the list of Request Types that are applicable for the given data mart. It's an observable array so that it can be updated dynamically.
    public RequestTypeList: KnockoutObservableArray<ViewModels.RequestTypeViewModel>;
    //This is just the toggle to show/hide the details in the list.
    public ShowAcls: KnockoutObservable<boolean>;
    constructor(
        ProjectDataMartDTO: Interfaces.IProjectDataMartWithRequestTypesDTO,
        permissionList: Interfaces.IPermissionDTO[],
        eventList: Interfaces.IEventDTO[],
        projectDataMartsEventList: Interfaces.IEventDTO[],
        securityGroupTree: Interfaces.ITreeItemDTO[],
        dataMartAcls: KnockoutObservableArray<ViewModels.AclProjectDataMartViewModel>,
        dataMartRequestTypeAcls: KnockoutObservableArray<ViewModels.AclProjectDataMartRequestTypeViewModel>,
        dataMartEvents: KnockoutObservableArray<ViewModels.ProjectDataMartEventViewModel>) {
        super(ProjectDataMartDTO);

        this.ShowAcls = ko.observable(false);

        //This is the definition for the view model that handles the Acls.
        //Note that the targets are the fields of the values that are for the specific acl (DataMartID and ProjectID) and it specifies what values should be added to any new acls that are created in the editor. The final param is the type it's going to create for new acls so that it knows what fields should be included ETC. If you run into errors saving, it's probably because you copied and pasted and did not change this type.
        this.DataMartSecurity = new SecurityViewModels.AclEditViewModel(permissionList.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.ProjectDataMarts) > -1;
        }), securityGroupTree, dataMartAcls,
            [
                {
                    Field: "DataMartID",
                    Value: ProjectDataMartDTO.DataMartID
                },
                {
                    Field: "ProjectID",
                    Value: ProjectDataMartDTO.ProjectID
                }
            ], ViewModels.AclProjectDataMartViewModel
        );

        //This pulls the request types right from the ProjectDataMart as queried from the DB. This is intentional for this special case so that it only shows the supported request types for that data mart. The overall version shows all request types supported by the project and updates as needed.
        this.RequestTypeList = ko.observableArray(ProjectDataMartDTO.RequestTypes.map((rt) => {
            return new ViewModels.RequestTypeViewModel(rt);
        }));

        this.DataMartRequestTypeSecurity = new SecurityAclRequestTypes.AclRequestTypeEditViewModel(this.RequestTypeList, securityGroupTree, dataMartRequestTypeAcls,
            [
                {
                    Field: "DataMartID",
                    Value: ProjectDataMartDTO.DataMartID
                },
                {
                    Field: "ProjectID",
                    Value: ProjectDataMartDTO.ProjectID
                }
            ], ViewModels.AclProjectDataMartRequestTypeViewModel
        );

        this.DataMartEvents = new EventsAcl.EventAclEditViewModel(projectDataMartsEventList, securityGroupTree, dataMartEvents, [
            {
                Field: "DataMartID",
                Value: ProjectDataMartDTO.DataMartID
            },
            {
                Field: "ProjectID",
                Value: ProjectDataMartDTO.ProjectID
            }
        ], ViewModels.ProjectDataMartEventViewModel);
    }

    public ToggleAcls() {
        this.ShowAcls(!this.ShowAcls());
    }
}

//This is the Organization equivalent of the Data Mart View model. The same rules apply.
export class OrganizationViewModel extends ViewModels.ProjectOrganizationViewModel {

    public OrganizationSecurity: SecurityViewModels.AclEditViewModel<ViewModels.AclProjectOrganizationViewModel>;
    public OrganizationEvents: EventsAcl.EventAclEditViewModel<ViewModels.ProjectOrganizationEventViewModel>;

    public ShowAcls: KnockoutObservable<boolean>;
    constructor(
        organizationDTO: Interfaces.IProjectOrganizationDTO,
        permissionList: Interfaces.IPermissionDTO[],
        eventList: Interfaces.IEventDTO[],
        securityGroupTree: Interfaces.ITreeItemDTO[],
        organizationAcls: KnockoutObservableArray<ViewModels.AclProjectOrganizationViewModel>,
        organizationEvents: KnockoutObservableArray<ViewModels.ProjectOrganizationEventViewModel>) {
        super(organizationDTO);

        this.ShowAcls = ko.observable(false);

        this.OrganizationSecurity = new SecurityViewModels.AclEditViewModel(permissionList.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.ProjectOrganizations) > -1;
        }), securityGroupTree, organizationAcls,
            [
                {
                    Field: "OrganizationID",
                    Value: organizationDTO.OrganizationID
                },
                {
                    Field: "ProjectID",
                    Value: organizationDTO.ProjectID
                }
            ], ViewModels.AclProjectOrganizationViewModel
        );

        this.OrganizationEvents = new EventsAcl.EventAclEditViewModel(eventList.filter((e) => {
            return e.Locations.indexOf(Enums.PermissionAclTypes.ProjectOrganizations) > -1;
        }), securityGroupTree, organizationEvents, [
            {
                Field: "OrganizationID",
                Value: organizationDTO.OrganizationID
            },
            {
                Field: "ProjectID",
                Value: organizationDTO.ProjectID
            }
        ], ViewModels.ProjectOrganizationEventViewModel);

    }

    public ToggleAcls() {
        this.ShowAcls(!this.ShowAcls());
    }
}


export class ProjectRequestTypeViewModel extends ViewModels.ProjectRequestTypeViewModel {
    public Selected: KnockoutObservable<boolean> = ko.observable(false);
    public Activities: KnockoutObservableArray<ViewModels.WorkflowActivityViewModel>;
    public onSelected: () => void;

    constructor(projectRequestType: Interfaces.IProjectRequestTypeDTO, vm: ViewModel) {
        super(projectRequestType);
        this.Activities = ko.observableArray<ViewModels.WorkflowActivityViewModel>();
        let self = this;
        this.onSelected = () => {
            self.Selected(!self.Selected());
            if (self.Selected() && self.Activities().length == 0) {
                Workflow.GetWorkflowActivitiesByWorkflowID(self.WorkflowID(), "ID ne cc2e0001-9b99-4c67-8ded-a3b600e1c696", null, "Name").done((activities) => { //Don't show the terminate activity
                    self.Activities.push.apply(self.Activities, activities.map((item) => { return new WorkflowActivityViewModel(item, self, vm); }));
                });
            }
        };
    }
}

export class WorkflowActivityViewModel extends ViewModels.WorkflowActivityViewModel {
    public Selected: KnockoutObservable<boolean> = ko.observable(false);

    //Security computed here
    public Security: SecurityViewModels.AclEditViewModel<ViewModels.AclProjectRequestTypeWorkflowActivityViewModel>;

    constructor(workflowActivity: Interfaces.IWorkflowActivityDTO, requestType: ProjectRequestTypeViewModel, vm: ViewModel) {
        super(workflowActivity);
        this.Security = new SecurityViewModels.AclEditViewModel(vm.PermissionList.filter((p) => {
            return p.Locations.indexOf(Enums.PermissionAclTypes.ProjectRequestTypeWorkflowActivity) > -1;
        }), vm.SecurityGroupTree, vm.ProjectRequestTypeWorkflowActivityAcls, [
            {
                Field: "ProjectID",
                Value: vm.Project.ID()
            },
            {
                Field: "RequestTypeID",
                Value: requestType.RequestTypeID()
            },
            {
                Field: "WorkflowActivityID",
                Value: this.ID()
            }
        ], ViewModels.AclProjectRequestTypeWorkflowActivityViewModel);
    }

    public onSelected(data: WorkflowActivityViewModel) {
        data.Selected(!data.Selected());
    }
}