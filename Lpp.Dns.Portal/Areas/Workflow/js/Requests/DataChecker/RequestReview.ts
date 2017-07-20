/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.WFDataChecker.RequestReview {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public Request: KnockoutComputed<Dns.ViewModels.RequestViewModel>;
        public AllowApproveReject: boolean;
        constructor(bindingControl: JQuery, approvePermission: boolean) {
            super(bindingControl);

            this.Request = ko.computed(() => {
                return Requests.Details.rovm.Request;
            });

            this.AllowApproveReject = approvePermission
        }


        public PostComplete(resultID: string) {
            if (!Requests.Details.rovm.Validate())
                return;

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
        }
    }
    Dns.WebApi.Users.AllowApproveRejectRequest(Requests.Details.rovm.Request.ID()).done((approvePermisssion) => {
        $(() => {

            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

            //Bind the view model for the activity
            var bindingControl = $("#DataCheckerRequestReview");
            vm = new ViewModel(bindingControl, approvePermisssion[0]);
            ko.applyBindings(vm, bindingControl[0]);

            var view = $("#qcViewWrapper").find("#viewQueryComposer");
            view.attr("id", "taskViewQueryComposer");

            //Hook up the Query Composer
            var query = Requests.Details.rovm.Request.Query() == null ? null : JSON.parse(Requests.Details.rovm.Request.Query());
            var visualTerms = Requests.Details.rovm.VisualTerms;
            Plugins.Requests.QueryBuilder.View.init(query, visualTerms, view);

            Requests.Details.rovm.ReadOnly(true);
        });
    });
} 