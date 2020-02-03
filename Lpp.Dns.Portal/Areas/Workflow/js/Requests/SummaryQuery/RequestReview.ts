 /// <reference path="../../../../../js/requests/details.ts" />

module Workflow.SummaryQuery.RequestReview {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public AbortRejectMessage: KnockoutObservable<string>;
        public AllowApprove: boolean;
        constructor(bindingControl: JQuery, approvePermission: boolean) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions);
            this.AbortRejectMessage = ko.observable("");

            this.AllowApprove = approvePermission;

            Requests.Details.rovm.RoutingsChanged.subscribe((info: any) => {
                //call function on the composer to update routing info
                Plugins.Requests.QueryBuilder.DataMartRouting.vm.UpdateRoutings(info);
            });
        }

        public PostComplete(resultID: string) {
            debugger;
            if (Plugins.Requests.QueryBuilder.Edit.vm !== undefined && Plugins.Requests.QueryBuilder.Edit.vm.fileUpload()) {
                var deleteFilesDeferred = $.Deferred().resolve();

                if (Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel != null && Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete().length > 0) {
                    deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete(), (d) => { return d.ID; }));
                }
                deleteFilesDeferred.done(() => {
                    if (!Requests.Details.rovm.Validate())
                        return;

                    var selectedDataMartIDs = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedDataMartIDs();
                    if (selectedDataMartIDs.length == 0 && resultID != "DFF3000B-B076-4D07-8D83-05EDE3636F4D") {
                        Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>A DataMart needs to be selected</p></div>');
                        return;
                    }

                    var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();

                    if (Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.Documents().length === 0) {
                        Global.Helpers.ShowConfirm("No Documents Uploaded", "<p>No documents have been uploaded.  Do you want to continue submitting the request?").done(() => {
                            Requests.Details.PromptForComment().done((comment) => {
                                Dns.WebApi.Requests.CompleteActivity({
                                    DemandActivityResultID: resultID,
                                    Dto: Requests.Details.rovm.Request.toData(),
                                    DataMarts: selectedDataMarts,
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
                        }).fail(() => { return; });
                    }
                    else {
                        Requests.Details.PromptForComment().done((comment) => {
                            Dns.WebApi.Requests.CompleteActivity({
                                DemandActivityResultID: resultID,
                                Dto: Requests.Details.rovm.Request.toData(),
                                DataMarts: selectedDataMarts,
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
                    }
                });
            }
            else if (Plugins.Requests.QueryBuilder.Edit.vm !== undefined) {
                if (!Requests.Details.rovm.Validate())
                    return;

                var emptyCriteraGroups: boolean = false
                ko.utils.arrayForEach(Plugins.Requests.QueryBuilder.MDQ.vm.Request.Where.Criteria(), (item) => {
                    if (item.Criteria().length === 0 && item.Terms().length === 0)
                        emptyCriteraGroups = true;
                });

                if (emptyCriteraGroups) {
                    Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The Criteria Group cannot be empty.</p></div>');
                    return;
                }

                var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();

                Requests.Details.PromptForComment()
                    .done((comment) => {

                        Requests.Details.rovm.Request.Query(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
                        Requests.Details.rovm.Request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions());

                        var dto = Requests.Details.rovm.Request.toData();

                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: dto,
                            DataMarts: selectedDataMarts,
                            Data: JSON.stringify(this.AbortRejectMessage()),
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
            }
            else {
                Requests.Details.PromptForComment()
                    .done((comment) => {
                        Dns.WebApi.Requests.CompleteActivity({
                            DemandActivityResultID: resultID,
                            Dto: Requests.Details.rovm.Request.toData(),
                            DataMarts: Requests.Details.rovm.RequestDataMarts().map((item) => {
                                return item.toData();
                            }),
                            Data: JSON.stringify(this.AbortRejectMessage()),
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
            }
        }

        public Abort() {
            Global.Helpers.ShowPrompt('Abort', 'Please enter a Abort message.').done((value: any) => {
                this.AbortRejectMessage = value
            });
        }
        public Reject() {
            Global.Helpers.ShowPrompt('Reject', 'Please enter a Reject message.').done((value: any) => {
                this.AbortRejectMessage = value
            });
        }
    }
    Dns.WebApi.Users.AllowApproveRejectRequest(Requests.Details.rovm.Request.ID()).done((approvePermisssion) => {
        $(() => {

            //Bind the view model for the activity
            var bindingControl = $("#DefaultRequestReview");
            vm = new ViewModel(bindingControl, approvePermisssion[0]);
            ko.applyBindings(vm, bindingControl[0]);
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            //Hook up the Query Composer
            var queryData = Requests.Details.rovm.Request.Query() == null ? null : JSON.parse(Requests.Details.rovm.Request.Query());
            var visualTerms = Requests.Details.rovm.VisualTerms;
            if (Requests.Details.rovm.ScreenPermissions.indexOf(Permissions.ProjectRequestTypeWorkflowActivities.EditTask.toLowerCase()) > -1) {

                Plugins.Requests.QueryBuilder.Edit.init(
                    queryData,
                    Requests.Details.rovm.FieldOptions,
                    Requests.Details.rovm.Request.Priority(),
                    Requests.Details.rovm.Request.DueDate(),
                    Requests.Details.rovm.Request.AdditionalInstructions(),
                    ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], (dm: Requests.Details.RequestDataMartViewModel) => dm.toData()),
                    Requests.Details.rovm.Request.RequestTypeID(),
                    visualTerms,
                    false,
                    Requests.Details.rovm.Request.ProjectID(),
                    "",
                    Requests.Details.rovm.Request.ID()
                ).done((viewModel) => {

                        if (viewModel) {
                            //Override the save function on the page that is already there and inject what's needed.
                            Requests.Details.rovm.RegisterRequestSaveFunction((request: Dns.ViewModels.RequestViewModel) => {
                                request.Query(JSON.stringify(viewModel.Request.toData()));
                                request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions() || '');
                                return true;
                            });
                        }

                    });
            } else {
                Plugins.Requests.QueryBuilder.View.init(queryData, visualTerms, $('#QCreadonly'));
            }

        });
    });
}  