var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var DistributedRegression;
    (function (DistributedRegression) {
        var ConductAnalysis;
        (function (ConductAnalysis) {
            var vm;
            var VirtualRoutingViewModel = (function () {
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
                    this.ResponseTime = routing.ResponseTime;
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
            ConductAnalysis.VirtualRoutingViewModel = VirtualRoutingViewModel;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, responses, viewResponseDetailPermissions, overrideableRoutingIDs, requestPermissions) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.responseIndex = 0;
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
                    self.SelectedCompleteRoutings = ko.observableArray([]);
                    self.SelectedAnalysisCenters = ko.observableArray([]);
                    self.CompletedRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (routing) {
                            return routing.RoutingType != Dns.Enums.RoutingType.AnalysisCenter &&
                                routing.Status == Dns.Enums.RoutingStatus.Completed ||
                                routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                                routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                                routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload ||
                                routing.Status == Dns.Enums.RoutingStatus.Failed;
                        });
                    });
                    self.AnalysisCenterRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings(), function (routing) {
                            return routing.RoutingType == Dns.Enums.RoutingType.AnalysisCenter;
                        });
                    });
                    self.AnalysisCenters = [];
                    ko.utils.arrayForEach(self.AnalysisCenterRoutings(), function (item) {
                        self.AnalysisCenters.push(new VirtualRoutingViewModel(item, null, null));
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
            ConductAnalysis.ViewModel = ViewModel;
            function init() {
                var id = Global.GetQueryParam("ID");
                var getResponseDetailPermissions = Dns.WebApi.Security.GetWorkflowActivityPermissionsForIdentity(Requests.Details.rovm.Request.ProjectID(), 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', Requests.Details.rovm.RequestType.ID, [Permissions.ProjectRequestTypeWorkflowActivities.ViewTask, Permissions.ProjectRequestTypeWorkflowActivities.EditTask]);
                $.when(Dns.WebApi.Response.GetForWorkflowRequest(id, false), getResponseDetailPermissions, Dns.WebApi.Requests.GetOverrideableRequestDataMarts(id, null, 'ID'), Dns.WebApi.Requests.GetPermissions([id], [Permissions.Request.ViewHistory])).done(function (responses, responseDetailPermissions, overrideableRoutingIDs, requestPermissions) {
                    $(function () {
                        var bindingControl = $("#DRConductAnalysis");
                        vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, responses[0], responseDetailPermissions, overrideableRoutingIDs, requestPermissions || []);
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            init();
        })(ConductAnalysis = DistributedRegression.ConductAnalysis || (DistributedRegression.ConductAnalysis = {}));
    })(DistributedRegression = Workflow.DistributedRegression || (Workflow.DistributedRegression = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=ConductAnalysis.js.map