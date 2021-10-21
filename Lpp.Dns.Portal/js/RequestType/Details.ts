/// <reference path="../_rootlayout.ts" />
module RequestType.Details {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        RequestTypeID: any;
        RequestType: Dns.ViewModels.RequestTypeViewModel;
        SelectedModels: KnockoutObservableArray<any>;

        RequestTypeAcls: KnockoutObservableArray<Dns.ViewModels.AclRequestTypeViewModel>;
        RequestTypeSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclRequestTypeViewModel>;

        Workflows: Dns.Interfaces.IWorkflowDTO[];

        RequestTypeTerms: KnockoutObservableArray<Dns.ViewModels.RequestTypeTermViewModel>;
        TermList: Dns.Interfaces.ITermDTO[];
        AddableTerms: KnockoutComputed<Dns.Interfaces.ITermDTO[]>;

        Save: () => void;
        Delete: () => void;
        DeleteTerm: (requestTypeTerm: Dns.ViewModels.RequestTypeTermViewModel) => void;
        AddRequestTypeTerm: (term: Dns.Interfaces.ITermDTO) => void;

        QueryDesigner: Plugins.Requests.QueryBuilder.QueryEditorHost;

        constructor(requestType: Dns.Interfaces.IRequestTypeDTO,
            requestTypeModels: Dns.Interfaces.IRequestTypeModelDTO[],
            requestTypeTerms: Dns.Interfaces.IRequestTypeTermDTO[],
            bindingControl: JQuery, screenPermissions: any[],
            permissionList: Dns.Interfaces.IPermissionDTO[],
            requestTypePermissions: Dns.Interfaces.IAclRequestTypeDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            workflows: Dns.Interfaces.IWorkflowDTO[],
            templates: Dns.Interfaces.ITemplateDTO[],
            termList: Dns.Interfaces.ITermDTO[],
            visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
            criteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[],
            hiddenTerms: Dns.Interfaces.ITemplateTermDTO[]
        ) {
            super(bindingControl, screenPermissions);

            let self = this;

            self.Workflows = workflows;

            self.RequestType = new Dns.ViewModels.RequestTypeViewModel(requestType);
            self.RequestTypeID = requestType.ID;
            self.RequestTypeTerms = ko.observableArray(requestTypeTerms.map((item) => {
                return new Dns.ViewModels.RequestTypeTermViewModel(item);
            }));
            self.TermList = termList;

            self.SelectedModels = ko.observableArray(ko.utils.arrayFilter(requestTypeModels, (rtm) => {
                let modelID = rtm.DataModelID.toLowerCase();
                return modelID == '321adaa1-a350-4dd0-93de-5de658a507df' || //Data Characterization
                    modelID == '7c69584a-5602-4fc0-9f3f-a27f329b1113' || //ESP
                    modelID == '85ee982e-f017-4bc4-9acd-ee6ee55d2446' || //PCORnet
                    modelID == 'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb' || //Summary Tables
                    modelID == '4c8a25dc-6816-4202-88f4-6d17e72a43bc' || //Distributed Regression
                    modelID == '1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154'; //Modular Program

            }).map((item) => {
                    return item.DataModelID.toLowerCase();
                })).extend({ rateLimit: 900, method: 'notifyWhenChangesStop' });            


            let termsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();

            this.QueryDesigner = new Plugins.Requests.QueryBuilder.QueryEditorHost({
                Templates: ko.utils.arrayMap(templates, (t) => new Dns.ViewModels.TemplateViewModel(t)),
                IsTemplateEdit: true,
                TemplateType: Dns.Enums.TemplateTypes.Request,
                RequestTypeTerms: self.RequestTypeTerms,
                RequestTypeModelIDs: self.SelectedModels,
                VisualTerms: visualTerms,
                CriteriaGroupTemplates: criteriaGroupTemplates,
                HiddenTerms: hiddenTerms,
                SupportsMultiQuery: ko.pureComputed<boolean>(() => self.RequestType.SupportMultiQuery()),
                TermsObserver: termsObserver
            });

            self.AddableTerms = ko.computed<Dns.Interfaces.ITermDTO[]>(() => {
                let results = self.TermList.filter((t) => {
                    let exists = false;
                    self.RequestTypeTerms().forEach((rtt) => {
                        if (rtt.TermID() == t.ID) {
                            exists = true;
                            return;
                        }
                    });

                    return !exists;
                });

                return results.sort(function (left, right) { return left.Name == right.Name ? 0 : (left.Name < right.Name ? -1 : 1) });
            });

            self.RequestTypeAcls = ko.observableArray(requestTypePermissions.map((item) => {
                return new Dns.ViewModels.AclRequestTypeViewModel(item);
            }));

            self.RequestTypeSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, self.RequestTypeAcls, [
                {
                    Field: "RequestTypeID",
                    Value: self.RequestTypeID
                }
            ], Dns.ViewModels.AclRequestTypeViewModel);


            self.WatchTitle(this.RequestType.Name, "Request Type: ");

            self.Save = () => {
                if (self.RequestType.WorkflowID() == null || self.RequestType.WorkflowID() == "") {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have a Workflow selected.</p>");
                    return;
                }

                if (this.QueryDesigner.onValidateEditors() == false)
                    return;

                //make sure that if the composer contains a file upload or modular term that it is only single query, and the interface is set to File Distribution and multi-query is disabled
                

                if (!super.Validate())
                    return;

                let requestTypeAcls = self.RequestTypeAcls();
                if (requestTypeAcls.length == 0) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this request type.</p>");
                    return;
                }

                let update = new Dns.ViewModels.UpdateRequestTypeRequestViewModel().toData();
                update.RequestType = self.RequestType.toData();
                update.Queries = self.QueryDesigner.ExportTemplates();
                update.Models = self.SelectedModels();
                update.Terms = self.RequestTypeTerms().map((t) => t.TermID());
                update.NotAllowedTerms = self.QueryDesigner.ExportHiddenTerms();
                update.Permissions = requestTypeAcls.map((a) => {
                    a.RequestTypeID(self.RequestTypeID);
                    return a.toData();
                });

                Dns.WebApi.RequestTypes.Save(update).done((results: Dns.Interfaces.IUpdateRequestTypeResponseDTO[]) => {
                    let result = results[0];
                    self.RequestType.ID(result.RequestType.ID);
                    self.RequestTypeID = result.RequestType.ID;
                    self.RequestType.Timestamp(result.RequestType.Timestamp);

                    window.history.replaceState(null, window.document.title, "/requesttype/details?ID=" + self.RequestTypeID);

                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                });

            };

            self.Delete = () => {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Request Type?</p>").done(() => {
                    Dns.WebApi.RequestTypes.Delete([self.RequestTypeID]).done(() => {
                        window.location.href = "/requesttype";
                    });
                });
            };

            self.DeleteTerm = (requestTypeTerm: Dns.ViewModels.RequestTypeTermViewModel) => {
                self.RequestTypeTerms.remove(requestTypeTerm);
            };

            self.AddRequestTypeTerm = (term: Dns.Interfaces.ITermDTO) => {
                self.RequestTypeTerms.push(new Dns.ViewModels.RequestTypeTermViewModel({
                    Description: term.Description,
                    OID: term.OID,
                    ReferenceUrl: term.ReferenceUrl,
                    RequestTypeID: self.RequestTypeID,
                    Term: term.Name,
                    TermID: term.ID
                }));
            };
        }

        onConfirmChangeToSingleQuery(data: ViewModel, evt: JQueryEventObject): boolean {
            let self = this;

            if (data.RequestType.SupportMultiQuery() && data.QueryDesigner.Queries().length > 1) {
                //changing from multi-query to single, warn will delete any queries after the first
                Global.Helpers.ShowConfirm('Please Confirm', '<p class="alert alert-warning">Changing to single Query will remove all Cohorts except for the first. Proceed?</p>')
                    .done(() => {
                        self.RequestType.SupportMultiQuery(false);
                    });

                evt.stopImmediatePropagation();
                return false;
            }
            return true;
        }

        public Cancel() {
            window.location.href = "/requesttype";
        }
    }

    function init() {
        let id: any = $.url().param("ID");
        $.when<any>(
            id == null ? null : Dns.WebApi.RequestTypes.Get(id),
            id == null ? null : Dns.WebApi.Templates.GetByRequestType(id),
            id == null ? [] : Dns.WebApi.RequestTypes.GetRequestTypeModels(id),
            id == null ? null : Dns.WebApi.RequestTypes.GetRequestTypeTerms(id),
            id == null ? null : Dns.WebApi.RequestTypes.GetPermissions([id], [PMNPermissions.RequestTypes.Delete, PMNPermissions.RequestTypes.Edit, PMNPermissions.RequestTypes.ManageSecurity]),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.RequestTypes]),
            Dns.WebApi.Security.GetRequestTypePermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            Dns.WebApi.Workflow.List(null, "ID,Name", "Name"),
            Dns.WebApi.Terms.List(),
            Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(),
            Dns.WebApi.Templates.CriteriaGroups(),
            Dns.WebApi.Templates.ListHiddenTermsByRequestType(id ? id : Constants.GuidEmpty),
        ).done((
            requestTypes: Dns.Interfaces.IRequestTypeDTO[],
            templates: Dns.Interfaces.ITemplateDTO[],
            requestTypeModels: Dns.Interfaces.IRequestTypeModelDTO[],
            requestTypeTerms: Dns.Interfaces.IRequestTypeTermDTO[],
            screenPermissions,
            permissionList,
            requestTypePermissions: Dns.Interfaces.IAclRequestTypeDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            workflows: Dns.Interfaces.IWorkflowDTO[],
            termList: Dns.Interfaces.ITermDTO[],
            visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
            criteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[],
            hiddenTerms: Dns.Interfaces.ITemplateTermDTO[]
        ) => {
            let requestType = (requestTypes == null || requestTypes.length == 0) ? new Dns.ViewModels.RequestTypeViewModel().toData() : requestTypes[0];

            if (templates == null || templates.length == 0) {
                let template = new Dns.ViewModels.TemplateViewModel().toData();
                template.Name = 'Cohort 1';
                template.Type = Dns.Enums.TemplateTypes.Request;
                template.ComposerInterface = Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery;
                template.QueryType = null;

                templates = [template];
            }

            $(() => {
                let bindingControl = $('#Content');
                vm = new ViewModel(
                    requestType,
                    requestTypeModels || [],
                    requestTypeTerms || [],
                    bindingControl,
                    screenPermissions || [PMNPermissions.RequestTypes.Delete, PMNPermissions.RequestTypes.Edit, PMNPermissions.RequestTypes.ManageSecurity],
                    permissionList || [],
                    requestTypePermissions || [],
                    securityGroupTree || [],
                    workflows,
                    templates,
                    termList,
                    visualTerms,
                    criteriaGroupTemplates,
                    hiddenTerms
                );

                $('#tabs').kendoTabStrip().data('kendoTabStrip').bind('show', (e) => {
                    if ($(e.contentElement).has('#txtNotes')) {
                        //to make the kendo editor initialize correctly it needs to be refreshed when the tab is show
                        let editor = $('#txtNotes').data('kendoEditor');
                        editor.refresh();
                    }
                });

                ko.applyBindings(vm, bindingControl[0]);
                vm.QueryDesigner.onKnockoutBind();

                $('#PageLoadingMessage').remove();

            });

        });
    }

    init();
}