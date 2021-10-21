/// <reference path="../../../../../js/requests/details.ts" />
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
    var Default;
    (function (Default) {
        var ResponseApproval;
        (function (ResponseApproval) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(routings, bindingControl) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    var self = _this;
                    self.Routings = routings;
                    self.ResponseTerms = [];
                    self.SelectedCompleteRoutings = ko.observableArray([]);
                    self.HasSelectedCompleteRoutings = ko.computed(function () {
                        return self.SelectedCompleteRoutings().length > 0;
                    });
                    self.CanGroupCompletedRoutings = ko.computed(function () {
                        return self.SelectedCompleteRoutings().length > 1;
                    });
                    self.CanUnGroupCompletedRoutings = ko.computed(function () {
                        //TODO: possible to know if the selecte routing is a group?
                        return self.SelectedCompleteRoutings().length > 1;
                    });
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
                    self.IncompleteRoutings = ko.computed(function () {
                        return ko.utils.arrayFilter(self.Routings, function (routing) {
                            return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                                routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                                routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                                routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                                routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                                routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload;
                        });
                    });
                    return _this;
                }
                ViewModel.prototype.ViewResponses = function () {
                    parent.location.href = "/WorkflowRequests/SummaryResponse?ID=" + vm.SelectedCompleteRoutings();
                };
                ViewModel.prototype.ApproveResponses = function () {
                    Global.Helpers.ShowPrompt('', 'Please enter a Approval message.').done(function (value) {
                        ko.utils.arrayForEach(vm.SelectedCompleteRoutings(), function (item) {
                            Dns.WebApi.Response.ApproveResponses({ Message: value, ResponseIDs: item });
                        });
                    });
                };
                ViewModel.prototype.RejectResponses = function () {
                    var _this = this;
                    Global.Helpers.ShowPrompt('', 'Please enter a rejection message.').done(function (value) {
                        ko.utils.arrayForEach(_this.SelectedCompleteRoutings(), function (item) {
                            Dns.WebApi.Response.RejectResponses({ Message: value, ResponseIDs: item });
                        });
                    });
                };
                ViewModel.prototype.PostComplete = function (resultID) {
                    if (!Requests.Details.rovm.Validate())
                        return;
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: null,
                            Data: null,
                            Comment: comment
                        }).done(function (results) {
                            var result = results[0];
                            if (result.Uri) {
                                Global.Helpers.RedirectTo(result.Uri);
                            }
                            else {
                                //Update the request etc. here 
                                Requests.Details.rovm.Request.ID(result.Entity.ID);
                                Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                Requests.Details.rovm.UpdateUrl();
                            }
                        });
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            ResponseApproval.ViewModel = ViewModel;
            $(function () {
                var id = Global.GetQueryParam("ID");
                $.when(Dns.WebApi.Requests.RequestDataMarts(id)).done(function (routings) {
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    var bindingControl = $("#DefaultViewResponse");
                    vm = new ViewModel(routings, bindingControl);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            });
        })(ResponseApproval = Default.ResponseApproval || (Default.ResponseApproval = {}));
    })(Default = Workflow.Default || (Workflow.Default = {}));
})(Workflow || (Workflow = {}));
