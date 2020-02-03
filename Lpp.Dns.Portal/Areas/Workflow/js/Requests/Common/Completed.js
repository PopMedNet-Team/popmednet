/// <reference path="../../../../../js/requests/details.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var Completed;
        (function (Completed) {
            var vm;
            var VirtualRoutingViewModel = (function () {
                function VirtualRoutingViewModel(routing, group) {
                    this.Name = '';
                    this.IsGroup = false;
                    this.Status = Dns.Enums.RoutingStatus.AwaitingResponseApproval;
                    this.Messages = '';
                    this.Routings = [];
                    this.Routings.push(routing);
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
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, routings, responseGroups, canViewResponses) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    _this.RoutingHistory = ko.observableArray([]);
                    _this.responseIndex = 0;
                    var self = _this;
                    self.Routings = routings;
                    self.AllowViewResults = ko.observable(canViewResponses);
                    self.CompletedRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings, function (routing) {
                            return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                                routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                                routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                                routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                                routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                        });
                    });
                    self.SelectedCompleteResponses = ko.observableArray([]);
                    self.HasSelectedCompleteRoutings = ko.computed(function () {
                        return self.SelectedCompleteResponses().length > 0;
                    });
                    self.VirtualRoutings = [];
                    //create the virtual routings, do the groups first
                    if (responseGroups.length > 0) {
                        ko.utils.arrayForEach(responseGroups, function (group) {
                            var routing = ko.utils.arrayFirst(self.Routings, function (r) { return r.ResponseGroupID == group.ID; });
                            var vr = new VirtualRoutingViewModel(routing, group);
                            vr.addRoutings(ko.utils.arrayFilter(self.Routings, function (r) { return r.ResponseGroupID == group.ID && r.ID != routing.ID; }));
                            self.VirtualRoutings.push(vr);
                        });
                    }
                    ko.utils.arrayForEach(self.CompletedRoutings(), function (routing) {
                        if (!routing.ResponseGroupID) {
                            self.VirtualRoutings.push(new VirtualRoutingViewModel(routing, null));
                        }
                    });
                    self.onShowRoutingHistory = function (item) {
                        $.ajax({
                            url: '/request/history?requestID=' + item.Routings[0].RequestID + '&virtualResponseID=' + item.Routings[0].ResponseID + '&routingInstanceID=' + item.Routings[0].ID,
                            type: 'GET',
                            dataType: 'json'
                        }).done(function (results) {
                            self.RoutingHistory.removeAll();
                            self.RoutingHistory.push.apply(self.RoutingHistory, results);
                            $('#responseHistoryDialog').modal('show');
                        });
                    };
                    var setupResponseTabView = function (responseView) {
                        var tabID = 'responsedetail_' + self.responseIndex;
                        self.responseIndex++;
                        var q = '//' + window.location.host + '/workflowrequests/responsedetail';
                        q += '?id=' + self.SelectedCompleteResponses();
                        q += '&view=' + responseView;
                        q += '&workflowID=' + Requests.Details.rovm.Request.WorkflowID();
                        var contentFrame = document.createElement('iframe');
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
                    self.onViewAggregatedResults = function () {
                        setupResponseTabView(Dns.Enums.TaskItemTypes.AggregateResponse);
                    };
                    self.onViewIndividualResults = function () {
                        setupResponseTabView(Dns.Enums.TaskItemTypes.Response);
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            Completed.ViewModel = ViewModel;
            function init() {
                var id = Global.GetQueryParam("ID");
                //Dns.WebApi.Response.CanViewResponses(id)
                //    .done((canViewResponses: boolean[]) => {
                //        var viewResponses = canViewResponses[0];
                //        debugger;
                //        $.when<any>(
                //            Dns.WebApi.Requests.RequestDataMarts(id),
                //            Dns.WebApi.Response.GetResponseGroupsByRequestID(id)
                //        ).done((
                //            routings: Dns.Interfaces.IRequestDataMartDTO[],
                //            responseGroups: Dns.Interfaces.IResponseGroupDTO[]
                //        ) => {
                //            console.log('done in completed init called');
                //            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                //            var bindingControl = $('#CompletedTaskView');
                //            vm = new ViewModel(bindingControl, routings, responseGroups || [], viewResponses);
                //            ko.applyBindings(vm, bindingControl[0]);
                //        });
                //    });
                //TODO: should be able to pull request datamarts from the root viewmodel
                //TODO: look at moving canviewresponse
                $.when(Dns.WebApi.Requests.RequestDataMarts(id).promise(), Dns.WebApi.Response.CanViewResponses(id).promise(), Dns.WebApi.Response.GetResponseGroupsByRequestID(id).promise()).done(function (routings, canViewResponses, responseGroups) {
                    $(function () {
                        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                        var bindingControl = $('#CompletedTaskView');
                        vm = new ViewModel(bindingControl, routings, responseGroups || [], canViewResponses[0]);
                        if (bindingControl[0]) {
                            ko.applyBindings(vm, bindingControl[0]);
                        }
                        else {
                            console.log('bindingControl not found!!');
                        }
                    });
                });
            }
            Completed.init = init;
            init();
        })(Completed = Common.Completed || (Common.Completed = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=Completed.js.map