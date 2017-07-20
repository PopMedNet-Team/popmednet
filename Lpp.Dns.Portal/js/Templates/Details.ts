/// <reference path="../_rootlayout.ts" />
/// <reference path="../../areas/querycomposer/js/edit.ts" />

module Templates.Details {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public Template: Dns.ViewModels.TemplateViewModel;
        public TemplateAcls: KnockoutObservableArray<Dns.ViewModels.AclTemplateViewModel>;
        public TemplateSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclTemplateViewModel>;
        public GlobalPermissionCheck: Dns.Interfaces.IHasGlobalSecurityForTemplateDTO;

        constructor(
            bindingControl: JQuery,
            screenPermissions: any[],
            templateData: Dns.Interfaces.ITemplateDTO,
            permissionList: Dns.Interfaces.IPermissionDTO[],
            templatePermissions: Dns.Interfaces.IAclTemplateDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            globalPermission: Dns.Interfaces.IHasGlobalSecurityForTemplateDTO) {

            super(bindingControl, screenPermissions);
            this.GlobalPermissionCheck = (globalPermission == undefined || null ? { CurrentUserHasGlobalPermission: false, SecurityGroupExistsForGlobalPermission: false } : globalPermission);

            this.Template = new Dns.ViewModels.TemplateViewModel(templateData);
            Global.Helpers.GetEnumString(Dns.Enums.TemplateTypesTranslation, this.Template.Type());

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
            
            this.Template.QueryType.subscribe((value) => {                
                Plugins.Requests.QueryBuilder.MDQ.vm.Request.Header.QueryType(value);
                Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList([], value, []);           
            });
        }

        public Save() {
            if (!super.Validate())
                return;
            
            if (this.Template.Name().length == 0) {
                Global.Helpers.ShowAlert("Required Field", "<p><i>Template Name</i> is a required field and may not be left blank.</p>");
                return;
            }

            if (this.TemplateAcls().length == 0) {
                if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == false) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this template.</p>");
                    return;
                } else {
                    if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                        Global.Helpers.ShowConfirm("Validation Error", "<p>You do not have permission to edit this template, you should add at least one security group to the Permissions tab if applicable. <br>Would you like to save anyway?</p>").done(() => { this.VerifiedSave() });
                        return;
                    } else {
                        if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == true)) {
                        }
                    }
                }
            }
            this.VerifiedSave();
        }

        public VerifiedSave() {
            if (!super.Validate())
                return;

            if (this.Template.Type() == Dns.Enums.TemplateTypes.CriteriaGroup) {
                this.Template.Data(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.Where.Criteria()[0].toData()));
            } else {                
                this.Template.Data(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
            }
            
            var data = this.Template.toData();
            Dns.WebApi.Templates.InsertOrUpdate([data]).done((results) => {
                var template = results[0];
                this.Template.ID(template.ID);
                this.Template.Timestamp(template.Timestamp);
                window.history.replaceState(null, window.document.title, "/templates/details?ID=" + template.ID);

                var templateAcls = this.TemplateAcls().map((a) => {
                    a.TemplateID(this.Template.ID());
                    return a.toData();
                });

                Dns.WebApi.Security.UpdateTemplatePermissions(templateAcls).done(() => {
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                });
            });
        }

        public Cancel() {
            window.location.href = "/templates";
        }

        public Delete() {
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Template?</p>").done(() => {
                Dns.WebApi.Templates.Delete([vm.Template.ID()]).done(() => {
                    window.location.href = "/templates";
                });
            });
        }
    }   
    
    function GetVisualTerms(): JQueryDeferred<IVisualTerm[]> {
        var d = $.Deferred<IVisualTerm[]>();

        $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
            .done((result: IVisualTerm[]) => {
            d.resolve(result);
        }).fail((e, description, error) => {
            d.reject(<any>e);
        });

        return d;
    } 

    function init() {
        var id: any = $.url().param("ID");

        $.when<any>(
                id == null ? null : Dns.WebApi.Templates.Get(id),
                id == null ? null : Dns.WebApi.Templates.GetPermissions([id], [Permissions.Templates.Delete, Permissions.Templates.Edit, Permissions.Templates.ManageSecurity]),
                Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Templates]),
                Dns.WebApi.Security.GetTemplatePermissions(id ? id : Constants.GuidEmpty),
                Dns.WebApi.Templates.GetGlobalTemplatePermissions(),
                Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
                GetVisualTerms()
            ).done((
                templates: Dns.Interfaces.ITemplateDTO[],
                screenPermissions: any[],
                permissionList: Dns.Interfaces.IPermissionDTO[],
                templatePermissions: Dns.Interfaces.IAclTemplateDTO[],
                globalPermissions: Dns.Interfaces.IHasGlobalSecurityForTemplateDTO[],
                securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
                visualTerms: IVisualTerm[]
            ) => {
            
            var template: Dns.Interfaces.ITemplateDTO = templates == null ? {
                ID: null,
                Name: '',
                Description: '',
                CreatedBy: User.AuthInfo.UserName,
                CreatedByID: User.ID,
                CreatedOn: moment().utc().toDate(),
                Data: '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[],"ObservationPeriod":{}}]}}',
                Timestamp: null,
                Type: Dns.Enums.TemplateTypes.Request,
                Notes: ''
            } : templates[0];

            var json: any = JSON.parse(((template.Data || '').trim() != '') ? template.Data : '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[],"ObservationPeriod":{}}]}}');

            var jTemplate: Dns.Interfaces.IQueryComposerRequestDTO;

            if (template.Type == Dns.Enums.TemplateTypes.CriteriaGroup) {
                jTemplate = {
                    Header: { Name: null, Description: null, ViewUrl: null, Grammar: null },
                    Where: { Criteria: [<Dns.Interfaces.IQueryComposerCriteriaDTO> json] },
                    Select: { Fields: [<Dns.Interfaces.IQueryComposerFieldDTO> json] }
                };
            } else {
                jTemplate = json;
            }

            //may need to be changed to Plugins.Requests.QueryBuilder.Edit.init(..
            Plugins.Requests.QueryBuilder.MDQ.init(jTemplate, [], null, null, '', [], null, visualTerms, true, template.ID);

            $(() => {
                var bindingControl = $('#Content');
                vm = new ViewModel(bindingControl, screenPermissions || [Permissions.Templates.Delete, Permissions.Templates.Edit, Permissions.Templates.ManageSecurity], template, permissionList || [], templatePermissions || [], securityGroupTree || [], globalPermissions[0]);
                ko.applyBindings(vm, bindingControl[0]);
            });

        });
    }

    init();
} 