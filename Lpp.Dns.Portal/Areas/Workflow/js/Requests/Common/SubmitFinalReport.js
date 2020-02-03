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
/// <reference path="../../../../controls/js/wffileupload/fortask.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var SubmitFinalReport;
        (function (SubmitFinalReport) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, screenPermissions, uploadViewModel) {
                    var _this = _super.call(this, bindingControl, screenPermissions) || this;
                    var self = _this;
                    self.UploadViewModel = uploadViewModel;
                    self.UploadViewModel.OnDocumentsUploaded.subscribe(function (documents) {
                        if (documents && documents.length > 0) {
                            Requests.Details.rovm.RefreshTaskDocuments();
                        }
                    });
                    return _this;
                }
                ViewModel.prototype.onModifyDraftReport = function () {
                    vm.UploadViewModel.BatchFileUpload().done(function () {
                        vm.PostComplete("ECCBF404-B3BA-4C5E-BB6E-388725938DC3");
                    });
                };
                ViewModel.prototype.onReviewFinalReport = function () {
                    vm.UploadViewModel.BatchFileUpload().done(function () {
                        vm.PostComplete("0CF45F91-6F2C-4283-BDC2-0896B552694A");
                    });
                };
                ViewModel.prototype.PostComplete = function (resultID) {
                    var completeRequestResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C';
                    var deleteFilesDeferred = $.Deferred().resolve();
                    if (this.UploadViewModel.DocumentsToDelete().length > 0) {
                        deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(this.UploadViewModel.DocumentsToDelete(), function (d) { return d.ID; }));
                    }
                    deleteFilesDeferred.done(function () {
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
                                    if (resultID.toUpperCase() == completeRequestResultID) {
                                        location.reload();
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
                    });
                };
                return ViewModel;
            }(Global.WorkflowActivityViewModel));
            SubmitFinalReport.ViewModel = ViewModel;
            $.when(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), "EndOn eq null && WorkflowActivityID eq " + Requests.Details.rovm.Request.CurrentWorkFlowActivityID()))
                .done(function (tasks) {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                Requests.Details.rovm.RegisterRequestSaveFunction(function (request) {
                    return true;
                });
                var bindingControl = $("#CommonSubmitFinalReport");
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#finalreportupload'), tasks));
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        })(SubmitFinalReport = Common.SubmitFinalReport || (Common.SubmitFinalReport = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=SubmitFinalReport.js.map