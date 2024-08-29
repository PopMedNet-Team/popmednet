module Plugins.Requests.QueryBuilder.MDQ {

    /** Manages the available terms, and term values for the MDQ designer. */
    export class TermProvider {
        /**
         * Initializes the term manager with the initial collections of Model IDs and Restricted Term IDs
         * @param visualTerms The collection of visual terms defined for the application.
         * @param adapterIDs The initial collection of adapter ID's to limit the terms to.
         * @param restrictToTermIDs The initial collection of specific terms provided by the request type.
         * @param adapterDetail The observable of the adapter detail the term must support.
         */
        constructor(visualTerms: IVisualTerm[], adapterIDs: any[], restrictToTermIDs: any[], adapterDetail: KnockoutObservable<Dns.Enums.QueryComposerQueryTypes>) {
            this._visualTerms = visualTerms;
            this._adapterDetail = adapterDetail;
            this._adapterIDs = adapterIDs;
            this._restrictToTermIDs = restrictToTermIDs;
            this.TermValueFilter = new MDQ.TermValueFilter(this._adapterIDs);
            this.SexForCritieria = this.TermValueFilter.SexValues;
            this.SettingForCritieria = this.TermValueFilter.SettingsValues;
            this.RaceEthnicityForCritieria = this.TermValueFilter.RaceEthnicityValues;
            this.AgeGroupForStratification = this.TermValueFilter.AgeRangeStratifications;
            this.ShowAgeRangeCalculationSelections = ko.observable(this.TermValueFilter.HasModel(MDQ.TermValueFilter.PCORnetModelID));

            this.CanImportCodeList = ko.pureComputed(this.hasDiagnosisOrProcedureTerm.bind(this));

            this.FilteredTerms = ko.observableArray<IVisualTerm>().extend({ deferred: true});
        }

        TermValueFilter: MDQ.TermValueFilter;
        SexForCritieria: KnockoutComputed<Dns.Structures.KeyValuePair[]>;
        SettingForCritieria: KnockoutComputed<Dns.Structures.KeyValuePair[]>;
        RaceEthnicityForCritieria: KnockoutComputed<Dns.Structures.KeyValuePair[]>;
        AgeGroupForStratification: KnockoutComputed<Dns.Structures.KeyValuePair[]>;
        ShowAgeRangeCalculationSelections: KnockoutObservable<boolean>;
        AvailableOrganizations: KnockoutObservableArray<Dns.Interfaces.IOrganizationDTO> = ko.observableArray<Dns.Interfaces.IOrganizationDTO>([]);
        CanImportCodeList: KnockoutComputed<boolean>;

        private readonly _visualTerms: IVisualTerm[];
        private readonly _adapterIDs: any[];
        private _restrictToTermIDs: any[];
        private readonly _adapterDetail: KnockoutObservable<Dns.Enums.QueryComposerQueryTypes>;

        public setRestrictedTermIDs(termIDs: any[]): void {
            this._restrictToTermIDs = termIDs;
        }

        /** The collection of terms available based on the specified parameters within the TermProvider */
        public FilteredTerms: KnockoutObservableArray<IVisualTerm>;

        /** Refreshes the available terms (FilteredTerms) based on the specified parameters within the TermProvider */
        Refresh() {
            if (this._visualTerms == null || this._visualTerms.length == 0) {
                this.FilteredTerms([]);
                return;
            }

            let self = this;
            
            self.TermValueFilter.UpdateModels(self._adapterIDs);
            self.ShowAgeRangeCalculationSelections(self.TermValueFilter.HasModel(MDQ.TermValueFilter.PCORnetModelID));

            if (self._restrictToTermIDs != null && self._restrictToTermIDs.length > 0) {
                
                let specificTerms: IVisualTerm[] = [];

                //build the list of visual terms available based on the specified terms
                self._visualTerms.forEach((t) => {
                    //if the term does not have any children and exists in the specific terms list add it
                    if ((t.Terms == null || t.Terms.length == 0) && self._restrictToTermIDs.indexOf(t.TermID) >= 0) {
                        specificTerms.push(t);
                    } else if (t.Terms != null && t.Terms.length > 0) {
                        //it is a category term, check if any of the child terms match
                        //clone the category, and build the child terms collection based on matching terms
                        let termCategory: IVisualTerm = { Description: t.Description, Design: null, IncludeInCriteria: t.IncludeInCriteria, IncludeInProjectors: t.IncludeInProjectors, IncludeInStratifiers: t.IncludeInStratifiers, Name: t.Name, TermID: t.TermID, ValueTemplate: t.ValueTemplate, Terms: [] };
                        t.Terms.forEach((childTerm) => {

                            if (self._restrictToTermIDs.indexOf(childTerm.TermID) >= 0) {
                                termCategory.Terms.push(childTerm);
                            }

                        });

                        if (termCategory.Terms.length > 0) {
                            specificTerms.push(termCategory);
                        }
                    }
                });

                self.FilteredTerms(specificTerms);
                return;
            }

            Dns.WebApi.RequestTypes.TermsByAdapterAndDetail({ Adapters: self._adapterIDs, QueryType: self._adapterDetail() }, true)
                .done((result) => {
                    let availableTermIDs = (result || []);
                    let termList: IVisualTerm[] = [];

                    let hasSummaryModel = false;
                    if ((self._adapterIDs.length == 1 && MDQ.TermValueFilter.ContainsModel(self._adapterIDs, MDQ.TermValueFilter.SummaryTablesModelID)) || self._adapterIDs.length == 0) {
                        hasSummaryModel = true;
                    }

                    self._visualTerms.forEach((t) => {
                        if ((t.Terms == null || t.Terms.length == 0) && availableTermIDs.indexOf(t.TermID.toLowerCase()) >= 0) {
                            termList.push(t);
                        }
                        if (t.Terms != null && t.Terms.length > 0) {
                            //the term is a category header, clone the category term and then add the child terms that are in the available terms list
                            let termCategory: IVisualTerm = { Description: t.Description, Design: null, IncludeInCriteria: t.IncludeInCriteria, IncludeInProjectors: t.IncludeInProjectors, IncludeInStratifiers: t.IncludeInStratifiers, Name: t.Name, TermID: t.TermID, ValueTemplate: t.ValueTemplate, Terms: [] };
                            t.Terms.forEach((childTerm) => {

                                //if the specified models is only summary tables, age range should not be available as a criteria, only stratifier
                                if (MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.AgeRangeID)) {
                                    //childTerm.IncludeInCriteria = options.IsTemplateEdit ? !(hasSummaryModel && RequestType.Details.vm.SelectedModels().length == 1) : !hasSummaryModel;
                                    childTerm.IncludeInCriteria = true;
                                }

                                //for Coverage, Drug Class, Drug Name, HCPCS, and all the ICD-9 code terms, stratification is only applicable for Summary Tables Model
                                //They are key indicator terms that are required by the adapter to indicate the type of summary query.
                                if (MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.CoverageID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.DrugClassID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.DrugNameID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.HCPCSProcedureCodesID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.ICD9Diagnosis3digitID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.ICD9Diagnosis4digitID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.ICD9Diagnosis5digitID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.ICD9Procedure3digitID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.ICD9Procedure4digitID) ||
                                    MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.MetadataRefreshID)
                                ) {
                                    childTerm.IncludeInStratifiers = hasSummaryModel;
                                }

                                if (availableTermIDs.indexOf(childTerm.TermID.toLowerCase()) >= 0) {
                                    termCategory.Terms.push(childTerm);
                                }

                            });

                            if (termCategory.Terms.length > 0) {
                                termList.push(termCategory);
                            }
                        }
                    });

                    self.FilteredTerms(termList);
                })
                .fail((err) => {
                    debugger;
                });

        }

        private hasDiagnosisOrProcedureTerm() {
            let diagnosisTerm = ko.utils.arrayFirst(this.FilteredTerms(), (term) => {
                return MDQ.Terms.Compare(term.TermID, MDQ.Terms.CombinedDiagnosisCodesID) && term.IncludeInCriteria;
            });

            let procedureTerm = ko.utils.arrayFirst(this.FilteredTerms(), (term) => {
                return MDQ.Terms.Compare(term.TermID, MDQ.Terms.ProcedureCodesID) && term.IncludeInCriteria;
            });

            return (diagnosisTerm != null || procedureTerm != null);
        }

        public static GetVisualTerms(): JQueryDeferred<Plugins.Requests.QueryBuilder.IVisualTerm[]> {
            let d = $.Deferred<Plugins.Requests.QueryBuilder.IVisualTerm[]>();

            $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
                .done((result: Plugins.Requests.QueryBuilder.IVisualTerm[]) => {
                    d.resolve(result);
                }).fail((e, description, error) => {
                    d.reject(<any>e);
                });

            return d;
        }

        /**
         * Inspects the provided queries and returns a collection of distinct term ID's for all the terms used.
         * @param queries The collection of queries to inspect.
         */
        public static FlattenToDistintTermIDs(queries: Dns.Interfaces.IQueryComposerQueryDTO[]): any[] {
            let allTerms: any[] = [];

            for (let i = 0; i < queries.length; i++) {
                TermProvider.GetTermIDsFromCriteria(queries[i].Where.Criteria).forEach((item) => allTerms.push(item));
            }

            //filter out the value if it equals the preceding value
            let distinctTerms = allTerms.sort().filter((value, index, src) => { return !index || value != src[index - 1]; });
            return distinctTerms;
        }

        /**
         * Returns a collection containing all of the ID's for each term contained within the query, values may be duplicated.
         * @param query The query to inspect.
         */
        public static FlattenToAllTermIDs(query: Dns.Interfaces.IQueryComposerQueryDTO) {
            return TermProvider.GetTermIDsFromCriteria(query.Where.Criteria);
        }

        /**
         * Returns a collection containing all the ID's for each term contained within the specified critiera collections, values may be duplicated.
         * @param criteriaCollection
         */
        public static GetTermIDsFromCriteria(criteriaCollection: Dns.Interfaces.IQueryComposerCriteriaDTO[]): any[] {
            let allTerms: any[] = [];

            for (let i = 0; i < criteriaCollection.length; i++) {

                let criteria = criteriaCollection[i];
                if (criteria.Terms && criteria.Terms.length > 0) {

                    criteria.Terms.forEach((t) => {

                        allTerms.push(t.Type)

                        if (t.Criteria && t.Criteria.length > 0) {
                            let termCriteriaTerms = TermProvider.GetTermIDsFromCriteria(t.Criteria);
                            if (termCriteriaTerms && termCriteriaTerms.length > 0) {
                                termCriteriaTerms.forEach((tt) => allTerms.push(tt));
                            }
                        }
                    });

                }

                if (criteria.Criteria && criteria.Criteria.length > 0) {
                    let subCriteriaTerms = TermProvider.GetTermIDsFromCriteria(criteria.Criteria);
                    if (subCriteriaTerms && subCriteriaTerms.length > 0) {
                        subCriteriaTerms.forEach((stt) => allTerms.push(stt));
                    }
                }

            }

            return allTerms;
        }

    }

}