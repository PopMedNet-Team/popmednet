/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.Common.ReviewFinalReport {
    var vm: ViewModel;
    export class ViewModel extends Global.WorkflowActivityViewModel {

        public Documents: KnockoutObservableArray<Dns.Interfaces.IExtendedDocumentDTO>;

        constructor(bindingControl: JQuery, screenPermissions: any[], tasks: Dns.Interfaces.ITaskDTO[]) {
            super(bindingControl, screenPermissions);

            var self = this;
            self.Documents = ko.observableArray([]);

            if (tasks && tasks.length > 0) {
                var submitSpecificationsTask = tasks[0];

                Dns.WebApi.Documents.ByTask([submitSpecificationsTask.ID], [Dns.Enums.TaskItemTypes.ActivityDataDocument])
                    .done((documents) => {
                        ko.utils.arrayForEach(documents, (d) => {
                            self.Documents.push(d);
                        });
                    });
            }
        }

        public PostComplete(resultID: string) {
            Requests.Details.PromptForComment()
                .done((comment: string) => {
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
                                location.reload();
                            }
                        });
                });

        }
    }

    $.when<any>(Dns.WebApi.Tasks.ByRequestID(Requests.Details.rovm.Request.ID(), null, null, "EndOn desc", null, 1))
        .done((tasks: Dns.Interfaces.ITaskDTO[]) => {
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            Requests.Details.rovm.RegisterRequestSaveFunction((request: Dns.ViewModels.RequestViewModel) => {
                return true;
            });

            var bindingControl = $("#CommonReviewFinalReport");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, tasks);

            $(() => {
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
}