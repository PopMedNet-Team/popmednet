var Request;
(function (Request) {
    var View;
    (function (View) {
        View.ResponseHistoryUrlTemplate = '';
        View.RawModel = null;
        View.RequestID = null;
        View.AllowEditRequestID = null;
        View.vmRoutings = null;
        var vmHeader = null;
        var HeaderViewModel = (function () {
            function HeaderViewModel(fieldOptions) {
                this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
                var self = this;
                this.FieldOptions = fieldOptions;
                self.IsFieldRequired = function (id) {
                    var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                    return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
                };
                self.IsFieldVisible = function (id) {
                    var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                    return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
                };
                self.PurposeOfUse_Display = function (modelPurposeOfUse) {
                    if (modelPurposeOfUse == null) {
                        return '';
                    }
                    var pou = ko.utils.arrayFirst(self.PurposeOfUseOptions, function (a) { return a.Value == modelPurposeOfUse; });
                    if (pou) {
                        return pou.Name;
                    }
                    else {
                        return '';
                    }
                };
            }
            return HeaderViewModel;
        }());
        View.HeaderViewModel = HeaderViewModel;
        var RoutingsViewModel = (function () {
            function RoutingsViewModel(modelData, overrideableRoutingIDs, request) {
                var _this = this;
                var self = this;
                this.Model = modelData;
                this.Routings = ko.observable(modelData.Routings);
                this.Request = request;
                this.SelectedRoutings = ko.observableArray([]);
                this.DataMartsToAdd = ko.observableArray([]);
                this.Responses = this.Model.Responses.map(function (item) { return new VirtualResponseViewModel(item); });
                this.SelectedResponses = ko.observableArray(this.Responses.map(function (item) { return item.ID; }));
                this.RejectMessage = ko.observable('');
                this.GroupName = ko.observable('');
                this.AggregationModes = modelData.AggregationModes;
                this.AggregationMode = ko.observable('');
                this.DisplayResultsClicked = ko.observable('');
                this.CurrentResponse = ko.observable(null);
                this.RoutingHistory = ko.observableArray([]);
                this.OverrideableRoutingIDs = overrideableRoutingIDs || [];
                this.IncompleteRoutings = ko.computed(function () { return ko.utils.arrayMap(_this.Routings(), function (item) { return ({ ID: item.RequestDataMartID, DataMart: item.DataMartName, DataMartID: item.ID, RequestID: item.RequestID, Status: item.Status, ResponseMessage: null, ResponseGroup: null, Properties: null, ErrorMessage: null, ErrorDetail: null, RejectReason: null, Priority: null, DueDate: null }); }); });
                self.OverrideableRoutings = ko.computed(function () {
                    return ko.utils.arrayFilter(self.IncompleteRoutings(), function (item) {
                        return ko.utils.arrayFirst(self.OverrideableRoutingIDs, function (id) { return item.ID.toUpperCase() == id.ID.toUpperCase(); }) != null;
                    });
                });
                self.CanOverrideRoutingStatus = ko.computed(function () { return self.OverrideableRoutings().length > 0; });
                //this.AggregationModes = [{ ID: 'proj', Name: 'Projected View' }, { ID: 'dont', Name: 'Individual View' }, { ID: 'do', Name: 'Aggregate View' }]; 
                self.incompleteRoutesSelectAll = ko.pureComputed({
                    read: function () {
                        return self.IncompleteRoutings().length > 0 && self.SelectedRoutings().length === self.IncompleteRoutings().length;
                    },
                    write: function (value) {
                        if (value) {
                            var allID = ko.utils.arrayMap(self.IncompleteRoutings(), function (i) { return i.ID; });
                            self.SelectedRoutings(allID);
                        }
                        else {
                            self.SelectedRoutings([]);
                        }
                    }
                });
                self.onIncompleteRoutingsDataBound = function (e) {
                    var header = e.sender.thead[0];
                    ko.cleanNode(header);
                    ko.applyBindings(self, header);
                };
                self.IncompleteRoutingGridConfig = {
                    data: self.Routings,
                    rowTemplate: 'routings-row-template',
                    altRowTemplate: 'routings-altrow-template',
                    useKOTemplates: true,
                    dataBound: self.onIncompleteRoutingsDataBound,
                    columns: [
                        { title: ' ', width: 35, headerTemplate: '<input type="checkbox" title="Select All/None" data-bind="checked:incompleteRoutesSelectAll, indeterminateValue:SelectedRoutings().length > 0 && SelectedRoutings().length < IncompleteRoutings().length" />' },
                        { field: 'DataMartName', title: 'DataMart' },
                        { field: 'Status', title: 'Status' },
                        { field: 'Priority', title: 'Priority' },
                        { field: 'DueDate', title: 'Due Date' },
                        { field: 'Message', title: 'Message' },
                        { title: ' ', width: 80 }
                    ]
                };
            }
            RoutingsViewModel.prototype.onAddDataMart = function () {
                var _this = this;
                Global.Helpers.ShowDialog("Select DataMarts To Add", "/request/selectdatamarts", ["Close"], 750, 410, { DataMarts: this.Model.UnAssignedDataMarts }).done(function (result) {
                    if (!result)
                        return;
                    _this.DataMartsToAdd(result);
                    $('#frmRoutings').submit();
                });
            };
            RoutingsViewModel.prototype.onEditRoutingStatus = function () {
                var _this = this;
                var invalidRoutes = [];
                var validRoutes = [];
                ko.utils.arrayForEach(this.SelectedRoutings(), function (id) {
                    var route = ko.utils.arrayFirst(_this.IncompleteRoutings(), function (r) { return r.ID == id; });
                    if (route) {
                        if (ko.utils.arrayFirst(_this.OverrideableRoutingIDs, function (or) { return route.ID == or.ID; }) == null) {
                            invalidRoutes.push(route);
                        }
                        else {
                            validRoutes.push(route);
                        }
                    }
                });
                if (invalidRoutes.length > 0) {
                    //show warning message that invalid routes have been selected.
                    var msg = "<div class=\"alert alert-warning\"><p>You do not have permission to override the routing status of the following DataMarts: </p><p style= \"padding:10px;\">";
                    msg = msg + invalidRoutes.map(function (ir) { return ir.DataMart; }).join();
                    msg = msg + "</p></div>";
                    Global.Helpers.ShowErrorAlert("Invalid DataMarts Selected", msg);
                    return;
                }
                Global.Helpers.ShowDialog("Edit Routing Status", "/dialogs/editroutingstatus", ["Close"], 950, 475, { IncompleteDataMartRoutings: validRoutes })
                    .done(function (result) {
                    for (var dm in result) {
                        if (result[dm].NewStatus == null || result[dm].NewStatus <= 0) {
                            Global.Helpers.ShowAlert("Validation Error", "Every checked Datamart Routing must have a specified New Routing Status.");
                            return;
                        }
                    }
                    if (dm == undefined) {
                        return;
                    }
                    else {
                        Dns.WebApi.Requests.UpdateRequestDataMarts(result).done(function () {
                            window.location.reload();
                        });
                    }
                });
            };
            RoutingsViewModel.prototype.onBulkEdit = function () {
                var _this = this;
                Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: this.Request.Priority, defaultDueDate: this.Request.DueDate })
                    .done(function (result) {
                    if (result != null) {
                        var newPriority;
                        if (result.UpdatePriority) {
                            newPriority = result.PriorityValue;
                        }
                        ;
                        var newDueDate;
                        if (result.UpdateDueDate) {
                            newDueDate = result.DueDateValue;
                        }
                        // update selected datamarts here
                        _this.Routings().forEach(function (dm) {
                            if (_this.SelectedRoutings().indexOf(dm.RequestDataMartID) != -1) {
                                if (newPriority != null) {
                                    dm.Priority = newPriority;
                                }
                                if (newDueDate != null) {
                                    dm.DueDate = newDueDate;
                                }
                            }
                        });
                        _this.IncompleteRoutings().forEach(function (dm) {
                            _this.Routings().forEach(function (r) {
                                if (dm.ID == r.RequestDataMartID) {
                                    dm.DueDate = r.DueDate;
                                    dm.Priority = r.Priority;
                                }
                            });
                        });
                        Dns.WebApi.Requests.UpdateRequestDataMartsMetadata(_this.IncompleteRoutings()).done(function () {
                            window.location.reload();
                        });
                    }
                });
            };
            RoutingsViewModel.prototype.ConvertDateToLocal = function (date) {
                return moment(moment(date).format("M/D/YYYY h:mm:ss A UTC")).local().format('M/D/YYYY h:mm:ss A');
                // Moment and Javascript appears to treat ASP.net Date as localtime as it has no TZ embedded.
                //var b = moment(date);
                //b.local();
                //return b.format('M/D/YYYY h:mm:ss A');
            };
            RoutingsViewModel.prototype.onView = function () {
                View.vmRoutings.DisplayResultsClicked('true');
                $('#frmRoutings').submit();
            };
            RoutingsViewModel.prototype.onSelectAggregationMode = function (mode) {
                View.vmRoutings.AggregationMode(mode.ID);
                View.vmRoutings.DisplayResultsClicked('true');
                $('#frmRoutings').submit();
            };
            RoutingsViewModel.prototype.onReject = function () {
                //capture reject message and submit form
                var message = prompt('Please enter rejection message', '');
                if (message == null || message == '')
                    return false;
                $("#btnRejectResponses").val("Reject");
                this.RejectMessage(message);
                return true;
            };
            RoutingsViewModel.prototype.onResubmit = function () {
                //capture message and submit form
                var message = prompt('Please enter resubmit message', '');
                if (message == null || message == '')
                    return false;
                this.RejectMessage(message);
                return true;
            };
            RoutingsViewModel.prototype.onGroup = function () {
                //capture group name and submit
                var message = prompt('Please specify a name for this group', '');
                if (message == null || message == '')
                    return false;
                this.GroupName(message);
                $("#hGroupResponses").val(this.SelectedResponses().join(","));
                return true;
            };
            RoutingsViewModel.prototype.onUnGroup = function () {
                $("#hUngroupResponses").val(this.SelectedResponses().join(","));
                return true;
            };
            RoutingsViewModel.prototype.onShowResponseHistory = function (item) {
                $.ajax({
                    url: '/request/history?requestID=' + item.RequestID + '&virtualResponseID=' + item.ID + '&routingInstanceID=' + item.RequestDataMartID,
                    type: 'GET',
                    dataType: 'json'
                }).done(function (results) {
                    View.vmRoutings.RoutingHistory(results);
                    $('#responseHistoryDialog').modal('show');
                });
            };
            RoutingsViewModel.prototype.onShowRoutingHistory = function (item) {
                $.ajax({
                    url: '/request/history?requestID=' + item.RequestID + '&routingInstanceID=' + item.RequestDataMartID,
                    type: 'GET',
                    dataType: 'json'
                }).done(function (results) {
                    View.vmRoutings.RoutingHistory(results);
                    $('#responseHistoryDialog').modal('show');
                });
            };
            return RoutingsViewModel;
        }());
        View.RoutingsViewModel = RoutingsViewModel;
        function onEditRequestMetadata() {
            var EditRequestIDBool = false;
            if (View.AllowEditRequestID.toLowerCase() == "true") {
                EditRequestIDBool = true;
            }
            var oldPriority = Request.View.vmRoutings.Request.Priority;
            var oldDueDate = Request.View.vmRoutings.Request.DueDate;
            Global.Helpers.ShowDialog("Edit Request Metadata", "/request/editrequestmetadatadialog", ["Close"], 1000, 570, { Requestid: View.RequestID, allowEditRequestID: EditRequestIDBool })
                .done(function (result) {
                if (result != null) {
                    $('#Request_Name').val(result.MetadataUpdate.Name);
                    $('#Request_DueDate').val(moment(result.MetadataUpdate.DueDate).format('MM/DD/YYYY'));
                    $('#Request_Priority').val(result.priorityname);
                    $('#Request_RequestID').val(result.MetadataUpdate.MSRequestID);
                    $('#Request_RequestCenter').val(result.requestercentername);
                    $('#Request_PurposeOfUse').val(result.purposeofusename);
                    $('#Request_PhiDisclosureLevel').val(result.MetadataUpdate.PhiDisclosureLevel);
                    $('#Request_Description').val(result.MetadataUpdate.Description);
                    $('#Request_ReportAggregationLevelID').val(result.reportAggregationLevelName);
                    $('#Task_Activity').val(result.activityname);
                    $('#Task_Order').val(result.taskordername);
                    $('#Task_Activity_Project').val(result.activityprojectname);
                    $('#Request_WorkplanType').val(result.workplantypename);
                    $('#Source_Task_Order').val(result.sourcetaskordername);
                    $('#Source_Task_Activity').val(result.sourceactivityname);
                    $('#Source_Task_Activity_Project').val(result.sourceactivityprojectname);
                    Request.View.vmRoutings.Routings().forEach(function (r) {
                        if (oldDueDate != result.dueDate) {
                            r.DueDate = result.dueDate;
                        }
                        if (oldPriority != result.priority) {
                            r.Priority = result.priority;
                        }
                    });
                    Request.View.vmRoutings.IncompleteRoutings().forEach(function (dm) {
                        Request.View.vmRoutings.Routings().forEach(function (r) {
                            if (dm.ID == r.RequestDataMartID) {
                                dm.DueDate = r.DueDate;
                                dm.Priority = r.Priority;
                            }
                        });
                    });
                    Dns.WebApi.Requests.UpdateRequestDataMartsMetadata(Request.View.vmRoutings.IncompleteRoutings()).done(function () {
                        window.location.reload();
                    });
                }
            });
        }
        View.onEditRequestMetadata = onEditRequestMetadata;
        function init() {
            $.when(Dns.WebApi.Projects.GetFieldOptions(Request.View.RawModel.ProjectID, User.ID), Dns.WebApi.Requests.GetOverrideableRequestDataMarts(Request.View.RequestID, null, 'ID'), Dns.WebApi.Requests.Get(Request.View.RequestID)).done(function (fieldOptions, overrideableRoutingIDs, request) {
                $(function () {
                    var bindingControl = $("#routings");
                    View.vmRoutings = new RoutingsViewModel(View.RawModel, overrideableRoutingIDs, request[0]);
                    ko.applyBindings(View.vmRoutings, bindingControl[0]);
                    var headerBindingControl = $("#frmDetails");
                    vmHeader = new HeaderViewModel(fieldOptions);
                    ko.applyBindings(vmHeader, headerBindingControl[0]);
                });
            });
        }
        View.init = init;
        function translateRoutingStatus(status) {
            return Global.Helpers.GetEnumString(Dns.Enums.RoutingStatusTranslation, status);
        }
        View.translateRoutingStatus = translateRoutingStatus;
        function TranslatePriority(priority) {
            var translated = null;
            Dns.Enums.PrioritiesTranslation.forEach(function (p) {
                if (p.value == priority) {
                    translated = p.text;
                }
            });
            return translated;
        }
        View.TranslatePriority = TranslatePriority;
        var VirtualResponseViewModel = (function () {
            function VirtualResponseViewModel(data) {
                this.ID = data.ID;
                this.RequestDataMartID = data.RequestDataMartID;
                this.RequestID = data.RequestID;
                this.DataMartName = data.DataMartName;
                this.Messages = data.Messages;
                this.ResponseTime = data.ResponseTime;
                this.ResponseTimeFormatted = moment(moment(this.ResponseTime).format("M/D/YYYY h:mm:ss A UTC")).local().format('M/D/YYYY h:mm:ss A');
                this.IsRejectedBeforeUpload = data.IsRejectedBeforeUpload;
                this.IsRejectedAfterUpload = data.IsRejectedAfterUpload;
                this.IsResultsModified = data.IsResultsModified;
                this.NeedsApproval = data.NeedsApproval;
                this.StatusFormatted = data.IsRejectedAfterUpload ? 'Rejected After Upload' : data.IsRejectedBeforeUpload ? 'Rejected Before Upload' : data.NeedsApproval ? 'Awaiting Approval' : data.IsResultsModified ? 'Results Modified' : 'Completed';
                this.CanView = data.CanView;
                this.CanGroup = data.CanGroup;
                this.CanApprove = data.CanApprove;
            }
            return VirtualResponseViewModel;
        }());
        View.VirtualResponseViewModel = VirtualResponseViewModel;
    })(View = Request.View || (Request.View = {}));
})(Request || (Request = {}));
//# sourceMappingURL=View.js.map