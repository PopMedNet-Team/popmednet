/// <reference path="../../../../../js/requests/details.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Workflow;
(function (Workflow) {
    var WFDataChecker;
    (function (WFDataChecker) {
        var RequestReview;
        (function (RequestReview) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, approvePermission) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.Request = ko.computed(function () {
                        return Requests.Details.rovm.Request;
                    });
                    _this.AllowApproveReject = approvePermission;
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
                    if (!Requests.Details.rovm.Validate())
                        return;
                    Requests.Details.PromptForComment()
                        .done(function (comment) {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: Requests.Details.rovm.RequestDataMarts().map(function (item) {
                                return item.toData();
                            }),
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
            RequestReview.ViewModel = ViewModel;
            Dns.WebApi.Users.AllowApproveRejectRequest(Requests.Details.rovm.Request.ID()).done(function (approvePermisssion) {
                $(function () {
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                    //Bind the view model for the activity
                    var bindingControl = $("#DataCheckerRequestReview");
                    vm = new ViewModel(bindingControl, approvePermisssion[0]);
                    ko.applyBindings(vm, bindingControl[0]);
                    var view = $("#qcViewWrapper").find("#viewQueryComposer");
                    view.attr("id", "taskViewQueryComposer");
                    //Hook up the Query Composer
                    var query = Requests.Details.rovm.Request.Query() == null ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                    var visualTerms = Requests.Details.rovm.VisualTerms;
                    Plugins.Requests.QueryBuilder.View.init(query, visualTerms, view);
                    Requests.Details.rovm.ReadOnly(true);
                });
            });
        })(RequestReview = WFDataChecker.RequestReview || (WFDataChecker.RequestReview = {}));
    })(WFDataChecker = Workflow.WFDataChecker || (Workflow.WFDataChecker = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=RequestReview.js.map