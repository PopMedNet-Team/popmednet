/// <reference path="../_rootlayout.ts" />
/// <reference path="../security/aclviewmodel.ts" />
/// <reference path="../events/editeventpermissions.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Projects;
(function (Projects) {
    var Details;
    (function (Details) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, project, projectRequestTypes, projectRequestTypeWorkflowActivityAcls, groups, dataMartList, organizationList, permissionList, requestTypes, projectDataMarts, projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEventPermissions, securityGroupTree, projectSecurityGroups, projectOrganizations, eventList, projectDataMartEventsList, groupid, requestTypeList, fieldOptions, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.AddDataMart = function (dm) {
                    //Get the request types for the datamart
                    //Add them to the list of request types
                    Dns.WebApi.DataMarts.GetRequestTypesByDataMarts([dm.ID]).done(function (results) {
                        _this.AddRequestTypes(results);
                        vm.ProjectDataMarts.push(new ProjectDataMartViewModel({
                            DataMartID: dm.ID,
                            DataMart: dm.Name,
                            Organization: dm.Organization,
                            ProjectID: _this.Project.ID(),
                            RequestTypes: results,
                            Project: _this.Project.Name(),
                            ProjectAcronym: _this.Project.Acronym()
                        }, vm.PermissionList, vm.EventList, vm.ProjectDataMartEventList, vm.SecurityGroupTree, vm.DataMartAcls, vm.DataMartRequestTypeAcls, vm.ProjectDataMartEvents));
                    });
                };
                _this.RemoveDataMart = function (pdm) {
                    Global.Helpers.ShowConfirm("Confirm DataMart Removal", "<p>Are you sure that you wish to remove " + pdm.DataMart() + " from the project?</p>").done(function () {
                        if (pdm.ProjectID())
                            Dns.WebApi.ProjectDataMarts.Remove([pdm.toData()]); //Can run async
                        var ndx = vm.ProjectDataMarts.indexOf(pdm);
                        // vm.ProjectDataMarts.splice(ndx, 1);
                        $('#acl' + pdm.DataMartID()).remove();
                        vm.ProjectDataMarts.remove(pdm); //Remove from the list.
                        _this.UpdateRequestTypes(); //Update the request types that are availble.
                    });
                };
                var self = _this;
                _this.CanManageSecurityTypes = ko.observable(_this.HasPermission(Permissions.Project.ManageRequestTypes));
                //Get lists
                _this.DataMartList = dataMartList;
                _this.OrganizationList = ko.observableArray(organizationList.map(function (item) {
                    return new Dns.ViewModels.OrganizationViewModel(item);
                }));
                _this.PermissionList = permissionList;
                _this.EventList = eventList;
                _this.ProjectDataMartEventList = projectDataMartEventsList;
                _this.SecurityGroupTree = securityGroupTree;
                _this.RequestTypes = ko.observableArray(requestTypes.map(function (item) {
                    return new Dns.ViewModels.RequestTypeViewModel(item);
                }));
                _this.RequestTypeList = requestTypeList;
                _this.Groups = groups;
                _this.Project = new Dns.ViewModels.ProjectViewModel(project);
                _this.Project.StartDate = ko.observable(project == null ? null : moment(project.StartDate).toDate());
                _this.Project.EndDate = ko.observable(project == null ? null : moment(project.EndDate).toDate());
                _this.SecurityGroups = ko.observableArray(projectSecurityGroups.map(function (sg) {
                    return new Dns.ViewModels.SecurityGroupViewModel(sg);
                }));
                _this.ProjectRequestTypes = ko.observableArray(projectRequestTypes.map(function (item) {
                    return new ProjectRequestTypeViewModel(item);
                }));
                //Acls
                _this.ProjectAcls = ko.observableArray(projectPermissions.map(function (item) {
                    return new Dns.ViewModels.AclProjectViewModel(item);
                }));
                _this.ProjectRequestTypeAcls = ko.observableArray(projectRequestTypePermissions.map(function (item) {
                    return new Dns.ViewModels.AclProjectRequestTypeViewModel(item);
                }));
                _this.ProjectRequestTypeWorkflowActivityAcls = ko.observableArray(projectRequestTypeWorkflowActivityAcls.map(function (item) {
                    return new Dns.ViewModels.AclProjectRequestTypeWorkflowActivityViewModel(item);
                }));
                _this.ProjectEvents = ko.observableArray(projectEventPermissions.map(function (item) {
                    return new Dns.ViewModels.ProjectEventViewModel(item);
                }));
                _this.ProjectDataMartEvents = ko.observableArray(projectDataMartEventPermissions.map(function (item) {
                    return new Dns.ViewModels.ProjectDataMartEventViewModel(item);
                }));
                _this.DataMartAcls = ko.observableArray(projectDataMartPermissions.map(function (item) {
                    return new Dns.ViewModels.AclProjectDataMartViewModel(item);
                }));
                _this.DataMartRequestTypeAcls = ko.observableArray(projectDataMartRequestTypePermissions.map(function (item) {
                    return new Dns.ViewModels.AclProjectDataMartRequestTypeViewModel(item);
                }));
                _this.OrganizationAcls = ko.observableArray(projectOrganizationPermissions.map(function (item) {
                    return new Dns.ViewModels.AclProjectOrganizationViewModel(item);
                }));
                _this.OrganizationEvents = ko.observableArray(projectOrganizationEventPermissions.map(function (item) {
                    return new Dns.ViewModels.ProjectOrganizationEventViewModel(item);
                }));
                _this.ProjectSecurity = new Security.Acl.AclEditViewModel(permissionList.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
                }), securityGroupTree, _this.ProjectAcls, [
                    {
                        Field: "ProjectID",
                        Value: _this.Project.ID()
                    }
                ], Dns.ViewModels.AclProjectViewModel);
                _this.ProjectRequestTypesSecurity = new Security.Acl.RequestTypes.AclRequestTypeEditViewModel(_this.RequestTypes, securityGroupTree, _this.ProjectRequestTypeAcls, [
                    {
                        Field: "ProjectID",
                        Value: _this.Project.ID()
                    }
                ], Dns.ViewModels.AclProjectRequestTypeViewModel);
                _this.ProjectEventSecurity = new Events.Acl.EventAclEditViewModel(eventList.filter(function (e) {
                    return e.Locations.indexOf(Dns.Enums.PermissionAclTypes.Projects) > -1;
                }), securityGroupTree, _this.ProjectEvents, [
                    {
                        Field: "ProjectID",
                        Value: _this.Project.ID()
                    }
                ], Dns.ViewModels.ProjectEventViewModel);
                _this.FieldOptionAcl = ko.observableArray(fieldOptions.map(function (e) {
                    if (e.ProjectID != self.Project.ID())
                        e.ProjectID = self.Project.ID();
                    return new Dns.ViewModels.AclProjectFieldOptionViewModel(e);
                }));
                _this.FieldOptions = new Security.Acl.FieldOption.AclFieldOptionEditViewModel(fieldOptions, securityGroupTree, _this.FieldOptionAcl, [
                    {
                        Field: "ProjectID",
                        Value: self.Project.ID()
                    }
                ], Dns.ViewModels.AclProjectFieldOptionViewModel);
                _this.ProjectDataMarts = ko.observableArray(projectDataMarts.map(function (item) { return new ProjectDataMartViewModel(item, _this.PermissionList, _this.EventList, _this.ProjectDataMartEventList, _this.SecurityGroupTree, _this.DataMartAcls, _this.DataMartRequestTypeAcls, _this.ProjectDataMartEvents); }));
                _this.Organizations = ko.observableArray(projectOrganizations.map(function (o) {
                    return new OrganizationViewModel(o, _this.PermissionList, _this.EventList, _this.SecurityGroupTree, _this.OrganizationAcls, _this.OrganizationEvents);
                }));
                //Set the Title. Every page should have this.
                _this.WatchTitle(_this.Project.Name, "Project: ");
                //List of datamarts that can be added to the project
                _this.AddableDataMartList = ko.computed(function () {
                    return self.DataMartList.filter(function (dm) {
                        var exists = false;
                        self.ProjectDataMarts().forEach(function (pdm) {
                            if (pdm.DataMartID() == dm.ID) {
                                exists = true;
                                return;
                            }
                        });
                        return !exists;
                    });
                });
                _this.AddableOrganizationList = ko.computed(function () {
                    var results = self.OrganizationList().filter(function (o) {
                        var exists = false;
                        self.Organizations().forEach(function (po) {
                            if (po.OrganizationID() == o.ID()) {
                                exists = true;
                                return;
                            }
                        });
                        return !exists;
                    });
                    return results;
                });
                _this.AddableRequestTypeList = ko.computed(function () {
                    var results = self.RequestTypeList.filter(function (rt) {
                        var exists = false;
                        self.ProjectRequestTypes().forEach(function (prt) {
                            if (prt.RequestTypeID() == rt.ID) {
                                exists = true;
                                return;
                            }
                        });
                        return !exists;
                    });
                    return results;
                });
                _this.Project.GroupID.subscribe(function (value) {
                    if (value == _this.Project.GroupID())
                        return;
                    (value == null ? null : Dns.WebApi.Organizations.ListByGroupMembership(value)).done(function (organizationList) {
                        _this.OrganizationList = ko.observableArray(organizationList.map(function (item) { return new Dns.ViewModels.OrganizationViewModel(item); }) || []);
                    });
                });
                if (groupid != null)
                    _this.Project.GroupID(groupid);
                self.Save = function (data, event, showPrompt) {
                    if (showPrompt === void 0) { showPrompt = true; }
                    var deferred = $.Deferred();
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
                    Dns.WebApi.Projects.InsertOrUpdate([self.Project.toData()]).done(function (project) {
                        //Update the values for the ID and timestamp as necessary.
                        self.Project.ID(project[0].ID);
                        self.Project.Timestamp(project[0].Timestamp);
                        window.history.replaceState(null, window.document.title, "/projects/details?ID=" + project[0].ID);
                        Layout.vmHeader.ReloadMenu();
                        //Save everything else
                        var dataMarts = self.ProjectDataMarts().map(function (dm) {
                            dm.ProjectID(self.Project.ID());
                            return dm.toData();
                        });
                        var organizations = self.Organizations().map(function (o) {
                            o.ProjectID(self.Project.ID());
                            return o.toData();
                        });
                        if (self.HasPermission(Permissions.Project.ManageSecurity)) {
                            var dataMartAcls = self.DataMartAcls().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var projectAcls = self.ProjectAcls().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var projectEventAcls = self.ProjectEvents().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var projectRequestTypeAcls = self.ProjectRequestTypeAcls().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var projectDataMartEventAcls = self.ProjectDataMartEvents().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var projectDataMartRequestTypeAcls = self.DataMartRequestTypeAcls().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var organizationAcls = self.OrganizationAcls().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var organizationEvents = self.OrganizationEvents().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var projectRequestTypeWorkFlowActivityAcls = self.ProjectRequestTypeWorkflowActivityAcls().map(function (a) {
                                a.ProjectID(self.Project.ID());
                                return a.toData();
                            });
                            var projectFieldOptionsAcls = self.FieldOptions.Acls().map(function (a) { return a.toData(); });
                        }
                        var projectRequestTypes = null;
                        if (self.HasPermission(Permissions.Project.ManageRequestTypes)) {
                            var projectRequestTypes = {
                                ProjectID: self.Project.ID(),
                                RequestTypes: self.ProjectRequestTypes().map(function (rt) {
                                    rt.ProjectID(self.Project.ID());
                                    return rt.toData();
                                })
                            };
                        }
                        var canManageSecurity = self.HasPermission(Permissions.Project.ManageSecurity);
                        var originalManageRequestTypes = self.HasPermission(Permissions.Project.ManageRequestTypes);
                        $.when(canManageSecurity ? Dns.WebApi.Security.UpdateProjectPermissions(projectAcls) : null, canManageSecurity ? Dns.WebApi.Events.UpdateProjectEventPermissions(projectEventAcls) : null, canManageSecurity ? Dns.WebApi.Events.UpdateProjectDataMartEventPermissions(projectDataMartEventAcls) : null, canManageSecurity ? Dns.WebApi.Events.UpdateProjectOrganizationEventPermissions(organizationEvents) : null, canManageSecurity ? Dns.WebApi.Security.UpdateProjectOrganizationPermissions(organizationAcls) : null, canManageSecurity ? Dns.WebApi.Security.UpdateProjectDataMartPermissions(dataMartAcls) : null, canManageSecurity && (projectFieldOptionsAcls != null && projectFieldOptionsAcls.length > 0) ? Dns.WebApi.Security.UpdateProjectFieldOptionPermissions(projectFieldOptionsAcls) : null).done(function () {
                            //update permissions
                            Dns.WebApi.Projects.GetPermissions([self.Project.ID()], [
                                Permissions.Project.Copy,
                                Permissions.Project.Delete,
                                Permissions.Project.Edit,
                                Permissions.Project.ManageSecurity,
                                Permissions.Project.ManageRequestTypes
                            ]).done(function (perms) {
                                //update the screen permissions with the newly set permissions
                                self.ScreenPermissions = perms.map(function (sp) {
                                    return sp.toLowerCase();
                                });
                                //now update the rest of the stuff using the new permissions
                                var canManageRequestTypes = self.HasPermission(Permissions.Project.ManageRequestTypes);
                                self.CanManageSecurityTypes(canManageRequestTypes);
                                $.when(canManageRequestTypes ? Dns.WebApi.Security.UpdateProjectRequestTypePermissions(projectRequestTypeAcls) : null, canManageRequestTypes ? Dns.WebApi.Security.UpdateProjectDataMartRequestTypePermissions(projectDataMartRequestTypeAcls) : null).done(function () {
                                    $.when(Dns.WebApi.ProjectDataMarts.InsertOrUpdate({ ProjectID: self.Project.ID(), DataMarts: dataMarts }), Dns.WebApi.ProjectOrganizations.InsertOrUpdate({ ProjectID: self.Project.ID(), Organizations: organizations }), (canManageRequestTypes && projectRequestTypes != null) ? Dns.WebApi.Projects.UpdateProjectRequestTypes(projectRequestTypes) : null, (canManageRequestTypes && projectRequestTypeWorkFlowActivityAcls != null) ? Dns.WebApi.Security.UpdateProjectRequestTypeWorkflowActivityPermissions(projectRequestTypeWorkFlowActivityAcls) : null).done(function () {
                                        if (showPrompt) {
                                            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>")
                                                .done(function () {
                                                if (canManageRequestTypes != originalManageRequestTypes && canManageRequestTypes) {
                                                    //if the permission to edit request types changes from denied to allowed, reload after save to make sure that all the requesttype collections are properly loaded back up.
                                                    window.location.reload();
                                                }
                                            });
                                        }
                                        ;
                                        deferred.resolve();
                                    }).fail(function (error) {
                                        deferred.reject();
                                    });
                                }).fail(function (error) {
                                    deferred.reject();
                                });
                            });
                        })
                            .fail(function (error) {
                            deferred.reject();
                        });
                    }).fail(function (error) {
                        //fail of initial save
                        deferred.reject();
                    });
                    return deferred;
                };
                return _this;
            }
            ViewModel.prototype.AddRequestTypes = function (requestTypes) {
                var _this = this;
                requestTypes.forEach(function (newrt) {
                    var exists = false;
                    _this.RequestTypes().forEach(function (oldrt) {
                        if (oldrt.ID() == newrt.ID) {
                            exists = true;
                            return;
                        }
                    });
                    if (!exists) {
                        _this.RequestTypes.push(new Dns.ViewModels.RequestTypeViewModel(newrt));
                    }
                });
            };
            ViewModel.prototype.AddProjectRequestType = function (requestType) {
                vm.ProjectRequestTypes.push(new ProjectRequestTypeViewModel({
                    ProjectID: vm.Project.ID,
                    RequestType: requestType.Name,
                    RequestTypeID: requestType.ID,
                    Template: requestType.Template,
                    Workflow: requestType.Workflow,
                    WorkflowID: requestType.WorkflowID
                }));
                vm.RequestTypes.push(new Dns.ViewModels.RequestTypeViewModel(requestType));
            };
            ViewModel.prototype.DeleteProjectRequestType = function (requestType) {
                vm.ProjectRequestTypes.remove(requestType);
                vm.RequestTypes.remove(function (item) {
                    return item.ID() == requestType.RequestTypeID();
                });
            };
            ViewModel.prototype.AddSecurityGroup = function () {
                vm.Save(null, null, false).done(function () {
                    window.location.href = '/securitygroups/details?OwnerID=' + vm.Project.ID();
                });
            };
            ViewModel.prototype.UpdateRequestTypes = function () {
                var _this = this;
                var deferred = $.Deferred();
                var dataMartIDs = this.ProjectDataMarts().map(function (dm) { return dm.DataMartID(); });
                Dns.WebApi.DataMarts.GetRequestTypesByDataMarts(dataMartIDs).done(function (results) {
                    //Remove ones that are there
                    for (var i = _this.RequestTypes().length - 1; i >= 0; i--) {
                        var oldrt = _this.RequestTypes()[i];
                        var exists = false;
                        results.forEach(function (newrt) {
                            if (oldrt.ID() == newrt) {
                                exists = true;
                                return;
                            }
                        });
                        if (exists)
                            continue;
                        _this.RequestTypes.remove(oldrt);
                    }
                    //Add ones that aren't there
                    results.forEach(function (newrt) {
                        var exists = false;
                        _this.RequestTypes().forEach(function (oldrt) {
                            if (oldrt.ID() == newrt.ID) {
                                exists = true;
                                return;
                            }
                        });
                        if (!exists)
                            _this.RequestTypes.push(new Dns.ViewModels.RequestTypeViewModel(newrt));
                    });
                    deferred.resolve();
                });
                return deferred;
            };
            ViewModel.prototype.AddOrganization = function (o) {
                vm.Organizations.push(new OrganizationViewModel({
                    Organization: o.Name(),
                    OrganizationID: o.ID(),
                    Project: vm.Project.Name(),
                    ProjectID: vm.Project.ID()
                }, vm.PermissionList, vm.EventList, vm.SecurityGroupTree, vm.OrganizationAcls, vm.OrganizationEvents));
            };
            ViewModel.prototype.RemoveOrganization = function (o) {
                Global.Helpers.ShowConfirm("Confirm Organization Removal", "<p>Are you sure that you wish to remove " + o.Organization() + " from the project?</p>").done(function () {
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
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this project?</p>").done(function () {
                    Dns.WebApi.Projects.Delete([vm.Project.ID()]).done(function () {
                        window.location.href = document.referrer;
                    });
                });
            };
            ViewModel.prototype.Copy = function () {
                Dns.WebApi.Projects.Copy(vm.Project.ID()).done(function (results) {
                    var newProjectID = results[0];
                    window.location.href = "/projects/details?ID=" + newProjectID;
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            //First we get the ID that was passed as a param to the page. (this will be null for a new project)
            var id = $.url().param("ID");
            //Get the GroupID that was passed as a param to the page, if we got here from the Group Detail screen.
            var groupid = $.url().param("GroupID");
            //Then we call all of the database calls that are necessary. By putting them in a $.when they all execute simultaniously and will complete at the length of the longest running request.
            $.when(id == null ? null : Dns.WebApi.Projects.GetPermissions([id], [
                Permissions.Project.Copy,
                Permissions.Project.Delete,
                Permissions.Project.Edit,
                Permissions.Project.ManageSecurity,
                Permissions.Project.ManageRequestTypes
            ]), id == null ? null : Dns.WebApi.Projects.Get(id), Dns.WebApi.Groups.List(null, "ID,Name"), Dns.WebApi.DataMarts.List(null, "ID,Name,Organization", "Name"), id == null ? null : Dns.WebApi.Security.GetProjectRequestTypeWorkflowActivityPermissions(id), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Projects,
                Dns.Enums.PermissionAclTypes.ProjectDataMarts,
                Dns.Enums.PermissionAclTypes.ProjectDataMartRequestTypes,
                Dns.Enums.PermissionAclTypes.ProjectOrganizations,
                Dns.Enums.PermissionAclTypes.ProjectRequestTypeWorkflowActivity
            ]), id == null ? null : Dns.WebApi.ProjectDataMarts.ListWithRequestTypes("ProjectID eq " + id), Dns.WebApi.Security.GetProjectDataMartPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Security.GetProjectPermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetProjectRequestTypePermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetProjectDataMartRequestTypePermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Events.GetProjectEventPermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Events.GetProjectDataMartEventPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Security.GetProjectOrganizationPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Events.GetProjectOrganizationEventPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), id == null ? null : Dns.WebApi.SecurityGroups.List("OwnerID eq " + id), id == null ? null : Dns.WebApi.ProjectOrganizations.List("ProjectID eq " + id), Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Projects]), Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.ProjectDataMarts]), Dns.WebApi.RequestTypes.ListAvailableRequestTypes(null, null, "Name"), id == null ? [] : Dns.WebApi.Security.GetProjectFieldOptionPermissions(id)).done(function (//This is the results for each of the calls. These are not typed in a when and have to be manually typed.
                screenPermissions, projects, groups, dataMartList, projectRequestTypeWorkflowActivityAcls, permissionList, projectDataMarts, projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEvents, securityGroupTree, projectSecurityGroups, projectOrganizations, eventList, projectDataMartsEventList, requestTypeList, fieldOptions) {
                var project = projects == null ? null : projects[0];
                //Now have our conditional queries that need to be executed. These should be items that depend on other items that you just got back to be queried.
                if (project != null && project.GroupID) {
                    $.when(screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetProjectRequestTypes(id) : null, screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetRequestTypes(id) : null, Dns.WebApi.Organizations.ListByGroupMembership(project.GroupID))
                        .done(function (projectRequestTypes, requestTypes, organizationList) {
                        $(function () {
                            //We get our binding control here because it's inside the document.ready. It cannot be assured anywhere else.
                            var bindingControl = $("#Content");
                            //Pass everything in to the view model here.
                            vm = new ViewModel(screenPermissions || [Permissions.Project.Edit, Permissions.Project.ManageSecurity], project, projectRequestTypes || [], projectRequestTypeWorkflowActivityAcls || [], groups, dataMartList || [], organizationList || [], permissionList, requestTypes || [], projectDataMarts || [], projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEvents, securityGroupTree, projectSecurityGroups || [], projectOrganizations || [], eventList || [], projectDataMartsEventList || [], groupid, requestTypeList, fieldOptions, bindingControl);
                            //Apply your bindings.
                            ko.applyBindings(vm, bindingControl[0]);
                        });
                    });
                }
                else {
                    $.when(id != null ? screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetProjectRequestTypes(id) : [] : [], id != null ? screenPermissions.indexOf(Permissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetRequestTypes(id) : [] : []).done(function (projectRequestTypes, requestTypes) {
                        $(function () {
                            var bindingControl = $("#Content");
                            vm = new ViewModel(screenPermissions || [Permissions.Project.Edit,
                                Permissions.Project.ManageSecurity,
                                Permissions.Project.ManageRequestTypes], project, projectRequestTypes, projectRequestTypeWorkflowActivityAcls || [], groups, dataMartList || [], [], permissionList, requestTypes, projectDataMarts || [], projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEvents, securityGroupTree, projectSecurityGroups || [], projectOrganizations || [], eventList || [], projectDataMartsEventList || [], groupid, requestTypeList, fieldOptions, bindingControl);
                            ko.applyBindings(vm, bindingControl[0]);
                        });
                    });
                }
            });
        }
        //This is called automatically because it's in the root. This means you don't have to setup a script tag in the page itself.
        init();
        //This is the datamart view model. It allows us to extend the default one and provide all of the options necessary for each data mart.
        var ProjectDataMartViewModel = (function (_super) {
            __extends(ProjectDataMartViewModel, _super);
            function ProjectDataMartViewModel(ProjectDataMartDTO, permissionList, eventList, projectDataMartsEventList, securityGroupTree, dataMartAcls, dataMartRequestTypeAcls, dataMartEvents) {
                var _this = _super.call(this, ProjectDataMartDTO) || this;
                _this.ShowAcls = ko.observable(false);
                //This is the definition for the view model that handles the Acls.
                //Note that the targets are the fields of the values that are for the specific acl (DataMartID and ProjectID) and it specifies what values should be added to any new acls that are created in the editor. The final param is the type it's going to create for new acls so that it knows what fields should be included ETC. If you run into errors saving, it's probably because you copied and pasted and did not change this type.
                _this.DataMartSecurity = new Security.Acl.AclEditViewModel(permissionList.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.ProjectDataMarts) > -1;
                }), securityGroupTree, dataMartAcls, [
                    {
                        Field: "DataMartID",
                        Value: ProjectDataMartDTO.DataMartID
                    },
                    {
                        Field: "ProjectID",
                        Value: ProjectDataMartDTO.ProjectID
                    }
                ], Dns.ViewModels.AclProjectDataMartViewModel);
                //This pulls the request types right from the ProjectDataMart as queried from the DB. This is intentional for this special case so that it only shows the supported request types for that data mart. The overall version shows all request types supported by the project and updates as needed.
                _this.RequestTypeList = ko.observableArray(ProjectDataMartDTO.RequestTypes.map(function (rt) {
                    return new Dns.ViewModels.RequestTypeViewModel(rt);
                }));
                _this.DataMartRequestTypeSecurity = new Security.Acl.RequestTypes.AclRequestTypeEditViewModel(_this.RequestTypeList, securityGroupTree, dataMartRequestTypeAcls, [
                    {
                        Field: "DataMartID",
                        Value: ProjectDataMartDTO.DataMartID
                    },
                    {
                        Field: "ProjectID",
                        Value: ProjectDataMartDTO.ProjectID
                    }
                ], Dns.ViewModels.AclProjectDataMartRequestTypeViewModel);
                _this.DataMartEvents = new Events.Acl.EventAclEditViewModel(projectDataMartsEventList, securityGroupTree, dataMartEvents, [
                    {
                        Field: "DataMartID",
                        Value: ProjectDataMartDTO.DataMartID
                    },
                    {
                        Field: "ProjectID",
                        Value: ProjectDataMartDTO.ProjectID
                    }
                ], Dns.ViewModels.ProjectDataMartEventViewModel);
                return _this;
            }
            ProjectDataMartViewModel.prototype.ToggleAcls = function () {
                this.ShowAcls(!this.ShowAcls());
            };
            return ProjectDataMartViewModel;
        }(Dns.ViewModels.ProjectDataMartWithRequestTypesViewModel));
        Details.ProjectDataMartViewModel = ProjectDataMartViewModel;
        //This is the Organization equivalent of the Data Mart View model. The same rules apply.
        var OrganizationViewModel = (function (_super) {
            __extends(OrganizationViewModel, _super);
            function OrganizationViewModel(organizationDTO, permissionList, eventList, securityGroupTree, organizationAcls, organizationEvents) {
                var _this = _super.call(this, organizationDTO) || this;
                _this.ShowAcls = ko.observable(false);
                _this.OrganizationSecurity = new Security.Acl.AclEditViewModel(permissionList.filter(function (p) {
                    return p.Locations.indexOf(Dns.Enums.PermissionAclTypes.ProjectOrganizations) > -1;
                }), securityGroupTree, organizationAcls, [
                    {
                        Field: "OrganizationID",
                        Value: organizationDTO.OrganizationID
                    },
                    {
                        Field: "ProjectID",
                        Value: organizationDTO.ProjectID
                    }
                ], Dns.ViewModels.AclProjectOrganizationViewModel);
                _this.OrganizationEvents = new Events.Acl.EventAclEditViewModel(eventList.filter(function (e) {
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
                return _this;
            }
            OrganizationViewModel.prototype.ToggleAcls = function () {
                this.ShowAcls(!this.ShowAcls());
            };
            return OrganizationViewModel;
        }(Dns.ViewModels.ProjectOrganizationViewModel));
        Details.OrganizationViewModel = OrganizationViewModel;
        var ProjectRequestTypeViewModel = (function (_super) {
            __extends(ProjectRequestTypeViewModel, _super);
            function ProjectRequestTypeViewModel(projectRequestType) {
                var _this = _super.call(this, projectRequestType) || this;
                _this.Selected = ko.observable(false);
                _this.Activities = ko.observableArray();
                var self = _this;
                _this.onSelected = function () {
                    self.Selected(!self.Selected());
                    if (self.Selected() && self.Activities().length == 0) {
                        Dns.WebApi.Workflow.GetWorkflowActivitiesByWorkflowID(self.WorkflowID(), "ID ne cc2e0001-9b99-4c67-8ded-a3b600e1c696", null, "Name").done(function (activities) {
                            self.Activities.push.apply(self.Activities, activities.map(function (item) { return new WorkflowActivityViewModel(item, _this); }));
                        });
                    }
                };
                return _this;
            }
            return ProjectRequestTypeViewModel;
        }(Dns.ViewModels.ProjectRequestTypeViewModel));
        Details.ProjectRequestTypeViewModel = ProjectRequestTypeViewModel;
        var WorkflowActivityViewModel = (function (_super) {
            __extends(WorkflowActivityViewModel, _super);
            function WorkflowActivityViewModel(workflowActivity, requestType) {
                var _this = _super.call(this, workflowActivity) || this;
                _this.Selected = ko.observable(false);
                _this.Security = new Security.Acl.AclEditViewModel(vm.PermissionList.filter(function (p) {
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
                        Value: _this.ID()
                    }
                ], Dns.ViewModels.AclProjectRequestTypeWorkflowActivityViewModel);
                return _this;
            }
            WorkflowActivityViewModel.prototype.onSelected = function (data) {
                data.Selected(!data.Selected());
            };
            return WorkflowActivityViewModel;
        }(Dns.ViewModels.WorkflowActivityViewModel));
        Details.WorkflowActivityViewModel = WorkflowActivityViewModel;
    })(Details = Projects.Details || (Projects.Details = {}));
})(Projects || (Projects = {}));
//# sourceMappingURL=Details.js.map