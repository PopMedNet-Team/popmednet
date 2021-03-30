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
                //Plugins.Requests.QueryBuilder.DataMartRouting.vm.UpdateRoutings(info);
            });
        }

        public PostComplete(resultID: string) {
            if (!Requests.Details.rovm.Validate())
                return;

            //var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();
            let selectedDataMarts = [];
            Requests.Details.PromptForComment()
                .done((comment) => {
                    throw new DOMException("Need to update for multiquery!");
                    //Requests.Details.rovm.Request.Query(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Query.toData()));
                    //Requests.Details.rovm.Request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions());

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
            //var queryData = Requests.Details.rovm.Request.Query() == null ? null : JSON.parse(Requests.Details.rovm.Request.Query());
            //var visualTerms = Requests.Details.rovm.VisualTerms;
            //if (Requests.Details.rovm.ScreenPermissions.indexOf(PMNPermissions.ProjectRequestTypeWorkflowActivities.EditTask.toLowerCase()) > -1) {

            //    Plugins.Requests.QueryBuilder.Edit.init(
            //        queryData,
            //        Requests.Details.rovm.FieldOptions,
            //        Requests.Details.rovm.Request.Priority(),
            //        Requests.Details.rovm.Request.DueDate(),
            //        Requests.Details.rovm.Request.AdditionalInstructions(),
            //        ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], (dm: Requests.Details.RequestDataMartViewModel) => dm.toData()),
            //        Requests.Details.rovm.Request.RequestTypeID(),
            //        visualTerms,
            //        false,
            //        Requests.Details.rovm.Request.ProjectID(),
            //        "",
            //        Requests.Details.rovm.Request.ID()
            //    ).done((viewModel) => {

            //            if (viewModel) {
            //                //Override the save function on the page that is already there and inject what's needed.
            //                Requests.Details.rovm.RegisterRequestSaveFunction((request: Dns.ViewModels.RequestViewModel) => {
            //                    request.Query(JSON.stringify(viewModel.Query.toData()));
            //                    request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions() || '');
            //                    return true;
            //                });
            //            }

            //        });
            //} else {
            //    Plugins.Requests.QueryBuilder.View.init(queryData, visualTerms, $('#QCreadonly'));
            //}

        });
    });
}  