/// <reference path="../../../js/_rootlayout.ts" />
module Plugins.Requests.QueryBuilder.View {

    export class ViewModel extends Global.PageViewModel {

        public Request: Dns.Interfaces.IQueryComposerRequestDTO;

        constructor(request: Dns.Interfaces.IQueryComposerRequestDTO, bindingControl: JQuery) {
            super(bindingControl);
            
            //TODO: look into how to easily confirm the properties on an object without going back and forth between interface and viewmodel
            this.Request = new Dns.ViewModels.QueryComposerRequestViewModel(request).toData();
        }

        public FilterForNonAggregateFields(query: Dns.Interfaces.IQueryComposerQueryDTO): Dns.Interfaces.IQueryComposerFieldDTO[] {

            return ko.utils.arrayFilter(query.Select.Fields, (f) => { return f.Aggregate == null; });
        }

        public ShowSubCriteriaConjuction(parentCriteria: Dns.Interfaces.IQueryComposerCriteriaDTO, subCriteria: Dns.Interfaces.IQueryComposerCriteriaDTO) {
            if (parentCriteria.Criteria.length < 2)
                return false;

            if (parentCriteria.Criteria.indexOf(subCriteria) == 0) {
                return false;
            }

            return true;
        }

        public TemplateSelector(data: Dns.Interfaces.IQueryComposerTermDTO) {
            return "v_" + data.Type.toLowerCase();
        }

        public StratifierTemplateSelector(data: Dns.Interfaces.IQueryComposerTermDTO) {
            return "sv_" + data.Type.toLowerCase();
        }

        public static DataCheckerDiagnosisCodeTypes = new Array(
            { Name: 'Any', Value: '' },
            { Name: 'ICD-9-CM', Value: '09' },
            { Name: 'ICD-10-CM', Value: '10' },
            { Name: 'ICD-11-CM', Value: '11' },
            { Name: 'SNOMED CT', Value: 'SM' },
            { Name: 'Other', Value: 'OT' });

        public static TranslateDataCheckerDiagnosisCodeType(value: string): string {
            let item = ko.utils.arrayFirst(ViewModel.DataCheckerDiagnosisCodeTypes,(i) => i.Value.toUpperCase() == (value || '').toUpperCase());
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
            { Name: 'Other', Value: 'OT' }
        );

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

        public static TranslateDataCheckerProcedureCodeType(value: string): string {
            let item = ko.utils.arrayFirst(ViewModel.DataCheckerProcedureCodeTypes,(i) => i.Value.toUpperCase() == (value || '').toUpperCase());
            if (item == null)
                return value;

            return item.Name;
        }
        
    }

    export function PrepareCollectionForDisplay(value: string, delimiter: string) : string {
        if (!value)
            return '';
        if (!delimiter)
            return value;
        return value.split(delimiter).map((s) => (s || '').trim()).join(delimiter.trim() + ' ');
    }

    export function initialize(query: Object, requestVM: Dns.ViewModels.RequestViewModel, bindingControl: JQuery): ViewModel {
        let queryRequestDTO: Dns.Interfaces.IQueryComposerRequestDTO;
        if ((<any>query).hasOwnProperty('SchemaVersion') == false) {
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

            queryRequestDTO.Queries = [query as Dns.Interfaces.IQueryComposerQueryDTO];

        } else {
            queryRequestDTO = query as Dns.Interfaces.IQueryComposerRequestDTO;
        }

        let vm = new ViewModel(queryRequestDTO, bindingControl);

        $(() => {
            ko.applyBindings(vm, bindingControl[0]);
        });

        return vm;
    }

    export function init(query: Object, visualTerms: IVisualTerm[], bindingControl: JQuery): ViewModel {
        //deprecated for initialize()
        throw new DOMException("Deprecated for initialize().");
        return null;
    }


    (<any>ko.bindingHandlers).DocumentsByRevision = {
        init: (element, valueAccessor, allBindings, viewModel, bindingContext) => {
            //element is the html element the binding is on
            //valueAccessor is[{RevisionSetID:''}]

            let val = ko.utils.unwrapObservable(valueAccessor());            
            let revisions = ko.utils.arrayMap(val,(d: any) => { return d.RevisionSetID });
            
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