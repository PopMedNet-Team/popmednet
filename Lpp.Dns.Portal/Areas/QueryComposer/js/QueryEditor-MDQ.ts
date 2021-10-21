/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="TermValueFilter.ts" />
/// <reference path="MDQ-Common.ts" />

namespace Plugins.Requests.QueryBuilder {

    export function DumpBindingContext(parents: any[]) : string {
        if (parents == null)
            return '';
        
        let hierarchy = [];
        for (let i = 0; i < parents.length; i++) {
            hierarchy.push(parents[i].constructor.toString().match(/\w+/g)[1] + ' ( parents[' + i +'] )');
        }
        
        return hierarchy.join(' => ');
    }

    export function Inspect(obj: any) : string {
        //debugger;

        if (obj == null)
            return '';

        return obj.constructor.toString().match(/\w+/g)[1];
    }

    export interface MDQViewModelInitializationParameters {
        Query: Dns.ViewModels.QueryComposerQueryViewModel;
        FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];
        CriteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[];
        VisualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[];
        RequestTypeModelIDs: KnockoutObservableArray<any>;
        TemplateID: any;
        RequestTypeID: any;
        IsTemplateEdit: boolean;
        TemplateType: Dns.Enums.TemplateTypes;
        RequestTypeTerms: KnockoutObservableArray<Dns.ViewModels.RequestTypeTermViewModel>;
        RegisterForHiddenTermExport: (templateID: any, provider: IExportHiddenTerms) => void;
        HiddenTerms: Dns.Interfaces.ITemplateTermDTO[];
        TermsObserver: Plugins.Requests.QueryBuilder.TermsObserver
    }

    export class MDQViewModel implements IExportHiddenTerms {

        Options: MDQViewModelInitializationParameters;

        TermValidators: Object = {}; // Validation functions for criteria tab concepts

        FilteredTermList: KnockoutObservableArray<IVisualTerm>;
        CriteriaTermList: KnockoutComputed<TermVm[]>;
        StratifiersTermList: KnockoutComputed<TermVm[]>;
        TemporalEventsTermList: KnockoutComputed<TermVm[]>;
        NonAggregateFields: KnockoutComputed<Dns.ViewModels.QueryComposerFieldViewModel[]>;
        //NotAllowedTerms: KnockoutComputed<Dns.Interfaces.ISectionSpecificTermDTO[]>;
        //TemplateTerms: Dns.Interfaces.ITemplateTermDTO[] = [];
        FilteredCriteriaGroupTemplates: KnockoutComputed<Dns.Interfaces.ITemplateDTO[]>;

        IsCriteriaGroupEdit: boolean;
        TermsColumnVisible: KnockoutComputed<boolean>;
        StratifiersColumnVisible: KnockoutComputed<boolean>;
        IsPresetQuery: KnockoutComputed<boolean>;
        HasTemporalEventTerms: KnockoutComputed<boolean>;

        /*A collection of all the active term instances in the query.*/
        private _activeTerms: KnockoutObservableArray<any>;

        private readonly TermProvider: MDQ.TermProvider;

        constructor(_options: MDQViewModelInitializationParameters) {
            
            let self = this;
            this.Options = _options;

            if (!this.Options.TemplateID) {
                if (this.Options.Query.Header.ID()) {
                    this.Options.TemplateID = this.Options.Query.Header.ID();
                } else {
                    let templateID = Constants.Guid.newGuid();
                    this.Options.Query.Header.ID(templateID);
                    this.Options.TemplateID = templateID;
                }
            }

            if (this.Options.IsTemplateEdit) {
                this.Options.RegisterForHiddenTermExport(ko.unwrap(this.Options.TemplateID), this);
            }

            this.IsCriteriaGroupEdit = this.Options.TemplateType === Dns.Enums.TemplateTypes.CriteriaGroup;
            this.IsPresetQuery = ko.pureComputed(() => self.Options.Query.Header.ComposerInterface() != Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery);

            this.TermProvider = new Plugins.Requests.QueryBuilder.MDQ.TermProvider(self.Options.VisualTerms, self.Options.RequestTypeModelIDs(), self.Options.RequestTypeTerms().map(t => t.RequestTypeID()), self.Options.Query.Header.QueryType);
            //make sure any new template properties exist on the request query json terms
            this.TermProvider.TermValueFilter.ConfirmTemplatePropertiesForViewModel(self.Options.Query, self.Options.VisualTerms);
            this.FilteredTermList = this.TermProvider.FilteredTerms;
            this.TermProvider.Refresh();
            
            self.Options.RequestTypeModelIDs.subscribe((newValue) => {
                self.TermProvider.Refresh();
            });

            self.Options.Query.Header.QueryType.subscribe((newValue) => {
                self.TermProvider.Refresh();
            });

            self.Options.RequestTypeTerms.subscribe((newValue) => {
                self.TermProvider.setRestrictedTermIDs(newValue.map(rtt => rtt.TermID()));
                self.TermProvider.Refresh();
            });

            
            let currentTerms = Plugins.Requests.QueryBuilder.MDQ.TermProvider.FlattenToAllTermIDs(this.Options.Query.toData());
            this._activeTerms = ko.observableArray(currentTerms);
            this.Options.TermsObserver.RegisterTermCollection(this.Options.Query.Header.ID(), this._activeTerms);

            this.Options.Query.Where.Criteria.subscribe((value) => {
                this._activeTerms(Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetTermIDsFromCriteria(value.map(c => c.toData())));
            }, this);

            self.CriteriaTermList = ko.computed(() => {

                let resultArr = ko.utils.arrayFilter(self.FilteredTermList(), (t) => {
                    return t.IncludeInCriteria && ((t.Terms != null && t.Terms.length > 0 && t.TermID == null && ko.utils.arrayFirst(t.Terms, (tt) => tt.IncludeInCriteria) != null) || t.TermID != null);
                }).map<TermVm>((t: IVisualTerm) => {
                    let tvm: TermVm = null;
                    let childTermVM: TermVm[] = [];
                    let templateTerm: Dns.Interfaces.ITemplateTermDTO;
                    let outerAllowed: boolean = false;

                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a group of terms, like Criteria or Demographic
                        t.Terms.forEach((it) => {
                            templateTerm = ko.utils.arrayFirst(self.Options.HiddenTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == it.TermID);
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

                    templateTerm = ko.utils.arrayFirst(self.Options.HiddenTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == t.TermID);

                    if (t.TermID != null) {
                        outerAllowed = templateTerm != null ? templateTerm.Allowed : true;
                    }
                    tvm = new TermVm(t, childTermVM, outerAllowed);

                    return tvm;

                });
                
                return resultArr;
            });

            self.StratifiersTermList = ko.computed(() => {
                let resultArr = ko.utils.arrayFilter(self.FilteredTermList(), (t) => {
                    return t.IncludeInStratifiers && ((t.Terms != null && t.Terms.length > 0 && t.TermID == null && ko.utils.arrayFirst(t.Terms, (tt) => tt.IncludeInStratifiers) != null) || t.TermID != null);
                }).map<TermVm>((t: IVisualTerm) => {

                    let tvm: TermVm = null;
                    let childTermVM: TermVm[] = [];
                    let templateTerm: Dns.Interfaces.ITemplateTermDTO;
                    let outerAllowed: boolean = false;

                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a group of terms, like Criteria or Demographic
                        t.Terms.forEach((it) => {
                            templateTerm = ko.utils.arrayFirst(self.Options.HiddenTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Stratification && tt.TermID == it.TermID);
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

                    templateTerm = ko.utils.arrayFirst(self.Options.HiddenTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Stratification && tt.TermID == t.TermID);

                    if (t.TermID != null) {
                        outerAllowed = templateTerm != null ? templateTerm.Allowed : true;
                    }

                    tvm = new TermVm(t, childTermVM, outerAllowed);

                    return tvm;

                });

                return resultArr;
            });

            self.TemporalEventsTermList = ko.computed(() => {

                let resultArr = ko.utils.arrayFilter(self.FilteredTermList(), (t) => {
                    //Only Diagnosis and Procedure terms are supported for temporal events criteria.
                    return MDQ.Terms.Compare(t.TermID, MDQ.Terms.CombinedDiagnosisCodesID) || MDQ.Terms.Compare(t.TermID, MDQ.Terms.ProcedureCodesID);

                }).map<TermVm>((t: IVisualTerm) => {
                    let tvm: TermVm = null;
                    let childTermVM: TermVm[] = [];
                    let templateTerm: Dns.Interfaces.ITemplateTermDTO;
                    let outerAllowed: boolean = false;

                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a group of terms, like Criteria or Demographic
                        t.Terms.forEach((it) => {
                            templateTerm = ko.utils.arrayFirst(self.Options.HiddenTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == it.TermID);
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

                    templateTerm = ko.utils.arrayFirst(self.Options.HiddenTerms, (tt) => tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == t.TermID);

                    if (t.TermID != null) {
                        outerAllowed = templateTerm != null ? templateTerm.Allowed : true;
                    }
                    tvm = new TermVm(t, childTermVM, outerAllowed);

                    return tvm;

                });

                return resultArr;
            });

            self.HasTemporalEventTerms = ko.computed(() => {
                return self.Options.IsTemplateEdit || (ko.utils.arrayFirst(self.TemporalEventsTermList(), (ti) => ti.Allowed() != null && ti.Allowed()) != null);
            });

            self.TermsColumnVisible = ko.computed(() => {
                let isVis: boolean = false;
                if (self.Options.IsTemplateEdit)
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
                let isVis: boolean = false;
                if (self.Options.IsTemplateEdit)
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

            self.NonAggregateFields = ko.pureComputed(() => {
                //hide the aggregate fields from view since they are not editable anyhow
                return ko.utils.arrayFilter(self.Options.Query.Select.Fields(), (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Aggregate() == null; });
            });

            self.FilteredCriteriaGroupTemplates = ko.pureComputed(() => {
                let filteredCriteriaTemplates = ko.utils.arrayFilter(self.Options.CriteriaGroupTemplates, (t: Dns.Interfaces.ITemplateDTO) => {
                    if (t.QueryType == null)
                        return true;

                    return t.QueryType == self.Options.Query.Header.QueryType();
                });
                return filteredCriteriaTemplates || [];
            });

        }

        public AreTermsValid(): boolean {
            let areTermsValid: boolean = true;
            $.each(this.TermValidators, function (key, value) {
                if (!value()) {
                    areTermsValid = false;
                }
            });
            if (!areTermsValid) {
                Global.Helpers.ShowAlert("Validation Error", "One or more terms contain invalid or insufficient information.");
                return false;
            }
            return true;
        }

        public FilterTermsForCriteria(terms: TermVm[]): TermVm[] {
            return ko.utils.arrayFilter(terms, (t) => t.IncludeInCriteria);
        }

        public FilterTermsForStratification(terms: TermVm[]): TermVm[] {
            return ko.utils.arrayFilter(terms, (t) => t.IncludeInStratifiers);
        }

        public onAddCriteriaGroup() {
            let criteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
            criteria.ID(Constants.Guid.newGuid());
            criteria.Name('Group ' + (this.Options.Query.Where.Criteria().length + 1));
            criteria.Exclusion(false);
            criteria.Operator(Dns.Enums.QueryComposerOperators.And);
            criteria.IndexEvent(false);
            criteria.Type(Dns.Enums.QueryComposerCriteriaTypes.Paragraph);
            this.Options.Query.Where.Criteria.push(criteria);
        }

        public onRemoveCriteriaGroup(criteria: Dns.ViewModels.QueryComposerCriteriaViewModel) {
            //context of 'this' is vm.Options.Query[n], need to cast 'this' as any before casting as QueryComposerViewModel to satisfy typescript
            let self = <any>this as Dns.ViewModels.QueryComposerQueryViewModel;
            self.Where.Criteria.remove(criteria);
        }

        public TemplateSelector(data: Dns.ViewModels.QueryComposerTermViewModel) {
            return "e_" + data.Type();
        }

        public StratifierTemplateSelector(data: Dns.ViewModels.QueryComposerFieldViewModel) {
            return "s_" + data.Type();
        }

        public onAddTemporalEventTerm(termTemplate: IVisualTerm, temporalEvent: Dns.ViewModels.QueryComposerTemporalEventViewModel, event: JQueryEventObject) {
            let rootCriteria: Dns.ViewModels.QueryComposerCriteriaViewModel;
            //only a single root criteria will be available for Temporal Events
            if (temporalEvent.Criteria().length == 0) {
                rootCriteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                rootCriteria.ID(Constants.Guid.newGuid());
                rootCriteria.Name('Group 1');
                rootCriteria.Exclusion(false);
                rootCriteria.IndexEvent(true);
                rootCriteria.Operator(Dns.Enums.QueryComposerOperators.And);
                rootCriteria.Type(Dns.Enums.QueryComposerCriteriaTypes.IndexEvent);
                temporalEvent.Criteria.push(rootCriteria);
            } else {
                rootCriteria = temporalEvent.Criteria()[0];
            }

            const termViewModel = new Dns.ViewModels.QueryComposerTermViewModel({
                Operator: Dns.Enums.QueryComposerOperators.And,
                Type: termTemplate.TermID,
                Values: Global.Helpers.CopyObject(termTemplate.ValueTemplate),
                Criteria: null,
                Design: termTemplate.Design
            });

            //add the term to the appropriate sub-criteria if same terms are to be OR'd within the same parent criteria.
            if (MDQ.Terms.GroupedTerms.indexOf(termTemplate.TermID.toUpperCase()) >= 0) {

                //the terms should be OR's together and the criteria AND'd to other sub-criteria and parent criteria terms.
                termViewModel.Operator(Dns.Enums.QueryComposerOperators.Or);

                //find the first sub-criteria that contains a matching term (each sub-criteria should only contain terms of the same type at this time)
                let criteria: Dns.ViewModels.QueryComposerCriteriaViewModel = ko.utils.arrayFirst(rootCriteria.Criteria(), (c) => {
                    let t = ko.utils.arrayFirst(c.Terms(), (tt) => true);
                    if (t == null)
                        return false;

                    //if the term is a noncode term compare the first term in the subcriteria, for these all the terms should be the same.
                    if (MDQ.Terms.NonCodeGroupedTerms.indexOf(termTemplate.TermID.toUpperCase()) >= 0) {
                        return MDQ.Terms.NonCodeGroupedTerms.indexOf(t.Type().toUpperCase()) >= 0 && MDQ.Terms.Compare(t.Type(), termTemplate.TermID);
                    }

                    //if it is a noncode term return if the first term of the sub-criteria is a noncode term or not.
                    //if the term is a code term it should return true.
                    return MDQ.Terms.NonCodeGroupedTerms.indexOf(t.Type().toUpperCase()) < 0;

                });

                if (criteria == null) {

                    criteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                    criteria.Operator(Dns.Enums.QueryComposerOperators.And);
                    criteria.Type(Dns.Enums.QueryComposerCriteriaTypes.Paragraph);
                    criteria.ID(Constants.Guid.newGuid());
                    criteria.Name(MDQ.Terms.NonCodeGroupedTerms.indexOf(termTemplate.TermID.toUpperCase()) >= 0 ? 'i_' + termTemplate.Name.replace(' ', '_') : 'i_codeterms');
                    criteria.Terms.push(termViewModel);

                    rootCriteria.Criteria.push(criteria);

                } else {
                    criteria.Terms.push(termViewModel);
                }


            } else {
                rootCriteria.Terms.push(termViewModel);
            }
        }

        public onAddTermToCriteria(termTemplate: IVisualTerm, criteria: Dns.ViewModels.QueryComposerCriteriaViewModel, event: JQueryEventObject) {
            //context of 'this' is the MDQViewModel instance
            let self = this;

            //Update 2014-12-04: Reverted to use CopyObject to create the Value Template to ensure properties are marked as oservables.
            //As observables, the values entered via Term Templates are saved properly.

            ////using Page.ObjectCopy was resulting in all the properties of termValues getting converted into observables.
            ////The the constructor of QueryComposerTermViewModel does not support this, doing plain copy to match what is happening in generated ViewModel.
            //var termValues = <any>{};
            //for (var prop in data.ValueTemplate) {
            //    termValues[prop] = data.ValueTemplate[prop];
            //}

            let termValues = Global.Helpers.CopyObject(termTemplate.ValueTemplate);
            let termViewModel = new Dns.ViewModels.QueryComposerTermViewModel({
                Operator: Dns.Enums.QueryComposerOperators.And,
                Type: termTemplate.TermID,
                Values: termValues,
                Criteria: null,
                Design: termTemplate.Design
            });

            //add the term to the appropriate sub-criteria if same terms are to be OR'd within the same parent criteria.
            if (MDQ.Terms.GroupedTerms.indexOf(termTemplate.TermID.toUpperCase()) >= 0) {

                //the terms should be OR's together and the criteria AND'd to other sub-criteria and parent criteria terms.
                termViewModel.Operator(Dns.Enums.QueryComposerOperators.Or);

                //find the first sub-criteria that contains a matching term (each sub-criteria should only contain terms of the same type at this time)
                let subCriteria: Dns.ViewModels.QueryComposerCriteriaViewModel = ko.utils.arrayFirst(criteria.Criteria(), (c) => {
                    let t = ko.utils.arrayFirst(c.Terms(), (tt) => true);
                    if (t == null)
                        return false;

                    //if the term is a noncode term compare the first term in the subcriteria, for these all the terms should be the same.
                    if (MDQ.Terms.NonCodeGroupedTerms.indexOf(termTemplate.TermID.toUpperCase()) >= 0) {
                        return MDQ.Terms.NonCodeGroupedTerms.indexOf(t.Type().toUpperCase()) >= 0 && t.Type().toUpperCase() == termTemplate.TermID.toUpperCase();
                    }

                    //if it is a noncode term return if the first term of the sub-criteria is a noncode term or not.
                    //if the term is a code term it should return true.
                    return MDQ.Terms.NonCodeGroupedTerms.indexOf(t.Type().toUpperCase()) < 0;

                });

                if (subCriteria == null) {

                    subCriteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                    subCriteria.Operator(Dns.Enums.QueryComposerOperators.And);
                    subCriteria.Type(Dns.Enums.QueryComposerCriteriaTypes.Paragraph);
                    subCriteria.ID(Constants.Guid.newGuid());
                    subCriteria.Name(MDQ.Terms.NonCodeGroupedTerms.indexOf(termTemplate.TermID.toUpperCase()) >= 0 ? 'i_' + termTemplate.Name.replace(' ', '_') : 'i_codeterms');
                    subCriteria.Terms.push(termViewModel);

                    criteria.Criteria.push(subCriteria);

                } else {
                    subCriteria.Terms.push(termViewModel);
                }


            } else {
                criteria.Terms.push(termViewModel);
            }

            //push for the criteria term being added
            self._activeTerms.push(termViewModel.Type());

            if (termTemplate.IncludeInStratifiers == false)
                return;


            let fieldsToAdd: Dns.ViewModels.QueryComposerFieldViewModel[] = [];

            let selectFields = self.Options.Query.Select.Fields();

            //specify the field as group by and included in the select
            let foundStratifyByField = ko.utils.arrayFirst(selectFields, (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Type().toUpperCase() == termTemplate.TermID.toUpperCase() && item.Aggregate() == null });
            if (foundStratifyByField == null) {
                fieldsToAdd.push(new Dns.ViewModels.QueryComposerFieldViewModel({
                    Type: termTemplate.TermID,
                    FieldName: termTemplate.Name,
                    Aggregate: null,
                    StratifyBy: null,
                    OrderBy: Dns.Enums.OrderByDirections.None,
                    Select: null,
                    GroupBy: null,
                }));
            }

            //any field included in the criteria is included as a count
            let foundCountField = ko.utils.arrayFirst(selectFields, (item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Type().toUpperCase() == termTemplate.TermID.toUpperCase() && item.Aggregate() != null; });
            if (foundCountField == null) {
                fieldsToAdd.push(new Dns.ViewModels.QueryComposerFieldViewModel({
                    Type: termTemplate.TermID,
                    Aggregate: Dns.Enums.QueryComposerAggregates.Count,
                    FieldName: termTemplate.Name + '_Count',
                    GroupBy: null,
                    StratifyBy: null,
                    OrderBy: Dns.Enums.OrderByDirections.None,
                    Select: null
                }));
            }

            if (fieldsToAdd.length > 0) {

                for (let i = 0; i < fieldsToAdd.length; i++) {

                    self.Options.Query.Select.Fields.push(fieldsToAdd[i]);

                    //push for each of the stratification terms being added
                    self._activeTerms.push(fieldsToAdd[i].Type());
                }
            }

        }

        public onAddCommonCodeToClinicalObservations(codeType: string) {
            
            let self = <any>this as Dns.ViewModels.QueryComposerTermViewModel;
            let codeValue = "";
            switch (codeType.toUpperCase()) {
                case "HEIGHT":
                    codeValue = "3137-7";
                    break;
                case "WEIGHT":
                    codeValue = "3141-9";
                    break;
                case "BMI":
                    codeValue = "39156-5";
                    break;
                case "SMOKING":
                    codeValue = "72166-2";
                    break;
                default:
                    return;
            }

            let termValue = self.Values() as IClinicalObservationsTermValues;
            let currentCodes = termValue.CodeValues();

            if (currentCodes.indexOf(codeValue) >= 0)
                return;

            if (currentCodes.length > 0 && currentCodes.charAt(currentCodes.length - 1) != ';') {
                currentCodes += ';';
            }

            currentCodes += (codeValue + ';');

            termValue.CodeValues(currentCodes);


            //this == the Dns.ViewModels.QueryComposerTermViewModel
            //data == the string descriptor value
            //codeType == this
        }

        public onDeleteTermFromCriteria(data: Dns.ViewModels.QueryComposerTermViewModel, criteriaGroup: Dns.ViewModels.QueryComposerCriteriaViewModel) {
            //context of 'this' is vm.Options.Query[n], need to cast 'this' as any before casting as QueryComposerViewModel to satisfy typescript
            let self = <any>this as MDQViewModel;

            //SuspendDataMartTimer();

            /** only removes from the criteria **/
            criteriaGroup.Terms.remove(data);
            
            /** remove the criteria if it is a sub-criteria and is empty **/
            ko.utils.arrayForEach(self.Options.Query.Where.Criteria(), (c) => {

                let subCriteriaToRemove = ko.utils.arrayFilter(c.Criteria(), (sc) => sc.Terms().length == 0);

                subCriteriaToRemove.forEach((sc) => {

                    c.Criteria.remove(sc);

                });
            });

            //remove 1 instance of the termID being removed from the query
            let indexOfTermID = self._activeTerms.indexOf(data.Type());
            if (indexOfTermID >= 0) {
                self._activeTerms.splice(indexOfTermID, 1);
            }

            //clean up TemporalEvents Criteria
            if (self.Options.Query.TemporalEvents() != null || self.Options.Query.TemporalEvents().length > 0) {
                ko.utils.arrayForEach(self.Options.Query.TemporalEvents(), (tevt) => {
                    let criteriaToDelete: Dns.ViewModels.QueryComposerCriteriaViewModel[] = [];

                    ko.utils.arrayForEach(tevt.Criteria(), (rootCriteria) => {
                        let subCriteriaToRemove = ko.utils.arrayFilter(rootCriteria.Criteria(), (subCrit) => subCrit.Terms().length == 0);
                        if (subCriteriaToRemove.length > 0) {
                            rootCriteria.Criteria.removeAll(subCriteriaToRemove);
                        }

                        if (rootCriteria.Criteria().length == 0 && rootCriteria.Terms().length == 0) {
                            criteriaToDelete.push(rootCriteria);
                        }
                    });

                    let emptyCriteria = ko.utils.arrayFilter(tevt.Criteria(), (cr) => cr.Criteria().length == 0 && cr.Terms().length == 0);
                    tevt.Criteria.removeAll(emptyCriteria);
                });
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

        public AddField(data: IVisualField, parent: Dns.ViewModels.QueryComposerSelectViewModel, event: JQueryEventObject) {
            
            let fieldsToAdd: Dns.ViewModels.QueryComposerFieldViewModel[] = [];

            let selectFields = parent.Fields();

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

            if (fieldsToAdd.length > 0) {
                for (let i = 0; i < fieldsToAdd.length; i++) {

                    parent.Fields.push(fieldsToAdd[i]);

                    //add the ID of the term for each term instance being added to the query
                    this._activeTerms.push(fieldsToAdd[i].Type());
                }                
            }

            if (MDQ.Terms.Compare(data.TermID, MDQ.Terms.PatientReportedOutcomeEncounterID) && this.Options.Query.TemporalEvents().length == 0) {
                let dto = new Dns.ViewModels.QueryComposerTemporalEventViewModel();
                dto.IndexEventDateIdentifier("HOSPITALIZATION_DATE");
                dto.DaysBefore(7);
                dto.DaysAfter(7);
                this.Options.Query.TemporalEvents.push(dto);
            }

        }

        public onDeleteField(data: Dns.ViewModels.QueryComposerFieldViewModel, parent: Dns.ViewModels.QueryComposerSelectViewModel) {
            //remove the stratifier and count fields for the term type
            let fieldsToDelete = ko.utils.arrayFilter(parent.Fields(), (f) => f.Type() == data.Type());
            if (fieldsToDelete.length > 0) {
                parent.Fields.removeAll(fieldsToDelete);

                for (let i = 0; i < fieldsToDelete.length; i++) {
                    let indexOfTermID = this._activeTerms.indexOf(fieldsToDelete[i].Type());
                    if (indexOfTermID >= 0) {
                        this._activeTerms.splice(indexOfTermID, 1);
                    }
                }
            }

            if (MDQ.Terms.Compare(data.Type(), MDQ.Terms.PatientReportedOutcomeEncounterID)) {
                this.Options.Query.TemporalEvents.removeAll();
            }
        }

        public AgeRangeCalculationSelections(): Dns.Structures.KeyValuePair[] {
            
            //editViewModel: ViewModel ==> the bound context will be the MDQQueryViewModel

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

        /**
         * Loops through all Element nodes of the template after render and update's the declare identifying attributes with a prefix unique to the instantiated template.
         * @param nodes The collection of DOMnodes of the rendered template.
         */
        public onUpdateTemplateElements(nodes: Node[]) {
            if (nodes == null || nodes.length == 0)
                return;

            let prefix: string = Constants.Guid.newGuid() + '_';

            let updateAttributeValue = (elmt: Element, key: string) => {
                if (elmt.hasAttribute(key)) {
                    let v = elmt.getAttribute(key);
                    if (v && v.trim().length > 0) {
                        elmt.setAttribute(key, prefix + v);
                    }
                }
            };

            //recursive function to prepend the prefix to all defined id using attributes
            let updateElementID = (element: Element) => {
                let attr = element.id;
                if (attr && attr.trim().length > 0) {
                    element.id = prefix + attr;
                }

                updateAttributeValue(element, "for");
                updateAttributeValue(element, "name");
                updateAttributeValue(element, "aria-labelledby");
                updateAttributeValue(element, "data-for");                

                if (element.children.length > 0) {
                    for (let i = 0; i < element.children.length; i++) {
                        updateElementID(element.children.item(i));
                    }
                }
            };            

            nodes.forEach((node: Node) => {
                if (node.nodeType != 1)
                    return;

                let element = node as Element;
                updateElementID(element);

            });
        }

        public ConfirmCombinedCodeTypeChanged(data: Dns.ViewModels.QueryComposerTermViewModel, e: kendo.ui.DropDownListSelectEvent) {
            if (data.Values == null || data.Values().CodeValues == null || data.Values().CodeValues() == null || data.Values().CodeValues().length == 0)
                return;

            let oldValue = e.sender.value();
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

            let oldValue = e.sender.value();
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
            let existingValues: string[] = null;

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
                let codes = ko.utils.arrayMap(results, (item) => item.Code).join('; ');

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

            let oldValue = e.sender.value();
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
            let existingValues: string[] = null;

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
                    let codes = ko.utils.arrayMap(results, (item) => item.Code).join('; ');

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
                    let codes = ko.utils.arrayMap(results, (item) => item.Code).join('; ');

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

        public OpenCodeSelector(data: Dns.ViewModels.QueryComposerTermViewModel, list: Dns.Enums.Lists, showCategoryDropdown?: boolean) {
            Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                ListID: list,
                Codes: data.Values == null || data.Values().CodeValues == null ? "" : data.Values().CodeValues(),
                ShowCategoryDropdown: showCategoryDropdown == null ? true : showCategoryDropdown
            }).done((results: Dns.Interfaces.ICodeSelectorValueDTO[]) => {
                if (!results)
                    return; //User clicked cancel

                data.Values.valueWillMutate();

                let codes = ko.utils.arrayMap(results, (item: Dns.Interfaces.ICodeSelectorValueDTO) => <Dns.Interfaces.ICodeSelectorValueDTO>{ Code: item.Code.trim(), Name: item.Name.trim(), ExpireDate: null });

                if (data.Values().CodeValues == null)
                    data.Values().CodeValues = [];

                data.Values().CodeValues.removeAll();
                codes.forEach((code) => {
                    data.Values().CodeValues.push(code);
                });

                data.Values.valueHasMutated();
            });
        }

        public onUploadCodeList() {
            let self = <any>this as Dns.ViewModels.QueryComposerQueryViewModel;
            
            Global.Helpers.ShowDialog("Import Code Values", "/QueryComposer/UploadCodeList", [], 670, 450, null).done((importResponse: Plugins.Requests.QueryBuilder.UploadCodeList.ResponseDTO) => {

                if (importResponse.Status === "Complete" && importResponse.Result != null && importResponse.Result.length > 0) {

                    let replaceTerms = importResponse.Criteria === 'Append' ? false : true;
                    let existingCriteriaCollection = self.Where.Criteria();

                    let containsCaseInsensitiveValue = (collection: string[], searchFor: string): boolean => {
                        if (searchFor == null || searchFor.length == 0)
                            return false;

                        for (let i = 0; i < collection.length; i++) {
                            if (searchFor.localeCompare(collection[i], 'en', { usage: 'search', sensitivity: 'accent' }) == 0)
                                return true;
                        }

                        return false;
                    }

                    //if the option is to replace, all existing code terms should be removed first, then the new terms imported
                    if (replaceTerms) {
                        existingCriteriaCollection.forEach((criteria) => {
                            let existingCodeTermSubCriteria = ko.utils.arrayFirst(criteria.Criteria(), (item) => { return item.Name() === "i_codeterms" });
                            if (existingCriteriaCollection) {
                                criteria.Criteria.remove(existingCodeTermSubCriteria);
                            }
                        });
                    }

                    let criteriaToImport: Dns.Interfaces.IQueryComposerCriteriaDTO[] = importResponse.Result as Dns.Interfaces.IQueryComposerCriteriaDTO[];

                    //foreach root criteria to import
                    for (let criteriaIndex = 0; criteriaIndex < criteriaToImport.length; criteriaIndex++) {
                        let importRootCriteria = criteriaToImport[criteriaIndex];

                        let existingRootCriteria: Dns.ViewModels.QueryComposerCriteriaViewModel = null;
                        if (existingCriteriaCollection.length > criteriaIndex) {
                            existingRootCriteria = existingCriteriaCollection[criteriaIndex];
                        } else {
                            //need to add a root criteria to the existing criteria collection
                            existingRootCriteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                            existingRootCriteria.ID(Constants.Guid.newGuid());
                            existingRootCriteria.Name('Group ' + (criteriaIndex + 1));
                            existingCriteriaCollection.push(existingRootCriteria);
                        }

                        //if the import root criteria is null do not alter the existing collection of criteria except to add a new one to keep in line with the criteria index
                        if (importRootCriteria == null)
                            continue;

                        let existingCodeTermSubCriteria = ko.utils.arrayFirst(existingRootCriteria.Criteria(), (item) => { return item.Name() === "i_codeterms" });
                        if (existingCodeTermSubCriteria == null) {

                            existingCodeTermSubCriteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                            existingCodeTermSubCriteria.ID(Constants.Guid.newGuid());
                            existingCodeTermSubCriteria.Operator(Dns.Enums.QueryComposerOperators.And);
                            existingCodeTermSubCriteria.Type(Dns.Enums.QueryComposerCriteriaTypes.Paragraph);
                            existingCodeTermSubCriteria.Name('i_codeterms');
                            existingCodeTermSubCriteria.Terms = ko.observableArray<Dns.ViewModels.QueryComposerTermViewModel>([]);

                            existingRootCriteria.Criteria.push(existingCodeTermSubCriteria);
                        }

                        let importCriteria = importRootCriteria.Criteria[0];

                        //make sure there are terms to import
                        if (importCriteria.Terms == null || importCriteria.Terms.length == 0)
                            continue;

                        let codeTerms: Dns.ViewModels.QueryComposerTermViewModel[] = [];
                        ko.utils.arrayForEach(existingCodeTermSubCriteria.Terms(), (existingTerm) => {

                            //replace or append to terms that already exist with the import term values
                            let importTerm = ko.utils.arrayFirst(importCriteria.Terms, (term) => (term.Type || '').localeCompare((existingTerm.Type() || ''), 'en', { usage: 'search', sensitivity: 'accent' }) == 0 && term.Values.CodeType === existingTerm.Values().CodeType() && term.Values.SearchMethodType === existingTerm.Values().SearchMethodType());

                            if (importTerm) {

                                if (replaceTerms) {

                                    existingTerm.Values().CodeValues(importTerm.Values.CodeValues);

                                } else {

                                    let currentCodes: string[] = (<string>existingTerm.Values().CodeValues()).split(';').map(c => (c || '').trim());
                                    let importCodesValues = importTerm.Values.CodeValues.split(';');
                                    ko.utils.arrayForEach(importCodesValues, (c: string) => {
                                        if (containsCaseInsensitiveValue(currentCodes, (c || '').trim()) == false) {
                                            currentCodes.push(c);
                                        }
                                    });

                                    existingTerm.Values().CodeValues(currentCodes.join('; '));
                                }

                            }

                            codeTerms.push(existingTerm);

                        });

                        //add any import terms that do not existing in the modified collection of terms
                        ko.utils.arrayForEach(importCriteria.Terms, (importTerm) => {

                            let existingTerm = ko.utils.arrayFirst(codeTerms, (term) => (importTerm.Type || '').localeCompare((term.Type() || ''), 'en', { usage: 'search', sensitivity: 'accent' }) == 0 && importTerm.Values.CodeType === term.Values().CodeType() && importTerm.Values.SearchMethodType === term.Values().SearchMethodType());
                            if (existingTerm != null) {
                                //term should have already been updated with the import values
                                return;
                            }

                            //add the term
                            let importTermViewModel = new Dns.ViewModels.QueryComposerTermViewModel(importTerm);
                            //convert the term values to observables
                            importTermViewModel.Values(Global.Helpers.CopyObject(importTerm.Values));
                            importTermViewModel.Operator(Dns.Enums.QueryComposerOperators.Or);

                            codeTerms.push(importTermViewModel);
                        });

                        //replace the existing terms collection with the updated terms collection
                        existingCodeTermSubCriteria.Terms(codeTerms);
                    }

                    self.Where.Criteria(existingCriteriaCollection);

                    //self.GetCompatibleDataMarts();

                }//end of successful parsing response guard clause
            }).fail((err) => {
                debugger;
            });//end of the import promise
        }

        public onSaveCriteriaGroup(criteriaGroup: Dns.ViewModels.QueryComposerCriteriaViewModel) {
            
            let params = {
                CriteriaGroup: criteriaGroup.toData(),
                AdapterDetail: this.Options.Query.Header.QueryType()
            };

            Global.Helpers.ShowDialog("Save Criteria Group", "/querycomposer/savecriteriagroup", ["close"], 650, 400, params);

        }

        public ApplyCriteriaGroupTemplate(criteriaGroup: Dns.ViewModels.QueryComposerCriteriaViewModel, template: Dns.Interfaces.ITemplateDTO) {
            let criteria = criteriaGroup;
            let criteriaTemplate = template;
            Global.Helpers.ShowConfirm("Apply Criteria Group Template", '<div class="alert alert-warning"><p style="text-align:center;">Apply criteria group template <strong>"' + template.Name + '"</strong> to criteria group <strong>"' + criteriaGroup.Name() + '"</strong>?<br/><br/><strong>This will replace all existing terms in criteria group "' + criteriaGroup.Name() + '".</strong><br/>Selected Stratifications will not be affected by the term import.</p></div>')
                .done(() => {

                    let templateDTO = JSON.parse(criteriaTemplate.Data) as Dns.Interfaces.IQueryComposerCriteriaDTO;
                    let templateViewModel = new Dns.ViewModels.QueryComposerCriteriaViewModel(templateDTO);
                    this.TermProvider.TermValueFilter.ConfirmCriteriaForViewModel(templateViewModel, this.Options.VisualTerms);

                    criteria.Criteria(templateViewModel.Criteria());
                    criteria.Terms(templateViewModel.Terms());

                });
        }

        /* Exports a collection of terms that are not allowed to be used/shown during request compostion. */
        public ExportHiddenTerms(): Dns.Interfaces.ISectionSpecificTermDTO[] {

            let hiddenTerms: Dns.Interfaces.ISectionSpecificTermDTO[] = [];

            let filterHiddenTerms = (templateID: any, terms: TermVm[], section: Dns.Enums.QueryComposerSections) => {
                templateID = ko.unwrap(templateID);
                ko.utils.arrayForEach(terms, (t) => {
                    if (t.Terms != null && t.Terms.length > 0) {
                        //it's a category term within the term menu
                        t.Terms.forEach((innerTerm) => {
                            if (innerTerm.Allowed() == false) {
                                hiddenTerms.push({
                                    TemplateID: templateID,
                                    TermID: innerTerm.TermID,
                                    Section: section
                                });
                            }
                        });
                    } else {
                        if (t.Allowed() == false) {
                            hiddenTerms.push({
                                TemplateID: templateID,
                                TermID: t.TermID,
                                Section: section
                            });
                        }
                    }

                });
            };

            filterHiddenTerms(this.Options.TemplateID, this.CriteriaTermList(), Dns.Enums.QueryComposerSections.Criteria);
            filterHiddenTerms(this.Options.TemplateID, this.StratifiersTermList(), Dns.Enums.QueryComposerSections.Stratification);

            return hiddenTerms;
        }

    }
}