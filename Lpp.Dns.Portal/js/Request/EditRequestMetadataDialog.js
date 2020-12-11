var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Requests;
(function (Requests) {
    var Utility;
    (function (Utility) {
        var EditRequestMetadataDialog;
        (function (EditRequestMetadataDialog) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(request, requesterCenterList, workplanList, reportAggregationLevelsList, activityTree, fieldOptions, allowEditRequestID, bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    var self = _this;
                    _this.FieldOptions = fieldOptions;
                    _this.Request = request;
                    _this.RequestName = ko.observable(self.Request.Name);
                    //this.DueDate = ko.observable(moment(self.Request.DueDate).toDate());
                    _this.DueDate = ko.observable(self.Request.DueDate); //
                    _this.Priority = ko.observable(self.Request.Priority);
                    _this.Priorities = new Array({ Name: 'Low', Value: 0 }, { Name: 'Medium', Value: 1 }, { Name: 'High', Value: 2 }, { Name: 'Urgent', Value: 3 });
                    _this.ProjectID = ko.observable(self.Request.ProjectID);
                    _this.PurposeOfUse = ko.observable(self.Request.PurposeOfUse);
                    _this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
                    _this.PhiDisclosureLevel = ko.observable(self.Request.PhiDisclosureLevel);
                    _this.PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });
                    _this.RequesterCenterID = ko.observable(self.Request.RequesterCenterID);
                    _this.RequesterCenters = requesterCenterList;
                    _this.RequesterCentersList = requesterCenterList;
                    _this.WorkplanTypeID = ko.observable(self.Request.WorkplanTypeID);
                    _this.WorkplanTypes = workplanList;
                    _this.WorkplanTypesList = workplanList;
                    _this.ReportAggregationLevelID = ko.observable(self.Request.ReportAggregationLevelID);
                    _this.ReportAggregationLevels = reportAggregationLevelsList.filter(function (ral) { return ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)); });
                    _this.MSRequestID = ko.observable(self.Request.MSRequestID);
                    _this.EditRequestIDAllowed = allowEditRequestID;
                    _this.Description = ko.observable(self.Request.Description);
                    _this.ProjectActivityTree = activityTree;
                    _this.SourceTaskOrderID = ko.observable(self.Request.SourceTaskOrderID);
                    _this.SourceActivityID = ko.observable(self.Request.SourceActivityID);
                    _this.SourceActivityProjectID = ko.observable(self.Request.SourceActivityProjectID);
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
                    if (_this.Request.ActivityID) {
                        //determine what the current budget activity is
                        var currentBudgetActivity = _this.findActivity(self.Request.ActivityID);
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
                    _this.RefreshActivitiesDataSources();
                    _this.isCheckedSource = ko.observable(self.BudgetTaskOrderID() == self.SourceTaskOrderID() && self.BudgetActivityID() == self.SourceActivityID() && self.BudgetActivityProjectID() == self.SourceActivityProjectID());
                    var mirrorActivities = function () {
                        self.BudgetTaskOrderID(self.SourceTaskOrderID());
                        self.BudgetActivityID(self.SourceActivityID());
                        self.BudgetActivityProjectID(self.SourceActivityProjectID());
                    };
                    var updatingFromSource = false;
                    self.isCheckedSource.subscribe(function (value) {
                        if (value) {
                            updatingFromSource = true;
                            mirrorActivities();
                            updatingFromSource = false;
                        }
                    });
                    self.SourceTaskOrderID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true) {
                            updatingFromSource = true;
                            mirrorActivities();
                            updatingFromSource = false;
                        }
                    });
                    self.SourceActivityID.subscribe(function (newValue) {
                        if (self.isCheckedSource() == true) {
                            updatingFromSource = true;
                            mirrorActivities();
                            updatingFromSource = false;
                        }
                    });
                    self.SourceActivityProjectID.subscribe(function (newValue) {
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
                    self.onSave = function () {
                        if (self.RequestName() == (null || "")) {
                            Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>Request Name is required.</p></div>');
                            return;
                        }
                        //if (self.DueDate() != null) {
                        //    self.DueDate().setHours(12);
                        //}
                        var update = {
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
                        Dns.WebApi.Requests.UpdateRequestMetadata(update).done(function () {
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
                        });
                    };
                    self.onCancel = function () {
                        self.Close(null);
                    };
                    self.IsFieldRequired = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                        return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
                    };
                    self.IsFieldVisible = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                        return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
                    };
                    if (self.IsFieldRequired("Budget-Source-CheckBox")) {
                        _this.isCheckedSource(true);
                    }
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
            EditRequestMetadataDialog.ViewModel = ViewModel;
            function init() {
                var window = Global.Helpers.GetDialogWindow();
                var parameters = (window.options).parameters;
                var requestid = parameters.Requestid || null;
                var allowEditRequestID = parameters.allowEditRequestID || false;
                $.when(Dns.WebApi.Requests.Get(requestid), Dns.WebApi.Requests.ListRequesterCenters(null, "ID,Name", "Name"), Dns.WebApi.Requests.ListWorkPlanTypes(null, "ID,Name", "Name"), Dns.WebApi.Requests.ListReportAggregationLevels(null, "ID,Name,DeletedOn", "Name")).done(function (request, requestCenterList, workplanList, reportAggregationLevelsList) {
                    var projectid = request[0].ProjectID;
                    $.when(Dns.WebApi.Projects.GetActivityTreeByProjectID(projectid)).done(function (activityTree) {
                        bind(request[0], requestCenterList, workplanList, reportAggregationLevelsList, activityTree, allowEditRequestID);
                    });
                });
            }
            EditRequestMetadataDialog.init = init;
            function bind(request, requestCenterList, workplanList, reportAggregationLevelsList, activityTree, allowEditRequestID) {
                $.when(Dns.WebApi.Projects.GetFieldOptions(request.ProjectID, User.ID)).done(function (fieldOptions) {
                    var bindingControl = $('#EditRequestMetadataDialog');
                    vm = new ViewModel(request, requestCenterList, workplanList, reportAggregationLevelsList, activityTree, fieldOptions, allowEditRequestID, bindingControl);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            init();
        })(EditRequestMetadataDialog = Utility.EditRequestMetadataDialog || (Utility.EditRequestMetadataDialog = {}));
    })(Utility = Requests.Utility || (Requests.Utility = {}));
})(Requests || (Requests = {}));
