/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="termvaluefilter.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var MDQ;
            (function (MDQ) {
                //this is the filtered list of terms for the request type
                MDQ.RequestTypeTermIDs = [];
                var ViewModel = /** @class */ (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(options) {
                        var _this = _super.call(this, options.BindingControl) || this;
                        _this.TemplateTerms = [];
                        _this.TermListUpdateDummy = ko.observable();
                        _this.TermValidators = {}; // Validation functions for criteria tab concepts
                        _this.CurrentlySelectedModels = [];
                        var self = _this;
                        _this.options = options;
                        _this._selfRef = _this;
                        _this.ProjectID = options.ProjectID;
                        _this.TemplateNotes = options.TemplateNotes || '';
                        _this.IsTemplateEdit = options.IsTemplateEdit;
                        _this.IsCriteriaGroupEdit = options.TemplateType === Dns.Enums.TemplateTypes.CriteriaGroup;
                        _this.IsPresetQuery = options.TemplateComposerInterface === Dns.Enums.QueryComposerInterface.PresetQuery;
                        _this.SubscriptionsArray = ko.observableArray();
                        //remove any traces fo the querytype term
                        ko.utils.arrayForEach(options.Request.Where.Criteria || [], function (croot) {
                            ko.utils.arrayForEach(croot.Terms || [], function (troot) {
                                croot.Terms = ko.utils.arrayFilter(croot.Terms, function (tt) { return !MDQ.Terms.Compare(tt.Type, MDQ.Terms.DataCheckerQueryTypeID); });
                            });
                            ko.utils.arrayForEach(croot.Criteria || [], function (csub) {
                                csub.Terms = ko.utils.arrayFilter(csub.Terms, function (tt) { return !MDQ.Terms.Compare(tt.Type, MDQ.Terms.DataCheckerQueryTypeID); });
                            });
                        });
                        //check out options.Models
                        var termValueFilter = new Plugins.Requests.QueryBuilder.MDQ.TermValueFilter(ko.utils.arrayMap(options.Models, function (m) { return m.DataModelID; }));
                        _this.SexForCritieria = ko.observableArray(termValueFilter.SexValues());
                        _this.SettingForCritieria = ko.observableArray(termValueFilter.SettingsValues());
                        _this.RaceEthnicityForCritieria = ko.observableArray(termValueFilter.RaceEthnicityValues());
                        _this.AgeGroupForStratification = ko.observableArray(termValueFilter.AgeRangeStratifications());
                        _this.ShowAgeRangeCalculationSelections = ko.observable(termValueFilter.HasModel(MDQ.TermValueFilter.PCORnetModelID));
                        _this.TermList = options.VisualTerms;
                        _this.FilteredTermList = ko.observableArray([]);
                        //make sure any new template properties exist on the request query json terms
                        termValueFilter.ConfirmTemplateProperties(options.Request, options.VisualTerms);
                        _this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(options.Request);
                        _this.AvailableOrganizations = ko.observableArray([]);
                        if (!_this.IsTemplateEdit) {
                            //make sure all the criteria have an ID set
                            var confirmCritieriaID = function (criteria) {
                                if (criteria.ID() == null)
                                    criteria.ID(Constants.Guid.newGuid());
                                if (criteria.Criteria() != null) {
                                    ko.utils.arrayForEach(criteria.Criteria(), function (crit) {
                                        if (crit.ID() == null)
                                            crit.ID(Constants.Guid.newGuid());
                                        confirmCritieriaID(crit);
                                    });
                                }
                            };
                            ko.utils.arrayForEach(_this.Request.Where.Criteria(), function (ccrit) {
                                confirmCritieriaID(ccrit);
                            });
                        }
                        _this.CriteriaGroupTemplates = options.CriteriaGroupTemplates;
                        self.RefreshFilteredTermList = function () {
                            var list = [];
                            self.TermList.forEach(function (term) {
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
                                    var hasSummaryModel = options.IsTemplateEdit ? RequestType.Details.vm.SelectedModels.indexOf('cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb') >= 0 : options.Models.length == 1 && termValueFilter.HasModel(MDQ.TermValueFilter.SummaryTablesModelID);
                                    var termCategory = { Name: term.Name, Description: term.Description, TermID: term.TermID, Terms: [], ValueTemplate: term.ValueTemplate, IncludeInCriteria: term.IncludeInCriteria, IncludeInStratifiers: term.IncludeInStratifiers, IncludeInProjectors: term.IncludeInProjectors };
                                    term.Terms.forEach(function (childTerm) {
                                        if (childTerm.TermID != null) {
                                            //if the specified models is only summary tables, age range should not be available as a criteria, only stratifier
                                            if (MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.AgeRangeID))
                                                childTerm.IncludeInCriteria = options.IsTemplateEdit ? !(hasSummaryModel && RequestType.Details.vm.SelectedModels().length == 1) : !hasSummaryModel;
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
                                                MDQ.Terms.Compare(childTerm.TermID, MDQ.Terms.ICD9Procedure4digitID)) {
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
                            Dns.WebApi.Terms.ListTemplateTerms(options.TemplateID).done(function (templateTerms) {
                                self.TemplateTerms = templateTerms;
                                _this.TermListUpdateDummy.notifySubscribers();
                            });
                        }
                        _this.CriteriaTermList = ko.computed(function () {
                            _this.TermListUpdateDummy();
                            var resultArr = ko.utils.arrayFilter(self.FilteredTermList(), function (t) {
                                return t.IncludeInCriteria && ((t.Terms != null && t.Terms.length > 0 && t.TermID == null && ko.utils.arrayFirst(t.Terms, function (tt) { return tt.IncludeInCriteria; }) != null) || t.TermID != null);
                            }).map(function (t) {
                                var tvm = null;
                                var childTermVM = [];
                                var templateTerm;
                                var outerAllowed = false;
                                if (t.Terms != null && t.Terms.length > 0) {
                                    //it's a group of terms, like Criteria or Demographic
                                    t.Terms.forEach(function (it) {
                                        templateTerm = ko.utils.arrayFirst(self.TemplateTerms, function (tt) { return tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == it.TermID; });
                                        childTermVM.push(new TermVm(it, [], templateTerm == null ? true : templateTerm.Allowed));
                                        if (templateTerm != null) {
                                            if (templateTerm.Allowed) {
                                                outerAllowed = true;
                                            }
                                        }
                                        else {
                                            outerAllowed = true;
                                        }
                                    });
                                }
                                templateTerm = ko.utils.arrayFirst(self.TemplateTerms, function (tt) { return tt.Section == Dns.Enums.QueryComposerSections.Criteria && tt.TermID == t.TermID; });
                                if (t.TermID != null) {
                                    outerAllowed = templateTerm != null ? templateTerm.Allowed : true;
                                }
                                tvm = new TermVm(t, childTermVM, outerAllowed);
                                return tvm;
                            });
                            $('#TermSelector').kendoMenu({ orientation: 'vertical' });
                            return resultArr;
                        });
                        _this.StratifiersTermList = ko.computed(function () {
                            _this.TermListUpdateDummy();
                            var resultArr = ko.utils.arrayFilter(self.FilteredTermList(), function (t) {
                                return t.IncludeInStratifiers && ((t.Terms != null && t.Terms.length > 0 && t.TermID == null && ko.utils.arrayFirst(t.Terms, function (tt) { return tt.IncludeInStratifiers; }) != null) || t.TermID != null);
                            }).map(function (t) {
                                var tvm = null;
                                var childTermVM = [];
                                var templateTerm;
                                var outerAllowed = false;
                                if (t.Terms != null && t.Terms.length > 0) {
                                    //it's a group of terms, like Criteria or Demographic
                                    t.Terms.forEach(function (it) {
                                        templateTerm = ko.utils.arrayFirst(self.TemplateTerms, function (tt) { return tt.Section == Dns.Enums.QueryComposerSections.Stratification && tt.TermID == it.TermID; });
                                        childTermVM.push(new TermVm(it, [], templateTerm == null ? true : templateTerm.Allowed));
                                        if (templateTerm != null) {
                                            if (templateTerm.Allowed) {
                                                outerAllowed = true;
                                            }
                                        }
                                        else {
                                            outerAllowed = true;
                                        }
                                    });
                                }
                                templateTerm = ko.utils.arrayFirst(self.TemplateTerms, function (tt) { return tt.Section == Dns.Enums.QueryComposerSections.Stratification && tt.TermID == t.TermID; });
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
                        _this.NotAllowedTerms = ko.computed(function () {
                            //The first two arrays filter out the allowed terms (default). Which section the term came from is preserved by keeping the two arrays seperate.
                            var criteriaTerms = [];
                            ko.utils.arrayForEach(self.CriteriaTermList(), function (t) {
                                if (t.Terms != null && t.Terms.length > 0) {
                                    //it's a group of terms, like Criteria or Demographic
                                    t.Terms.forEach(function (it) {
                                        if (!it.Allowed()) {
                                            criteriaTerms.push(it);
                                        }
                                    });
                                }
                                else {
                                    if (!t.Allowed())
                                        criteriaTerms.push(t);
                                }
                            });
                            var stratTerms = [];
                            ko.utils.arrayForEach(self.StratifiersTermList(), function (t) {
                                if (t.Terms != null && t.Terms.length > 0) {
                                    //it's a group of terms, like Criteria or Demographic
                                    t.Terms.forEach(function (it) {
                                        if (!it.Allowed()) {
                                            stratTerms.push(it);
                                        }
                                    });
                                }
                                else {
                                    if (!t.Allowed())
                                        stratTerms.push(t);
                                }
                            });
                            //the last two arrays hold the terms as ISectionSpecificDTOs, a form that can be saved. 
                            var notAllowedCritTerms = criteriaTerms.map(function (t) {
                                var sectionTerm = {
                                    Section: Dns.Enums.QueryComposerSections.Criteria,
                                    TermID: t.TermID
                                };
                                return sectionTerm;
                            });
                            var notAllowedStratTerms = stratTerms.map(function (t) {
                                var sectionTerm = {
                                    Section: Dns.Enums.QueryComposerSections.Stratification,
                                    TermID: t.TermID
                                };
                                return sectionTerm;
                            });
                            //Since the section the terms came from is now preserved by a property in the DTO, the arrays can be concated
                            return notAllowedCritTerms.concat(notAllowedStratTerms);
                        }, _this, { deferEvaluation: true });
                        _this.UpdateTermList = function (modelID, adapterDetail, restrictToTermsID) {
                            var values = [];
                            if (modelID != null) {
                                ko.utils.arrayForEach(ko.utils.arrayMap(modelID, function (id) { return 'modelID=' + id; }), function (i) { return values.push(i); });
                            }
                            if (adapterDetail != null && adapterDetail != '') {
                                values.push('adapterDetail=' + adapterDetail);
                            }
                            if (restrictToTermsID != null) {
                                ko.utils.arrayForEach(ko.utils.arrayMap(restrictToTermsID, function (id) { return 'termID=' + id; }), function (i) { return values.push(i); });
                            }
                            //ko.utils.arrayForEach(ko.utils.arrayMap(restrictToTermsID, (id) => 'termID=' + id), (i) => values.push(i));
                            var termValueFilter = new Plugins.Requests.QueryBuilder.MDQ.TermValueFilter(modelID);
                            self.SexForCritieria(termValueFilter.SexValues());
                            self.SettingForCritieria = ko.observableArray(termValueFilter.SettingsValues());
                            self.RaceEthnicityForCritieria = ko.observableArray(termValueFilter.RaceEthnicityValues());
                            self.AgeGroupForStratification = ko.observableArray(termValueFilter.AgeRangeStratifications());
                            self.ShowAgeRangeCalculationSelections = ko.observable(termValueFilter.HasModel(MDQ.TermValueFilter.PCORnetModelID));
                            var query = values.join('&');
                            Dns.WebApi.Helpers.GetAPIResult('RequestTypes/GetTermsFilteredBy?' + query, true).done(function (terms) {
                                //When the observable collection changes it voids the kendoMenu, need to re-initialize the menu after changes.
                                var criteriaMenu = $('#TermSelector').data('kendoMenu');
                                criteriaMenu.destroy();
                                var selectMenu = $('#FieldsSelector').data('kendoMenu');
                                selectMenu.destroy();
                                Plugins.Requests.QueryBuilder.MDQ.RequestTypeTermIDs = terms.map(function (t) { return t.TermID; }) || [];
                                self.RefreshFilteredTermList();
                                $('#TermSelector').kendoMenu({ orientation: 'vertical' });
                                $('#FieldsSelector').kendoMenu({ orientation: 'vertical' });
                            });
                        };
                        MDQ.GetDataMartTimer = options.IsTemplateEdit ? null : 0;
                        _this.AgeRangeCalculationSelections = function (criteria, editViewModel) {
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
                        _this.NonAggregateFields = ko.computed(function () {
                            //hide the aggregate fields from view since they are not editable anyhow
                            var filtered = ko.utils.arrayFilter(self.Request.Select.Fields(), function (item) { return item.Aggregate() == null; });
                            return filtered;
                        });
                        try {
                            _this.CanSaveCriteriaGroup(Templates.Details.vm === undefined || Templates.Details.vm == null);
                        }
                        catch (e) {
                            _this.CanSaveCriteriaGroup = ko.observable(true);
                        }
                        if (options.Request == null) {
                            var criteriaVM = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                            if (!options.IsTemplateEdit)
                                criteriaVM.ID(Constants.Guid.newGuid());
                            _this.Request.Where.Criteria.push(criteriaVM);
                            if (_this.IsTemplateEdit == false) {
                                self.SubscriptionsArray.push({
                                    CriteriaGroup: criteriaVM,
                                    Subscription: criteriaVM.Terms.subscribe(function () {
                                        MDQ.GetDataMartTimer = setInterval((function () { return self.GetCompatibleDataMarts(); }).bind(self), 2000);
                                    })
                                });
                            }
                        }
                        else {
                            var codeTerms = [
                                MDQ.Terms.DrugClassID,
                                MDQ.Terms.DrugNameID,
                                MDQ.Terms.HCPCSProcedureCodesID,
                                MDQ.Terms.ICD9Diagnosis3digitID,
                                MDQ.Terms.ICD9Diagnosis4digitID,
                                MDQ.Terms.ICD9Diagnosis5digitID,
                                MDQ.Terms.ICD9Procedure3digitID,
                                MDQ.Terms.ICD9Procedure4digitID,
                                MDQ.Terms.ZipCodeID,
                                MDQ.Terms.CombinedDiagnosisCodesID,
                                MDQ.Terms.ESPCombinedDiagnosisCodesID,
                                MDQ.Terms.ProcedureCodesID // Procedure Codes
                            ];
                            var convertTerms = function (terms) {
                                terms.forEach(function (term) {
                                    var termValues = Global.Helpers.ConvertTermObject(term.Values());
                                    term.Values(termValues);
                                    if (codeTerms.indexOf(term.Type().toUpperCase()) >= 0) {
                                        if (term.Values != null && term.Values().CodeValues != null) {
                                            //Do not re-map as the CodeValues property already exists...
                                        }
                                        else {
                                            _this.TermList.forEach(function (item) {
                                                if (item.Terms == null || item.Terms.length == 0) {
                                                    if (term.Type() == item.TermID) {
                                                        var termValuesUpdated = Global.Helpers.CopyObject(item.ValueTemplate);
                                                        term.Values(termValuesUpdated);
                                                    }
                                                }
                                                if (item.Terms != null && item.Terms.length > 0) {
                                                    item.Terms.forEach(function (childTerm) {
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
                            _this.Request.Where.Criteria().forEach(function (cvm) {
                                var selfVM = _this;
                                convertTerms(cvm.Terms());
                                cvm.Criteria().forEach(function (subCriteria) {
                                    convertTerms(subCriteria.Terms());
                                });
                            });
                            //Add subscriptions here to listen for criteria changes
                            if (_this.IsTemplateEdit == false) {
                                _this.Request.Where.Criteria().forEach(function (criteriaVM) {
                                    //subscribe for the main terms
                                    self.SubscriptionsArray.push({
                                        CriteriaGroup: criteriaVM,
                                        Subscription: criteriaVM.Terms.subscribe(function () {
                                            MDQ.GetDataMartTimer = setInterval((function () { return self.GetCompatibleDataMarts(); }).bind(self), 2000);
                                        })
                                    });
                                    //subscribe for the sub-criteria colletion change (ie, adding removing a sub-criteria)
                                    self.SubscriptionsArray.push({
                                        CriteriaGroup: criteriaVM,
                                        Subscription: criteriaVM.Criteria.subscribe(function () {
                                            MDQ.GetDataMartTimer = setInterval((function () { return self.GetCompatibleDataMarts(); }).bind(self), 2000);
                                        })
                                    });
                                    //subscribe to the terms for each subcriteria
                                    ko.utils.arrayForEach(criteriaVM.Criteria(), function (subcrit) {
                                        self.SubscriptionsArray.push({
                                            CriteriaGroup: subcrit,
                                            Subscription: subcrit.Terms.subscribe(function () {
                                                MDQ.GetDataMartTimer = setInterval((function () { return self.GetCompatibleDataMarts(); }).bind(self), 2000);
                                            })
                                        });
                                    });
                                });
                                var strQuery = "";
                                if (_this.Request != null) {
                                    strQuery = JSON.stringify(_this.Request.toData());
                                }
                                //Call the GetCompatibleDataMarts method.
                                Plugins.Requests.QueryBuilder.DataMartRouting.vm.LoadDataMarts(options.ProjectID, strQuery);
                            }
                        }
                        _this.GetCompatibleDataMarts = function () {
                            clearInterval(MDQ.GetDataMartTimer);
                            //Do not attempt to get datamarts in the template editor
                            if (self.IsTemplateEdit)
                                return;
                            var strQuery = "";
                            if (self.Request != null) {
                                strQuery = JSON.stringify(self.Request.toData());
                            }
                            //Call the GetCompatibleDataMarts method.
                            Plugins.Requests.QueryBuilder.DataMartRouting.vm.LoadDataMarts(options.ProjectID, strQuery);
                        };
                        //Add subscription for the Stratification Fields...
                        if (!_this.IsTemplateEdit) {
                            _this.Request.Select.Fields.subscribe(function () {
                                MDQ.GetDataMartTimer = setInterval((function () { return self.GetCompatibleDataMarts(); }).bind(self), 2000);
                            });
                        }
                        self.onExportJSON = function () {
                            return 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(self.Request.toData()));
                        };
                        self.TermsColumnVisible = ko.computed(function () {
                            var isVis = false;
                            if (self.IsTemplateEdit)
                                isVis = true;
                            else {
                                ko.utils.arrayForEach(self.CriteriaTermList(), function (item) {
                                    if (item.Terms.length > 0) {
                                        ko.utils.arrayForEach(item.Terms, function (child) {
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
                        self.StratifiersColumnVisible = ko.computed(function () {
                            var isVis = false;
                            if (self.IsTemplateEdit)
                                isVis = true;
                            else {
                                ko.utils.arrayForEach(self.StratifiersTermList(), function (item) {
                                    if (item.Terms.length > 0) {
                                        ko.utils.arrayForEach(item.Terms, function (child) {
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
                        return _this;
                    }
                    ViewModel.prototype.AreTermsValid = function () {
                        var areTermsValid = true;
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
                    };
                    ViewModel.prototype.FilterTermsForCriteria = function (terms) {
                        return ko.utils.arrayFilter(terms, function (t) { return t.IncludeInCriteria; });
                    };
                    ViewModel.prototype.FilterTermsForStratification = function (terms) {
                        return ko.utils.arrayFilter(terms, function (t) { return t.IncludeInStratifiers; });
                    };
                    ViewModel.prototype.btnAddCriteriaGroup_Click = function (data, event) {
                        var criteriaVM = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                        if (!this.IsTemplateEdit)
                            criteriaVM.ID(Constants.Guid.newGuid());
                        this.Request.Where.Criteria.push(criteriaVM);
                        if (this.IsTemplateEdit == false) {
                            var self = this;
                            self.SubscriptionsArray.push({
                                CriteriaGroup: criteriaVM,
                                Subscription: criteriaVM.Terms.subscribe(function () {
                                    var iself = self;
                                    MDQ.GetDataMartTimer = setInterval((function () { return iself.GetCompatibleDataMarts(); }).bind(iself), 5000);
                                })
                            });
                            self.SubscriptionsArray.push({
                                CriteriaGroup: criteriaVM,
                                Subscription: criteriaVM.Criteria.subscribe(function () {
                                    var iself = self;
                                    MDQ.GetDataMartTimer = setInterval((function () { return iself.GetCompatibleDataMarts(); }).bind(iself), 5000);
                                })
                            });
                        }
                    };
                    ViewModel.prototype.btnDeleteCriteriaGroup_Click = function (data) {
                        var self = this;
                        if (self.SubscriptionsArray != null) {
                            var itemArr = self.SubscriptionsArray.remove(function (item) { return item.CriteriaGroup == data; });
                            if (itemArr != null) {
                                itemArr.forEach(function (item) {
                                    item.Subscription.dispose();
                                });
                            }
                        }
                        self.Request.Where.Criteria.remove(data);
                        itemArr = null;
                        this.GetCompatibleDataMarts();
                    };
                    ViewModel.prototype.btnSaveCriteriaGroup_Click = function (criteriaGroup) {
                        var parameters = {
                            CriteriaGroup: criteriaGroup.toData(),
                            AdapterDetail: this.Request.Header.QueryType(),
                            TemplateID: this.options.TemplateID,
                            RequestTypeID: this.options.RequestTypeID,
                            RequestID: this.options.RequestID
                        };
                        Global.Helpers.ShowDialog("Save Criteria Group", "/querycomposer/savecriteriagroup", ["close"], 650, 400, parameters).done(function () {
                        });
                    };
                    ViewModel.prototype.AddCriteriaGroupTemplate = function (data, event) {
                        var _this = this;
                        Dns.WebApi.Templates.Get(data.ID).done(function (template) {
                            var criteria = new Dns.ViewModels.QueryComposerCriteriaViewModel(JSON.parse(template[0].Data));
                            var codeTerms = [
                                MDQ.Terms.DrugClassID,
                                MDQ.Terms.DrugNameID,
                                MDQ.Terms.HCPCSProcedureCodesID,
                                MDQ.Terms.ICD9Diagnosis3digitID,
                                MDQ.Terms.ICD9Diagnosis4digitID,
                                MDQ.Terms.ICD9Diagnosis5digitID,
                                MDQ.Terms.ICD9Procedure3digitID,
                                MDQ.Terms.ICD9Procedure4digitID,
                                MDQ.Terms.ZipCodeID,
                                MDQ.Terms.CombinedDiagnosisCodesID,
                                MDQ.Terms.ESPCombinedDiagnosisCodesID,
                                MDQ.Terms.ProcedureCodesID // Procedure Codes
                            ];
                            var convertTerms = function (terms) {
                                terms.forEach(function (term) {
                                    var termValues = Global.Helpers.ConvertTermObject(term.Values());
                                    term.Values(termValues);
                                    if (codeTerms.indexOf(term.Type().toUpperCase()) >= 0) {
                                        if (term.Values != null && term.Values().CodeValues != null) {
                                            //Do not re-map as the CodeValues property already exists...
                                        }
                                        else {
                                            _this.TermList.forEach(function (item) {
                                                if (item.Terms == null || item.Terms.length == 0) {
                                                    if (term.Type() == item.TermID) {
                                                        var termValuesUpdated = Global.Helpers.CopyObject(item.ValueTemplate);
                                                        term.Values(termValuesUpdated);
                                                    }
                                                }
                                                if (item.Terms != null && item.Terms.length > 0) {
                                                    item.Terms.forEach(function (childTerm) {
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
                            criteria.Criteria().forEach(function (subCriteria) {
                                convertTerms(subCriteria.Terms());
                            });
                            if (!_this.IsTemplateEdit)
                                criteria.ID(Constants.Guid.newGuid());
                            if (MDQ.vm.Request.Where.Criteria()[0].Criteria().length == 0 && MDQ.vm.Request.Where.Criteria().length == 1)
                                MDQ.vm.Request.Where.Criteria().pop();
                            MDQ.vm.Request.Where.Criteria.push(criteria);
                        });
                    };
                    ViewModel.prototype.AddTerm = function (root, data, parent, event) {
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
                            var criteria = ko.utils.arrayFirst(parent.Criteria(), function (c) {
                                var t = ko.utils.arrayFirst(c.Terms(), function (tt) { return true; });
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
                                    Subscription: criteria.Terms.subscribe(function () {
                                        MDQ.GetDataMartTimer = setInterval((function () { return self.GetCompatibleDataMarts(); }).bind(self), 2000);
                                    })
                                });
                            }
                            else {
                                criteria.Terms.push(termViewModel);
                            }
                        }
                        else {
                            parent.Terms.push(termViewModel);
                        }
                        if (data.IncludeInStratifiers == false)
                            return;
                        SuspendDataMartTimer();
                        var fieldsToAdd = [];
                        var selectFields = self.Request.Select.Fields();
                        //specify the field as group by and included in the select
                        var foundStratifyByField = ko.utils.arrayFirst(selectFields, function (item) { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() == null; });
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
                        var foundCountField = ko.utils.arrayFirst(selectFields, function (item) { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() != null; });
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
                    };
                    ViewModel.prototype.AddField = function (data, parent, event) {
                        SuspendDataMartTimer();
                        var fieldsToAdd = [];
                        var selectFields = parent.Fields();
                        var foundStratifyByField = ko.utils.arrayFirst(selectFields, function (item) { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() == null; });
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
                        if (!ko.utils.arrayFirst(selectFields, function (item) { return item.Type().toUpperCase() == data.TermID.toUpperCase() && item.Aggregate() != null; })) {
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
                    };
                    ViewModel.prototype.TemplateSelector = function (data) {
                        return "e_" + data.Type();
                    };
                    ViewModel.prototype.StratifierTemplateSelector = function (data) {
                        return "s_" + data.Type();
                    };
                    ViewModel.prototype.OpenCodeSelector = function (data, list, showCategoryDropdown) {
                        Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                            ListID: list,
                            Codes: data.Values == null || data.Values().CodeValues == null ? "" : data.Values().CodeValues(),
                            ShowCategoryDropdown: showCategoryDropdown == null ? true : showCategoryDropdown
                        }).done(function (results) {
                            if (!results)
                                return; //User clicked cancel
                            data.Values.valueWillMutate();
                            var codes = ko.utils.arrayMap(results, function (item) { return ({ Code: item.Code.trim(), Name: item.Name.trim(), ExpireDate: null }); });
                            if (data.Values().CodeValues == null)
                                data.Values().CodeValues = [];
                            data.Values().CodeValues.removeAll();
                            codes.forEach(function (code) {
                                data.Values().CodeValues.push(code);
                            });
                            data.Values.valueHasMutated();
                        });
                    };
                    ViewModel.prototype.ConfirmCombinedCodeTypeChanged = function (data, e) {
                        if (data.Values == null || data.Values().CodeValues == null || data.Values().CodeValues() == null || data.Values().CodeValues().length == 0)
                            return;
                        var oldValue = e.sender.value();
                        Global.Helpers.ShowConfirm("Code Change Confirmation", "Changing the Code Set will reset the selected values, would you like to continue?").fail(function () {
                            e.sender.value(oldValue);
                            data.Values().CodeType(oldValue);
                            return;
                        }).done(function () {
                            data.Values().CodeValues('');
                            return;
                        });
                    };
                    ViewModel.prototype.ConfirmProcedureCodeChanged = function (data, e) {
                        if (data.Values == null || data.Values().CodeValues == null || data.Values().CodeValues() == null || data.Values().CodeValues().length == 0)
                            return;
                        var oldValue = e.sender.value();
                        Global.Helpers.ShowConfirm("Code Change Confirmation", "Changing the Code Set will reset the selected values, would you like to continue?").fail(function () {
                            e.sender.value(oldValue);
                            data.Values().CodeType(oldValue);
                            return;
                        }).done(function () {
                            data.Values().CodeValues('');
                            return;
                        });
                    };
                    ViewModel.prototype.OpenCombinedCodeSelector = function (data, codeType) {
                        //codeType will indicate the type of list to use in the selector
                        if (codeType != Dns.Enums.DiagnosisCodeTypes.ICD9) {
                            alert('Only ICD-9 diagnosis codes are supported by the code selector, please enter the codes manually into the text field separated by semi-colons.');
                            return;
                        }
                        //need to get the current values and split by semi-colon, and add to code selector values
                        var existingValues = null;
                        if (data.Values != null && data.Values().CodeValues != null) {
                            existingValues = ko.utils.arrayFilter(ko.utils.arrayMap((data.Values().CodeValues() || '').split(';'), function (c) { return (c || '').trim(); }), function (c) { return c.length > 0; });
                        }
                        Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                            ListID: Dns.Enums.Lists.SPANDiagnosis,
                            Codes: existingValues,
                            ShowCategoryDropdown: true
                        }).done(function (results) {
                            if (!results)
                                return; //User clicked cancel
                            //combine the selected codes into a single string
                            var codes = ko.utils.arrayMap(results, function (item) { return item.Code; }).join('; ');
                            data.Values.valueWillMutate();
                            if (data.Values().CodeValues == null) {
                                data.Values().CodeValues = ko.observable(codes);
                            }
                            else {
                                data.Values().CodeValues(codes);
                            }
                            data.Values.valueHasMutated();
                        });
                    };
                    ViewModel.prototype.ConfirmESPCombinedCodeTypeChanged = function (data, e) {
                        if (data.Values == null || data.Values().CodeValues == null || data.Values().CodeValues() == null || data.Values().CodeValues().length == 0)
                            return;
                        var oldValue = e.sender.value();
                        Global.Helpers.ShowConfirm("Code Change Confirmation", "Changing the Code Set will reset the selected values, would you like to continue?").fail(function () {
                            e.sender.value(oldValue);
                            data.Values().CodeType(oldValue);
                            return;
                        }).done(function () {
                            data.Values().CodeValues('');
                            return;
                        });
                    };
                    ViewModel.prototype.OpenESPCombinedCodeSelector = function (data, codeType) {
                        //codeType will indicate the type of list to use in the selector
                        if (codeType != Dns.Enums.ESPCodes.ICD9 && codeType != Dns.Enums.ESPCodes.ICD10) {
                            alert('Only ICD-9 diagnosis codes are supported by the code selector, please enter the codes manually into the text field separated by semi-colons.');
                            return;
                        }
                        //need to get the current values and split by semi-colon, and add to code selector values
                        var existingValues = null;
                        if (data.Values != null && data.Values().CodeValues != null) {
                            existingValues = ko.utils.arrayFilter(ko.utils.arrayMap((data.Values().CodeValues() || '').split(';'), function (c) { return (c || '').trim(); }), function (c) { return c.length > 0; });
                        }
                        if (codeType == Dns.Enums.ESPCodes.ICD9) {
                            Global.Helpers.ShowDialog("Code Selector", "/Dialogs/CodeSelector", ["Close"], 960, 620, {
                                ListID: Dns.Enums.ESPCodes.ICD9,
                                Codes: existingValues,
                                ShowCategoryDropdown: true
                            }).done(function (results) {
                                if (!results)
                                    return; //User clicked cancel
                                //combine the selected codes into a single string
                                var codes = ko.utils.arrayMap(results, function (item) { return item.Code; }).join('; ');
                                data.Values.valueWillMutate();
                                if (data.Values().CodeValues == null) {
                                    data.Values().CodeValues = ko.observable(codes);
                                }
                                else {
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
                            }).done(function (results) {
                                if (!results)
                                    return; //User clicked cancel
                                //combine the selected codes into a single string
                                var codes = ko.utils.arrayMap(results, function (item) { return item.Code; }).join('; ');
                                data.Values.valueWillMutate();
                                if (data.Values().CodeValues == null) {
                                    data.Values().CodeValues = ko.observable(codes);
                                }
                                else {
                                    data.Values().CodeValues(codes);
                                }
                                data.Values.valueHasMutated();
                            });
                        }
                    };
                    ViewModel.prototype.DeleteTerm = function (data, criteriaGroup) {
                        SuspendDataMartTimer();
                        /** only removes from the criteria **/
                        criteriaGroup.Terms.remove(data);
                        /** remove the criteria if it is a sub-criteria and is empty **/
                        ko.utils.arrayForEach(this.Request.Where.Criteria(), function (c) {
                            var subCriteriaToRemove = ko.utils.arrayFilter(c.Criteria(), function (sc) { return sc.Terms().length == 0; });
                            subCriteriaToRemove.forEach(function (sc) {
                                //clean up the sub-criteria's terms collection subscription since the sub-criteria is getting removed.                    
                                var subscriptionsToDispose = MDQ.vm.SubscriptionsArray.remove(function (cs) { return cs.CriteriaGroup == sc; });
                                if (subscriptionsToDispose != null) {
                                    subscriptionsToDispose.forEach(function (item) {
                                        item.Subscription.dispose();
                                    });
                                }
                                subscriptionsToDispose = null;
                                SuspendDataMartTimer();
                                c.Criteria.remove(sc);
                            });
                        });
                    };
                    ViewModel.prototype.DeleteField = function (data, selectFields) {
                        SuspendDataMartTimer();
                        /** removes the count as well **/
                        var fieldsToRemove = ko.utils.arrayFilter(selectFields.Fields(), function (field) {
                            return field.Type() == data.Type();
                        });
                        for (var i = 0; i < fieldsToRemove.length; i++) {
                            selectFields.Fields.remove(fieldsToRemove[i]);
                            if (i < fieldsToRemove.length - 1)
                                SuspendDataMartTimer();
                        }
                    };
                    ViewModel.prototype.ShowSubCriteriaConjuction = function (parentCriteria, subCriteria) {
                        if (parentCriteria.Criteria().length < 2)
                            return false;
                        if (parentCriteria.Criteria().indexOf(subCriteria) == 0) {
                            return false;
                        }
                        return true;
                    };
                    ViewModel.DataCheckerProcedureCodeTypes = new Array({ Name: 'Any', Value: '' }, { Name: 'ICD-9-CM', Value: '09' }, { Name: 'ICD-10-CM', Value: '10' }, { Name: 'ICD-11-CM', Value: '11' }, { Name: 'CPT Category II', Value: 'C2' }, { Name: 'CPT Category III', Value: 'C3' }, { Name: 'CPT-4 (i.e., HCPCS Level I)', Value: 'C4' }, { Name: 'HCPCS (i.e., HCPCS Level II)', Value: 'HC' }, { Name: 'HCPCS Level III', Value: 'H3' }, { Name: 'LOINC', Value: 'LC' }, { Name: 'Local Homegrown', Value: 'LO' }, { Name: 'NDC', Value: 'ND' }, { Name: 'Revenue', Value: 'RE' }, { Name: 'Other', Value: 'OT' });
                    ViewModel.DataCheckerDiagnosisCodeTypes = new Array({ Name: 'Any', Value: '' }, { Name: 'ICD-9-CM', Value: '09' }, { Name: 'ICD-10-CM', Value: '10' }, { Name: 'ICD-11-CM', Value: '11' }, { Name: 'SNOMED CT', Value: 'SM' }, { Name: 'Other', Value: 'OT' });
                    ViewModel.YearlyQuarters = ko.observableArray(['Q1', 'Q2', 'Q3', 'Q4']);
                    /**
                    Grouped terms are to be combined using OR within a sub-criteria that will be AND'd with the other terms of the parent criteria.
                    */
                    ViewModel.GroupedTerms = [
                        //Condition
                        MDQ.Terms.ConditionID,
                        //HCPCS Procedure Codes
                        MDQ.Terms.HCPCSProcedureCodesID,
                        //Combined Diagnosis Codes
                        MDQ.Terms.CombinedDiagnosisCodesID,
                        //ICD9 Diagnosis Codes 3 digit
                        MDQ.Terms.ICD9Diagnosis3digitID,
                        //ICD9 Diagnosis Codes 4 digit
                        MDQ.Terms.ICD9Diagnosis4digitID,
                        //ICD9 Diagnosis Codes 5 digit
                        MDQ.Terms.ICD9Diagnosis5digitID,
                        //ICD9 Procedure Codes 3 digit
                        MDQ.Terms.ICD9Procedure3digitID,
                        //ICD9 Procedure Codes 4 digit
                        MDQ.Terms.ICD9Procedure4digitID,
                        //ESP Combined Diagnosis Codes
                        MDQ.Terms.ESPCombinedDiagnosisCodesID,
                        //Drug Class
                        MDQ.Terms.DrugClassID,
                        //Drug Name
                        MDQ.Terms.DrugNameID,
                        //Visits
                        MDQ.Terms.VisitsID,
                        //Age Range
                        MDQ.Terms.AgeRangeID,
                        //Sex
                        MDQ.Terms.SexID,
                        //Code Metric
                        MDQ.Terms.CodeMetricID,
                        //Coverage
                        MDQ.Terms.CoverageID,
                        //Criteria
                        MDQ.Terms.CriteriaID,
                        //Dispensing Metric
                        MDQ.Terms.DispensingMetricID,
                        //Ethnicity
                        MDQ.Terms.EthnicityID,
                        //Facility
                        MDQ.Terms.FacilityID,
                        //Height
                        MDQ.Terms.HeightID,
                        //Hispanic
                        MDQ.Terms.HispanicID,
                        //Observation Period
                        MDQ.Terms.ObservationPeriodID,
                        //Quarter Year
                        MDQ.Terms.QuarterYearID,
                        //Race
                        MDQ.Terms.RaceID,
                        //Setting
                        MDQ.Terms.SettingID,
                        //Tobacco Use
                        MDQ.Terms.TobaccoUseID,
                        //Weight
                        MDQ.Terms.WeightID,
                        //Year
                        MDQ.Terms.YearID,
                        //Zip Code
                        MDQ.Terms.ZipCodeID,
                        //Vitals Measure Date
                        MDQ.Terms.VitalsMeasureDateID,
                        // Procedure Codes
                        MDQ.Terms.ProcedureCodesID
                    ];
                    /** Non-code terms that still need to use a sub-criteria to handle multiple term's OR'd together */
                    ViewModel.NonCodeGroupedTerms = [
                        //Visits
                        MDQ.Terms.VisitsID,
                        //Age Range
                        MDQ.Terms.AgeRangeID,
                        //Sex
                        MDQ.Terms.SexID,
                        //Code Metric
                        MDQ.Terms.CodeMetricID,
                        //Coverage
                        MDQ.Terms.CoverageID,
                        //Criteria
                        MDQ.Terms.CriteriaID,
                        //Dispensing Metric
                        MDQ.Terms.DispensingMetricID,
                        //Ethnicity
                        MDQ.Terms.EthnicityID,
                        //Facility
                        MDQ.Terms.FacilityID,
                        //Height
                        MDQ.Terms.HeightID,
                        //Hispanic
                        MDQ.Terms.HispanicID,
                        //Observation Period
                        MDQ.Terms.ObservationPeriodID,
                        //Quarter Year
                        MDQ.Terms.QuarterYearID,
                        //Race
                        MDQ.Terms.RaceID,
                        //Setting
                        MDQ.Terms.SettingID,
                        //Tobacco Use
                        MDQ.Terms.TobaccoUseID,
                        //Weight
                        MDQ.Terms.WeightID,
                        //Year
                        MDQ.Terms.YearID,
                        //Zip Code
                        MDQ.Terms.ZipCodeID,
                        //Vitals Measure Date
                        MDQ.Terms.VitalsMeasureDateID,
                    ];
                    return ViewModel;
                }(Global.PageViewModel));
                MDQ.ViewModel = ViewModel;
                function GetVisualTerms() {
                    var d = $.Deferred();
                    $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
                        .done(function (result) {
                        d.resolve(result);
                    }).fail(function (e, description, error) {
                        d.reject(e);
                    });
                    return d;
                }
                MDQ.GetVisualTerms = GetVisualTerms;
                function init(rawRequestData, fieldOptions, defaultPriority, defaultDueDate, additionalInstructions, existingRequestDataMarts, requestTypeID, visualTerms, isTemplateEdit, projectID, templateID) {
                    if (isTemplateEdit === void 0) { isTemplateEdit = false; }
                    if (projectID === void 0) { projectID = Global.GetQueryParam("projectID"); }
                    var requestID = null;
                    if (isTemplateEdit) {
                        templateID = templateID || Global.GetQueryParam("ID");
                    }
                    else {
                        var templateID = templateID || Global.GetQueryParam("templateID");
                        var requestID = Global.GetQueryParam("ID");
                    }
                    var promise = $.Deferred();
                    $.when(templateID == null ? null : Dns.WebApi.Templates.Get(templateID), requestTypeID == null ? ((templateID == null) ? null : Dns.WebApi.Helpers.GetAPIResult('RequestTypes/GetTermsFilteredBy?templateID=' + templateID)) : Dns.WebApi.RequestTypes.GetFilteredTerms(requestTypeID), requestTypeID == null ? null : Dns.WebApi.Templates.GetByRequestType(requestTypeID), Dns.WebApi.Templates.List("Type eq Lpp.Dns.DTO.Enums.TemplateTypes'" + Dns.Enums.TemplateTypes.CriteriaGroup + "'", "ID,Name", "Name"), visualTerms == null ? GetVisualTerms() : visualTerms, requestTypeID == null ? null : Dns.WebApi.RequestTypes.GetRequestTypeModels(requestTypeID)).done(function (queryTemplates, requestTypeTerms, requestTypeTemplates, criteriaGroupTemplates, visualTerms, models) {
                        //Load the Template Terms
                        if (requestTypeTerms) {
                            requestTypeTerms.forEach(function (term) {
                                Plugins.Requests.QueryBuilder.MDQ.RequestTypeTermIDs.push(term.TermID);
                            });
                        }
                        if (templateID == null) {
                            //Do nothing here. The request is being loaded, and RawRequestData has already been populated.
                        }
                        else {
                            var queryTemplate = queryTemplates == null ? {
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
                            var json = JSON.parse(((queryTemplate.Data || '').trim() != '') ? queryTemplate.Data : '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[] }]}}');
                            var jTemplate;
                            if (queryTemplate.Type == Dns.Enums.TemplateTypes.CriteriaGroup) {
                                jTemplate = {
                                    Header: { Name: null, Description: null, ViewUrl: null, Grammar: null, DueDate: null, Priority: null, QueryType: queryTemplate.QueryType, SubmittedOn: null },
                                    Where: { Criteria: [json] },
                                    Select: { Fields: [json] }
                                };
                            }
                            else {
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
                        var options = {
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
                        MDQ.vm = new ViewModel(options);
                        ko.applyBindings(MDQ.vm, bindingControl[0]);
                        promise.resolve(MDQ.vm);
                    });
                    return promise;
                }
                MDQ.init = init;
                ;
                function SuspendDataMartTimer() {
                    if (MDQ.GetDataMartTimer != null) {
                        clearInterval(MDQ.GetDataMartTimer);
                    }
                }
                MDQ.SuspendDataMartTimer = SuspendDataMartTimer;
                ko.bindingHandlers.AgeRangeCalculationTypeExtender = {
                    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        // This will be called when the binding is first applied to an element
                        var value = valueAccessor();
                        var isTemplateEdit = bindingContext.$root.IsTemplateEdit;
                        var isRequired = bindingContext.$data.Values().CalculateAsOfRequired;
                        value.subscribe(function (newValue) {
                            if (newValue != '7') {
                                var calculationDate = bindingContext.$data.Values().CalculateAsOf;
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
                    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        // This will be called once when the binding is first applied to an element,
                        // and again whenever any observables/computeds that are accessed change
                        // Update the DOM element based on the supplied values here.    
                    }
                };
                ko.bindingHandlers.DataPartnerTypeExtender = {
                    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        // This will be called when the binding is first applied to an element
                        var value = valueAccessor();
                        var root = bindingContext.$root;
                        var isTemplateEdit = bindingContext.$root.IsTemplateEdit;
                        if (isTemplateEdit == false) {
                            var projectID = root.ProjectID || Global.GetQueryParam("ProjectID");
                            if (projectID != null) {
                                Dns.WebApi.Requests.GetOrganizationsForRequest(projectID).done(function (results) {
                                    root.AvailableOrganizations(results);
                                });
                            }
                        }
                    }
                };
                //end of module
            })(MDQ = QueryBuilder.MDQ || (QueryBuilder.MDQ = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
var CriteriaGroupSubscription = /** @class */ (function () {
    function CriteriaGroupSubscription() {
    }
    return CriteriaGroupSubscription;
}());
var TermVm = /** @class */ (function () {
    function TermVm(term, childTerms, allowed) {
        this.Name = term.Name;
        this.TermID = term.TermID;
        this.Description = term.Description;
        this.Terms = childTerms;
        this.ValueTemplate = term.ValueTemplate;
        this.IncludeInCriteria = term.IncludeInCriteria;
        this.IncludeInStratifiers = term.IncludeInStratifiers;
        this.IncludeInProjectors = term.IncludeInProjectors;
        this.Allowed = ko.observable(allowed);
    }
    return TermVm;
}());
//# sourceMappingURL=MDQ.js.map