/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.Default.CreateRequest {
    var vm: ViewModel;

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public Request: Dns.ViewModels.RequestViewModel;

        constructor(bindingControl: JQuery) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions);
            
            this.Request = Requests.Details.rovm.Request;
            Requests.Details.rovm.RoutingsChanged.subscribe((info: any) => {
                //call function on the composer to update routing info
                Plugins.Requests.QueryBuilder.Edit.vm.UpdateRoutings(info);
            });
        }

        public PostComplete(resultID: string) {
            if (Plugins.Requests.QueryBuilder.Edit.vm.fileUpload()) {

                var deleteFilesDeferred = $.Deferred().resolve();

                if (Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel != null && Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete().length > 0) {
                    deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete(), (d) => { return d.ID; }));
                }
              deleteFilesDeferred.done(() => {
                  if (!Requests.Details.rovm.Validate())
                    return;
                  if (Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.Documents().length === 0) {
                      Global.Helpers.ShowConfirm("No Documents Uploaded", "<p>No documents have been uploaded.  Do you want to continue submitting the request?").done(() => {
                    var selectedDataMartIDs = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedDataMartIDs();
                    if (selectedDataMartIDs.length == 0 && resultID != "DFF3000B-B076-4D07-8D83-05EDE3636F4D") {
                      Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>A DataMart needs to be selected</p></div>');
                      return;
                    }

                    var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();
                    var uploadCriteria = Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.serializeCriteria();
                    Requests.Details.rovm.Request.Query(uploadCriteria);

                    var AdditionalInstructions = $('#DataMarts_AdditionalInstructions').val()
                    var dto = Requests.Details.rovm.Request.toData()
                    dto.AdditionalInstructions = AdditionalInstructions;

                    Requests.Details.PromptForComment().done((comment) => {
                      Dns.WebApi.Requests.CompleteActivity({
                        DemandActivityResultID: resultID,
                        Dto: dto,
                        DataMarts: selectedDataMarts,
                        Data: JSON.stringify(ko.utils.arrayMap(Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.Documents(), (d) => { return d.RevisionSetID; })),
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
                      var selectedDataMartIDs = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedDataMartIDs();
                      if (selectedDataMartIDs.length == 0 && resultID != "DFF3000B-B076-4D07-8D83-05EDE3636F4D") {
                        Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>A DataMart needs to be selected</p></div>');
                        return;
                      }

                      var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();
                      var uploadCriteria = Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.serializeCriteria();
                      Requests.Details.rovm.Request.Query(uploadCriteria);

                      var AdditionalInstructions = $('#DataMarts_AdditionalInstructions').val()
                      var dto = Requests.Details.rovm.Request.toData()
                      dto.AdditionalInstructions = AdditionalInstructions;

                      Requests.Details.PromptForComment().done((comment) => {
                        Dns.WebApi.Requests.CompleteActivity({
                          DemandActivityResultID: resultID,
                          Dto: dto,
                          DataMarts: selectedDataMarts,
                          Data: JSON.stringify(ko.utils.arrayMap(Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.Documents(), (d) => { return d.RevisionSetID; })),
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

            } else {

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

                if (!Plugins.Requests.QueryBuilder.MDQ.vm.AreTermsValid())
                    return;

                var selectedDataMartIDs = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedDataMartIDs();
                if (selectedDataMartIDs.length == 0 && resultID != "DFF3000B-B076-4D07-8D83-05EDE3636F4D") {
                    Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>A DataMart needs to be selected</p></div>');
                    return;
                }

                var selectedDataMarts = Plugins.Requests.QueryBuilder.DataMartRouting.vm.SelectedRoutings();


                Requests.Details.PromptForComment()
                    .done((comment) => {

                        Plugins.Requests.QueryBuilder.MDQ.vm.Request.Header.Name(Requests.Details.rovm.Request.Name());
                        Plugins.Requests.QueryBuilder.MDQ.vm.Request.Header.ViewUrl(location.protocol + '//' + location.host + '/querycomposer/summaryview?ID=' + Requests.Details.rovm.Request.ID());
                        Plugins.Requests.QueryBuilder.MDQ.vm.Request.Header.SubmittedOn(new Date());

                        Requests.Details.rovm.Request.Query(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
                        Requests.Details.rovm.Request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions());

                        var dto = Requests.Details.rovm.Request.toData();

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
    }

    $(() => {

        //Bind the view model for the activity
        var bindingControl = $("#DefaultCreateRequest");
        vm = new ViewModel(bindingControl);
        ko.applyBindings(vm, bindingControl[0]);
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        //Hook up the Query Composer
        var queryData = Requests.Details.rovm.Request.Query() == null ? null : JSON.parse(Requests.Details.rovm.Request.Query());
        var visualTerms = Requests.Details.rovm.VisualTerms;
        Plugins.Requests.QueryBuilder.Edit.init(
            queryData,
            Requests.Details.rovm.FieldOptions,
            Requests.Details.rovm.Request.Priority(),
            Requests.Details.rovm.Request.DueDate(),
            Requests.Details.rovm.Request.AdditionalInstructions(),
            ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], (dm: Requests.Details.RequestDataMartViewModel) => <Dns.Interfaces.IRequestDataMartDTO>{ DataMart: dm.DataMart(), DataMartID: dm.DataMartID(), RequestID: dm.RequestID(), ID: dm.ID(), Status: dm.Status(), Priority: dm.Priority(), DueDate: dm.DueDate(), ErrorMessage: null, ErrorDetail: null, RejectReason: null, Properties: null, ResponseGroup: null, ResponseMessage: null}),
            
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
                        request.AdditionalInstructions(Plugins.Requests.QueryBuilder.DataMartRouting.vm.DataMartAdditionalInstructions());
                        return true;
                    });
                }
            });

    });
} 