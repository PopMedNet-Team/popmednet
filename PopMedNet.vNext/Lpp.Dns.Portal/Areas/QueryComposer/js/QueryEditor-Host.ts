/// <reference path="../../../js/_rootlayout.ts" />

namespace Plugins.Requests.QueryBuilder {
    
    if (!ko.components.isRegistered('queryeditor-mdq')) {
        let templateFromFilePathLoader = {
            loadTemplate: (name, templateConfig, callback) => {
                if (templateConfig.fromUrl) {
                    $.get(templateConfig.fromUrl, (htmlString: string) => {
                        ko.components.defaultLoader.loadTemplate(name, htmlString, callback);
                    });
                } else {
                    callback(null);
                }
            }
        };

        ko.components.loaders.unshift(templateFromFilePathLoader);

        ko.components.register('queryeditor-mdq', {
            template: {
                fromUrl: '/QueryComposer/QueryEditorMDQ'
            },
            viewModel: Plugins.Requests.QueryBuilder.MDQViewModel
        });
    }

    export interface QueryEditorHostInitializationParameters {
        Templates: Dns.ViewModels.TemplateViewModel[];
        IsTemplateEdit: boolean;
        TemplateType: Dns.Enums.TemplateTypes;
        RequestTypeTerms: KnockoutObservableArray<Dns.ViewModels.RequestTypeTermViewModel>;
        RequestTypeModelIDs?: KnockoutObservableArray<any>;
        VisualTerms: IVisualTerm[];
        CriteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[];
        HiddenTerms: Dns.Interfaces.ITemplateTermDTO[]
        SupportsMultiQuery: KnockoutObservable<boolean>;
        TermsObserver: Plugins.Requests.QueryBuilder.TermsObserver
        ProjectID: any;
    }

    export interface IQueryEditorHost {
        ExportTemplates: () => Dns.Interfaces.ITemplateDTO[];
        ExportQueries: () => Dns.Interfaces.IQueryComposerQueryDTO[];
        RegisterHiddenTermExporter: (templateID: any, provider: IExportHiddenTerms) => void;
        //ValidateForFileDistribution: () => string;
    }

    export interface IExportHiddenTerms {
        ExportHiddenTerms: () => Dns.Interfaces.ISectionSpecificTermDTO[];
    }

    /**
     * QueryEditor Host contains the editors required for designing multi-query requests.
     * */
    export class QueryEditorHost implements IQueryEditorHost, IExportHiddenTerms {

        readonly Options: QueryEditorHostInitializationParameters;
        Queries: KnockoutObservableArray<Dns.ViewModels.QueryComposerQueryViewModel>;
        private QueryIDs: KnockoutComputed<any[]>;
        private QueryNames: KnockoutComputed<any[]>;
        RequestTypeID: any;
        VisualTerms: IVisualTerm[];
        CriteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[];
        private HiddenTermsTemplateMap: Global.KeyValuePair<any, IExportHiddenTerms>[] = [];
        private QueryComposerInterfaceTypes: KnockoutComputed<Dns.Structures.KeyValuePair[]>;

        /**
         * Initializes a new QueryEditorHost.
         */
        constructor(_options: QueryEditorHostInitializationParameters) {            

            this.Options = _options;
            this.VisualTerms = this.Options.VisualTerms;
            this.CriteriaGroupTemplates = this.Options.CriteriaGroupTemplates;
            
            this.Queries = ko.observableArray<Dns.ViewModels.QueryComposerQueryViewModel>(ko.utils.arrayMap(this.Options.Templates, (template: Dns.ViewModels.TemplateViewModel) => {
                let q: Dns.ViewModels.QueryComposerQueryViewModel = null;
                
                if (template.Type() == Dns.Enums.TemplateTypes.Request) {
                    //full request template that represents a full cohort query
                    q = new Dns.ViewModels.QueryComposerQueryViewModel((template.Data() || '').length > 0 ? JSON.parse(template.Data()) : null);
                } else {
                    //criteria only template that can be added to a full cohort query
                    q = new Dns.ViewModels.QueryComposerQueryViewModel();
                    q.Where.Criteria([new Dns.ViewModels.QueryComposerCriteriaViewModel(JSON.parse(template.Data()))]);
                    if (template.ComposerInterface() != Dns.Enums.QueryComposerInterface.PresetQuery) {
                        template.ComposerInterface(Dns.Enums.QueryComposerInterface.PresetQuery);
                    }
                }
                
                //TODO: potentially fix any invalid/removed terms - ie. DataChecker query type term (moved to header)
                
                //set the query header information to the template properties by reference
                q.Header.ID = template.ID;
                q.Header.Name = template.Name;
                q.Header.ComposerInterface = template.ComposerInterface;
                q.Header.QueryType = template.QueryType;
                q.Header.Description = template.Description;

                if (q.Where.Criteria() == null || q.Where.Criteria().length == 0) {
                    q.Where.Criteria.push(new Dns.ViewModels.QueryComposerCriteriaViewModel());
                    let criteria = q.Where.Criteria()[0];
                    criteria.ID(Constants.Guid.newGuid());
                    criteria.Name("Group 1");
                }

                return q;
            }));

            this.QueryIDs = ko.pureComputed<any[]>(() => {
                return ko.utils.arrayFilter(this.Queries().map((q) => q.Header.ID()), (id) => { return id != null && id != ''; });
            });

            this.QueryNames = ko.pureComputed<any[]>(() => {
                return ko.utils.arrayFilter(this.Queries().map((q) => q.Header.Name()), (name) => { return name != null && name != ''; });
            })

            this.RequestTypeID = this.Options.Templates[0].ID();

            let self = this;
            this.Options.SupportsMultiQuery.subscribe((newValue) => {
                if (newValue == false && self.Queries().length > 1) {
                    let queries = self.Queries();
                    for (let i = 1; i < queries.length; i++) {
                        self.Queries.remove(queries[i]);
                    }
                }
            });

            this.QueryComposerInterfaceTypes = ko.pureComputed<Dns.Structures.KeyValuePair[]>(() => {

                let options: Dns.Structures.KeyValuePair[] = [
                    { value: Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery, text: 'Flexible Menu Driven Query' },
                    { value: Dns.Enums.QueryComposerInterface.PresetQuery, text: 'Preset Query' },
                    { value: Dns.Enums.QueryComposerInterface.FileDistribution, text: 'File Distribution' }
                ];

                return options;
            });
                        
        }

        public onKnockoutBind() {
            let bindingContainer = $('#QueryComposerHost');
            ko.applyBindings(this, bindingContainer[0]);
        }

        public onNewCohort() {
            let q = new Dns.ViewModels.QueryComposerQueryViewModel();
            q.Header.ID(Constants.Guid.newGuid());
            q.Header.Name('Cohort ' + (this.Queries().length + 1));
            q.Header.ComposerInterface(Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery);

            let defaultCriteria = new Dns.ViewModels.QueryComposerCriteriaViewModel();
            defaultCriteria.ID(Constants.Guid.newGuid());
            defaultCriteria.Name('Group 1');
            q.Where.Criteria.push(defaultCriteria);

            this.Queries.push(q);
        }

        public onRemoveCohort(query: Dns.ViewModels.QueryComposerQueryViewModel, evt: JQueryEventObject): void {
            this.Queries.remove(query);
            this.Options.TermsObserver.RemoveTermCollection(query.Header.ID());
        }

        public ExportTemplates(): Dns.Interfaces.ITemplateDTO[] {
            let exportTemplates = this.Queries().map((query, index) => {
                let t = new Dns.ViewModels.TemplateViewModel().toData();
                t.ID = query.Header.ID();
                t.Name = query.Header.Name();
                t.Order = index;
                t.QueryType = query.Header.QueryType();
                t.ComposerInterface = query.Header.ComposerInterface();
                t.Data = JSON.stringify(query.toData());

                return t;
            });

            return exportTemplates;
        }

        public ExportQueries(): Dns.Interfaces.IQueryComposerQueryDTO[] {
            return this.Queries().map(q => q.toData());
        }

        public RegisterHiddenTermExporter(templateID: any, provider: IExportHiddenTerms) {
            let existing = ko.utils.arrayFirst(this.HiddenTermsTemplateMap, (item) => Constants.Guid.equals(item.key, templateID));
            if (existing != null) {
                existing.value = provider;
            } else {
                this.HiddenTermsTemplateMap.push(new Global.KeyValuePair<any, IExportHiddenTerms>(templateID, provider));
            }
        };

        /* Exports a collection of terms that are not allowed to be used/shown during request compostion. */
        public ExportHiddenTerms(): Dns.Interfaces.ISectionSpecificTermDTO[] {
            let hiddenTerms: Dns.Interfaces.ISectionSpecificTermDTO[] = [];

            this.HiddenTermsTemplateMap.forEach((m) => {
                let ht = m.value.ExportHiddenTerms();
                if (ht && ht.length > 0) {
                    hiddenTerms = hiddenTerms.concat(ht);
                }
            });

            return hiddenTerms;
        }

        public onValidateEditors(): boolean {
            let queryIDs = this.QueryIDs();
            for (let i = 0; i < queryIDs.length; i++) {
                let editor = ko.contextFor(document.getElementById('queryeditor-mdq-' + queryIDs[i]).children[0]).$component as MDQViewModel;
                if (editor) {      
                    if (editor.AreTermsValid() == false) {
                        return false;
                    }
                }
            }

            return true;
        }

        public VerifyNoDuplicates(): boolean {
            let cohortNames = this.QueryNames();
            let duplicates = cohortNames.filter((item, index) => cohortNames.indexOf(item) != index);
            if (duplicates.length > 0) {
                Global.Helpers.ShowAlert("Validation Error", "Cohort names must be unique.");
                return false;
            }
            else return true;
        }

        public onExportJSON(): string {
            let queries = this.Queries().map((k) => k.toData());
            return 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(queries));
        }
    }
}