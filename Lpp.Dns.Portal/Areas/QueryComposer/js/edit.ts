/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="termvaluefilter.ts" />
/// <reference path="../../../js/requests/details.ts" />

module Plugins.Requests.QueryBuilder.Edit {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {

        private RequestDTO: Dns.Interfaces.IQueryComposerRequestDTO;
        private RequestTypeDTO: Dns.Interfaces.IRequestTypeDTO;
        private ProjectID: any;
        private RequestID: any;
        public IsFileUpload: boolean = false;
        private MDQ_Host: Plugins.Requests.QueryBuilder.QueryEditorHost = null;
        private FileUpload_Host: Controls.WFFileUpload.Index.ViewModel = null;
        private RoutingsEditor: Plugins.Requests.QueryBuilder.DataMartRouting.ViewModel;
        TermsObserver: Plugins.Requests.QueryBuilder.TermsObserver;
        private AttachmentsEditor: Controls.WFFileUpload.ForAttachments.ViewModel;

        get SelectedRoutings(): Dns.Interfaces.IRequestDataMartDTO[] {
            return this.RoutingsEditor.SelectedRoutings();
        }

        get AdditionalInstructions(): string {
            return this.RoutingsEditor.DataMartAdditionalInstructions();
        }

        /**
         * Initializes the edit container holding the QueryComposer editor
         * @param bindingControl The binding element for knockout.
         * @param requestDTO The QueryComposer RequestDTO.
         * @param requestID The ID of the actual request.
         * @param requestTypeDTO The request type DTO.
         */
        constructor(
            bindingControl: JQuery,
            requestDTO: Dns.Interfaces.IQueryComposerRequestDTO,
            projectID: any,
            requestID: any,
            requestTypeDTO: Dns.Interfaces.IRequestTypeDTO,
            requestTypeModelIDs: any[],
            requestTypeTerms: Dns.Interfaces.IRequestTypeTermDTO[],
            visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
            criteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[],
            hiddenTerms: Dns.Interfaces.ITemplateTermDTO[],
            routingsEditor: Plugins.Requests.QueryBuilder.DataMartRouting.ViewModel,
            attachmentsEditor: Controls.WFFileUpload.ForAttachments.ViewModel
        ) {
            super(bindingControl);
            let self = this;
            
            this.ProjectID = projectID;
            this.RequestID = requestID;
            this.RequestDTO = requestDTO;
            this.RequestTypeDTO = requestTypeDTO;
            this.RoutingsEditor = routingsEditor;
            this.AttachmentsEditor = attachmentsEditor;

            if (this.RequestDTO.SchemaVersion !== "2.0") {
                this.RequestDTO.SchemaVersion = "2.0";
            }

            this.TermsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();

            if (this.RequestDTO.Queries != null && this.RequestDTO.Queries.length > 0 &&
                (this.RequestDTO.Queries[0].Header.ComposerInterface == Dns.Enums.QueryComposerInterface.FileDistribution ||
                Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.QueryContainsTerm(this.RequestDTO.Queries[0], Plugins.Requests.QueryBuilder.MDQ.Terms.FileUploadID) ||
                Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.QueryContainsTerm(this.RequestDTO.Queries[0], Plugins.Requests.QueryBuilder.MDQ.Terms.ModularProgramID))
            ) {
                if (this.RequestDTO.Queries[0].Header.ComposerInterface != Dns.Enums.QueryComposerInterface.FileDistribution) {
                    this.RequestDTO.Queries[0].Header.ComposerInterface = Dns.Enums.QueryComposerInterface.FileDistribution;
                }
                this.IsFileUpload = true;
                this.FileUpload_Host = new Controls.WFFileUpload.Index.ViewModel($('#FileUploadControl'), this.ScreenPermissions, this.RequestDTO.Queries[0], QueryBuilder.MDQ.Terms.FileUploadID);
                let currentTerms = Plugins.Requests.QueryBuilder.MDQ.TermProvider.FlattenToAllTermIDs(this.RequestDTO.Queries[0]);
                this.TermsObserver.RegisterTermCollection(this.RequestDTO.Queries[0].Header.ID, ko.observableArray<any>(currentTerms));
            } else {

                let index = 0;
                let templateViewModels = ko.utils.arrayMap(this.RequestDTO.Queries, (query) => {
                    let template = new Dns.ViewModels.TemplateViewModel();
                    template.ID(query.Header.ID);
                    template.ComposerInterface(query.Header.ComposerInterface);
                    template.Data(JSON.stringify(query));
                    template.Description(query.Header.Description);
                    template.Name(query.Header.Name);
                    template.Order(index++);
                    template.QueryType(query.Header.QueryType);
                    template.RequestTypeID(requestTypeDTO.ID);
                    template.Type(Dns.Enums.TemplateTypes.Request);
                    return template;
                }); 

                let initParams: Plugins.Requests.QueryBuilder.QueryEditorHostInitializationParameters = {
                    IsTemplateEdit: false,
                    RequestTypeModelIDs: ko.observableArray(requestTypeModelIDs),
                    RequestTypeTerms: ko.observableArray<Dns.ViewModels.RequestTypeTermViewModel>(requestTypeTerms.map((item) => {
                        return new Dns.ViewModels.RequestTypeTermViewModel(item);
                    })),
                    Templates: templateViewModels,
                    TemplateType: Dns.Enums.TemplateTypes.Request,
                    CriteriaGroupTemplates: criteriaGroupTemplates,
                    VisualTerms: visualTerms,
                    HiddenTerms: hiddenTerms,
                    SupportsMultiQuery: ko.observable<boolean>(this.RequestTypeDTO.SupportMultiQuery),
                    TermsObserver: this.TermsObserver
                };

                this.MDQ_Host = new Plugins.Requests.QueryBuilder.QueryEditorHost(initParams);
            }

            this.TermsObserver.DistinctTerms.subscribe((newValue) => {
                this.RoutingsEditor.LoadDataMarts(this.ProjectID, newValue);
            }, this);

            this.RoutingsEditor.LoadDataMarts(this.ProjectID, this.TermsObserver.DistinctTerms());
        }

        public Validate(): boolean {
            if (this.MDQ_Host != null) {
                //check that there are no empty criteria groups
                let emptyCriteriaGroupQuery = ko.utils.arrayFirst(this.MDQ_Host.ExportQueries(), (query) => {
                    if (ko.utils.arrayFirst(query.Where.Criteria, (criteria) => { return (criteria.Terms == null || criteria.Terms.length == 0) && (criteria.Criteria == null || criteria.Criteria.length == 0); }) != null) {
                        return true;
                    }
                    return false;
                });

                if (emptyCriteriaGroupQuery != null) {
                    Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The Criteria Group cannot be empty.</p></div>');
                    return false;
                }

                if (!this.MDQ_Host.onValidateEditors()) {
                    return false;
                }

                super.Validate();
            }

            return true;
        }

        public VerifyNoDuplicates(): boolean {
            if (this.MDQ_Host != null) {
                return this.MDQ_Host.VerifyNoDuplicates();
            }
            //file distribution only allows for a single cohort.
            return true;
        }

        public fileUploadDMLoad() {
            this.RoutingsEditor.LoadDataMarts(this.ProjectID, this.TermsObserver.DistinctTerms());
        }

        public UpdateRoutings(updates) {
            this.RoutingsEditor.UpdateRoutings(updates);
        }

        public bindComponents() {
            if (this.MDQ_Host != null) {
                this.MDQ_Host.onKnockoutBind();
            } else {
                this.FileUpload_Host.onKnockoutBind();
            }
        }

        public exportRequestDTO(): Dns.Interfaces.IQueryComposerRequestDTO {
            //clone the requestDTO object, and update the query dto's
            let requestDTO = JSON.parse(JSON.stringify(this.RequestDTO)) as Dns.Interfaces.IQueryComposerRequestDTO;
            requestDTO.Queries = this.MDQ_Host == null ? this.FileUpload_Host.ExportQueries() : this.MDQ_Host.ExportQueries();

            return requestDTO;
        }

        public updateRequestHeader(name: string, viewUrl: string, submittedOn?: Date) {
            this.RequestDTO.Header.Name = name;
            this.RequestDTO.Header.ViewUrl = viewUrl;
            this.RequestDTO.Header.SubmittedOn = submittedOn;
        }


    }


    export function init(
        requestDTO: Dns.Interfaces.IQueryComposerRequestDTO,
        fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
        defaultPriority: Dns.Enums.Priorities,
        defaultDueDate: Date,
        additionalInstructions: string,
        existingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
        requestTypeDTO: Dns.Interfaces.IRequestTypeDTO,
        projectID: any = Global.GetQueryParam("projectID"),
        requestID: any
    ): JQueryPromise<Plugins.Requests.QueryBuilder.Edit.ViewModel> {
        let promise = $.Deferred<Plugins.Requests.QueryBuilder.Edit.ViewModel>();
        
        $.when<any>(
            Dns.WebApi.RequestTypes.GetRequestTypeModels(requestTypeDTO.ID, null, 'DataModelID'),
            Dns.WebApi.RequestTypes.GetRequestTypeTerms(requestTypeDTO.ID),
            Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(),
            Dns.WebApi.Templates.CriteriaGroups(),
            Dns.WebApi.Templates.ListHiddenTermsByRequestType(requestTypeDTO.ID),
            Controls.WFFileUpload.ForAttachments.init($('#attachments_upload'), true)
        ).done((
            requestTypeModelIDs,
            requestTypeTerms: Dns.Interfaces.IRequestTypeTermDTO[],
            visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
            criteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[],
            hiddenTerms: Dns.Interfaces.ITemplateTermDTO[],
            attachmentsVM: Controls.WFFileUpload.ForAttachments.ViewModel
        ) => {

            $(() => {

                let routingsVM = Plugins.Requests.QueryBuilder.DataMartRouting.init(fieldOptions, existingRequestDataMarts, defaultDueDate, defaultPriority, additionalInstructions);
                
                let bindingControl = $('#ComposerControl');
                vm = new ViewModel(bindingControl, requestDTO, projectID, requestID, requestTypeDTO, (requestTypeModelIDs || []).map((m) => m.DataModelID), requestTypeTerms, visualTerms, criteriaGroupTemplates, hiddenTerms, routingsVM, attachmentsVM);

                if (vm.IsFileUpload == false) {
                    $('#FileUploadContainer').remove();
                } else {
                    $('#QueryEditorHostContainer').remove();
                }

                ko.applyBindings(vm, bindingControl[0]);
                vm.bindComponents();
                routingsVM.onKnockoutBind();

                promise.resolve(vm);
            });

        });

        return promise;
    }


}


