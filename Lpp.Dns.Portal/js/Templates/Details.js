/// <reference path="../_rootlayout.ts" />
///// <reference path="../../areas/querycomposer/js/edit.ts" />
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
var Templates;
(function (Templates) {
    var Details;
    (function (Details) {
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl, screenPermissions, templateData, permissionList, templatePermissions, securityGroupTree, globalPermission, visualTerms, criteriaGroupTemplates) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.GlobalPermissionCheck = (globalPermission == undefined || null ? { CurrentUserHasGlobalPermission: false, SecurityGroupExistsForGlobalPermission: false } : globalPermission);
                _this.Template = new Dns.ViewModels.TemplateViewModel(templateData);
                _this.TemplateAcls = ko.observableArray(templatePermissions.map(function (item) {
                    return new Dns.ViewModels.AclTemplateViewModel(item);
                }));
                _this.TemplateSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, _this.TemplateAcls, [
                    {
                        Field: "TemplateID",
                        Value: _this.Template.ID()
                    }
                ], Dns.ViewModels.AclTemplateViewModel);
                _this.WatchTitle(_this.Template.Name, "Query Composer Template: ");
                var termsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();
                _this.QueryDesigner = new Plugins.Requests.QueryBuilder.QueryEditorHost({
                    IsTemplateEdit: true,
                    Templates: [_this.Template],
                    RequestTypeModelIDs: null,
                    RequestTypeTerms: null,
                    TemplateType: Dns.Enums.TemplateTypes.CriteriaGroup,
                    VisualTerms: visualTerms,
                    CriteriaGroupTemplates: criteriaGroupTemplates,
                    HiddenTerms: [],
                    SupportsMultiQuery: ko.observable(false),
                    TermsObserver: termsObserver
                });
                _this.CanSave = ko.observable(_this.Template.CreatedByID() == User.ID || _this.HasPermission(PMNPermissions.Templates.Edit));
                return _this;
            }
            ViewModel.prototype.Save = function () {
                if (!_super.prototype.Validate.call(this))
                    return;
                if (this.Template.Name().length == 0) {
                    Global.Helpers.ShowAlert("Required Field", "<p><i>Template Name</i> is a required field and may not be left blank.</p>");
                    return;
                }
                var deferred = null;
                if (this.TemplateAcls().length == 0) {
                    if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == false) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this template.</p>");
                        return;
                    }
                    else {
                        if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                            deferred = Global.Helpers.ShowConfirm("Validation Error", "<p>You do not have permission to edit this template, you should add at least one security group to the Permissions tab if applicable. <br>Would you like to save anyway?</p>");
                            return;
                        }
                        else {
                            if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == true)) {
                            }
                        }
                    }
                }
                if (deferred == null) {
                    deferred = $.Deferred();
                    deferred.resolve();
                }
                var self = this;
                deferred.done(function () {
                    self.Template.Data(JSON.stringify(self.QueryDesigner.Queries()[0].Where.Criteria()[0].toData()));
                    var data = self.Template.toData();
                    var templateID = self.Template.ID();
                    var templateAcls = self.TemplateAcls().map(function (a) {
                        a.TemplateID(templateID);
                        return a.toData();
                    });
                    Dns.WebApi.Security.UpdateTemplatePermissions(templateAcls)
                        .then(function () {
                        return Dns.WebApi.Templates.Update([data]);
                    })
                        .done(function (results) {
                        self.Template.Timestamp(results[0].Timestamp);
                        window.history.replaceState(null, window.document.title, "/templates/details?ID=" + results[0].ID);
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.location.href = "/templates";
            };
            ViewModel.prototype.Delete = function () {
                var templateID = this.Template.ID();
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Template?</p>").done(function () {
                    Dns.WebApi.Templates.Delete([templateID]).done(function () {
                        window.location.href = "/templates";
                    });
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            $.when(id == null ? null : Dns.WebApi.Templates.Get(id), id == null ? null : Dns.WebApi.Templates.GetPermissions([id], [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity]), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Templates]), Dns.WebApi.Security.GetTemplatePermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Templates.GetGlobalTemplatePermissions(), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(), Dns.WebApi.Templates.CriteriaGroups()).done(function (templates, screenPermissions, permissionList, templatePermissions, globalPermissions, securityGroupTree, visualTerms, criteriaGroupTemplates) {
                var template = templates[0];
                $(function () {
                    var bindingControl = $('#Content');
                    Details.vm = new ViewModel(bindingControl, screenPermissions || [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity], template, permissionList || [], templatePermissions || [], securityGroupTree || [], globalPermissions[0], visualTerms, criteriaGroupTemplates);
                    ko.applyBindings(Details.vm, bindingControl[0]);
                    Details.vm.QueryDesigner.onKnockoutBind();
                });
            });
        }
        init();
    })(Details = Templates.Details || (Templates.Details = {}));
})(Templates || (Templates = {}));
