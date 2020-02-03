/// <reference path="../_rootlayout.ts" />
/// <reference path="../security/aclviewmodel.ts" />
/// <reference path="../events/editeventpermissions.ts" />

module Projects.Details {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {

        public Project: Dns.ViewModels.ProjectViewModel;
        public ProjectDataMarts: KnockoutObservableArray<ProjectDataMartViewModel>;
        public ProjectRequestTypes: KnockoutObservableArray<ProjectRequestTypeViewModel>;

        public SecurityGroups: KnockoutObservableArray<Dns.ViewModels.SecurityGroupViewModel>;
        public Organizations: KnockoutObservableArray<OrganizationViewModel>;
        public DataMartList: Dns.Interfaces.IDataMartDTO[];
        public OrganizationList: KnockoutObservableArray<Dns.ViewModels.OrganizationViewModel>;
        public PermissionList: Dns.Interfaces.IPermissionDTO[];
        public EventList: Dns.Interfaces.IEventDTO[];
        public ProjectDataMartEventList: Dns.Interfaces.IEventDTO[];
        public SecurityGroupTree: Dns.Interfaces.ITreeItemDTO[];
        public RequestTypes: KnockoutObservableArray<Dns.ViewModels.RequestTypeViewModel>;
        public AddableDataMartList: KnockoutComputed<Dns.Interfaces.IDataMartDTO[]>;
        public AddableOrganizationList: KnockoutComputed<Dns.ViewModels.OrganizationViewModel[]>;

        public AddableRequestTypeList: KnockoutComputed<Dns.Interfaces.IRequestTypeDTO[]>;
        public RequestTypeList: Dns.Interfaces.IRequestTypeDTO[];

        public Groups: Dns.Interfaces.IGroupDTO[];

        public ProjectAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectViewModel>;
        public ProjectRequestTypeAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectRequestTypeViewModel>;
        public ProjectEvents: KnockoutObservableArray<Dns.ViewModels.ProjectEventViewModel>;
        public DataMartAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectDataMartViewModel>;
        public ProjectDataMartEvents: KnockoutObservableArray<Dns.ViewModels.ProjectDataMartEventViewModel>;
        public DataMartRequestTypeAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectDataMartRequestTypeViewModel>;
        public OrganizationAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectOrganizationViewModel>;
        public OrganizationEvents: KnockoutObservableArray<Dns.ViewModels.ProjectOrganizationEventViewModel>;
        public ProjectRequestTypeWorkflowActivityAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectRequestTypeWorkflowActivityViewModel>;

        public ProjectSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclProjectViewModel>;
        public ProjectRequestTypesSecurity: Security.Acl.RequestTypes.AclRequestTypeEditViewModel<Dns.ViewModels.AclProjectRequestTypeViewModel>;
        public ProjectEventSecurity: Events.Acl.EventAclEditViewModel<Dns.ViewModels.ProjectEventViewModel>;

        public FieldOptions: Security.Acl.FieldOption.AclFieldOptionEditViewModel<Dns.ViewModels.BaseFieldOptionAclViewModel>;
        public FieldOptionAcl: KnockoutObservableArray<Dns.ViewModels.AclProjectFieldOptionViewModel>;

        public CanManageSecurityTypes: KnockoutObservable<boolean>;

        public Save: (data, event: JQueryEventObject, showPrompt: boolean) => JQueryDeferred<void>;

        constructor(
            screenPermissions: any[],
            project: Dns.Interfaces.IProjectDTO,
            projectRequestTypes: Dns.Interfaces.IProjectRequestTypeDTO[],
            projectRequestTypeWorkflowActivityAcls,
            groups: Dns.Interfaces.IGroupDTO[],
            dataMartList: Dns.Interfaces.IDataMartDTO[],
            organizationList: Dns.Interfaces.IOrganizationDTO[],
            permissionList: Dns.Interfaces.IPermissionDTO[],
            requestTypes: Dns.Interfaces.IRequestTypeDTO[],
            projectDataMarts: Dns.Interfaces.IProjectDataMartWithRequestTypesDTO[],
            projectDataMartPermissions: Dns.Interfaces.IAclProjectDataMartDTO[],
            projectPermissions: Dns.Interfaces.IAclProjectDTO[],
            projectRequestTypePermissions: Dns.Interfaces.IAclProjectRequestTypeDTO[],
            projectDataMartRequestTypePermissions: Dns.Interfaces.IAclProjectDataMartRequestTypeDTO[],
            projectEventPermissions: Dns.Interfaces.IProjectEventDTO[],
            projectDataMartEventPermissions: Dns.Interfaces.IProjectDataMartEventDTO[],
            projectOrganizationPermissions: Dns.Interfaces.IAclProjectOrganizationDTO[],
            projectOrganizationEventPermissions: Dns.Interfaces.IProjectOrganizationEventDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            projectSecurityGroups: Dns.Interfaces.ISecurityGroupDTO[],
            projectOrganizations: Dns.Interfaces.IProjectOrganizationDTO[],
            eventList: Dns.Interfaces.IEventDTO[],
            projectDataMartEventsList: Dns.Interfaces.IEventDTO[],
            groupid: any,
            requestTypeList: Dns.Interfaces.IRequestTypeDTO[],
            fieldOptions: Dns.Interfaces.IAclProjectFieldOptionDTO[],
            bindingControl: JQuery) {
            super(bindingControl, screenPermissions);

            var self = this;

            this.CanManageSecurityTypes = ko.observable(this.HasPermission(Permissions.Project.ManageRequestTypes));

            //Get lists
            this.DataMartList = dataMartList;
            this.OrganizationList = ko.observableArray(organizationList.map((item) => {
                return new Dns.ViewModels.OrganizationViewModel(item);
            }));
            this.PermissionList = permissionList;
            this.EventList = eventList;
            this.ProjectDataMartEventList = projectDataMartEventsList;
            this.SecurityGroupTree = securityGroupTree;
            this.RequestTypes = ko.observableArray<Dns.ViewModels.RequestTypeViewModel>(requestTypes.map((item) => {
                return new Dns.ViewModels.RequestTypeViewModel(item);
            }));
            this.RequestTypeList = requestTypeList;
            this.Groups = groups;
            this.Project = new Dns.ViewModels.ProjectViewModel(project);
            this.Project.StartDate = ko.observable(project == null ? null : moment(project.StartDate).toDate());
            this.Project.EndDate = ko.observable(project == null ? null : moment(project.EndDate).toDate());
            this.SecurityGroups = ko.observableArray(projectSecurityGroups.map((sg) => {
                return new Dns.ViewModels.SecurityGroupViewModel(sg);
            }));

            this.ProjectRequestTypes = ko.observableArray(projectRequestTypes.map((item) => {
                return new ProjectRequestTypeViewModel(item);
            }));          

            //Acls
            this.ProjectAcls = ko.observableArray(projectPermissions.map((item) => {
                return new Dns.ViewModels.AclProjectViewModel(item);
            }));
            this.ProjectRequestTypeAcls = ko.observableArray(projectRequestTypePermissions.map((item) => {
                return new Dns.ViewModels.AclProjectRequestTypeViewModel(item);
            }));

            this.ProjectRequestTypeWorkflowActivityAcls = ko.observableArray<Dns.ViewModels.AclProjectRequestTypeWorkflowActivityViewModel>(projectRequestTypeWorkflowActivityAcls.map((item) => {
                return new Dns.ViewModels.AclProjectRequestTypeWorkflowActivityViewModel(item);
            }));

            this.ProjectEvents = ko.observableArray(projectEventPermissions.map((item) => {
                return new Dns.ViewModels.ProjectEventViewModel(item);
            }));
            this.ProjectDataMartEvents = ko.observableArray(projectDataMartEventPermissions.map((item) => {
                return new Dns.ViewModels.ProjectDataMartEventViewModel(item);
            }));
            this.DataMartAcls = ko.observableArray(projectDataMartPermissions.map((item) => {
                return new Dns.ViewModels.AclProjectDataMartViewModel(item);
            }));
            this.DataMartRequestTypeAcls = ko.observableArray(projectDataMartRequestTypePermissions.map((item) => {
                return new Dns.ViewModels.AclProjectDataMartRequestTypeViewModel(item);
            }));
            this.OrganizationAcls = ko.observableArray(projectOrganizationPermissions.map((item) => {
                return new Dns.ViewModels.AclProjectOrganizationViewModel(item);
            }));
            this.OrganizationEvents = ko.observableArray(projectOrganizationEventPermissions.map((item) => {
                return new Dns.ViewModels.ProjectOrganizationEventViewModel(item);
            }));

            this.ProjectSecurity = new Security.Acl.AclEditViewModel(permissionList.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
            }), securityGroupTree, this.ProjectAcls, [
                    {
                        Field: "ProjectID",
                        Value: this.Project.ID()
                    }
                ], Dns.ViewModels.AclProjectViewModel);

            this.ProjectRequestTypesSecurity = new Security.Acl.RequestTypes.AclRequestTypeEditViewModel(this.RequestTypes, securityGroupTree, this.ProjectRequestTypeAcls, [
                {
                    Field: "ProjectID",
                    Value: this.Project.ID()
                }
            ], Dns.ViewModels.AclProjectRequestTypeViewModel);

            this.ProjectEventSecurity = new Events.Acl.EventAclEditViewModel(eventList.filter((e) => {
                return e.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
            }), securityGroupTree, this.ProjectEvents, [
                    {
                        Field: "ProjectID",
                        Value: this.Project.ID()
                    }
            ], Dns.ViewModels.ProjectEventViewModel);

            this.FieldOptionAcl = ko.observableArray(fieldOptions.map((e) => {
                if(e.ProjectID != self.Project.ID())
                    e.ProjectID = self.Project.ID();
                return new Dns.ViewModels.AclProjectFieldOptionViewModel(e);
            }));

            this.FieldOptions = new Security.Acl.FieldOption.AclFieldOptionEditViewModel(fieldOptions, securityGroupTree, this.FieldOptionAcl, [
                {
                    Field: "ProjectID",
                    Value: self.Project.ID()
                }], Dns.ViewModels.AclProjectFieldOptionViewModel);

            this.ProjectDataMarts = ko.observableArray(projectDataMarts.map((item) => { return new ProjectDataMartViewModel(item, this.PermissionList, this.EventList, this.ProjectDataMartEventList, this.SecurityGroupTree, this.DataMartAcls, this.DataMartRequestTypeAcls, this.ProjectDataMartEvents); }));

            this.Organizations = ko.observableArray(projectOrganizations.map((o) => {
                return new OrganizationViewModel(o, this.PermissionList, this.EventList, this.SecurityGroupTree, this.OrganizationAcls, this.OrganizationEvents);
            }));


            //Set the Title. Every page should have this.
            this.WatchTitle(this.Project.Name, "Project: ");

            //List of datamarts that can be added to the project
            this.AddableDataMartList = ko.computed<Dns.Interfaces.IDataMartDTO[]>(() => {
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

            this.AddableOrganizationList = ko.computed<Dns.ViewModels.OrganizationViewModel[]>(() => {
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

            this.AddableRequestTypeList = ko.computed<Dns.Interfaces.IRequestTypeDTO[]>(() => {
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

                (value == null ? null : Dns.WebApi.Organizations.ListByGroupMembership(value)).done((organizationList) => {
                    this.OrganizationList = ko.observableArray(organizationList.map((item) => { return new Dns.ViewModels.OrganizationViewModel(item); }) || []);
                });
            });

            if (groupid != null)
                this.Project.GroupID(groupid);

            self.Save = (data, event: JQueryEventObject, showPrompt: boolean = true) => {

                var deferred = $.Deferred<void>();

                if (!self.Validate()) {
                    deferred.reject();
                    if (event != null)
                        event.preventDefault();
                    return;
                }

                //Removed by request of HPCI
                //if (this.ProjectAcls().length == 0) {
                //    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have setup at least one group or user that has access to the project.</p>");
                //    deferred.reject();
                //    return;
                //}

                //Only save if the root project saved.
                Dns.WebApi.Projects.InsertOrUpdate([self.Project.toData()]).done((project) => {
                    //Update the values for the ID and timestamp as necessary.
                    self.Project.ID(project[0].ID);
                    self.Project.Timestamp(project[0].Timestamp);
                    window.history.replaceState(null, window.document.title, "/projects/details?ID=" + project[0].ID);

                    Layout.vmHeader.ReloadMenu();

                    //Save everything else
                    var dataMarts = self.ProjectDataMarts().map((dm) => {
                        dm.ProjectID(self.Project.ID());
                        return dm.toData();
                    });

                    var organizations = self.Organizations().map((o) => {
                        o.ProjectID(self.Project.ID());
                        return o.toData();
                    });
                    if (self.HasPermission(Permissions.Project.ManageSecurity)) {
                        var dataMartAcls = self.DataMartAcls().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var projectAcls = self.ProjectAcls().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var projectEventAcls = self.ProjectEvents().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var projectRequestTypeAcls = self.ProjectRequestTypeAcls().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var projectDataMartEventAcls = self.ProjectDataMartEvents().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var projectDataMartRequestTypeAcls = self.DataMartRequestTypeAcls().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var organizationAcls = self.OrganizationAcls().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var organizationEvents = self.OrganizationEvents().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var projectRequestTypeWorkFlowActivityAcls = self.ProjectRequestTypeWorkflowActivityAcls().map((a) => {
                            a.ProjectID(self.Project.ID());
                            return a.toData();
                        });

                        var projectFieldOptionsAcls = self.FieldOptions.Acls().map((a) => <Dns.Interfaces.IAclProjectFieldOptionDTO> a.toData());                        
                    }

                    var projectRequestTypes: Dns.Interfaces.IUpdateProjectRequestTypesDTO = null;

                    if (self.HasPermission(Permissions.Project.ManageRequestTypes)) {
                        var projectRequestTypes = {
                            ProjectID: self.Project.ID(),
                            RequestTypes: self.ProjectRequestTypes().map((rt) => {
                                rt.ProjectID(self.Project.ID());
                                return rt.toData();
                            })
                        };
                    }

                    var canManageSecurity = self.HasPermission(Permissions.Project.ManageSecurity);
                    var originalManageRequestTypes = self.HasPermission(Permissions.Project.ManageRequestTypes);
                    
                    $.when<any>(
                        canManageSecurity ? Dns.WebApi.Security.UpdateProjectPermissions(projectAcls) : null,
                        canManageSecurity ? Dns.WebApi.Events.UpdateProjectEventPermissions(projectEventAcls) : null,
                        canManageSecurity ? Dns.WebApi.Events.UpdateProjectDataMartEventPermissions(projectDataMartEventAcls) : null,
                        canManageSecurity ? Dns.WebApi.Events.UpdateProjectOrganizationEventPermissions(organizationEvents) : null,
                        canManageSecurity ? Dns.WebApi.Security.UpdateProjectOrganizationPermissions(organizationAcls) : null,
                        canManageSecurity ? Dns.WebApi.Security.UpdateProjectDataMartPermissions(dataMartAcls) : null,
                        canManageSecurity && (projectFieldOptionsAcls != null && projectFieldOptionsAcls.length > 0) ? Dns.WebApi.Security.UpdateProjectFieldOptionPermissions(projectFieldOptionsAcls) : null
                        ).done(() => {
                    
                        //update permissions
                        Dns.WebApi.Projects.GetPermissions([self.Project.ID()], [
                            Permissions.Project.Copy,
                            Permissions.Project.Delete,
                            Permissions.Project.Edit,
                            Permissions.Project.ManageSecurity,
                            Permissions.Project.ManageRequestTypes]).done((perms) => {
                        
                            //update the screen permissions with the newly set permissions
                            self.ScreenPermissions = perms.map((sp: string) => {
                                return sp.toLowerCase();
                            });

                            //now update the rest of the stuff using the new permissions
                            var canManageRequestTypes = self.HasPermission(Permissions.Project.ManageRequestTypes);
                            self.CanManageSecurityTypes(canManageRequestTypes);

                            $.when<any>(
                                canManageRequestTypes ? Dns.WebApi.Security.UpdateProjectRequestTypePermissions(projectRequestTypeAcls) : null,
                                canManageRequestTypes ? Dns.WebApi.Security.UpdateProjectDataMartRequestTypePermissions(projectDataMartRequestTypeAcls) : null
                                ).done(() => {

                                $.when<any>(
                                    Dns.WebApi.ProjectDataMarts.InsertOrUpdate({ ProjectID: self.Project.ID(), DataMarts: dataMarts }),
                                    Dns.WebApi.ProjectOrganizations.InsertOrUpdate({ ProjectID: self.Project.ID(), Organizations: organizations }),
                                    (canManageRequestTypes && projectRequestTypes != null) ? Dns.WebApi.Projects.UpdateProjectRequestTypes(projectRequestTypes) : null,
                                        (canManageRequestTypes && projectRequestTypeWorkFlowActivityAcls != null) ? Dns.WebApi.Security.UpdateProjectRequestTypeWorkflowActivityPermissions(projectRequestTypeWorkFlowActivityAcls) : null
                                //Add others here.
                                    ).done(() => {

                                    if (showPrompt) {
                                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>")
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
                });

                return deferred;

            };
        }

        public AddDataMart = (dm: Dns.Interfaces.IDataMartDTO) => {
            //Get the request types for the datamart

            //Add them to the list of request types
            Dns.WebApi.DataMarts.GetRequestTypesByDataMarts([dm.ID]).done((results) => {
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
            Global.Helpers.ShowConfirm("Confirm DataMart Removal", "<p>Are you sure that you wish to remove " + pdm.DataMart() + " from the project?</p>").done(() => {

                if (pdm.ProjectID())
                    Dns.WebApi.ProjectDataMarts.Remove([pdm.toData()]); //Can run async

                var ndx = vm.ProjectDataMarts.indexOf(pdm);
                // vm.ProjectDataMarts.splice(ndx, 1);
                $('#acl' + pdm.DataMartID()).remove();
                vm.ProjectDataMarts.remove(pdm); //Remove from the list.
                this.UpdateRequestTypes(); //Update the request types that are availble.
            });
        }

        public AddRequestTypes(requestTypes: Dns.Interfaces.IRequestTypeDTO[]) {
            requestTypes.forEach((newrt) => {
                var exists = false;
                this.RequestTypes().forEach((oldrt) => {
                    if (oldrt.ID() == newrt.ID) {
                        exists = true;
                        return;
                    }
                });

                if (!exists) {
                    this.RequestTypes.push(new Dns.ViewModels.RequestTypeViewModel(newrt));
                }
            });
        }

        public AddProjectRequestType(requestType: Dns.Interfaces.IRequestTypeDTO) {
            vm.ProjectRequestTypes.push(new ProjectRequestTypeViewModel({
                ProjectID: vm.Project.ID,
                RequestType: requestType.Name,
                RequestTypeID: requestType.ID,
                Template: requestType.Template,
                Workflow: requestType.Workflow,
                WorkflowID: requestType.WorkflowID
            }));

            vm.RequestTypes.push(new Dns.ViewModels.RequestTypeViewModel(requestType));
        }

        public DeleteProjectRequestType(requestType: ProjectRequestTypeViewModel) {
            vm.ProjectRequestTypes.remove(requestType);
            vm.RequestTypes.remove((item) => {
                return item.ID() == requestType.RequestTypeID();
            });
        }

        public AddSecurityGroup() {
            vm.Save(null, null, false).done(() => {
                window.location.href = '/securitygroups/details?OwnerID=' + vm.Project.ID();
            });
        }

        public UpdateRequestTypes(): JQueryDeferred<void> {
            var deferred = $.Deferred<void>();
            var dataMartIDs = this.ProjectDataMarts().map((dm) => { return dm.DataMartID(); });
            Dns.WebApi.DataMarts.GetRequestTypesByDataMarts(dataMartIDs).done((results) => {
                //Remove ones that are there
                for (var i = this.RequestTypes().length - 1; i >= 0; i--) {
                    var oldrt = this.RequestTypes()[i];

                    var exists = false;
                    results.forEach((newrt) => {
                        if (oldrt.ID() == newrt) {
                            exists = true;
                            return;
                        }
                    });

                    if (exists)
                        continue;

                    this.RequestTypes.remove(oldrt);
                }

                //Add ones that aren't there
                results.forEach((newrt) => {
                    var exists = false;
                    this.RequestTypes().forEach((oldrt) => {
                        if (oldrt.ID() == newrt.ID) {
                            exists = true;
                            return;
                        }
                    });

                    if (!exists)
                        this.RequestTypes.push(new Dns.ViewModels.RequestTypeViewModel(newrt));
                });

                deferred.resolve();
            });

            return deferred;
        }

        public AddOrganization(o: Dns.ViewModels.OrganizationViewModel) {
            vm.Organizations.push(new OrganizationViewModel({
                Organization: o.Name(),
                OrganizationID: o.ID(),
                Project: vm.Project.Name(),
                ProjectID: vm.Project.ID()
            }, vm.PermissionList, vm.EventList, vm.SecurityGroupTree, vm.OrganizationAcls, vm.OrganizationEvents));
        }

        public RemoveOrganization(o: OrganizationViewModel) {
            Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the project?</p>").done(() => {

                if (o.ProjectID())
                    Dns.WebApi.ProjectOrganizations.Remove([o.toData()]);

                $('#acl' + o.OrganizationID()).remove();
                vm.Organizations.remove(o);

                //if (o.ProjectID()) {
                //    Dns.WebApi.ProjectOrganizations.Remove([o.toData()]).done(() => {
                //        vm.Organizations.remove(o);
                //    });
                //} else {
                //vm.Organizations.remove(o);
                //}
            });
        }

        public Cancel() {
            window.history.back();
        }

        public Delete() {
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this project?</p>").done(() => {
                Dns.WebApi.Projects.Delete([vm.Project.ID()]).done(() => {
                    window.location.href = document.referrer;
                });
            });
        }

        public Copy() {
            Dns.WebApi.Projects.Copy(vm.Project.ID()).done((results) => {
                var newProjectID = results[0];

                window.location.href = "/projects/details?ID=" + newProjectID;
            });
        }
    }

    function init() {
        //First we get the ID that was passed as a param to the page. (this will be null for a new project)
        var id: any = $.url().param("ID");

        //Get the GroupID that was passed as a param to the page, if we got here from the Group Detail screen.
        var groupid: any = $.url().param("GroupID");

        //Then we call all of the database calls that are necessary. By putting them in a $.when they all execute simultaniously and will complete at the length of the longest running request.
        $.when<any>(
            id == null ? null : Dns.WebApi.Projects.GetPermissions([id], [
                Permissions.Project.Copy,
                Permissions.Project.Delete,
                Permissions.Project.Edit,
                Permissions.Project.ManageSecurity,
                Permissions.Project.ManageRequestTypes]),
            id == null ? null : Dns.WebApi.Projects.Get(id),
            Dns.WebApi.Groups.List(null, "ID,Name"),
            Dns.WebApi.DataMarts.List(null, "ID,Name,Organization", "Name"),
            id == null ? null : Dns.WebApi.Security.GetProjectRequestTypeWorkflowActivityPermissions(id),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Projects,
                Dns.Enums.PermissionAclTypes.ProjectDataMarts,
                Dns.Enums.PermissionAclTypes.ProjectDataMartRequestTypes,
                Dns.Enums.PermissionAclTypes.ProjectOrganizations,
                Dns.Enums.PermissionAclTypes.ProjectRequestTypeWorkflowActivity
            ]),
            id == null ? null : Dns.WebApi.ProjectDataMarts.ListWithRequestTypes("ProjectID eq " + id),
            Dns.WebApi.Security.GetProjectDataMartPermissions(id ? id : Constants.GuidEmpty, null),
            Dns.WebApi.Security.GetProjectPermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetProjectRequestTypePermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetProjectDataMartRequestTypePermissions(id ? id : Constants.GuidEmpty, null),
            Dns.WebApi.Events.GetProjectEventPermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Events.GetProjectDataMartEventPermissions(id ? id : Constants.GuidEmpty, null),
            Dns.WebApi.Security.GetProjectOrganizationPermissions(id ? id : Constants.GuidEmpty, null),
            Dns.WebApi.Events.GetProjectOrganizationEventPermissions(id ? id : Constants.GuidEmpty, null),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            id == null ? null : Dns.WebApi.SecurityGroups.List("OwnerID eq " + id),
            id == null ? null : Dns.WebApi.ProjectOrganizations.List("ProjectID eq " + id),
            Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Projects]),
            Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.ProjectDataMarts]),
            Dns.WebApi.RequestTypes.ListAvailableRequestTypes(null, null, "Name"),
            id == null ? [] : Dns.WebApi.Security.GetProjectFieldOptionPermissions(id)
            ).done(( //This is the results for each of the calls. These are not typed in a when and have to be manually typed.
            screenPermissions: any[],
            projects: Dns.Interfaces.IProjectDTO,
            groups,
            dataMartList,
            projectRequestTypeWorkflowActivityAcls: Dns.Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[],
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
            projectDataMartsEventList,
            requestTypeList,
            fieldOptions) => {
            var project: Dns.Interfaces.IProjectDTO = projects == null ? null : projects[0];
            //Now have our conditional queries that need to be executed. These should be items that depend on other items that you just got back to be queried.
            if (project != null && project.GroupID) {
                $.when<any>(
                    screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetProjectRequestTypes(id) : null,
                    screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetRequestTypes(id) : null,
                    Dns.WebApi.Organizations.ListByGroupMembership(project.GroupID))
                    .done((
                    projectRequestTypes: Dns.Interfaces.IProjectRequestTypeDTO[],
                    requestTypes: Dns.Interfaces.IRequestTypeDTO[],
                    organizationList) => {
                    $(() => {
                        //We get our binding control here because it's inside the document.ready. It cannot be assured anywhere else.
                        var bindingControl = $("#Content");
                        //Pass everything in to the view model here.
                        vm = new ViewModel(
                            screenPermissions || [Permissions.Project.Edit, Permissions.Project.ManageSecurity],
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
                            eventList || [],
                            projectDataMartsEventList || [],
                            groupid,
                            requestTypeList,
                            fieldOptions,
                            bindingControl);

                        //Apply your bindings.
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            } else {
                $.when<any>(
                    id != null ? screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetProjectRequestTypes(id) : [] : [],
                    id != null ? screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetRequestTypes(id) : [] : []
                        ).done((
                        projectRequestTypes: Dns.Interfaces.IProjectRequestTypeDTO[],
                        requestTypes: Dns.Interfaces.IRequestTypeDTO[]) => {
                        $(() => {
                            var bindingControl = $("#Content");

                            vm = new ViewModel(screenPermissions || [Permissions.Project.Edit,
                                Permissions.Project.ManageSecurity,
                                Permissions.Project.ManageRequestTypes],
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
                                eventList || [],
                                projectDataMartsEventList || [],
                                groupid,
                                requestTypeList,
                                fieldOptions,
                                bindingControl);

                            ko.applyBindings(vm, bindingControl[0]);
                        });
                    });

            }
        });
    }

    //This is called automatically because it's in the root. This means you don't have to setup a script tag in the page itself.
    init();

    //This is the datamart view model. It allows us to extend the default one and provide all of the options necessary for each data mart.
    export class ProjectDataMartViewModel extends Dns.ViewModels.ProjectDataMartWithRequestTypesViewModel {
        //This references the ACL editor (which must be added to the pages' script section) and passes everything it needs to work
        public DataMartSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclProjectDataMartViewModel>;
        //This is the Request Type ACL Editor. Same applies here.
        public DataMartRequestTypeSecurity: Security.Acl.RequestTypes.AclRequestTypeEditViewModel<Dns.ViewModels.AclProjectDataMartRequestTypeViewModel>;
        //this is the Event editor. Same applies here.
        public DataMartEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.DataMartEventViewModel>;
        //This is the list of Request Types that are applicable for the given data mart. It's an observable array so that it can be updated dynamically.
        public RequestTypeList: KnockoutObservableArray<Dns.ViewModels.RequestTypeViewModel>;
        //This is just the toggle to show/hide the details in the list.
        public ShowAcls: KnockoutObservable<boolean>;
        constructor(
            ProjectDataMartDTO: Dns.Interfaces.IProjectDataMartWithRequestTypesDTO,
            permissionList: Dns.Interfaces.IPermissionDTO[],
            eventList: Dns.Interfaces.IEventDTO[],
            projectDataMartsEventList: Dns.Interfaces.IEventDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            dataMartAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectDataMartViewModel>,
            dataMartRequestTypeAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectDataMartRequestTypeViewModel>,
            dataMartEvents: KnockoutObservableArray<Dns.ViewModels.ProjectDataMartEventViewModel>) {
            super(ProjectDataMartDTO);

            this.ShowAcls = ko.observable(false);

            //This is the definition for the view model that handles the Acls.
            //Note that the targets are the fields of the values that are for the specific acl (DataMartID and ProjectID) and it specifies what values should be added to any new acls that are created in the editor. The final param is the type it's going to create for new acls so that it knows what fields should be included ETC. If you run into errors saving, it's probably because you copied and pasted and did not change this type.
            this.DataMartSecurity = new Security.Acl.AclEditViewModel(permissionList.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.ProjectDataMarts) > -1;
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
                ], Dns.ViewModels.AclProjectDataMartViewModel
                );

            //This pulls the request types right from the ProjectDataMart as queried from the DB. This is intentional for this special case so that it only shows the supported request types for that data mart. The overall version shows all request types supported by the project and updates as needed.
            this.RequestTypeList = ko.observableArray(ProjectDataMartDTO.RequestTypes.map((rt) => {
                return new Dns.ViewModels.RequestTypeViewModel(rt);
            }));

            this.DataMartRequestTypeSecurity = new Security.Acl.RequestTypes.AclRequestTypeEditViewModel(this.RequestTypeList, securityGroupTree, dataMartRequestTypeAcls,
                [
                    {
                        Field: "DataMartID",
                        Value: ProjectDataMartDTO.DataMartID
                    },
                    {
                        Field: "ProjectID",
                        Value: ProjectDataMartDTO.ProjectID
                    }
                ], Dns.ViewModels.AclProjectDataMartRequestTypeViewModel
                );

            this.DataMartEvents = new Events.Acl.EventAclEditViewModel(projectDataMartsEventList, securityGroupTree, dataMartEvents, [
                    {
                        Field: "DataMartID",
                        Value: ProjectDataMartDTO.DataMartID
                    },
                    {
                        Field: "ProjectID",
                        Value: ProjectDataMartDTO.ProjectID
                    }
            ], Dns.ViewModels.ProjectDataMartEventViewModel);
        }

        public ToggleAcls() {
            this.ShowAcls(!this.ShowAcls());
        }
    }

    //This is the Organization equivalent of the Data Mart View model. The same rules apply.
    export class OrganizationViewModel extends Dns.ViewModels.ProjectOrganizationViewModel {

        public OrganizationSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclProjectOrganizationViewModel>;
        public OrganizationEvents: Events.Acl.EventAclEditViewModel<Dns.ViewModels.ProjectOrganizationEventViewModel>;

        public ShowAcls: KnockoutObservable<boolean>;
        constructor(
            organizationDTO: Dns.Interfaces.IProjectOrganizationDTO,
            permissionList: Dns.Interfaces.IPermissionDTO[],
            eventList: Dns.Interfaces.IEventDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            organizationAcls: KnockoutObservableArray<Dns.ViewModels.AclProjectOrganizationViewModel>,
            organizationEvents: KnockoutObservableArray<Dns.ViewModels.ProjectOrganizationEventViewModel>) {
            super(organizationDTO);

            this.ShowAcls = ko.observable(false);

            this.OrganizationSecurity = new Security.Acl.AclEditViewModel(permissionList.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.ProjectOrganizations) > -1;
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
                ], Dns.ViewModels.AclProjectOrganizationViewModel
                );

            this.OrganizationEvents = new Events.Acl.EventAclEditViewModel(eventList.filter((e) => {
                return e.Locations.indexOf(Dns.Enums.PermissionAclTypes.ProjectOrganizations) > -1;
            }), securityGroupTree, organizationEvents, [
                    {
                        Field: "OrganizationID",
                        Value: organizationDTO.OrganizationID
                    },
                    {
                        Field: "ProjectID",
                        Value: organizationDTO.ProjectID
                    }
                ], Dns.ViewModels.ProjectOrganizationEventViewModel);

        }

        public ToggleAcls() {
            this.ShowAcls(!this.ShowAcls());
        }
    }


    export class ProjectRequestTypeViewModel extends Dns.ViewModels.ProjectRequestTypeViewModel {
        public Selected: KnockoutObservable<boolean> = ko.observable(false);
        public Activities: KnockoutObservableArray<Dns.ViewModels.WorkflowActivityViewModel>;
        public onSelected: () => void;

        constructor(projectRequestType: Dns.Interfaces.IProjectRequestTypeDTO) {
            super(projectRequestType);
            this.Activities = ko.observableArray<Dns.ViewModels.WorkflowActivityViewModel>();
            var self = this;
            this.onSelected = () => {
                self.Selected(!self.Selected());
                if (self.Selected() && self.Activities().length == 0) {
                    Dns.WebApi.Workflow.GetWorkflowActivitiesByWorkflowID(self.WorkflowID(), "ID ne cc2e0001-9b99-4c67-8ded-a3b600e1c696", null, "Name").done((activities) => { //Don't show the terminate activity
                        self.Activities.push.apply(self.Activities, activities.map((item) => { return new WorkflowActivityViewModel(item, this); }));
                    });
                }
            };
        }
    }

    export class WorkflowActivityViewModel extends Dns.ViewModels.WorkflowActivityViewModel {
        public Selected: KnockoutObservable<boolean> = ko.observable(false);

        //Security computed here
        public Security: Security.Acl.AclEditViewModel<Dns.ViewModels.AclProjectRequestTypeWorkflowActivityViewModel>;

        constructor(workflowActivity: Dns.Interfaces.IWorkflowActivityDTO, requestType: ProjectRequestTypeViewModel) {
            super(workflowActivity);
            this.Security = new Security.Acl.AclEditViewModel(vm.PermissionList.filter((p) => {
                return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.ProjectRequestTypeWorkflowActivity) > -1;
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
                ], Dns.ViewModels.AclProjectRequestTypeWorkflowActivityViewModel);
        }

        public onSelected(data: WorkflowActivityViewModel) {
            data.Selected(!data.Selected());
        }
    }
} 