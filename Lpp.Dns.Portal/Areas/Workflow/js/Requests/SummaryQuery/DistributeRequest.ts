/// <reference path="../../../../../js/requests/details.ts" />

module Workflow.SummaryQuery.DistributeRequest {
    export var vm: ViewModel;
    export var GetDataMartTimer;

    export class Routings {
        //binding fields here
        public Priority: KnockoutObservable<Dns.Enums.Priorities>;
        public DueDate: KnockoutObservable<Date>;
        public Name: string;
        public Organization: string;
        public OrganizationID: any;
        public DataMartID: any;
        public RequestID: any;

        private _existingRequestDataMart: Dns.Interfaces.IRequestDataMartDTO;
        public HasExistingRouting: boolean;

        public toRequestDataMartDTO: () => Dns.Interfaces.IRequestDataMartDTO;

        constructor(dataMart: Dns.Interfaces.IDataMartListDTO, existingRequestDataMart: Dns.Interfaces.IRequestDataMartDTO) {
            this.Priority = ko.observable(existingRequestDataMart != null ? existingRequestDataMart.Priority : dataMart.Priority);
            this.DueDate = ko.observable(existingRequestDataMart != null ? existingRequestDataMart.DueDate : dataMart.DueDate);

            this.Name = dataMart.Name;
            this.Organization = dataMart.Organization;
            this.OrganizationID = dataMart.OrganizationID;
            this.DataMartID = dataMart.ID;
            this._existingRequestDataMart = existingRequestDataMart;
            this.HasExistingRouting = this._existingRequestDataMart != null;

            var self = this;
            self.toRequestDataMartDTO = () => {

                var route: Dns.Interfaces.IRequestDataMartDTO = null;
                if (self._existingRequestDataMart != null) {
                    //do a deep copy clone of the existing routing information;
                    route = <Dns.Interfaces.IRequestDataMartDTO>jQuery.extend(true, {}, self._existingRequestDataMart);
                } else {
                    route = new Dns.ViewModels.RequestDataMartViewModel().toData();
                    route.DataMartID = self.DataMartID;
                    route.RequestID = self.RequestID;
                    route.DataMart = self.Name;
                }

                route.Priority = self.Priority();
                route.DueDate = self.DueDate();

                return route;
            };

        }
    }

    export class ViewModel extends Global.WorkflowActivityViewModel {
        public Request: Dns.ViewModels.QueryComposerRequestViewModel;

        public DataMarts: KnockoutObservableArray<Workflow.SummaryQuery.DistributeRequest.Routings>;
        public ExistingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[];
        public SelectedDataMartIDs: KnockoutObservableArray<any>;
        public DataMartAdditionalInstructions: KnockoutObservable<string>;

        private CanSubmit: KnockoutComputed<boolean>;
        private DataMartsSelectAll: () => void;
        private DataMartsClearAll: () => void;

        public DataMartsBulkEdit: () => void;

        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];
        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;

        
        constructor(query: Dns.Interfaces.IQueryComposerRequestDTO, routes: Routings[], fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[], additionalInstructions: string, bindingControl: JQuery) {
            super(bindingControl, Requests.Details.rovm.ScreenPermissions);

            this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(query);
            this.SelectedDataMartIDs = ko.observableArray(ko.utils.arrayMap(ko.utils.arrayFilter(routes, (rt) => rt.HasExistingRouting), (rdm) => rdm.DataMartID));        
            this.DataMarts = ko.observableArray(routes);
            this.DataMartAdditionalInstructions = ko.observable(additionalInstructions || '');

            var self = this;

            this.DataMartsBulkEdit = () => {

                Global.Helpers.ShowDialog("Edit Routings", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 500, 400, { defaultPriority: Requests.Details.rovm.Request.Priority(), defaultDueDate: Requests.Details.rovm.Request.DueDate() })
                    .done((result: any) => {
                        if (result != null) {
                            var priority: Dns.Enums.Priorities = null;
                            if (result.UpdatePriority) {
                                priority = result.PriorityValue;
                            }

                            if (result.UpdatePriority || result.UpdateDueDate) {
                                ko.utils.arrayFilter(self.DataMarts(), (route) => {
                                    return self.SelectedDataMartIDs.indexOf(route.DataMartID) > -1;
                                }).forEach((route) => {
                                    if (priority != null)
                                        route.Priority(priority);
                                    if (result.UpdateDueDate)
                                        route.DueDate(new Date(result.stringDate));
                                });
                            }
                        }
                    });

            };

            Requests.Details.rovm.RoutingsChanged.subscribe((info: any) => {
                var newPriority = info != null ? info.newPriority : null;
                var newDueDate = info != null ? info.newDueDate : null;

                this.DataMarts().forEach((dm) => {
                    if (newPriority != null) {
                        dm.Priority(newPriority);
                    }
                    if (newDueDate != null) {
                        dm.DueDate(newDueDate);
                    }
                });

            });

            this.DataMartsSelectAll = () => {
                var datamartIDs = ko.utils.arrayMap(self.DataMarts(), (rt) => rt.DataMartID);
                self.SelectedDataMartIDs(datamartIDs);
            };

            this.DataMartsClearAll = () => {
                self.SelectedDataMartIDs.removeAll();
            };

            this.CanSubmit = ko.computed(() => {
                return self.HasPermission(Permissions.ProjectRequestTypeWorkflowActivities.CloseTask) && self.SelectedDataMartIDs().length > 0;
            }); 

            self.FieldOptions = fieldOptions || [];

            self.IsFieldRequired = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions || [], (item) => { return item.FieldIdentifier == id; });
                return options != null && options.Permission != null && options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            self.IsFieldVisible = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions || [], (item) => { return item.FieldIdentifier == id; });
                return options == null || (options.Permission != null && options.Permission != Dns.Enums.FieldOptionPermissions.Hidden);
            };
            
        }

        public PostComplete(resultID: string) {
            if (!(typeof Plugins.Requests.QueryBuilder.Edit === "undefined") && Plugins.Requests.QueryBuilder.Edit.vm.fileUpload()) {

                var deleteFilesDeferred = $.Deferred().resolve();

                if (Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel != null && Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete().length > 0) {
                    deleteFilesDeferred = Dns.WebApi.Documents.Delete(ko.utils.arrayMap(Plugins.Requests.QueryBuilder.Edit.vm.UploadViewModel.DocumentsToDelete(), (d) => { return d.ID; }));
                }
                deleteFilesDeferred.done(() => {

                    if (!Requests.Details.rovm.Validate())
                        return;
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
                });

            } else {
                if (!Requests.Details.rovm.Validate())
                    return;

                var self = this;
                var selectedDataMarts = ko.utils.arrayMap(ko.utils.arrayFilter(this.DataMarts(), (route) => {
                    return self.SelectedDataMartIDs.indexOf(route.DataMartID) > -1;
                }), (route) => route.toRequestDataMartDTO());

                Requests.Details.PromptForComment()
                    .done((comment) => {

                        var dto = Requests.Details.rovm.Request.toData();
                        var additionalInstructions = $('#DataMarts_AdditionalInstructions').val();
                        dto.AdditionalInstructions = additionalInstructions;

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

    function init() {
        Requests.Details.rovm.SaveRequestID("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        
        Dns.WebApi.Requests.GetCompatibleDataMarts({
            TermIDs: null,
            ProjectID: Requests.Details.rovm.Request.ProjectID(),
            Request: Requests.Details.rovm.Request.Query(),
            RequestID: Requests.Details.rovm.Request.ID()
        }).done((dataMarts) => {
            
            var existingRequestDataMarts = ko.utils.arrayMap(Requests.Details.rovm.RequestDataMarts() || [], (dm: Requests.Details.RequestDataMartViewModel) => dm.toData());
            var routes = [];
            for (var di = 0; di < dataMarts.length; di++) {
                var dm = dataMarts[di];
                dm.Priority = Requests.Details.rovm.Request.Priority();
                dm.DueDate = Requests.Details.rovm.Request.DueDate();

                var existingRoute = ko.utils.arrayFirst(existingRequestDataMarts, (r) => r.DataMartID == dm.ID);

                routes.push(new Routings(dm, existingRoute));
            }

            $(() => {
                var bindingControl = $("#DefaultDistributeRequest");
                
                var queryData = Requests.Details.rovm.Request.Query() == null ? null : JSON.parse(Requests.Details.rovm.Request.Query());
                vm = new ViewModel(queryData,
                    routes,
                    Requests.Details.rovm.FieldOptions,
                    Requests.Details.rovm.Request.AdditionalInstructions(),
                    bindingControl);

                ko.applyBindings(vm, bindingControl[0]);

                var visualTerms = Requests.Details.rovm.VisualTerms;
                //Hook up the Query Composer readonly view
                Plugins.Requests.QueryBuilder.View.init(queryData, visualTerms, $('#QCreadonly'));
            });
        });
    }

    init();
}   