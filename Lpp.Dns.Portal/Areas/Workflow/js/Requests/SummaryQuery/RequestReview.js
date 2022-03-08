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
        var RequestReview;
        (function (RequestReview) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, approvePermission) {
                    var _this = _super.call(this, bindingControl, Requests.Details.rovm.ScreenPermissions) || this;
                    _this.AbortRejectMessage = ko.observable("");
                    _this.AllowApprove = approvePermission;
                    Requests.Details.rovm.RoutingsChanged.subscribe(function (info) {
                    });
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    var _this = this;
                    if (!Requests.Details.rovm.Validate())
                        return;
                    var selectedDataMarts = [];
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        throw new DOMException("Need to update for multiquery!");
                        var dto = Requests.Details.rovm.Request.toData();
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: dto,
                            DataMarts: selectedDataMarts,
                            Data: JSON.stringify(_this.AbortRejectMessage()),
                            Comment: comment
                        }).done(function (results) {
                            var result = results[0];
                            if (result.Uri) {
                                Global.Helpers.RedirectTo(result.Uri);
                            }
                            else {
                                Requests.Details.rovm.Request.ID(result.Entity.ID);
                                Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                Requests.Details.rovm.UpdateUrl();
                            }
                        });
                    });
                };
                ViewModel.prototype.Abort = function () {
                    var _this = this;
                    Global.Helpers.ShowPrompt('Abort', 'Please enter a Abort message.').done(function (value) {
                        _this.AbortRejectMessage = value;
                    });
                };
                ViewModel.prototype.Reject = function () {
                    var _this = this;
                    Global.Helpers.ShowPrompt('Reject', 'Please enter a Reject message.').done(function (value) {
                        _this.AbortRejectMessage = value;
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            RequestReview.ViewModel = ViewModel;
            Dns.WebApi.Users.AllowApproveRejectRequest(Requests.Details.rovm.Request.ID()).done(function (approvePermisssion) {
                $(function () {
                    var bindingControl = $("#DefaultRequestReview");
                    vm = new ViewModel(bindingControl, approvePermisssion[0]);
                    ko.applyBindings(vm, bindingControl[0]);
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                });
            });
        })(RequestReview = SummaryQuery.RequestReview || (SummaryQuery.RequestReview = {}));
    })(SummaryQuery = Workflow.SummaryQuery || (Workflow.SummaryQuery = {}));
})(Workflow || (Workflow = {}));
