var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../_rootlayout.ts" />
var RequestType;
(function (RequestType) {
    var Details;
    (function (Details) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(requestType, requestTypeModels, requestTypeTerms, bindingControl, screenPermissions, permissionList, requestTypePermissions, securityGroupTree, workflows, templates, termList, template) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                self.Workflows = workflows;
                self.Templates = templates;
                self.RequestType = new Dns.ViewModels.RequestTypeViewModel(requestType);
                self.Template = new Dns.ViewModels.TemplateViewModel(template);
                //set default template
                if (self.Template.ComposerInterface() == null)
                    self.Template.ComposerInterface(Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery);
                self.SelectedModels = ko.observableArray(ko.utils.arrayFilter(requestTypeModels, function (rtm) {
                    var modelID = rtm.DataModelID.toLowerCase();
                    return modelID == '321adaa1-a350-4dd0-93de-5de658a507df' ||
                        modelID == '7c69584a-5602-4fc0-9f3f-a27f329b1113' ||
                        modelID == '85ee982e-f017-4bc4-9acd-ee6ee55d2446' ||
                        modelID == 'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb' ||
                        modelID == '4c8a25dc-6816-4202-88f4-6d17e72a43bc' ||
                        modelID == '1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154'; //Modular Program
                }).map(function (item) {
                    return item.DataModelID.toLowerCase();
                }));
                self.RequestTypeTerms = ko.observableArray(requestTypeTerms.map(function (item) {
                    return new Dns.ViewModels.RequestTypeTermViewModel(item);
                }));
                self.TermList = termList;
                self.AddableTerms = ko.computed(function () {
                    var results = self.TermList.filter(function (t) {
                        var exists = false;
                        self.RequestTypeTerms().forEach(function (rtt) {
                            if (rtt.TermID() == t.ID) {
                                exists = true;
                                return;
                            }
                        });
                        return !exists;
                    });
                    return results.sort(function (left, right) { return left.Name == right.Name ? 0 : (left.Name < right.Name ? -1 : 1); });
                });
                self.RequestTypeAcls = ko.observableArray(requestTypePermissions.map(function (item) {
                    return new Dns.ViewModels.AclRequestTypeViewModel(item);
                }));
                self.RequestTypeSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, self.RequestTypeAcls, [
                    {
                        Field: "RequestTypeID",
                        Value: self.RequestType.ID()
                    }
                ], Dns.ViewModels.AclRequestTypeViewModel);
                self.Template.QueryType.subscribe(function (value) {
                    if (self.Template.Type() == Dns.Enums.TemplateTypes.Request) {
                        Plugins.Requests.QueryBuilder.MDQ.vm.Request.Header.QueryType(value);
                        Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(self.SelectedModels(), value, self.RequestTypeTerms().map(function (t) { return t.TermID(); }));
                    }
                });
                self.SelectedModels.subscribe(function (values) {
                    if (self.Template.Type() == Dns.Enums.TemplateTypes.Request) {
                        Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(values, self.Template.QueryType(), self.RequestTypeTerms().map(function (t) { return t.TermID(); }));
                    }
                });
                self.RequestTypeTerms.subscribe(function (values) {
                    if (self.Template.Type() == Dns.Enums.TemplateTypes.Request) {
                        Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(self.SelectedModels(), self.Template.QueryType(), values.map(function (t) { return t.TermID(); }));
                    }
                });
                self.WatchTitle(_this.RequestType.Name, "Request Type: ");
                self.Save = function () {
                    if (self.RequestType.WorkflowID() == null || self.RequestType.WorkflowID() == "") {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have a Workflow Selected.</p>");
                        return;
                    }
                    if (!_super.prototype.Validate.call(_this))
                        return;
                    if (self.RequestTypeAcls().length == 0) {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this request type.</p>");
                        return;
                    }
                    self.Template.Data(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
                    var update = new Dns.ViewModels.UpdateRequestTypeRequestViewModel().toData();
                    update.RequestType = self.RequestType.toData();
                    update.Template = self.Template.toData();
                    update.Models = self.SelectedModels();
                    update.Terms = self.RequestTypeTerms().map(function (t) { return t.TermID(); });
                    update.NotAllowedTerms = Plugins.Requests.QueryBuilder.MDQ.vm.NotAllowedTerms();
                    Dns.WebApi.RequestTypes.Save(update).done(function (results) {
                        var result = results[0];
                        self.RequestType.ID(result.RequestType.ID);
                        self.RequestType.Timestamp(result.RequestType.Timestamp);
                        self.RequestType.TemplateID(result.Template.ID);
                        window.history.replaceState(null, window.document.title, "/requesttype/details?ID=" + self.RequestType.ID());
                        self.Template.ID(result.Template.ID);
                        self.Template.Timestamp(result.Template.Timestamp);
                        self.Template.CreatedByID(result.Template.CreatedByID);
                        var requestTypeAcls = self.RequestTypeAcls().map(function (a) {
                            a.RequestTypeID(self.RequestType.ID());
                            return a.toData();
                        });
                        Dns.WebApi.Security.UpdateRequestTypePermissions(requestTypeAcls).done(function () {
                            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                        });
                    });
                };
                self.Delete = function () {
                    Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Request Type?</p>").done(function () {
                        Dns.WebApi.RequestTypes.Delete([self.RequestType.ID()]).done(function () {
                            window.location.href = "/requesttype";
                        });
                    });
                };
                self.DeleteTerm = function (requestTypeTerm) {
                    self.RequestTypeTerms.remove(requestTypeTerm);
                };
                self.AddRequestTypeTerm = function (term) {
                    self.RequestTypeTerms.push(new Dns.ViewModels.RequestTypeTermViewModel({
                        Description: term.Description,
                        OID: term.OID,
                        ReferenceUrl: term.ReferenceUrl,
                        RequestTypeID: self.RequestType.ID(),
                        Term: term.Name,
                        TermID: term.ID
                    }));
                };
                return _this;
            }
            ViewModel.prototype.Cancel = function () {
                window.location.href = "/requesttype";
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
            $.when(id == null ? null : Dns.WebApi.RequestTypes.Get(id), id == null ? [] : Dns.WebApi.RequestTypes.GetRequestTypeModels(id), id == null ? null : Dns.WebApi.RequestTypes.GetRequestTypeTerms(id), id == null ? null : Dns.WebApi.RequestTypes.GetPermissions([id], [Permissions.RequestTypes.Delete, Permissions.RequestTypes.Edit, Permissions.RequestTypes.ManageSecurity]), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.RequestTypes]), Dns.WebApi.Security.GetRequestTypePermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Dns.WebApi.Workflow.List(null, "ID,Name", "Name"), Dns.WebApi.Templates.List("Type eq Lpp.Dns.DTO.Enums.TemplateTypes'" + Dns.Enums.TemplateTypes.Request + "'", "ID,Name", "Name"), Dns.WebApi.Terms.List(), GetVisualTerms()).done(function (requestTypes, requestTypeModels, requestTypeTerms, screenPermissions, permissionList, requestTypePermissions, securityGroupTree, workflows, templates, termList, visualTerms) {
                var requestType = (requestTypes == null || requestTypes.length == 0) ? new Dns.ViewModels.RequestTypeViewModel().toData() : requestTypes[0];
                $.when(requestType.TemplateID == null ? null : Dns.WebApi.Templates.Get(requestType.TemplateID)).done(function (templates) {
                    var template;
                    if (templates == null || templates.length == 0) {
                        template = new Dns.ViewModels.TemplateViewModel().toData();
                        template.Type = Dns.Enums.TemplateTypes.Request;
                        template.ComposerInterface = Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery;
                        template.QueryType = null;
                    }
                    else {
                        template = templates[0];
                    }
                    var json = JSON.parse(((template.Data || '').trim() != '') ? template.Data : '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[],"ObservationPeriod":{}}]}}');
                    $(function () {
                        var bindingControl = $('#Content');
                        Details.vm = new ViewModel(requestType, requestTypeModels || [], requestTypeTerms || [], bindingControl, screenPermissions || [Permissions.RequestTypes.Delete,
                            Permissions.RequestTypes.Edit,
                            Permissions.RequestTypes.ManageSecurity], permissionList || [], requestTypePermissions || [], securityGroupTree || [], workflows, templates, termList, template);
                        Plugins.Requests.QueryBuilder.Edit.init(json, [], null, null, '', [], null, visualTerms, true, null, requestType.TemplateID, null).done(function () {
                            Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(Details.vm.SelectedModels(), Details.vm.Template.QueryType(), Details.vm.RequestTypeTerms().map(function (t) { return t.TermID(); }));
                        });
                        $('#tabs').kendoTabStrip().data('kendoTabStrip').bind('show', function (e) {
                            if ($(e.contentElement).has('#txtNotes')) {
                                //to make the kendo editor initialize correctly it needs to be refreshed when the tab is show
                                var editor = $('#txtNotes').data('kendoEditor');
                                editor.refresh();
                            }
                        });
                        ko.applyBindings(Details.vm, bindingControl[0]);
                    });
                });
            });
        }
        init();
    })(Details = RequestType.Details || (RequestType.Details = {}));
})(RequestType || (RequestType = {}));
//# sourceMappingURL=Details.js.map