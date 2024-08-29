 /// <reference path="../../../../../js/requests/details.ts" />

module Workflow.Default.ResponseApproval {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public CompletedRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        public IncompleteRoutings: KnockoutComputed<Dns.Interfaces.IRequestDataMartDTO[]>;
        private Routings: Dns.Interfaces.IRequestDataMartDTO[];
        private ResponseTerms: any[];
        private SelectedCompleteRoutings: KnockoutObservableArray<any>;
        private HasSelectedCompleteRoutings: KnockoutComputed<boolean>;
        private CanGroupCompletedRoutings: KnockoutComputed<boolean>;
        private CanUnGroupCompletedRoutings: KnockoutComputed<boolean>;
        constructor(routings: Dns.Interfaces.IRequestDataMartDTO[], bindingControl: JQuery) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions);
            var self = this;

            self.Routings = routings;
            self.ResponseTerms = [];
            self.SelectedCompleteRoutings = ko.observableArray([]);
            self.HasSelectedCompleteRoutings = ko.computed(() => {
                return self.SelectedCompleteRoutings().length > 0;
            });
            self.CanGroupCompletedRoutings = ko.computed(() => {
                return self.SelectedCompleteRoutings().length > 1;
            });
            self.CanUnGroupCompletedRoutings = ko.computed(() => {
                //TODO: possible to know if the selecte routing is a group?
                return self.SelectedCompleteRoutings().length > 1;
            });

            self.CompletedRoutings = ko.computed(() => {                
                return ko.utils.arrayFilter(self.Routings, (routing) => {
                    return routing.Status == Dns.Enums.RoutingStatus.Completed ||
                        routing.Status == Dns.Enums.RoutingStatus.ResultsModified ||
                        routing.Status == Dns.Enums.RoutingStatus.AwaitingResponseApproval ||
                        routing.Status == Dns.Enums.RoutingStatus.RequestRejected ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload ||
                        routing.Status == Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                });
            });
            self.IncompleteRoutings = ko.computed(() => {
                return ko.utils.arrayFilter(self.Routings, (routing) => {
                    return routing.Status != Dns.Enums.RoutingStatus.Completed &&
                        routing.Status != Dns.Enums.RoutingStatus.ResultsModified &&
                        routing.Status != Dns.Enums.RoutingStatus.AwaitingResponseApproval &&
                        routing.Status != Dns.Enums.RoutingStatus.RequestRejected &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedBeforeUpload &&
                        routing.Status != Dns.Enums.RoutingStatus.ResponseRejectedAfterUpload
                });
            });
        }

        public ViewResponses() {
            parent.location.href = "/WorkflowRequests/SummaryResponse?ID=" + vm.SelectedCompleteRoutings();
            
        }

        public ApproveResponses() {

            Global.Helpers.ShowPrompt('', 'Please enter a Approval message.').done((value: any) => {
                ko.utils.arrayForEach(vm.SelectedCompleteRoutings(), function (item) {
                    
                        Dns.WebApi.Response.ApproveResponses({ Message: value, ResponseIDs: item});
                  
                });
               
            });
            
        }

        public RejectResponses() {


            Global.Helpers.ShowPrompt('', 'Please enter a rejection message.').done((value: any) => {
                ko.utils.arrayForEach(this.SelectedCompleteRoutings(), function (item) {
                    
                        Dns.WebApi.Response.RejectResponses({ Message: value, ResponseIDs: item});
            
                });
            });
            
        }


        public PostComplete(resultID: string) {
            if (!Requests.Details.rovm.Validate())
                return;
            Requests.Details.PromptForComment()
                .done((comment) => {
                    Dns.WebApi.Requests.CompleteActivity({
                        DemandActivityResultID: resultID,
                        Dto: Requests.Details.rovm.Request.toData(),
                        DataMarts: null,

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

    $(() => {
        var id: any = Global.GetQueryParam("ID");
        $.when<any>(Dns.WebApi.Requests.RequestDataMarts(id)).done((routings: Dns.Interfaces.IRequestDataMartDTO[]) => {
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            var bindingControl = $("#DefaultViewResponse");
            vm = new ViewModel(routings, bindingControl);
            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });

        });
    });
} 