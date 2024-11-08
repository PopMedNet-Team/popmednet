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
var Workflow;
(function (Workflow) {
    var SummaryQuery;
    (function (SummaryQuery) {
        var DistributeRequest;
        (function (DistributeRequest) {
            var Routings = (function () {
                function Routings(dataMart, existingRequestDataMart) {
                    this.Priority = ko.observable(existingRequestDataMart != null ? existingRequestDataMart.Priority : dataMart.Priority);
                    this.DueDate = ko.observable(existingRequestDataMart != null ? existingRequestDataMart.DueDate : dataMart.DueDate);
                    this.Name = dataMart.Name;
                    this.Organization = dataMart.Organization;
                    this.OrganizationID = dataMart.OrganizationID;
                    this.DataMartID = dataMart.ID;
                    this._existingRequestDataMart = existingRequestDataMart;
                    this.HasExistingRouting = this._existingRequestDataMart != null;
                    var self = this;
                    self.toRequestDataMartDTO = function () {
                        var route = null;
                        if (self._existingRequestDataMart != null) {
                            route = jQuery.extend(true, {}, self._existingRequestDataMart);
                        }
                        else {
                            route = new Dns.ViewModels.RequestDataMartViewModel().toData();
                            route.DataMartID = self.DataMartID;
                            route.RequestID = self.RequestID;
                            route.DataMart = self.Name;
                        }
                        route.Priority = self.Priority();
                        route.DueDate = self.DueDate();
                        return route;
                    };
                }
                return Routings;
            }());
            DistributeRequest.Routings = Routings;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(query, routes, fieldOptions, additionalInstructions, bindingControl) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    _this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(query);
                    _this.SelectedDataMartIDs = ko.observableArray(ko.utils.arrayMap(ko.utils.arrayFilter(routes, function (rt) { return rt.HasExistingRouting; }), function (rdm) { return rdm.DataMartID; }));
                    _this.DataMarts = ko.observableArray(routes);
                    _this.DataMartAdditionalInstructions = ko.observable(additionalInstructions || '');
                    var self = _this;
                    _this.DataMartsBulkEdit = function () {
                        Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: Requests.Details.rovm.Request.Priority(), defaultDueDate: Requests.Details.rovm.Request.DueDate() })
                            .done(function (result) {
                            if (result != null) {
                                var priority = null;
                                if (result.UpdatePriority) {
                                    priority = result.PriorityValue;
                                }
                                if (result.UpdatePriority || result.UpdateDueDate) {
                                    ko.utils.arrayFilter(self.DataMarts(), function (route) {
                                        return self.SelectedDataMartIDs.indexOf(route.DataMartID) > -1;
                                    }).forEach(function (route) {
                                        if (priority != null)
                                            route.Priority(priority);
                                        if (result.UpdateDueDate)
                                            route.DueDate(new Date(result.stringDate));
                                    });
                                }
                            }
                        });
                    };
                    Requests.Details.rovm.RoutingsChanged.subscribe(function (info) {
                        var newPriority = info != null ? info.newPriority : null;
                        var newDueDate = info != null ? info.newDueDate : null;
                        _this.DataMarts().forEach(function (dm) {
                            if (newPriority != null) {
                                dm.Priority(newPriority);
                            }
                            if (newDueDate != null) {
                                dm.DueDate(newDueDate);
                            }
                        });
                    });
                    _this.CanSubmit = ko.computed(function () {
                        return self.HasPermission(PMNPermissions.ProjectRequestTypeWorkflowActivities.CloseTask) && self.SelectedDataMartIDs().length > 0;
                    });
                    self.FieldOptions = fieldOptions || [];
                    self.IsFieldRequired = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions || [], function (item) { return item.FieldIdentifier == id; });
                        return options != null && options.Permission != null && options.Permission == Dns.Enums.FieldOptionPermissions.Required;
                    };
                    self.IsFieldVisible = function (id) {
                        var options = ko.utils.arrayFirst(self.FieldOptions || [], function (item) { return item.FieldIdentifier == id; });
                        return options == null || (options.Permission != null && options.Permission != Dns.Enums.FieldOptionPermissions.Hidden);
                    };
                    self.RoutesSelectAll = ko.pureComputed({
                        read: function () {
                            return self.DataMarts().length > 0 && self.SelectedDataMartIDs().length === self.DataMarts().length;
                        },
                        write: function (value) {
                            if (value) {
                                var allID = ko.utils.arrayMap(self.DataMarts(), function (i) { return i.DataMartID; });
                                self.SelectedDataMartIDs(allID);
                            }
                            else {
                                self.SelectedDataMartIDs([]);
                            }
                        }
                    });
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            DistributeRequest.ViewModel = ViewModel;
            function init() {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                Dns.WebApi.Requests.GetCompatibleDataMarts({
                    TermIDs: null,
                    ProjectID: Requests.Details.rovm.Request.ProjectID(),
                    Request: Requests.Details.rovm.Request.Query(),
                    RequestID: Requests.Details.rovm.Request.ID()
                }).done(function (dataMarts) {
                    var existingRequestDataMarts = ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], function (dm) { return dm.toData(); });
                    var routes = [];
                    for (var di = 0; di < dataMarts.length; di++) {
                        var dm = dataMarts[di];
                        dm.Priority = Requests.Details.rovm.Request.Priority();
                        dm.DueDate = Requests.Details.rovm.Request.DueDate();
                        var existingRoute = ko.utils.arrayFirst(existingRequestDataMarts, function (r) { return r.DataMartID == dm.ID; });
                        routes.push(new Routings(dm, existingRoute));
                    }
                });
            }
            init();
        })(DistributeRequest = SummaryQuery.DistributeRequest || (SummaryQuery.DistributeRequest = {}));
    })(SummaryQuery = Workflow.SummaryQuery || (Workflow.SummaryQuery = {}));
})(Workflow || (Workflow = {}));
