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
            
            let isTemplateEdit = bindingContext.$component.Options.IsTemplateEdit;
            if (isTemplateEdit == false) {

                let termProvider = bindingContext.$component.TermProvider;
                let projectID = bindingContext.$root.Options.ProjectID || Global.GetQueryParam("ProjectID");
                if (projectID != null) {
                    Dns.WebApi.Requests.GetOrganizationsForRequest(projectID).done((results) => {
                        termProvider.AvailableOrganizations(results);
                    });
                }
            }

        }
    };

    /* A custom binding to allow check/uncheck all bound checkboxes for a term Value() collection property. */
    ko.bindingHandlers.CheckAllCheckboxExtender = {        
        init: (element, valueAccessor, allBindings, viewModel, bindingContext) => {

           let settings: any = ko.unwrap(valueAccessor());

            //settings.valueCollection : observable containing the values of the selected options
            //settings.allValues : an array containing all the options available
            //settings.valueKey : the name of the property to use as the value for the checkboxes

            let allValues = [];
            if (settings.valueKey) {
                allValues = ko.unwrap(settings.allValues).map((v) => v[settings.valueKey]);
            } else {
                allValues = ko.unwrap(settings.allValues);
            }
            
            let allValuesLength: number = settings.allValues.length;

            const valuesCollection: KnockoutObservableArray<string> = settings.valuesCollection;
            const checkbox: HTMLInputElement = element;

            //should set initial to checked if the value collection is all the options
            checkbox.checked = allValuesLength == settings.valuesCollection().length;

            let suspendHandler: boolean = false;
            let suspendSubscription: boolean = false;

            if (ko.isSubscribable(settings.allValues)) {
                settings.allValues.subscribe((newValue) => {
                    suspendHandler = true;

                    if (settings.valueKey) {
                        allValues = ko.unwrap(newValue).map((v) => v[settings.valueKey]);
                    } else {
                        allValues = ko.unwrap(newValue);
                    }
                    
                    allValuesLength = allValues.length;
                    checkbox.checked = allValuesLength == settings.valuesCollection().length;

                    suspendHandler = false;
                });
            }

            

            let onCheckAllChanged = function () {
                if (suspendHandler)
                    return;

                suspendSubscription = true;
                //need to deep copy clone all values or else the reference is held and it becomes the values of the valuesCollection - sneaky bug...
                valuesCollection((checkbox.checked ? JSON.parse(JSON.stringify(allValues)) : []));
                suspendSubscription = false;
            };

            checkbox.addEventListener('change', onCheckAllChanged);

            settings.valuesCollection.subscribe((newValue) => {
                if (suspendSubscription)
                    return;

                suspendHandler = true;
                checkbox.checked = newValue != null && newValue.length > 0 && newValue.length == allValuesLength;
                suspendHandler = false;
            });

        }
        //update: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
        //    console.log("CheckAllCheckboxExtender.update");
        //    // This will be called once when the binding is first applied to an element,
        //    // and again whenever any observables/computeds that are accessed change
        //    // Update the DOM element based on the supplied values here.    
        //}
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