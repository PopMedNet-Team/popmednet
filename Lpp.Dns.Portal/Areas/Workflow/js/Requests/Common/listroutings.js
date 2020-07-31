/// <reference path="../../../../../js/requests/details.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var ListRoutings;
        (function (ListRoutings) {
            var VirtualRoutingViewModel = /** @class */ (function () {
                function VirtualRoutingViewModel(routing, group, responses) {
                    var _this = this;
                    this.Name = '';
                    this.IsGroup = false;
                    this.Status = Dns.Enums.RoutingStatus.AwaitingResponseApproval;
                    this.Messages = '';
                    this.Routings = [];
                    this.Routings.push(routing);
                    this.Children = ko.observableArray([]);
                    this.IsGroup = group != null;
                    if (this.IsGroup) {
                        this.ID = group.ID;
                        this.Name = group.Name;
                    }
                    else {
                        this.ID = routing.ResponseID;
                        this.Name = routing.DataMart;
                    }
                    this.Status = routing.Status;
                    this.Messages = '';
                    this.addToMessages(routing.ErrorMessage);
                    this.addToMessages(routing.ResponseMessage);
                    this.Children = ko.observableArray([]);
                    if (responses != undefined || responses != null) {
                        ko.utils.arrayForEach(responses, function (response) {
                            _this.Children.push({
                                ID: response.ID,
                                Name: 'Response ' + response.Count++,
                                RequestDataMartID: response.RequestDataMartID,
                                ResponseGroupID: response.ResponseGroupID,
                                RespondedByID: response.ResponseGroupID,
                                ResponseTime: response.ResponseTime,
                                Count: response.Count,
                                SubmittedOn: response.SubmittedOn,
                                SubmittedByID: response.SubmittedByID,
                                SubmitMessage: response.SubmitMessage,
                                ResponseMessage: response.ResponseMessage
                            });
                        });
                    }
                    this.Children.sort(function (l, r) { return l.Count > r.Count ? -1 : 1; });
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
            ListRoutings.VirtualRoutingViewModel = VirtualRoutingViewModel;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, responses, responseGroups, overrideableRoutingIDs, canViewIndividualResponses, canViewAggregateResponses, requestTypeModels, requestPermissions) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    _this.NewGroupingName = null;
                    _this.RoutingHistory = ko.observableArray([]);
                    _this.ResubmissionMessage = null;
                    _this.responseIndex = 0;
                    _this.ShowReportingOptions = false;
                    _this.ViewChildResponse = function (id) {
                        var self = _this;
                        var responseView = Dns.Enums.TaskItemTypes.Response;
                        var tabID = 'responsedetail_' + self.responseIndex;
                        self.responseIndex++;
                        var q = '//' + window.location.host + '/workflowrequests/responsedetail';
                        q += '?id=' + id;
                        q += '&view=' + responseView;
                        q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();
                        var contentFrame = document.createElement('iframe');
                        contentFrame.id = 'responsedetailframe_' + self.responseIndex;
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
                    var self = _this;
                    self.Routings = ko.observableArray(responses.RequestDataMarts || []);
                    self.ProjectID = Requests.Details.rovm.Request.ProjectID();
                    self.OverrideableRoutingIDs = overrideableRoutingIDs || [];
                    self.SelectedCompleteResponses = ko.observableArray([]);
                    self.SelectedIncompleteRoutings = ko.observableArray([]);
                    self.AllowViewIndividualResults = canViewIndividualResponses;
                    self.AllowViewAggregateResults = canViewAggregateResponses;
                    self.isDefault = (Requests.Details.rovm.Request.WorkflowID().toUpperCase() == 'F64E0001-4F9A-49F0-BF75-A3B501396946');
                    self.AllowCopy = Requests.Details.rovm.AllowCopy();
                    self.AllowViewRoutingHistory = ko.utils.arrayFirst(requestPermissions, function (p) { return p.toUpperCase() == PMNPermissions.Request.ViewHistory; }) != null;
                    self.AllowAggregateView = true;
                    //Do not allow Aggregate view for request types models associated with DataChecker, ModularProgram Models, and File Distribution Models.  Also Metadata Refrsh Query Type       
                    requestTypeModels.forEach(function (rt) {
                        if (Plugins.Requests.QueryBuilder.MDQ.Terms.Compare(rt, Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.DataCheckerModelID) ||
                            Plugins.Requests.QueryBuilder.MDQ.Terms.Compare(rt, Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.ModularProgramModelID) ||
                            Plugins.Requests.QueryBuilder.MDQ.Terms.Compare(rt, Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.DistributedRegressionModelID) ||
                            Plugins.Requests.QueryBuilder.MDQ.Terms.Compare(rt, Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.FileDistributionModelID) ||
                            (Plugins.Requests.QueryBuilder.MDQ.Terms.Compare(rt, Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.SummaryTablesModelID) && Requests.Details.rovm.OverviewQCviewViewModel.Request.Header.QueryType() != null && Requests.Details.rovm.OverviewQCviewViewModel.Request.Header.QueryType().toString() == Dns.Enums.QueryComposerQueryTypes.SummaryTable_Metadata_Refresh.toString())) {
                            self.AllowAggregateView = false;
                        }
                    });
                    self.DataMartsToAdd = ko.observableArray([]);
                    self.strDataMartsToAdd = '';
                    self.strDataMartsToCancel = '';
                    self.DataMartsToChangeRoutingStatus = ko.observableArray([]);
                    _this.ShowReportingOptions = (Requests.Details.rovm.RequestType.WorkflowID || '').toUpperCase() == '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' || (Requests.Details.rovm.RequestType.WorkflowID || '').toUpperCase() == '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D';
                    var showRoutingHistory = function (requestDataMartID, requestID) {
                        Dns.WebApi.Requests.GetResponseHistory(requestDataMartID, requestID).done(function (results) {
                            self.RoutingHistory.removeAll();
                            var errorMesssages = ko.utils.arrayMap(ko.utils.arrayFilter(results, function (r) { return (r.ErrorMessage || '').length > 0; }), function (r) {
                                return r.ErrorMessage;
                            });
                            if (errorMesssages.length > 0) {
                                Global.Helpers.ShowErrorAlert("Error Retrieving History", errorMesssages.join('<br/>'), 500);
                            }
                            else {
                                self.RoutingHistory.push.apply(self.RoutingHistory, results);
                                $('#responseHistoryDialog').modal('show');
                            }
                        });
                    };
                    self.onShowRoutingHistory = function (item) {
                        showRoutingHistory(item.Routings[0].ID, item.Routings[0].RequestID);
                    };
                    self.onShowIncompleteRoutingHistory = function (item) {
                        showRoutingHistory(item.ID, item.RequestID);
                    };
                    Requests.Details.rovm.RoutingsChanged.subscribe(function (info) {
                        //call function on the composer to update routing info
                        _this.UpdateRoutings(info);
                    });
                    self.CompletedRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (routing) {
                            return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                                routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                                routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                                routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                        }).sort(function (a, b) {
                            if (a.DataMart > b.DataMart) {
                                return 1;
                            }
                            if (a.DataMart < b.DataMart) {
                                return -1;
                            }
                            else {
                                return 0;
                            }
                        });
                    });
                    //may need edits to not hide rejected?
                    self.IncompleteRoutings = ko.observable(ko.utils.arrayFilter(self.Routings(), function (routing) {
                        return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                            routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                            routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                            routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                    }).sort(function (a, b) {
                        if (a.DataMart > b.DataMart) {
                            return 1;
                        }
                        if (a.DataMart < b.DataMart) {
                            return -1;
                        }
                        else {
                            return 0;
                        }
                    }));
                    self.HasSelectedCompleteRoutings = ko.computed(function () {
                        if (self.SelectedCompleteResponses().length === 0) {
                            return false;
                        }
                        var _loop_1 = function () {
                            var itemID = self.SelectedCompleteResponses()[i];
                            var route = ko.utils.arrayFirst(self.VirtualRoutings(), function (route) {
                                return route.ID === itemID;
                            });
                            if (route.Status === Dns.Enums.RoutingStatus.RequestRejected || route.Status === Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload)
                                return { value: false };
                        };
                        for (var i = 0; i < self.SelectedCompleteResponses().length; i++) {
                            var state_1 = _loop_1();
                            if (typeof state_1 === "object")
                                return state_1.value;
                        }
                        return true;
                    });
                    self.CanGroupCompletedRoutings = ko.computed(function () {
                        return self.SelectedCompleteResponses().length > 1;
                    });
                    self.CanUnGroupCompletedRoutings = ko.computed(function () {
                        if (self.SelectedCompleteResponses().length == 1) {
                            var virtualResponse = ko.utils.arrayFirst(self.VirtualRoutings(), function (routing) { return routing.ID == self.SelectedCompleteResponses()[0]; });
                            return virtualResponse != null && virtualResponse.IsGroup;
                        }
                        return false;
                    });
                    self.HasSelectedIncompleteRoutings = ko.computed(function () {
                        return self.SelectedIncompleteRoutings().length > 0;
                    });
                    self.VirtualRoutings = ko.observableArray([]);
                    //create the virtual routings, do the groups first
                    if (responseGroups.length > 0) {
                        ko.utils.arrayForEach(responseGroups, function (group) {
                            var routing = ko.utils.arrayFirst(self.Routings(), function (r) { return r.ResponseGroupID == group.ID; });
                            var vr = new VirtualRoutingViewModel(routing, group);
                            vr.addRoutings(ko.utils.arrayFilter(self.Routings(), function (r) { return r.ResponseGroupID == group.ID && r.ID != routing.ID; }));
                            self.VirtualRoutings.push(vr);
                        });
                    }
                    ko.utils.arrayForEach(self.CompletedRoutings(), function (routing) {
                        if (!routing.ResponseGroupID) {
                            var routeResponses = responses.Responses.filter(function (res) {
                                return res.RequestDataMartID == routing.ID;
                            });
                            self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null, routeResponses));
                        }
                    });
                    Requests.Details.rovm.ReloadRoutingsRequired.subscribe(function () {
                        //reload the response groups and responses
                        $.when(Dns.WebApi.Response.GetForWorkflowRequest(Requests.Details.rovm.Request.ID(), false), Dns.WebApi.Response.GetResponseGroupsByRequestID(Requests.Details.rovm.Request.ID())).done(function (rts, rg) {
                            //redo the virtual routes
                            var virtualRoutes = [];
                            if (rg != null && rg.length > 0) {
                                ko.utils.arrayForEach(rg, function (group) {
                                    var r = ko.utils.arrayFirst(rts[0].RequestDataMarts, function (rx) { return rx.ResponseGroupID == group.ID; });
                                    var virtRt = new VirtualRoutingViewModel(r, group);
                                    virtualRoutes.push(virtRt);
                                });
                            }
                            ko.utils.arrayForEach(rts[0].RequestDataMarts, function (rt) {
                                if (rt.ResponseGroupID == null && (rt.Status == Dns.Enums.RoutingStatus.Completed ||
                                    rt.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                                    rt.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                                    rt.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                                    rt.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                                    rt.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload)) {
                                    virtualRoutes.push(new VirtualRoutingViewModel(rt, null));
                                }
                            });
                            self.Routings(rts[0].RequestDataMarts);
                            self.VirtualRoutings(virtualRoutes);
                        });
                    });
                    self.onRemoveDataMart = function () {
                        Global.Helpers.ShowConfirm('Confirm', '<div class="alert alert-warning" style="line-height:2.0em;"><p>Removed DataMarts will have their Status changed to "Canceled". Are you sure you want to remove the selected DataMart(s)?</><p style="text-align:center;" >Select "Yes" to confirm, else select "No".</p></div>').fail(function () {
                            return;
                        }).done(function () {
                            self.strDataMartsToCancel = self.SelectedIncompleteRoutings().toString();
                            self.PostComplete('5E010001-1353-44E9-9204-A3B600E263E9');
                        });
                    };
                    self.onAddDataMartDialog = function () {
                        Dns.WebApi.Requests.GetCompatibleDataMarts({
                            TermIDs: null,
                            ProjectID: self.ProjectID,
                            Request: Requests.Details.rovm.Request.Query(),
                            RequestID: Requests.Details.rovm.Request.ID()
                        }).done(function (dataMarts) {
                            //compatible datamarts
                            var newDataMarts = dataMarts;
                            var i = 0;
                            var allRoutes = self.Routings();
                            while (i < 100) {
                                var dm = allRoutes[i];
                                //removing already submitted DMs from the list of available DMs
                                if (dm != null || undefined) {
                                    var exisitngDataMarts = ko.utils.arrayFirst(newDataMarts, function (datamart) { return datamart.ID == dm.DataMartID; });
                                    ko.utils.arrayRemoveItem(newDataMarts, exisitngDataMarts);
                                }
                                else {
                                    break;
                                }
                                i++;
                            }
                            ;
                            Global.Helpers.ShowDialog("Select DataMarts To Add", "/workflow/workflowrequests/adddatamartdialog", ["Close"], 750, 410, {
                                CurrentRoutings: responses.RequestDataMarts, AllDataMarts: newDataMarts
                            }).done(function (result) {
                                if (!result) {
                                    return;
                                }
                                self.DataMartsToAdd(result);
                                self.strDataMartsToAdd = self.DataMartsToAdd().toString();
                                self.PostComplete('15BDEF13-6E86-4E0F-8790-C07AE5B798A8');
                            });
                        });
                    };
                    self.onEditRoutingStatusDialog = function () {
                        //only pass the routes that are selected and overrideable
                        var invalidRoutes = [];
                        var validRoutes = [];
                        ko.utils.arrayForEach(self.SelectedIncompleteRoutings(), function (r) {
                            var route = ko.utils.arrayFirst(self.IncompleteRoutings(), function (ir) { return ir.ID == r; });
                            if (route) {
                                if (ko.utils.arrayFirst(self.OverrideableRoutingIDs, function (or) { return route.ID == or.ID; }) == null) {
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
                        Global.Helpers.ShowDialog("Edit DataMart Routing Status", "/Dialogs/EditRoutingStatus", ["Close"], 950, 475, { IncompleteDataMartRoutings: validRoutes })
                            .done(function (result) {
                            for (var dm in result) {
                                //code in this loop should never be hit, handled in EditRoutingStatus.
                                if (result[dm].NewStatus == null || result[dm].NewStatus <= 0) {
                                    Global.Helpers.ShowAlert("Validation Error", "Every checked Datamart Routing must have a specified New Routing Status.");
                                    return;
                                }
                            }
                            if (dm == undefined) {
                                return;
                            }
                            else {
                                self.DataMartsToChangeRoutingStatus(result);
                                self.PostComplete('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C');
                            }
                        });
                    };
                    self.onDataMartsBulkEdit = function () {
                        Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: Requests.Details.rovm.Request.Priority(), defaultDueDate: Requests.Details.rovm.Request.DueDate() })
                            .done(function (result) {
                            if (result != null) {
                                //update values for selected incomplete routings
                                var routings = self.IncompleteRoutings();
                                var updatedRoutings = [];
                                self.IncompleteRoutings([]);
                                var newDueDate = new Date(result.stringDate);
                                routings.forEach(function (dm) {
                                    if (self.SelectedIncompleteRoutings.indexOf(dm.ID) != -1) {
                                        if (result.UpdateDueDate) {
                                            dm.DueDate = newDueDate;
                                        }
                                        if (result.UpdatePriority) {
                                            dm.Priority = result.PriorityValue;
                                        }
                                    }
                                    updatedRoutings.push(dm);
                                });
                                self.IncompleteRoutings(updatedRoutings);
                                //save values for selected incomplete routings
                                self.PostComplete('4F7E1762-E453-4D12-8037-BAE8A95523F7');
                            }
                        });
                    };
                    self.onComplete = function () {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: 'E1C90001-B582-4180-9A71-A3B600EA0C27',
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: self.Routings(),
                            Data: null,
                            Comment: null
                        }).done(function (results) {
                            //force a reload of the page
                            window.location.href = "/requests/details?ID=" + Requests.Details.rovm.Request.ID();
                        });
                    };
                    self.onCompleteWorkflow = function () {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: 'E93CED3B-4B55-4991-AF84-07058ABE315C',
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: self.Routings(),
                            Data: null,
                            Comment: null
                        }).done(function (results) {
                            //force a reload of the page
                            window.location.href = "/requests/details?ID=" + Requests.Details.rovm.Request.ID();
                        });
                    };
                    self.onGroupResponses = function () {
                        //show a dialog to get the group name
                        Global.Helpers.ShowPrompt('Group Name', 'Please enter a name for the grouping:', 600, true).done(function (result) {
                            self.NewGroupingName = result;
                            self.PostComplete('49F9C682-9FAD-4AE5-A2C5-19157E227186');
                        });
                    };
                    self.OverrideableRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (item) {
                            return ko.utils.arrayFirst(self.OverrideableRoutingIDs, function (id) { return item.ID.toUpperCase() == id.ID.toUpperCase(); }) != null;
                        });
                    });
                    self.CanOverrideRoutingStatus = ko.computed(function () { return self.OverrideableRoutings().length > 0; });
                    self.onResubmitRoutings = function () {
                        Global.Helpers.ShowPrompt("Resubmit Message", 'Please enter resubmit message:', 600, false).done(function (result) {
                            self.ResubmissionMessage = result;
                            self.PostComplete('22AE0001-0B5A-4BA9-BB55-A3B600E2728C');
                        });
                    };
                    self.TranslatePriority = function (item) {
                        var translated = null;
                        Dns.Enums.PrioritiesTranslation.forEach(function (p) {
                            if (p.value == item) {
                                translated = p.text;
                            }
                        });
                        return translated;
                    };
                    self.formatDueDate = function (item) {
                        if (item == null)
                            return '';
                        return moment(item).format('MM/D/YYYY');
                    };
                    self.onViewResponses = function (data, evt) {
                        var ctl = $(evt.target);
                        var resultID = ctl.attr("data-ResultID");
                        var responseView = Dns.Enums.TaskItemTypes.Response;
                        if (resultID.toUpperCase() == '354A8015-5C1D-42F7-BE31-B9FCEF4A8798') {
                            //aggregate view
                            responseView = Dns.Enums.TaskItemTypes.AggregateResponse;
                        }
                        var tabID = 'responsedetail_' + self.responseIndex;
                        self.responseIndex++;
                        var q = '//' + window.location.host + '/workflowrequests/responsedetail';
                        q += '?id=' + self.SelectedCompleteResponses();
                        q += '&view=' + responseView;
                        q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();
                        var contentFrame = document.createElement('iframe');
                        contentFrame.id = 'responsedetailframe_' + self.responseIndex;
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
                    self.completeRoutesSelectAll = ko.pureComputed({
                        read: function () {
                            return self.CompletedRoutings().length > 0 && self.SelectedCompleteResponses().length === self.CompletedRoutings().length;
                        },
                        write: function (value) {
                            if (value) {
                                var allID = ko.utils.arrayMap(self.VirtualRoutings(), function (i) { return i.ID; });
                                self.SelectedCompleteResponses(allID);
                            }
                            else {
                                self.SelectedCompleteResponses([]);
                            }
                        }
                    });
                    self.incompleteRoutesSelectAll = ko.pureComputed({
                        read: function () {
                            return self.IncompleteRoutings().length > 0 && self.SelectedIncompleteRoutings().length === self.IncompleteRoutings().length;
                        },
                        write: function (value) {
                            if (value) {
                                var allID = ko.utils.arrayMap(self.IncompleteRoutings(), function (i) { return i.ID; });
                                self.SelectedIncompleteRoutings(allID);
                            }
                            else {
                                self.SelectedIncompleteRoutings([]);
                            }
                        }
                    });
                    return _this;
                }
                ViewModel.prototype.UpdateRoutings = function (updates) {
                    var newPriority = updates != null ? updates.newPriority : null;
                    var newDueDate = updates != null ? updates.newDueDate : null;
                    if (newPriority != null) {
                        var requestDataMarts = this.IncompleteRoutings();
                        var updatedDataMarts = [];
                        this.IncompleteRoutings([]);
                        requestDataMarts.forEach(function (rdm) {
                            rdm.Priority = newPriority;
                            updatedDataMarts.push(rdm);
                        });
                        this.IncompleteRoutings(updatedDataMarts);
                    }
                    if (newDueDate != null) {
                        var requestDataMarts = this.IncompleteRoutings();
                        var updatedDataMarts = [];
                        this.IncompleteRoutings([]);
                        requestDataMarts.forEach(function (rdm) {
                            rdm.DueDate = newDueDate;
                            updatedDataMarts.push(rdm);
                        });
                        this.IncompleteRoutings(updatedDataMarts);
                    }
                };
                ViewModel.prototype.PostComplete = function (resultID) {
                    var _this = this;
                    if (!Requests.Details.rovm.Validate())
                        return;
                    var triggerRefresh = true;
                    var groupResultID = '49F9C682-9FAD-4AE5-A2C5-19157E227186';
                    var ungroupResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA';
                    //The viewResponseID is not directly called by the viewing button but used when calling the PostComplete. The Individual and Aggregate ID's are on the view result buttons.
                    var viewResponseID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F';
                    var viewIndiviualResultsID = '8BB67F67-764F-433B-9B61-0307836E61D8';
                    var viewAggregateResultsID = '354A8015-5C1D-42F7-BE31-B9FCEF4A8798';
                    var addDataMartsResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8';
                    var removeDataMartsResultID = '5E010001-1353-44E9-9204-A3B600E263E9';
                    var editRoutingStatusResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C';
                    var resubmitRoutingsResultID = '22AE0001-0B5A-4BA9-BB55-A3B600E2728C';
                    var routingsBulkEditID = '4F7E1762-E453-4D12-8037-BAE8A95523F7';
                    var data = null;
                    if (resultID.toUpperCase() == viewIndiviualResultsID || resultID.toUpperCase() == viewAggregateResultsID) {
                        var selectedResponses = [];
                        this.SelectedCompleteResponses().forEach(function (id) {
                            var virtualRouting = ko.utils.arrayFirst(_this.VirtualRoutings(), function (vr) { return vr.ID == id; });
                            if (virtualRouting != null) {
                                ko.utils.arrayForEach(ko.utils.arrayMap(virtualRouting.Routings, function (r) { return r.ResponseID; }), function (vr) { selectedResponses.push(vr); });
                            }
                        });
                        data = JSON.stringify({ 'ResponseID': selectedResponses, 'Mode': (resultID.toUpperCase() == viewIndiviualResultsID.toUpperCase()) ? Dns.Enums.TaskItemTypes.Response : Dns.Enums.TaskItemTypes.AggregateResponse });
                        resultID = viewResponseID;
                    }
                    else if (resultID.toUpperCase() == ungroupResultID) {
                        //will be collection of group IDs.
                        data = this.SelectedCompleteResponses().join(',');
                    }
                    else if (resultID.toUpperCase() == groupResultID) {
                        //include the group name and selected responses
                        data = JSON.stringify({
                            GroupName: this.NewGroupingName,
                            Responses: this.SelectedCompleteResponses()
                        });
                    }
                    else if (resultID.toUpperCase() == addDataMartsResultID) {
                        data = this.strDataMartsToAdd;
                    }
                    else if (resultID.toUpperCase() == removeDataMartsResultID) {
                        data = this.strDataMartsToCancel.toUpperCase();
                        triggerRefresh = false;
                    }
                    else if (resultID.toUpperCase() == editRoutingStatusResultID) {
                        data = JSON.stringify(this.DataMartsToChangeRoutingStatus());
                    }
                    else if (resultID.toUpperCase() == routingsBulkEditID) {
                        data = this.IncompleteRoutings;
                    }
                    else if (resultID.toUpperCase() == resubmitRoutingsResultID) {
                        data = JSON.stringify({
                            Responses: this.SelectedCompleteResponses(),
                            ResubmissionMessage: this.ResubmissionMessage
                        });
                    }
                    //clear out the grouping name so that it doesn't accidentally get used again.
                    this.NewGroupingName = null;
                    this.ResubmissionMessage = null;
                    var datamarts = this.Routings();
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: datamarts,
                            Data: data,
                            Comment: comment
                        }).done(function (results) {
                            var result = results[0];
                            if (triggerRefresh) {
                                if (result.Uri) {
                                    Global.Helpers.RedirectTo(result.Uri);
                                }
                                else {
                                    //Update the request etc. here 
                                    Requests.Details.rovm.Request.ID(result.Entity.ID);
                                    Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                    Requests.Details.rovm.UpdateUrl();
                                }
                            }
                            else {
                                //Need to go back to the endpoint cause the results information doesnt contain anything about DM Statuses
                                Dns.WebApi.Requests.RequestDataMarts(result.Entity.ID, "Status ne Lpp.Dns.DTO.Enums.RoutingStatus'" + Dns.Enums.RoutingStatus.Canceled + "'").done(function (response) {
                                    if (response.length == 0) {
                                        Dns.WebApi.Requests.TerminateRequest(result.Entity.ID).done(function () {
                                            window.location.reload();
                                        });
                                    }
                                    else {
                                        if (result.Uri) {
                                            Global.Helpers.RedirectTo(result.Uri);
                                        }
                                        else {
                                            //Update the request etc. here 
                                            Requests.Details.rovm.Request.ID(result.Entity.ID);
                                            Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                            Requests.Details.rovm.UpdateUrl();
                                        }
                                    }
                                });
                            }
                        });
                    });
                };
                ViewModel.prototype.OpenChildDetail = function (id) {
                    var img = $('#img-' + id);
                    var child = $('#response-' + id);
                    if (img.hasClass('k-i-plus-sm')) {
                        img.removeClass('k-i-plus-sm');
                        img.addClass('k-i-minus-sm');
                        child.show();
                    }
                    else {
                        img.addClass('k-i-plus-sm');
                        img.removeClass('k-i-minus-sm');
                        child.hide();
                    }
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            ListRoutings.ViewModel = ViewModel;
            function init() {
                $(function () {
                    var id = Global.GetQueryParam("ID");
                    $.when(Dns.WebApi.Response.GetForWorkflowRequest(id, false), Dns.WebApi.Response.CanViewIndividualResponses(id), Dns.WebApi.Response.CanViewAggregateResponses(id), Dns.WebApi.Response.GetResponseGroupsByRequestID(id), Dns.WebApi.Requests.GetOverrideableRequestDataMarts(id, null, 'ID'), Dns.WebApi.Requests.GetModelIDsforRequest(id), Dns.WebApi.Requests.GetPermissions([id], [PMNPermissions.Request.ViewHistory, PMNPermissions]))
                        .done(function (responses, canViewIndividualResponses, canViewAggregateResponses, responseGroups, overrideableRoutingIDs, requestTypeModels, requestPermissions) {
                        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                        var bindingControl = $("#CommonListRoutings");
                        ListRoutings.vm = new ViewModel(bindingControl, responses[0], responseGroups || [], overrideableRoutingIDs, canViewIndividualResponses[0], canViewAggregateResponses[0], requestTypeModels, requestPermissions || []);
                        $(function () {
                            ko.applyBindings(ListRoutings.vm, bindingControl[0]);
                        });
                    });
                });
            }
            ListRoutings.init = init;
            init();
        })(ListRoutings = Common.ListRoutings || (Common.ListRoutings = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
