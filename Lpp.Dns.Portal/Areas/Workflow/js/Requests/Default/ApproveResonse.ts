/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.Default.ApproveResponse {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public CompletedRequestDataMarts: KnockoutObservableArray<RequestDataMartViewModel>;
        public IncompleteRequestDataMarts: KnockoutObservableArray<Dns.Interfaces.IRequestDataMartDTO>;
        constructor(completeddms: Dns.Interfaces.IRequestDataMartDTO[], incompleteddms: Dns.Interfaces.IRequestDataMartDTO[], bindingControl: JQuery) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions);
            this.CompletedRequestDataMarts = ko.observableArray(completeddms.map((rdm) => {
                return new RequestDataMartViewModel(rdm);
            }));
            this.IncompleteRequestDataMarts = ko.observableArray(incompleteddms);
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
        //Override the save function on the page that is already there and inject what's needed.
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        Requests.Details.rovm.RegisterRequestSaveFunction((request: Dns.ViewModels.RequestViewModel) => {
            throw new DOMException("Not updated for multiquery yet!");
            //request.Query(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Query.toData()));
            return true;
        });
        var dmStatuses: Dns.Interfaces.IRequestDataMartDTO[];
        var id: any = Global.GetQueryParam("ID");
        var completedDMs: Dns.Interfaces.IRequestDataMartDTO[];
        var incompletedDMs: Dns.Interfaces.IRequestDataMartDTO[];
        completedDMs = [];
        incompletedDMs = [];
        $.when<any>(
            Dns.WebApi.Requests.RequestDataMarts(id).done((results: Dns.Interfaces.IRequestDataMartDTO[]) => {
                dmStatuses = results;
            })
            ).then(() => {
                $.when<any>(

                    ko.utils.arrayForEach(dmStatuses, function (data) {
                        if (Dns.Enums.RoutingStatus.Completed == data.Status) {
                            completedDMs.push(data[0]);
                        }
                        else {
                            console.log(data);
                            incompletedDMs.push(data);

                        }
                        return;
                    })
                    ).then(() => {
                        var bindingControl = $("#DefaultViewResponse");
                        vm = new ViewModel(completedDMs, incompletedDMs, bindingControl);
                        ko.applyBindings(vm, bindingControl[0]);
                    })
            });


        //Bind the view model for the activity






    });
    export class RequestDataMartViewModel extends Dns.ViewModels.RequestDataMartViewModel {
        public Selected: KnockoutObservable<boolean>;

        constructor(requestDataMart: Dns.Interfaces.IRequestDataMartDTO) {
            super(requestDataMart);

            this.Selected = ko.observable(false);
        }
    }
}   