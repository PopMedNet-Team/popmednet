/// <reference path="../../../Lpp.Pmn.Resources/Scripts/typings/bootstrap.dns.d.ts" />
/// <reference path="../../../Lpp.Pmn.Resources/Scripts/typings/knockout/knockout.d.ts" />

module Request.Edit {
    export var RawModel: any = null;
    export var GetActivitiesUrlTemplate: string = '';
    export var GetSubActivitiesUrlTemplate: string = '';

    var vmDataMarts: RequestDataMartsViewModel;
    var vmRequestDetails: RequestDetailsViewModel;

    export class Routings {
        //binding fields here
        public Priority: KnockoutObservable<Dns.Enums.Priorities>;
        public DueDate: KnockoutObservable<Date>;
        public Selected: boolean;
        public Name: string;
        public Organization: string;
        public OrganizationID: any;
        public ID: any;
        public RequestID: any;

        constructor(dataMart: Dns.ViewModels.DataMartListViewModel) {
            this.Priority = ko.observable(dataMart.Priority());
            this.DueDate = ko.observable(dataMart.DueDate());
            this.Name = dataMart.Name();
            this.Organization = dataMart.Organization();
            this.OrganizationID = dataMart.OrganizationID();
            this.ID = dataMart.ID();

        }
    }

    export class RequestDetailsViewModel extends Global.PageViewModel {

        public Name: KnockoutObservable<string>;
        public Priority: KnockoutObservable<number>;//actually an enum
        public Priorities: Array<any>;
        public DueDate: KnockoutObservable<Date>;
        public PurposeOfUse: KnockoutObservable<string>;
        public PurposeOfUseOptions: Array<any>;
        public PurposeOfUse_Display: KnockoutComputed<string>;
        public PhiDisclosureLevel: KnockoutObservable<string>;
        public PhiDisclosureLevelOptions: Array<any>;
        public RequesterCenterID: KnockoutObservable<any>;
        public RequesterCenters: Array<any>;
        public ProjectID: KnockoutObservable<any>;
        public ProjectName: KnockoutObservable<string>;
        public Description: KnockoutObservable<string>;
        public AdditionalInstructions: KnockoutObservable<string>;
        public isCheckedSource: KnockoutObservable<boolean>;
        public EnableBudgetMirroringCheckbox: KnockoutComputed<boolean>;
        public BudgetTaskOrderID: KnockoutObservable<any>;
        public BudgetActivityID: KnockoutObservable<any>;
        public BudgetActivityProjectID: KnockoutObservable<any>;
        public ReportAggregationLevelID: KnockoutObservable<any>;
        public ReportAggregationLevels: Array<any>;

        public MSRequestID: KnockoutObservable<string>;
        public EditRequestIDAllowed: boolean;

        public SelectedActivityID: KnockoutComputed<any>;

        public BudgetTaskOrder_DropDownArray: Array<any>;
        public BudgetActivity_DropDownArray: Array<any>;
        public BudgetActivityProject_DropDownArray: Array<any>;

        public dsTaskOrders: kendo.data.DataSource;
        public dsActivities: kendo.data.DataSource;
        public dsActivityProjects: kendo.data.DataSource;
        public ProjectActivityTree: Dns.Interfaces.IActivityDTO[];
        public AllActivitiesFlat: Dns.Interfaces.IActivityDTO[];
        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];

        public SourceActivityID: KnockoutObservable<any>;
        public SourceActivityProjectID: KnockoutObservable<any>;
        public SourceTaskOrderID: KnockoutObservable<any>;
        public dsSourceTaskOrders: kendo.data.DataSource;
        public dsSourceActivities: kendo.data.DataSource;
        public dsSourceActivityProjects: kendo.data.DataSource;

        private UpdateActivityDataSources: () => void;

        public UpdateBudgetTaskOrder_DisplayName: () => string;
        public UpdateBudgetActivity_DisplayName: () => string;
        public UpdateBudgetActivityProject_DisplayName: () => string;

        public WorkplanTypeID: KnockoutObservable<any>;
        public WorkplanTypes: Array<any>;

        constructor(rawModel: any, activityTree: Dns.Interfaces.IActivityDTO[], fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[], bindingControl: JQuery) {
            super(bindingControl);

            var self = this;

            this.FieldOptions = fieldOptions;

            this.Name = ko.observable(rawModel.Request.Name);
            this.Priority = ko.observable(rawModel.Request.Priority);
            this.DueDate = ko.observable(moment(rawModel.Request.DueDate).toDate());
            this.PurposeOfUse = ko.observable(rawModel.Header.PurposeOfUse);
            this.PhiDisclosureLevel = ko.observable(rawModel.Header.PhiDisclosureLevel);
            this.RequesterCenterID = ko.observable(rawModel.Header.RequesterCenterID);
            this.ProjectID = ko.observable(rawModel.Request.ProjectID);
            this.ProjectName = ko.observable(rawModel.Request.ProjectName);
            this.Description = ko.observable(rawModel.Header.Description);
            this.AdditionalInstructions = ko.observable(rawModel.Header.AdditionalInstructions);
            this.WorkplanTypeID = ko.observable(rawModel.Header.WorkplanTypeID);
            this.MSRequestID = ko.observable(rawModel.Request.MSRequestID);
            this.ReportAggregationLevelID = ko.observable(rawModel.Request.ReportAggregationLevelID);

            this.EditRequestIDAllowed = rawModel.AllowEditRequestID;

            this.ProjectActivityTree = activityTree;
            this.AllActivitiesFlat = rawModel.Activities;

            this.BudgetTaskOrderID = ko.observable<any>(null);
            this.BudgetActivityProjectID = ko.observable<any>(null);
            this.BudgetActivityID = ko.observable<any>(null);

            this.dsActivityProjects = new kendo.data.DataSource({
                data: []
            });
            this.dsActivities = new kendo.data.DataSource({
                data: []
            });
            this.dsTaskOrders = new kendo.data.DataSource({
                data: []
            });

            //Source Task/Activity/ActivityProject
            
            this.SourceActivityProjectID = ko.observable(rawModel.Request.SourceActivityProjectID);
            this.dsSourceActivityProjects = new kendo.data.DataSource({
                data: []
            });
            this.SourceActivityID = ko.observable(rawModel.Request.SourceActivityID);
            this.dsSourceActivities = new kendo.data.DataSource({
                data: []
            });
            this.SourceTaskOrderID = ko.observable(rawModel.Request.SourceTaskOrderID);
            this.dsSourceTaskOrders = new kendo.data.DataSource({
                data: []
            });

            this.isCheckedSource = ko.observable(rawModel.Header.MirrorBudgetFields);

            var mirrorActivities = () => {
                self.BudgetTaskOrderID(self.SourceTaskOrderID());
                self.BudgetActivityID(self.SourceActivityID());
                self.BudgetActivityProjectID(self.SourceActivityProjectID());
            }

            //set the initial values of the task order, activity, and activity project based on the activity ID saved to the request.
            var currentActivityID = rawModel.Request.ActivityID;
            if (currentActivityID) {
                var activity = ko.utils.arrayFirst(self.AllActivitiesFlat,(a: Dns.Interfaces.IActivityDTO) => a.ID.toLowerCase() == currentActivityID.toLowerCase());
                if (activity) {
                    if (activity.TaskLevel == 1) {
                        this.BudgetTaskOrderID(currentActivityID);
                    } else if (activity.TaskLevel == 2) {
                        this.BudgetTaskOrderID(activity.ParentActivityID);
                        this.BudgetActivityID(currentActivityID);
                    } else if (activity.TaskLevel == 3) {
                        this.BudgetActivityProjectID(currentActivityID);
                        this.BudgetActivityID(activity.ParentActivityID);
                        activity = ko.utils.arrayFirst(self.AllActivitiesFlat,(a: Dns.Interfaces.IActivityDTO) => a.ID.toLowerCase() == activity.ParentActivityID.toLowerCase() && a.TaskLevel == 2);
                        this.BudgetTaskOrderID(activity.ParentActivityID);
                    }
                }
            }

            this.SelectedActivityID = ko.computed(() => {
                //the selected activity will be the first one available in the following order: activity project, activity, task order
                //this is the value that actually gets posted back on save
                if (self.BudgetActivityProjectID() != null) {
                    return self.BudgetActivityProjectID();
                }

                if (self.BudgetActivityID() != null) {
                    return self.BudgetActivityID();
                }

                return self.BudgetTaskOrderID();
            });

            this.UpdateActivityDataSources = () => {
                self.dsTaskOrders.data(ko.utils.arrayFilter(self.ProjectActivityTree, (a: Dns.Interfaces.IActivityDTO) => a.Deleted == false));
                self.dsSourceTaskOrders.data(ko.utils.arrayFilter(self.ProjectActivityTree,(a: Dns.Interfaces.IActivityDTO) => a.Deleted == false));

                var activities = [];
                var activityProjects = [];

                self.BudgetTaskOrder_DropDownArray = ko.utils.arrayFilter(self.ProjectActivityTree,(a: Dns.Interfaces.IActivityDTO) => a.Deleted == false)

                self.ProjectActivityTree.forEach((to) => {
                    activities = activities.concat(ko.utils.arrayFilter(to.Activities, (a: Dns.Interfaces.IActivityDTO) => a.Deleted == false));

                    to.Activities.forEach((a) => {
                        activityProjects = activityProjects.concat(ko.utils.arrayFilter(a.Activities, (a: Dns.Interfaces.IActivityDTO) => a.Deleted == false));
                    });
                });

                self.dsActivities.data(activities);
                self.dsActivityProjects.data(activityProjects);

                var source_activities = [];
                var source_activityProjects = [];
                var source_activityID = null;
                var source_taskOrderID = null;
                this.ProjectActivityTree.forEach((tow) => {
                    source_activities = source_activities.concat(tow.Activities);
                    tow.Activities.forEach((b) => {
                        source_activityProjects = source_activityProjects.concat(b.Activities);
                    });
                });


                self.dsSourceActivities.data(source_activities);
                self.BudgetActivity_DropDownArray = source_activities;

                self.dsSourceActivityProjects.data(source_activityProjects);
                self.BudgetActivityProject_DropDownArray = source_activityProjects;

            };

            this.UpdateBudgetTaskOrder_DisplayName = () => {
                var taskorderSelection = ko.utils.arrayFirst(self.BudgetTaskOrder_DropDownArray,(a) => a.ID == self.BudgetTaskOrderID());
                if (taskorderSelection) {
                    return taskorderSelection.Name;
                } else {
                    return 'Not Selected'
                }
            };


            this.UpdateBudgetActivity_DisplayName = () => {
                var activitySelection = ko.utils.arrayFirst(self.BudgetActivity_DropDownArray,(a) => a.ID == self.BudgetActivityID());
                if (activitySelection) {
                    return activitySelection.Name;
                } else {
                    return 'Not Selected'
                }
            };

            this.UpdateBudgetActivityProject_DisplayName = () => {
                var activityProjectSelection = ko.utils.arrayFirst(self.BudgetActivityProject_DropDownArray,(a) => a.ID == self.BudgetActivityProjectID());
                if (activityProjectSelection) {
                    return activityProjectSelection.Name;
                } else {
                    return 'Not Selected'
                }
            };


            this.ProjectID.subscribe((value) => {
                if (!value) {
                    self.ProjectActivityTree = [];
                    self.UpdateActivityDataSources();
                } else {
                    Dns.WebApi.Projects.GetActivityTreeByProjectID(value).done((activities) => {
                        self.ProjectActivityTree = activities;
                        self.UpdateActivityDataSources();
                    });
                }
            });

            var updatingFromSource = false;
            self.isCheckedSource.subscribe((value) => {
                rawModel.Header.MirrorBudgetFields = value;
                if (value) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;

                }
            });

            self.SourceTaskOrderID.subscribe((newValue) => {

                self.SourceActivityProjectID(null);
                self.SourceActivityID(null);

                if (self.isCheckedSource() == true) {
                    updatingFromSource = true;
                    mirrorActivities();
                    updatingFromSource = false;
                }
            });

            self.SourceActivityID.subscribe((newValue) => {
                self.SourceActivityProjectID(null);

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
                } else if (self.isCheckedSource() == false && updatingFromSource != true) {
                    updatingFromSource = true;
                    self.BudgetActivityProjectID(null);
                    self.BudgetActivityID(null);
                    updatingFromSource = false;
                }
            });

            self.BudgetActivityID.subscribe((newValue) => {
                if (self.isCheckedSource() == true && updatingFromSource != true) {
                    mirrorActivities();
                } else if (self.isCheckedSource() == false && updatingFromSource != true) {
                    updatingFromSource = true;
                    self.BudgetActivityProjectID(null);
                    updatingFromSource = false;
                }
            });

            self.BudgetActivityProjectID.subscribe((newValue) => {
                if (self.isCheckedSource() == true && updatingFromSource != true) {
                    mirrorActivities();
                }
            });

            self.Priority.subscribe((newValue) => {

                vmDataMarts.DataMarts().forEach((dm) => {
                    dm.Priority(newValue);
                });    

            });
            self.DueDate.subscribe((newValue) => {

                vmDataMarts.DataMarts().forEach((dm) => {
                    dm.DueDate(newValue);
                });

            });

            self.EnableBudgetMirroringCheckbox = ko.computed(() => { return self.SourceTaskOrderID() != null && self.SourceTaskOrderID() != ''; });

            if (this.ProjectID()) {
                self.UpdateActivityDataSources();
            }

            this.Priorities = new Array({ Name: 'Low', Value: 0 }, { Name: 'Medium', Value: 1 }, { Name: 'High', Value: 2 }, { Name: 'Urgent', Value: 3 });
            this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
            this.PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });
            this.RequesterCenters = rawModel.RequesterCenters;
            this.WorkplanTypes = rawModel.WorkplanTypes;
            this.ReportAggregationLevels = rawModel.ReportAggregationLevels.filter((ral) => ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)));


            this.PurposeOfUse_Display = ko.computed(() => {
                if (self.PurposeOfUse() == null)
                    return '';
                var pou = ko.utils.arrayFirst(self.PurposeOfUseOptions,(a) => a.Value == self.PurposeOfUse());
                if (pou) {
                    return pou.Name;
                } else { return ''; }
            });

        }

        public IsFieldVisible(id) {
            var options = ko.utils.arrayFirst(vmRequestDetails.FieldOptions,(item) => { return item.FieldIdentifier == id; });
            return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
        }

        public IsFieldRequired(id) {
            var options = ko.utils.arrayFirst(vmRequestDetails.FieldOptions,(item) => { return item.FieldIdentifier == id; });
            return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
        }

        public onClearProject() {
            this.ProjectID(null);
            this.ProjectName('');
        }

        public onSelectProject() {
            Global.Helpers.ShowDialog("Select Project", "/request/selectproject", ["Close"], 550, 400, { Projects: Request.Edit.RawModel.Projects }).done((result) => {
                if (!result)
                    return;

                this.ProjectID(result.ID);
                this.ProjectName(result.Name);
                (<any>$("form")).formChanged(true);
            });
        }
    }

    export class RequestDataMartsViewModel {
        public Model: IRequestDataMartsEditModel;
        public DataMarts: KnockoutObservableArray<Request.Edit.Routings>;
        public SelectedDataMartIDs: KnockoutObservableArray<string>;
        public SerializedSelectedDataMarts: KnockoutObservable<string>;
        public SelectedRequestDataMarts: KnockoutObservableArray<Dns.Interfaces.IRequestDataMartDTO>;
        public RequestPriority: KnockoutObservable<Dns.Enums.Priorities>;
        public RequestDueDate: KnockoutObservable<Date>;
        public DataMartsBulkEdit: () => void;
        public SelectedRoutings: () => Dns.Interfaces.IRequestDataMartDTO[];

        constructor(model: IRequestDataMartsEditModel) {
            this.Model = model;
            var self = this;
            var selectedDataMarts = (model.SelectedDataMarts || '').split(',') || [];
            this.SelectedDataMartIDs = ko.observableArray(selectedDataMarts);
            this.SelectedRequestDataMarts = ko.observableArray(model.SelectedRequestDataMarts);
            this.RequestPriority = ko.observable(model.RequestPriority);
            this.RequestDueDate = ko.observable(model.RequestDueDate);

            this.SerializedSelectedDataMarts = ko.observable(JSON.stringify(model.SelectedRequestDataMarts));

            $("#frm").submit(() => {
                self.SelectedRequestDataMarts(self.SelectedRoutings());
                var dataMarts = JSON.stringify(self.SelectedRequestDataMarts());
                self.SerializedSelectedDataMarts(dataMarts);
            });

            this.DataMarts = ko.observableArray([]);
            model.DataMarts.forEach((d: Dns.Interfaces.IDataMartListDTO) => { 

                if (d.Organization == null || d.Organization.length == 0)
                    d.Organization = 'N/A';

                var dm = new Dns.ViewModels.DataMartListViewModel(d);
                var dataMart = new Request.Edit.Routings(dm);

                //if the dataMart is a selected datamart, set the saved due date and priority
                self.SelectedRequestDataMarts().forEach((sdm) => {
                    if (sdm.DataMartID == d.ID) {
                        dataMart.DueDate(moment(sdm.DueDate).toDate());
                        dataMart.Priority(sdm.Priority);
                    }
                });
                //if the dataMart has not been selected, set the default due date and Priority as the request's due date and Priority
                if (self.SelectedDataMartIDs.indexOf(d.ID) == -1) {
                    dataMart.DueDate(this.RequestDueDate());
                    dataMart.Priority(this.RequestPriority());
                }
                this.DataMarts.push(dataMart);
                this.DataMarts.sort(function (l, r) { return l.Name > r.Name ? 1 : -1 });
            });

            this.SelectedRoutings = () => {
                var dms: Dns.Interfaces.IRequestDataMartDTO[];
                dms = [];
                self.DataMarts().forEach((dm) => {
                    if (self.SelectedDataMartIDs().indexOf(dm.ID) != -1) {
                        var dataMart: Dns.Interfaces.IRequestDataMartDTO = (new Dns.ViewModels.RequestDataMartViewModel()).toData();
                        dataMart.DataMartID = dm.ID;                   
                        dataMart.Priority = dm.Priority();
                        dataMart.DueDate = dm.DueDate();
                        dms.push(dataMart);
                    }
                });

                return dms;
            };

            this.SelectedRequestDataMarts(self.SelectedRoutings());

            this.DataMartsBulkEdit = () => {
                Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: self.RequestPriority(), defaultDueDate: self.RequestDueDate() })
                    .done((result: any) => {
                        if (result != null) {
                            var newPriority: Dns.Enums.Priorities;
                            if (result.UpdatePriority) {
                                newPriority = result.PriorityValue;
                            };

                            var newDueDate: Date = new Date(result.stringDate);
                            if (!result.UpdateDueDate) {
                                newDueDate = null;
                            }

                            // update selected datamarts here
                            self.DataMarts().forEach((dm) => {
                                if (self.SelectedDataMartIDs().indexOf(dm.ID) != -1) {
                                    if (result.UpdatePriority)
                                    {
                                        dm.Priority(newPriority);
                                    }
                                    if (result.UpdateDueDate)
                                    {
                                        dm.DueDate(newDueDate);
                                    }
                                }
                            });
                        }

                        (<any>$("form")).formChanged(true); 
                    });

            }

        }

        public viewSelectedDataMarts() {
            alert(this.SelectedDataMartIDs().join('\n'));
        }

        public DataMartsSelectAll() {
            vmDataMarts.DataMarts().forEach((dm) => {
                if (vmDataMarts.SelectedDataMartIDs.indexOf(dm.ID) < 0)
                    vmDataMarts.SelectedDataMartIDs.push(dm.ID);
            });

            return false;
        }

        public DataMartsClearAll() {
            vmDataMarts.SelectedDataMartIDs.removeAll();

            return false;
        }

    }


    export function init() {
        $.when<any>(
            Dns.WebApi.Projects.GetActivityTreeByProjectID(RawModel.Request.ProjectID), 
            Dns.WebApi.Projects.GetFieldOptions(RawModel.Request.ProjectID, User.ID)
            ).done((activityTree: Dns.Interfaces.IActivityDTO[], fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[]) => {        

            var requestDetailsBindingContainer = $('#frm');
            vmRequestDetails = new RequestDetailsViewModel(RawModel, activityTree, fieldOptions, requestDetailsBindingContainer);

            vmDataMarts = new RequestDataMartsViewModel({
                DataMarts: RawModel.DataMarts,
                AllAuthorizedDataMarts: RawModel.AllAuthorizedDataMarts,
                SelectedDataMarts: RawModel.SelectedDataMarts,
                SelectedRequestDataMarts: RawModel.SelectedRequestDataMarts,
                RequestPriority: RawModel.Request.Priority,
                RequestDueDate: moment(RawModel.Request.DueDate).toDate()
            });

            $(() => {
                ko.applyBindings(vmRequestDetails, requestDetailsBindingContainer[0]);
                ko.applyBindings(vmDataMarts, $('#RequestDataMarts')[0]);
                $('.dataMartMetadata').change(() => { (<any>$("form")).formChanged(true); });
            });

        });

    }

    export interface IRequestDataMartsEditModel {
        DataMarts: Dns.Interfaces.IDataMartListDTO[];
        AllAuthorizedDataMarts: Dns.Interfaces.IDataMartListDTO[];
        SelectedDataMarts: string;
        SelectedRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[];
        RequestPriority: Dns.Enums.Priorities;
        RequestDueDate: Date;
    } 

}