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
/// <reference path="../../../js/_rootlayout.ts" />
var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var View;
            (function (View) {
                var ViewModel = /** @class */ (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(query, visualTerms, bindingControl) {
                        var _this = _super.call(this, bindingControl) || this;
                        _this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(query);
                        if (query == null) {
                            var criteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                            criteria.ID(Constants.Guid.newGuid());
                            _this.Request.Where.Criteria.push(criteria);
                        }
                        var self = _this;
                        _this.NonAggregateFields = ko.computed(function () {
                            //hide the aggregate fields from view since they are not editable anyhow
                            var filtered = ko.utils.arrayFilter(self.Request.Select.Fields(), function (item) { return item.Aggregate() == null; });
                            return filtered;
                        });
                        //Load the Concept's TermValues as observables.
                        if (_this.Request == null || _this.Request.Where.Criteria().length == 0) {
                            //This is a new request, no previously defined criteria found.
                        }
                        else {
                            var termValueFilter = new Plugins.Requests.QueryBuilder.MDQ.TermValueFilter([]);
                            termValueFilter.ConfirmTemplateProperties(query, visualTerms);
                            var convertTerms = function (terms) {
                                terms.forEach(function (term) {
                                    var termValues = Global.Helpers.ConvertTermObject(term.Values());
                                    term.Values(termValues);
                                    if (ViewModel.CodeTerms.indexOf(term.Type().toUpperCase()) >= 0) {
                                        if (term.Values != null && term.Values().CodeValues != null) {
                                            //Do not re-map as the CodeValues property already exists...
                                        }
                                        else {
                                            visualTerms.forEach(function (item) {
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
                                var selfVM = self;
                                convertTerms(cvm.Terms());
                                cvm.Criteria().forEach(function (subCriteria) {
                                    convertTerms(subCriteria.Terms());
                                });
                            });
                        }
                        return _this;
                    }
                    ViewModel.prototype.TemplateSelector = function (data) {
                        return "v_" + data.Type().toLowerCase();
                    };
                    ViewModel.prototype.StratifierTemplateSelector = function (data) {
                        return "sv_" + data.Type().toLowerCase();
                    };
                    ViewModel.TranslateDataCheckerDiagnosisCodeType = function (value) {
                        var item = ko.utils.arrayFirst(ViewModel.DataCheckerDiagnosisCodeTypes, function (i) { return i.Value.toUpperCase() == (value || '').toUpperCase(); });
                        if (item == null)
                            return value;
                        return item.Name;
                    };
                    ViewModel.TranslateDataCheckerProcedureCodeType = function (value) {
                        var item = ko.utils.arrayFirst(ViewModel.DataCheckerProcedureCodeTypes, function (i) { return i.Value.toUpperCase() == (value || '').toUpperCase(); });
                        if (item == null)
                            return value;
                        return item.Name;
                    };
                    ViewModel.prototype.ShowSubCriteriaConjuction = function (parentCriteria, subCriteria) {
                        if (parentCriteria.Criteria().length < 2)
                            return false;
                        if (parentCriteria.Criteria().indexOf(subCriteria) == 0) {
                            return false;
                        }
                        return true;
                    };
                    ViewModel.CodeTerms = [
                        //drug class
                        "75290001-0E78-490C-9635-A3CA01550704",
                        //drug name
                        "0E1F0001-CA0C-42D2-A9CC-A3CA01550E84",
                        //HCPCS Procedure Codes
                        "096A0001-73B4-405D-B45F-A3CA014C6E7D",
                        //ICD9 Diagnosis Codes 3 digit
                        "5E5020DC-C0E4-487F-ADF2-45431C2B7695",
                        //ICD9 Diagnosis Codes 4 digit
                        "D0800001-2810-48ED-96B9-A3D40146BAAE",
                        //ICD9 Diagnosis Codes 5 digit
                        "80750001-6C3B-4C2D-90EC-A3D40146C26D",
                        //ICD9 Procedure Codes 3 digit
                        "E1CC0001-1D9A-442A-94C4-A3CA014C7B94",
                        //ICD9 Procedure Codes 4 digit
                        "9E870001-1D48-4AA3-8889-A3D40146CCB3",
                        //Zip Code
                        "8B5FAA77-4A4B-4AC7-B817-69F1297E24C5",
                        //Combinded Diagnosis Codes
                        "86110001-4BAB-4183-B0EA-A4BC0125A6A7"
                    ];
                    ViewModel.DataCheckerDiagnosisCodeTypes = new Array({ Name: 'Any', Value: '' }, { Name: 'ICD-9-CM', Value: '09' }, { Name: 'ICD-10-CM', Value: '10' }, { Name: 'ICD-11-CM', Value: '11' }, { Name: 'SNOMED CT', Value: 'SM' }, { Name: 'Other', Value: 'OT' });
                    ViewModel.DataCheckerProcedureCodeTypes = new Array({ Name: 'Any', Value: '' }, { Name: 'ICD-9-CM', Value: '09' }, { Name: 'ICD-10-CM', Value: '10' }, { Name: 'ICD-11-CM', Value: '11' }, { Name: 'CPT Category II', Value: 'C2' }, { Name: 'CPT Category III', Value: 'C3' }, { Name: 'CPT-4 (i.e., HCPCS Level I)', Value: 'C4' }, { Name: 'HCPCS (i.e., HCPCS Level II)', Value: 'HC' }, { Name: 'HCPCS Level III', Value: 'H3' }, { Name: 'LOINC', Value: 'LC' }, { Name: 'Local Homegrown', Value: 'LO' }, { Name: 'NDC', Value: 'ND' }, { Name: 'Revenue', Value: 'RE' }, { Name: 'Other', Value: 'OT' });
                    return ViewModel;
                }(Global.PageViewModel));
                View.ViewModel = ViewModel;
                function PrepareCollectionForDisplay(value, delimiter) {
                    if (!value)
                        return '';
                    if (!delimiter)
                        return value;
                    return value.split(delimiter).map(function (s) { return (s || '').trim(); }).join(delimiter.trim() + ' ');
                }
                View.PrepareCollectionForDisplay = PrepareCollectionForDisplay;
                function init(query, visualTerms, bindingControl) {
                    var vm = new ViewModel(query, visualTerms, bindingControl);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                    return vm;
                }
                View.init = init;
                ko.bindingHandlers.DocumentsByRevision = {
                    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        //element is the html element the binding is on
                        //valueAccessor is[{RevisionSetID:''}]
                        var val = ko.utils.unwrapObservable(valueAccessor());
                        var revisions = ko.utils.arrayMap(val, function (d) { return d.RevisionSetID; });
                        Dns.WebApi.Documents.ByRevisionID(revisions)
                            .done(function (results) {
                            if (results && results.length > 0) {
                                results.forEach(function (d) { return $('<tr><td>' + d.Name + '</td><td>' + Global.Helpers.formatFileSize(d.Length) + '</td></tr>').appendTo(element); });
                            }
                            else {
                                $('<tr style="background-color:#eee;"><td style="text-align:center;" colspan="2">No Documents Uploaded</td></tr>').appendTo(element);
                            }
                        });
                    }
                };
            })(View = QueryBuilder.View || (QueryBuilder.View = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
//# sourceMappingURL=View.js.map