/// <reference path="../../../js/_rootlayout.ts" />
module Plugins.Requests.QueryBuilder.View {

    export class ViewModel extends Global.PageViewModel {
        public Request: Dns.ViewModels.QueryComposerRequestViewModel;

        public NonAggregateFields: KnockoutComputed<Dns.ViewModels.QueryComposerFieldViewModel[]>;

        private static CodeTerms: any[] = [
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

        constructor(query: Dns.Interfaces.IQueryComposerRequestDTO, visualTerms: IVisualTerm[], bindingControl: JQuery) {
            super(bindingControl);
            
            this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(query);
            if (query == null) {
                var criteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
                criteria.ID(Constants.Guid.newGuid());
                this.Request.Where.Criteria.push(criteria);
            }

            var self = this;
            this.NonAggregateFields = ko.computed(() => {
                //hide the aggregate fields from view since they are not editable anyhow
                var filtered = ko.utils.arrayFilter(self.Request.Select.Fields(),(item: Dns.ViewModels.QueryComposerFieldViewModel) => { return item.Aggregate() == null; });
                return filtered;
            });

            //Load the Concept's TermValues as observables.
            if (this.Request == null || this.Request.Where.Criteria().length == 0) {
                //This is a new request, no previously defined criteria found.
            }
            else {
                var termValueFilter = new Plugins.Requests.QueryBuilder.MDQ.TermValueFilter([]);
                termValueFilter.ConfirmTemplateProperties(query, visualTerms);

                var convertTerms = (terms: Dns.ViewModels.QueryComposerTermViewModel[]) => {
                    terms.forEach((term) => {
                        var termValues = Global.Helpers.ConvertTermObject(term.Values());
                        term.Values(termValues);

                        if (ViewModel.CodeTerms.indexOf(term.Type().toUpperCase()) >= 0) {

                            if (term.Values != null && term.Values().CodeValues != null) {
                                //Do not re-map as the CodeValues property already exists...
                            }
                            else {
                                visualTerms.forEach((item) => {
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
                    var selfVM = self;

                    convertTerms(cvm.Terms());

                    cvm.Criteria().forEach((subCriteria) => {
                        convertTerms(subCriteria.Terms());
                    });
                });

                this.Request.TemporalEvents().forEach((temporalEvent) => {
                    temporalEvent.Criteria().forEach((cvm) => {
                        convertTerms(cvm.Terms());
                        cvm.Criteria().forEach((subCriteria) => {
                            convertTerms(subCriteria.Terms());
                        });
                    });

                });
            }
        }

        public TemplateSelector(data: Dns.ViewModels.QueryComposerTermViewModel) {
            return "v_" + data.Type().toLowerCase();
        }

        public StratifierTemplateSelector(data: Dns.ViewModels.QueryComposerFieldViewModel) {
            return "sv_" + data.Type().toLowerCase();
        }

        public static DataCheckerDiagnosisCodeTypes = new Array(
            { Name: 'Any', Value: '' },
            { Name: 'ICD-9-CM', Value: '09' },
            { Name: 'ICD-10-CM', Value: '10' },
            { Name: 'ICD-11-CM', Value: '11' },
            { Name: 'SNOMED CT', Value: 'SM' },
            { Name: 'Other', Value: 'OT' });

        public static TranslateDataCheckerDiagnosisCodeType(value: string): string {
            var item = ko.utils.arrayFirst(ViewModel.DataCheckerDiagnosisCodeTypes,(i) => i.Value.toUpperCase() == (value || '').toUpperCase());
            if (item == null)
                return value;

            return item.Name;
        }

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

        public static TranslateDataCheckerProcedureCodeType(value: string): string {
            var item = ko.utils.arrayFirst(ViewModel.DataCheckerProcedureCodeTypes,(i) => i.Value.toUpperCase() == (value || '').toUpperCase());
            if (item == null)
                return value;

            return item.Name;
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

    export function PrepareCollectionForDisplay(value: string, delimiter: string) : string {
        if (!value)
            return '';
        if (!delimiter)
            return value;
        return value.split(delimiter).map((s) => (s || '').trim()).join(delimiter.trim() + ' ');
    }

    export function init(query: Dns.Interfaces.IQueryComposerRequestDTO, visualTerms: IVisualTerm[], bindingControl:JQuery) : ViewModel {
        var vm = new ViewModel(query, visualTerms, bindingControl);

        $(() => {
            ko.applyBindings(vm, bindingControl[0]);
        });

        return vm;
    }


    (<any>ko.bindingHandlers).DocumentsByRevision = {
        init: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
            //element is the html element the binding is on
            //valueAccessor is[{RevisionSetID:''}]

            var val = ko.utils.unwrapObservable(valueAccessor());            
            var revisions = ko.utils.arrayMap(val,(d: any) => { return d.RevisionSetID });
            
            Dns.WebApi.Documents.ByRevisionID(revisions)
                .done(results => {                    
                    if (results && results.length > 0) {
                        results.forEach(d => $('<tr><td>' + d.Name + '</td><td>' + Global.Helpers.formatFileSize(d.Length) + '</td></tr>').appendTo(element));
                    } else {
                        $('<tr style="background-color:#eee;"><td style="text-align:center;" colspan="2">No Documents Uploaded</td></tr>').appendTo(element);
                    }
                });             
        }
    };

    


}