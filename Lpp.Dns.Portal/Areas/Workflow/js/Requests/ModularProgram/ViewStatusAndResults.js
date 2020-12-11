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
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var ModularProgram;
    (function (ModularProgram) {
        var ViewStatusAndResults;
        (function (ViewStatusAndResults) {
            var vm;
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
            ViewStatusAndResults.VirtualRoutingViewModel = VirtualRoutingViewModel;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, responses, responseGroups, responseSearchTerms, viewResponseDetailPermissions, overrideableRoutingIDs, requestPermissions) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.NewGroupingName = null;
                    _this.ResubmissionMessage = null;
                    _this.responseIndex = 0;
                    _this.RoutingHistory = ko.observableArray([]);
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
                    _this.ViewResponseDetailPermissions = viewResponseDetailPermissions || [];
                    var self = _this;
                    self.Routings = ko.observableArray(responses.RequestDataMarts || []);
                    self.OverrideableRoutingIDs = overrideableRoutingIDs || [];
                    self.ResponseTerms = ko.utils.arrayMap(responseSearchTerms, function (t) { return new DisplaySearchTermViewModel(t); });
                    self.SelectedCompleteRoutings = ko.observableArray([]);
                    self.SelectedIncompleteRoutings = ko.observableArray([]);
                    self.DataMartsToAdd = ko.observableArray([]);
                    self.strDataMartsToAdd = '';
                    self.strDataMartsToCancel = '';
                    self.DataMartsToChange = ko.observableArray([]);
                    self.strDataMartsToChange = '';
                    self.AllowViewRoutingHistory = ko.utils.arrayFirst(requestPermissions, function (p) { return p.toUpperCase() == PMNPermissions.Request.ViewHistory; }) != null;
                    self.CompletedRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (routing) {
                            return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                                routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                                routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                                routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                        });
                    });
                    self.IncompleteRoutings = ko.observable(ko.utils.arrayFilter(self.Routings(), function (routing) {
                        return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                            routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                            routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                            routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                    }));
                    self.HasSelectedCompleteRoutings = ko.computed(function () {
                        return self.SelectedCompleteRoutings().length > 0;
                    });
                    self.HasSelectedIncompleteRoutings = ko.computed(function () {
                        return self.SelectedIncompleteRoutings().length > 0;
                    });
                    self.CanGroupCompletedRoutings = ko.computed(function () {
                        return self.HasPermission(PMNPermissions.ProjectRequestTypeWorkflowActivities.EditTask) && self.SelectedCompleteRoutings().length > 1;
                    });
                    self.CanUnGroupCompletedRoutings = ko.computed(function () {
                        if (self.SelectedCompleteRoutings().length == 1) {
                            var virtualResponse = ko.utils.arrayFirst(self.VirtualRoutings, function (routing) { return routing.ID == self.SelectedCompleteRoutings()[0]; });
                            return virtualResponse != null && virtualResponse.IsGroup;
                        }
                        return false;
                    });
                    self.VirtualRoutings = [];
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
                    self.CanCompleteActivity = ko.computed(function () {
                        return self.HasPermission(PMNPermissions.ProjectRequestTypeWorkflowActivities.CloseTask) && self.IncompleteRoutings().length == 0;
                    });
                    self.ViewResponse = function (data, evt) {
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
                        q += '?id=' + self.SelectedCompleteRoutings();
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
                    self.onGroupResponses = function () {
                        Global.Helpers.ShowPrompt('Group Name', 'Please enter a name for the grouping:', 600, true).done(function (result) {
                            self.NewGroupingName = result;
                            self.PostComplete('49F9C682-9FAD-4AE5-A2C5-19157E227186');
                        });
                    };
                    self.HasViewResponseDetailPermission = function (permissionID) {
                        return ko.utils.arrayFirst(self.ViewResponseDetailPermissions, function (item) {
                            return item.toLowerCase() == permissionID.toLowerCase();
                        }) != null;
                    };
                    self.onEditRoutingStatusDialog = function () {
                        Global.Helpers.ShowDialog("Select DataMarts to Edit", "/Dialogs/EditRoutingStatus", ["Close"], 750, 310, { IncompleteDataMartRoutings: self.OverrideableRoutings() })
                            .done(function (result) {
                            for (var dm in result) {
                                if (result[dm].NewStatus == null) {
                                    Global.Helpers.ShowAlert("Validation Error", "Every checked Datamart Routing must have a specified New Routing Status.");
                                    return;
                                }
                            }
                            if (dm == undefined) {
                                return;
                            }
                            else {
                                self.DataMartsToChange(result);
                                self.strDataMartsToChange = JSON.stringify(self.DataMartsToChange());
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
                    self.OverrideableRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (item) {
                            return ko.utils.arrayFirst(self.OverrideableRoutingIDs, function (id) { return item.ID.toUpperCase() == id.ID.toUpperCase(); }) != null;
                        });
                    });
                    self.CanOverrideRoutingStatus = ko.computed(function () { return self.OverrideableRoutings().length > 0; });
                    self.onResubmitRoutings = function () {
                        Global.Helpers.ShowPrompt("Resubmit Message", 'Please enter resubmit message:', 600, false).done(function (result) {
                            self.ResubmissionMessage = result;
                            self.PostComplete('5C5E0001-10A6-4992-A8BE-A3F4012D5FEB');
                        });
                    };
                    self.onRemoveDataMart = function () {
                        Global.Helpers.ShowConfirm('Confirm', '<div class="alert alert-warning" style="line-height:2.0em;"><p>Removed DataMarts will have their Status changed to "Canceled". Are you sure you want to remove the selected DataMart(s)?</><p style="text-align:center;" >Select "Yes" to confirm, else select "No".</p></div>').fail(function () {
                            return;
                        }).done(function () {
                            self.strDataMartsToCancel = self.SelectedIncompleteRoutings().toString();
                            self.PostComplete('5E010001-1353-44E9-9204-A3B600E263E9');
                        });
                    };
                    self.onAddDataMartDialog = function () {
                        var modularProgramTermID = 'a1ae0001-e5b4-46d2-9fad-a3d8014fffd8';
                        Dns.WebApi.Requests.GetCompatibleDataMarts({
                            TermIDs: [modularProgramTermID],
                            ProjectID: Requests.Details.rovm.Request.ProjectID(),
                            Request: "",
                            RequestID: Requests.Details.rovm.Request.ID()
                        }).done(function (dataMarts) {
                            //compatible datamarts
                            var newDataMarts = dataMarts;
                            var i = 0;
                            while (i < 100) {
                                var dm = self.Routings()[i];
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
                    self.TranslatePriority = function (item) {
                        var translated = null;
                        Dns.Enums.PrioritiesTranslation.forEach(function (p) {
                            if (p.value == item) {
                                translated = p.text;
                            }
                        });
                        return translated;
                    };
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
                    self.completedRoutesSelectAll = ko.pureComputed({
                        read: function () {
                            return self.CompletedRoutings().length > 0 && self.SelectedCompleteRoutings().length === self.CompletedRoutings().length;
                        },
                        write: function (value) {
                            if (value) {
                                var allID = ko.utils.arrayMap(self.VirtualRoutings, function (i) { return i.ID; });
                                self.SelectedCompleteRoutings(allID);
                            }
                            else {
                                self.SelectedCompleteRoutings([]);
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
                ViewModel.prototype.ApproveResponses = function () {
                    var _this = this;
                    Global.Helpers.ShowPrompt('', 'Please enter a Approval message.').done(function (value) {
                        Dns.WebApi.Response.ApproveResponses({ Message: value, ResponseIDs: _this.SelectedCompleteRoutings() }).done(function () {
                            window.location.reload();
                        });
                    });
                };
                ViewModel.prototype.RejectResponses = function () {
                    var _this = this;
                    Global.Helpers.ShowPrompt('', 'Please enter a rejection message.').done(function (value) {
                        Dns.WebApi.Response.RejectResponses({ Message: value, ResponseIDs: _this.SelectedCompleteRoutings() }).done(function () {
                            window.location.reload();
                        });
                    });
                };
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
                    var editRoutingStatusResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C';
                    var routingsBulkEditID = '4F7E1762-E453-4D12-8037-BAE8A95523F7';
                    var groupResultID = '49F9C682-9FAD-4AE5-A2C5-19157E227186';
                    var ungroupResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA';
                    var completeRequestResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C';
                    var resubmitRoutingsResultID = '5C5E0001-10A6-4992-A8BE-A3F4012D5FEB';
                    var removeDataMartsResultID = '5E010001-1353-44E9-9204-A3B600E263E9';
                    var addDataMartsResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8';
                    var data = null;
                    var triggerRefresh = true;
                    if (resultID.toUpperCase() == editRoutingStatusResultID) {
                        data = this.strDataMartsToChange;
                    }
                    else if (resultID.toUpperCase() == routingsBulkEditID) {
                        data = this.IncompleteRoutings;
                    }
                    else if (resultID.toUpperCase() == ungroupResultID) {
                        //will be collection of group IDs.
                        data = this.SelectedCompleteRoutings().join(',');
                    }
                    else if (resultID.toUpperCase() == removeDataMartsResultID) {
                        data = this.strDataMartsToCancel.toUpperCase();
                        triggerRefresh = false;
                    }
                    else if (resultID.toUpperCase() == addDataMartsResultID) {
                        data = this.strDataMartsToAdd;
                    }
                    else if (resultID.toUpperCase() == groupResultID) {
                        //include the group name and selected responses
                        data = JSON.stringify({
                            GroupName: this.NewGroupingName,
                            Responses: this.SelectedCompleteRoutings()
                        });
                    }
                    else if (resultID.toUpperCase() == resubmitRoutingsResultID) {
                        data = JSON.stringify({
                            ResubmissionMessage: this.ResubmissionMessage,
                            Responses: this.SelectedCompleteRoutings()
                        });
                    }
                    //clear out the grouping name so that it doesn't accidentally get used again.
                    this.NewGroupingName = null;
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
            ViewStatusAndResults.ViewModel = ViewModel;
            function init() {
                var id = Global.GetQueryParam("ID");
                //get the permissions for the view response detail, use to control the dialog view showing the result files
                var getResponseDetailPermissions = Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Requests.Details.rovm.Request.ProjectID(), '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', Requests.Details.rovm.RequestType.ID, [PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewTask, PMNPermissions.ProjectRequestTypeWorkflowActivities.EditTask]);
                $.when(Dns.WebApi.Response.GetForWorkflowRequest(id, false), Dns.WebApi.Response.GetResponseGroupsByRequestID(id), Dns.WebApi.Requests.GetRequestSearchTerms(id), getResponseDetailPermissions, Dns.WebApi.Requests.GetOverrideableRequestDataMarts(id, null, 'ID'), Dns.WebApi.Requests.GetPermissions([id], [PMNPermissions.Request.ViewHistory])).done(function (responses, responseGroups, searchTerms, responseDetailPermissions, overrideableRoutingIDs, requestPermissions) {
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    var bindingControl = $("#MPViewStatusAndResults");
                    vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseGroups || [], searchTerms || [], responseDetailPermissions, overrideableRoutingIDs, requestPermissions || []);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            ViewStatusAndResults.init = init;
            init();
            var DisplaySearchTermViewModel = /** @class */ (function () {
                function DisplaySearchTermViewModel(term) {
                    var _this = this;
                    this.VariableNames = [
                        'Text',
                        'ICD9DiagnosisCode',
                        'ICD9ProcedureCode',
                        'HCPCSCode',
                        'GenericDrugCode',
                        'DrugClassCode',
                        'SexStratifier',
                        'AgeStratifier',
                        'ClinicalSetting',
                        'ObservationPeriod',
                        'Coverage',
                        'OutputCriteria',
                        'MetricType',
                        'MSReqID',
                        'MSProjID',
                        'MSWPType',
                        'MSWPID',
                        'MSVerID',
                        'RequestID',
                        'MP1Cycles',
                        'MP2Cycles',
                        'MP3Cycles',
                        'MP4Cycles',
                        'MP5Cycles',
                        'MP6Cycles',
                        'MP7Cycles',
                        'MP8Cycles',
                        'MP1Scenarios',
                        'MP2Scenarios',
                        'MP3Scenarios',
                        'MP4Scenarios',
                        'MP5Scenarios',
                        'MP6Scenarios',
                        'MP7Scenarios',
                        'MP8Scenarios',
                        'NumScen',
                        'NumCycle',
                        'RequesterCenter',
                        'WorkplanType'
                    ];
                    this.searchTerm = term;
                    this.Variable = ko.computed(function () {
                        if (term.Type < _this.VariableNames.length)
                            return _this.VariableNames[term.Type];
                        return term.Type.toString();
                    });
                    this.Value = ko.computed(function () {
                        if (_this.searchTerm.StringValue)
                            return _this.searchTerm.StringValue;
                        if (_this.searchTerm.NumberValue)
                            return _this.searchTerm.NumberValue.toString();
                        if (_this.searchTerm.NumberFrom && _this.searchTerm.NumberTo)
                            return _this.searchTerm.NumberFrom + ' - ' + _this.searchTerm.NumberTo;
                        if (_this.searchTerm.NumberFrom)
                            return _this.searchTerm.NumberFrom.toString();
                        if (_this.searchTerm.NumberTo)
                            return _this.searchTerm.NumberTo.toString();
                        if (_this.searchTerm.DateFrom && _this.searchTerm.DateTo)
                            return _this.searchTerm.DateFrom + ' - ' + _this.searchTerm.DateTo;
                        if (_this.searchTerm.DateFrom)
                            return _this.searchTerm.DateFrom.toString();
                        if (_this.searchTerm.DateTo)
                            return _this.searchTerm.DateTo.toString();
                    });
                }
                return DisplaySearchTermViewModel;
            }());
        })(ViewStatusAndResults = ModularProgram.ViewStatusAndResults || (ModularProgram.ViewStatusAndResults = {}));
    })(ModularProgram = Workflow.ModularProgram || (Workflow.ModularProgram = {}));
})(Workflow || (Workflow = {}));
