/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.DistributedRegression.Distribution {
    var vm: ViewModel;

    export class Routings {
        //binding fields here
        public Priority: KnockoutObservable<Dns.Enums.Priorities>;
        public DueDate: KnockoutObservable<Date>;
        public RoutingType: KnockoutObservable<Dns.Enums.RoutingType>
        public Selected: boolean;
        public Name: string;
        public Organization: string;
        public OrganizationID: any;
        public DataMartID: any;
        public RequestID: any;

        constructor(dataMart: Dns.Interfaces.IDataMartListDTO) {
            this.Priority = ko.observable(dataMart.Priority);
            this.DueDate = ko.observable(dataMart.DueDate);
            this.Name = dataMart.Name;
            this.Organization = dataMart.Organization;
            this.OrganizationID = dataMart.OrganizationID;
            this.DataMartID = dataMart.ID;
            this.RoutingType = ko.observable(null);
        }
    }

    export class ViewModel extends Global.WorkflowActivityViewModel {

        public DataMarts: KnockoutObservableArray<Workflow.DistributedRegression.Distribution.Routings>;
        public SelectedDataMartIDs: KnockoutObservableArray<any>;
        public DataMartsBulkEdit: () => void;
        private ExistingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[];
        private UploadViewModel: Controls.WFFileUpload.Index.ViewModel;
        private CanSubmit: KnockoutComputed<boolean>;
        private DataMartsSelectAll: () => boolean;
        private DataMartsClearAll: () => boolean;
        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];
        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;
        public AttachmentsVM: Controls.WFFileUpload.ForAttachments.ViewModel;

        constructor(bindingControl: JQuery, screenPermissions: any[], fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[], datamarts: Dns.Interfaces.IDataMartListDTO[], existingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[], uploadViewModel: Controls.WFFileUpload.Index.ViewModel) {
            super(bindingControl, screenPermissions);

            this.ExistingRequestDataMarts = existingRequestDataMarts || [];
            this.SelectedDataMartIDs = ko.observableArray(ko.utils.arrayMap(existingRequestDataMarts, (i) => { return i.DataMartID; }));
            this.DataMarts = ko.observableArray([]); 
            this.UploadViewModel = uploadViewModel;            

            var self = this;
            if (datamarts != null) {
                datamarts.forEach((dm) => {
                    var dataMart = new Workflow.DistributedRegression.Distribution.Routings(dm);
                    dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                    dataMart.Priority(Requests.Details.rovm.Request.Priority());
                    var selectedDM = ko.utils.arrayFirst(existingRequestDataMarts, (item) => {
                        return item.DataMartID == dataMart.DataMartID;
                    });
                    if (selectedDM != null)
                        dataMart.RoutingType(selectedDM.RoutingType);
                    self.DataMarts.push(dataMart);
                });
            }
            this.DataMartsBulkEdit = () => {

                Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: Requests.Details.rovm.Request.Priority(), defaultDueDate: Requests.Details.rovm.Request.DueDate() })
                    .done((result: any) => {
                        if (result != null) {
                            var newPriority: Dns.Enums.Priorities;
                            if (result.UpdatePriority) {
                                newPriority = result.PriorityValue;
                            };

                            var newDueDate: Date = new Date(result.stringDate);
                            if (!result.UpdateDueDate) {
                                newDueDate = null;
                            }

                            // update selected datamarts here
                            self.DataMarts().forEach((dm) => {
                                if (self.SelectedDataMartIDs().indexOf(dm.DataMartID) != -1) {
                                    if (result.UpdatePriority) {
                                        dm.Priority(newPriority);
                                    }
                                    if (result.UpdateDueDate) {
                                        dm.DueDate(newDueDate);
                                    }
                                }
                            });
                        }
                    });

            };

            Requests.Details.rovm.RoutingsChanged.subscribe((info: any) => {
                var newPriority = info != null ? info.newPriority : null;
                var newDueDate = info != null ? info.newDueDate : null;
                if (newPriority != null) {
                    var requestDataMarts = this.DataMarts();
                    var updatedDataMarts = [];
                    requestDataMarts.forEach((rdm) => {
                        rdm.Priority(newPriority);
                        updatedDataMarts.push(rdm);
                    });
                    this.DataMarts.removeAll();
                    this.DataMarts(updatedDataMarts);
                }
                if (newDueDate != null) {
                    var requestDataMarts = this.DataMarts();
                    var updatedDataMarts = [];
                    requestDataMarts.forEach((rdm) => {
                        rdm.DueDate(newDueDate);
                        updatedDataMarts.push(rdm);
                    });
                    this.DataMarts.removeAll();
                    this.DataMarts(updatedDataMarts);
                }

            });

            this.DataMartsSelectAll = () => {
                self.DataMarts().forEach((dm) => {
                    if (self.SelectedDataMartIDs.indexOf(dm.DataMartID) < 0)
                        self.SelectedDataMartIDs.push(dm.DataMartID);
                });

                return false;
            };

            this.DataMartsClearAll = () => {
                self.SelectedDataMartIDs.removeAll();
                return false;
            };

            this.CanSubmit = ko.computed(() => {
                return self.HasPermission(Permissions.ProjectRequestTypeWorkflowActivities.CloseTask) && self.SelectedDataMartIDs().length > 0;
            }); 

            this.FieldOptions = fieldOptions;

            self.IsFieldRequired = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions, (item) => { return item.FieldIdentifier == id; });
                return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            self.IsFieldVisible = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions, (item) => { return item.FieldIdentifier == id; });
                return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
            };

            Requests.Details.rovm.Request.ID.subscribe(newVal => {
                $.when<any>(
                    newVal != null ? Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }) : null,
                    newVal != null ? Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID()) : null
                ).done((datamarts: Dns.Interfaces.IDataMartListDTO[], selectedDataMarts: Dns.Interfaces.IRequestDataMartDTO[]) => {
                    self.DataMarts.removeAll();
                    datamarts.forEach((dm) => {
                        var dataMart = new Workflow.DistributedRegression.Distribution.Routings(dm);
                        dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                        dataMart.Priority(Requests.Details.rovm.Request.Priority());
                        var selectedDM = ko.utils.arrayFirst(selectedDataMarts, (item) => {
                            return item.DataMartID == dataMart.DataMartID;
                        });
                        if (selectedDM != null)
                            dataMart.RoutingType(selectedDM.RoutingType);
                        self.DataMarts.push(dataMart);
                    });
                   
                    self.ExistingRequestDataMarts = selectedDataMarts
                    self.SelectedDataMartIDs.removeAll();
                    self.SelectedDataMartIDs(ko.utils.arrayMap(selectedDataMarts, (i) => { return i.DataMartID; }));
                    var query = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                    var uploadViewModel = Controls.WFFileUpload.Index.init($('#DRUpload'), query, modularProgramTermID);
                    self.UploadViewModel = uploadViewModel;
                    Controls.WFFileUpload.ForAttachments.init($('#attachments_upload'), true).done((viewModel) => {
                        self.AttachmentsVM = viewModel;
                    })
                });

            });
        }

          public PostComplete(resultID: string) {
            let self = this;
            var uploadCriteria = vm.UploadViewModel.serializeCriteria();
            Requests.Details.rovm.Request.Query(uploadCriteria);
            var AdditionalInstructions = $('#DataMarts_AdditionalInstructions').val()
            var dto = Requests.Details.rovm.Request.toData()
            dto.AdditionalInstructions = AdditionalInstructions;
            var acCount: number = 0;
            var dpCount: number = 0
            var requestDataMarts = ko.utils.arrayMap(vm.SelectedDataMartIDs(), (datamartID) => {
                var dm = (new Dns.ViewModels.RequestDataMartViewModel()).toData();
                vm.DataMarts().forEach((d) => {
                    if (d.DataMartID == datamartID) {
                        dm.DueDate = d.DueDate();
                        dm.Priority = d.Priority();
                        dm.RoutingType = d.RoutingType();
                        if (d.RoutingType() == Dns.Enums.RoutingType.AnalysisCenter)
                            acCount++;
                        else if (d.RoutingType() == Dns.Enums.RoutingType.DataPartner)
                            dpCount++;
                    }
                   
                });
                dm.RequestID = Requests.Details.rovm.Request.ID();
                dm.DataMartID = datamartID;
               
                return dm;
            });
            var badDms = ko.utils.arrayFilter(requestDataMarts, (rdm) => {
                return rdm.RoutingType == undefined || rdm.RoutingType == null || rdm.RoutingType == -1
            });
          if (badDms.length == 0 && acCount == 1 && dpCount > 0) {
            if (self.UploadViewModel.Documents().length === 0) {
                Global.Helpers.ShowConfirm("No Documents Uploaded", "<p>No documents have been uploaded.  Do you want to continue submitting the request?").done(() => {
                  Dns.WebApi.Requests.CompleteActivity({
                    DemandActivityResultID: resultID,
                    Dto: dto,
                    DataMarts: requestDataMarts,
                    Data: JSON.stringify(ko.utils.arrayMap(vm.UploadViewModel.Documents(), (d) => { return d.RevisionSetID; })),
                    Comment: null
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
                }).fail(() => { return; });
              }
              else {
                Dns.WebApi.Requests.CompleteActivity({
                  DemandActivityResultID: resultID,
                  Dto: dto,
                  DataMarts: requestDataMarts,
                  Data: JSON.stringify(ko.utils.arrayMap(vm.UploadViewModel.Documents(), (d) => { return d.RevisionSetID; })),
                  Comment: null
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
              }
            }
            else
            {
                if (badDms.length > 0)
                    Global.Helpers.ShowAlert('DataMart Type Not Selected', '<p>Please Verify that all selected DataMarts have their Type specified</p>');
                else if (acCount == 0 || acCount > 1)
                    Global.Helpers.ShowAlert('Analysis Center Type Selection Error', '<p>Please Verify that only 1 DataMart is selected to Analysis Center</p>');
                else if (dpCount == 0)
                    Global.Helpers.ShowAlert('DataPartner Type Selection Error', '<p>Please Verify that a Data Partner is selected.</p>');
            }
            
        }


    }

    var modularProgramTermID = 'a1ae0001-e5b4-46d2-9fad-a3d8014fffd8';
    $.when<any>(
        Requests.Details.rovm.Request.ID() != null ? Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }) : null,
        Requests.Details.rovm.Request.ID() != null ? Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID()): null
    )
        .done((datamarts, selectedDataMarts) => {

            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            var query = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
            var uploadViewModel = Requests.Details.rovm.Request.ID() != null ? Controls.WFFileUpload.Index.init($('#DRUpload'), query, modularProgramTermID) : null;
            //Bind the view model for the activity
            var bindingControl = $("#DRDistribution");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Requests.Details.rovm.FieldOptions, datamarts, selectedDataMarts || [], uploadViewModel);

            if (Requests.Details.rovm.Request.ID() != null) {
                Controls.WFFileUpload.ForAttachments.init($('#attachments_upload'), true).done((viewModel) => {
                    vm.AttachmentsVM = viewModel;
                });
            }
            
        $(() => {            
            ko.applyBindings(vm, bindingControl[0]);
        });
    });

}