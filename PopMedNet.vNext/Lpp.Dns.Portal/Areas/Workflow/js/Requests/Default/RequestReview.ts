/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.Default.RequestReview {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public Request: KnockoutComputed<Dns.ViewModels.RequestViewModel>;
        public AllowApproveReject: boolean;
        constructor(bindingControl: JQuery, approveRejectPermission: boolean) {
            super(bindingControl);

            this.Request = ko.computed(() => {
                return Requests.Details.rovm.Request;
            });

            this.AllowApproveReject = approveRejectPermission;
        }


        public PostComplete(resultID: string) {
            if (!Requests.Details.rovm.Validate())
                return;

            let rejected: boolean = resultID.toUpperCase() == "EA120001-7A35-4829-9F2D-A3B600E25013";

            Requests.Details.PromptForComment()
                .done((comment) => {
                    Dns.WebApi.Requests.CompleteActivity({
                        DemandActivityResultID: resultID,
                        Dto: Requests.Details.rovm.Request.toData(),

                        DataMarts: Requests.Details.rovm.RequestDataMarts().map((item) => {
                            return item.toData();
                        }),
                        Data: rejected ? comment : null,
                        Comment: comment
                    }).done((results) => {
                        if (rejected) {
                            //force a reload simpler than trying to change everything to terminated state
                            location.reload();
                        } else {

                            var result = results[0];
                            if (result.Uri) {
                                Global.Helpers.RedirectTo(result.Uri);
                            } else {
                                //Update the request etc. here 
                                Requests.Details.rovm.Request.ID(result.Entity.ID);
                                Requests.Details.rovm.Request.Timestamp(result.Entity.Timestamp);
                                Requests.Details.rovm.UpdateUrl();
                            }
                        }
                    });
                });
        }
    }

    //wrap this to execute after call to check for Approve or Reject Submission Permission
    Dns.WebApi.Users.AllowApproveRejectRequest(Requests.Details.rovm.Request.ID()).done((approveRejectPermisssion) => {
        $(() => {
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

            //Bind the view model for the activity
            let bindingControl = $("#DefaultRequestReview");
            vm = new ViewModel(bindingControl, approveRejectPermisssion[0]);
            ko.applyBindings(vm, bindingControl[0]);

            let querySummaryContainer = $("#qcViewWrapper");
            let view = querySummaryContainer.find("#QueryComposerOverview");
            view.attr("id", "taskViewQueryComposerOverview");

            Plugins.Requests.QueryBuilder.View.initialize(JSON.parse(vm.Request().Query()), vm.Request(), querySummaryContainer);


            Requests.Details.rovm.ReadOnly(true);
        });
    });
    
} 