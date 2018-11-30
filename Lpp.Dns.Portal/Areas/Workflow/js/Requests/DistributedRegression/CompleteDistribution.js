var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var DistributedRegression;
    (function (DistributedRegression) {
        var CompleteDistribution;
        (function (CompleteDistribution) {
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
            CompleteDistribution.VirtualRoutingViewModel = VirtualRoutingViewModel;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, responses, viewResponseDetailPermissions, overrideableRoutingIDs, requestPermissions) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.ResubmissionMessage = null;
                    _this.CompleteRoutingMessage = ko.observable(null);
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
                    self.SelectedCompleteRoutings = ko.observableArray([]);
                    self.SelectedIncompleteRoutings = ko.observableArray([]);
                    self.SelectedAnalysisCenters = ko.observableArray([]);
                    self.DataMartsToAdd = ko.observableArray([]);
                    self.strDataMartsToAdd = '';
                    self.strDataMartsToCancel = '';
                    self.DataMartsToChange = ko.observableArray([]);
                    self.strDataMartsToChange = '';
                    self.AllowViewRoutingHistory = ko.utils.arrayFirst(requestPermissions, function (p) { return p.toUpperCase() == Permissions.Request.ViewHistory; }) != null;
                    self.CompletedRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (routing) {
                            return routing.RoutingType != Dns.Enums.RoutingType.AnalysisCenter &&
                                routing.Status == Dns.Enums.RoutingStatus.Completed ||
                                routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                                routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                                routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                        });
                    });
                    self.IncompleteRoutings = ko.observable(ko.utils.arrayFilter(self.Routings(), function (routing) {
                        return routing.RoutingType != Dns.Enums.RoutingType.AnalysisCenter &&
                            routing.Status != Dns.Enums.RoutingStatus.Completed &&
                            routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                            routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                            routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                            routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                    }));
                    self.AnalysisCenterRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (routing) {
                            return routing.RoutingType == Dns.Enums.RoutingType.AnalysisCenter;
                        });
                    });
                    self.HasSelectedCompleteRoutings = ko.computed(function () {
                        return self.SelectedCompleteRoutings().length > 0;
                    });
                    self.HasSelectedIncompleteRoutings = ko.computed(function () {
                        return self.SelectedIncompleteRoutings().length > 0;
                    });
                    self.HasSelectedAnalysisCenters = ko.computed(function () {
                        return self.SelectedAnalysisCenters().length > 0;
                    });
                    self.VirtualRoutings = [];
                    ko.utils.arrayForEach(self.CompletedRoutings(), function (routing) {
                        if (!routing.ResponseGroupID && routing.RoutingType != Dns.Enums.RoutingType.AnalysisCenter) {
                            var routeResponses = responses.Responses.filter(function (res) {
                                return res.RequestDataMartID == routing.ID;
                            });
                            self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null, routeResponses));
                        }
                    });
                    self.AnalysisCenters = [];
                    ko.utils.arrayForEach(self.AnalysisCenterRoutings(), function (routing) {
                        var resp = responses.Responses.filter(function (res) {
                            return res.RequestDataMartID == routing.ID;
                        });
                        self.AnalysisCenters.push(new VirtualRoutingViewModel(routing, null, resp));
                    });
                    self.CanCompleteActivity = ko.computed(function () {
                        return self.HasPermission(Permissions.ProjectRequestTypeWorkflowActivities.CloseTask) && ko.utils.arrayFilter(self.IncompleteRoutings(), function (r) { return r.Status != Dns.Enums.RoutingStatus.Canceled; }).length == 0;
                    });
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
                    self.onCompleteRouting = function () {
                        if (self.SelectedIncompleteRoutings().length != 1) {
                            Global.Helpers.ShowAlert('Invalid Selection', '<p>Please select only a single DataMart.</p>');
                        }
                        ko.utils.arrayForEach(self.SelectedIncompleteRoutings(), function (dm) {
                            var response = ko.utils.arrayFirst(self.IncompleteRoutings(), function (item) {
                                return item.ID == dm;
                            });
                            Global.Helpers.ShowDialog("Upload Response for " + response.DataMart, "/controls/WFFileUpload/ResponseForDataPartner", [""], 960, 725, { RequestID: Requests.Details.rovm.Request.ID, ResponseID: response.ResponseID, DataMart: response.DataMart }).done(function (res) {
                                if (res != undefined || res != null) {
                                    if (res.Status == "Completed") {
                                        self.CompleteRoutingMessage(res.Comment);
                                        self.PostComplete("8A68399F-D562-4A98-87C9-195D3D83A103");
                                    }
                                    else if (res.Status == "Cancel") {
                                        return;
                                    }
                                    else if (res.Status == "Failed") {
                                        Global.Helpers.ShowAlert("Upload Failed", "<p>The Upload failed</p>");
                                    }
                                }
                            });
                        });
                    };
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
                    var routingsBulkEditID = '4F7E1762-E453-4D12-8037-BAE8A95523F7';
                    var completeRequestResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C';
                    var resubmitRoutingsResultID = '5C5E0001-10A6-4992-A8BE-A3F4012D5FEB';
                    var removeDataMartsResultID = '5E010001-1353-44E9-9204-A3B600E263E9';
                    var addDataMartsResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8';
                    var uploadResponseResultID = '8A68399F-D562-4A98-87C9-195D3D83A103';
                    var data = null;
                    var triggerRefresh = true;
                    if (resultID.toUpperCase() == routingsBulkEditID) {
                        data = this.IncompleteRoutings;
                    }
                    else if (resultID.toUpperCase() == removeDataMartsResultID) {
                        data = this.strDataMartsToCancel.toUpperCase();
                        triggerRefresh = false;
                    }
                    else if (resultID.toUpperCase() == addDataMartsResultID) {
                        data = this.strDataMartsToAdd;
                    }
                    else if (resultID.toUpperCase() == resubmitRoutingsResultID) {
                        data = JSON.stringify({
                            ResubmissionMessage: this.ResubmissionMessage,
                            Responses: this.SelectedCompleteRoutings()
                        });
                    }
                    else if (resultID.toUpperCase() == uploadResponseResultID) {
                        var reqDM = ko.utils.arrayFirst(this.IncompleteRoutings(), function (item) {
                            var res = ko.utils.arrayFirst(_this.SelectedIncompleteRoutings(), function (re) {
                                return re;
                            });
                            return item.ID == res;
                        });
                        data = JSON.stringify({
                            RequestDM: reqDM.ID,
                            Response: reqDM.ResponseID,
                            Comment: this.CompleteRoutingMessage()
                        });
                    }
                    var datamarts = this.Routings();
                    Dns.WebApi.Requests.CompleteActivity({
                        DemandActivityResultID: resultID,
                        Dto: Requests.Details.rovm.Request.toData(),
                        DataMarts: datamarts,
                        Data: data,
                        Comment: null
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
                ViewModel.prototype.onViewResponses = function () {
                    var self = this;
                    var responseView = Dns.Enums.TaskItemTypes.Response;
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
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            CompleteDistribution.ViewModel = ViewModel;
            function init() {
                var id = Global.GetQueryParam("ID");
                //get the permissions for the view response detail, use to control the dialog view showing the result files
                var getResponseDetailPermissions = Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Requests.Details.rovm.Request.ProjectID(), 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', Requests.Details.rovm.RequestType.ID, [Permissions.ProjectRequestTypeWorkflowActivities.ViewTask, Permissions.ProjectRequestTypeWorkflowActivities.EditTask]);
                $.when(Dns.WebApi.Response.GetForWorkflowRequest(id, false), getResponseDetailPermissions, Dns.WebApi.Requests.GetOverrideableRequestDataMarts(id, null, 'ID'), Dns.WebApi.Requests.GetPermissions([id], [Permissions.Request.ViewHistory])).done(function (responses, responseDetailPermissions, overrideableRoutingIDs, requestPermissions) {
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    var bindingControl = $("#DRCompleteDistribution");
                    vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseDetailPermissions, overrideableRoutingIDs, requestPermissions || []);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            CompleteDistribution.init = init;
            init();
        })(CompleteDistribution = DistributedRegression.CompleteDistribution || (DistributedRegression.CompleteDistribution = {}));
    })(DistributedRegression = Workflow.DistributedRegression || (Workflow.DistributedRegression = {}));
})(Workflow || (Workflow = {}));
