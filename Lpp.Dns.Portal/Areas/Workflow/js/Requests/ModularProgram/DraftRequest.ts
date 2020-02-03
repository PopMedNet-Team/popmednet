/// <reference path="../../../../controls/js/wffileupload/fortask.ts" />
/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.ModularProgram.DraftRequest {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        private UploadViewModel: Controls.WFFileUpload.ForTask.ViewModel;

        constructor(bindingControl: JQuery, screenPermissions: any[], uploadViewModel: Controls.WFFileUpload.ForTask.ViewModel) {
            super(bindingControl, screenPermissions);
            var self = this;
            self.UploadViewModel = uploadViewModel;
        }

        public onSave() {
            if (Requests.Details.rovm.Request.ID() == null) {
                $.when<any>(Requests.Details.rovm.DefaultSave(false)).then(() => {
                    Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done((tasks: Dns.Interfaces.ITaskDTO[]) => {
                        vm.UploadViewModel.CurrentTask = tasks[0];
                        vm.UploadViewModel.BatchFileUpload().done(() => {
                            vm.PostComplete("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                        });
                    });
                })
            }
            else {
                vm.UploadViewModel.BatchFileUpload().done(() => {
                    vm.PostComplete("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                });
            }
        }
        public onSubmit() {
            if (Requests.Details.rovm.Request.ID() == null) {
                $.when<any>(Requests.Details.rovm.DefaultSave(false)).then(() => {
                    Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID()).done((tasks: Dns.Interfaces.ITaskDTO[]) => {
                        vm.UploadViewModel.CurrentTask = tasks[0];
                        vm.UploadViewModel.BatchFileUpload().done(() => {
                            vm.PostComplete("6248E8B1-7C7C-4959-993F-352C722821A6");
                        });
                    });
                })
            }
            else {
                vm.UploadViewModel.BatchFileUpload().done(() => {
                    vm.PostComplete("6248E8B1-7C7C-4959-993F-352C722821A6");
                });
            }
        }



        public PostComplete(resultID: string) {
                var deleteFilesDeferred = $.Deferred().resolve();

                if (this.UploadViewModel != null && this.UploadViewModel.DocumentsToDelete().length > 0) {
                    deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(this.UploadViewModel.DocumentsToDelete(),(d) => { return d.ID; }));
                }
                deleteFilesDeferred.done(() => {
                    Requests.Details.PromptForComment()
                        .done((comment) => {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: [],
                            Data: JSON.stringify(null),
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



    $(() => {
        if (Requests.Details.rovm.Request.ID() != undefined || Requests.Details.rovm.Request.ID() != null)
        {
            Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID())
                .done((tasks: Dns.Interfaces.ITaskDTO[]) => {
                $(() => {
                    var bindingControl = $("#TaskContent");
                    vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mp_draftreq_upload'), tasks));
                    ko.applyBindings(vm, bindingControl[0]);
                    Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
                });
            })
           
        }
        else {
            var bindingControl = $("#TaskContent");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Controls.WFFileUpload.ForTask.init($('#mp_draftreq_upload'), []));
            ko.applyBindings(vm, bindingControl[0]);
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        }
        Requests.Details.rovm.RegisterRequestSaveFunction((request: Dns.ViewModels.RequestViewModel) => {
            return true;
        });

        
    });
    }