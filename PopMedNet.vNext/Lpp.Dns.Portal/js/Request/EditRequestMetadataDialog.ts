module Requests.Utility.EditRequestMetadataDialog {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        private Request: Dns.Interfaces.IRequestDTO;
        private Requestvm: Dns.ViewModels.RequestViewModel;

        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];

        public RequestName: KnockoutObservable<string>;
        public PurposeOfUse: KnockoutObservable<string>;
        public PurposeOfUseOptions: Array<any>;
        public PhiDisclosureLevel: KnockoutObservable<string>;
        public PhiDisclosureLevelOptions: Array<any>;
        public RequesterCenterID: KnockoutObservable<any>;
        public RequesterCenters: Array<any>;
        public RequesterCenter: KnockoutObservable<any>;
        public RequesterCentersList: Dns.Interfaces.IRequesterCenterDTO[];
        public ProjectID: KnockoutObservable<any>;
        public Description: KnockoutObservable<string>;

        public MSRequestID: KnockoutObservable<string>;
        public EditRequestIDAllowed: boolean;

        public DueDate: KnockoutObservable<Date>;//
        public Priority: KnockoutObservable<number>;//actually an enum
        public Priorities: Array<any>;

        public WorkplanTypeID: KnockoutObservable<any>;
        public WorkplanTypes: Array<any>;
        public WorkplanTypesList: Dns.Interfaces.IWorkplanTypeDTO[];

        public ReportAggregationLevelID: KnockoutObservable<any>;
        public ReportAggregationLevels: Array<any>;

        public ProjectActivityTree: Dns.Interfaces.IActivityDTO[];
        public isCheckedSource: KnockoutObservable<boolean>;

        public SourceTaskOrderID: KnockoutObservable<any>;
        public SourceActivityID: KnockoutObservable<any>;
        public SourceActivityProjectID: KnockoutObservable<any>;
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

        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;
         
        constructor(
            request: Dns.Interfaces.IRequestDTO,
            requesterCenterList: Dns.Interfaces.IRequesterCenterDTO[],
            workplanList: Dns.Interfaces.IWorkplanTypeDTO[],
            reportAggregationLevelsList: Dns.Interfaces.IReportAggregationLevelDTO[],
            activityTree: Dns.Interfaces.IActivityDTO[],
            fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
            allowEditRequestID: boolean,
            bindingControl: JQuery) {
            super(bindingControl);

            var self = this;

            this.FieldOptions = fieldOptions;

            this.Request = request;

            this.RequestName = ko.observable(self.Request.Name);

            //this.DueDate = ko.observable(moment(self.Request.DueDate).toDate());
            this.DueDate = ko.observable(self.Request.DueDate);//
            this.Priority = ko.observable(self.Request.Priority);
            this.Priorities = new Array({ Name: 'Low', Value: 0 }, { Name: 'Medium', Value: 1 }, { Name: 'High', Value: 2 }, { Name: 'Urgent', Value: 3 });

            this.ProjectID = ko.observable(self.Request.ProjectID);
            this.PurposeOfUse = ko.observable(self.Request.PurposeOfUse);
            this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });

            this.PhiDisclosureLevel = ko.observable(self.Request.PhiDisclosureLevel);
            this.PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });

            this.RequesterCenterID = ko.observable(self.Request.RequesterCenterID);
            this.RequesterCenters = requesterCenterList;
            this.RequesterCentersList = requesterCenterList;

            this.WorkplanTypeID = ko.observable(self.Request.WorkplanTypeID);
            this.WorkplanTypes = workplanList;
            this.WorkplanTypesList = workplanList;

            this.ReportAggregationLevelID = ko.observable(self.Request.ReportAggregationLevelID);
            this.ReportAggregationLevels = reportAggregationLevelsList.filter((ral) => ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)));

            this.MSRequestID = ko.observable(self.Request.MSRequestID);
            this.EditRequestIDAllowed = allowEditRequestID;

            this.Description = ko.observable(self.Request.Description);

            this.ProjectActivityTree = activityTree;

            this.SourceTaskOrderID = ko.observable<any>(self.Request.SourceTaskOrderID);
            this.SourceActivityID = ko.observable<any>(self.Request.SourceActivityID);
            this.SourceActivityProjectID = ko.observable<any>(self.Request.SourceActivityProjectID);
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

            if (this.Request.ActivityID) {
                //determine what the current budget activity is
                
                var currentBudgetActivity = this.findActivity(self.Request.ActivityID);
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

            this.RefreshActivitiesDataSources();

            this.isCheckedSource = ko.observable(self.BudgetTaskOrderID() == self.SourceTaskOrderID() && self.BudgetActivityID() == self.SourceActivityID() && self.BudgetActivityProjectID() == self.SourceActivityProjectID());

            var mirrorActivities = () => {
                self.BudgetTaskOrderID(self.SourceTaskOrderID());
                self.BudgetActivityID(self.SourceActivityID());
                self.BudgetActivityProjectID(self.SourceActivityProjectID());
            }

            var updatingFromSource = false;
            self.isCheckedSource.subscribe((value) => {
                if (value) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;
                }
            });

            self.SourceTaskOrderID.subscribe((newValue) => {
                if (self.isCheckedSource() == true) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;
                }
            });

            self.SourceActivityID.subscribe((newValue) => {
                if (self.isCheckedSource() == true) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;
                }
            });

            self.SourceActivityProjectID.subscribe((newValue) => {
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

            self.onSave = () => {
                
                if (self.RequestName() == (null || "")) {
                    Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>Request Name is required.</p></div>');
                    return;
                }

                //if (self.DueDate() != null) {
                //    self.DueDate().setHours(12);
                //}

                var update = <Dns.Interfaces.IRequestMetadataDTO>{
                    ID: self.Request.ID,
                    Timestamp: null,
                    Name: self.RequestName(),
                    Description: self.Description(),
                    DueDate: self.DueDate(),
                    Priority: self.Priority(),
                    PhiDisclosureLevel: self.PhiDisclosureLevel(),                    
                    PurposeOfUse: self.PurposeOfUse(),
                    RequesterCenterID: self.RequesterCenterID(),
                    WorkplanTypeID: self.WorkplanTypeID(),
                    SourceActivityID: self.SourceActivityID(),
                    SourceActivityProjectID: self.SourceActivityProjectID(),
                    SourceTaskOrderID: self.SourceTaskOrderID(),
                    TaskOrderID: self.BudgetTaskOrderID(),
                    ActivityID: self.BudgetActivityID(),
                    ActivityProjectID: self.BudgetActivityProjectID(),
                    MSRequestID: self.MSRequestID(),
                    ReportAggregationLevelID: self.ReportAggregationLevelID()
                };
                
                if (update.TaskOrderID == null || update.TaskOrderID == '') {
                    update.TaskOrderID = null;
                    update.ActivityID = null;
                }
                if (update.ActivityID == null) {
                    update.ActivityProjectID = null;
                }

                if (update.SourceTaskOrderID == null || update.SourceTaskOrderID == '') {
                    update.SourceTaskOrderID = null;
                    update.SourceActivityID = null;
                }
                if (update.SourceActivityID == null) {
                    update.SourceActivityProjectID = null;
                }
                
                var requestercentername = $('#RequesterCenterID').data("kendoDropDownList").text();
                var workplantypename = $('#WorkplanTypeID').data("kendoDropDownList").text();
                var reportAggregationLevelName = $('#ReportAggregationLevelID').data("kendoDropDownList").text();
                var priorityname = $('#RequestPriority').data("kendoDropDownList").text();
                var purposeofusename = $('#PurposeOfUse').data("kendoDropDownList").text();

                var taskordername = $('#BudgetTaskOrderID').data("kendoDropDownList").text();                
                var activityname = $('#BudgetActivityID').data("kendoDropDownList").text();
                var activityprojectname = $('#BudgetActivityProjectID').data("kendoDropDownList").text();                
                var sourcetaskordername = $('#SourceTaskOrderID').data("kendoDropDownList").text();
                var sourceactivityname = $('#SourceActivityID').data("kendoDropDownList").text();
                var sourceactivityprojectname = $('#SourceActivityProjectID').data("kendoDropDownList").text();
                  
               
                //push DTO to server
                Dns.WebApi.Requests.UpdateRequestMetadata(update).done(
                    () => {
                        //information to push back to dialog
                        self.Close({
                            MetadataUpdate: update,
                            priorityname: priorityname,
                            purposeofusename: purposeofusename,
                            requestercentername: requestercentername,
                            activityname: activityname,
                            taskordername: taskordername,
                            workplantypename: workplantypename,
                            activityprojectname: activityprojectname,
                            sourcetaskordername: sourcetaskordername,
                            sourceactivityname: sourceactivityname,
                            sourceactivityprojectname: sourceactivityprojectname,
                            reportAggregationLevelName: reportAggregationLevelName,
                            priority: self.Priority(),
                            dueDate: self.DueDate()
                        });
                    }
                    );
                
            };

            self.onCancel = () => {
                self.Close(null);
            };

            self.IsFieldRequired = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            self.IsFieldVisible = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
            };

            if (self.IsFieldRequired("Budget-Source-CheckBox")) {
                this.isCheckedSource(true);
            }
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
        var requestid = <string>parameters.Requestid || null;
        var allowEditRequestID = <boolean>parameters.allowEditRequestID || false;

        $.when<any>(
            Dns.WebApi.Requests.Get(requestid),
            Dns.WebApi.Requests.ListRequesterCenters(null, "ID,Name", "Name"),
            Dns.WebApi.Requests.ListWorkPlanTypes(null, "ID,Name", "Name"),
            Dns.WebApi.Requests.ListReportAggregationLevels(null, "ID,Name,DeletedOn", "Name")
            ).done((request: Dns.Interfaces.IRequestDTO[], requestCenterList, workplanList, reportAggregationLevelsList) => {

            var projectid = request[0].ProjectID;
            $.when<any>(
                Dns.WebApi.Projects.GetActivityTreeByProjectID(projectid)
                ).done(( activityTree: Dns.Interfaces.IActivityDTO[] ) => {
                bind(request[0], requestCenterList, workplanList, reportAggregationLevelsList, activityTree, allowEditRequestID);
                });

        });
    }

    function bind(
        request: Dns.Interfaces.IRequestDTO,
        requestCenterList,
        workplanList,
        reportAggregationLevelsList,
        activityTree: Dns.Interfaces.IActivityDTO[],
        allowEditRequestID: boolean
        ) {
        $.when<any>(
            Dns.WebApi.Projects.GetFieldOptions(request.ProjectID, User.ID)
            ).done((fieldOptions) => {
            var bindingControl = $('#EditRequestMetadataDialog');
            vm = new ViewModel(request, requestCenterList, workplanList, reportAggregationLevelsList, activityTree, fieldOptions, allowEditRequestID, bindingControl);
            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });
            });
    }
    init();
}
