/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="./MDQ.ts" />
var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var MDQ;
            (function (MDQ) {
                var ImportCodeListHelper = /** @class */ (function () {
                    function ImportCodeListHelper(view) {
                        this.viewModel = view;
                    }
                    /**
                     * Imports the code terms defined in the import criteria into the existing criteria for the request
                     * @param criteriaToImport  The criteria containing the code terms to import
                     * @param replaceTerms      Indicates if existing code terms should be replaced or have the import values appended
                     */
                    ImportCodeListHelper.prototype.ImportCodeList = function (criteriaToImport, replaceTerms) {
                        this.ClearSubscriptions();
                        var existingCriteriaCollection = this.viewModel.Request.Where.Criteria();
                        var containsCaseInsensitiveValue = function (collection, searchFor) {
                            if (searchFor == null || searchFor.length == 0)
                                return false;
                            for (var i = 0; i < collection.length; i++) {
                                if (searchFor.localeCompare(collection[i], 'en', { usage: 'search', sensitivity: 'accent' }) == 0)
                                    return true;
                            }
                            return false;
                        };
                        //if the option is to replace, all existing code terms should be removed first, then the new terms imported
                        if (replaceTerms) {
                            existingCriteriaCollection.forEach(function (criteria) {
                                var existingCodeTermSubCriteria = ko.utils.arrayFirst(criteria.Criteria(), function (item) { return item.Name() === "i_codeterms"; });
                                if (existingCriteriaCollection) {
                                    criteria.Criteria.remove(existingCodeTermSubCriteria);
                                }
                            });
                        }
                        var _loop_1 = function (criteriaIndex) {
                            var importRootCriteria = criteriaToImport[criteriaIndex];
                            var existingRootCriteria = null;
                            if (existingCriteriaCollection.length > criteriaIndex) {
                                existingRootCriteria = existingCriteriaCollection[criteriaIndex];
                            }
                            else {
                                //need to add a root criteria to the existing criteria collection
                                existingRootCriteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                                existingRootCriteria.ID(Constants.Guid.newGuid());
                                existingRootCriteria.Name('Group ' + (criteriaIndex + 1));
                                existingCriteriaCollection.push(existingRootCriteria);
                            }
                            //if the import root criteria is null do not alter the existing collection of criteria except to add a new one to keep in line with the criteria index
                            if (importRootCriteria == null)
                                return "continue";
                            var existingCodeTermSubCriteria = ko.utils.arrayFirst(existingRootCriteria.Criteria(), function (item) { return item.Name() === "i_codeterms"; });
                            if (existingCodeTermSubCriteria == null) {
                                existingCodeTermSubCriteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                                existingCodeTermSubCriteria.ID(Constants.Guid.newGuid());
                                existingCodeTermSubCriteria.Operator(Dns.Enums.QueryComposerOperators.And);
                                existingCodeTermSubCriteria.Type(Dns.Enums.QueryComposerCriteriaTypes.Paragraph);
                                existingCodeTermSubCriteria.Name('i_codeterms');
                                existingCodeTermSubCriteria.Terms = ko.observableArray([]);
                                existingRootCriteria.Criteria.push(existingCodeTermSubCriteria);
                            }
                            var importCriteria = importRootCriteria.Criteria[0];
                            //make sure there are terms to import
                            if (importCriteria.Terms == null || importCriteria.Terms.length == 0)
                                return "continue";
                            var codeTerms = [];
                            ko.utils.arrayForEach(existingCodeTermSubCriteria.Terms(), function (existingTerm) {
                                //replace or append to terms that already exist with the import term values
                                var importTerm = ko.utils.arrayFirst(importCriteria.Terms, function (term) { return (term.Type || '').localeCompare((existingTerm.Type() || ''), 'en', { usage: 'search', sensitivity: 'accent' }) == 0 && term.Values.CodeType === existingTerm.Values().CodeType() && term.Values.SearchMethodType === existingTerm.Values().SearchMethodType(); });
                                if (importTerm) {
                                    if (replaceTerms) {
                                        existingTerm.Values().CodeValues(importTerm.Values.CodeValues);
                                    }
                                    else {
                                        var currentCodes_1 = existingTerm.Values().CodeValues().split(';').map(function (c) { return (c || '').trim(); });
                                        var importCodesValues = importTerm.Values.CodeValues.split(';');
                                        ko.utils.arrayForEach(importCodesValues, function (c) {
                                            if (containsCaseInsensitiveValue(currentCodes_1, (c || '').trim()) == false) {
                                                currentCodes_1.push(c);
                                            }
                                        });
                                        existingTerm.Values().CodeValues(currentCodes_1.join('; '));
                                    }
                                }
                                codeTerms.push(existingTerm);
                            });
                            //add any import terms that do not existing in the modified collection of terms
                            ko.utils.arrayForEach(importCriteria.Terms, function (importTerm) {
                                var existingTerm = ko.utils.arrayFirst(codeTerms, function (term) { return (importTerm.Type || '').localeCompare((term.Type() || ''), 'en', { usage: 'search', sensitivity: 'accent' }) == 0 && importTerm.Values.CodeType === term.Values().CodeType() && importTerm.Values.SearchMethodType === term.Values().SearchMethodType(); });
                                if (existingTerm != null) {
                                    //term should have already been updated with the import values
                                    return;
                                }
                                //add the term
                                var importTermViewModel = new Dns.ViewModels.QueryComposerTermViewModel(importTerm);
                                //convert the term values to observables
                                importTermViewModel.Values(Global.Helpers.CopyObject(importTerm.Values));
                                importTermViewModel.Operator(Dns.Enums.QueryComposerOperators.Or);
                                codeTerms.push(importTermViewModel);
                            });
                            //replace the existing terms collection with the updated terms collection
                            existingCodeTermSubCriteria.Terms(codeTerms);
                        };
                        //foreach root criteria to import
                        for (var criteriaIndex = 0; criteriaIndex < criteriaToImport.length; criteriaIndex++) {
                            _loop_1(criteriaIndex);
                        }
                        this.AddSubscriptions(existingCriteriaCollection);
                        this.viewModel.Request.Where.Criteria(existingCriteriaCollection);
                    };
                    ImportCodeListHelper.prototype.ClearSubscriptions = function () {
                        ko.utils.arrayForEach(this.viewModel.SubscriptionsArray(), function (subscription) {
                            subscription.Subscription.dispose();
                        });
                        this.viewModel.SubscriptionsArray([]);
                    };
                    ImportCodeListHelper.prototype.AddSubscriptions = function (criteriaCollection) {
                        var _this = this;
                        var subscriptions = [];
                        criteriaCollection.forEach(function (rootCriteria) {
                            //subscribe for the root criteria terms collection changes
                            subscriptions.push({
                                CriteriaGroup: rootCriteria,
                                Subscription: rootCriteria.Terms.subscribe(function () {
                                    MDQ.GetDataMartTimer = setInterval((function () { return _this.viewModel.GetCompatibleDataMarts(); }).bind(_this.viewModel), 2000);
                                })
                            });
                            //subscribe for the sub-criteria colletion change (ie, adding removing a sub-criteria)
                            subscriptions.push({
                                CriteriaGroup: rootCriteria,
                                Subscription: rootCriteria.Criteria.subscribe(function () {
                                    MDQ.GetDataMartTimer = setInterval((function () { return _this.viewModel.GetCompatibleDataMarts(); }).bind(_this.viewModel), 2000);
                                })
                            });
                            //subscribe to the terms for each subcriteria
                            ko.utils.arrayForEach(rootCriteria.Criteria(), function (subcriteria) {
                                subscriptions.push({
                                    CriteriaGroup: subcriteria,
                                    Subscription: subcriteria.Terms.subscribe(function () {
                                        MDQ.GetDataMartTimer = setInterval((function () { return _this.viewModel.GetCompatibleDataMarts(); }).bind(_this.viewModel), 2000);
                                    })
                                });
                            });
                        });
                        this.viewModel.SubscriptionsArray(subscriptions);
                    };
                    return ImportCodeListHelper;
                }());
                MDQ.ImportCodeListHelper = ImportCodeListHelper;
            })(MDQ = QueryBuilder.MDQ || (QueryBuilder.MDQ = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
