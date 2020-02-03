var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
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
        var ReviewPreDistributionTesting;
        (function (ReviewPreDistributionTesting) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, tasks) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    var self = _this;
                    self.Documents = ko.observableArray([]);
                    if (tasks && tasks.length > 0) {
                        var submitSpecificationsTask = tasks[0];
                        Dns.WebApi.Documents.ByTask([submitSpecificationsTask.ID], [Dns.Enums.TaskItemTypes.ActivityDataDocument])
                            .done(function (documents) {
                            ko.utils.arrayForEach(documents, function (d) {
                                self.Documents.push(d);
                            });
                        });
                    }
                    return _this;
                }
                ViewModel.prototype.PostComplete = function (resultID) {
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
            ReviewPreDistributionTesting.ViewModel = ViewModel;
            $.when(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), null, null, "EndOn desc", null, 1))
                .done(function (tasks) {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                Requests.Details.rovm.RegisterRequestSaveFunction(function (request) {
                    return true;
                });
                var bindingControl = $("#MPReviewPreDistributionTesting");
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, tasks);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        })(ReviewPreDistributionTesting = ModularProgram.ReviewPreDistributionTesting || (ModularProgram.ReviewPreDistributionTesting = {}));
    })(ModularProgram = Workflow.ModularProgram || (Workflow.ModularProgram = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=PreDistributionTestingReview.js.map