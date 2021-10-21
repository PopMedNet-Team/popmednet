var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
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
                    function ViewModel(request, bindingControl) {
                        var _this = _super.call(this, bindingControl) || this;
                        //TODO: look into how to easily confirm the properties on an object without going back and forth between interface and viewmodel
                        _this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(request).toData();
                        return _this;
                    }
                    ViewModel.prototype.FilterForNonAggregateFields = function (query) {
                        return ko.utils.arrayFilter(query.Select.Fields, function (f) { return f.Aggregate == null; });
                    };
                    ViewModel.prototype.ShowSubCriteriaConjuction = function (parentCriteria, subCriteria) {
                        if (parentCriteria.Criteria.length < 2)
                            return false;
                        if (parentCriteria.Criteria.indexOf(subCriteria) == 0) {
                            return false;
                        }
                        return true;
                    };
                    ViewModel.prototype.TemplateSelector = function (data) {
                        return "v_" + data.Type.toLowerCase();
                    };
                    ViewModel.prototype.StratifierTemplateSelector = function (data) {
                        return "sv_" + data.Type.toLowerCase();
                    };
                    ViewModel.TranslateDataCheckerDiagnosisCodeType = function (value) {
                        var item = ko.utils.arrayFirst(ViewModel.DataCheckerDiagnosisCodeTypes, function (i) { return i.Value.toUpperCase() == (value || '').toUpperCase(); });
                        if (item == null)
                            return value;
                        return item.Name;
                    };
                    /**
                     * Loops through all Element nodes of the template after render and update's the declare identifying attributes with a prefix unique to the instantiated template.
                     * @param nodes The collection of DOMnodes of the rendered template.
                     */
                    ViewModel.prototype.onUpdateTemplateElements = function (nodes) {
                        if (nodes == null || nodes.length == 0)
                            return;
                        var prefix = Constants.Guid.newGuid() + '_';
                        var updateAttributeValue = function (elmt, key) {
                            if (elmt.hasAttribute(key)) {
                                var v = elmt.getAttribute(key);
                                if (v && v.trim().length > 0) {
                                    elmt.setAttribute(key, prefix + v);
                                }
                            }
                        };
                        //recursive function to prepend the prefix to all defined id using attributes
                        var updateElementID = function (element) {
                            var attr = element.id;
                            if (attr && attr.trim().length > 0) {
                                element.id = prefix + attr;
                            }
                            updateAttributeValue(element, "for");
                            updateAttributeValue(element, "name");
                            updateAttributeValue(element, "aria-labelledby");
                            updateAttributeValue(element, "data-for");
                            if (element.children.length > 0) {
                                for (var i = 0; i < element.children.length; i++) {
                                    updateElementID(element.children.item(i));
                                }
                            }
                        };
                        nodes.forEach(function (node) {
                            if (node.nodeType != 1)
                                return;
                            var element = node;
                            updateElementID(element);
                        });
                    };
                    ViewModel.TranslateDataCheckerProcedureCodeType = function (value) {
                        var item = ko.utils.arrayFirst(ViewModel.DataCheckerProcedureCodeTypes, function (i) { return i.Value.toUpperCase() == (value || '').toUpperCase(); });
                        if (item == null)
                            return value;
                        return item.Name;
                    };
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
                function initialize(query, requestVM, bindingControl) {
                    var queryRequestDTO;
                    if (query.hasOwnProperty('SchemaVersion') == false) {
                        //Only a multi-query request will have a SchemaVersion property.
                        //Going to assume request type hasn't been converted to the new multi-query schema.
                        //Automactially upgrade, assume the current json only has a single query and it matches the first specifiec for the request type.
                        //The 'query' parameter is original non-multi query json, need to wrap in new request json.
                        queryRequestDTO = new Dns.ViewModels.QueryComposerRequestViewModel().toData();
                        queryRequestDTO.Header.ID = requestVM.ID();
                        queryRequestDTO.Header.Name = requestVM.Name();
                        queryRequestDTO.Header.Description = requestVM.Description();
                        queryRequestDTO.Header.DueDate = requestVM.DueDate();
                        queryRequestDTO.Header.Priority = requestVM.Priority();
                        queryRequestDTO.Header.SubmittedOn = requestVM.SubmittedOn();
                        queryRequestDTO.Queries = [query];
                    }
                    else {
                        queryRequestDTO = query;
                    }
                    var vm = new ViewModel(queryRequestDTO, bindingControl);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                    return vm;
                }
                View.initialize = initialize;
                function init(query, visualTerms, bindingControl) {
                    //deprecated for initialize()
                    throw new DOMException("Deprecated for initialize().");
                    return null;
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
