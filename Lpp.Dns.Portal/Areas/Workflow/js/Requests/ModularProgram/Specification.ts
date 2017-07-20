/// <reference path="../../../../controls/js/wffileupload/fortask.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.ModularProgram.Specification {
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

        public onSave() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            });
        }
        public onEditWorkingSpecification() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("E57E6B65-140F-452B-95FF-04BDB16BCD2D");
            });
        }
        public onSpecificationReview() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("C8260E90-2C8B-435A-85C8-372B021C3E9F");
            });
        }

        public PostComplete(resultID: string) {

            var deleteFilesDeferred = $.Deferred().resolve();

            if (this.UploadViewModel.DocumentsToDelete().length > 0) {
                deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(this.UploadViewModel.DocumentsToDelete(), (d) => { return d.ID; }));
            }

            deleteFilesDeferred.done(() => {
                Requests.Details.PromptForComment()
                    .done((comment) => {
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
                                    //Update the request etc. here 
                                    Requests.Details.rovm.Request.ID(result.Entity.ID);
                                    Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                    Requests.Details.rovm.UpdateUrl();
                                }
                            });
                    });
            });             
        }
    }

    $.when<any>(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), "EndOn eq null && WorkflowActivityID eq " + Requests.Details.rovm.Request.CurrentWorkFlowActivityID()))
        .done((tasks: Dns.Interfaces.ITaskDTO[]) => {
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            Requests.Details.rovm.RegisterRequestSaveFunction((request: Dns.ViewModels.RequestViewModel) => {
                return true;
            });

            var bindingControl = $("#MPSubmitSpecifications");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mpupload'), tasks));

        $(() => {
            ko.applyBindings(vm, bindingControl[0]);
        });
    });
}