/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.ModularProgram.PreDistributionTesting {
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
                vm.PostComplete("15F10745-0AAE-4EBB-8D8C-D38E85534EC3");
            });
        }
        public onPreDistibutionTestingReview() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("8D035265-44EF-40AE-A1CD-30C9EF9871DB");
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
            var bindingControl = $("#MPPreDistributionTesting");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mp_predist_upload'), tasks));

            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });
        });

}