/// <reference path="../../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.Common.EditWFRequestMetadataDialog {
    var vm: ViewModel;
    export class ViewModel extends Global.DialogViewModel {
        public DetailsViewModel: Requests.Details.RequestOverviewViewModel;
        public Request: Dns.ViewModels.RequestViewModel;

        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];

        public EditRequestIDAllowed: boolean;

        public RequesterCenterList: Dns.Interfaces.IRequesterCenterDTO[];
        public PurposeOfUseOptions: Array<any>;
        public PhiDisclosureLevelOptions: Array<any>;
        public WorkplanTypesList: Dns.Interfaces.IWorkplanTypeDTO[];
        public ReportAggregationLevelsList: Dns.Interfaces.IReportAggregationLevelDTO[];

        public ProjectActivityTree: Dns.Interfaces.IActivityDTO[];

        public isCheckedSource: KnockoutObservable<boolean>;

        public BudgetTaskOrderID: KnockoutObservable<any>;
        public BudgetActivityID: KnockoutObservable<any>;
        public BudgetActivityProjectID: KnockoutObservable<any>;

        public dsTaskOrders: kendo.data.DataSource;
        public dsActivities: kendo.data.DataSource;
        public dsActivityProjects: kendo.data.DataSource;
        public dsSourceActivities: kendo.data.DataSource;
        public dsSourceActivityProjects: kendo.data.DataSource;

        private findActivity: (id: any) => Dns.Interfaces.IActivityDTO;

        private onSave: () => void;
        private onCancel: () => void;
        private Saving: KnockoutObservable<boolean> = ko.observable(false);

        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;

        constructor(
            bindingControl: JQuery,
            detailsViewModel: Requests.Details.RequestOverviewViewModel,
            fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
            allowEditRequestID: boolean,
            isNewRequest: boolean,
            oldRequestDueDate: Date,
            oldRequestPriority: Dns.Enums.Priorities
            )
        {
            super(bindingControl);
            var self = this;

            this.FieldOptions = fieldOptions;
            
            this.DetailsViewModel = detailsViewModel;

            this.Request = new Dns.ViewModels.RequestViewModel(detailsViewModel.Request.toData());
            
            this.EditRequestIDAllowed = allowEditRequestID;
            
            //Lists
            this.RequesterCenterList = ko.utils.arrayMap(detailsViewModel.RequesterCenterList,(l) => <Dns.Interfaces.IRequesterCenterDTO>{ ID: l.ID, Name: l.Name, RequesterCenterID: l.RequesterCenterID, NetworkID: l.NetworkID });
            this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
            this.PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });
            this.WorkplanTypesList = ko.utils.arrayMap(detailsViewModel.WorkPlanTypeList,(l) => <Dns.Interfaces.IWorkplanTypeDTO>{ID: l.ID, Acronym: l.Acronym, Name: l.Name, NetworkID: l.NetworkID, WorkplanTypeID: l.WorkplanTypeID });

            var reportAggregationLevels = ko.utils.arrayMap(detailsViewModel.ReportAggregationLevelList, (l) => <Dns.Interfaces.IReportAggregationLevelDTO>{ ID: l.ID, Name: l.Name, NetworkID: l.NetworkID, DeletedOn: l.DeletedOn });
            //remove RALs that have been deleted
            this.ReportAggregationLevelsList = reportAggregationLevels.filter((ral) => ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)));

            this.ProjectActivityTree = detailsViewModel.ProjectActivityTree;            

            this.BudgetTaskOrderID = ko.observable<any>(null);
            this.BudgetActivityProjectID = ko.observable<any>(null);
            this.BudgetActivityID = ko.observable<any>(null);

            this.findActivity = (id: any) => {
                if (id == null)
                    return null;

                for (var i = 0; i < self.ProjectActivityTree.length; i++) {
                    var act: Dns.Interfaces.IActivityDTO = self.ProjectActivityTree[i];
                    if (act.ID == id) {
                        return act;
                    }

                    for (var j = 0; j < act.Activities.length; j++) {
                        var act2: Dns.Interfaces.IActivityDTO = act.Activities[j];
                        if (act2.ID == id) {
                            return act2;
                        }

                        for (var k = 0; k < act2.Activities.length; k++) {
                            var act3: Dns.Interfaces.IActivityDTO = act2.Activities[k];
                            if (act3.ID == id) {
                                return act3;
                            }
                        }
                    }
                }

                return null;
            };
            
            if (this.Request.ActivityID()) {
                //determine what the current budget activity is
                
                var currentBudgetActivity = this.findActivity(self.Request.ActivityID());
                if (currentBudgetActivity != null) {
                    if (currentBudgetActivity.TaskLevel == 1) {
                        //task order
                        self.BudgetTaskOrderID(currentBudgetActivity.ID);
                    } else if (currentBudgetActivity.TaskLevel == 2) {
                        //activity
                        self.BudgetTaskOrderID(currentBudgetActivity.ParentActivityID);
                        self.BudgetActivityID(currentBudgetActivity.ID);
                    } else {
                        //activity project

                        //find the activity (parent) of the activity project to get the task order id
                        var activity = this.findActivity(currentBudgetActivity.ParentActivityID);
                        self.BudgetTaskOrderID(activity.ParentActivityID);
                        self.BudgetActivityID(currentBudgetActivity.ParentActivityID);
                        self.BudgetActivityProjectID(currentBudgetActivity.ID);                        
                    }
                }
            }            

            //Task/Activity/Activity Project
            this.dsTaskOrders = new kendo.data.DataSource({
                data: []
            });
            this.dsActivities = new kendo.data.DataSource({
                data: []
            });
            this.dsActivityProjects = new kendo.data.DataSource({
                data: []
            });


            this.dsSourceActivities = new kendo.data.DataSource({
                data: []
            });           
            this.dsSourceActivityProjects = new kendo.data.DataSource({
                data: []
            }); 
            
            //When the task order or activity changes need to reset child values
            this.Request.SourceTaskOrderID.subscribe((value) => {
                self.Request.SourceActivityID(null);
                self.Request.SourceActivityProjectID(null);
            });

            this.Request.SourceActivityID.subscribe((value) => {
                self.Request.SourceActivityProjectID(null);
            });

            this.BudgetTaskOrderID.subscribe((value) => {
                self.BudgetActivityID(null);
                self.BudgetActivityProjectID(null);
            });

            this.BudgetActivityID.subscribe((value) => {
                self.BudgetActivityProjectID(null);
            });

            this.RefreshActivitiesDataSources();
            
            this.isCheckedSource = ko.observable(this.Request.MirrorBudgetFields());

            var mirrorActivities = () => {
                self.BudgetTaskOrderID(self.Request.SourceTaskOrderID());
                self.BudgetActivityID(self.Request.SourceActivityID());
                self.BudgetActivityProjectID(self.Request.SourceActivityProjectID());  
            }

            var updatingFromSource = false;
            self.isCheckedSource.subscribe((value) => {
                if (value) {
                    updatingFromSource = true;
                    mirrorActivities();   
                    updatingFromSource = false;           
                }
            });

            self.Request.SourceTaskOrderID.subscribe((newValue) => {
                if (self.isCheckedSource() == true) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;
                }
            });

            self.Request.SourceActivityID.subscribe((newValue) => {
                if (self.isCheckedSource() == true) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;
                }
            });

            self.Request.SourceActivityProjectID.subscribe((newValue) => {
                if (self.isCheckedSource() == true) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;
                }
            });

            self.BudgetTaskOrderID.subscribe((newValue) => {
                if (self.isCheckedSource() == true && updatingFromSource != true) {
                    mirrorActivities();
                }
            });

            self.BudgetActivityID.subscribe((newValue) => {
                if (self.isCheckedSource() == true && updatingFromSource != true) {
                    mirrorActivities();
                }
            });

            self.BudgetActivityProjectID.subscribe((newValue) => {
                if (self.isCheckedSource() == true && updatingFromSource != true) {
                    mirrorActivities();
                }
            });
                       
            //Edit this one
            //Save and Cancel
            self.onSave = () => {
                if (!self.Validate()) {
                    Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>Please check for Required Fields and Save again.</p></div>', 500).done(() => { return;});
                    return;
                }

                //disable the save and cancel buttons
                self.Saving(true);
                //show progress over the dialog to prevent any more button pushes and indicate that it is trying to do something
                kendo.ui.progress($('#Content'), true);                

                if (self.Request.SourceTaskOrderID() == null || self.Request.SourceTaskOrderID() == '') {
                    self.Request.SourceTaskOrderID(null);
                    self.Request.SourceActivityID(null);
                }
                if (self.Request.SourceActivityID() == null) {
                    self.Request.SourceActivityProjectID(null);
                }

                if (self.BudgetTaskOrderID() == null || self.BudgetTaskOrderID() == '') {
                    self.BudgetTaskOrderID(null);
                    self.BudgetActivityID(null);

                }
                if (self.BudgetActivityID() == null) {
                    self.BudgetActivityProjectID(null);
                }

                self.DetailsViewModel.Request.Name(self.Request.Name());
                self.DetailsViewModel.Request.MSRequestID(self.Request.MSRequestID());
                self.DetailsViewModel.Request.Priority(self.Request.Priority());
                self.DetailsViewModel.Request.DueDate(self.Request.DueDate());
                self.DetailsViewModel.Request.RequesterCenterID(self.Request.RequesterCenterID());
                self.DetailsViewModel.Request.PurposeOfUse(self.Request.PurposeOfUse());
                self.DetailsViewModel.Request.PhiDisclosureLevel(self.Request.PhiDisclosureLevel());
                self.DetailsViewModel.Request.WorkplanTypeID(self.Request.WorkplanTypeID());
                self.DetailsViewModel.Request.Description(self.Request.Description());  
                self.DetailsViewModel.Request.ReportAggregationLevelID(self.Request.ReportAggregationLevelID());              

                self.DetailsViewModel.Request.SourceActivityID(self.Request.SourceActivityID());
                self.DetailsViewModel.Request.SourceTaskOrderID(self.Request.SourceTaskOrderID());
                self.DetailsViewModel.Request.SourceActivityProjectID(self.Request.SourceActivityProjectID());
                self.DetailsViewModel.Request.MirrorBudgetFields(self.isCheckedSource());

                self.DetailsViewModel.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

                if ((self.BudgetActivityProjectID() != null) && (self.BudgetActivityProjectID() != '')) {
                    self.DetailsViewModel.Request.ActivityID(self.BudgetActivityProjectID());
                } else if ((self.BudgetActivityID() != null) && (self.BudgetActivityID() != '')) {
                    self.DetailsViewModel.Request.ActivityID(self.BudgetActivityID());
                } else if ((self.BudgetTaskOrderID() != null) && (self.BudgetTaskOrderID() != '')) {
                    self.DetailsViewModel.Request.ActivityID(self.BudgetTaskOrderID());
                } else {
                    self.DetailsViewModel.Request.ActivityID(null);
                }
                
                               
                //If the Request Priority or Due Date has changed update the RequestDataMarts Priority and Due Date on the Details View Model
                var changedPriority = null;
                if (self.Request.Priority() != oldRequestPriority) {
                    self.DetailsViewModel.RequestDataMarts().forEach((dm) => {
                        dm.Priority(self.Request.Priority());
                    });

                    changedPriority = self.Request.Priority();
                }
                var changedDueDate = null;
                if (self.Request.DueDate() != oldRequestDueDate) {
                    self.DetailsViewModel.RequestDataMarts().forEach((dm) => {
                        dm.DueDate(self.Request.DueDate());
                    });

                    changedDueDate = self.Request.DueDate();
                }
                //call notify metadata function here to fire subscription notification for request datamarts if the Request Priority or Due Date have changed
                //if the priority or due date have not changed, then the corresponding variables passed through will be null
                if ((self.Request.DueDate() != oldRequestDueDate) || (self.Request.Priority() != oldRequestPriority)) {
                    self.DetailsViewModel.NotifyMetadataChanged({ newPriority: changedPriority, newDueDate: changedDueDate });
                }

                var refreshAssignees = self.DetailsViewModel.Request.ID() == null;

                var errorHandler = (err: any) => {
                    //disable the save and cancel buttons
                    self.Saving(false);
                    //show progress over the dialog to prevent any more button pushes and indicate that it is trying to do something
                    kendo.ui.progress($('#Content'), false);  
                };

                detailsViewModel.DefaultSave(false, isNewRequest, errorHandler).done(
                    () => {
                        if (!isNewRequest) {
                            if (refreshAssignees) {
                                self.DetailsViewModel.onRefreshRequestUsers();
                            }
                        } 

                        self.DetailsViewModel.RefreshBudgetActivities();                       
                        self.Close(true);
                    });
                
            };

            self.onCancel = () => {

                if (isNewRequest) {
                    Global.Helpers.ShowConfirm("Cancel Confirmation", "<p>You are about to Cancel the Request without saving. Are you sure you wish to continue?</p>").done(() => {
                        window.history.back();
                    });
                }
                else {
                    self.Close(null);
                }
            };

            self.IsFieldRequired = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            if (self.IsFieldRequired("Budget-Source-CheckBox")) {
                this.isCheckedSource(true);
            }

            self.IsFieldVisible = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
            };
        } 

        private RefreshActivitiesDataSources() {
            this.dsTaskOrders.data(this.ProjectActivityTree);
            
            var activities = [];
            var activityProjects = [];

            this.ProjectActivityTree.forEach((to) => {

                activities = activities.concat(to.Activities);

                to.Activities.forEach((a) => {
                    activityProjects = activityProjects.concat(a.Activities);
                });
            });

            this.dsActivities.data(activities);
            this.dsActivityProjects.data(activityProjects);

            this.dsSourceActivities.data(activities);
            this.dsSourceActivityProjects.data(activityProjects);
           
        }        
    }

    export function init() {
       
        var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        var parameters = (<any>(window.options)).parameters;        
        var detailsViewModel = <Requests.Details.RequestOverviewViewModel>(parameters.DetailsViewModel || null);
        
        var allowEditRequestID = <boolean>(parameters.AllowEditRequestID || false);
        var isNewRequest = <boolean>(parameters.NewRequest);
        var oldRequestPriority = <Dns.Enums.Priorities>(parameters.OldRequestPriority);
        var oldRequestDueDate = <Date>(parameters.OldRequestDueDate);
        $.when<any>(
            Dns.WebApi.Projects.GetFieldOptions(detailsViewModel.Request.ProjectID(), User.ID)
            ).done((fieldOptions) => {
            
                var bindingControl = $('#EditWFRequestMetadataDialog');
                vm = new ViewModel(bindingControl, detailsViewModel, fieldOptions, allowEditRequestID, isNewRequest, oldRequestDueDate, oldRequestPriority);
                
                $(() => {
                    ko.applyBindings(vm, bindingControl[0]);
                });
                
            });        
    }

    init();
}