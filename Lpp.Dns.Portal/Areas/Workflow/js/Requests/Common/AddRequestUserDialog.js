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
/// <reference path="../../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
var Workflow;
(function (Workflow) {
    var Common;
    (function (Common) {
        var AddRequestUserDialog;
        (function (AddRequestUserDialog) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl, request, workflowRoles, users) {
                    var _this = _super.call(this, bindingControl) || this;
                    var self = _this;
                    self.FullUsers = users;
                    self.FilteredUsers = ko.observableArray([]);
                    self.NotWorking = ko.observable(true);
                    self.Request = request;
                    self.WorkflowRoles = workflowRoles;
                    self.SelectedWorkflowRoleID = ko.observable(workflowRoles[0].ID);
                    self.SelectedUser = ko.observable(null);
                    self.dsOrganizations = $.Enumerable.From(ko.utils.arrayMap(users, function (u) { return ({ ID: u.OrganizationID, Name: u.Organization }); })).Distinct(function (i) { return i.ID; }).ToArray();
                    self.onSelectOrganization = function (arg) {
                        var selectedRow = arg.sender.select()[0];
                        var dataItem = arg.sender.dataItem(selectedRow);
                        self.SelectedUser(null);
                        var orgUsers = ko.utils.arrayFilter(self.FullUsers, function (u) { return u.OrganizationID.toLowerCase() == dataItem.ID.toLowerCase(); });
                        self.FilteredUsers(orgUsers);
                    };
                    self.onSelectUser = function (arg) {
                        var selectedRow = arg.sender.select()[0];
                        var dataItem = arg.sender.dataItem(selectedRow);
                        self.SelectedUser(dataItem);
                    };
                    self.onSave = function () {
                        if (self.SelectedUser() == null || self.SelectedWorkflowRoleID() == null)
                            return;
                        self.NotWorking(false);
                        Global.Helpers.ShowExecuting();
                        var selectedWorkflowRole = ko.utils.arrayFirst(self.WorkflowRoles, function (item) { return item.ID == self.SelectedWorkflowRoleID(); });
                        var selectedUser = self.SelectedUser();
                        var requestUser = {
                            RequestID: self.Request.ID(),
                            UserID: selectedUser.ID,
                            Username: selectedUser.UserName,
                            Email: selectedUser.Email,
                            FullName: (((selectedUser.FirstName || '') + ' ' + (selectedUser.MiddleName || '')).trim() + ' ' + selectedUser.LastName).trim(),
                            WorkflowRoleID: selectedWorkflowRole.ID,
                            IsRequestCreatorRole: selectedWorkflowRole.IsRequestCreator,
                            WorkflowRole: selectedWorkflowRole.Name
                        };
                        Dns.WebApi.RequestUsers.Insert([requestUser])
                            .done(function (result) {
                            self.Close(result);
                        }).always(function () {
                            self.NotWorking(true);
                            Global.Helpers.HideExecuting();
                        });
                    };
                    self.onCancel = function () {
                        self.Close(null);
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.DialogViewModel));
            AddRequestUserDialog.ViewModel = ViewModel;
            function init() {
                var window = Global.Helpers.GetDialogWindow();
                var request = (window.options).parameters.Request || null;
                $.when(Dns.WebApi.Workflow.GetWorkflowRolesByWorkflowID(request.WorkflowID(), 'IsRequestCreator eq false'), Dns.WebApi.Users.List('Active eq true and Deleted eq false and not (OrganizationID eq null)', null, 'Organization')).done(function (workflowRoles, users) {
                    var bindingControl = $('#NewRequestUserDialog');
                    vm = new ViewModel(bindingControl, request, workflowRoles, users);
                    $(function () {
                        ko.applyBindings(vm, bindingControl[0]);
                    });
                });
            }
            AddRequestUserDialog.init = init;
            init();
        })(AddRequestUserDialog = Common.AddRequestUserDialog || (Common.AddRequestUserDialog = {}));
    })(Common = Workflow.Common || (Workflow.Common = {}));
})(Workflow || (Workflow = {}));
//# sourceMappingURL=AddRequestUserDialog.js.map