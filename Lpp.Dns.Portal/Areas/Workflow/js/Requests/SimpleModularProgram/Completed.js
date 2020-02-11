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
    var SimpleModularProgram;
    (function (SimpleModularProgram) {
        var Completed;
        (function (Completed) {
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
            Completed.VirtualRoutingViewModel = VirtualRoutingViewModel;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, responses, responseGroups, responseSearchTerms, viewResponseDetailPermissions, requestPermissions) {
                    var _this = _super.call(this, bindingControl) || this;
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
                    _this = _super.call(this, bindingControl, screenPermissions) || this;
                    _this.ViewResponseDetailPermissions = viewResponseDetailPermissions || [];
                    var self = _this;
                    self.Routings = ko.observableArray(responses.RequestDataMarts || []);
                    self.ResponseTerms = ko.utils.arrayMap(responseSearchTerms, function (t) { return new DisplaySearchTermViewModel(t); });
                    self.SelectedCompleteRoutings = ko.observableArray([]);
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
                    self.HasSelectedCompleteRoutings = ko.computed(function () {
                        return self.SelectedCompleteRoutings().length > 0;
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
                    self.HasViewResponseDetailPermission = function (permissionID) {
                        return ko.utils.arrayFirst(self.ViewResponseDetailPermissions, function (item) {
                            return item.toLowerCase() == permissionID.toLowerCase();
                        }) != null;
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
                    return _this;
                }
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
            Completed.ViewModel = ViewModel;
            function init() {
                var id = Global.GetQueryParam("ID");
                //get the permissions for the view response detail, use to control the dialog view showing the result files
                var getResponseDetailPermissions = Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Requests.Details.rovm.Request.ProjectID(), 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', Requests.Details.rovm.RequestType.ID, [PMNPermissions.ProjectRequestTypeWorkflowActivities.ViewTask, PMNPermissions.ProjectRequestTypeWorkflowActivities.EditTask]);
                $.when(Dns.WebApi.Response.GetForWorkflowRequest(id, false), Dns.WebApi.Response.GetResponseGroupsByRequestID(id), Dns.WebApi.Requests.GetRequestSearchTerms(id), getResponseDetailPermissions, Dns.WebApi.Requests.GetPermissions([id], [PMNPermissions.Request.ViewHistory])).done(function (responses, responseGroups, searchTerms, responseDetailPermissions, requestPermissions) {
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    var bindingControl = $("#CompletedTaskView");
                    vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseGroups || [], searchTerms || [], responseDetailPermissions, requestPermissions || []);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            Completed.init = init;
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
        })(Completed = SimpleModularProgram.Completed || (SimpleModularProgram.Completed = {}));
    })(SimpleModularProgram = Workflow.SimpleModularProgram || (Workflow.SimpleModularProgram = {}));
})(Workflow || (Workflow = {}));
