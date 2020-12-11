/// <reference path="../_rootlayout.ts" />
/// <reference path="../../areas/querycomposer/js/edit.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
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
            function ViewModel(bindingControl, screenPermissions, templateData, permissionList, templatePermissions, securityGroupTree, globalPermission) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.GlobalPermissionCheck = (globalPermission == undefined || null ? { CurrentUserHasGlobalPermission: false, SecurityGroupExistsForGlobalPermission: false } : globalPermission);
                _this.Template = new Dns.ViewModels.TemplateViewModel(templateData);
                Global.Helpers.GetEnumString(Dns.Enums.TemplateTypesTranslation, _this.Template.Type());
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
                _this.Template.QueryType.subscribe(function (value) {
                    Plugins.Requests.QueryBuilder.MDQ.vm.Request.Header.QueryType(value);
                    Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList([], value, []);
                });
                return _this;
            }
            ViewModel.prototype.Save = function () {
                var _this = this;
                if (!_super.prototype.Validate.call(this))
                    return;
                if (this.Template.Name().length == 0) {
                    Global.Helpers.ShowAlert("Required Field", "<p><i>Template Name</i> is a required field and may not be left blank.</p>");
                    return;
                }
                if (this.TemplateAcls().length == 0) {
                    if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == false) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this template.</p>");
                        return;
                    }
                    else {
                        if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == false)) {
                            Global.Helpers.ShowConfirm("Validation Error", "<p>You do not have permission to edit this template, you should add at least one security group to the Permissions tab if applicable. <br>Would you like to save anyway?</p>").done(function () { _this.VerifiedSave(); });
                            return;
                        }
                        else {
                            if ((this.GlobalPermissionCheck.SecurityGroupExistsForGlobalPermission == true) && (this.GlobalPermissionCheck.CurrentUserHasGlobalPermission == true)) {
                            }
                        }
                    }
                }
                this.VerifiedSave();
            };
            ViewModel.prototype.VerifiedSave = function () {
                var _this = this;
                if (!_super.prototype.Validate.call(this))
                    return;
                if (this.Template.Type() == Dns.Enums.TemplateTypes.CriteriaGroup) {
                    this.Template.Data(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.Where.Criteria()[0].toData()));
                }
                else {
                    this.Template.Data(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
                }
                var data = this.Template.toData();
                Dns.WebApi.Templates.InsertOrUpdate([data]).done(function (results) {
                    var template = results[0];
                    _this.Template.ID(template.ID);
                    _this.Template.Timestamp(template.Timestamp);
                    window.history.replaceState(null, window.document.title, "/templates/details?ID=" + template.ID);
                    var templateAcls = _this.TemplateAcls().map(function (a) {
                        a.TemplateID(_this.Template.ID());
                        return a.toData();
                    });
                    Dns.WebApi.Security.UpdateTemplatePermissions(templateAcls).done(function () {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.location.href = "/templates";
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Template?</p>").done(function () {
                    Dns.WebApi.Templates.Delete([Details.vm.Template.ID()]).done(function () {
                        window.location.href = "/templates";
                    });
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function GetVisualTerms() {
            var d = $.Deferred();
            $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
                .done(function (result) {
                d.resolve(result);
            }).fail(function (e, description, error) {
                d.reject(e);
            });
            return d;
        }
        function init() {
            var id = $.url().param("ID");
            $.when(id == null ? null : Dns.WebApi.Templates.Get(id), id == null ? null : Dns.WebApi.Templates.GetPermissions([id], [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity]), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Templates]), Dns.WebApi.Security.GetTemplatePermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Templates.GetGlobalTemplatePermissions(), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), GetVisualTerms()).done(function (templates, screenPermissions, permissionList, templatePermissions, globalPermissions, securityGroupTree, visualTerms) {
                var template = templates == null ? {
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
                var json = JSON.parse(((template.Data || '').trim() != '') ? template.Data : '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[],"ObservationPeriod":{}}]}}');
                var jTemplate;
                if (template.Type == Dns.Enums.TemplateTypes.CriteriaGroup) {
                    jTemplate = {
                        Header: { Name: null, Description: null, ViewUrl: null, Grammar: null, SubmittedOn: null },
                        Where: { Criteria: [json] },
                        Select: { Fields: [json] },
                        TemporalEvents: []
                    };
                }
                else {
                    jTemplate = json;
                }
                //may need to be changed to Plugins.Requests.QueryBuilder.Edit.init(..
                Plugins.Requests.QueryBuilder.MDQ.init(jTemplate, [], null, null, '', [], null, visualTerms, true, template.ID);
                $(function () {
                    var bindingControl = $('#Content');
                    Details.vm = new ViewModel(bindingControl, screenPermissions || [PMNPermissions.Templates.Delete, PMNPermissions.Templates.Edit, PMNPermissions.Templates.ManageSecurity], template, permissionList || [], templatePermissions || [], securityGroupTree || [], globalPermissions[0]);
                    ko.applyBindings(Details.vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Details = Templates.Details || (Templates.Details = {}));
})(Templates || (Templates = {}));
