/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="termvaluefilter.ts" />

module Plugins.Requests.QueryBuilder.MDQ {
    //TODO: confirm that all references to singleton vm have been removed
    export var vm: ViewModel;
    export var GetDataMartTimer;

    //this is the filtered list of terms for the request type
    export var RequestTypeTermIDs: any[] = [];

    export interface ViewModelOptions {
        Request: Dns.Interfaces.IQueryComposerRequestDTO,
        FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
        TemplateNotes: string,
        AdditionalInstructions: string,
        DefaultPriority: Dns.Enums.Priorities,
        DefaultDueDate: Date,
        CriteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[],
        ExistingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
        ProjectID: any,
        VisualTerms: IVisualTerm[],
        Models: Dns.Interfaces.IRequestTypeModelDTO[],
        TemplateID: any,
        RequestTypeID: any,
        RequestID: any,
        IsTemplateEdit: boolean,
        TemplateType: Dns.Enums.TemplateTypes,
        TemplateComposerInterface: Dns.Enums.QueryComposerInterface,
        BindingControl: JQuery
    }

    export class ViewModel extends Global.PageViewModel {
        private options: ViewModelOptions;
        public Request: Dns.ViewModels.QueryComposerRequestViewModel;
        public CriteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[];

        public TermList: IVisualTerm[];
        public FilteredTermList: KnockoutObservableArray<IVisualTerm>;
        public CriteriaTermList: KnockoutComputed<TermVm[]>;
        public StratifiersTermList: KnockoutComputed<TermVm[]>;
        private RefreshFilteredTermList: () => void;
        public UpdateTermList: (modelID: any[], adapterDetail: any, restrictToTermID: any[]) => void;
        public NotAllowedTerms: KnockoutComputed<Dns.Interfaces.ISectionSpecificTermDTO[]>;
        public TemplateTerms: Dns.Interfaces.ITemplateTermDTO[] = [];
        public TermListUpdateDummy: KnockoutObservable<any> = ko.observable<any>();

        public SubscriptionsArray: KnockoutObservableArray<CriteriaGroupSubscription>;
        public CanSaveCriteriaGroup: KnockoutObservable<boolean>;
        public IsTemplateEdit: boolean;
        public IsCriteriaGroupEdit: boolean;
        public IsPresetQuery: boolean;
        public TemplateNotes: string;
        public TermsColumnVisible: KnockoutComputed<boolean>;
        public StratifiersColumnVisible: KnockoutComputed<boolean>;

        public AvailableOrganizations: KnockoutObservableArray<Dns.Interfaces.IOrganizationDTO>;

        public GetCompatibleDataMarts: () => void;

        public SexForCritieria: KnockoutObservableArray<Dns.Structures.KeyValuePair>;
        public SettingForCritieria: KnockoutObservableArray<Dns.Structures.KeyValuePair>;
        public RaceEthnicityForCritieria: KnockoutObservableArray<Dns.Structures.KeyValuePair>;
        public AgeGroupForStratification: KnockoutObservableArray<Dns.Structures.KeyValuePair>;
        public AgeRangeCalculationSelections: (currentCritiera: any, editViewModel: any) => Dns.Structures.KeyValuePair[];
        public ShowAgeRangeCalculationSelections: KnockoutObservable<boolean>;

		public CurrentlySelectedModels: any = [];

        public onExportJSON: () => string;

        public static DataCheckerProcedureCodeTypes = new Array(
            { Name: 'Any', Value: '' },
            { Name: 'ICD-9-CM', Value: '09' },
            { Name: 'ICD-10-CM', Value: '10' },
            { Name: 'ICD-11-CM', Value: '11' },
            { Name: 'CPT Category II', Value: 'C2' },
            { Name: 'CPT Category III', Value: 'C3' },
            { Name: 'CPT-4 (i.e., HCPCS Level I)', Value: 'C4' },
            { Name: 'HCPCS (i.e., HCPCS Level II)', Value: 'HC' },
            { Name: 'HCPCS Level III', Value: 'H3' },
            { Name: 'LOINC', Value: 'LC' },
            { Name: 'Local Homegrown', Value: 'LO' },
            { Name: 'NDC', Value: 'ND' },
            { Name: 'Revenue', Value: 'RE' },
            { Name: 'Other', Value: 'OT' });


        public static DataCheckerDiagnosisCodeTypes = new Array(
            { Name: 'Any', Value: '' },
            { Name: 'ICD-9-CM', Value: '09' },
            { Name: 'ICD-10-CM', Value: '10' },
            { Name: 'ICD-11-CM', Value: '11' },
            { Name: 'SNOMED CT', Value: 'SM' },
            { Name: 'Other', Value: 'OT' });

        public NonAggregateFields: KnockoutComputed<Dns.ViewModels.QueryComposerFieldViewModel[]>;

        public ProjectID: any;
        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];
        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;

        public static YearlyQuarters: KnockoutObservableArray<string> = ko.observableArray<string>(['Q1', 'Q2', 'Q3', 'Q4']);

        public _selfRef: ViewModel;

        constructor(options: ViewModelOptions) {
            super(options.BindingControl);

            var self = this;
            this.options = options;
            this._selfRef = this;
            this.ProjectID = options.ProjectID;



            this.TemplateNotes = options.TemplateNotes || '';
            this.IsTemplateEdit = options.IsTemplateEdit;
            this.IsCriteriaGroupEdit = options.TemplateType === Dns.Enums.TemplateTypes.CriteriaGroup;
            this.IsPresetQuery = options.TemplateComposerInterface === Dns.Enums.QueryComposerInterface.PresetQuery;
            this.SubscriptionsArray = ko.observableArray<CriteriaGroupSubscription>();

            //remove any traces fo the querytype term
            ko.utils.arrayForEach(options.Request.Where.Criteria || [], (croot) => {
                ko.utils.arrayForEach(croot.Terms || [], (troot) => {
                    croot.Terms = ko.utils.arrayFilter(croot.Terms, (tt) => { return !Terms.Compare(tt.Type, Terms.DataCheckerQueryTypeID) });
                });
                ko.utils.arrayForEach(croot.Criteria || [], (csub) => {
                    csub.Terms = ko.utils.arrayFilter(csub.Terms, (tt) => { return !Terms.Compare(tt.Type, Terms.DataCheckerQueryTypeID) });
                });
            });
			//check out options.Models
            var termValueFilter = new Plugins.Requests.QueryBuilder.MDQ.TermValueFilter(ko.utils.arrayMap(options.Models, (m) => m.DataModelID));
            this.SexForCritieria =  ko.observableArray(termValueFilter.SexValues());
            this.SettingForCritieria =  ko.observableArray( termValueFilter.SettingsValues());
            this.RaceEthnicityForCritieria =  ko.observableArray(termValueFilter.RaceEthnicityValues());
            this.AgeGroupForStratification =  ko.observableArray(termValueFilter.AgeRangeStratifications());
            this.ShowAgeRangeCalculationSelections = ko.observable(termValueFilter.HasModel(TermValueFilter.PCORnetModelID));
            this.TermList = options.VisualTerms;
            this.FilteredTermList = ko.observableArray([]);

            //make sure any new template properties exist on the request query json terms
            termValueFilter.ConfirmTemplateProperties(options.Request, options.VisualTerms);

            this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(options.Request);
            this.AvailableOrganizations = ko.observableArray([]);

            if (!this.IsTemplateEdit) {
                //make sure all the criteria have an ID set
                var confirmCritieriaID = (criteria: Dns.ViewModels.QueryComposerCriteriaViewModel) => {
                    if (criteria.ID() == null)
                        criteria.ID(Constants.Guid.newGuid());

                    if (criteria.Criteria() != null) {
                        ko.utils.arrayForEach(criteria.Criteria(), (crit) => {
                            if (crit.ID() == null)
                                crit.ID(Constants.Guid.newGuid());

                            confirmCritieriaID(crit);
                        });
                    }
                };

                ko.utils.arrayForEach(this.Request.Where.Criteria(), (ccrit) => {
                    confirmCritieriaID(ccrit);
                });
            }

            this.CriteriaGroupTemplates = options.CriteriaGroupTemplates;            

            self.RefreshFilteredTermList = () => {
                var list = [];
                self.TermList.forEach((term) => {
                    if (Plugins.Requests.QueryBuilder.MDQ.RequestTypeTermIDs.length == 0) {
                        //Allow all terms.
                        list.push(term);
                    }


                    if (term.Terms == null || term.Terms.length == 0) {
                        if (Plugins.Requests.QueryBuilder.MDQ.RequestTypeTermIDs.indexOf(term.TermID) >= 0) {
                            list.push(term);
                        }
                    }
                    if (term.Terms != null && term.Terms.length > 0) {
                        
                        var hasSummaryModel = options.IsTemplateEdit ? RequestType.Details.vm.SelectedModels.indexOf('cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb') >= 0 : options.Models.length == 1 && termValueFilter.HasModel(TermValueFilter.SummaryTablesModelID);


                        var termCategory = { Name: term.Name, Description: term.Description, TermID: term.TermID, Terms: [], ValueTemplate: term.ValueTemplate, IncludeInCriteria: term.IncludeInCriteria, IncludeInStratifiers: term.IncludeInStratifiers, IncludeInProjectors: term.IncludeInProjectors };
                        term.Terms.forEach((childTerm) => {

                            if (childTerm.TermID != null) {

                                //if the specified models is only summary tables, age range should not be available as a criteria, only stratifier
                                if (Terms.Compare(childTerm.TermID, Terms.AgeRangeID))
                                    childTerm.IncludeInCriteria = options.IsTemplateEdit ? !(hasSummaryModel && RequestType.Details.vm.SelectedModels().length == 1) : !hasSummaryModel;

                                //for Coverage, Drug Class, Drug Name, HCPCS, and all the ICD-9 code terms, stratification is only applicable for Summary Tables Model
                                //They are key indicator terms that are required by the adapter to indicate the type of summary query.
                                if (Terms.Compare(childTerm.TermID, Terms.CoverageID) ||
                                    Terms.Compare(childTerm.TermID, Terms.DrugClassID) ||
                                    Terms.Compare(childTerm.TermID, Terms.DrugNameID) ||
                                    Terms.Compare(childTerm.TermID, Terms.HCPCSProcedureCodesID) ||
                                    Terms.Compare(childTerm.TermID, Terms.ICD9Diagnosis3digitID) ||
                                    Terms.Compare(childTerm.TermID, Terms.ICD9Diagnosis4digitID) ||
                                    Terms.Compare(childTerm.TermID, Terms.ICD9Diagnosis5digitID) ||
                                    Terms.Compare(childTerm.TermID, Terms.ICD9Procedure3digitID) ||
                                    Terms.Compare(childTerm.TermID, Terms.ICD9Procedure4digitID)
                                ) {
                                    childTerm.IncludeInStratifiers = hasSummaryModel;
                                }
                            }

                            if (Plugins.Requests.QueryBuilder.MDQ.RequestTypeTermIDs.indexOf(childTerm.TermID) >= 0) {
                                termCategory.Terms.push(childTerm);
                            }
                        });

                        if (termCategory.Terms.length > 0) {
                            list.push(termCategory);
                        }
                    }
                });

                self.FilteredTermList(list);

            };

            self.RefreshFilteredTermList();

            //load up the templateTerms that aren't allowed under the requestType/template
            if (options.TemplateID) {
                Dns.WebApi.Terms.ListTemplateTerms(options.TemplateID).done((templateTerms: Dns.Interfaces.ITemplateTermDTO[]) => {
                    self.TemplateTerms = templateTerms;
                    this.TermListUpdateDummy.notifySubscribers();
                });
            }

            this.CriteriaTermList = ko.computed(() => {


                this.TermListUpdateDummy();
                var resultArr = ko.utils.arrayFilter(self.FilteredTermList(), (t) => {
                    return t.IncludeInCriteria && ((t.Terms != null && t.Terms.length > 0 && t.TermID == null && ko.utils.arrayFirst(t.Terms, (tt) => tt.IncludeInCriteria) != null) || t.TermID != null);
                }).map<TermVm>((t: IVisualTerm) => {
                    var tvm: TermVm = null;
                    var childTermVM: TermVm[] = [];
                    var templateTerm: Dns.Interfaces.ITemplateTermDTO;
                    var outerAllowed: boolean = false;

                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a group of terms, like Criteria or Demographic
                        t.Terms.forEach((it) => {
                            templateTerm = ko.utils.arrayFirst(self.TemplateTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == it.TermID);
                            childTermVM.push(new TermVm(it, [], templateTerm == null ? true : templateTerm.Allowed));
                            if (templateTerm != null) {
                                if (templateTerm.Allowed) {
                                    outerAllowed = true;
                                }
                            } else {
                                outerAllowed = true;
                            }
                        });
                    }

                    templateTerm = ko.utils.arrayFirst(self.TemplateTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == t.TermID);

                    if (t.TermID != null) {
                        outerAllowed = templateTerm != null ? templateTerm.Allowed : true;
                    }
                    tvm = new TermVm(t, childTermVM, outerAllowed);

                    return tvm;

                });

                $('#TermSelector').kendoMenu({ orientation: 'vertical' });

                return resultArr;

            });


            this.StratifiersTermList = ko.computed(() => {
                this.TermListUpdateDummy();
                var resultArr = ko.utils.arrayFilter(self.FilteredTermList(), (t) => {
                    return t.IncludeInStratifiers && ((t.Terms != null && t.Terms.length > 0 && t.TermID == null && ko.utils.arrayFirst(t.Terms, (tt) => tt.IncludeInStratifiers) != null) || t.TermID != null);
                }).map<TermVm>((t: IVisualTerm) => {

                    var tvm: TermVm = null;
                    var childTermVM: TermVm[] = [];
                    var templateTerm: Dns.Interfaces.ITemplateTermDTO;
                    var outerAllowed: boolean = false;

                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a group of terms, like Criteria or Demographic
                        t.Terms.forEach((it) => {
                            templateTerm = ko.utils.arrayFirst(self.TemplateTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Stratification && tt.TermID == it.TermID);
                            childTermVM.push(new TermVm(it, [], templateTerm == null ? true : templateTerm.Allowed));
                            if (templateTerm != null) {
                                if (templateTerm.Allowed) {
                                    outerAllowed = true;
                                }
                            } else {
                                outerAllowed = true;
                            }
                        });
                    }

                    templateTerm = ko.utils.arrayFirst(self.TemplateTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Stratification && tt.TermID == t.TermID);

                    if (t.TermID != null) {
                        outerAllowed = templateTerm != null ? templateTerm.Allowed : true;
                    }

                    tvm = new TermVm(t, childTermVM, outerAllowed);

                    return tvm;

                });
                $('#FieldsSelector').kendoMenu({ orientation: 'vertical' });
                return resultArr;
            });


            //holds all of the terms that aren't allowed by the request type. 
            //updates when toggling checkboxes next to terms in template edit mode
            this.NotAllowedTerms = ko.computed(() => {
                //The first two arrays filter out the allowed terms (default). Which section the term came from is preserved by keeping the two arrays seperate.

                var criteriaTerms: TermVm[] = [];
                ko.utils.arrayForEach(self.CriteriaTermList(), (t) => {
                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a group of terms, like Criteria or Demographic
                        t.Terms.forEach((it) => {
                            if (!it.Allowed()) {
                                criteriaTerms.push(it);
                            }
                        });
                    } else {
                        if (!t.Allowed())
                            criteriaTerms.push(t);
                    }
                });

                var stratTerms: TermVm[] = [];
                ko.utils.arrayForEach(self.StratifiersTermList(), (t) => {
                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a group of terms, like Criteria or Demographic
                        t.Terms.forEach((it) => {
                            if (!it.Allowed()) {
                                stratTerms.push(it);
                            }
                        });
                    } else {
                        if (!t.Allowed())
                            stratTerms.push(t);
                    }
                });

                //the last two arrays hold the terms as ISectionSpecificDTOs, a form that can be saved. 
                var notAllowedCritTerms: Dns.Interfaces.ISectionSpecificTermDTO[] = criteriaTerms.map(function (t: TermVm): Dns.Interfaces.ISectionSpecificTermDTO {
                    var sectionTerm: Dns.Interfaces.ISectionSpecificTermDTO = {
                        Section: Dns.Enums.QueryComposerSections.Criteria,
                        TermID: t.TermID
                    };

                    return sectionTerm;
                });

                var notAllowedStratTerms: Dns.Interfaces.ISectionSpecificTermDTO[] = stratTerms.map(function (t: TermVm): Dns.Interfaces.ISectionSpecificTermDTO {
                    var sectionTerm: Dns.Interfaces.ISectionSpecificTermDTO = {
                        Section: Dns.Enums.QueryComposerSections.Stratification,
                        TermID: t.TermID
                    }
                    return sectionTerm;
                });

                //Since the section the terms came from is now preserved by a property in the DTO, the arrays can be concated
                return notAllowedCritTerms.concat(notAllowedStratTerms);
            }, this, { deferEvaluation: true });


            this.UpdateTermList = (modelID, adapterDetail, restrictToTermsID) => {

                var values = []
                if (modelID != null) {
                    ko.utils.arrayForEach(ko.utils.arrayMap(modelID, (id) => 'modelID=' + id), (i) => values.push(i));
                }
                if (adapterDetail != null && adapterDetail != '') {
                    values.push('adapterDetail=' + adapterDetail);
                }
                if (restrictToTermsID != null) {
                    ko.utils.arrayForEach(ko.utils.arrayMap(restrictToTermsID, (id) => 'termID=' + id), (i) => values.push(i));
                }
			
				//ko.utils.arrayForEach(ko.utils.arrayMap(restrictToTermsID, (id) => 'termID=' + id), (i) => values.push(i));
				var termValueFilter = new Plugins.Requests.QueryBuilder.MDQ.TermValueFilter(modelID);
				self.SexForCritieria(termValueFilter.SexValues());
				self.SettingForCritieria =  ko.observableArray( termValueFilter.SettingsValues());
				self.RaceEthnicityForCritieria =  ko.observableArray(termValueFilter.RaceEthnicityValues());
				self.AgeGroupForStratification =  ko.observableArray(termValueFilter.AgeRangeStratifications());
				self.ShowAgeRangeCalculationSelections = ko.observable(termValueFilter.HasModel(TermValueFilter.PCORnetModelID));
                var query = values.join('&');

                Dns.WebApi.Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetTermsFilteredBy?' + query, true).done((terms) => {

                    //When the observable collection changes it voids the kendoMenu, need to re-initialize the menu after changes.
                    var criteriaMenu = $('#TermSelector').data('kendoMenu');
                    criteriaMenu.destroy();

                    var selectMenu = $('#FieldsSelector').data('kendoMenu');
                    selectMenu.destroy();

                    Plugins.Requests.QueryBuilder.MDQ.RequestTypeTermIDs = terms.map((t) => t.TermID) || [];
                    self.RefreshFilteredTermList();

                    $('#TermSelector').kendoMenu({ orientation: 'vertical' });
                    $('#FieldsSelector').kendoMenu({ orientation: 'vertical' });


                });
            };

            GetDataMartTimer = options.IsTemplateEdit ? null : 0;

            this.AgeRangeCalculationSelections = (criteria: Dns.ViewModels.QueryComposerCriteriaViewModel, editViewModel: ViewModel) => {

                ////make a shallow clone of the translations array, and then add the placeholder default item
                //var calculationTypes = ko.utils.arrayGetDistinctValues(Dns.Enums.AgeRangeCalculationTypeTranslation);                

                //if (criteria.ID() != editViewModel.Request.Where.Criteria()[0].ID()) {
                //    //for any criteria other than the first one only the first two calculation types are valid
                //    //1) At first encounter that meets the criteria in this criteria group
                //    //2) At the last encounter that meets the criteria in this criteria group
                //    calculationTypes.splice(2, calculationTypes.length - 2);
                //}


                //until calculation within criteria groups is fully implemented remove those options from the availabe calculation types - PMNDEV-5698
                var calculationTypes = ko.utils.arrayGetDistinctValues(Dns.Enums.AgeRangeCalculationTypeTranslation);
                calculationTypes.splice(0, 2);

                return calculationTypes;
            };

            this.NonAggregateFields = ko.computed(() => {
                //hide the aggregate fields from view since they are not editable anyhow
                var filtered = ko.utils.arrayFilter(self.Request.Select.Fields(), (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Aggregate() == null; });
                
				return filtered;
            });

            try {
                this.CanSaveCriteriaGroup(Templates.Details.vm === undefined || Templates.Details.vm == null);
            } catch (e) {
                this.CanSaveCriteriaGroup = ko.observable(true);
            }

            if (options.Request == null) {
                var criteriaVM = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                if (!options.IsTemplateEdit)
                    criteriaVM.ID(Constants.Guid.newGuid());

                this.Request.Where.Criteria.push(criteriaVM);

                if (this.IsTemplateEdit == false) {

                    self.SubscriptionsArray.push({
                        CriteriaGroup: criteriaVM,
                        Subscription: criteriaVM.Terms.subscribe(() => {
                            GetDataMartTimer = setInterval((() => self.GetCompatibleDataMarts()).bind(self), 2000);
                        })
                    });

                }
            }
            else {

                var codeTerms = [
                    Terms.DrugClassID,//drug class
                    Terms.DrugNameID,//drug name
                    Terms.HCPCSProcedureCodesID,//HCPCS Procedure Codes
                    Terms.ICD9Diagnosis3digitID,//ICD9 Diagnosis Codes 3 digit
                    Terms.ICD9Diagnosis4digitID,//ICD9 Diagnosis Codes 4 digit
                    Terms.ICD9Diagnosis5digitID,//ICD9 Diagnosis Codes 5 digit
                    Terms.ICD9Procedure3digitID,//ICD9 Procedure Codes 3 digit
                    Terms.ICD9Procedure4digitID,//ICD9 Procedure Codes 4 digit
                    Terms.ZipCodeID,//Zip Code
                    Terms.CombinedDiagnosisCodesID,//Combinded Diagnosis Codes
                    Terms.ESPCombinedDiagnosisCodesID,//ESP Combined Diagnosis Codes
                    Terms.ProcedureCodesID // Procedure Codes
                ];

                var convertTerms = (terms: Dns.ViewModels.QueryComposerTermViewModel[]) => {
                    terms.forEach((term) => {
                        var termValues = Global.Helpers.ConvertTermObject(term.Values());
                        term.Values(termValues);

                        if (codeTerms.indexOf(term.Type().toUpperCase()) >= 0) {

                            if (term.Values != null && term.Values().CodeValues != null) {
                                //Do not re-map as the CodeValues property already exists...
                            }
                            else {
                                this.TermList.forEach((item) => {
                                    if (item.Terms == null || item.Terms.length == 0) {
                                        if (term.Type() == item.TermID) {
                                            var termValuesUpdated = Global.Helpers.CopyObject(item.ValueTemplate);
                                            term.Values(termValuesUpdated);
                                        }
                                    }
                                    if (item.Terms != null && item.Terms.length > 0) {
                                        item.Terms.forEach((childTerm) => {
                                            if (term.Type() == childTerm.TermID) {
                                                var termValuesUpdated = Global.Helpers.CopyObject(childTerm.ValueTemplate);
                                                term.Values(termValuesUpdated);
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    });

                };


                this.Request.Where.Criteria().forEach((cvm) => {
                    var selfVM = this;

                    convertTerms(cvm.Terms());

                    cvm.Criteria().forEach((subCriteria) => {
                        convertTerms(subCriteria.Terms());
                    });
                });


                //Add subscriptions here to listen for criteria changes
                if (this.IsTemplateEdit == false) {

                    this.Request.Where.Criteria().forEach((criteriaVM) => {
                        //subscribe for the main terms

                        self.SubscriptionsArray.push({
                            CriteriaGroup: criteriaVM,
                            Subscription: criteriaVM.Terms.subscribe(() => {
                                GetDataMartTimer = setInterval((() => self.GetCompatibleDataMarts()).bind(self), 2000);
                            })
                        });

                        //subscribe for the sub-criteria colletion change (ie, adding removing a sub-criteria)
                        self.SubscriptionsArray.push({
                            CriteriaGroup: criteriaVM,
                            Subscription: criteriaVM.Criteria.subscribe(() => {
                                GetDataMartTimer = setInterval((() => self.GetCompatibleDataMarts()).bind(self), 2000);
                            })
                        });

                        //subscribe to the terms for each subcriteria
                        ko.utils.arrayForEach(criteriaVM.Criteria(), (subcrit) => {
                            self.SubscriptionsArray.push({
                                CriteriaGroup: subcrit,
                                Subscription: subcrit.Terms.subscribe(() => {
                                    GetDataMartTimer = setInterval((() => self.GetCompatibleDataMarts()).bind(self), 2000);
                                })
                            });
                        });



                    });

                    var strQuery = "";
                    if (this.Request != null) {
                        strQuery = JSON.stringify(this.Request.toData())
                    }
                    //Call the GetCompatibleDataMarts method.
                    Plugins.Requests.QueryBuilder.DataMartRouting.vm.LoadDataMarts(options.ProjectID, strQuery);

                }
            }

            this.GetCompatibleDataMarts = () => {
                clearInterval(GetDataMartTimer);

                //Do not attempt to get datamarts in the template editor
                if (self.IsTemplateEdit)
                    return;
                var strQuery = "";
                if (self.Request != null) {
                    strQuery = JSON.stringify(self.Request.toData())
                }
                //Call the GetCompatibleDataMarts method.
                Plugins.Requests.QueryBuilder.DataMartRouting.vm.LoadDataMarts(options.ProjectID, strQuery);
            };

            //Add subscription for the Stratification Fields...
            if (!this.IsTemplateEdit) {
                this.Request.Select.Fields.subscribe(() => {
                    GetDataMartTimer = setInterval((() => self.GetCompatibleDataMarts()).bind(self), 2000);
                });
            }

            self.onExportJSON = () => {
                return 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(self.Request.toData()));
            };

            self.TermsColumnVisible = ko.computed(() => {
                var isVis: boolean = false;
                if (self.IsTemplateEdit)
                    isVis = true;
                else {
                    ko.utils.arrayForEach(self.CriteriaTermList(), (item) => {
                        if (item.Terms.length > 0) {
                            ko.utils.arrayForEach(item.Terms, (child) => {
                                if (child.Allowed() && child.IncludeInCriteria)
                                    isVis = true;
                                else
                                    isVis = false;
                            });

                        }
                        else {
                            if (item.Allowed() && item.IncludeInCriteria)
                                isVis = true;
                            else
                                isVis = false;
                        }
                    });
                }
                return isVis;
            });
            self.StratifiersColumnVisible = ko.computed(() => {
                var isVis: boolean = false;
                if (self.IsTemplateEdit)
                    isVis = true;
                else {
                    ko.utils.arrayForEach(self.StratifiersTermList(), (item) => {
                        if (item.Terms.length > 0) {
                            ko.utils.arrayForEach(item.Terms, (child) => {
                                if (child.Allowed() && child.IncludeInStratifiers)
                                    isVis = true;
                                else
                                    isVis = false;
                            });

                        }
                        else {
                            if (item.Allowed() && item.IncludeInStratifiers)
                                isVis = true;
                            else
                                isVis = false;
                        }
                    });
                }
                return isVis;
            });
        }

        public FilterTermsForCriteria(terms: TermVm[]): TermVm[] {
            return ko.utils.arrayFilter(terms, (t) => t.IncludeInCriteria);
        }

        public FilterTermsForStratification(terms: TermVm[]): TermVm[] {
            return ko.utils.arrayFilter(terms, (t) => t.IncludeInStratifiers);
        }

        public btnAddCriteriaGroup_Click(data: ViewModel, event: JQueryEventObject) {
            var criteriaVM = new Dns.ViewModels.QueryComposerCriteriaViewModel();
            if (!this.IsTemplateEdit)
                criteriaVM.ID(Constants.Guid.newGuid());

            this.Request.Where.Criteria.push(criteriaVM);

            if (this.IsTemplateEdit == false) {

                var self = this;

                self.SubscriptionsArray.push({
                    CriteriaGroup: criteriaVM,
                    Subscription: criteriaVM.Terms.subscribe(() => {
                        var iself = self;
                        GetDataMartTimer = setInterval((() => iself.GetCompatibleDataMarts()).bind(iself), 5000);
                    })
                });

                self.SubscriptionsArray.push({
                    CriteriaGroup: criteriaVM,
                    Subscription: criteriaVM.Criteria.subscribe(() => {
                        var iself = self;
                        GetDataMartTimer = setInterval((() => iself.GetCompatibleDataMarts()).bind(iself), 5000);
                    })
                });
            }


        }

        public btnDeleteCriteriaGroup_Click(data: Dns.ViewModels.QueryComposerCriteriaViewModel) {
            var self = this;

            if (self.SubscriptionsArray != null) {
                var itemArr = self.SubscriptionsArray.remove((item) => { return item.CriteriaGroup == data; });
                if (itemArr != null) {
                    itemArr.forEach((item) => {
                        item.Subscription.dispose();
                    });
                }
            }

            self.Request.Where.Criteria.remove(data);
            itemArr = null;
        }

        public btnSaveCriteriaGroup_Click(criteriaGroup: Dns.ViewModels.QueryComposerCriteriaViewModel) {

            var parameters = {
                CriteriaGroup: criteriaGroup.toData(),
                AdapterDetail: this.Request.Header.QueryType(),
                TemplateID: this.options.TemplateID,
                RequestTypeID: this.options.RequestTypeID,
                RequestID: this.options.RequestID
            };

            Global.Helpers.ShowDialog("Save Criteria Group", "/querycomposer/savecriteriagroup", ["close"], 650, 400, parameters).done(() => {

            });
        }

        public AddCriteriaGroupTemplate(data: Dns.Interfaces.ITemplateDTO, event: JQueryEventObject) {
            Dns.WebApi.Templates.Get(data.ID).done((template) => {

                var criteria = new Dns.ViewModels.QueryComposerCriteriaViewModel(JSON.parse(template[0].Data));

                var codeTerms = [
                    Terms.DrugClassID,//drug class
                    Terms.DrugNameID,//drug name
                    Terms.HCPCSProcedureCodesID,//HCPCS Procedure Codes
                    Terms.ICD9Diagnosis3digitID,//ICD9 Diagnosis Codes 3 digit
                    Terms.ICD9Diagnosis4digitID,//ICD9 Diagnosis Codes 4 digit
                    Terms.ICD9Diagnosis5digitID,//ICD9 Diagnosis Codes 5 digit
                    Terms.ICD9Procedure3digitID,//ICD9 Procedure Codes 3 digit
                    Terms.ICD9Procedure4digitID,//ICD9 Procedure Codes 4 digit
                    Terms.ZipCodeID,//Zip Code
                    Terms.CombinedDiagnosisCodesID,//Combinded Diagnosis Codes
                    Terms.ESPCombinedDiagnosisCodesID,//ESP Combined Diagnosis Codes
                    Terms.ProcedureCodesID // Procedure Codes
                ];

                var convertTerms = (terms: Dns.ViewModels.QueryComposerTermViewModel[]) => {
                    terms.forEach((term) => {
                        var termValues = Global.Helpers.ConvertTermObject(term.Values());
                        term.Values(termValues);

                        if (codeTerms.indexOf(term.Type().toUpperCase()) >= 0) {

                            if (term.Values != null && term.Values().CodeValues != null) {
                                //Do not re-map as the CodeValues property already exists...
                            }
                            else {
                                this.TermList.forEach((item) => {
                                    if (item.Terms == null || item.Terms.length == 0) {
                                        if (term.Type() == item.TermID) {
                                            var termValuesUpdated = Global.Helpers.CopyObject(item.ValueTemplate);
                                            term.Values(termValuesUpdated);
                                        }
                                    }
                                    if (item.Terms != null && item.Terms.length > 0) {
                                        item.Terms.forEach((childTerm) => {
                                            if (term.Type() == childTerm.TermID) {
                                                var termValuesUpdated = Global.Helpers.CopyObject(childTerm.ValueTemplate);
                                                term.Values(termValuesUpdated);
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    });

                };

                convertTerms(criteria.Terms());

                criteria.Criteria().forEach((subCriteria) => {
                    convertTerms(subCriteria.Terms());
                });

                if (!this.IsTemplateEdit)
                    criteria.ID(Constants.Guid.newGuid());

                if (vm.Request.Where.Criteria()[0].Criteria().length == 0 && vm.Request.Where.Criteria().length == 1)
                    vm.Request.Where.Criteria().pop();

                vm.Request.Where.Criteria.push(criteria);
            });
        }

        /**
        Grouped terms are to be combined using OR within a sub-criteria that will be AND'd with the other terms of the parent criteria.
        */
        private static GroupedTerms: any[] = [
            //Condition
            Terms.ConditionID,
            //HCPCS Procedure Codes
            Terms.HCPCSProcedureCodesID,
            //Combined Diagnosis Codes
            Terms.CombinedDiagnosisCodesID,
            //ICD9 Diagnosis Codes 3 digit
            Terms.ICD9Diagnosis3digitID,
            //ICD9 Diagnosis Codes 4 digit
            Terms.ICD9Diagnosis4digitID,
            //ICD9 Diagnosis Codes 5 digit
            Terms.ICD9Diagnosis5digitID,
            //ICD9 Procedure Codes 3 digit
            Terms.ICD9Procedure3digitID,
            //ICD9 Procedure Codes 4 digit
            Terms.ICD9Procedure4digitID,
            //ESP Combined Diagnosis Codes
            Terms.ESPCombinedDiagnosisCodesID,
            //Drug Class
            Terms.DrugClassID,
            //Drug Name
            Terms.DrugNameID,
            //Visits
            Terms.VisitsID,
            //Age Range
            Terms.AgeRangeID,
            //Sex
            Terms.SexID,
            //Code Metric
            Terms.CodeMetricID,
            //Coverage
            Terms.CoverageID,
            //Criteria
            Terms.CriteriaID,
            //Dispensing Metric
            Terms.DispensingMetricID,
            //Ethnicity
            Terms.EthnicityID,
            //Facility
            Terms.FacilityID,
            //Height
            Terms.HeightID,
            //Hispanic
            Terms.HispanicID,
            //Observation Period
            Terms.ObservationPeriodID,
            //Quarter Year
            Terms.QuarterYearID,
            //Race
            Terms.RaceID,
            //Setting
            Terms.SettingID,
            //Tobacco Use
            Terms.TobaccoUseID,
            //Weight
            Terms.WeightID,
            //Year
            Terms.YearID,
            //Zip Code
            Terms.ZipCodeID,
            //Vitals Measure Date
            Terms.VitalsMeasureDateID,
            // Procedure Codes
            Terms.ProcedureCodesID 
        ];

        /** Non-code terms that still need to use a sub-criteria to handle multiple term's OR'd together */
        private static NonCodeGroupedTerms: any[] = [
            //Visits
            Terms.VisitsID,
            //Age Range
            Terms.AgeRangeID,
            //Sex
            Terms.SexID,
            //Code Metric
            Terms.CodeMetricID,
            //Coverage
            Terms.CoverageID,
            //Criteria
            Terms.CriteriaID,
            //Dispensing Metric
            Terms.DispensingMetricID,
            //Ethnicity
            Terms.EthnicityID,
            //Facility
            Terms.FacilityID,
            //Height
            Terms.HeightID,
            //Hispanic
            Terms.HispanicID,
            //Observation Period
            Terms.ObservationPeriodID,
            //Quarter Year
            Terms.QuarterYearID,
            //Race
            Terms.RaceID,
            //Setting
            Terms.SettingID,
            //Tobacco Use
            Terms.TobaccoUseID,
            //Weight
            Terms.WeightID,
            //Year
            Terms.YearID,
            //Zip Code
            Terms.ZipCodeID,
            //Vitals Measure Date
            Terms.VitalsMeasureDateID,
        ];

        public AddTerm(root: ViewModel, data: IVisualTerm, parent: Dns.ViewModels.QueryComposerCriteriaViewModel, event: JQueryEventObject) {

            var self = root;

            SuspendDataMartTimer();

            //Update 2014-12-04: Reverted to use CopyObject to create the Value Template to ensure properties are marked as oservables.
            //As observables, the values entered via Term Templates are saved properly.

            ////using Page.ObjectCopy was resulting in all the properties of termValues getting converted into observables.
            ////The the constructor of QueryComposerTermViewModel does not support this, doing plain copy to match what is happening in generated ViewModel.
            //var termValues = <any>{};
            //for (var prop in data.ValueTemplate) {
            //    termValues[prop] = data.ValueTemplate[prop];
            //}

            var termValues = Global.Helpers.CopyObject(data.ValueTemplate);


            var termViewModel = new Dns.ViewModels.QueryComposerTermViewModel({
                Operator: Dns.Enums.QueryComposerOperators.And,
                Type: data.TermID,
                Values: termValues,
                Criteria: null,
                Design: data.Design
            });

            //add the term to the appropriate sub-criteria if same terms are to be OR'd within the same parent criteria.
            if (ViewModel.GroupedTerms.indexOf(data.TermID.toUpperCase()) >= 0) {

                //the terms should be OR's together and the criteria AND'd to other sub-criteria and parent criteria terms.
                termViewModel.Operator(Dns.Enums.QueryComposerOperators.Or);

                //find the first sub-criteria that contains a matching term (each sub-criteria should only contain terms of the same type at this time)
                var criteria: Dns.ViewModels.QueryComposerCriteriaViewModel = ko.utils.arrayFirst(parent.Criteria(), (c) => {
                    var t = ko.utils.arrayFirst(c.Terms(), (tt) => true);
                    if (t == null)
                        return false;

                    //if the term is a noncode term compare the first term in the subcriteria, for these all the terms should be the same.
                    if (ViewModel.NonCodeGroupedTerms.indexOf(data.TermID.toUpperCase()) >= 0) {
                        return ViewModel.NonCodeGroupedTerms.indexOf(t.Type().toUpperCase()) >= 0 && t.Type().toUpperCase() == data.TermID.toUpperCase();
                    }

                    //if it is a noncode term return if the first term of the sub-criteria is a noncode term or not.
                    //if the term is a code term it should return true.
                    return ViewModel.NonCodeGroupedTerms.indexOf(t.Type().toUpperCase()) < 0;

                });

                if (criteria == null) {

                    criteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                    criteria.Operator(Dns.Enums.QueryComposerOperators.And);
                    criteria.Type(Dns.Enums.QueryComposerCriteriaTypes.Paragraph);
                    criteria.ID(Constants.Guid.newGuid());
                    criteria.Name(ViewModel.NonCodeGroupedTerms.indexOf(data.TermID.toUpperCase()) >= 0 ? 'i_' + data.Name.replace(' ', '_') : 'i_codeterms');
                    criteria.Terms.push(termViewModel);

                    parent.Criteria.push(criteria);

                    self.SubscriptionsArray.push({
                        CriteriaGroup: criteria,
                        Subscription: criteria.Terms.subscribe(() => {
                            GetDataMartTimer = setInterval((() => self.GetCompatibleDataMarts()).bind(self), 2000);
                        })
                    });

                } else {
                    criteria.Terms.push(termViewModel);
                }


            } else {
                parent.Terms.push(termViewModel);
            }

            if (data.IncludeInStratifiers == false)
                return;

            SuspendDataMartTimer();

            var fieldsToAdd: Dns.ViewModels.QueryComposerFieldViewModel[] = [];

            var selectFields = self.Request.Select.Fields();
            
            //specify the field as group by and included in the select
            var foundStratifyByField = ko.utils.arrayFirst(selectFields, (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() == null });
            if (foundStratifyByField == null) {
                fieldsToAdd.push(new Dns.ViewModels.QueryComposerFieldViewModel({
                    Type: data.TermID,
                    FieldName: data.Name,
                    Aggregate: null,
                    StratifyBy: null,
                    OrderBy: Dns.Enums.OrderByDirections.None,
                    Select: null,
                    GroupBy: null,
                }));
            }

            //any field included in the criteria is included as a count
            var foundCountField = ko.utils.arrayFirst(selectFields, (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() != null; });
            if (foundCountField == null) {
                fieldsToAdd.push(new Dns.ViewModels.QueryComposerFieldViewModel({
                    Type: data.TermID,
                    Aggregate: Dns.Enums.QueryComposerAggregates.Count,
                    FieldName: data.Name + '_Count',
                    GroupBy: null,
                    StratifyBy: null,
                    OrderBy: Dns.Enums.OrderByDirections.None,
                    Select: null
                }));
            }

            if (fieldsToAdd.length > 0) {

                for (var i = 0; i < fieldsToAdd.length; i++) {

                    self.Request.Select.Fields.push(fieldsToAdd[i]);

                    if (i < fieldsToAdd.length - 1)
                        SuspendDataMartTimer();

                }
            }

        }

        public AddField(data: IVisualField, parent: Dns.ViewModels.QueryComposerSelectViewModel, event: JQueryEventObject) {

            SuspendDataMartTimer();

            var fieldsToAdd: Dns.ViewModels.QueryComposerFieldViewModel[] = [];

            var selectFields = parent.Fields();

            var foundStratifyByField = ko.utils.arrayFirst(selectFields, (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() == null });
            if (foundStratifyByField == null) {
                fieldsToAdd.push(new Dns.ViewModels.QueryComposerFieldViewModel({
                    Aggregate: null,
                    FieldName: data.Name.replace(' ', '_'),
                    GroupBy: null,
                    StratifyBy: null,
                    OrderBy: Dns.Enums.OrderByDirections.None,
                    Select: null,
                    Type: data.TermID
                }));
            }

            //any field included in the criteria is included as a count
            if (!ko.utils.arrayFirst(selectFields, (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() != null; })) {
                fieldsToAdd.push(new Dns.ViewModels.QueryComposerFieldViewModel({
                    Type: data.TermID,
                    Aggregate: Dns.Enums.QueryComposerAggregates.Count,
                    FieldName: data.Name.replace(' ', '_') + '_Count',
                    GroupBy: null,
                    StratifyBy: null,
                    OrderBy: Dns.Enums.OrderByDirections.None,
                    Select: null
                }));
            }

            for (var i = 0; i < fieldsToAdd.length; i++) {

                parent.Fields.push(fieldsToAdd[i]);

                if (i < fieldsToAdd.length - 1)
                    SuspendDataMartTimer();
            }

        }

        public TemplateSelector(data: Dns.ViewModels.QueryComposerTermViewModel) {
            return "e_" + data.Type();
        }

        public StratifierTemplateSelector(data: Dns.ViewModels.QueryComposerFieldViewModel) {
            return "s_" + data.Type();
        }

        public OpenCodeSelector(data: Dns.ViewModels.QueryComposerTermViewModel, list: Dns.Enums.Lists, showCategoryDropdown?: boolean) {
            Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                ListID: list,
                Codes: data.Values == null || data.Values().CodeValues == null ? "" : data.Values().CodeValues(),
                ShowCategoryDropdown: showCategoryDropdown == null ? true : showCategoryDropdown
            }).done((results: Dns.Interfaces.ICodeSelectorValueDTO[]) => {
                if (!results)
                    return; //User clicked cancel

                data.Values.valueWillMutate();

                var codes = ko.utils.arrayMap(results, (item: Dns.Interfaces.ICodeSelectorValueDTO) => <Dns.Interfaces.ICodeSelectorValueDTO>{ Code: item.Code.trim(), Name: item.Name.trim(), ExpireDate: null });

                if (data.Values().CodeValues == null)
                    data.Values().CodeValues = [];

                data.Values().CodeValues.removeAll();
                codes.forEach((code) => {
                    data.Values().CodeValues.push(code);
                });

                data.Values.valueHasMutated();
            });
        }

        public ConfirmCombinedCodeTypeChanged(data: Dns.ViewModels.QueryComposerTermViewModel, e: kendo.ui.DropDownListSelectEvent) {
            if (data.Values == null || data.Values().CodeValues == null || data.Values().CodeValues() == null || data.Values().CodeValues().length == 0)
                return;

            var oldValue = e.sender.value();
            Global.Helpers.ShowConfirm("Code Change Confirmation", "Changing the Code Set will reset the selected values, would you like to continue?").fail(() => {
                e.sender.value(oldValue);
                data.Values().CodeType(oldValue);
                return;
            }).done(() => {
                data.Values().CodeValues('');
                return;
            });
        }

        public ConfirmProcedureCodeChanged(data: Dns.ViewModels.QueryComposerTermViewModel, e: kendo.ui.DropDownListSelectEvent) {
            if (data.Values == null || data.Values().CodeValues == null || data.Values().CodeValues() == null || data.Values().CodeValues().length == 0)
                return;

            var oldValue = e.sender.value();
            Global.Helpers.ShowConfirm("Code Change Confirmation", "Changing the Code Set will reset the selected values, would you like to continue?").fail(() => {
                e.sender.value(oldValue);
                data.Values().CodeType(oldValue);
                return;
            }).done(() => {
                data.Values().CodeValues('');
                return;
            });
        }

        public OpenCombinedCodeSelector(data: Dns.ViewModels.QueryComposerTermViewModel, codeType: Dns.Enums.DiagnosisCodeTypes) {
            //codeType will indicate the type of list to use in the selector
            if (codeType != Dns.Enums.DiagnosisCodeTypes.ICD9) {
                alert('Only ICD-9 diagnosis codes are supported by the code selector, please enter the codes manually into the text field separated by semi-colons.');
                return;
            }

            //need to get the current values and split by semi-colon, and add to code selector values
            var existingValues: string[] = null;

            if (data.Values != null && data.Values().CodeValues != null) {
                existingValues = ko.utils.arrayFilter(ko.utils.arrayMap((data.Values().CodeValues() || '').split(';'), (c: string) => (c || '').trim()), (c: string) => c.length > 0);
            }

            Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                ListID: Dns.Enums.Lists.SPANDiagnosis,
                Codes: existingValues,
                ShowCategoryDropdown: true
            }).done((results: Dns.Interfaces.ICodeSelectorValueDTO[]) => {
                if (!results)
                    return; //User clicked cancel

                //combine the selected codes into a single string
                var codes = ko.utils.arrayMap(results, (item) => item.Code).join('; ');

                data.Values.valueWillMutate();

                if (data.Values().CodeValues == null) {
                    data.Values().CodeValues = ko.observable(codes);
                } else {
                    data.Values().CodeValues(codes);
                }

                data.Values.valueHasMutated();
            });

        }
        public ConfirmESPCombinedCodeTypeChanged(data: Dns.ViewModels.QueryComposerTermViewModel, e: kendo.ui.DropDownListSelectEvent) {
            if (data.Values == null || data.Values().CodeValues == null || data.Values().CodeValues() == null || data.Values().CodeValues().length == 0)
                return;

            var oldValue = e.sender.value();
            Global.Helpers.ShowConfirm("Code Change Confirmation", "Changing the Code Set will reset the selected values, would you like to continue?").fail(() => {
                e.sender.value(oldValue);
                data.Values().CodeType(oldValue);
                return;
            }).done(() => {
                data.Values().CodeValues('');
                return;
            });
        }
        public OpenESPCombinedCodeSelector(data: Dns.ViewModels.QueryComposerTermViewModel, codeType: Dns.Enums.ESPCodes) {
            //codeType will indicate the type of list to use in the selector
            if (codeType != Dns.Enums.ESPCodes.ICD9 && codeType != Dns.Enums.ESPCodes.ICD10) {
                alert('Only ICD-9 diagnosis codes are supported by the code selector, please enter the codes manually into the text field separated by semi-colons.');
                return;
            }

            //need to get the current values and split by semi-colon, and add to code selector values
            var existingValues: string[] = null;

            if (data.Values != null && data.Values().CodeValues != null) {
                existingValues = ko.utils.arrayFilter(ko.utils.arrayMap((data.Values().CodeValues() || '').split(';'), (c: string) => (c || '').trim()), (c: string) => c.length > 0);
            }


            if (codeType == Dns.Enums.ESPCodes.ICD9) {
                Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                    ListID: Dns.Enums.ESPCodes.ICD9,
                    Codes: existingValues,
                    ShowCategoryDropdown: true
                }).done((results: Dns.Interfaces.ICodeSelectorValueDTO[]) => {
                    if (!results)
                        return; //User clicked cancel

                    //combine the selected codes into a single string
                    var codes = ko.utils.arrayMap(results, (item) => item.Code).join('; ');

                    data.Values.valueWillMutate();

                    if (data.Values().CodeValues == null) {
                        data.Values().CodeValues = ko.observable(codes);
                    } else {
                        data.Values().CodeValues(codes);
                    }

                    data.Values.valueHasMutated();
                });
            }
            else {
                Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                    ListID: Dns.Enums.ESPCodes.ICD10,
                    Codes: existingValues,
                    ShowCategoryDropdown: true
                }).done((results: Dns.Interfaces.ICodeSelectorValueDTO[]) => {
                    if (!results)
                        return; //User clicked cancel

                    //combine the selected codes into a single string
                    var codes = ko.utils.arrayMap(results, (item) => item.Code).join('; ');

                    data.Values.valueWillMutate();

                    if (data.Values().CodeValues == null) {
                        data.Values().CodeValues = ko.observable(codes);
                    } else {
                        data.Values().CodeValues(codes);
                    }

                    data.Values.valueHasMutated();
                });
            }

        }

        public DeleteTerm(data: Dns.ViewModels.QueryComposerTermViewModel, criteriaGroup: Dns.ViewModels.QueryComposerCriteriaViewModel) {

            SuspendDataMartTimer();

            /** only removes from the criteria **/
            criteriaGroup.Terms.remove(data);

            /** remove the criteria if it is a sub-criteria and is empty **/
            ko.utils.arrayForEach(this.Request.Where.Criteria(), (c) => {

                var subCriteriaToRemove = ko.utils.arrayFilter(c.Criteria(), (sc) => sc.Terms().length == 0);

                subCriteriaToRemove.forEach((sc) => {

                    //clean up the sub-criteria's terms collection subscription since the sub-criteria is getting removed.                    
                    var subscriptionsToDispose = vm.SubscriptionsArray.remove((cs) => { return cs.CriteriaGroup == sc; });
                    if (subscriptionsToDispose != null) {
                        subscriptionsToDispose.forEach((item) => {
                            item.Subscription.dispose();
                        });
                    }
                    subscriptionsToDispose = null;

                    SuspendDataMartTimer();
                    c.Criteria.remove(sc);

                });
            });
        }

        public DeleteField(data: Dns.ViewModels.QueryComposerFieldViewModel, selectFields: Dns.ViewModels.QueryComposerSelectViewModel) {

            SuspendDataMartTimer();

            /** removes the count as well **/

            var fieldsToRemove: Dns.ViewModels.QueryComposerFieldViewModel[] = ko.utils.arrayFilter(selectFields.Fields(), (field: Dns.ViewModels.QueryComposerFieldViewModel) => {
                return field.Type() == data.Type()
            });

            for (var i = 0; i < fieldsToRemove.length; i++) {
                selectFields.Fields.remove(fieldsToRemove[i]);

                if (i < fieldsToRemove.length - 1)
                    SuspendDataMartTimer();
            }

        }

        public ShowSubCriteriaConjuction(parentCriteria: Dns.ViewModels.QueryComposerCriteriaViewModel, subCriteria: Dns.ViewModels.QueryComposerCriteriaViewModel) {
            if (parentCriteria.Criteria().length < 2)
                return false;

            if (parentCriteria.Criteria().indexOf(subCriteria) == 0) {
                return false;
            }

            return true;
        }
    }

    export function GetVisualTerms(): JQueryDeferred<IVisualTerm[]> {
        var d = $.Deferred<IVisualTerm[]>();

        $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
            .done((result: IVisualTerm[]) => {
                d.resolve(result);
            }).fail((e, description, error) => {
                d.reject(<any>e);
            });

        return d;
    }

    export function init(
        rawRequestData: Dns.Interfaces.IQueryComposerRequestDTO,
        fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
        defaultPriority: Dns.Enums.Priorities,
        defaultDueDate: Date,
        additionalInstructions: string,
        existingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
        requestTypeID: any,
        visualTerms: IVisualTerm[],
        isTemplateEdit: boolean = false,
        projectID: any = Global.GetQueryParam("projectID"),
        templateID?: any
    ): JQueryPromise<ViewModel> {
        var requestID: any = null;

        if (isTemplateEdit) {
            templateID = templateID || Global.GetQueryParam("ID");
        } else {
            var templateID: any = templateID || Global.GetQueryParam("templateID");
            var requestID: any = Global.GetQueryParam("ID");
        }


        var promise = $.Deferred<ViewModel>();
        $.when<any>(
            templateID == null ? null : Dns.WebApi.Templates.Get(templateID),
            requestTypeID == null ? ((templateID == null) ? null : Dns.WebApi.Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetTermsFilteredBy?templateID=' + templateID)) : Dns.WebApi.RequestTypes.GetFilteredTerms(requestTypeID),
            requestTypeID == null ? null : Dns.WebApi.Templates.GetByRequestType(requestTypeID),
            Dns.WebApi.Templates.List("Type eq Lpp.Dns.DTO.Enums.TemplateTypes'" + Dns.Enums.TemplateTypes.CriteriaGroup + "'", "ID,Name", "Name"),
            visualTerms == null ? GetVisualTerms() : visualTerms,
            requestTypeID == null ? null : Dns.WebApi.RequestTypes.GetRequestTypeModels(requestTypeID)
        ).done((
            queryTemplates: Dns.Interfaces.ITemplateDTO[],
            requestTypeTerms: Dns.Interfaces.IRequestTypeTermDTO[],
            requestTypeTemplates: Dns.Interfaces.ITemplateDTO[],
            criteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[],
            visualTerms: IVisualTerm[],
            models: Dns.Interfaces.IRequestTypeModelDTO[]
        ) => {
            //Load the Template Terms
            if (requestTypeTerms) {
                requestTypeTerms.forEach((term) => {
                    Plugins.Requests.QueryBuilder.MDQ.RequestTypeTermIDs.push(term.TermID);
                });
            }



            if (templateID == null) {
                //Do nothing here. The request is being loaded, and RawRequestData has already been populated.
            }
            else {
                var queryTemplate: Dns.Interfaces.ITemplateDTO = queryTemplates == null ? {
                    ID: null,
                    Name: '',
                    Description: '',
                    CreatedBy: User.AuthInfo.UserName,
                    CreatedByID: User.ID,
                    CreatedOn: moment().utc().toDate(),
                    Data: '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[] }]}}',
                    Timestamp: null,
                    Type: Dns.Enums.TemplateTypes.Request,
                    Notes: '',
                    ComposerInterface: Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery,
                } : queryTemplates[0];

                var json: any = JSON.parse(((queryTemplate.Data || '').trim() != '') ? queryTemplate.Data : '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[] }]}}');

                var jTemplate: Dns.Interfaces.IQueryComposerRequestDTO;
                if (queryTemplate.Type == Dns.Enums.TemplateTypes.CriteriaGroup) {
                    jTemplate = {
                        Header: { Name: null, Description: null, ViewUrl: null, Grammar: null, DueDate: null, Priority: null, QueryType: queryTemplate.QueryType },
                        Where: { Criteria: [<Dns.Interfaces.IQueryComposerCriteriaDTO>json] },
                        Select: { Fields: [<Dns.Interfaces.IQueryComposerFieldDTO>json] }
                    };
                } else {
                    jTemplate = json;
                }

                rawRequestData = jTemplate;
            }

            var templateNotes = '';
            var templateComposerInterface = Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery;
            if (requestTypeTemplates != null && requestTypeTemplates.length > 0) {
                if (!templateID)
                    templateID = requestTypeTemplates[0].ID;

                templateNotes = requestTypeTemplates[0].Notes || '';
                templateComposerInterface = requestTypeTemplates[0].ComposerInterface || Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery;
            }

            var bindingControl = $("#QueryComposer");

            var options: ViewModelOptions = {
                Request: rawRequestData,
                FieldOptions: fieldOptions,
                TemplateNotes: templateNotes,
                AdditionalInstructions: additionalInstructions,
                DefaultPriority: defaultPriority,
                DefaultDueDate: defaultDueDate,
                CriteriaGroupTemplates: criteriaGroupTemplates,
                ExistingRequestDataMarts: existingRequestDataMarts,
                ProjectID: projectID,
                VisualTerms: visualTerms,
                Models: models || [],
                TemplateID: templateID,
                RequestTypeID: requestTypeID,
                RequestID: requestID,
                IsTemplateEdit: isTemplateEdit,
                TemplateType: queryTemplates == null ? Dns.Enums.TemplateTypes.Request : queryTemplates[0].Type,
                TemplateComposerInterface: templateComposerInterface,
                BindingControl: bindingControl
            };

            vm = new ViewModel(options);
            ko.applyBindings(vm, bindingControl[0]);
            promise.resolve(vm);
        });
        return promise;
    };

    export function SuspendDataMartTimer() {
        if (GetDataMartTimer != null) {
            clearInterval(GetDataMartTimer);
        }
    }

    ko.bindingHandlers.AgeRangeCalculationTypeExtender = {
        init: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
            // This will be called when the binding is first applied to an element

            var value: KnockoutObservable<any> = valueAccessor();

            var isTemplateEdit = bindingContext.$root.IsTemplateEdit;
            var isRequired = bindingContext.$data.Values().CalculateAsOfRequired;

            value.subscribe((newValue) => {
                if (newValue != '7') {
                    var calculationDate: KnockoutObservable<any> = bindingContext.$data.Values().CalculateAsOf;
                    if (calculationDate() != null) {
                        calculationDate.valueWillMutate();
                        calculationDate(null);
                        calculationDate.valueHasMutated();
                    }
                }

                if (isRequired) {
                    isRequired(value() == '7' && isTemplateEdit);
                }

            });

            if (isRequired) {
                isRequired(value() == '7' && isTemplateEdit);
            }
        },
        update: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
            // This will be called once when the binding is first applied to an element,
            // and again whenever any observables/computeds that are accessed change
            // Update the DOM element based on the supplied values here.    
        }
    };

    ko.bindingHandlers.DataPartnerTypeExtender = {
        init: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
            // This will be called when the binding is first applied to an element
            var value: KnockoutObservable<any> = valueAccessor();
            var root = bindingContext.$root;

            var isTemplateEdit = bindingContext.$root.IsTemplateEdit;
            if (isTemplateEdit == false) {

                var projectID = root.ProjectID || Global.GetQueryParam("ProjectID");
                if (projectID != null) {
                    Dns.WebApi.Requests.GetOrganizationsForRequest(projectID).done((results) => {
                        root.AvailableOrganizations(results);
                    });
                }

            }

        }
    };

    //end of module
}

interface KnockoutBindingHandlers {
    AgeRangeCalculationTypeExtender: KnockoutBindingHandler;
    DataPartnerTypeExtender: KnockoutBindingHandler;
}

class CriteriaGroupSubscription {
    CriteriaGroup: Dns.ViewModels.QueryComposerCriteriaViewModel;
    Subscription: KnockoutSubscription;
}

interface IVisualTerm extends IVisualField {
    Description: string;
    Terms: IVisualTerm[];
    ValueTemplate: any;
    IncludeInCriteria: boolean;
    IncludeInStratifiers: boolean;
    IncludeInProjectors: boolean;
    Design: Dns.Interfaces.IDesignDTO;
}

class TermVm {
    Name: string;
    TermID: string;
    Description: string;
    Terms: TermVm[];
    ValueTemplate: any;
    IncludeInCriteria: boolean;
    IncludeInStratifiers: boolean;
    IncludeInProjectors: boolean;
    Allowed: KnockoutObservable<boolean>;

    constructor(term: IVisualTerm, childTerms: TermVm[], allowed: boolean) {
        this.Name = term.Name;
        this.TermID = term.TermID;
        this.Description = term.Description;
        this.Terms = childTerms;
        this.ValueTemplate = term.ValueTemplate;
        this.IncludeInCriteria = term.IncludeInCriteria;
        this.IncludeInStratifiers = term.IncludeInStratifiers;
        this.IncludeInProjectors = term.IncludeInProjectors;
        this.Allowed = ko.observable<boolean>(allowed);
    }
}

interface IVisualField {
    Name: string;
    TermID: any;
}

