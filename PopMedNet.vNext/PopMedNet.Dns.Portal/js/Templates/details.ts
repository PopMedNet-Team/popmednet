import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";
import { PMNPermissions } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import * as Enums from '../Dns.Enums.js';
import * as SecurityAcl from '../security/AclViewModel.js';

export class ViewModel extends Global.PageViewModel {
    Template: ViewModels.TemplateViewModel;
    TemplateAcls: KnockoutObservableArray<ViewModels.AclTemplateViewModel>;
    TemplateSecurity: SecurityAcl.AclEditViewModel<ViewModels.AclTemplateViewModel>;
    GlobalPermissionCheck: Interfaces.IHasGlobalSecurityForTemplateDTO;
    //QueryDesigner: Plugins.Requests.QueryBuilder.QueryEditorHost;
    CanSave: KnockoutObservable<boolean>;
    CanEdit: KnockoutObservable<boolean>;
    CanDelete: KnockoutObservable<boolean>;
    TemplateTypeName: KnockoutComputed<string>;
    QueryComposerQueryTypesTranslation = Enums.QueryComposerQueryTypesTranslation;
    DisplayQueryType: KnockoutComputed<string>;

    constructor(
        bindingControl: JQuery,
        screenPermissions: any[],
        templateData: Interfaces.ITemplateDTO,
        permissionList: Interfaces.IPermissionDTO[],
        templatePermissions: Interfaces.IAclTemplateDTO[],
        securityGroupTree: Interfaces.ITreeItemDTO[],
        globalPermission: Interfaces.IHasGlobalSecurityForTemplateDTO,
        visualTerms: any,//visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
        criteriaGroupTemplates: Interfaces.ITemplateDTO[]) {

        super(bindingControl, screenPermissions);
        this.GlobalPermissionCheck = (globalPermission == undefined || null ? { CurrentUserHasGlobalPermission: false, SecurityGroupExistsForGlobalPermission: false } : globalPermission);

        this.Template = new ViewModels.TemplateViewModel(templateData);

        this.TemplateAcls = ko.observableArray(templatePermissions.map((item) => {
            return new ViewModels.AclTemplateViewModel(item);
        }));

        this.TemplateSecurity = new SecurityAcl.AclEditViewModel(permissionList, securityGroupTree, this.TemplateAcls, [
            {
                Field: "TemplateID",
                Value: this.Template.ID()
            }
        ], ViewModels.AclTemplateViewModel);

        this.WatchTitle(this.Template.Name, "Query Composer Template: ");
        
        this.DisplayQueryType = ko.computed({
            read: function () {
                return this.Template.QueryType();
            },
            write: function (value) {
                if (value == null || value == '') {
                    this.Template.QueryType(null);
                } else {
                    this.Template.QueryType(parseInt(value));
                }
            }
        }, this);

        //let termsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();

        //this.QueryDesigner = new Plugins.Requests.QueryBuilder.QueryEditorHost({
        //    IsTemplateEdit: true,
        //    Templates: [this.Template],
        //    RequestTypeModelIDs: null,
        //    RequestTypeTerms: null,
        //    TemplateType: Dns.Enums.TemplateTypes.CriteriaGroup,
        //    VisualTerms: visualTerms,
        //    CriteriaGroupTemplates: criteriaGroupTemplates,
        //    HiddenTerms: [],
        //    SupportsMultiQuery: ko.observable<boolean>(false),
        //    TermsObserver: termsObserver,
        //    ProjectID: null
        //});

        this.CanSave = ko.observable(this.Template.CreatedByID() == Global.User.ID || this.HasPermission(PMNPermissions.Templates.Edit));
        this.CanEdit = ko.observable(this.HasPermission(PMNPermissions.Templates.Edit));
        this.CanDelete = ko.observable(this.HasPermission(PMNPermissions.Templates.Delete));

        this.TemplateTypeName = ko.pureComputed(() => Global.Helpers.GetEnumString(Enums.TemplateTypesTranslation, this.Template.Type()));
    }

    Save() {
        if (!super.Validate())
            return;

        if (this.Template.Name().length == 0) {
            Global.Helpers.ShowAlert("Required Field", "<p><i>Template Name</i> is a required field and may not be left blank.</p>");
            return;
        }

        let deferred: JQueryDeferred<any> = null;
        if (this.TemplateAcls().length == 0) {
            if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == false) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this template.</p>");
                return;
            } else {
                if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                    deferred = Global.Helpers.ShowConfirm("Validation Error", "<p>You do not have permission to edit this template, you should add at least one security group to the Permissions tab if applicable. <br>Would you like to save anyway?</p>");
                    return;
                } else {
                    if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == true)) {
                    }
                }
            }
        }

        if (deferred == null) {
            deferred = $.Deferred<any>();
            deferred.resolve();
        }

        let self = this as ViewModel;
        deferred.done(() => {
            //self.Template.Data(JSON.stringify(self.QueryDesigner.Queries()[0].Where.Criteria()[0].toData()));
            let data = self.Template.toData();            

            let templateID = self.Template.ID();
            let templateAcls = self.TemplateAcls().map((a) => {
                a.TemplateID(templateID);
                return a.toData();
            });

            WebApi.Security.UpdateTemplatePermissions(templateAcls)
                .then(() => {
                    return WebApi.Templates.InsertOrUpdate([data]);
                })
                .done((results) => {
                    self.Template.Timestamp(results[0].Timestamp);
                    window.history.replaceState(null, window.document.title, "/templates/details?ID=" + results[0].ID);
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                });
        });
    }

    Cancel() {
        window.location.href = "/templates";
    }

    Delete() {
        let templateID = this.Template.ID();
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Template?</p>").done(() => {
            WebApi.Templates.Delete([templateID]).done(() => {
                window.location.href = "/templates";
            });
        });
    }
}

let id: any = Global.GetQueryParam("ID");

$.when<any>(
    id == null ? null : WebApi.Templates.Get(id),
    id == null ? null : WebApi.Templates.GetPermissions([id], [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity]),
    WebApi.Security.GetPermissionsByLocation([Enums.PermissionAclTypes.Templates]),
    WebApi.Security.GetTemplatePermissions(id ? id : Constants.GuidEmpty),
    WebApi.Templates.GetGlobalTemplatePermissions(),
    WebApi.Security.GetAvailableSecurityGroupTree(),
    null,//Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(),
    WebApi.Templates.CriteriaGroups()
).done((
    template: Interfaces.ITemplateDTO,
    screenPermissions: any[],
    permissionList: Interfaces.IPermissionDTO[],
    templatePermissions: Interfaces.IAclTemplateDTO[],
    globalPermissions: Interfaces.IHasGlobalSecurityForTemplateDTO[],
    securityGroupTree: Interfaces.ITreeItemDTO[],
    visualTerms: any,// Plugins.Requests.QueryBuilder.IVisualTerm[],
    criteriaGroupTemplates: Interfaces.ITemplateDTO[]
) => {

    $(() => {
        let bindingControl = $('#Content');
        let vm = new ViewModel(bindingControl,
            screenPermissions || [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity],
            template,
            permissionList || [],
            templatePermissions || [],
            securityGroupTree || [],
            globalPermissions[0],
            visualTerms,
            criteriaGroupTemplates);

        ko.applyBindings(vm, bindingControl[0]);
        //vm.QueryDesigner.onKnockoutBind();

        $('#PageLoadingMessage').remove();
    });

});