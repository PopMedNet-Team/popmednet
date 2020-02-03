/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="./MDQ.ts" />
namespace Plugins.Requests.QueryBuilder.MDQ {

    export class ImportCodeListHelper {
        private viewModel: Plugins.Requests.QueryBuilder.MDQ.ViewModel;

        constructor(view: Plugins.Requests.QueryBuilder.MDQ.ViewModel) {
            this.viewModel = view;
        }

        /**
         * Imports the code terms defined in the import criteria into the existing criteria for the request
         * @param criteriaToImport  The criteria containing the code terms to import
         * @param replaceTerms      Indicates if existing code terms should be replaced or have the import values appended
         */
        public ImportCodeList(criteriaToImport: Dns.Interfaces.IQueryComposerCriteriaDTO[], replaceTerms: boolean): void {
            this.ClearSubscriptions();

            let existingCriteriaCollection = this.viewModel.Request.Where.Criteria();

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

                    let existingTerm = ko.utils.arrayFirst(codeTerms, (term) => (importTerm.Type || '').localeCompare((term.Type() || ''), 'en', { usage: 'search', sensitivity: 'accent' }) == 0  && importTerm.Values.CodeType === term.Values().CodeType() && importTerm.Values.SearchMethodType === term.Values().SearchMethodType());
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

            this.AddSubscriptions(existingCriteriaCollection);
            this.viewModel.Request.Where.Criteria(existingCriteriaCollection);
        }

        private ClearSubscriptions() {
            
            ko.utils.arrayForEach(this.viewModel.SubscriptionsArray(), (subscription: CriteriaGroupSubscription) => {
                subscription.Subscription.dispose();
            });

            this.viewModel.SubscriptionsArray([]);
        }

        private AddSubscriptions(criteriaCollection: Dns.ViewModels.QueryComposerCriteriaViewModel[]) {
            let subscriptions: CriteriaGroupSubscription[] = [];
            
            criteriaCollection.forEach((rootCriteria) => {
                //subscribe for the root criteria terms collection changes
                subscriptions.push({
                    CriteriaGroup: rootCriteria,
                    Subscription: rootCriteria.Terms.subscribe(() => {
                        GetDataMartTimer = setInterval((() => this.viewModel.GetCompatibleDataMarts()).bind(this.viewModel), 2000);
                    })
                });

                //subscribe for the sub-criteria colletion change (ie, adding removing a sub-criteria)
                subscriptions.push({
                    CriteriaGroup: rootCriteria,
                    Subscription: rootCriteria.Criteria.subscribe(() => {
                        GetDataMartTimer = setInterval((() => this.viewModel.GetCompatibleDataMarts()).bind(this.viewModel), 2000);
                    })
                });

                //subscribe to the terms for each subcriteria
                ko.utils.arrayForEach(rootCriteria.Criteria(), (subcriteria) => {
                    subscriptions.push({
                        CriteriaGroup: subcriteria,
                        Subscription: subcriteria.Terms.subscribe(() => {
                            GetDataMartTimer = setInterval((() => this.viewModel.GetCompatibleDataMarts()).bind(this.viewModel), 2000);
                        })
                    });
                });
            });

            this.viewModel.SubscriptionsArray(subscriptions);

        }
    }
}