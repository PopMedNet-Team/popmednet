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
/// <reference path="../../../../controls/js/wffileupload/fortask.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var SubmitDraftReport;
        (function (SubmitDraftReport) {
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
                ViewModel.prototype.onViewStatusAndResults = function () {
                    vm.UploadViewModel.BatchFileUpload().done(function () {
                        vm.PostComplete("B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA");
                    });
                };
                ViewModel.prototype.onReviewDraftReport = function () {
                    vm.UploadViewModel.BatchFileUpload().done(function () {
                        vm.PostComplete("7385973B-1C4F-4224-A13C-F148685F0217");
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
            SubmitDraftReport.ViewModel = ViewModel;
            $.when(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), "EndOn eq null && WorkflowActivityID eq " + Requests.Details.rovm.Request.CurrentWorkFlowActivityID()))
                .done(function (tasks) {
                Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                var bindingControl = $("#CommonSubmitDraftReport");
                vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#draftreportupload'), tasks));
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        })(SubmitDraftReport = Common.SubmitDraftReport || (Common.SubmitDraftReport = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
