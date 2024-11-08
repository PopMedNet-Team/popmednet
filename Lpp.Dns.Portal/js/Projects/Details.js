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
    var Details;
    (function (Details) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, project, projectRequestTypes, projectRequestTypeWorkflowActivityAcls, groups, dataMartList, organizationList, permissionList, requestTypes, projectDataMarts, projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEventPermissions, securityGroupTree, projectSecurityGroups, projectOrganizations, eventList, projectDataMartEventsList, groupid, requestTypeList, fieldOptions, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions, $('#frmProjectDetails')) || this;
                _this.DeletedDataMarts = [];
                _this.DeletedOrganizations = [];
                _this.AddDataMart = function (dm) {
                    _this.DeletedDataMarts = ko.utils.arrayFilter(_this.DeletedDataMarts, function (item) { return item.DataMartID() !== dm.ID; });
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
                            _this.DeletedDataMarts.push(pdm);
                        pdm.DataMartSecurity.ClearAllGroups();
                        pdm.DataMartRequestTypeSecurity.ClearAllGroups();
                        pdm.DataMartEvents.ClearAllGroups();
                        $('#acl' + pdm.DataMartID()).remove();
                        vm.ProjectDataMarts.remove(pdm);
                        _this.UpdateRequestTypes();
                    });
                };
                var self = _this;
                _this.CanManageSecurityTypes = ko.observable(_this.HasPermission(PMNPermissions.Project.ManageRequestTypes));
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
                _this.WatchTitle(_this.Project.Name, "Project: ");
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
                    Global.Helpers.ShowExecuting();
                    var deferred = $.Deferred();
                    if (!self.Validate()) {
                        deferred.reject();
                        if (event != null)
                            event.preventDefault();
                        return;
                    }
                    Dns.WebApi.Projects.InsertOrUpdate([self.Project.toData()]).done(function (project) {
                        var projectID = project[0].ID;
                        self.Project.ID(projectID);
                        self.Project.Timestamp(project[0].Timestamp);
                        window.history.replaceState(null, window.document.title, "/projects/details?ID=" + projectID);
                        Layout.vmHeader.ReloadMenu();
                        var dataMarts = self.ProjectDataMarts().map(function (dm) {
                            dm.ProjectID(projectID);
                            return dm.toData();
                        });
                        var organizations = self.Organizations().map(function (o) {
                            o.ProjectID(projectID);
                            return o.toData();
                        });
                        var deletedDataMarts = self.DeletedDataMarts.map(function (dm) {
                            return dm.toData();
                        });
                        var deletedOrgs = self.DeletedOrganizations.map(function (o) {
                            return o.toData();
                        });
                        var dataMartAcls = null;
                        var projectAcls = null;
                        var projectEventAcls = null;
                        var projectRequestTypeAcls = null;
                        var projectDataMartEventAcls = null;
                        var projectDataMartRequestTypeAcls = null;
                        var organizationAcls = null;
                        var organizationEvents = null;
                        var projectRequestTypeWorkFlowActivityAcls = null;
                        var projectFieldOptionsAcls = null;
                        if (self.HasPermission(PMNPermissions.Project.ManageSecurity)) {
                            dataMartAcls = self.DataMartAcls().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            projectAcls = self.ProjectAcls().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            projectEventAcls = self.ProjectEvents().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            projectRequestTypeAcls = self.ProjectRequestTypeAcls().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            projectDataMartEventAcls = self.ProjectDataMartEvents().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            projectDataMartRequestTypeAcls = self.DataMartRequestTypeAcls().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            organizationAcls = self.OrganizationAcls().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            organizationEvents = self.OrganizationEvents().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            projectRequestTypeWorkFlowActivityAcls = self.ProjectRequestTypeWorkflowActivityAcls().map(function (a) {
                                a.ProjectID(projectID);
                                return a.toData();
                            });
                            projectFieldOptionsAcls = self.FieldOptions.Acls().map(function (a) { return a.toData(); });
                        }
                        var projectRequestTypes = null;
                        if (self.HasPermission(PMNPermissions.Project.ManageRequestTypes)) {
                            projectRequestTypes = {
                                ProjectID: projectID,
                                RequestTypes: self.ProjectRequestTypes().map(function (rt) {
                                    rt.ProjectID(projectID);
                                    return rt.toData();
                                })
                            };
                        }
                        var canManageSecurity = self.HasPermission(PMNPermissions.Project.ManageSecurity);
                        var originalManageRequestTypes = self.HasPermission(PMNPermissions.Project.ManageRequestTypes);
                        $.when(canManageSecurity ? Dns.WebApi.Security.UpdateProjectPermissions(projectAcls) : null, canManageSecurity ? Dns.WebApi.Events.UpdateProjectEventPermissions(projectEventAcls) : null, canManageSecurity ? Dns.WebApi.Events.UpdateProjectDataMartEventPermissions(projectDataMartEventAcls) : null, canManageSecurity ? Dns.WebApi.Events.UpdateProjectOrganizationEventPermissions(organizationEvents) : null, canManageSecurity ? Dns.WebApi.Security.UpdateProjectOrganizationPermissions(organizationAcls) : null, canManageSecurity ? Dns.WebApi.Security.UpdateProjectDataMartPermissions(dataMartAcls) : null, canManageSecurity && (projectFieldOptionsAcls != null && projectFieldOptionsAcls.length > 0) ? Dns.WebApi.Security.UpdateProjectFieldOptionPermissions(projectFieldOptionsAcls) : null).done(function () {
                            Dns.WebApi.Projects.GetPermissions([projectID], [
                                PMNPermissions.Project.Copy,
                                PMNPermissions.Project.Delete,
                                PMNPermissions.Project.Edit,
                                PMNPermissions.Project.ManageSecurity,
                                PMNPermissions.Project.ManageRequestTypes
                            ]).done(function (perms) {
                                self.ScreenPermissions = perms.map(function (sp) {
                                    return sp.toLowerCase();
                                });
                                var canManageRequestTypes = self.HasPermission(PMNPermissions.Project.ManageRequestTypes);
                                self.CanManageSecurityTypes(canManageRequestTypes);
                                $.when(canManageRequestTypes ? Dns.WebApi.Security.UpdateProjectRequestTypePermissions(projectRequestTypeAcls) : null, canManageRequestTypes ? Dns.WebApi.Security.UpdateProjectDataMartRequestTypePermissions(projectDataMartRequestTypeAcls) : null).done(function () {
                                    $.when(self.DeletedDataMarts.length > 0 ? Dns.WebApi.ProjectDataMarts.Remove(deletedDataMarts) : null, self.DeletedOrganizations.length > 0 ? Dns.WebApi.ProjectOrganizations.Remove(deletedOrgs) : null).then(function () {
                                        Dns.WebApi.ProjectDataMarts.InsertOrUpdate({ ProjectID: projectID, DataMarts: dataMarts }),
                                            Dns.WebApi.ProjectOrganizations.InsertOrUpdate({ ProjectID: projectID, Organizations: organizations }),
                                            (canManageRequestTypes && projectRequestTypes != null) ? Dns.WebApi.Projects.UpdateProjectRequestTypes(projectRequestTypes) : null,
                                            (canManageRequestTypes && projectRequestTypeWorkFlowActivityAcls != null) ? Dns.WebApi.Security.UpdateProjectRequestTypeWorkflowActivityPermissions(projectRequestTypeWorkFlowActivityAcls) : null;
                                    }).done(function () {
                                        if (showPrompt) {
                                            Global.Helpers.ShowAlert("Save", '<p style="text-align:center;">Save completed successfully!</p>')
                                                .done(function () {
                                                if (canManageRequestTypes != originalManageRequestTypes && canManageRequestTypes) {
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
                        deferred.reject();
                    }).always(function () {
                        Global.Helpers.HideExecuting();
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
                        vm.DeletedOrganizations.push(o);
                    o.OrganizationSecurity.ClearAllGroups();
                    o.OrganizationEvents.ClearAllGroups();
                    $('#acl' + o.OrganizationID()).remove();
                    vm.Organizations.remove(o);
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
            var id = $.url().param("ID");
            var groupid = $.url().param("GroupID");
            $.when(id == null ? null : Dns.WebApi.Projects.GetPermissions([id], [
                PMNPermissions.Project.Copy,
                PMNPermissions.Project.Delete,
                PMNPermissions.Project.Edit,
                PMNPermissions.Project.ManageSecurity,
                PMNPermissions.Project.ManageRequestTypes
            ]), id == null ? null : Dns.WebApi.Projects.Get(id), Dns.WebApi.Groups.List(null, "ID,Name"), Dns.WebApi.DataMarts.List(null, "ID,Name,Organization", "Name"), id == null ? null : Dns.WebApi.Security.GetProjectRequestTypeWorkflowActivityPermissions(id), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Projects,
                Dns.Enums.PermissionAclTypes.ProjectDataMarts,
                Dns.Enums.PermissionAclTypes.ProjectDataMartRequestTypes,
                Dns.Enums.PermissionAclTypes.ProjectOrganizations,
                Dns.Enums.PermissionAclTypes.ProjectRequestTypeWorkflowActivity
            ]), id == null ? null : Dns.WebApi.ProjectDataMarts.ListWithRequestTypes("ProjectID eq " + id), Dns.WebApi.Security.GetProjectDataMartPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Security.GetProjectPermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetProjectRequestTypePermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetProjectDataMartRequestTypePermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Events.GetProjectEventPermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Events.GetProjectDataMartEventPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Security.GetProjectOrganizationPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Events.GetProjectOrganizationEventPermissions(id ? id : Constants.GuidEmpty, null), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), id == null ? null : Dns.WebApi.SecurityGroups.List("OwnerID eq " + id), id == null ? null : Dns.WebApi.ProjectOrganizations.List("ProjectID eq " + id), Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Projects]), Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.ProjectDataMarts]), Dns.WebApi.RequestTypes.ListAvailableRequestTypes(null, null, "Name"), id == null ? [] : Dns.WebApi.Security.GetProjectFieldOptionPermissions(id)).done(function (screenPermissions, projects, groups, dataMartList, projectRequestTypeWorkflowActivityAcls, permissionList, projectDataMarts, projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEvents, securityGroupTree, projectSecurityGroups, projectOrganizations, eventList, projectDataMartsEventList, requestTypeList, fieldOptions) {
                var project = projects == null ? null : projects[0];
                if (project != null && project.GroupID) {
                    $.when(screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetProjectRequestTypes(id) : null, screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetRequestTypes(id) : null, Dns.WebApi.Organizations.ListByGroupMembership(project.GroupID))
                        .done(function (projectRequestTypes, requestTypes, organizationList) {
                        $(function () {
                            var bindingControl = $("#Content");
                            vm = new ViewModel(screenPermissions || [PMNPermissions.Project.Edit, PMNPermissions.Project.ManageSecurity], project, projectRequestTypes || [], projectRequestTypeWorkflowActivityAcls || [], groups, dataMartList || [], organizationList || [], permissionList, requestTypes || [], projectDataMarts || [], projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEvents, securityGroupTree, projectSecurityGroups || [], projectOrganizations || [], eventList || [], projectDataMartsEventList || [], groupid, requestTypeList, fieldOptions, bindingControl);
                            ko.applyBindings(vm, bindingControl[0]);
                            $('#PageLoadingMessage').remove();
                        });
                    });
                }
                else {
                    $.when(id != null ? screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetProjectRequestTypes(id) : [] : [], id != null ? screenPermissions.indexOf(PMNPermissions.Project.ManageRequestTypes.toLowerCase()) > -1 ? Dns.WebApi.Projects.GetRequestTypes(id) : [] : []).done(function (projectRequestTypes, requestTypes) {
                        $(function () {
                            var bindingControl = $("#Content");
                            vm = new ViewModel(screenPermissions || [PMNPermissions.Project.Edit,
                                PMNPermissions.Project.ManageSecurity,
                                PMNPermissions.Project.ManageRequestTypes], project, projectRequestTypes, projectRequestTypeWorkflowActivityAcls || [], groups, dataMartList || [], [], permissionList, requestTypes, projectDataMarts || [], projectDataMartPermissions, projectPermissions, projectRequestTypePermissions, projectDataMartRequestTypePermissions, projectEventPermissions, projectDataMartEventPermissions, projectOrganizationPermissions, projectOrganizationEvents, securityGroupTree, projectSecurityGroups || [], projectOrganizations || [], eventList || [], projectDataMartsEventList || [], groupid, requestTypeList, fieldOptions, bindingControl);
                            ko.applyBindings(vm, bindingControl[0]);
                            $('#PageLoadingMessage').remove();
                        });
                    });
                }
            });
        }
        init();
        var ProjectDataMartViewModel = (function (_super) {
            __extends(ProjectDataMartViewModel, _super);
            function ProjectDataMartViewModel(ProjectDataMartDTO, permissionList, eventList, projectDataMartsEventList, securityGroupTree, dataMartAcls, dataMartRequestTypeAcls, dataMartEvents) {
                var _this = _super.call(this, ProjectDataMartDTO) || this;
                _this.ShowAcls = ko.observable(false);
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
