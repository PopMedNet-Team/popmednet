interface KnockoutBindingHandlers {
    AgeRangeCalculationTypeExtender: KnockoutBindingHandler;
    DataPartnerTypeExtender: KnockoutBindingHandler;
}

namespace Plugins.Requests.QueryBuilder {

    export class CriteriaGroupSubscription {
        CriteriaGroup: Dns.ViewModels.QueryComposerCriteriaViewModel;
        Subscription: KnockoutSubscription;
    }

    export interface IVisualField {
        Name: string;
        TermID: any;
    }

    export interface IVisualTerm extends IVisualField {
        Description: string;
        Terms: IVisualTerm[];
        ValueTemplate: any;
        IncludeInCriteria: boolean;
        IncludeInStratifiers: boolean;
        IncludeInProjectors: boolean;
        Design: Dns.Interfaces.IDesignDTO;
    }

    export class TermVm {
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

    ko.bindingHandlers.AgeRangeCalculationTypeExtender = {
        init: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
            // This will be called when the binding is first applied to an element

            let value: KnockoutObservable<any> = valueAccessor();

            let isTemplateEdit = bindingContext.$component.Options.IsTemplateEdit;
            let isRequired = bindingContext.$data.Values().CalculateAsOfRequired;

            value.subscribe((newValue) => {
                if (newValue != '7') {
                    let calculationDate: KnockoutObservable<any> = bindingContext.$data.Values().CalculateAsOf;
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
            let value: KnockoutObservable<any> = valueAccessor();
            let root = bindingContext.$component.TermProvider;

            let isTemplateEdit = bindingContext.$component.Options.IsTemplateEdit;
            if (isTemplateEdit == false) {

                let projectID = root.ProjectID || Global.GetQueryParam("ProjectID");
                if (projectID != null) {
                    Dns.WebApi.Requests.GetOrganizationsForRequest(projectID).done((results) => {
                        root.AvailableOrganizations(results);
                    });
                }

            }

        }
    };

    /* A custom binding to allow check/uncheck all bound checkboxes for a term Value() collection property. */
    ko.bindingHandlers.CheckAllCheckboxExtender = {        
        init: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
            let settings: any = valueAccessor();

            settings.valuesCollection.subscribe((newValue) => {
                if (newValue == null || newValue.length == 0) {
                    element.checked = false;
                } else {
                    element.checked = true;
                }
            });

            element.addEventListener('change', () => {
                if (element.checked) {
                    settings.valuesCollection(ko.unwrap(settings.allValues));
                } else {
                    settings.valuesCollection([]);
                }
            });

        }
    }

    export const YearlyQuarters: string[] = ['Q1', 'Q2', 'Q3', 'Q4'];

    export class IClinicalObservationsTermValues {
        CodeSet: KnockoutObservable<Dns.Enums.ClinicalObservationsCodeSet>;
        CodeValues: KnockoutObservable<string>;
        SearchMethodType: KnockoutObservable<Dns.Enums.TextSearchMethodType>;
        ResultRangeMin: KnockoutObservable<number>;
        ResultRangeMax: KnockoutObservable<number>;
        QualitativeResult: KnockoutObservable<Dns.Enums.LOINCQualitativeResultType>;
        ResultModifier: KnockoutObservable<Dns.Enums.LOINCResultModifierType>;
    }
}