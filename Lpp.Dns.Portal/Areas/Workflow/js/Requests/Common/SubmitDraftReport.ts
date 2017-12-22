/// <reference path="../../../../controls/js/wffileupload/fortask.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.Common.SubmitDraftReport {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        private UploadViewModel: Controls.WFFileUpload.ForTask.ViewModel;

        constructor(bindingControl: JQuery, screenPermissions: any[], uploadViewModel: Controls.WFFileUpload.ForTask.ViewModel) {
            super(bindingControl, screenPermissions);
            
            var self = this;
            self.UploadViewModel = uploadViewModel;
            self.UploadViewModel.OnDocumentsUploaded.subscribe(documents => {
                if (documents && documents.length > 0) {
                    Requests.Details.rovm.RefreshTaskDocuments();
                }
            });
        }

        public onViewStatusAndResults() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA");
            });
 
        }
        public onReviewDraftReport() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("7385973B-1C4F-4224-A13C-F148685F0217");
            });
        }

        public PostComplete(resultID: string) {

            var completeRequestResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C';

            var deleteFilesDeferred = $.Deferred().resolve();

            if (this.UploadViewModel.DocumentsToDelete().length > 0) {
                deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(this.UploadViewModel.DocumentsToDelete(), (d) => { return d.ID; }));
            }

            deleteFilesDeferred.done(() => {
                Requests.Details.PromptForComment()
                    .done((comment: string) => {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: Requests.Details.rovm.RequestDataMarts().map((item) => {
                                return item.toData();
                            }),
                            Data: null,
                            Comment: comment
                        }).done((results) => {
                                var result = results[0];
                                if (result.Uri) {
                                    Global.Helpers.RedirectTo(result.Uri);
                                } else {
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
        }
    }

    $.when<any>(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), "EndOn eq null && WorkflowActivityID eq " + Requests.Details.rovm.Request.CurrentWorkFlowActivityID()))
        .done((tasks: Dns.Interfaces.ITaskDTO[]) => {
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            var bindingControl = $("#CommonSubmitDraftReport");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#draftreportupload'), tasks));

            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
}