import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import * as Enums from '../Dns.Enums.js';
import * as SecurityAcl from '../security/AclViewModel.js';

export class ViewModel extends Global.PageViewModel {
    RequestTypeID: any;
    RequestType: ViewModels.RequestTypeViewModel;
    SelectedModels: KnockoutObservableArray<any>;

    RequestTypeAcls: KnockoutObservableArray<ViewModels.AclRequestTypeViewModel>;
    RequestTypeSecurity: SecurityAcl.AclEditViewModel<ViewModels.AclRequestTypeViewModel>;

    Workflows: Interfaces.IWorkflowDTO[];

    RequestTypeTerms: KnockoutObservableArray<ViewModels.RequestTypeTermViewModel>;
    TermList: Interfaces.ITermDTO[];
    AddableTerms: KnockoutComputed<Interfaces.ITermDTO[]>;

    Save: () => void;
    Delete: () => void;
    DeleteTerm: (requestTypeTerm: ViewModels.RequestTypeTermViewModel) => void;
    AddRequestTypeTerm: (term: Interfaces.ITermDTO) => void;
    public CanEdit: boolean;
    public CanDelete: boolean;

    //TODO: vNext implement
    //QueryDesigner: Plugins.Requests.QueryBuilder.QueryEditorHost;

    constructor(requestType: Interfaces.IRequestTypeDTO,
        requestTypeModels: Interfaces.IRequestTypeModelDTO[],
        requestTypeTerms: Interfaces.IRequestTypeTermDTO[],
        bindingControl: JQuery, screenPermissions: any[],
        permissionList: Interfaces.IPermissionDTO[],
        requestTypePermissions: Interfaces.IAclRequestTypeDTO[],
        securityGroupTree: Interfaces.ITreeItemDTO[],
        workflows: Interfaces.IWorkflowDTO[],
        templates: Interfaces.ITemplateDTO[],
        termList: Interfaces.ITermDTO[],
        //visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
        visualTerms: any,
        criteriaGroupTemplates: Interfaces.ITemplateDTO[],
        hiddenTerms: Interfaces.ITemplateTermDTO[]
    ) {
        super(bindingControl, screenPermissions);

        let self = this;

        self.Workflows = workflows;

        self.RequestType = new ViewModels.RequestTypeViewModel(requestType);
        self.RequestTypeID = requestType.ID;
        self.RequestTypeTerms = ko.observableArray(requestTypeTerms.map((item) => {
            return new ViewModels.RequestTypeTermViewModel(item);
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

        this.CanEdit = this.HasPermission(PMNPermissions.RequestTypes.Edit);
        this.CanDelete = this.HasPermission(PMNPermissions.RequestTypes.Delete);

        //TODO: vNext implement
        //let termsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();
        //TODO: vNext implement
        //this.QueryDesigner = new Plugins.Requests.QueryBuilder.QueryEditorHost({
        //    Templates: ko.utils.arrayMap(templates, (t) => new ViewModels.TemplateViewModel(t)),
        //    IsTemplateEdit: true,
        //    TemplateType: Enums.TemplateTypes.Request,
        //    RequestTypeTerms: self.RequestTypeTerms,
        //    RequestTypeModelIDs: self.SelectedModels,
        //    VisualTerms: visualTerms,
        //    CriteriaGroupTemplates: criteriaGroupTemplates,
        //    HiddenTerms: hiddenTerms,
        //    SupportsMultiQuery: ko.pureComputed<boolean>(() => self.RequestType.SupportMultiQuery()),
        //    TermsObserver: termsObserver,
        //    ProjectID: null
        //});

        self.AddableTerms = ko.computed<Interfaces.ITermDTO[]>(() => {
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
            return new ViewModels.AclRequestTypeViewModel(item);
        }));

        self.RequestTypeSecurity = new SecurityAcl.AclEditViewModel(permissionList, securityGroupTree, self.RequestTypeAcls, [
            {
                Field: "RequestTypeID",
                Value: self.RequestTypeID
            }
        ], ViewModels.AclRequestTypeViewModel);


        self.WatchTitle(this.RequestType.Name, "Request Type: ");

        self.Save = () => {
            if (self.RequestType.WorkflowID() == null || self.RequestType.WorkflowID() == "") {
                Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have a Workflow selected.</p>");
                return;
            }

            //TODO: vNext implement
            //if (this.QueryDesigner.onValidateEditors() == false)
            //    return;

            //make sure that if the composer contains a file upload or modular term that it is only single query, and the interface is set to File Distribution and multi-query is disabled


            if (!super.Validate())
                return;

            let requestTypeAcls = self.RequestTypeAcls();
            if (requestTypeAcls.length == 0) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this request type.</p>");
                return;
            }

            let update = new ViewModels.UpdateRequestTypeRequestViewModel().toData();
            update.RequestType = self.RequestType.toData();
            //update.Queries = self.QueryDesigner.ExportTemplates();
            update.Models = self.SelectedModels();
            update.Terms = self.RequestTypeTerms().map((t) => t.TermID());
            //update.NotAllowedTerms = self.QueryDesigner.ExportHiddenTerms();
            update.Permissions = requestTypeAcls.map((a) => {
                a.RequestTypeID(self.RequestTypeID);
                return a.toData();
            });

            WebApi.RequestTypes.Save(update).done((results: Interfaces.IUpdateRequestTypeResponseDTO[]) => {
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
                WebApi.RequestTypes.Delete([self.RequestTypeID]).done(() => {
                    window.location.href = "/requesttype";
                });
            });
        };

        self.DeleteTerm = (requestTypeTerm: ViewModels.RequestTypeTermViewModel) => {
            self.RequestTypeTerms.remove(requestTypeTerm);
        };

        self.AddRequestTypeTerm = (term: Interfaces.ITermDTO) => {
            self.RequestTypeTerms.push(new ViewModels.RequestTypeTermViewModel({
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
        //TODO: vNext implement
        //let self = this;

        //if (data.RequestType.SupportMultiQuery() && data.QueryDesigner.Queries().length > 1) {
        //    //changing from multi-query to single, warn will delete any queries after the first
        //    Global.Helpers.ShowConfirm('Please Confirm', '<p class="alert alert-warning">Changing to single Query will remove all Cohorts except for the first. Proceed?</p>')
        //        .done(() => {
        //            self.RequestType.SupportMultiQuery(false);
        //        });

        //    evt.stopImmediatePropagation();
        //    return false;
        //}
        return true;
    }

    public Cancel() {
        window.location.href = "/requesttype";
    }
}

let id: string = Global.GetQueryParam("ID");
$.when<any>(
    id == null ? null : WebApi.RequestTypes.Get(id),
    id == null ? null : WebApi.Templates.GetByRequestType(id),
    id == null ? [] : WebApi.RequestTypes.GetRequestTypeModels(id),
    id == null ? null : WebApi.RequestTypes.GetRequestTypeTerms(id),
    id == null ? null : WebApi.RequestTypes.GetPermissions([id], [PMNPermissions.RequestTypes.Delete, PMNPermissions.RequestTypes.Edit, PMNPermissions.RequestTypes.ManageSecurity]),
    WebApi.Security.GetPermissionsByLocation([Enums.PermissionAclTypes.RequestTypes]),
    WebApi.Security.GetRequestTypePermissions(id ? id : Constants.GuidEmpty),
    WebApi.Security.GetAvailableSecurityGroupTree(),
    WebApi.Workflow.List(null, "ID,Name", "Name"),
    WebApi.Terms.List(),
    null,//Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(),
    WebApi.Templates.CriteriaGroups(),
    WebApi.Templates.ListHiddenTermsByRequestType(id ? id : Constants.GuidEmpty),
).done((
    requestTypes: Interfaces.IRequestTypeDTO,
    templates: Interfaces.ITemplateDTO[],
    requestTypeModels: Interfaces.IRequestTypeModelDTO[],
    requestTypeTerms: Interfaces.IRequestTypeTermDTO[],
    screenPermissions,
    permissionList,
    requestTypePermissions: Interfaces.IAclRequestTypeDTO[],
    securityGroupTree: Interfaces.ITreeItemDTO[],
    workflows: Interfaces.IWorkflowDTO[],
    termList: Interfaces.ITermDTO[],
    //TODO: vNext implement
    //visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
    visualTerms:any,
    criteriaGroupTemplates: Interfaces.ITemplateDTO[],
    hiddenTerms: Interfaces.ITemplateTermDTO[]
) => {
    let requestType = (requestTypes == null) ? new ViewModels.RequestTypeViewModel().toData() : requestTypes;

    if (templates == null || templates.length == 0) {
        let template = new ViewModels.TemplateViewModel().toData();
        template.Name = 'Cohort 1';
        template.Type = Enums.TemplateTypes.Request;
        template.ComposerInterface = Enums.QueryComposerInterface.FlexibleMenuDrivenQuery;
        template.QueryType = null;

        templates = [template];
    }

    $(() => {
        let bindingControl = $('#Content');
        let vm = new ViewModel(
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
        //TODO: vNext implement
        //vm.QueryDesigner.onKnockoutBind();

        $('#PageLoadingMessage').remove();

    });

});