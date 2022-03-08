/// <reference path="../_rootlayout.ts" />
///// <reference path="../../areas/querycomposer/js/edit.ts" />

module Templates.Details {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        Template: Dns.ViewModels.TemplateViewModel;
        TemplateAcls: KnockoutObservableArray<Dns.ViewModels.AclTemplateViewModel>;
        TemplateSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclTemplateViewModel>;
        GlobalPermissionCheck: Dns.Interfaces.IHasGlobalSecurityForTemplateDTO;
        QueryDesigner: Plugins.Requests.QueryBuilder.QueryEditorHost;
        CanSave: KnockoutObservable<boolean>;
        constructor(
            bindingControl: JQuery,
            screenPermissions: any[],
            templateData: Dns.Interfaces.ITemplateDTO,
            permissionList: Dns.Interfaces.IPermissionDTO[],
            templatePermissions: Dns.Interfaces.IAclTemplateDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            globalPermission: Dns.Interfaces.IHasGlobalSecurityForTemplateDTO,
            visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
            criteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[]) {

            super(bindingControl, screenPermissions);
            this.GlobalPermissionCheck = (globalPermission == undefined || null ? { CurrentUserHasGlobalPermission: false, SecurityGroupExistsForGlobalPermission: false } : globalPermission);

            this.Template = new Dns.ViewModels.TemplateViewModel(templateData);

            this.TemplateAcls = ko.observableArray(templatePermissions.map((item) => {
                return new Dns.ViewModels.AclTemplateViewModel(item);
            }));

            this.TemplateSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, this.TemplateAcls, [
                {
                    Field: "TemplateID",
                    Value: this.Template.ID()
                }
            ], Dns.ViewModels.AclTemplateViewModel);

            this.WatchTitle(this.Template.Name, "Query Composer Template: ");

            let termsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();

            this.QueryDesigner = new Plugins.Requests.QueryBuilder.QueryEditorHost({
                IsTemplateEdit: true,
                Templates: [this.Template],
                RequestTypeModelIDs: null,
                RequestTypeTerms: null,
                TemplateType: Dns.Enums.TemplateTypes.CriteriaGroup,
                VisualTerms: visualTerms,
                CriteriaGroupTemplates: criteriaGroupTemplates,
                HiddenTerms: [],
                SupportsMultiQuery: ko.observable<boolean>(false),
                TermsObserver: termsObserver
            });

            this.CanSave = ko.observable(this.Template.CreatedByID() == User.ID || this.HasPermission(PMNPermissions.Templates.Edit));
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
                self.Template.Data(JSON.stringify(self.QueryDesigner.Queries()[0].Where.Criteria()[0].toData()));
                let data = self.Template.toData();
                let templateID = self.Template.ID();
                let templateAcls = self.TemplateAcls().map((a) => {
                    a.TemplateID(templateID);
                    return a.toData();
                });

                Dns.WebApi.Security.UpdateTemplatePermissions(templateAcls)
                    .then(() => {
                        return Dns.WebApi.Templates.Update([data]);
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
                Dns.WebApi.Templates.Delete([templateID]).done(() => {
                    window.location.href = "/templates";
                });
            });
        }
    }   

    function init() {
        let id: any = $.url().param("ID");

        $.when<any>(
                id == null ? null : Dns.WebApi.Templates.Get(id),
                id == null ? null : Dns.WebApi.Templates.GetPermissions([id], [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity]),
                Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Templates]),
                Dns.WebApi.Security.GetTemplatePermissions(id ? id : Constants.GuidEmpty),
                Dns.WebApi.Templates.GetGlobalTemplatePermissions(),
                Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
                Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(),
                Dns.WebApi.Templates.CriteriaGroups()
            ).done((
                templates: Dns.Interfaces.ITemplateDTO[],
                screenPermissions: any[],
                permissionList: Dns.Interfaces.IPermissionDTO[],
                templatePermissions: Dns.Interfaces.IAclTemplateDTO[],
                globalPermissions: Dns.Interfaces.IHasGlobalSecurityForTemplateDTO[],
                securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
                visualTerms: Plugins.Requests.QueryBuilder.IVisualTerm[],
                criteriaGroupTemplates: Dns.Interfaces.ITemplateDTO[]
            ) => {

            let template = templates[0];

            $(() => {
                let bindingControl = $('#Content');
                vm = new ViewModel(bindingControl,
                    screenPermissions || [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity],
                    template,
                    permissionList || [],
                    templatePermissions || [],
                    securityGroupTree || [],
                    globalPermissions[0],
                    visualTerms,
                    criteriaGroupTemplates);

                ko.applyBindings(vm, bindingControl[0]);
                vm.QueryDesigner.onKnockoutBind();

                $('#PageLoadingMessage').remove();
            });

        });
    }

    init();
} 