/// <reference path="../../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../../js/requests/details.ts" />
module Workflow.Common.AddRequestUserDialog {
    var vm: ViewModel;
    export class ViewModel extends Global.DialogViewModel {

        private Request: Dns.ViewModels.RequestViewModel;
        private WorkflowRoles: Dns.Interfaces.IWorkflowRoleDTO[];

        private SelectedWorkflowRoleID: KnockoutObservable<any>;
        private SelectedUser: KnockoutObservable<Dns.Interfaces.IUserDTO>;

        private onSave: () => void;
        private onCancel: () => void;

        private NotWorking: KnockoutObservable<boolean>;

        private dsOrganizations: any[];
        private FullUsers: Dns.Interfaces.IUserDTO[];
        private FilteredUsers: KnockoutObservableArray<Dns.Interfaces.IUserDTO>;

        private onSelectOrganization: (arg: kendo.ui.GridChangeEvent) => void;
        private onSelectUser: (arg: kendo.ui.GridChangeEvent) => void;

        constructor(bindingControl: JQuery, request: Dns.ViewModels.RequestViewModel, workflowRoles: Dns.Interfaces.IWorkflowRoleDTO[], users: Dns.Interfaces.IUserDTO[]) {
            super(bindingControl);
            var self = this;

            self.FullUsers = users;
            self.FilteredUsers = ko.observableArray([]);
            self.NotWorking = ko.observable(true);
            self.Request = request;
            self.WorkflowRoles = workflowRoles;
            self.SelectedWorkflowRoleID = ko.observable(workflowRoles[0].ID);
            self.SelectedUser = ko.observable(null);            

            self.dsOrganizations = $.Enumerable.From(ko.utils.arrayMap(users, u => <any>{ ID: u.OrganizationID, Name: u.Organization })).Distinct(i => i.ID).ToArray();

            self.onSelectOrganization = (arg: kendo.ui.GridChangeEvent) => {
                var selectedRow = arg.sender.select()[0];
                var dataItem = <any>arg.sender.dataItem(selectedRow);

                self.SelectedUser(null);

                var orgUsers = ko.utils.arrayFilter(self.FullUsers, u => u.OrganizationID.toLowerCase() == dataItem.ID.toLowerCase());     
                $("#grdUsers").data("kendoGrid").dataSource.data(orgUsers);
                $("#grdUsers").data("kendoGrid").refresh();
            };

            self.onSelectUser = (arg: kendo.ui.GridChangeEvent) => {
                var selectedRow = arg.sender.select()[0];
                var dataItem = <any>arg.sender.dataItem(selectedRow);

                self.SelectedUser(<Dns.Interfaces.IUserDTO>dataItem);
            };

            self.onSave = () => {
                if (self.SelectedUser() == null || self.SelectedWorkflowRoleID() == null)
                    return;

                self.NotWorking(false);
                Global.Helpers.ShowExecuting();
                
                var selectedWorkflowRole = ko.utils.arrayFirst(self.WorkflowRoles, item => item.ID == self.SelectedWorkflowRoleID());
                var selectedUser: Dns.Interfaces.IUserDTO = self.SelectedUser();
                var requestUser: Dns.Interfaces.IRequestUserDTO = {
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
                    .done(result => {
                        self.Close(result);
                    }).always(() => {
                        self.NotWorking(true);
                        Global.Helpers.HideExecuting();
                    });
            };
            self.onCancel = () => {
                self.Close(null);
            };
        }
    }

    export function init() {
        var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
        var request = <Dns.ViewModels.RequestViewModel>(<any>(window.options)).parameters.Request || null;

        $.when<any>(
            Dns.WebApi.Workflow.GetWorkflowRolesByWorkflowID(request.WorkflowID(), 'IsRequestCreator eq false'),
            Dns.WebApi.Users.List('Active eq true and Deleted eq false and not (OrganizationID eq null)', null, 'Organization')
            ).done((workflowRoles, users) => {
                var bindingControl = $('#NewRequestUserDialog');
                vm = new ViewModel(bindingControl, request, workflowRoles, users);
                $(() => {
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
    }

    init();
}