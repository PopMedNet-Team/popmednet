/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.ModularProgram.WorkingSpecification {   
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public AbortRejectMessage: KnockoutObservable<string>;
        private UploadViewModel: Controls.WFFileUpload.ForTask.ViewModel;

        constructor(bindingControl: JQuery, screenPermissions: any[], uploadViewModel: Controls.WFFileUpload.ForTask.ViewModel) {
            super(bindingControl, screenPermissions);   
            this.UploadViewModel = uploadViewModel;         
        }

        public onSave() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            });
        }
        public onWorkingSpecificationReview() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("2BEF97A9-1A3A-46F8-B1D1-7E9E6F6F902A");
            });
        }
        public onSpecificationReview() {
            vm.UploadViewModel.BatchFileUpload().done(() => {
                vm.PostComplete("14B7E8CF-4CF2-4C3D-A97E-E69C5D090FC0");
            });
        }

        public PostComplete(resultID: string) {
            if (!Requests.Details.rovm.Validate())
                return;
            this.AbortRejectMessage = ko.observable("");

            if (vm.UploadViewModel.DocumentsToDelete().length > 0) {
                //don't wait for return, if fails on delete the user can delete from documents tab.
                Dns.WebApi.Documents.Delete(ko.utils.arrayMap(this.UploadViewModel.DocumentsToDelete(), (d) => { return d.ID; }));
            }

            Requests.Details.PromptForComment()
                .done((comment) => {

                    var data: Dns.Interfaces.IRequestCompletionRequestDTO = {
                        DemandActivityResultID: resultID,
                        Dto: Requests.Details.rovm.Request.toData(),
                        DataMarts: Requests.Details.rovm.RequestDataMarts().map((item) => {
                            return item.toData();
                        }),
                        Data: null,
                        Comment: comment
                    };

                    Dns.WebApi.Requests.CompleteActivity(data).done((results) => {
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
        }
    }

    $.when<any>(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), "EndOn eq null && WorkflowActivityID eq " + Requests.Details.rovm.Request.CurrentWorkFlowActivityID()))
        .done((tasks: Dns.Interfaces.ITaskDTO[]) => {
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            var bindingControl = $("#MPWorkingSpecification");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mpupload'), tasks));

            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
}