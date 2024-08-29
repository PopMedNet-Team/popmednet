﻿/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.ModularProgram.DistributeRequest {
    var vm: ViewModel;

    export class Routings {
        //binding fields here
        public Priority: KnockoutObservable<Dns.Enums.Priorities>;
        public DueDate: KnockoutObservable<Date>;
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

        }
    }

    export class ViewModel extends Global.WorkflowActivityViewModel {

        public DataMarts: KnockoutObservableArray<Workflow.ModularProgram.DistributeRequest.Routings>;
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

        constructor(bindingControl: JQuery, screenPermissions: any[], fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[], datamarts: Dns.Interfaces.IDataMartListDTO[], existingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[], uploadViewModel: Controls.WFFileUpload.Index.ViewModel) {
            super(bindingControl, screenPermissions);

            this.ExistingRequestDataMarts = existingRequestDataMarts || [];
            this.SelectedDataMartIDs = ko.observableArray(ko.utils.arrayMap(existingRequestDataMarts, (i) => { return i.DataMartID; }));
            this.DataMarts = ko.observableArray([]); 
            this.UploadViewModel = uploadViewModel;            

            var self = this;

            datamarts.forEach((dm) => {
                var dataMart = new Workflow.ModularProgram.DistributeRequest.Routings(dm);
                dataMart.DueDate(Requests.Details.rovm.Request.DueDate());
                dataMart.Priority(Requests.Details.rovm.Request.Priority());
                self.DataMarts.push(dataMart);
            });

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
                return self.HasPermission(PMNPermissions.ProjectRequestTypeWorkflowActivities.CloseTask) && self.SelectedDataMartIDs().length > 0;
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
        }

        public PostComplete(resultID: string) {
            var uploadCriteria = vm.UploadViewModel.serializeCriteria();
            Requests.Details.rovm.Request.Query(uploadCriteria);
            var AdditionalInstructions = $('#DataMarts_AdditionalInstructions').val()
            var dto = Requests.Details.rovm.Request.toData()
            dto.AdditionalInstructions = AdditionalInstructions;

            var requestDataMarts = ko.utils.arrayMap(vm.SelectedDataMartIDs(), (datamartID) => {

                var rdm = ko.utils.arrayFirst(vm.ExistingRequestDataMarts, (d) => { return d.DataMartID == datamartID; });
                if (rdm)
                    return rdm;

                var dm = (new Dns.ViewModels.RequestDataMartViewModel()).toData();
                vm.DataMarts().forEach((d) => {
                    if (d.DataMartID == datamartID) {
                        dm.DueDate = d.DueDate();
                        dm.Priority = d.Priority();
                    }
                });
                dm.RequestID = Requests.Details.rovm.Request.ID();
                dm.DataMartID = datamartID;

                return dm;
            });

            Requests.Details.PromptForComment()
                .done((comment) => {
                    Dns.WebApi.Requests.CompleteActivity({
                        DemandActivityResultID: resultID,
                        Dto: dto ,
                        DataMarts: requestDataMarts,
                        Data: JSON.stringify(ko.utils.arrayMap(vm.UploadViewModel.Documents(), (d) => { return d.RevisionSetID; })),
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

    var modularProgramTermID = 'a1ae0001-e5b4-46d2-9fad-a3d8014fffd8';
    $.when<any>(
        Dns.WebApi.Requests.GetCompatibleDataMarts({ TermIDs: [modularProgramTermID], ProjectID: Requests.Details.rovm.Request.ProjectID(), Request: "", RequestID: Requests.Details.rovm.Request.ID() }),
        Dns.WebApi.Requests.RequestDataMarts(Requests.Details.rovm.Request.ID())
    )
        .done((datamarts, selectedDataMarts) => {
            Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
            var query = (Requests.Details.rovm.Request.Query() == null || Requests.Details.rovm.Request.Query() === '') ? null : JSON.parse(Requests.Details.rovm.Request.Query());
            var uploadViewModel = Controls.WFFileUpload.Index.init($('#mpupload'), query, modularProgramTermID);

            //Bind the view model for the activity
            var bindingControl = $("#MPDistributeRequest");
            vm = new ViewModel(bindingControl, Requests.Details.rovm.ScreenPermissions, Requests.Details.rovm.FieldOptions, datamarts, selectedDataMarts || [], uploadViewModel);

        $(() => {            
            ko.applyBindings(vm, bindingControl[0]);            
        });
    });

}