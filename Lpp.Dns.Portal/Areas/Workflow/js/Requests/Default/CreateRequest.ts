/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.Default.CreateRequest {
    var vm: ViewModel;
    const SaveResultID: string = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D';
    const SubmitResultID: string = '48B20001-BD0B-425D-8D49-A3B5015A2258';

    export class ViewModel extends Global.WorkflowActivityViewModel {
        
        public Request: Dns.ViewModels.RequestViewModel;
        public QueryEditor: Plugins.Requests.QueryBuilder.Edit.ViewModel;
        public SubmitButtonText: KnockoutObservable<string> = ko.observable("Submit");

        constructor(bindingControl: JQuery) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions);
            
            this.Request = Requests.Details.rovm.Request;
            let self = this;
            
            Plugins.Requests.QueryBuilder.Edit.init(
                    JSON.parse(this.Request.Query()) as Dns.Interfaces.IQueryComposerRequestDTO,
                    Requests.Details.rovm.FieldOptions,
                    this.Request.Priority(),
                    this.Request.DueDate(),
                    this.Request.AdditionalInstructions(),
                    ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], (dm: Requests.Details.RequestDataMartViewModel) => <Dns.Interfaces.IRequestDataMartDTO>{ DataMart: dm.DataMart(), DataMartID: dm.DataMartID(), RequestID: dm.RequestID(), ID: dm.ID(), Status: dm.Status(), Priority: dm.Priority(), DueDate: dm.DueDate(), ErrorMessage: null, ErrorDetail: null, RejectReason: null, Properties: null, ResponseGroup: null, ResponseMessage: null }),
                    Requests.Details.rovm.RequestType,
                    this.Request.ProjectID(),
                    this.Request.ID())
                .done((viewModel) => {
                    self.QueryEditor = viewModel;

                    Requests.Details.rovm.RegisterRequestSaveFunction((request) => {
                        request.Query(JSON.stringify(viewModel.exportRequestDTO()));
                        request.AdditionalInstructions(viewModel.AdditionalInstructions);
                        return true;
                    });

                    if (self.ScreenPermissions.indexOf(PMNPermissions.Request.SkipSubmissionApproval.toLowerCase()) < 0 && !viewModel.IsFileUpload) {
                        self.SubmitButtonText("Submit For Review");
                    }
                });
        }

        public PostComplete(resultID: string) {

            if (!Requests.Details.rovm.Validate()) {
                let valMessage = Requests.Details.rovm.ValidationMessage();
                return;
            }

            if (!this.QueryEditor.Validate())
                return;

            if (!this.QueryEditor.VerifyNoDuplicates())
                return;

            let selectedDataMarts = this.QueryEditor.SelectedRoutings;
            if (selectedDataMarts.length == 0 && resultID != SaveResultID) {
                Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>A DataMart needs to be selected</p></div>');
                return;
            }

            Requests.Details.PromptForComment()
                .done((comment) => {

                    this.QueryEditor.updateRequestHeader(Requests.Details.rovm.Request.Name(), location.protocol + '//' + location.host + '/querycomposer/summaryview?ID=' + Requests.Details.rovm.Request.ID(), new Date());

                    let queryJSON = JSON.stringify(this.QueryEditor.exportRequestDTO());
                        
                    Requests.Details.rovm.Request.Query(queryJSON);
                    Requests.Details.rovm.Request.AdditionalInstructions(this.QueryEditor.AdditionalInstructions);

                    let dto = Requests.Details.rovm.Request.toData();

                    Dns.WebApi.Requests.CompleteActivity({
                        DemandActivityResultID: resultID,
                        Dto: dto,
                        DataMarts: selectedDataMarts,

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

        //Bind the view model for the activity
        let bindingControl = $("#DefaultCreateRequest");
        vm = new ViewModel(bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
        Requests.Details.rovm.SaveRequestID(SaveResultID);

    });
} 