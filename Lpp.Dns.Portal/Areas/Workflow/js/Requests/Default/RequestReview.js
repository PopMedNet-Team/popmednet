/// <reference path="../../../../../js/requests/details.ts" />
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
var Workflow;
(function (Workflow) {
    var Default;
    (function (Default) {
        var RequestReview;
        (function (RequestReview) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, approveRejectPermission) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.Request = ko.computed(function () {
                        return Requests.Details.rovm.Request;
                    });
                    _this.AllowApproveReject = approveRejectPermission;
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    if (!Requests.Details.rovm.Validate())
                        return;
                    var rejected = resultID.toUpperCase() == "EA120001-7A35-4829-9F2D-A3B600E25013";
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: Requests.Details.rovm.RequestDataMarts().map(function (item) {
                                return item.toData();
                            }),
                            Data: rejected ? comment : null,
                            Comment: comment
                        }).done(function (results) {
                            if (rejected) {
                                //force a reload simpler than trying to change everything to terminated state
                                location.reload();
                            }
                            else {
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
                            }
                        });
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            RequestReview.ViewModel = ViewModel;
            //wrap this to execute after call to check for Approve or Reject Submission Permission
            Dns.WebApi.Users.AllowApproveRejectRequest(Requests.Details.rovm.Request.ID()).done(function (approveRejectPermisssion) {
                $(function () {
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    //Bind the view model for the activity
                    var bindingControl = $("#DefaultRequestReview");
                    vm = new ViewModel(bindingControl, approveRejectPermisssion[0]);
                    ko.applyBindings(vm, bindingControl[0]);
                    var querySummaryContainer = $("#qcViewWrapper");
                    var view = querySummaryContainer.find("#QueryComposerOverview");
                    view.attr("id", "taskViewQueryComposerOverview");
                    Plugins.Requests.QueryBuilder.View.initialize(JSON.parse(vm.Request().Query()), vm.Request(), querySummaryContainer);
                    Requests.Details.rovm.ReadOnly(true);
                });
            });
        })(RequestReview = Default.RequestReview || (Default.RequestReview = {}));
    })(Default = Workflow.Default || (Workflow.Default = {}));
})(Workflow || (Workflow = {}));
