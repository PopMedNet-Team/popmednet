var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../_rootlayout.ts" />
var Requests;
(function (Requests) {
    var Details;
    (function (Details) {
        var RequestOverviewViewModel = (function (_super) {
            __extends(RequestOverviewViewModel, _super);
            function RequestOverviewViewModel(request, parentRequest, requestDataMarts, requestType, workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, requestUsers, fieldOptions, bindingControl, screenPermissions, visualTerms, responseGroups, canViewResponses, currentTask) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
                _this.PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });
                _this.ValidationFunctions = [];
                _this.SaveFunctions = [];
                _this.SaveFormFunctions = [];
                _this.TaskDocumentsViewModel = null;
                _this.OverviewQCviewViewModel = null;
                _this.HasHistory = ko.observable(false);
                _this.responseIndex = 0;
                var self = _this;
                _this.RoutingsChanged = new ko.subscribable();
                _this.ReloadRoutingsRequired = new ko.subscribable();
                _this.Request = request;
                _this.ParentRequest = parentRequest;
                _this.CurrentTask = currentTask;
                _this.FieldOptions = fieldOptions;
                _this.ReadOnly = ko.observable(false);
                _this.VisualTerms = visualTerms || [];
                _this.SaveRequestID = ko.observable("");
                //Lists
                _this.RequestType = requestType;
                _this.RequesterCenterList = requesterCenterList;
                _this.WorkPlanTypeList = workPlanTypeList;
                //remove RALs that have been deleted
                _this.ReportAggregationLevelList = reportAggregationLevelList.filter(function (ral) { return ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)); });
                _this.Priority_Display = ko.computed(function () {
                    var pair = ko.utils.arrayFirst(Dns.Enums.PrioritiesTranslation, function (i) { return i.value == self.Request.Priority(); });
                    if (pair)
                        return pair.text;
                    return '';
                });
                _this.RequesterCenter_Display = ko.computed(function () {
                    if (self.Request.RequesterCenterID) {
                        var requestCenter = ko.utils.arrayFirst(self.RequesterCenterList, function (rc) { return rc.ID == self.Request.RequesterCenterID(); });
                        if (requestCenter)
                            return requestCenter.Name;
                    }
                    return '';
                });
                _this.ReportAggregationLevel_Display = ko.computed(function () {
                    if (self.Request.ReportAggregationLevelID) {
                        var reportAggregationLevel = ko.utils.arrayFirst(self.ReportAggregationLevelList, function (ral) { return ral.ID == self.Request.ReportAggregationLevelID(); });
                        if (reportAggregationLevel != null)
                            return reportAggregationLevel.Name;
                    }
                    return '';
                });
                _this.ProjectActivityTree = activityTree;
                _this.RefreshActivitiesDataSources();
                _this.Description_Display = ko.computed(function () {
                    return _this.Request.Description().split("<p></p>").join("<p>&nbsp;</p>");
                }, _this, { pure: true });
                _this.SourceTaskOrder_Display = ko.computed(function () {
                    if (self.Request.SourceTaskOrderID() == null)
                        return '';
                    var sa = ko.utils.arrayFirst(self.ProjectActivityTree, function (a) { return a.ID == self.Request.SourceTaskOrderID(); });
                    if (sa) {
                        return sa.Name;
                    }
                });
                _this.SourceActivity_Display = ko.computed(function () {
                    if (self.Request.SourceActivityID() == null)
                        return '';
                    var sa = ko.utils.arrayFirst(self.AllActivities, function (a) { return a.ID == self.Request.SourceActivityID(); });
                    if (sa) {
                        return sa.Name;
                    }
                });
                _this.SourceActivityProject_Display = ko.computed(function () {
                    if (self.Request.SourceActivityProjectID() == null)
                        return '';
                    var sa = ko.utils.arrayFirst(self.AllActivityProjects, function (a) { return a.ID == self.Request.SourceActivityProjectID(); });
                    if (sa) {
                        return sa.Name;
                    }
                });
                _this.PurposeOfUse_Display = ko.computed(function () {
                    if (self.Request.PurposeOfUse() == null)
                        return '';
                    var pou = ko.utils.arrayFirst(self.PurposeOfUseOptions, function (a) { return a.Value == self.Request.PurposeOfUse(); });
                    if (pou) {
                        return pou.Name;
                    }
                    return '';
                });
                _this.TaskOrderName = ko.observable();
                _this.ActivityName = ko.observable();
                _this.ActivityProjectName = ko.observable();
                _this.RefreshBudgetActivities = function () {
                    if (self.Request.ActivityID()) {
                        //determine what the current budget activity is
                        var findActivity = function (id) {
                            if (id == null)
                                return null;
                            for (var i = 0; i < self.ProjectActivityTree.length; i++) {
                                var act = self.ProjectActivityTree[i];
                                if (act.ID == id) {
                                    return act;
                                }
                                for (var j = 0; j < act.Activities.length; j++) {
                                    var act2 = act.Activities[j];
                                    if (act2.ID == id) {
                                        return act2;
                                    }
                                    for (var k = 0; k < act2.Activities.length; k++) {
                                        var act3 = act2.Activities[k];
                                        if (act3.ID == id) {
                                            return act3;
                                        }
                                    }
                                }
                            }
                            return null;
                        };
                        var currentBudgetActivity = findActivity(self.Request.ActivityID());
                        if (currentBudgetActivity != null) {
                            if (currentBudgetActivity.TaskLevel == 1) {
                                //task order
                                self.TaskOrderName(currentBudgetActivity.Name);
                                self.ActivityName('');
                                self.ActivityProjectName('');
                            }
                            else if (currentBudgetActivity.TaskLevel == 2) {
                                //activity
                                var taskOrder = findActivity(currentBudgetActivity.ParentActivityID);
                                self.TaskOrderName(taskOrder.Name);
                                self.ActivityName(currentBudgetActivity.Name);
                                self.ActivityProjectName('');
                            }
                            else {
                                //activity project
                                self.ActivityProjectName(currentBudgetActivity.Name);
                                currentBudgetActivity = findActivity(currentBudgetActivity.ParentActivityID);
                                self.ActivityName(currentBudgetActivity.Name);
                                currentBudgetActivity = findActivity(currentBudgetActivity.ParentActivityID);
                                self.TaskOrderName(currentBudgetActivity.Name);
                            }
                        }
                    }
                };
                self.RefreshBudgetActivities();
                _this.Identifier = ko.computed({
                    read: function () {
                        return self.Request.UserIdentifier() != null ? self.Request.UserIdentifier() : self.Request.Identifier() ? self.Request.Identifier().toString() : null;
                    },
                    write: function (value) {
                        self.Request.UserIdentifier(value);
                    }
                });
                _this.RequestDataMarts = ko.observableArray(requestDataMarts.map(function (rdm) {
                    return new RequestDataMartViewModel(rdm);
                }));
                self.CanViewResponses = canViewResponses;
                self.SelectedCompleteResponses = ko.observableArray([]);
                self.HasSelectedCompleteRoutings = ko.pureComputed(function () { return self.SelectedCompleteResponses().length > 0; });
                var virtualRoutes = [];
                if (responseGroups != null && responseGroups.length > 0) {
                    ko.utils.arrayForEach(responseGroups, function (group) {
                        var routing = ko.utils.arrayFirst(requestDataMarts, function (r) { return r.ResponseGroupID == group.ID; });
                        var vr = new VirtualRoutingViewModel(routing, group);
                        vr.addRoutings(ko.utils.arrayFilter(requestDataMarts, function (r) { return r.ResponseGroupID == group.ID && r.ID != routing.ID; }));
                        virtualRoutes.push(vr);
                    });
                }
                ko.utils.arrayForEach(requestDataMarts, function (routing) {
                    if (routing.ResponseGroupID == null) {
                        virtualRoutes.push(new VirtualRoutingViewModel(routing, null));
                    }
                });
                self.VirtualRoutings = ko.observableArray(virtualRoutes);
                self.CompletedRoutings = ko.computed(function () {
                    return ko.utils.arrayFilter(self.VirtualRoutings(), function (routing) {
                        return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                            routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                            routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                            routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                            routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                            routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                    }).sort(function (a, b) {
                        if (a.Name > b.Name) {
                            return 1;
                        }
                        if (a.Name < b.Name) {
                            return -1;
                        }
                        else {
                            return 0;
                        }
                    });
                });
                ////may need edits to not hide rejected?
                self.IncompleteRoutings = ko.computed(function () {
                    return ko.utils.arrayFilter(self.VirtualRoutings(), function (routing) {
                        return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                            routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                            routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                            routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                    }).sort(function (a, b) {
                        if (a.Name > b.Name) {
                            return 1;
                        }
                        if (a.Name < b.Name) {
                            return -1;
                        }
                        else {
                            return 0;
                        }
                    });
                });
                self.AllRoutings = ko.pureComputed(function () {
                    return $.Enumerable.From(self.VirtualRoutings()).OrderBy(function (x) { return x.Status; }).ThenBy(function (x) { return x.Name; }).ToArray();
                });
                //Workflow
                _this.WorkflowActivity = new Dns.ViewModels.WorkflowActivityViewModel(workFlowActivity);
                //Boolean to hide Edit Metadata button if activity is "Terminate Request"
                _this.IsNotTerminatedRequest = ko.computed(function () {
                    if (self.WorkflowActivity.ID() == "cc2e0001-9b99-4c67-8ded-a3b600e1c696")
                        return false;
                    if (self.WorkflowActivity.ID() != "cc2e0001-9b99-4c67-8ded-a3b600e1c696")
                        return true;
                });
                _this.RequestIsComplete = ko.computed(function () {
                    return self.Request.Status() == Dns.Enums.RequestStatuses.Complete ||
                        self.Request.Status() == Dns.Enums.RequestStatuses.CompleteWithReport ||
                        self.Request.Status() == Dns.Enums.RequestStatuses.Cancelled ||
                        self.Request.Status() == Dns.Enums.RequestStatuses.Failed ||
                        self.Request.Status() == Dns.Enums.RequestStatuses.RequestRejected ||
                        self.Request.Status() == Dns.Enums.RequestStatuses.TerminatedPriorToDistribution;
                });
                _this.WatchTitle(_this.Request.Name, "Request: ");
                _this.SetTaskDocumentsViewModel = function (vm) {
                    self.TaskDocumentsViewModel = vm;
                };
                _this.RefreshTaskDocuments = function () {
                    if (self.TaskDocumentsViewModel)
                        self.TaskDocumentsViewModel.onRefreshDocuments();
                };
                _this.AssignedWorkflowRequestUsers = new kendo.data.DataSource({
                    data: requestUsers
                });
                _this.SelectedRequestUser = ko.observable(null);
                _this.onRemoveRequestUser = function (e) {
                    if (self.SelectedRequestUser() == null)
                        return;
                    var message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> this workflow role user?</p>';
                    message += '<p><strong>' + self.SelectedRequestUser().FullName + ' - ' + self.SelectedRequestUser().WorkflowRole + '</strong></p></div>';
                    Global.Helpers.ShowConfirm("Delete Workflow Role User?", message).done(function () {
                        var url = '/RequestUsers/Delete';
                        $.ajax({
                            type: "DELETE",
                            url: !jQuery.support.cors ? '/api/post?Url=' + encodeURIComponent(url) : ServiceUrl + url,
                            dataType: 'json',
                            data: JSON.stringify([self.SelectedRequestUser()]),
                            contentType: 'application/json; charset=utf-8'
                        }).done(function () {
                            var selectedRequestUser = self.SelectedRequestUser();
                            var raw = self.AssignedWorkflowRequestUsers.data();
                            for (var i = raw.length - 1; i >= 0; i--) {
                                var item = raw[i];
                                if (selectedRequestUser.UserID == item.UserID && selectedRequestUser.WorkflowRoleID == item.WorkflowRoleID) {
                                    self.AssignedWorkflowRequestUsers.remove(item);
                                }
                            }
                            self.SelectedRequestUser.valueWillMutate();
                            self.SelectedRequestUser(null);
                            self.SelectedRequestUser.valueHasMutated();
                        });
                    });
                };
                _this.onAddRequestUser = function () {
                    var action = null;
                    if (!self.Request.ID()) {
                        //save the request in current state   
                        if (!_this.Validate())
                            return;
                        if (!_this.ValidateRequest())
                            return;
                        if (!_this.SaveRequest(false))
                            return;
                        action = self.DefaultSave(false);
                    }
                    else {
                        action = $.Deferred().resolve();
                    }
                    action.done(function () {
                        Global.Helpers.ShowDialog("Add Workflow User", "/workflow/workflowrequests/addrequestuserdialog", ['close'], 714, 550, { Request: self.Request })
                            .done(function (results) {
                            if (!results)
                                return;
                            results.forEach(function (u) {
                                self.AssignedWorkflowRequestUsers.add(u);
                            });
                        });
                    });
                };
                _this.onRefreshRequestUsers = function () {
                    Dns.WebApi.RequestUsers.List('RequestID eq ' + self.Request.ID()).done(function (requestUsers) {
                        self.AssignedWorkflowRequestUsers.data(requestUsers);
                    });
                };
                var editidpermission = _this.HasPermission(Permissions.Project.EditRequestID);
                _this.onEditWFRequestMetadata = function () {
                    if (!self.Validate()) {
                        //trigger validation on the form before allowing edit of metadata
                        Global.Helpers.ShowErrorAlert('Validation Error', '<p class="alert alert-warning" role="alert">One or more validation errors were found in the current task editor, and need to be addressed before continuing Metadata edit.</p>');
                        return;
                    }
                    //save current Priority and Due Date settings in order to monitor changes after metadata has been edited
                    var oldRequestPriority = self.Request.Priority();
                    var oldRequestDueDate = self.Request.DueDate();
                    Global.Helpers.ShowDialog("Edit Request Metadata", "/workflow/workflowrequests/editwfrequestmetadatadialog", [], 700, 700, { DetailsViewModel: self, AllowEditRequestID: editidpermission, NewRequest: false, OldRequestPriority: oldRequestPriority, OldRequestDueDate: oldRequestDueDate })
                        .done(function (result) {
                    });
                };
                _this.AllowCopy = ko.observable(false);
                if (self.Request.ID() != null) {
                    Dns.WebApi.Requests.AllowCopyRequest(self.Request.ID()).done(function (allow) {
                        self.AllowCopy(allow[0]);
                    });
                }
                _this.onCopy = function () {
                    Dns.WebApi.Requests.CopyRequest(self.Request.ID()).done(function (reqID) {
                        //load new request page using the new request ID
                        var q = '//' + window.location.host + '/requests/details?ID=' + reqID[0];
                        window.location.assign(q);
                    }).fail(function (ex) {
                    });
                };
                _this.NotifyMetadataChanged = function (item) {
                    self.RoutingsChanged.notifySubscribers(item);
                };
                _this.NotifyReloadRoutes = function () {
                    //notify subscribers that the routes have changed and should be reloaded
                    self.ReloadRoutingsRequired.notifySubscribers();
                };
                _this.onRequestUserRowSelected = function (e) {
                    var grid = $(e.sender.wrapper).data('kendoGrid');
                    var rows = grid.select();
                    if (rows.length == 0) {
                        self.SelectedRequestUser(null);
                        return;
                    }
                    var selectedRequestUser = grid.dataItem(rows[0]);
                    self.SelectedRequestUser(selectedRequestUser);
                };
                _this.EnableRemoveRequestUser = ko.computed(function () {
                    return self.SelectedRequestUser() != null && !self.SelectedRequestUser().IsRequestCreatorRole;
                });
                _this.DefaultSaveTaskDocument = function () {
                    self.DefaultResultSave('<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The request needs to be saved before being able to upload a document.</p> <p style="font-size:larger;">Would you like to save the Request now?</p><p><small>(This will cause the page to be reloaded, and you will need to initiate the upload again.)</small></p></div>');
                };
                _this.DefaultResultSave = function (warningMessage) {
                    Global.Helpers.ShowConfirm('Save Required Before Continuing', warningMessage).done(function () {
                        _this.DefaultSave(true);
                    });
                };
                self.IsFieldRequired = function (id) {
                    var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                    return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
                };
                self.IsFieldVisible = function (id) {
                    var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                    return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
                };
                self.onShowRoutingHistory = function (routing) {
                    Dns.WebApi.Requests.GetResponseHistory(routing.RequestDataMartID, routing.RequestID).done(function (responseHistory) {
                        Global.Helpers.ShowDialog("History", "/dialogs/routinghistory", ['close'], 500, 300, { responseHistory: responseHistory[0] })
                            .done(function () {
                        });
                    });
                };
                var setupResponseTabView = function (responseView) {
                    var tabID = 'responsedetail_' + self.responseIndex;
                    self.responseIndex++;
                    var q = '//' + window.location.host + '/workflowrequests/responsedetail';
                    q += '?id=' + self.SelectedCompleteResponses();
                    q += '&view=' + responseView;
                    q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();
                    var contentFrame = document.createElement('iframe');
                    contentFrame.src = q;
                    contentFrame.setAttribute('style', 'margin:0px;padding:0px;border:none;width:100%;height:940px;min-height:940px;');
                    contentFrame.setAttribute('scrolling', 'no');
                    var contentContainer = $('<div class="tab-pane fade" id="' + tabID + '"></div>');
                    contentContainer.append(contentFrame);
                    $('#root-tab-content').append(contentContainer);
                    var tl = $('<li></li>');
                    var ta = $('<a href="#' + tabID + '" role="tab" data-toggle="tab" style="display:inline-block">Response Detail <i class="glyphicon glyphicon-remove-circle"></i></a>');
                    var tac = ta.find('i');
                    tac.click(function (evt) {
                        evt.stopPropagation();
                        evt.preventDefault();
                        tl.remove();
                        $('#' + tabID).remove();
                        if ($(tl).hasClass('active')) {
                            tl.removeClass('active');
                            $('#tabs a:last').tab('show');
                        }
                    });
                    tl.append(ta);
                    $('#tabs').append(tl);
                };
                self.onToggleCompleteRoutes = ko.pureComputed({
                    read: function () {
                        return self.CompletedRoutings().length > 0 && self.SelectedCompleteResponses().length === self.CompletedRoutings().length;
                    },
                    write: function (value) {
                        if (value) {
                            var allID = ko.utils.arrayMap(self.CompletedRoutings(), function (i) { return i.ID; });
                            self.SelectedCompleteResponses(allID);
                        }
                        else {
                            self.SelectedCompleteResponses([]);
                        }
                    }
                });
                self.onViewAggregatedResults = function () {
                    setupResponseTabView(Dns.Enums.TaskItemTypes.AggregateResponse);
                };
                self.onViewIndividualResults = function () {
                    setupResponseTabView(Dns.Enums.TaskItemTypes.Response);
                };
                return _this;
            }
            RequestOverviewViewModel.prototype.DefaultSave = function (reload, isNewRequest, errorHandler) {
                if (isNewRequest === void 0) { isNewRequest = null; }
                if (errorHandler === void 0) { errorHandler = null; }
                var self = this;
                var deferred = $.Deferred();
                if (isNewRequest == null) {
                    isNewRequest = false;
                }
                if (!isNewRequest) {
                    if (!this.Validate()) {
                        Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>There was a Vaildation Error on the Task Tab</p></div>');
                        return deferred.reject();
                    }
                    if (!this.ValidateRequest()) {
                        Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>There was a Vaildation Error on the Task Tab</p></div>');
                        return deferred.reject();
                    }
                }
                if (!this.SaveRequest(false))
                    return deferred.reject();
                var dto = self.Request.toData();
                Dns.WebApi.Requests.CompleteActivity({
                    DemandActivityResultID: this.SaveRequestID(),
                    Dto: dto,
                    DataMarts: Requests.Details.rovm.RequestDataMarts().map(function (item) {
                        return item.toData();
                    }),
                    Data: JSON.stringify(self.SaveFormDTO == null ? null : self.SaveFormDTO.toData()),
                    Comment: null
                }).done(function (results) {
                    var result = results[0];
                    if (result.Uri) {
                        //// Need to Go back to API cause results dont return back FULL DTO info
                        $.when(Dns.WebApi.Requests.Get(result.Entity.ID), Dns.WebApi.Tasks.ByRequestID(result.Entity.ID))
                            .done(function (res, tasks) {
                            self.Request.update(res[0]);
                            self.CurrentTask = ko.utils.arrayFirst(tasks, function (t) { return t.WorkflowActivityID == self.Request.CurrentWorkFlowActivityID() && t.EndOn == null; });
                            if (reload) {
                                Global.Helpers.RedirectTo(result.Uri);
                            }
                            else {
                                Global.Helpers.HistoryReplaceState(self.Request.Name(), result.Uri);
                            }
                            deferred.resolve(true);
                        });
                    }
                    else {
                        //Update the request etc. here 
                        self.Request.ID(result.Entity.ID);
                        self.Request.Timestamp(result.Entity.Timestamp);
                        Global.Helpers.HistoryReplaceState(self.Request.Name(), '/requests/details?ID=' + self.Request.ID());
                        deferred.resolve(true);
                    }
                }).fail(function (er) {
                    if (errorHandler != null)
                        errorHandler(er);
                });
                return deferred;
            };
            RequestOverviewViewModel.prototype.DataMartSelectAll = function () {
                Details.rovm.RequestDataMarts().forEach(function (dm) {
                    dm.Selected(true);
                });
            };
            RequestOverviewViewModel.prototype.RefreshActivitiesDataSources = function () {
                var activities = [];
                var activityProjects = [];
                this.ProjectActivityTree.forEach(function (to) {
                    activities = activities.concat(to.Activities);
                    to.Activities.forEach(function (a) {
                        activityProjects = activityProjects.concat(a.Activities);
                    });
                });
                this.AllActivities = activities;
                this.AllActivityProjects = activityProjects;
            };
            RequestOverviewViewModel.prototype.RegisterValidationFunction = function (fnc) {
                this.ValidationFunctions.push(fnc);
            };
            RequestOverviewViewModel.prototype.RegisterRequestSaveFunction = function (fnc) {
                this.SaveFunctions.push(fnc);
            };
            RequestOverviewViewModel.prototype.RegisterRequestFormSaveFunction = function (fnc) {
                this.SaveFormFunctions.push(fnc);
            };
            RequestOverviewViewModel.prototype.Save = function (showMessage) {
                if (showMessage === void 0) { showMessage = true; }
                var deferred = $.Deferred();
                if (!this.Validate())
                    return deferred.reject();
                if (!this.ValidateRequest())
                    return deferred.reject();
                if (!this.SaveRequest(false))
                    return deferred.reject();
                //Post the request as is to the server
                return this.PostSave(showMessage);
            };
            RequestOverviewViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            RequestOverviewViewModel.prototype.Terminate = function () {
                Global.Helpers.ShowConfirm("Terminate Confirmation", "<p>Are you sure that you wish to terminate this Request?</p>").done(function () {
                    Dns.WebApi.Requests.TerminateRequest(Details.rovm.Request.ID()).done(function () {
                        window.history.back();
                    });
                });
            };
            RequestOverviewViewModel.prototype.ValidateRequest = function () {
                for (var i = 0; i < this.ValidationFunctions.length; i++) {
                    if (!this.ValidationFunctions[i](this.Request))
                        return false;
                }
                return true;
            };
            RequestOverviewViewModel.prototype.SaveRequest = function (submit) {
                for (var i = 0; i < this.SaveFunctions.length; i++) {
                    if (!this.SaveFunctions[i](this.Request))
                        return false;
                }
                //Do whatever other common save stuff happens here.
                return true;
            };
            RequestOverviewViewModel.prototype.SaveFormRequest = function (submit) {
                for (var i = 0; i < this.SaveFormFunctions.length; i++) {
                    if (!this.SaveFormFunctions[i](this.SaveFormDTO))
                        return false;
                }
                //Do whatever other common save stuff happens here.
                return true;
            };
            RequestOverviewViewModel.prototype.PostSave = function (showMessage) {
                var deferred = $.Deferred();
                var request = this.Request.toData();
                //Post it as a save
                Dns.WebApi.Requests.InsertOrUpdate([request]).done(function (results) {
                    Details.rovm.Request.ID(results[0].ID);
                    Details.rovm.Request.Timestamp(results[0].Timestamp);
                    //Update the history
                    window.history.replaceState(null, window.document.title, "/requests/details?ID=" + results[0].ID);
                    //Save the datamarts here
                    //Save anything else here if you want.
                    if (showMessage)
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(function () {
                            deferred.resolve(true);
                            return;
                        });
                    deferred.resolve(true);
                }).fail(function () {
                    deferred.fail();
                });
                return deferred;
            };
            RequestOverviewViewModel.prototype.UpdateUrl = function () {
                if (this.Request.ID()) {
                    window.history.replaceState(null, this.Request.Name(), "/requests/details?ID=" + this.Request.ID());
                }
                else {
                    window.history.replaceState(null, this.Request.Name(), window.location.href);
                }
            };
            return RequestOverviewViewModel;
        }(Global.PageViewModel));
        Details.RequestOverviewViewModel = RequestOverviewViewModel;
        function GetVisualTerms() {
            var d = $.Deferred();
            $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
                .done(function (result) {
                d.resolve(result);
            }).fail(function (e, description, error) {
                d.reject(e);
            });
            return d;
        }
        function init() {
            var id = Global.GetQueryParam("ID");
            var requestTypeID;
            var request;
            var parentRequest;
            var workFlowActivity;
            var requestTypeWorkflowActivityPermissions = [
                Permissions.ProjectRequestTypeWorkflowActivities.ViewRequestOverview,
                Permissions.ProjectRequestTypeWorkflowActivities.ViewTask,
                Permissions.ProjectRequestTypeWorkflowActivities.EditTask,
                Permissions.ProjectRequestTypeWorkflowActivities.ViewComments,
                Permissions.ProjectRequestTypeWorkflowActivities.AddComments,
                Permissions.ProjectRequestTypeWorkflowActivities.ViewDocuments,
                Permissions.ProjectRequestTypeWorkflowActivities.AddDocuments,
                Permissions.ProjectRequestTypeWorkflowActivities.ReviseDocuments,
                Permissions.ProjectRequestTypeWorkflowActivities.CloseTask,
                Permissions.ProjectRequestTypeWorkflowActivities.ViewRequestOverview,
                Permissions.ProjectRequestTypeWorkflowActivities.TerminateWorkflow,
                Permissions.ProjectRequestTypeWorkflowActivities.EditRequestMetadata,
                Permissions.ProjectRequestTypeWorkflowActivities.ViewTrackingTable
            ];
            $.when(Dns.WebApi.Requests.ListRequesterCenters(null, "ID,Name", "Name"), Dns.WebApi.Requests.ListWorkPlanTypes(null, "ID,Name", "Name"), Dns.WebApi.Requests.ListReportAggregationLevels(null, "ID,Name,DeletedOn", "Name"), 
            //Dns.WebApi.Projects.GetFieldOptions(pid),
            GetVisualTerms()).done(function (requesterCenterList, workPlanTypeList, reportAggregationLevelList, visualTerms) {
                if (!id) {
                    //Get the starting workflow activity
                    requestTypeID = Global.GetQueryParam("requestTypeID");
                    var projectID = Global.GetQueryParam("ProjectID");
                    var templateID = Global.GetQueryParam("templateID");
                    var parentRequestID = Global.GetQueryParam("ParentRequestID");
                    var userID = Global.GetQueryParam("UserID");
                    //This is new, we need to get extensive information about the workflow, request type, etc.
                    $.when(parentRequestID == null ? [] : Dns.WebApi.Requests.Get(parentRequestID), Dns.WebApi.RequestTypes.Get(requestTypeID), Dns.WebApi.Workflow.GetWorkflowEntryPointByRequestTypeID(requestTypeID), templateID == null ? null : Dns.WebApi.Templates.Get(templateID), Dns.WebApi.Projects.GetFieldOptions(projectID, User.ID), Dns.WebApi.Projects.GetPermissions([Global.GetQueryParam("ProjectID")], [Permissions.Request.AssignRequestLevelNotifications, Permissions.Project.EditRequestID, Permissions.Request.OverrideDataMartRoutingStatus, Permissions.Request.ApproveRejectResponse]), Dns.WebApi.Projects.GetActivityTreeByProjectID(projectID)).done(function (parentRequests, requestTypes, workflowActivities, templates, fieldOptions, projPermissions, activityTree) {
                        if (parentRequests.length != 0) {
                            parentRequest = new Dns.ViewModels.RequestViewModel(parentRequests[0]);
                        }
                        workFlowActivity = workflowActivities[0];
                        Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Global.GetQueryParam("ProjectID"), workFlowActivity.ID, requestTypeID, requestTypeWorkflowActivityPermissions)
                            .done(function (permissions) {
                            request = new Dns.ViewModels.RequestViewModel();
                            request.Name(requestTypes[0].Name);
                            request.CreatedByID(User.ID);
                            request.CreatedOn(new Date());
                            request.UpdatedByID(User.ID);
                            request.UpdatedOn(new Date());
                            request.CompletedOn(null);
                            request.Description("");
                            request.RequestTypeID(requestTypeID);
                            request.Priority(Dns.Enums.Priorities.Medium);
                            request.CurrentWorkFlowActivityID(workFlowActivity.ID);
                            request.ProjectID(Global.GetQueryParam("ProjectID"));
                            request.OrganizationID(User.EmployerID);
                            request.WorkflowID(requestTypes[0].WorkflowID);
                            request.ParentRequestID(parentRequestID);
                            if (templates != null && templates.length > 0 && templates[0].Type == Dns.Enums.TemplateTypes.Request && templates[0].Data != null) {
                                request.Query(templates[0].Data);
                            }
                            projPermissions.forEach(function (pItem) {
                                permissions.push(pItem);
                            });
                            bind(request, parentRequest, [], requestTypes[0], workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, [], [], fieldOptions, permissions, visualTerms, null, false);
                        });
                    });
                }
                else {
                    //This is an existing request, need to look for the task and workflowactivity id and display accordingly.
                    $.when(Dns.WebApi.Requests.Get(id), Dns.WebApi.Tasks.ByRequestID(id)).done(function (requests, tasks) {
                        request = new Dns.ViewModels.RequestViewModel(requests[0]);
                        var projectID = request.ProjectID();
                        var parentRequestID = requests[0].ParentRequestID;
                        $.when(parentRequestID == null ? [] : Dns.WebApi.Requests.Get(parentRequestID), Dns.WebApi.Requests.RequestDataMarts(request.ID()), Dns.WebApi.RequestTypes.Get(request.RequestTypeID()), Dns.WebApi.Workflow.GetWorkflowActivity(request.CurrentWorkFlowActivityID()), Dns.WebApi.Projects.GetActivityTreeByProjectID(request.ProjectID()), Dns.WebApi.RequestUsers.List('RequestID eq ' + id), Dns.WebApi.Projects.GetFieldOptions(projectID, User.ID), Dns.WebApi.Projects.GetPermissions([request.ProjectID()], [Permissions.Request.AssignRequestLevelNotifications, Permissions.Project.EditRequestID, Permissions.Request.OverrideDataMartRoutingStatus, Permissions.Request.ApproveRejectResponse, Permissions.Request.ChangeRoutingsAfterSubmission, Permissions.Project.ResubmitRequests]), Dns.WebApi.Organizations.GetPermissions([request.OrganizationID()], [Permissions.Request.AssignRequestLevelNotifications, Permissions.Request.ChangeRoutingsAfterSubmission]), Dns.WebApi.Response.GetResponseGroupsByRequestID(request.ID()), Dns.WebApi.Response.CanViewResponses(request.ID())).done(function (parentRequests, requestDataMarts, requestTypes, workflowActivities, activityTree, requestUsers, fieldOptions, projPermissions, reqTypePermissions, responseGroups, canViewResponses) {
                            if (parentRequests.length != 0) {
                                parentRequest = new Dns.ViewModels.RequestViewModel(parentRequests[0]);
                            }
                            workFlowActivity = workflowActivities[0];
                            Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(request.ProjectID(), workFlowActivity.ID, request.RequestTypeID(), requestTypeWorkflowActivityPermissions)
                                .done(function (permissions) {
                                projPermissions.forEach(function (pItem) {
                                    permissions.push(pItem);
                                });
                                reqTypePermissions.forEach(function (pItem) {
                                    if (permissions.indexOf(pItem) < 0) {
                                        permissions.push(pItem);
                                    }
                                });
                                bind(request, parentRequest, requestDataMarts, requestTypes[0], workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, tasks, requestUsers, fieldOptions, permissions, visualTerms, responseGroups, canViewResponses[0]);
                            });
                        });
                    });
                }
            });
        }
        Details.init = init;
        function bind(request, parentRequest, requestDataMarts, requestType, workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, tasks, requestUsers, fieldOptions, screenPermissions, visualTerms, responseGroups, canViewResponses) {
            $(function () {
                //Load the activity into the task panel.
                var activity = ko.utils.arrayFirst(WorkflowActivityList, function (item) {
                    return item.WorkflowID.toLowerCase() == request.WorkflowID().toLowerCase() && item.ID.toLowerCase() == workFlowActivity.ID.toLowerCase();
                });
                if (activity != null) {
                    $("#TaskContent").load("/workflow/workflowrequests/" + activity.Path);
                }
                else {
                    var commonActivity = ko.utils.arrayFirst(WorkflowActivityList, function (item) {
                        return item.ID == workFlowActivity.ID && item.WorkflowID == "";
                    });
                    $("#TaskContent").load("/workflow/workflowrequests/" + commonActivity.Path);
                }
                var currentTask = ko.utils.arrayFirst(tasks, function (item) { return item.WorkflowActivityID == request.CurrentWorkFlowActivityID() && item.EndOn == null; });
                var bindingControl = $("#ContentWrapper");
                Details.rovm = new RequestOverviewViewModel(request, parentRequest, requestDataMarts, requestType, workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, requestUsers, fieldOptions, bindingControl, screenPermissions, visualTerms, responseGroups, canViewResponses, currentTask);
                var taskID = Global.GetQueryParam("TaskID");
                //If new, or TaskID passed, set the tab to the Task tab
                if (workFlowActivity.End) {
                    //$("#tabs #aTask").remove();
                    $('#tabs #aDocuments').remove();
                }
                else if (!request.ID() || taskID) {
                    $("#tabs #aTask").tab('show');
                }
                ko.applyBindings(Details.rovm, bindingControl[0]);
                var viewOverview = ko.utils.arrayFirst(screenPermissions, function (p) { return p.toLowerCase() == Permissions.ProjectRequestTypeWorkflowActivities.ViewRequestOverview.toLowerCase(); }) != null;
                var viewComments = ko.utils.arrayFirst(screenPermissions, function (p) { return p.toLowerCase() == Permissions.ProjectRequestTypeWorkflowActivities.ViewComments.toLowerCase(); }) != null;
                var viewTask = ko.utils.arrayFirst(screenPermissions, function (p) { return p.toLowerCase() == Permissions.ProjectRequestTypeWorkflowActivities.ViewTask.toLowerCase(); }) != null;
                var viewDocuments = ko.utils.arrayFirst(screenPermissions, function (p) { return p.toLowerCase() == Permissions.ProjectRequestTypeWorkflowActivities.ViewDocuments.toLowerCase(); }) != null;
                var assignRequestNotifications = ko.utils.arrayFirst(screenPermissions, function (p) { return p.toLowerCase() == Permissions.Request.AssignRequestLevelNotifications.toLowerCase(); }) != null;
                var viewTrackingTable = ko.utils.arrayFirst(screenPermissions, function (p) { return p.toLowerCase() == Permissions.ProjectRequestTypeWorkflowActivities.ViewTrackingTable.toLowerCase(); }) != null;
                // Load the view of the criteria only if #viewQueryComposer element is present
                if (viewOverview && $('#viewQueryComposer').length) {
                    Details.rovm.OverviewQCviewViewModel = Plugins.Requests.QueryBuilder.View.init(JSON.parse(request.Query()), visualTerms, $('#overview-queryview'));
                }
                //Notifications
                if (assignRequestNotifications) {
                    Controls.WFNotifications.List.init($('#WFNotifications'), screenPermissions, Details.rovm.Request.ID(), request.CurrentWorkFlowActivity(), request.CurrentWorkFlowActivityID());
                }
                //history
                Controls.WFHistory.List.init(request.ID() || Constants.GuidEmpty);
                Controls.WFHistory.List.HistoryItemsChanged.subscribe(function (hasHistory) { Details.rovm.HasHistory(hasHistory); });
                //init activity specific comments
                var activityCommentsVM = viewComments ? Controls.WFComments.List.init($('#Comments'), screenPermissions, Details.rovm.Request.ID(), request.CurrentWorkFlowActivity(), request.CurrentWorkFlowActivityID()) : null;
                //init all comments for request; user needs to have permission to view the overview
                var overallCommentsVM = (viewOverview) ? Controls.WFComments.List.init($('#OverallComments'), screenPermissions, Details.rovm.Request.ID(), null, null) : null;
                if (viewComments) {
                    activityCommentsVM.OnNewCommentAdded.subscribe(function (newComments) {
                        //there will never be document references for new comments from the comment control.
                        activityCommentsVM.AddCommentToDataSource(newComments, null);
                    });
                }
                if (overallCommentsVM) {
                    overallCommentsVM.OnNewCommentAdded.subscribe(function (newComments) {
                        //there will never be document references for new comments from the comment control.
                        overallCommentsVM.AddCommentToDataSource(newComments, null);
                    });
                }
                if (viewComments || viewOverview) {
                    $.when(Dns.WebApi.Comments.ByRequestID(request.ID() || Constants.GuidEmpty, null), Dns.WebApi.Comments.GetDocumentReferencesByRequest(request.ID() || Constants.GuidEmpty, null)).done(function (comments, documentReferences) {
                        if (viewOverview) {
                            var nonTaskComments = ko.utils.arrayFilter(comments, function (c) { return c.WorkflowActivityID == null; });
                            var nonTaskDocumentReferences = ko.utils.arrayFilter(documentReferences, function (d) { return ko.utils.arrayFirst(nonTaskComments, function (c) { return c.ID == d.CommentID; }) != null; });
                            overallCommentsVM.RefreshDataSource(nonTaskComments, nonTaskDocumentReferences);
                        }
                        if (viewComments) {
                            var taskComments = ko.utils.arrayFilter(comments, function (c) { return c.WorkflowActivityID != null; });
                            var taskDocRefs = ko.utils.arrayFilter(documentReferences, function (d) { return ko.utils.arrayFirst(taskComments, function (c) { return c.ID == d.CommentID; }) != null; });
                            activityCommentsVM.RefreshDataSource(taskComments, taskDocRefs);
                        }
                    });
                }
                if (viewDocuments) {
                    var activityDocumentsVM = null;
                    //task specific documents                
                    if (currentTask != null) {
                        activityDocumentsVM = Controls.WFDocuments.List.init(currentTask, tasks.map(function (m) { return m.ID; }), $('#TaskDocuments'), screenPermissions);
                        Details.rovm.SetTaskDocumentsViewModel(activityDocumentsVM);
                        activityDocumentsVM.NewDocumentUploaded.subscribe(function (newDocument) {
                            //get comments for the document
                            Dns.WebApi.Comments.ByDocumentID(newDocument.ID).done(function (comments) {
                                //create the document references
                                var documentReferences = ko.utils.arrayMap(comments, function (c) {
                                    return {
                                        CommentID: c.ID,
                                        DocumentID: newDocument.ID,
                                        DocumentName: newDocument.Name,
                                        FileName: newDocument.FileName,
                                        RevisionSetID: newDocument.RevisionSetID
                                    };
                                });
                                //add the new document to the comment grids.
                                if (viewOverview) {
                                    overallCommentsVM.AddCommentToDataSource(comments, documentReferences);
                                }
                                if (viewComments) {
                                    var taskComments = ko.utils.arrayFilter(comments, function (c) { return c.WorkflowActivityID == request.CurrentWorkFlowActivityID(); });
                                    var taskDocRefs = ko.utils.arrayFilter(documentReferences, function (d) { return ko.utils.arrayFirst(comments, function (c) { return c.ID == d.CommentID; }) != null; });
                                    if (taskComments.length > 0) {
                                        activityCommentsVM.AddCommentToDataSource(taskComments, taskDocRefs);
                                    }
                                }
                            });
                        });
                    }
                    var overallDocumentsVM = null;
                    //non-task specific documents (request documents)
                    if (viewOverview) {
                        overallDocumentsVM = Controls.WFDocuments.List.initForRequest(request.ID(), $('#OverviewDocuments'), screenPermissions);
                        overallDocumentsVM.NewDocumentUploaded.subscribe(function (newDocument) {
                            //get comments for the document
                            Dns.WebApi.Comments.ByDocumentID(newDocument.ID).done(function (comments) {
                                //create the document references
                                var documentReferences = ko.utils.arrayMap(comments, function (c) {
                                    return {
                                        CommentID: c.ID,
                                        DocumentID: newDocument.ID,
                                        DocumentName: newDocument.Name,
                                        FileName: newDocument.FileName,
                                        RevisionSetID: newDocument.RevisionSetID
                                    };
                                });
                                //refresh the comment grids
                                if (viewOverview) {
                                    overallCommentsVM.AddCommentToDataSource(comments, documentReferences);
                                }
                                if (viewComments) {
                                    var taskComments = ko.utils.arrayFilter(comments, function (c) { return c.WorkflowActivityID == request.CurrentWorkFlowActivityID(); });
                                    var taskDocRefs = ko.utils.arrayFilter(documentReferences, function (d) { return ko.utils.arrayFirst(comments, function (c) { return c.ID == d.CommentID; }) != null; });
                                    if (taskComments.length > 0) {
                                        activityCommentsVM.AddCommentToDataSource(taskComments, taskDocRefs);
                                    }
                                }
                            });
                        });
                    }
                } //end view documents permission
                if (viewTask && Controls.WFTrackingTable && Controls.WFTrackingTable.Display) {
                    Controls.WFTrackingTable.Display.init(request.ID(), screenPermissions);
                }
                //Load other tabs here.
                //Use the workflow to use jquery load to load the partial for the task view as required
                //If new request, open Edit Request Metadata Dialog automatically
                var alloweditrequestpermission = ko.utils.arrayFirst(screenPermissions, function (p) { return p.toLowerCase() == Permissions.Project.EditRequestID.toLowerCase(); }) != null;
                ;
                if (request.ID() == null) {
                    Global.Helpers.ShowDialog("Edit Request Metadata", "/workflow/workflowrequests/editwfrequestmetadatadialog", [], 700, 700, { DetailsViewModel: Details.rovm, AllowEditRequestID: alloweditrequestpermission || false, NewRequest: true })
                        .done(function (result) {
                        //if (Plugins.Requests.QueryBuilder.Edit != undefined) {
                        //    Plugins.Requests.QueryBuilder.Edit.vm.Priority(rovm.Request.Priority());
                        //    Plugins.Requests.QueryBuilder.Edit.vm.DueDate(rovm.Request.DueDate());
                        //}
                        //if a template was specified need to refresh the datamarts available
                        if (Plugins.Requests.QueryBuilder.MDQ != undefined && typeof Plugins.Requests.QueryBuilder.MDQ.vm !== "undefined") {
                            Plugins.Requests.QueryBuilder.MDQ.vm.GetCompatibleDataMarts();
                        }
                        else if (!(typeof Plugins.Requests.QueryBuilder.Edit === "undefined") && Plugins.Requests.QueryBuilder.Edit.vm.fileUpload()) {
                            Plugins.Requests.QueryBuilder.Edit.vm.fileUploadDMLoad();
                        }
                    });
                }
            });
        }
        init();
        var RequestDataMartViewModel = (function (_super) {
            __extends(RequestDataMartViewModel, _super);
            function RequestDataMartViewModel(requestDataMart) {
                var _this = _super.call(this, requestDataMart) || this;
                _this.Selected = ko.observable(false);
                _this.Priority = ko.observable(requestDataMart.Priority);
                _this.DueDate = ko.observable(requestDataMart.DueDate);
                return _this;
            }
            return RequestDataMartViewModel;
        }(Dns.ViewModels.RequestDataMartViewModel));
        Details.RequestDataMartViewModel = RequestDataMartViewModel;
        /** Shows a dialog that allows the user to enter a comment. The comment entered will be returned as the result. */
        function PromptForComment() {
            return Global.Helpers.ShowDialog('Enter a Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null).promise();
        }
        Details.PromptForComment = PromptForComment;
        var VirtualRoutingViewModel = (function () {
            function VirtualRoutingViewModel(routing, group) {
                this.DataMartID = null;
                this.ResponseGroupID = null;
                this.Name = '';
                this.IsGroup = false;
                this.Status = Dns.Enums.RoutingStatus.AwaitingResponseApproval;
                this.Messages = '';
                this.Routings = [];
                this.Routings.push(routing);
                this.IsGroup = group != null;
                if (this.IsGroup) {
                    this.ID = group.ID;
                    this.Name = group.Name;
                    this.ResponseGroupID = group.ID;
                }
                else {
                    this.ID = routing.ResponseID;
                    this.Name = routing.DataMart;
                    this.DataMartID = routing.DataMartID;
                }
                this.RequestDataMartID = routing.ID;
                this.RequestID = routing.RequestID;
                this.ResponseTime = routing.ResponseTime;
                this.Status = routing.Status;
                this.Messages = '';
                this.addToMessages(routing.ErrorMessage);
                this.addToMessages(routing.ResponseMessage);
            }
            VirtualRoutingViewModel.prototype.addRoutings = function (routings) {
                var _this = this;
                if (routings == null || routings.length == 0)
                    return;
                ko.utils.arrayFilter(routings, function (n) { return ko.utils.arrayFirst(_this.Routings, function (r) { return r.ID == n.ID; }) == null; }).forEach(function (routing) {
                    _this.addToMessages(routing.ErrorMessage);
                    _this.addToMessages(routing.ResponseMessage);
                    _this.Routings.push(routing);
                });
            };
            VirtualRoutingViewModel.prototype.addToMessages = function (message) {
                if (message != null && message.trim().length > 0) {
                    if (this.Messages != null && this.Messages.trim().length > 0)
                        this.Messages += '<br/>';
                    this.Messages += message;
                }
            };
            return VirtualRoutingViewModel;
        }());
        Details.VirtualRoutingViewModel = VirtualRoutingViewModel;
    })(Details = Requests.Details || (Requests.Details = {}));
})(Requests || (Requests = {}));
//# sourceMappingURL=details.js.map