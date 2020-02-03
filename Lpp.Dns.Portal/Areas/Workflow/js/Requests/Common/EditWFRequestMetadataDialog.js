var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var EditWFRequestMetadataDialog;
        (function (EditWFRequestMetadataDialog) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, detailsViewModel, fieldOptions, allowEditRequestID, isNewRequest, oldRequestDueDate, oldRequestPriority) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.Saving = ko.observable(false);
                    var self = _this;
                    _this.FieldOptions = fieldOptions;
                    _this.DetailsViewModel = detailsViewModel;
                    _this.Request = new Dns.ViewModels.RequestViewModel(detailsViewModel.Request.toData());
                    _this.EditRequestIDAllowed = allowEditRequestID;
                    //Lists
                    _this.RequesterCenterList = ko.utils.arrayMap(detailsViewModel.RequesterCenterList, function (l) { return ({ ID: l.ID, Name: l.Name, RequesterCenterID: l.RequesterCenterID, NetworkID: l.NetworkID }); });
                    _this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
                    _this.PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });
                    _this.WorkplanTypesList = ko.utils.arrayMap(detailsViewModel.WorkPlanTypeList, function (l) { return ({ ID: l.ID, Acronym: l.Acronym, Name: l.Name, NetworkID: l.NetworkID, WorkplanTypeID: l.WorkplanTypeID }); });
                    var reportAggregationLevels = ko.utils.arrayMap(detailsViewModel.ReportAggregationLevelList, function (l) { return ({ ID: l.ID, Name: l.Name, NetworkID: l.NetworkID, DeletedOn: l.DeletedOn }); });
                    //remove RALs that have been deleted
                    _this.ReportAggregationLevelsList = reportAggregationLevels.filter(function (ral) { return ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)); });
                    _this.ProjectActivityTree = detailsViewModel.ProjectActivityTree;
                    _this.BudgetTaskOrderID = ko.observable(null);
                    _this.BudgetActivityProjectID = ko.observable(null);
                    _this.BudgetActivityID = ko.observable(null);
                    _this.findActivity = function (id) {
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
                    if (_this.Request.ActivityID()) {
                        //determine what the current budget activity is
                        var currentBudgetActivity = _this.findActivity(self.Request.ActivityID());
                        if (currentBudgetActivity != null) {
                            if (currentBudgetActivity.TaskLevel == 1) {
                                //task order
                                self.BudgetTaskOrderID(currentBudgetActivity.ID);
                            }
                            else if (currentBudgetActivity.TaskLevel == 2) {
                                //activity
                                self.BudgetTaskOrderID(currentBudgetActivity.ParentActivityID);
                                self.BudgetActivityID(currentBudgetActivity.ID);
                            }
                            else {
                                //activity project
                                //find the activity (parent) of the activity project to get the task order id
                                var activity = _this.findActivity(currentBudgetActivity.ParentActivityID);
                                self.BudgetTaskOrderID(activity.ParentActivityID);
                                self.BudgetActivityID(currentBudgetActivity.ParentActivityID);
                                self.BudgetActivityProjectID(currentBudgetActivity.ID);
                            }
                        }
                    }
                    //Task/Activity/Activity Project
                    _this.dsTaskOrders = new kendo.data.DataSource({
                        data: []
                    });
                    _this.dsActivities = new kendo.data.DataSource({
                        data: []
                    });
                    _this.dsActivityProjects = new kendo.data.DataSource({
                        data: []
                    });
                    _this.dsSourceActivities = new kendo.data.DataSource({
                        data: []
                    });
                    _this.dsSourceActivityProjects = new kendo.data.DataSource({
                        data: []
                    });
                    //When the task order or activity changes need to reset child values
                    _this.Request.SourceTaskOrderID.subscribe(function (value) {
                        self.Request.SourceActivityID(null);
                        self.Request.SourceActivityProjectID(null);
                    });
                    _this.Request.SourceActivityID.subscribe(function (value) {
                        self.Request.SourceActivityProjectID(null);
                    });
                    _this.BudgetTaskOrderID.subscribe(function (value) {
                        self.BudgetActivityID(null);
                        self.BudgetActivityProjectID(null);
                    });
                    _this.BudgetActivityID.subscribe(function (value) {
                        self.BudgetActivityProjectID(null);
                    });
                    _this.RefreshActivitiesDataSources();
                    _this.isCheckedSource = ko.observable(_this.Request.MirrorBudgetFields());
                    var mirrorActivities = function () {
                        self.BudgetTaskOrderID(self.Request.SourceTaskOrderID());
                        self.BudgetActivityID(self.Request.SourceActivityID());
                        self.BudgetActivityProjectID(self.Request.SourceActivityProjectID());
                    };
                    var updatingFromSource = false;
                    self.isCheckedSource.subscribe(function (value) {
                        if (value) {
                            updatingFromSource = true;
                            mirrorActivities();
                            updatingFromSource = false;
                        }
                    });
                    self.Request.SourceTaskOrderID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true) {
                            updatingFromSource = true;
                            mirrorActivities();
                            updatingFromSource = false;
                        }
                    });
                    self.Request.SourceActivityID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true) {
                            updatingFromSource = true;
                            mirrorActivities();
                            updatingFromSource = false;
                        }
                    });
                    self.Request.SourceActivityProjectID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true) {
                            updatingFromSource = true;
                            mirrorActivities();
                            updatingFromSource = false;
                        }
                    });
                    self.BudgetTaskOrderID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true && updatingFromSource != true) {
                            mirrorActivities();
                        }
                    });
                    self.BudgetActivityID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true && updatingFromSource != true) {
                            mirrorActivities();
                        }
                    });
                    self.BudgetActivityProjectID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true && updatingFromSource != true) {
                            mirrorActivities();
                        }
                    });
                    //Edit this one
                    //Save and Cancel
                    self.onSave = function () {
                        if (!self.Validate()) {
                            Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>Please check for Required Fields and Save again.</p></div>', 500).done(function () { return; });
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
                        }
                        else if ((self.BudgetActivityID() != null) && (self.BudgetActivityID() != '')) {
                            self.DetailsViewModel.Request.ActivityID(self.BudgetActivityID());
                        }
                        else if ((self.BudgetTaskOrderID() != null) && (self.BudgetTaskOrderID() != '')) {
                            self.DetailsViewModel.Request.ActivityID(self.BudgetTaskOrderID());
                        }
                        else {
                            self.DetailsViewModel.Request.ActivityID(null);
                        }
                        //If the Request Priority or Due Date has changed update the RequestDataMarts Priority and Due Date on the Details View Model
                        var changedPriority = null;
                        if (self.Request.Priority() != oldRequestPriority) {
                            self.DetailsViewModel.RequestDataMarts().forEach(function (dm) {
                                dm.Priority(self.Request.Priority());
                            });
                            changedPriority = self.Request.Priority();
                        }
                        var changedDueDate = null;
                        if (self.Request.DueDate() != oldRequestDueDate) {
                            self.DetailsViewModel.RequestDataMarts().forEach(function (dm) {
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
                        var errorHandler = function (err) {
                            //disable the save and cancel buttons
                            self.Saving(false);
                            //show progress over the dialog to prevent any more button pushes and indicate that it is trying to do something
                            kendo.ui.progress($('#Content'), false);
                        };
                        detailsViewModel.DefaultSave(false, isNewRequest, errorHandler).done(function () {
                            if (!isNewRequest) {
                                if (refreshAssignees) {
                                    self.DetailsViewModel.onRefreshRequestUsers();
                                }
                            }
                            self.DetailsViewModel.RefreshBudgetActivities();
                            self.Close(true);
                        });
                    };
                    self.onCancel = function () {
                        if (isNewRequest) {
                            Global.Helpers.ShowConfirm("Cancel Confirmation", "<p>You are about to Cancel the Request without saving. Are you sure you wish to continue?</p>").done(function () {
                                window.history.back();
                            });
                        }
                        else {
                            self.Close(null);
                        }
                    };
                    self.IsFieldRequired = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                        return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
                    };
                    if (self.IsFieldRequired("Budget-Source-CheckBox")) {
                        _this.isCheckedSource(true);
                    }
                    self.IsFieldVisible = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                        return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
                    };
                    return _this;
                }
                ViewModel.prototype.RefreshActivitiesDataSources = function () {
                    this.dsTaskOrders.data(this.ProjectActivityTree);
                    var activities = [];
                    var activityProjects = [];
                    this.ProjectActivityTree.forEach(function (to) {
                        activities = activities.concat(to.Activities);
                        to.Activities.forEach(function (a) {
                            activityProjects = activityProjects.concat(a.Activities);
                        });
                    });
                    this.dsActivities.data(activities);
                    this.dsActivityProjects.data(activityProjects);
                    this.dsSourceActivities.data(activities);
                    this.dsSourceActivityProjects.data(activityProjects);
                };
                return ViewModel;
            }(Global.DialogViewModel));
            EditWFRequestMetadataDialog.ViewModel = ViewModel;
            function init() {
                var window = Global.Helpers.GetDialogWindow();
                var parameters = (window.options).parameters;
                var detailsViewModel = (parameters.DetailsViewModel || null);
                var allowEditRequestID = (parameters.AllowEditRequestID || false);
                var isNewRequest = (parameters.NewRequest);
                var oldRequestPriority = (parameters.OldRequestPriority);
                var oldRequestDueDate = (parameters.OldRequestDueDate);
                $.when(Dns.WebApi.Projects.GetFieldOptions(detailsViewModel.Request.ProjectID(), User.ID)).done(function (fieldOptions) {
                    var bindingControl = $('#EditWFRequestMetadataDialog');
                    vm = new ViewModel(bindingControl, detailsViewModel, fieldOptions, allowEditRequestID, isNewRequest, oldRequestDueDate, oldRequestPriority);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            EditWFRequestMetadataDialog.init = init;
            init();
        })(EditWFRequestMetadataDialog = Common.EditWFRequestMetadataDialog || (Common.EditWFRequestMetadataDialog = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=EditWFRequestMetadataDialog.js.map