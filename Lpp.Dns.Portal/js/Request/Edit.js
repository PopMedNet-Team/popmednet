var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Request;
(function (Request) {
    var Edit;
    (function (Edit) {
        Edit.RawModel = null;
        Edit.GetActivitiesUrlTemplate = '';
        Edit.GetSubActivitiesUrlTemplate = '';
        var vmDataMarts;
        var vmRequestDetails;
        var Routings = (function () {
            function Routings(dataMart) {
                this.Priority = ko.observable(dataMart.Priority());
                this.DueDate = ko.observable(dataMart.DueDate());
                this.Name = dataMart.Name();
                this.Organization = dataMart.Organization();
                this.OrganizationID = dataMart.OrganizationID();
                this.ID = dataMart.ID();
            }
            return Routings;
        }());
        Edit.Routings = Routings;
        var RequestDetailsViewModel = (function (_super) {
            __extends(RequestDetailsViewModel, _super);
            function RequestDetailsViewModel(rawModel, activityTree, fieldOptions, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                _this.FieldOptions = fieldOptions;
                _this.Name = ko.observable(rawModel.Request.Name);
                _this.Priority = ko.observable(rawModel.Request.Priority);
                _this.DueDate = ko.observable(moment(rawModel.Request.DueDate).toDate());
                _this.PurposeOfUse = ko.observable(rawModel.Header.PurposeOfUse);
                _this.PhiDisclosureLevel = ko.observable(rawModel.Header.PhiDisclosureLevel);
                _this.RequesterCenterID = ko.observable(rawModel.Header.RequesterCenterID);
                _this.ProjectID = ko.observable(rawModel.Request.ProjectID);
                _this.ProjectName = ko.observable(rawModel.Request.ProjectName);
                _this.Description = ko.observable(rawModel.Header.Description);
                _this.AdditionalInstructions = ko.observable(rawModel.Header.AdditionalInstructions);
                _this.WorkplanTypeID = ko.observable(rawModel.Header.WorkplanTypeID);
                _this.MSRequestID = ko.observable(rawModel.Request.MSRequestID);
                _this.ReportAggregationLevelID = ko.observable(rawModel.Request.ReportAggregationLevelID);
                _this.EditRequestIDAllowed = rawModel.AllowEditRequestID;
                _this.ProjectActivityTree = activityTree;
                _this.AllActivitiesFlat = rawModel.Activities;
                _this.BudgetTaskOrderID = ko.observable(null);
                _this.BudgetActivityProjectID = ko.observable(null);
                _this.BudgetActivityID = ko.observable(null);
                _this.dsActivityProjects = new kendo.data.DataSource({
                    data: []
                });
                _this.dsActivities = new kendo.data.DataSource({
                    data: []
                });
                _this.dsTaskOrders = new kendo.data.DataSource({
                    data: []
                });
                _this.SourceActivityProjectID = ko.observable(rawModel.Request.SourceActivityProjectID);
                _this.dsSourceActivityProjects = new kendo.data.DataSource({
                    data: []
                });
                _this.SourceActivityID = ko.observable(rawModel.Request.SourceActivityID);
                _this.dsSourceActivities = new kendo.data.DataSource({
                    data: []
                });
                _this.SourceTaskOrderID = ko.observable(rawModel.Request.SourceTaskOrderID);
                _this.dsSourceTaskOrders = new kendo.data.DataSource({
                    data: []
                });
                _this.isCheckedSource = ko.observable(rawModel.Header.MirrorBudgetFields);
                var mirrorActivities = function () {
                    self.BudgetTaskOrderID(self.SourceTaskOrderID());
                    self.BudgetActivityID(self.SourceActivityID());
                    self.BudgetActivityProjectID(self.SourceActivityProjectID());
                };
                var currentActivityID = rawModel.Request.ActivityID;
                if (currentActivityID) {
                    var activity = ko.utils.arrayFirst(self.AllActivitiesFlat, function (a) { return a.ID.toLowerCase() == currentActivityID.toLowerCase(); });
                    if (activity) {
                        if (activity.TaskLevel == 1) {
                            _this.BudgetTaskOrderID(currentActivityID);
                        }
                        else if (activity.TaskLevel == 2) {
                            _this.BudgetTaskOrderID(activity.ParentActivityID);
                            _this.BudgetActivityID(currentActivityID);
                        }
                        else if (activity.TaskLevel == 3) {
                            _this.BudgetActivityProjectID(currentActivityID);
                            _this.BudgetActivityID(activity.ParentActivityID);
                            activity = ko.utils.arrayFirst(self.AllActivitiesFlat, function (a) { return a.ID.toLowerCase() == activity.ParentActivityID.toLowerCase() && a.TaskLevel == 2; });
                            _this.BudgetTaskOrderID(activity.ParentActivityID);
                        }
                    }
                }
                _this.SelectedActivityID = ko.computed(function () {
                    if (self.BudgetActivityProjectID() != null) {
                        return self.BudgetActivityProjectID();
                    }
                    if (self.BudgetActivityID() != null) {
                        return self.BudgetActivityID();
                    }
                    return self.BudgetTaskOrderID();
                });
                _this.UpdateActivityDataSources = function () {
                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.ProjectActivityTree, function (a) { return a.Deleted == false; }));
                    self.dsSourceTaskOrders.data(ko.utils.arrayFilter(self.ProjectActivityTree, function (a) { return a.Deleted == false; }));
                    var activities = [];
                    var activityProjects = [];
                    self.BudgetTaskOrder_DropDownArray = ko.utils.arrayFilter(self.ProjectActivityTree, function (a) { return a.Deleted == false; });
                    self.ProjectActivityTree.forEach(function (to) {
                        activities = activities.concat(ko.utils.arrayFilter(to.Activities, function (a) { return a.Deleted == false; }));
                        to.Activities.forEach(function (a) {
                            activityProjects = activityProjects.concat(ko.utils.arrayFilter(a.Activities, function (a) { return a.Deleted == false; }));
                        });
                    });
                    self.dsActivities.data(activities);
                    self.dsActivityProjects.data(activityProjects);
                    var source_activities = [];
                    var source_activityProjects = [];
                    var source_activityID = null;
                    var source_taskOrderID = null;
                    _this.ProjectActivityTree.forEach(function (tow) {
                        source_activities = source_activities.concat(tow.Activities);
                        tow.Activities.forEach(function (b) {
                            source_activityProjects = source_activityProjects.concat(b.Activities);
                        });
                    });
                    self.dsSourceActivities.data(source_activities);
                    self.BudgetActivity_DropDownArray = source_activities;
                    self.dsSourceActivityProjects.data(source_activityProjects);
                    self.BudgetActivityProject_DropDownArray = source_activityProjects;
                };
                _this.UpdateBudgetTaskOrder_DisplayName = function () {
                    var taskorderSelection = ko.utils.arrayFirst(self.BudgetTaskOrder_DropDownArray, function (a) { return a.ID == self.BudgetTaskOrderID(); });
                    if (taskorderSelection) {
                        return taskorderSelection.Name;
                    }
                    else {
                        return 'Not Selected';
                    }
                };
                _this.UpdateBudgetActivity_DisplayName = function () {
                    var activitySelection = ko.utils.arrayFirst(self.BudgetActivity_DropDownArray, function (a) { return a.ID == self.BudgetActivityID(); });
                    if (activitySelection) {
                        return activitySelection.Name;
                    }
                    else {
                        return 'Not Selected';
                    }
                };
                _this.UpdateBudgetActivityProject_DisplayName = function () {
                    var activityProjectSelection = ko.utils.arrayFirst(self.BudgetActivityProject_DropDownArray, function (a) { return a.ID == self.BudgetActivityProjectID(); });
                    if (activityProjectSelection) {
                        return activityProjectSelection.Name;
                    }
                    else {
                        return 'Not Selected';
                    }
                };
                _this.ProjectID.subscribe(function (value) {
                    if (!value) {
                        self.ProjectActivityTree = [];
                        self.UpdateActivityDataSources();
                    }
                    else {
                        Dns.WebApi.Projects.GetActivityTreeByProjectID(value).done(function (activities) {
                            self.ProjectActivityTree = activities;
                            self.UpdateActivityDataSources();
                        });
                    }
                });
                var updatingFromSource = false;
                self.isCheckedSource.subscribe(function (value) {
                    rawModel.Header.MirrorBudgetFields = value;
                    if (value) {
                        updatingFromSource = true;
                        mirrorActivities();
                        updatingFromSource = false;
                    }
                });
                self.SourceTaskOrderID.subscribe(function (newValue) {
                    self.SourceActivityProjectID(null);
                    self.SourceActivityID(null);
                    if (self.isCheckedSource() == true) {
                        updatingFromSource = true;
                        mirrorActivities();
                        updatingFromSource = false;
                    }
                });
                self.SourceActivityID.subscribe(function (newValue) {
                    self.SourceActivityProjectID(null);
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
                    else if (self.isCheckedSource() == false && updatingFromSource != true) {
                        updatingFromSource = true;
                        self.BudgetActivityProjectID(null);
                        self.BudgetActivityID(null);
                        updatingFromSource = false;
                    }
                });
                self.BudgetActivityID.subscribe(function (newValue) {
                    if (self.isCheckedSource() == true && updatingFromSource != true) {
                        mirrorActivities();
                    }
                    else if (self.isCheckedSource() == false && updatingFromSource != true) {
                        updatingFromSource = true;
                        self.BudgetActivityProjectID(null);
                        updatingFromSource = false;
                    }
                });
                self.BudgetActivityProjectID.subscribe(function (newValue) {
                    if (self.isCheckedSource() == true && updatingFromSource != true) {
                        mirrorActivities();
                    }
                });
                self.Priority.subscribe(function (newValue) {
                    vmDataMarts.DataMarts().forEach(function (dm) {
                        dm.Priority(newValue);
                    });
                });
                self.DueDate.subscribe(function (newValue) {
                    vmDataMarts.DataMarts().forEach(function (dm) {
                        dm.DueDate(newValue);
                    });
                });
                self.EnableBudgetMirroringCheckbox = ko.computed(function () { return self.SourceTaskOrderID() != null && self.SourceTaskOrderID() != ''; });
                if (_this.ProjectID()) {
                    self.UpdateActivityDataSources();
                }
                _this.Priorities = new Array({ Name: 'Low', Value: 0 }, { Name: 'Medium', Value: 1 }, { Name: 'High', Value: 2 }, { Name: 'Urgent', Value: 3 });
                _this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
                _this.PhiDisclosureLevelOptions = new Array({ Name: 'Aggregated', Value: 'Aggregated' }, { Name: 'Limited', Value: 'Limited' }, { Name: 'De-identified', Value: 'De-identified' }, { Name: 'PHI', Value: 'PHI' });
                _this.RequesterCenters = rawModel.RequesterCenters;
                _this.WorkplanTypes = rawModel.WorkplanTypes;
                _this.ReportAggregationLevels = rawModel.ReportAggregationLevels.filter(function (ral) { return ((ral.DeletedOn == undefined) || (ral.DeletedOn == null)); });
                _this.PurposeOfUse_Display = ko.computed(function () {
                    if (self.PurposeOfUse() == null)
                        return '';
                    var pou = ko.utils.arrayFirst(self.PurposeOfUseOptions, function (a) { return a.Value == self.PurposeOfUse(); });
                    if (pou) {
                        return pou.Name;
                    }
                    else {
                        return '';
                    }
                });
                return _this;
            }
            RequestDetailsViewModel.prototype.IsFieldVisible = function (id) {
                var options = ko.utils.arrayFirst(vmRequestDetails.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
            };
            RequestDetailsViewModel.prototype.IsFieldRequired = function (id) {
                var options = ko.utils.arrayFirst(vmRequestDetails.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };
            RequestDetailsViewModel.prototype.onClearProject = function () {
                this.ProjectID(null);
                this.ProjectName('');
            };
            RequestDetailsViewModel.prototype.onSelectProject = function () {
                var _this = this;
                Global.Helpers.ShowDialog("Select Project", "/request/selectproject", ["Close"], 550, 400, { Projects: Request.Edit.RawModel.Projects }).done(function (result) {
                    if (!result)
                        return;
                    _this.ProjectID(result.ID);
                    _this.ProjectName(result.Name);
                    $("form").formChanged(true);
                });
            };
            return RequestDetailsViewModel;
        }(Global.PageViewModel));
        Edit.RequestDetailsViewModel = RequestDetailsViewModel;
        var RequestDataMartsViewModel = (function () {
            function RequestDataMartsViewModel(model) {
                var _this = this;
                this.Model = model;
                var self = this;
                var selectedDataMarts = (model.SelectedDataMarts || '').split(',') || [];
                this.SelectedDataMartIDs = ko.observableArray(selectedDataMarts);
                this.SelectedRequestDataMarts = ko.observableArray(model.SelectedRequestDataMarts);
                this.RequestPriority = ko.observable(model.RequestPriority);
                this.RequestDueDate = ko.observable(model.RequestDueDate);
                this.SerializedSelectedDataMarts = ko.observable(JSON.stringify(model.SelectedRequestDataMarts));
                $("#frm").submit(function () {
                    self.SelectedRequestDataMarts(self.SelectedRoutings());
                    var dataMarts = JSON.stringify(self.SelectedRequestDataMarts());
                    self.SerializedSelectedDataMarts(dataMarts);
                });
                this.DataMarts = ko.observableArray([]);
                model.DataMarts.forEach(function (d) {
                    if (d.Organization == null || d.Organization.length == 0)
                        d.Organization = 'N/A';
                    var dm = new Dns.ViewModels.DataMartListViewModel(d);
                    var dataMart = new Request.Edit.Routings(dm);
                    self.SelectedRequestDataMarts().forEach(function (sdm) {
                        if (sdm.DataMartID == d.ID) {
                            dataMart.DueDate(moment(sdm.DueDate).toDate());
                            dataMart.Priority(sdm.Priority);
                        }
                    });
                    if (self.SelectedDataMartIDs.indexOf(d.ID) == -1) {
                        dataMart.DueDate(_this.RequestDueDate());
                        dataMart.Priority(_this.RequestPriority());
                    }
                    _this.DataMarts.push(dataMart);
                    _this.DataMarts.sort(function (l, r) { return l.Name > r.Name ? 1 : -1; });
                });
                this.SelectedRoutings = function () {
                    var dms;
                    dms = [];
                    self.DataMarts().forEach(function (dm) {
                        if (self.SelectedDataMartIDs().indexOf(dm.ID) != -1) {
                            var dataMart = (new Dns.ViewModels.RequestDataMartViewModel()).toData();
                            dataMart.DataMartID = dm.ID;
                            dataMart.Priority = dm.Priority();
                            dataMart.DueDate = dm.DueDate();
                            dms.push(dataMart);
                        }
                    });
                    return dms;
                };
                this.SelectedRequestDataMarts(self.SelectedRoutings());
                this.DataMartsBulkEdit = function () {
                    Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: self.RequestPriority(), defaultDueDate: self.RequestDueDate() })
                        .done(function (result) {
                        if (result != null) {
                            var newPriority;
                            if (result.UpdatePriority) {
                                newPriority = result.PriorityValue;
                            }
                            ;
                            var newDueDate = new Date(result.stringDate);
                            if (!result.UpdateDueDate) {
                                newDueDate = null;
                            }
                            self.DataMarts().forEach(function (dm) {
                                if (self.SelectedDataMartIDs().indexOf(dm.ID) != -1) {
                                    if (result.UpdatePriority) {
                                        dm.Priority(newPriority);
                                    }
                                    if (result.UpdateDueDate) {
                                        dm.DueDate(newDueDate);
                                    }
                                }
                            });
                        }
                        $("form").formChanged(true);
                    });
                };
            }
            RequestDataMartsViewModel.prototype.viewSelectedDataMarts = function () {
                alert(this.SelectedDataMartIDs().join('\n'));
            };
            RequestDataMartsViewModel.prototype.DataMartsSelectAll = function () {
                vmDataMarts.DataMarts().forEach(function (dm) {
                    if (vmDataMarts.SelectedDataMartIDs.indexOf(dm.ID) < 0)
                        vmDataMarts.SelectedDataMartIDs.push(dm.ID);
                });
                return false;
            };
            RequestDataMartsViewModel.prototype.DataMartsClearAll = function () {
                vmDataMarts.SelectedDataMartIDs.removeAll();
                return false;
            };
            return RequestDataMartsViewModel;
        }());
        Edit.RequestDataMartsViewModel = RequestDataMartsViewModel;
        function init() {
            $.when(Dns.WebApi.Projects.GetActivityTreeByProjectID(Edit.RawModel.Request.ProjectID), Dns.WebApi.Projects.GetFieldOptions(Edit.RawModel.Request.ProjectID, User.ID)).done(function (activityTree, fieldOptions) {
                var requestDetailsBindingContainer = $('#frm');
                vmRequestDetails = new RequestDetailsViewModel(Edit.RawModel, activityTree, fieldOptions, requestDetailsBindingContainer);
                vmDataMarts = new RequestDataMartsViewModel({
                    DataMarts: Edit.RawModel.DataMarts,
                    AllAuthorizedDataMarts: Edit.RawModel.AllAuthorizedDataMarts,
                    SelectedDataMarts: Edit.RawModel.SelectedDataMarts,
                    SelectedRequestDataMarts: Edit.RawModel.SelectedRequestDataMarts,
                    RequestPriority: Edit.RawModel.Request.Priority,
                    RequestDueDate: moment(Edit.RawModel.Request.DueDate).toDate()
                });
                $(function () {
                    ko.applyBindings(vmRequestDetails, requestDetailsBindingContainer[0]);
                    ko.applyBindings(vmDataMarts, $('#RequestDataMarts')[0]);
                    $('.dataMartMetadata').change(function () { $("form").formChanged(true); });
                });
            });
        }
        Edit.init = init;
    })(Edit = Request.Edit || (Request.Edit = {}));
})(Request || (Request = {}));
