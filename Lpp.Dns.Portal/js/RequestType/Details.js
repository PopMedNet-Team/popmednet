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
var RequestType;
(function (RequestType) {
    var Details;
    (function (Details) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(requestType, requestTypeModels, requestTypeTerms, bindingControl, screenPermissions, permissionList, requestTypePermissions, securityGroupTree, workflows, templates, termList, visualTerms, criteriaGroupTemplates, hiddenTerms) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                self.Workflows = workflows;
                self.RequestType = new Dns.ViewModels.RequestTypeViewModel(requestType);
                self.RequestTypeID = requestType.ID;
                self.RequestTypeTerms = ko.observableArray(requestTypeTerms.map(function (item) {
                    return new Dns.ViewModels.RequestTypeTermViewModel(item);
                }));
                self.TermList = termList;
                self.SelectedModels = ko.observableArray(ko.utils.arrayFilter(requestTypeModels, function (rtm) {
                    var modelID = rtm.DataModelID.toLowerCase();
                    return modelID == '321adaa1-a350-4dd0-93de-5de658a507df' ||
                        modelID == '7c69584a-5602-4fc0-9f3f-a27f329b1113' ||
                        modelID == '85ee982e-f017-4bc4-9acd-ee6ee55d2446' ||
                        modelID == 'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb' ||
                        modelID == '4c8a25dc-6816-4202-88f4-6d17e72a43bc' ||
                        modelID == '1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154';
                }).map(function (item) {
                    return item.DataModelID.toLowerCase();
                })).extend({ rateLimit: 900, method: 'notifyWhenChangesStop' });
                var termsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();
                _this.QueryDesigner = new Plugins.Requests.QueryBuilder.QueryEditorHost({
                    Templates: ko.utils.arrayMap(templates, function (t) { return new Dns.ViewModels.TemplateViewModel(t); }),
                    IsTemplateEdit: true,
                    TemplateType: Dns.Enums.TemplateTypes.Request,
                    RequestTypeTerms: self.RequestTypeTerms,
                    RequestTypeModelIDs: self.SelectedModels,
                    VisualTerms: visualTerms,
                    CriteriaGroupTemplates: criteriaGroupTemplates,
                    HiddenTerms: hiddenTerms,
                    SupportsMultiQuery: ko.pureComputed(function () { return self.RequestType.SupportMultiQuery(); }),
                    TermsObserver: termsObserver
                });
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
                        Value: self.RequestTypeID
                    }
                ], Dns.ViewModels.AclRequestTypeViewModel);
                self.WatchTitle(_this.RequestType.Name, "Request Type: ");
                self.Save = function () {
                    if (self.RequestType.WorkflowID() == null || self.RequestType.WorkflowID() == "") {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have a Workflow selected.</p>");
                        return;
                    }
                    if (_this.QueryDesigner.onValidateEditors() == false)
                        return;
                    if (!_super.prototype.Validate.call(_this))
                        return;
                    var requestTypeAcls = self.RequestTypeAcls();
                    if (requestTypeAcls.length == 0) {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this request type.</p>");
                        return;
                    }
                    var update = new Dns.ViewModels.UpdateRequestTypeRequestViewModel().toData();
                    update.RequestType = self.RequestType.toData();
                    update.Queries = self.QueryDesigner.ExportTemplates();
                    update.Models = self.SelectedModels();
                    update.Terms = self.RequestTypeTerms().map(function (t) { return t.TermID(); });
                    update.NotAllowedTerms = self.QueryDesigner.ExportHiddenTerms();
                    update.Permissions = requestTypeAcls.map(function (a) {
                        a.RequestTypeID(self.RequestTypeID);
                        return a.toData();
                    });
                    Dns.WebApi.RequestTypes.Save(update).done(function (results) {
                        var result = results[0];
                        self.RequestType.ID(result.RequestType.ID);
                        self.RequestTypeID = result.RequestType.ID;
                        self.RequestType.Timestamp(result.RequestType.Timestamp);
                        window.history.replaceState(null, window.document.title, "/requesttype/details?ID=" + self.RequestTypeID);
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
                };
                self.Delete = function () {
                    Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Request Type?</p>").done(function () {
                        Dns.WebApi.RequestTypes.Delete([self.RequestTypeID]).done(function () {
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
                        RequestTypeID: self.RequestTypeID,
                        Term: term.Name,
                        TermID: term.ID
                    }));
                };
                return _this;
            }
            ViewModel.prototype.onConfirmChangeToSingleQuery = function (data, evt) {
                var self = this;
                if (data.RequestType.SupportMultiQuery() && data.QueryDesigner.Queries().length > 1) {
                    Global.Helpers.ShowConfirm('Please Confirm', '<p class="alert alert-warning">Changing to single Query will remove all Cohorts except for the first. Proceed?</p>')
                        .done(function () {
                        self.RequestType.SupportMultiQuery(false);
                    });
                    evt.stopImmediatePropagation();
                    return false;
                }
                return true;
            };
            ViewModel.prototype.Cancel = function () {
                window.location.href = "/requesttype";
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            $.when(id == null ? null : Dns.WebApi.RequestTypes.Get(id), id == null ? null : Dns.WebApi.Templates.GetByRequestType(id), id == null ? [] : Dns.WebApi.RequestTypes.GetRequestTypeModels(id), id == null ? null : Dns.WebApi.RequestTypes.GetRequestTypeTerms(id), id == null ? null : Dns.WebApi.RequestTypes.GetPermissions([id], [PMNPermissions.RequestTypes.Delete, PMNPermissions.RequestTypes.Edit, PMNPermissions.RequestTypes.ManageSecurity]), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.RequestTypes]), Dns.WebApi.Security.GetRequestTypePermissions(id ? id : Constants.GuidEmpty), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Dns.WebApi.Workflow.List(null, "ID,Name", "Name"), Dns.WebApi.Terms.List(), Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(), Dns.WebApi.Templates.CriteriaGroups(), Dns.WebApi.Templates.ListHiddenTermsByRequestType(id ? id : Constants.GuidEmpty)).done(function (requestTypes, templates, requestTypeModels, requestTypeTerms, screenPermissions, permissionList, requestTypePermissions, securityGroupTree, workflows, termList, visualTerms, criteriaGroupTemplates, hiddenTerms) {
                var requestType = (requestTypes == null || requestTypes.length == 0) ? new Dns.ViewModels.RequestTypeViewModel().toData() : requestTypes[0];
                if (templates == null || templates.length == 0) {
                    var template = new Dns.ViewModels.TemplateViewModel().toData();
                    template.Name = 'Cohort 1';
                    template.Type = Dns.Enums.TemplateTypes.Request;
                    template.ComposerInterface = Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery;
                    template.QueryType = null;
                    templates = [template];
                }
                $(function () {
                    var bindingControl = $('#Content');
                    Details.vm = new ViewModel(requestType, requestTypeModels || [], requestTypeTerms || [], bindingControl, screenPermissions || [PMNPermissions.RequestTypes.Delete, PMNPermissions.RequestTypes.Edit, PMNPermissions.RequestTypes.ManageSecurity], permissionList || [], requestTypePermissions || [], securityGroupTree || [], workflows, templates, termList, visualTerms, criteriaGroupTemplates, hiddenTerms);
                    $('#tabs').kendoTabStrip().data('kendoTabStrip').bind('show', function (e) {
                        if ($(e.contentElement).has('#txtNotes')) {
                            var editor = $('#txtNotes').data('kendoEditor');
                            editor.refresh();
                        }
                    });
                    ko.applyBindings(Details.vm, bindingControl[0]);
                    Details.vm.QueryDesigner.onKnockoutBind();
                    $('#PageLoadingMessage').remove();
                });
            });
        }
        init();
    })(Details = RequestType.Details || (RequestType.Details = {}));
})(RequestType || (RequestType = {}));
