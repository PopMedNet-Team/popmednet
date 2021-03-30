/// <reference path="../_rootlayout.ts" />
declare var WorkflowActivityList: Requests.Details.IVisualWorkflowActivity[];

module Requests.Details {
    export var rovm: RequestOverviewViewModel;

    export class RequestOverviewViewModel extends Global.PageViewModel {
        public ReadOnly: KnockoutObservable<boolean>;

        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];

        public Request: Dns.ViewModels.RequestViewModel;
        public ParentRequest: Dns.ViewModels.RequestViewModel;
        public RequestType: Dns.Interfaces.IRequestTypeDTO;
        public Identifier: KnockoutComputed<string>;
        public RequestDataMarts: KnockoutObservableArray<RequestDataMartViewModel>;
        public WorkflowActivity: Dns.ViewModels.WorkflowActivityViewModel;
        public CurrentTask: Dns.Interfaces.ITaskDTO;

        public RequesterCenterList: Dns.Interfaces.IRequesterCenterDTO[];
        public WorkPlanTypeList: Dns.Interfaces.IWorkplanTypeDTO[];
        public ReportAggregationLevelList: Dns.Interfaces.IReportAggregationLevelDTO[];
        public PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
        public PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });

        public ProjectActivityTree: Dns.Interfaces.IActivityDTO[];
        public AllActivities: Dns.Interfaces.IActivityDTO[];
        public AllActivityProjects: Dns.Interfaces.IActivityDTO[];

        public TaskOrderName: KnockoutObservable<any>;
        public ActivityName: KnockoutObservable<any>;
        public ActivityProjectName: KnockoutObservable<any>;
        public RefreshBudgetActivities: () => void;

        public Description_Display: KnockoutComputed<string>;
        public SourceTaskOrder_Display: KnockoutComputed<string>;
        public SourceActivity_Display: KnockoutComputed<string>;
        public SourceActivityProject_Display: KnockoutComputed<string>;

        public PurposeOfUse_Display: KnockoutComputed<string>;   
        public Priority_Display: KnockoutComputed<string>;
        public RequesterCenter_Display: KnockoutComputed<string>;
        public ReportAggregationLevel_Display: KnockoutComputed<string>;

        public SetTaskDocumentsViewModel: (viewModel: Controls.WFDocuments.List.ViewModel) => void;
        public RefreshTaskDocuments: () => void;
        public AttachmentsDocuments: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO> = ko.observableArray([]);

        private ValidationFunctions: Array<(request: Dns.ViewModels.RequestViewModel) => boolean> = [];
        private SaveFunctions: Array<(request: Dns.ViewModels.RequestViewModel) => boolean> = [];
        private SaveFormFunctions: Array<(request: Dns.ViewModels.RequestFormViewModel) => boolean> = [];
        private TaskDocumentsViewModel: Controls.WFDocuments.List.ViewModel = null;

        private AssignedWorkflowRequestUsers: any;
        private SelectedRequestUser: KnockoutObservable<Dns.Interfaces.IRequestUserDTO>;
        public onRequestUserRowSelected: (e: any) => void;
        public onRemoveRequestUser: (e: any) => void;
        public onAddRequestUser: () => void;
        public onRefreshRequestUsers: () => void;
        public EnableRemoveRequestUser: KnockoutComputed<boolean>;

        public onEditWFRequestMetadata: () => void;

        public onCopy: () => void;
        public AllowCopy: KnockoutObservable<boolean>;

        public onOpenGraphsTab: () => void;

        public SaveRequestID: KnockoutObservable<any>;
        public SaveFormDTO: Dns.ViewModels.RequestFormViewModel;
        public OverviewQCviewViewModel: Plugins.Requests.QueryBuilder.View.ViewModel = null;
        public HasHistory: KnockoutObservable<boolean> = ko.observable(false);

        public DefaultSaveTaskDocument: () => void;
        public DefaultResultSave: (warningMessage: string) => void;

        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;

        public IsNotTerminatedRequest: KnockoutComputed<boolean>;

        public onShowRoutingHistory: (routing: VirtualRoutingViewModel) => void;

        public RoutingsChanged: KnockoutSubscribable<any>;
        public NotifyMetadataChanged: (details: any) => void;
        public ReloadRoutingsRequired: KnockoutSubscribable<any>;
        public NotifyReloadRoutes: () => void;

        public CompletedRoutings: KnockoutComputed<VirtualRoutingViewModel[]>;
        public IncompleteRoutings: KnockoutComputed<VirtualRoutingViewModel[]>;
        public AllRoutings: KnockoutComputed<VirtualRoutingViewModel[]>;
        public VirtualRoutings: KnockoutObservableArray<VirtualRoutingViewModel>;
        public SelectedCompleteResponses: KnockoutObservableArray<any>;
        public HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        public onToggleCompleteRoutes: KnockoutComputed<boolean>;
        public onViewAggregatedResults: () => void;
        public onViewIndividualResults: () => void;
        private responseIndex: number = 0;
        public CanViewIndividualResponses: boolean;
        public CanViewAggregateResponses: boolean;
        public AllowAggregateView: boolean;
        public RequestIsComplete: KnockoutComputed<boolean>;

        constructor(
            request: Dns.ViewModels.RequestViewModel,
            parentRequest: Dns.ViewModels.RequestViewModel,
            requestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
            requestType: Dns.Interfaces.IRequestTypeDTO,
            workFlowActivity: Dns.Interfaces.IWorkflowActivityDTO,
            requesterCenterList: Dns.Interfaces.IRequesterCenterDTO[],
            workPlanTypeList: Dns.Interfaces.IWorkplanTypeDTO[],
            reportAggregationLevelList: Dns.Interfaces.IReportAggregationLevelDTO[],
            activityTree: Dns.Interfaces.IActivityDTO[],
            requestUsers: Dns.Interfaces.IRequestUserDTO[],
            fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
            bindingControl: JQuery,
            screenPermissions: any[],
            responseGroups: Dns.Interfaces.IResponseGroupDTO[],
            canViewIndividualResponses: boolean,
            canViewAggregateResponses: boolean,
            currentTask: Dns.Interfaces.ITaskDTO,
            requestTypeModels: any[]
        ) {
            super(bindingControl, screenPermissions);

            let self = this;

            this.RoutingsChanged = new ko.subscribable();
            this.ReloadRoutingsRequired = new ko.subscribable();
            this.Request = request;
            this.ParentRequest = parentRequest;
            this.CurrentTask = currentTask;
            
            this.FieldOptions = fieldOptions;

            this.ReadOnly = ko.observable(false);
            this.SaveRequestID = ko.observable("");    

            //Lists
            this.RequestType = requestType;
            this.RequesterCenterList = requesterCenterList;

            this.WorkPlanTypeList = workPlanTypeList;

            //remove RALs that have been deleted
            this.ReportAggregationLevelList = reportAggregationLevelList.filter((ral) => ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)));

            this.Priority_Display = ko.computed(() => {
                let pair = ko.utils.arrayFirst(Dns.Enums.PrioritiesTranslation,(i) => i.value == self.Request.Priority());
                if (pair)
                    return pair.text;

                return '';
            });

            this.RequesterCenter_Display = ko.computed(() => {
                if (self.Request.RequesterCenterID) {
                    let requestCenter = ko.utils.arrayFirst(self.RequesterCenterList,(rc) => { return rc.ID == self.Request.RequesterCenterID(); });
                    if (requestCenter)
                        return requestCenter.Name;
                }
                return '';
            });

            this.ReportAggregationLevel_Display = ko.computed(() => {
                if (self.Request.ReportAggregationLevelID) {
                    let reportAggregationLevel = ko.utils.arrayFirst(self.ReportAggregationLevelList, (ral) => { return ral.ID == self.Request.ReportAggregationLevelID(); });
                    if (reportAggregationLevel != null)
                        return reportAggregationLevel.Name;  
                }
                return '';
            });

            this.ProjectActivityTree = activityTree;
            this.RefreshActivitiesDataSources();

            this.Description_Display = ko.computed(() => {
                return this.Request.Description().split("<p></p>").join("<p>&nbsp;</p>");
            }, this, {pure : true});

            this.SourceTaskOrder_Display = ko.computed(() => {
                if (self.Request.SourceTaskOrderID() == null)
                    return '';

                let sa = ko.utils.arrayFirst(self.ProjectActivityTree,(a) => a.ID == self.Request.SourceTaskOrderID());
                if (sa) {
                    return sa.Name;
                }
            });

            this.SourceActivity_Display = ko.computed(() => {
                if (self.Request.SourceActivityID() == null)
                    return '';

                let sa = ko.utils.arrayFirst(self.AllActivities,(a) => a.ID == self.Request.SourceActivityID());
                if (sa) {
                    return sa.Name;
                }
            });

            this.SourceActivityProject_Display = ko.computed(() => {
                if (self.Request.SourceActivityProjectID() == null)
                    return '';

                let sa = ko.utils.arrayFirst(self.AllActivityProjects,(a) => a.ID == self.Request.SourceActivityProjectID());
                if (sa) {
                    return sa.Name;
                }
            });

            this.PurposeOfUse_Display = ko.computed(() => {
                if (self.Request.PurposeOfUse() == null)
                    return '';
                let pou = ko.utils.arrayFirst(self.PurposeOfUseOptions,(a) => a.Value == self.Request.PurposeOfUse());
                if (pou) {
                    return pou.Name;
                }
				return '';
            });

            this.TaskOrderName = ko.observable<any>();
            this.ActivityName = ko.observable<any>();
            this.ActivityProjectName = ko.observable<any>();

            this.RefreshBudgetActivities = () => {
                if (self.Request.ActivityID()) {
                    //determine what the current budget activity is

                    let findActivity = (id: any) => {
                        if (id == null)
                            return null;

                        for (let i = 0; i < self.ProjectActivityTree.length; i++) {
                            let act: Dns.Interfaces.IActivityDTO = self.ProjectActivityTree[i];
                            if (act.ID == id) {
                                return act;
                            }

                            for (let j = 0; j < act.Activities.length; j++) {
                                let act2: Dns.Interfaces.IActivityDTO = act.Activities[j];
                                if (act2.ID == id) {
                                    return act2;
                                }

                                for (let k = 0; k < act2.Activities.length; k++) {
                                    let act3: Dns.Interfaces.IActivityDTO = act2.Activities[k];
                                    if (act3.ID == id) {
                                        return act3;
                                    }
                                }
                            }
                        }

                        return null;
                    };

                    let currentBudgetActivity = findActivity(self.Request.ActivityID());
                    if (currentBudgetActivity != null) {
                        if (currentBudgetActivity.TaskLevel == 1) {
                            //task order
                            self.TaskOrderName(currentBudgetActivity.Name);
                            self.ActivityName('');
                            self.ActivityProjectName('');

                        } else if (currentBudgetActivity.TaskLevel == 2) {
                            //activity

                            let taskOrder = findActivity(currentBudgetActivity.ParentActivityID);
                            self.TaskOrderName(taskOrder.Name);

                            self.ActivityName(currentBudgetActivity.Name);
                            self.ActivityProjectName('');

                        } else {
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

            this.Identifier = ko.computed<string>({
                read: () => {
                    return self.Request.UserIdentifier() != null ? self.Request.UserIdentifier() : self.Request.Identifier() ? self.Request.Identifier().toString() : null;
                },
                write: (value) => {
                    self.Request.UserIdentifier(value);
                }
            });
            this.RequestDataMarts = ko.observableArray(requestDataMarts.map((rdm) => {
                return new RequestDataMartViewModel(rdm);
            }));

            self.CanViewIndividualResponses = canViewIndividualResponses;
            self.CanViewAggregateResponses = canViewAggregateResponses;

            self.AllowAggregateView = true;

            //Do not allow Aggregate view for request types associated with DataChecker and ModularProgram Models            
            requestTypeModels.forEach((rt) => {
                if (Constants.Guid.equals(rt, '321ADAA1-A350-4DD0-93DE-5DE658A507DF') || Constants.Guid.equals(rt, '1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154') || Constants.Guid.equals(rt, 'CE347EF9-3F60-4099-A221-85084F940EDE')) {
                    self.AllowAggregateView = false;
                }
            });

            self.SelectedCompleteResponses = ko.observableArray([]);
            self.HasSelectedCompleteRoutings = ko.pureComputed(() => self.SelectedCompleteResponses().length > 0);

            let virtualRoutes = [];
            if (responseGroups != null && responseGroups.length > 0) {
                ko.utils.arrayForEach(responseGroups, group => {
                    let routing = ko.utils.arrayFirst(requestDataMarts, r => r.ResponseGroupID == group.ID);

                    let vr = new VirtualRoutingViewModel(routing, group);
                    vr.addRoutings(ko.utils.arrayFilter(requestDataMarts, r => r.ResponseGroupID == group.ID && r.ID != routing.ID));

                    virtualRoutes.push(vr);
                });
            }

            ko.utils.arrayForEach(requestDataMarts, (routing) => {
                if (routing.ResponseGroupID == null) {                    
                    virtualRoutes.push(new VirtualRoutingViewModel(routing, null));
                }
            });
            self.VirtualRoutings = ko.observableArray(virtualRoutes);

            self.CompletedRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.VirtualRoutings(), (routing) => {
                    return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                        routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                        routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                        routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                }).sort(
					function(a,b)
					{ 
						if(a.Name > b.Name)
						{
							return 1;
						}
						if(a.Name < b.Name)
						{
							return -1;
						}
						else 
						{
							return 0;
						}
					}
				);
            });

            ////may need edits to not hide rejected?
            self.IncompleteRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.VirtualRoutings(), (routing) => {
                    return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                        routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                        routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                        routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                }).sort(
					function(a,b)
					{ 
						if(a.Name > b.Name)
						{
							return 1;
						}
						if(a.Name < b.Name)
						{
							return -1;
						}
						else 
						{
							return 0;
						}
					}
				);
            });            

            self.AllRoutings = ko.pureComputed(() => {
                return $.Enumerable.From(self.VirtualRoutings()).OrderBy(x => x.Status).ThenBy(x => x.Name).ToArray();
            });
            
            //Workflow
            this.WorkflowActivity = new Dns.ViewModels.WorkflowActivityViewModel(workFlowActivity);

            //Boolean to hide Edit Metadata button if activity is "Terminate Request"
            this.IsNotTerminatedRequest = ko.computed(() => {
                if (self.WorkflowActivity.ID() == "cc2e0001-9b99-4c67-8ded-a3b600e1c696")
                    return false;
                if (self.WorkflowActivity.ID() != "cc2e0001-9b99-4c67-8ded-a3b600e1c696")
                    return true;

            });

            this.RequestIsComplete = ko.computed(() => {
                return self.Request.Status() == Dns.Enums.RequestStatuses.Complete ||
                    self.Request.Status() == Dns.Enums.RequestStatuses.CompleteWithReport ||
                    self.Request.Status() == Dns.Enums.RequestStatuses.Cancelled ||
                    self.Request.Status() == Dns.Enums.RequestStatuses.Failed ||
                    self.Request.Status() == Dns.Enums.RequestStatuses.RequestRejected ||
                    self.Request.Status() == Dns.Enums.RequestStatuses.TerminatedPriorToDistribution;
            });


            this.WatchTitle(this.Request.Name, "Request: ");

            this.SetTaskDocumentsViewModel = (vm) => {
                self.TaskDocumentsViewModel = vm;
            };

            this.RefreshTaskDocuments = () => {
                if (self.TaskDocumentsViewModel)
                    self.TaskDocumentsViewModel.onRefreshDocuments();

                Controls.WFHistory.List.refreshHistory();
            };

            this.AssignedWorkflowRequestUsers = new kendo.data.DataSource({
                data: requestUsers
            });

            this.SelectedRequestUser = ko.observable(null);

            this.onRemoveRequestUser = (e) => {
                if (self.SelectedRequestUser() == null)
                    return;

                let message = '<div class="alert alert-warning"><p>Are you sure you want to <strong>delete</strong> this workflow role user?</p>';
                message += '<p><strong>' + self.SelectedRequestUser().FullName + ' - ' + self.SelectedRequestUser().WorkflowRole + '</strong></p></div>';

                Global.Helpers.ShowConfirm("Delete Workflow Role User?", message).done(() => {
                    let url = '/RequestUsers/Delete';
                    $.ajax({
                        type: "DELETE",
                        url: !jQuery.support.cors ? '/api/post?Url=' + encodeURIComponent(url) : ServiceUrl + url,
                        dataType: 'json',
                        data: JSON.stringify([self.SelectedRequestUser()]),
                        contentType: 'application/json; charset=utf-8'
                    }).done(() => {

                        let selectedRequestUser = self.SelectedRequestUser();
                        let raw = self.AssignedWorkflowRequestUsers.data();

                        for (let i = raw.length - 1; i >= 0; i--) {
                            let item = raw[i];
                            if (selectedRequestUser.UserID == item.UserID && selectedRequestUser.WorkflowRoleID == item.WorkflowRoleID) {
                                (<kendo.data.DataSource>self.AssignedWorkflowRequestUsers).remove(item);
                            }
                        }

                        self.SelectedRequestUser.valueWillMutate();
                        self.SelectedRequestUser(null);
                        self.SelectedRequestUser.valueHasMutated();

                    });
                });
            };

            this.onAddRequestUser = () => {
                let action: JQueryDeferred<boolean> = null;
                if (!self.Request.ID()) {
                    //save the request in current state   
                    if (!this.Validate())
                        return;

                    if (!this.ValidateRequest())
                        return;

                    if (!this.SaveRequest(false))
                        return;

                    action = self.DefaultSave(false);

                } else {
                    action = $.Deferred<boolean>().resolve();
                }

                action.done(() => {
                    Global.Helpers.ShowDialog("Add Workflow User", "/workflow/workflowrequests/addrequestuserdialog", ['close'], 714, 550, { Request: self.Request })
                        .done((results: Dns.Interfaces.IRequestUserDTO[]) => {

                        if (!results)
                            return;

                        results.forEach(u => {
                            (<kendo.data.DataSource>self.AssignedWorkflowRequestUsers).add(u);
                        });

                    });
                });
            };

            this.onRefreshRequestUsers = () => {
                Dns.WebApi.RequestUsers.List('RequestID eq ' + self.Request.ID()).done((requestUsers) => {
                    (<kendo.data.DataSource>self.AssignedWorkflowRequestUsers).data(requestUsers);
                });
            };

            let editidpermission = this.HasPermission(PMNPermissions.Project.EditRequestID);

            this.onEditWFRequestMetadata = () => {

                if (!self.Validate()) {
                    //trigger validation on the form before allowing edit of metadata
                    Global.Helpers.ShowErrorAlert('Validation Error', '<p class="alert alert-warning" role="alert">One or more validation errors were found in the current task editor, and need to be addressed before continuing Metadata edit.</p>');
                    return;
                } 

                //save current Priority and Due Date settings in order to monitor changes after metadata has been edited
                let oldRequestPriority = self.Request.Priority();
                let oldRequestDueDate = self.Request.DueDate();

                Global.Helpers.ShowDialog("Edit Request Metadata", "/workflow/workflowrequests/editwfrequestmetadatadialog", [], 700, 700, { DetailsViewModel: self, AllowEditRequestID: editidpermission, NewRequest: false, OldRequestPriority: oldRequestPriority, OldRequestDueDate: oldRequestDueDate })
                    .done((result: any) => {

                    });

            };

            this.AllowCopy = ko.observable(false);
            if (self.Request.ID() != null) {
                Dns.WebApi.Requests.AllowCopyRequest(self.Request.ID()).done((allow) => {
                    self.AllowCopy(allow[0]);
                });
            }

            this.onCopy = () => {
                Dns.WebApi.Requests.CopyRequest(self.Request.ID()).done((reqID) => {
                    //load new request page using the new request ID
                    let q = '//' + window.location.host + '/requests/details?ID=' + reqID[0];
                    window.location.assign(q);
   
                }).fail((ex) => {
                });
            };

            this.NotifyMetadataChanged = (item: any) => {
                self.RoutingsChanged.notifySubscribers(item);
            };

            this.NotifyReloadRoutes = () => {
                //notify subscribers that the routes have changed and should be reloaded
                self.ReloadRoutingsRequired.notifySubscribers();
            };

            this.onRequestUserRowSelected = (e) => {
                let grid = $(e.sender.wrapper).data('kendoGrid');
                let rows = grid.select();
                if (rows.length == 0) {
                    self.SelectedRequestUser(null);
                    return;
                }

                let selectedRequestUser = <any>grid.dataItem(rows[0]);
                self.SelectedRequestUser(<Dns.Interfaces.IRequestUserDTO> selectedRequestUser);
            };

            this.EnableRemoveRequestUser = ko.computed(() => {
                return self.SelectedRequestUser() != null && !self.SelectedRequestUser().IsRequestCreatorRole;
            });

            this.DefaultSaveTaskDocument = () => {
                self.DefaultResultSave('<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The request needs to be saved before being able to upload a document.</p> <p style="font-size:larger;">Would you like to save the Request now?</p><p><small>(This will cause the page to be reloaded, and you will need to initiate the upload again.)</small></p></div>');
            };

            this.DefaultResultSave = (warningMessage: string) => {
                Global.Helpers.ShowConfirm('Save Required Before Continuing', warningMessage).done(() => {
                    this.DefaultSave(true);
                });
            }

            self.IsFieldRequired = (id: string) => {
                let options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            self.IsFieldVisible = (id: string) => {
                let options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
            };

            self.onShowRoutingHistory = (routing: VirtualRoutingViewModel) => {
                Dns.WebApi.Requests.GetResponseHistory(routing.RequestDataMartID, routing.RequestID).done((responseHistory) => {
                    Global.Helpers.ShowDialog("History", "/dialogs/routinghistory", ['close'], 600, 300, { responseHistory: responseHistory[0] })
                        .done(() => {
                        });
                });   
            };

            let setupResponseTabView = (responseView: Dns.Enums.TaskItemTypes) => {
                let tabID = 'responsedetail_' + self.responseIndex;
                self.responseIndex++;

                let q = '//' + window.location.host + '/workflowrequests/responsedetail';
                q += '?id=' + self.SelectedCompleteResponses();
                q += '&view=' + responseView;
                q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();

                let contentFrame = document.createElement('iframe');
                contentFrame.src = q;
                contentFrame.setAttribute('style', 'margin:0px;padding:0px;border:none;width:100%;height:940px;min-height:940px;');
                contentFrame.setAttribute('scrolling', 'no');

                let contentContainer = $('<div class="tab-pane fade" id="' + tabID + '"></div>');
                contentContainer.append(contentFrame);
                $('#root-tab-content').append(contentContainer);

                let tl = $('<li></li>');
                let ta = $('<a href="#' + tabID + '" role="tab" data-toggle="tab" style="display:inline-block">Response Detail <i class="glyphicon glyphicon-remove-circle"></i></a>');

                let tac = ta.find('i');

                tac.click((evt) => {
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

            self.onToggleCompleteRoutes = ko.pureComputed<boolean>({
                read: () => {
                    return self.CompletedRoutings().length > 0 && self.SelectedCompleteResponses().length === self.CompletedRoutings().length;
                },
                write: (value) => {
                    if (value) {
                        let allID = ko.utils.arrayMap(self.CompletedRoutings(), (i) => { return i.ID; });
                        self.SelectedCompleteResponses(allID);
                    } else {
                        self.SelectedCompleteResponses([]);
                    }
                }
            });

            self.onViewAggregatedResults = () => {
                setupResponseTabView(Dns.Enums.TaskItemTypes.AggregateResponse);
            };

            self.onViewIndividualResults = () => {
                setupResponseTabView(Dns.Enums.TaskItemTypes.Response);
            };

        }

        public DefaultSave(reload: boolean, isNewRequest: boolean = null, errorHandler: (err: any) => void = null): JQueryDeferred<boolean> {
            let self = this;
            let deferred = $.Deferred<boolean>();
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

            let dto: Dns.Interfaces.IRequestDTO = self.Request.toData();

            Dns.WebApi.Requests.CompleteActivity({
                DemandActivityResultID: this.SaveRequestID(),
                Dto: dto,
                DataMarts: Requests.Details.rovm.RequestDataMarts().map((item) => {
                    return item.toData();
                }),
                Data: JSON.stringify(self.SaveFormDTO == null ? null : self.SaveFormDTO.toData()),
                Comment: null
            }).done((results) => {

                let result = results[0];
                if (result.Uri) {
                    //// Need to Go back to API cause results dont return back FULL DTO info
                    $.when<any>(Dns.WebApi.Requests.Get(result.Entity.ID),
                        Dns.WebApi.Tasks.ByRequestID(result.Entity.ID))
                        .done((res: Dns.Interfaces.IRequestDTO[], tasks: Dns.Interfaces.ITaskDTO[]) => {
                            self.Request.update(res[0]);
                            self.CurrentTask = ko.utils.arrayFirst(tasks, (t: Dns.Interfaces.ITaskDTO) => { return t.WorkflowActivityID == self.Request.CurrentWorkFlowActivityID() && t.EndOn == null; });
                            
                            if (reload) {
                                Global.Helpers.RedirectTo(result.Uri);
                            } else {
                                Global.Helpers.HistoryReplaceState(self.Request.Name(), result.Uri);
                            }

                            deferred.resolve(true);
                        });

                    } else {
                        //Update the request etc. here 
                        self.Request.ID(result.Entity.ID);
                        self.Request.Timestamp(result.Entity.Timestamp);
                    
                        Global.Helpers.HistoryReplaceState(self.Request.Name(), '/requests/details?ID=' + self.Request.ID());

                        deferred.resolve(true);
                    }

                }).fail((er) => {
                    if (errorHandler != null)
                        errorHandler(er);
                });
            return deferred;

        }

        public DataMartSelectAll() {
            rovm.RequestDataMarts().forEach((dm) => {
                dm.Selected(true);
            });
        }

        private RefreshActivitiesDataSources() {
            let activities = [];
            let activityProjects = [];

            this.ProjectActivityTree.forEach((to) => {

                activities = activities.concat(to.Activities);

                to.Activities.forEach((a) => {
                    activityProjects = activityProjects.concat(a.Activities);
                });
            });

            this.AllActivities = activities;
            this.AllActivityProjects = activityProjects;
        }

        public RegisterValidationFunction(fnc: (request: Dns.ViewModels.RequestViewModel) => boolean) {
            this.ValidationFunctions.push(fnc);
        }

        public RegisterRequestSaveFunction(fnc: (request: Dns.ViewModels.RequestViewModel) => boolean) {
            this.SaveFunctions.push(fnc);
        }

        public RegisterRequestFormSaveFunction(fnc: (request: Dns.ViewModels.RequestFormViewModel) => boolean) {
            this.SaveFormFunctions.push(fnc);
        }

        public Save(showMessage: boolean = true): JQueryDeferred<boolean> {
            let deferred = $.Deferred<boolean>();

            if (!this.Validate())
                return deferred.reject();

            if (!this.ValidateRequest())
                return deferred.reject();

            if (!this.SaveRequest(false))
                return deferred.reject();
            
            //Post the request as is to the server
            return this.PostSave(showMessage);
        }

        public Cancel() {
            window.history.back();
        }


        public Terminate() {
            Global.Helpers.ShowConfirm("Terminate Confirmation", "<p>Are you sure that you wish to terminate this Request?</p>").done(() => {
                Dns.WebApi.Requests.TerminateRequest(rovm.Request.ID()).done(() => {
                    window.history.back();
                });
            });
        }



        private ValidateRequest(): boolean {
            for (let i = 0; i < this.ValidationFunctions.length; i++) {
                if (!this.ValidationFunctions[i](this.Request))
                    return false;
            }

            return true;
        }

        private SaveRequest(submit: boolean): boolean {
            for (let i = 0; i < this.SaveFunctions.length; i++) {
                if (!this.SaveFunctions[i](this.Request))
                    return false;
            }

            //Do whatever other common save stuff happens here.

            return true;
        }
        private SaveFormRequest(submit: boolean): boolean {
            for (let i = 0; i < this.SaveFormFunctions.length; i++) {
                if (!this.SaveFormFunctions[i](this.SaveFormDTO))
                    return false;
            }

            //Do whatever other common save stuff happens here.

            return true;
        }

        private PostSave(showMessage: boolean): JQueryDeferred<boolean> {
            let deferred = $.Deferred<boolean>();
            let request = this.Request.toData();

            //Post it as a save
            Dns.WebApi.Requests.InsertOrUpdate([request]).done((results) => {
                rovm.Request.ID(results[0].ID);
                rovm.Request.Timestamp(results[0].Timestamp);
                
                //Update the history
                window.history.replaceState(null, window.document.title, "/requests/details?ID=" + results[0].ID);

                //Save the datamarts here

                //Save anything else here if you want.

                if (showMessage)
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                        deferred.resolve(true);
                        return;
                    });

                deferred.resolve(true);
            }).fail(() => {
                deferred.fail();
            });

            return deferred;
        }

        public UpdateUrl() {
            if (this.Request.ID()) {
                window.history.replaceState(null, this.Request.Name(), "/requests/details?ID=" + this.Request.ID());
            } else {
                window.history.replaceState(null, this.Request.Name(), window.location.href);
            }
        }
    }

    export function init() {
        let id: any = Global.GetQueryParam("ID");
        let requestTypeID: any;
        let request: Dns.ViewModels.RequestViewModel;
        let parentRequest: Dns.ViewModels.RequestViewModel;
        let workFlowActivity: Dns.Interfaces.IWorkflowActivityDTO;
        let requestTypeWorkflowActivityPermissions = [
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewRequestOverview,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewTask,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.EditTask,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ModifyAttachments,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewAttachments,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewComments,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.AddComments,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewDocuments,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.AddDocuments,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ReviseDocuments,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.CloseTask,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewRequestOverview,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.TerminateWorkflow,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.EditRequestMetadata,
            PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewTrackingTable
        ];
        $.when<any>(
            Dns.WebApi.Requests.ListRequesterCenters(null, "ID,Name", "Name"),
            Dns.WebApi.Requests.ListWorkPlanTypes(null, "ID,Name", "Name"),
            Dns.WebApi.Requests.ListReportAggregationLevels(null, "ID,Name,DeletedOn", "Name"),
        ).done((requesterCenterList, workPlanTypeList, reportAggregationLevelList) => {

            if (!id) {
                //Get the starting workflow activity
                requestTypeID = Global.GetQueryParam("requestTypeID");
                let projectID = Global.GetQueryParam("ProjectID");
                //let templateID: any = Global.GetQueryParam("templateID");
                let parentRequestID = Global.GetQueryParam("ParentRequestID");
                let userID = Global.GetQueryParam("UserID");

                //This is new, we need to get extensive information about the workflow, request type, etc.
                $.when<any>(
                    parentRequestID == null ? [] : Dns.WebApi.Requests.Get(parentRequestID),
                    Dns.WebApi.RequestTypes.Get(requestTypeID),
                    Dns.WebApi.Workflow.GetWorkflowEntryPointByRequestTypeID(requestTypeID),
                    Dns.WebApi.Templates.GetByRequestType(requestTypeID, null, null, "Order"),
                    Dns.WebApi.Projects.GetFieldOptions(projectID, User.ID),
                    Dns.WebApi.Projects.GetPermissions([projectID], [PMNPermissions.Request.AssignRequestLevelNotifications, PMNPermissions.Project.EditRequestID, PMNPermissions.Request.OverrideDataMartRoutingStatus, PMNPermissions.Request.ApproveRejectResponse, PMNPermissions.Request.SkipSubmissionApproval]),
                    Dns.WebApi.Projects.GetActivityTreeByProjectID(projectID)
                    ).done((
                    parentRequests: Dns.Interfaces.IRequestDTO[],
                    requestTypes: Dns.Interfaces.IRequestTypeDTO[],
                    workflowActivities: Dns.Interfaces.IWorkflowActivityDTO[],
                    templates: Dns.Interfaces.ITemplateDTO[],
                    fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
                    projPermissions: any[],
                    activityTree: Dns.Interfaces.IActivityDTO[]
                    ) => {

                    if (parentRequests.length != 0) {
                        parentRequest = new Dns.ViewModels.RequestViewModel(parentRequests[0]);
                    }

                    workFlowActivity = workflowActivities[0];

                        Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(projectID, workFlowActivity.ID, requestTypeID, requestTypeWorkflowActivityPermissions)
                        .done((permissions: any[]) => {

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
                            request.ProjectID(projectID);
                            request.OrganizationID(User.EmployerID);
                            request.WorkflowID(requestTypes[0].WorkflowID);
                            request.ParentRequestID(parentRequestID);

                            if (templates != null && templates.length > 0) {
                                let sorted = templates.sort((a, b) => { return a.Order - b.Order; });

                                let mqViewModel = new Dns.ViewModels.QueryComposerRequestViewModel();
                                mqViewModel.SchemaVersion("2.0");
                                mqViewModel.Header.Priority(Dns.Enums.Priorities.Medium);
                                for (let i = 0; i < sorted.length; i++) {
                                    if (sorted[i].Type != Dns.Enums.TemplateTypes.Request)
                                        continue;

                                    mqViewModel.Queries.push(new Dns.ViewModels.QueryComposerQueryViewModel(JSON.parse(sorted[i].Data)));
                                }

                                request.Query(JSON.stringify(mqViewModel.toData()));
                            }

                            projPermissions.forEach((pItem) => {
                                permissions.push(pItem);
                            });

                            bind(request, parentRequest, [], requestTypes[0], workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, [], [], fieldOptions, permissions, null, false, false, []);


                        });


                });
            } else {
                //This is an existing request, need to look for the task and workflowactivity id and display accordingly.
                $.when<any>(
                    Dns.WebApi.Requests.Get(id),
                    Dns.WebApi.Tasks.ByRequestID(id)
                    ).done((requests: Dns.Interfaces.IRequestDTO[], tasks: Dns.Interfaces.ITaskDTO[]) => {
                    request = new Dns.ViewModels.RequestViewModel(requests[0]);
                    let projectID = request.ProjectID();
                    let parentRequestID = requests[0].ParentRequestID;

                    $.when<any>(
                        parentRequestID == null ? [] : Dns.WebApi.Requests.Get(parentRequestID),
                        Dns.WebApi.Requests.RequestDataMarts(request.ID()),
                        Dns.WebApi.RequestTypes.Get(request.RequestTypeID()),
                        Dns.WebApi.Workflow.GetWorkflowActivity(request.CurrentWorkFlowActivityID()),
                        Dns.WebApi.Projects.GetActivityTreeByProjectID(request.ProjectID()),
                        Dns.WebApi.RequestUsers.List('RequestID eq ' + id),
                        Dns.WebApi.Projects.GetFieldOptions(projectID, User.ID),
                        Dns.WebApi.Projects.GetPermissions([request.ProjectID()], [PMNPermissions.Request.AssignRequestLevelNotifications, PMNPermissions.Project.EditRequestID, PMNPermissions.Request.OverrideDataMartRoutingStatus, PMNPermissions.Request.ApproveRejectResponse, PMNPermissions.Request.ChangeRoutingsAfterSubmission, PMNPermissions.Project.ResubmitRequests, PMNPermissions.Request.SkipSubmissionApproval]),
                        Dns.WebApi.Organizations.GetPermissions([request.OrganizationID()], [PMNPermissions.Request.AssignRequestLevelNotifications, PMNPermissions.Request.ChangeRoutingsAfterSubmission]),
                        Dns.WebApi.Response.GetResponseGroupsByRequestID(request.ID()),
                        Dns.WebApi.Response.CanViewIndividualResponses(request.ID()),
                        Dns.WebApi.Response.CanViewAggregateResponses(request.ID()),
                        Dns.WebApi.Requests.GetRequestTypeModels(request.ID()),
                        Dns.WebApi.Templates.GetByRequestType(request.RequestTypeID(), null, null, "Order"),
                    //Get the work flow activity that it's on
                        ).done((
                            parentRequests: Dns.Interfaces.IRequestDTO[],
                            requestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
                            requestTypes: Dns.Interfaces.IRequestTypeDTO[],
                            workflowActivities: Dns.Interfaces.IWorkflowActivityDTO[],
                            activityTree: Dns.Interfaces.IActivityDTO[],
                            requestUsers: Dns.Interfaces.IRequestUserDTO[],
                            fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
                            projPermissions: any[],
                            reqTypePermissions: any[],
                            responseGroups: Dns.Interfaces.IResponseGroupDTO[],
                            canViewIndividualResponses: boolean[],
                            canViewAggregateResponses: boolean[],
                            requestTypeModels: any[],
                            requestTypeTemplates: Dns.Interfaces.ITemplateDTO[]
                        ) => {
                        if (parentRequests.length != 0) {
                            parentRequest = new Dns.ViewModels.RequestViewModel(parentRequests[0]);
                        }

                        workFlowActivity = workflowActivities[0];
                        Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(request.ProjectID(), workFlowActivity.ID, request.RequestTypeID(), requestTypeWorkflowActivityPermissions)
                            .done((permissions: any[]) => {
                            projPermissions.forEach((pItem) => {
                                permissions.push(pItem);
                            });
                            reqTypePermissions.forEach((pItem) => {
                                if (permissions.indexOf(pItem) < 0) {
                                    permissions.push(pItem);
                                }
                                });

                                let obj = JSON.parse(request.Query());
                                if (obj && obj.Header.hasOwnProperty('ComposerInterface')) {
                                    //only a queryDTO will have ComposerInterface as a property of the Header.
                                    //going to assume request type hasn't been converted to the new multi-query schema.
                                    //Automactially upgrade, assume the current json only has a single query and it matches the first specifiec for the request type.
                                    let queryObj = obj as Dns.Interfaces.IQueryComposerQueryDTO;
                                    let requestTypeTemplate = requestTypeTemplates[0];
                                    queryObj.Header.ID = requestTypeTemplate.ID;
                                    queryObj.Header.ComposerInterface = requestTypeTemplate.ComposerInterface;
                                    queryObj.Header.QueryType = requestTypeTemplate.QueryType;

                                    if ((queryObj.Header.Name || '').length == 0) {
                                        queryObj.Header.Name = requestTypeTemplate.Name;
                                    }

                                    //convert to multi-query
                                    let mq: Dns.Interfaces.IQueryComposerRequestDTO = new Dns.ViewModels.QueryComposerRequestViewModel().toData();
                                    mq.SchemaVersion = "2.0";
                                    mq.Header.ID = id;
                                    mq.Header.DueDate = request.DueDate();
                                    mq.Header.Description = request.Description();
                                    mq.Header.Name = request.Name();
                                    mq.Header.Priority = request.Priority();
                                    mq.Header.SubmittedOn = request.SubmittedOn();
                                    mq.Header.ViewUrl = queryObj.Header.ViewUrl;
                                    mq.Queries.push(queryObj);

                                    request.Query(JSON.stringify(mq));
                                }

                            bind(request, parentRequest, requestDataMarts, requestTypes[0], workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, tasks, requestUsers, fieldOptions, permissions, responseGroups, canViewIndividualResponses[0], canViewAggregateResponses[0], requestTypeModels);
                        });

                    });
                });
            }
        });
    }

    function bind(
        request: Dns.ViewModels.RequestViewModel,
        parentRequest: Dns.ViewModels.RequestViewModel,
        requestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
        requestType: Dns.Interfaces.IRequestTypeDTO,
        workFlowActivity: Dns.Interfaces.IWorkflowActivityDTO,
        requesterCenterList,
        workPlanTypeList,
        reportAggregationLevelList,
        activityTree: Dns.Interfaces.IActivityDTO[],
        tasks: Dns.Interfaces.ITaskDTO[],
        requestUsers: Dns.Interfaces.IRequestUserDTO[],
        fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
        screenPermissions: any[],
        responseGroups: Dns.Interfaces.IResponseGroupDTO[],
        canViewIndividualResponses: boolean,
        canViewAggregateResonses: boolean,
        requestTypeModels: any[]
        ) { //Add Params here after data is retrieved
        $(() => {
            
            //Load the activity into the task panel.
            let activity = ko.utils.arrayFirst(WorkflowActivityList, function (item) {
                return item.WorkflowID.toLowerCase() == request.WorkflowID().toLowerCase() && item.ID.toLowerCase() == workFlowActivity.ID.toLowerCase() 

            });
            if (activity != null) {
                $("#TaskContent").load("/workflow/workflowrequests/" + activity.Path);
            }
            else {
                let commonActivity = ko.utils.arrayFirst(WorkflowActivityList, function (item) {
                    return item.ID == workFlowActivity.ID && item.WorkflowID == ""
                });
                $("#TaskContent").load("/workflow/workflowrequests/" + commonActivity.Path);
            }

            let currentTask: Dns.Interfaces.ITaskDTO = ko.utils.arrayFirst(tasks, (item) => { return item.WorkflowActivityID == request.CurrentWorkFlowActivityID() && item.EndOn == null; });
            let bindingControl = $("#ContentWrapper");

            rovm = new RequestOverviewViewModel(request, parentRequest, requestDataMarts, requestType, workFlowActivity, requesterCenterList, workPlanTypeList, reportAggregationLevelList, activityTree, requestUsers, fieldOptions, bindingControl, screenPermissions, responseGroups, canViewIndividualResponses, canViewAggregateResonses, currentTask, requestTypeModels);

            let taskID: any = Global.GetQueryParam("TaskID");
            //If new, or TaskID passed, set the tab to the Task tab
            if (workFlowActivity.End) {

            } else if (!request.ID() || taskID) {
                $("#tabs #aTask").tab('show');
            }

            ko.applyBindings(rovm, bindingControl[0]);

            let viewOverview = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewRequestOverview.toLowerCase()) != null;
            let viewComments = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewComments.toLowerCase()) != null;
            let viewTask = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewTask.toLowerCase()) != null;
            let viewDocuments = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewDocuments.toLowerCase()) != null;
            let viewAttachments = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewAttachments.toLowerCase()) != null;
            let assignRequestNotifications = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.Request.AssignRequestLevelNotifications.toLowerCase()) != null;
            let viewTrackingTable = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewTrackingTable.toLowerCase()) != null;
      
            // Load the view of the criteria only if #viewQueryComposer element is present
            if (viewOverview && $('#QueryComposerOverview').length) {
                rovm.OverviewQCviewViewModel = Plugins.Requests.QueryBuilder.View.initialize(JSON.parse(request.Query()), request, $('#overview-queryview'));
            }
            
            //Notifications
            if (assignRequestNotifications) {
                Controls.WFNotifications.List.init($('#WFNotifications'), screenPermissions, rovm.Request.ID(), request.CurrentWorkFlowActivity(), request.CurrentWorkFlowActivityID());
            }

            //history
            Controls.WFHistory.List.init(request.ID() || Constants.GuidEmpty);
            Controls.WFHistory.List.HistoryItemsChanged.subscribe((hasHistory: boolean) => { rovm.HasHistory(hasHistory); });        

            //init activity specific comments
            let activityCommentsVM = viewComments ? Controls.WFComments.List.init($('#Comments'), screenPermissions, rovm.Request.ID(), request.CurrentWorkFlowActivity(), request.CurrentWorkFlowActivityID()) : null;
            //init all comments for request; user needs to have permission to view the overview
            let overallCommentsVM = (viewOverview) ? Controls.WFComments.List.init($('#OverallComments'), screenPermissions, rovm.Request.ID(), null, null) : null;

            if (viewComments) {
                activityCommentsVM.OnNewCommentAdded.subscribe((newComments) => {
                    //there will never be document references for new comments from the comment control.
                    activityCommentsVM.AddCommentToDataSource(newComments, null);
                });
            }

            if (overallCommentsVM) {
                overallCommentsVM.OnNewCommentAdded.subscribe((newComments) => {
                    //there will never be document references for new comments from the comment control.
                    overallCommentsVM.AddCommentToDataSource(newComments, null);
                });
            }

            if (viewComments || viewOverview) {
                $.when<any>(
                    Dns.WebApi.Comments.ByRequestID(request.ID() || Constants.GuidEmpty, null),
                    Dns.WebApi.Comments.GetDocumentReferencesByRequest(request.ID() || Constants.GuidEmpty, null),
                    Dns.WebApi.Documents.ByTask(tasks.map(m => { return m.ID }), null, null, 'ItemID desc,RevisionSetID desc,CreatedOn desc')
                ).done((comments: Dns.Interfaces.IWFCommentDTO[], documentReferences: Dns.Interfaces.ICommentDocumentReferenceDTO[], docs: Dns.Interfaces.IExtendedDocumentDTO[]) => {

                    if (viewOverview) {
                        let nonTaskComments = ko.utils.arrayFilter(comments, c => { return c.WorkflowActivityID == null; });
                        let nonTaskDocumentReferences = ko.utils.arrayFilter(documentReferences, d => { return ko.utils.arrayFirst(nonTaskComments, c => c.ID == d.CommentID) != null });
                        overallCommentsVM.RefreshDataSource(nonTaskComments, nonTaskDocumentReferences);
                    }

                    if (viewAttachments) {
                        let sets: Dns.Interfaces.IExtendedDocumentDTO[] = [];
                        ko.utils.arrayForEach(docs, (item) => {
                            if (item.Kind === "Attachment.Input" || item.Kind === "Attachment.Output") {
                                let alreadyAdded = ko.utils.arrayFilter(sets, (childItem) => { return item.RevisionSetID === childItem.RevisionSetID });
                                if (alreadyAdded.length === 0) {
                                    let filtered = ko.utils.arrayFilter(docs, (childItems) => { return item.RevisionSetID === childItems.RevisionSetID });
                                    if (filtered.length > 1) {
                                        filtered.sort((a: Dns.Interfaces.IExtendedDocumentDTO, b: Dns.Interfaces.IExtendedDocumentDTO) => {
                                            //sort by version number - highest to lowest, and then date created - newest to oldest
                                            if (a.MajorVersion === b.MajorVersion) {

                                                if (a.MinorVersion === b.MinorVersion) {

                                                    if (a.BuildVersion === b.BuildVersion) {

                                                        if (a.RevisionVersion === b.RevisionVersion) {
                                                            return <any>b.CreatedOn - <any>a.CreatedOn;
                                                        }
                                                        return b.RevisionVersion - a.RevisionVersion;
                                                    }
                                                    return b.BuildVersion - a.BuildVersion;

                                                }
                                                return b.MinorVersion - a.MinorVersion;

                                            }
                                            return b.MajorVersion - a.MajorVersion;
                                        });

                                        sets.push(filtered[0]);
                                    }
                                    else {
                                        sets.push(item);
                                    }
                                }
                            }
                        });
                        rovm.AttachmentsDocuments(sets);
                    }

                    if (viewComments) {
                        let taskComments = ko.utils.arrayFilter(comments, c => { return c.WorkflowActivityID != null; });
                        let taskDocRefs = ko.utils.arrayFilter(documentReferences, d => { return ko.utils.arrayFirst(taskComments, c => c.ID == d.CommentID) != null; });
                        activityCommentsVM.RefreshDataSource(taskComments, taskDocRefs);
                    }

                });
            }

            if (viewDocuments || viewAttachments) {

                let activityDocumentsVM: Controls.WFDocuments.List.ViewModel = null;
                //task specific documents                
                if (currentTask != null) {
                    activityDocumentsVM = Controls.WFDocuments.List.init(currentTask, tasks.map(m => { return m.ID }), $('#TaskDocuments'), screenPermissions);
                    rovm.SetTaskDocumentsViewModel(activityDocumentsVM);

                    activityDocumentsVM.NewDocumentUploaded.subscribe((newDocument: Dns.Interfaces.IExtendedDocumentDTO) => {
                        //get comments for the document
                        Dns.WebApi.Comments.ByDocumentID(newDocument.ID).done(comments => {
                            //create the document references
                            let documentReferences = ko.utils.arrayMap(comments, c => {
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
                                let taskComments = ko.utils.arrayFilter(comments, c => { return c.WorkflowActivityID == request.CurrentWorkFlowActivityID(); });
                                let taskDocRefs = ko.utils.arrayFilter(documentReferences, d => { return ko.utils.arrayFirst(comments, c => c.ID == d.CommentID) != null; });
                                if (taskComments.length > 0) {
                                    activityCommentsVM.AddCommentToDataSource(taskComments, taskDocRefs);
                                }
                            }

                        });
                    });
                } else {

                    //on the completed step, need to list the previous task documents, but do not allow or expect upload
                    //create a dummy complete task to pass the documents view model.
                    let tt = new Dns.ViewModels.TaskViewModel();
                    tt.ID(null);
                    tt.PercentComplete(100);
                    tt.Status(Dns.Enums.TaskStatuses.Complete);
                    tt.Type(Dns.Enums.TaskTypes.Task);

                    activityDocumentsVM = Controls.WFDocuments.List.init(tt.toData(), tasks.map(m => m.ID), $('#TaskDocuments'), screenPermissions);
                    rovm.SetTaskDocumentsViewModel(activityDocumentsVM);

                }


                let overallDocumentsVM: Controls.WFDocuments.List.ViewModel = null;
                //non-task specific documents (request documents)
                if (viewOverview && viewDocuments) {
                    overallDocumentsVM = Controls.WFDocuments.List.initForRequest(request.ID(), $('#OverviewDocuments'), screenPermissions);

                    overallDocumentsVM.NewDocumentUploaded.subscribe((newDocument: Dns.Interfaces.IExtendedDocumentDTO) => {
                        //get comments for the document
                        Dns.WebApi.Comments.ByDocumentID(newDocument.ID).done(comments => {
                            //create the document references
                            let documentReferences = ko.utils.arrayMap(comments, c => {
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
                                let taskComments = ko.utils.arrayFilter(comments, c => { return c.WorkflowActivityID == request.CurrentWorkFlowActivityID(); });
                                let taskDocRefs = ko.utils.arrayFilter(documentReferences, d => { return ko.utils.arrayFirst(comments, c => c.ID == d.CommentID) != null; });
                                if (taskComments.length > 0) {
                                    activityCommentsVM.AddCommentToDataSource(taskComments, taskDocRefs);
                                }
                            }

                        });
                    });

                }

            }//end view documents permission
            
            if (viewTask && Controls.WFTrackingTable && Controls.WFTrackingTable.Display) {
                Controls.WFTrackingTable.Display.init(request.ID(), screenPermissions);
            }

            if (viewTask && Controls.WFEnhancedEventLog && Controls.WFEnhancedEventLog.Display) {
                Controls.WFEnhancedEventLog.Display.init(request.ID(), screenPermissions);
            }

            //Load other tabs here.

            //Use the workflow to use jquery load to load the partial for the task view as required

            //If new request, open Edit Request Metadata Dialog automatically
            let alloweditrequestpermission = ko.utils.arrayFirst(screenPermissions, p => p.toLowerCase() == PMNPermissions.Project.EditRequestID.toLowerCase()) != null;;
            if (request.ID() == null) {
                Global.Helpers.ShowDialog("Edit Request Metadata", "/workflow/workflowrequests/editwfrequestmetadatadialog", [], 700, 700, { DetailsViewModel: rovm, AllowEditRequestID: alloweditrequestpermission || false, NewRequest: true })
                    .done((result: any) => {

                        if (!(typeof Plugins.Requests.QueryBuilder.Edit === "undefined") && Plugins.Requests.QueryBuilder.Edit.vm.IsFileUpload) {
                            Plugins.Requests.QueryBuilder.Edit.vm.fileUploadDMLoad();
                        }

                        Controls.WFHistory.List.setRequestID(rovm.Request.ID());
                });
            }


            // ===== Scroll to Top ==== 
            $(window).scroll(() => {
                if ($(this).scrollTop() >= 450) {
                    $('#return-to-top').fadeIn(200);
                } else {
                    $('#return-to-top').fadeOut(200);
                }
            });
            $('#return-to-top').click(() => { $('body,html').animate({ scrollTop: 0 }, 500); });
        });
    }

    init();

    export class RequestDataMartViewModel extends Dns.ViewModels.RequestDataMartViewModel {
        public Selected: KnockoutObservable<boolean>;

        constructor(requestDataMart: Dns.Interfaces.IRequestDataMartDTO) {
            super(requestDataMart);

            this.Selected = ko.observable(false);
            this.Priority = ko.observable(requestDataMart.Priority);
            this.DueDate = ko.observable(requestDataMart.DueDate);
        }
    }

    export interface IVisualWorkflowActivity {
        ID: any;
        WorkflowID: any;
        Name: string;
        Path: string
    }

    /** Shows a dialog that allows the user to enter a comment. The comment entered will be returned as the result. */
    export function PromptForComment(): JQueryPromise<any> {
        return Global.Helpers.ShowDialog('Enter a Comment', '/controls/wfcomments/simplecomment-dialog', ['Close'], 600, 320, null).promise();
    }

    export class VirtualRoutingViewModel {
        public ID: any;
        public RequestDataMartID: any;
        public RequestID: any;
        public DataMartID: any = null;
        public ResponseGroupID: any = null;        
        public Name: string = '';
        public IsGroup: boolean = false;
        public Status: Dns.Enums.RoutingStatus = Dns.Enums.RoutingStatus.AwaitingResponseApproval;
        public Messages: string = '';
        public Routings: Dns.Interfaces.IRequestDataMartDTO[];

        constructor(routing: Dns.Interfaces.IRequestDataMartDTO, group: Dns.Interfaces.IResponseGroupDTO) {

            this.Routings = [];
            this.Routings.push(routing);

            this.IsGroup = group != null;
            if (this.IsGroup) {
                this.ID = group.ID;
                this.Name = group.Name;
                this.ResponseGroupID = group.ID;
            } else {
                this.ID = routing.ResponseID;
                this.Name = routing.DataMart;
                this.DataMartID = routing.DataMartID;
            }
            this.RequestDataMartID = routing.ID;
            this.RequestID = routing.RequestID;
            
            
            this.Status = routing.Status;
            this.Messages = '';
            this.addToMessages(routing.ErrorMessage);
            this.addToMessages(routing.ResponseMessage);


        }

        public addRoutings(routings: Dns.Interfaces.IRequestDataMartDTO[]) { //
            if (routings == null || routings.length == 0)
                return;

            ko.utils.arrayFilter(routings, n => ko.utils.arrayFirst(this.Routings, r => r.ID == n.ID) == null).forEach(routing => {
                this.addToMessages(routing.ErrorMessage);
                this.addToMessages(routing.ResponseMessage);

                this.Routings.push(routing);
            });
        }

        private addToMessages(message: string) {
            if (message != null && message.trim().length > 0) {
                if (this.Messages != null && this.Messages.trim().length > 0)
                    this.Messages += '<br/>';
                this.Messages += message;
            }
        }
    }
} 