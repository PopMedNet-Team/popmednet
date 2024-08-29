import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as ViewModels from '../Lpp.Dns.ViewModels.js';
import * as WebApi from "../Lpp.Dns.WebApi.js";
import { PMNPermissions, UserSettingHelper } from "../_RootLayout.js";
import * as Constants from '../../scripts/page/constants.js';
import * as Enums from '../Dns.Enums.js';
import * as SecurityAcl from '../security/AclViewModel.js';
import * as EventsAcl from "../events/EditEventPermissions.js";
export class ViewModel extends Global.PageViewModel {
    User;
    IsProfile;
    SecurityGroups;
    Name;
    AccountStatus;
    ShowActivation;
    //Security
    Security;
    UserAcls;
    //Events
    Events;
    UserEvents;
    //Subscriptions
    Subscriptions;
    //Lists
    OrganizationList;
    SecurityGroupTree;
    dsSecurityGroupsTree;
    SecurityGroupSelected;
    SubscribableEventsList;
    AssignedNotificationsList;
    dataSource;
    //UI Events
    RemoveSecurityGroup;
    //For temporarily stopping this.Active subscription callback
    StopActiveCallback = false;
    CanChangeLogin;
    CanChangePassword;
    CanManageSecurity;
    CanManageNotifications;
    CanViewUser;
    CanDeleteUser;
    CanEditUser;
    FrequenciesTranslation = Enums.FrequenciesTranslation;
    constructor(screenPermissions, canApproveReject, user, securityGroups, userAcls, userEvents, subscribedEvents, organizationList, securityGroupTree, permissionList, eventList, subscribableEventsList, assignedNotificationsList, bindingControl) {
        super(bindingControl, screenPermissions);
        this.CanChangeLogin = this.HasPermission(PMNPermissions.User.ChangeLogin);
        this.CanChangePassword = this.HasPermission(PMNPermissions.User.ChangePassword);
        this.CanManageSecurity = this.HasPermission(PMNPermissions.User.ManageSecurity);
        this.CanManageNotifications = this.HasPermission(PMNPermissions.User.ManageNotifications);
        this.CanViewUser = this.HasPermission(PMNPermissions.User.View);
        this.CanDeleteUser = this.HasPermission(PMNPermissions.User.Delete);
        this.CanEditUser = this.HasPermission(PMNPermissions.User.Edit);
        let self = this;
        this.IsProfile = Global.User.ID == user.ID;
        this.User = new ViewModels.UserViewModel(user);
        this.SecurityGroups = ko.observableArray(securityGroups.map((sg) => {
            return new ViewModels.SecurityGroupViewModel(sg);
        }));
        // Allow activate visibility for the login user only if s/he has Approve Registration rights
        // for the new user's organization either via group ownership or the new user's organization is null.
        this.ShowActivation = ko.computed(() => {
            return canApproveReject;
        });
        this.User.Active.subscribe((value) => {
            if (this.StopActiveCallback == true) {
                this.StopActiveCallback = false;
                return;
            }
            if (value) {
                if (typeof this.User.ID() === 'undefined' || this.User.ID() == null) {
                    this.StopActiveCallback = true;
                    this.User.Active(false);
                    Global.Helpers.ShowAlert("Validation Error", "<p>Set password before marking user active.</p>");
                    return;
                }
                else {
                    WebApi.Users.HasPassword(this.User.ID()).done((hasPassword) => {
                        if (hasPassword.length > 0 && hasPassword[0] == false) {
                            this.StopActiveCallback = true;
                            this.User.Active(false);
                            Global.Helpers.ShowAlert("Validation Error", "<p>Set password before marking user active.</p>");
                            return;
                        }
                    }).fail(() => {
                        this.StopActiveCallback = true;
                        this.User.Active(false);
                        Global.Helpers.ShowAlert("Validation Error", "<p>Set password before marking user active.</p>");
                        return;
                    });
                    ;
                }
                if (!this.User.ActivatedOn())
                    this.User.ActivatedOn(new Date(Date.now()));
            }
            if (value) {
                this.User.RejectedByID(null);
                this.User.RejectedOn(null);
                this.User.RejectReason(null);
                this.User.RejectedBy(null);
                this.User.DeactivatedByID(null);
                this.User.DeactivatedOn(null);
                this.User.DeactivatedOn(null);
                this.User.DeactivatedBy(null);
            }
            else {
                Global.Helpers.ShowDialog("Deactivate User: " + this.User.FirstName() + " " + this.User.LastName(), "/users/deactivate", ["close"], 600, 400).done((reason) => {
                    if (reason != null) {
                        this.User.DeactivatedBy("Me");
                        this.User.DeactivatedByID(Global.User.ID);
                        this.User.DeactivatedOn(new Date());
                        this.User.DeactivationReason(reason);
                    }
                    else {
                        this.User.Active(true);
                    }
                });
            }
        });
        //Permissions
        this.UserAcls = ko.observableArray(userAcls.map((a) => {
            return new ViewModels.AclUserViewModel(a);
        }));
        this.Security = new SecurityAcl.AclEditViewModel(permissionList, securityGroupTree, this.UserAcls, [{ Field: "UserID", Value: this.User ? this.User.ID() : null }], ViewModels.AclUserViewModel);
        //Events
        this.UserEvents = ko.observableArray(userEvents.map((e) => {
            return new ViewModels.UserEventViewModel(e);
        }));
        this.Events = new EventsAcl.EventAclEditViewModel(eventList, securityGroupTree, this.UserEvents, [{ Field: "UserID", Value: this.User.ID() }], ViewModels.UserEventViewModel);
        //Subscriptions
        this.Subscriptions = ko.observableArray(subscribedEvents.map((se) => {
            return new ViewModels.UserEventSubscriptionViewModel(se);
        }));
        this.SubscribableEventsList = ko.observableArray(subscribableEventsList.map((el) => {
            return new EventViewModel(this, el);
        }));
        this.AssignedNotificationsList = ko.observableArray(assignedNotificationsList.map((el) => {
            return new ViewModels.AssignedUserNotificationViewModel(el);
        }));
        //Lists
        this.OrganizationList = organizationList;
        this.OrganizationList.sort(function (a, b) {
            if (a.Name > b.Name) {
                return 1;
            }
            if (a.Name < b.Name) {
                return -1;
            }
            else {
                return 0;
            }
        });
        this.SecurityGroupTree = securityGroupTree;
        this.dsSecurityGroupsTree = new kendo.data.HierarchicalDataSource({
            data: this.SecurityGroupTree,
            schema: {
                model: {
                    id: "ID",
                    hasChildren: "HasChildren",
                    children: "SubItems"
                }
            }
        });
        this.SecurityGroupSelected = (e) => {
            let tree = $("#tvSecurityGroupSelector").data("kendoTreeView");
            let node = tree.dataItem(e.node);
            if (!node || !node.id) {
                e.preventDefault();
                tree.expand(e.node);
                return;
            }
            let hasGroup = false;
            self.SecurityGroups().forEach((g) => {
                if (g.ID() == node.id) {
                    hasGroup = true;
                    return;
                }
            });
            if (!hasGroup) {
                //Do the add of the group here with an empty Acl
                self.SecurityGroups.push(new ViewModels.SecurityGroupViewModel({
                    ID: node.id,
                    Name: node["Name"],
                    Path: node["Path"],
                    ParentSecurityGroup: node["ParentSecurityGroup"],
                    Kind: node["Kind"],
                    Owner: node["Owner"],
                    OwnerID: node["OwnerID"],
                    ParentSecurityGroupID: node["ParentSecurityGroupID"],
                    Timestamp: node["Timestamp"],
                    Type: 1
                }));
            }
            //Close the drop down.
            $('#btnAddSecurityGroup').dropdown('toggle');
            return false;
        };
        //Account Status Badge
        this.AccountStatus = ko.computed(() => {
            return this.User.Deleted() ? "Deleted" : this.User.RejectedOn() != null ? "Rejected" : this.User.DeactivatedOn() != null ? "Deactivated" : this.User.ActivatedOn() == null && !this.User.Active() ? "Pending" : this.User.Active() ? "Active" : "Locked";
        });
        //Title
        this.Name = ko.observable(user.FirstName + " " + user.LastName);
        this.User.FirstName.subscribe(this.UpdateName);
        this.User.LastName.subscribe(this.UpdateName);
        this.WatchTitle(this.Name, "User: ");
        //Events
        this.RemoveSecurityGroup = (data) => {
            Global.Helpers.ShowConfirm("Removal Confirmation", "<p>Are you sure that you wish to remove " + self.Name() + " from the " + data.Path() + " group?</p>").done(() => {
                self.SecurityGroups.remove(data);
            });
        };
    }
    onActivate(e) {
        if (e != undefined && e.item.id == "tbAuthentication") {
            let authGrid = $('#AuthGrid');
            if (authGrid.children().length > 0) {
                for (let i = 0; i < authGrid.children().length; i++) {
                    let child = authGrid.children()[i];
                    child.remove();
                }
            }
            WebApi.Users.ListDistinctEnvironments(this.User.ID()).done((audits) => {
                let environments = ko.utils.arrayGetDistinctValues(audits.map(item => {
                    if (item.Environment === null || item.Environment.trim() === "") {
                        return {
                            Name: "Other",
                            ID: "Other"
                        };
                    }
                    return {
                        Name: item.Environment,
                        ID: item.Environment.replace(" ", "")
                    };
                }));
                WebApi.Users.GetSetting(environments.map((item) => { return "Users.Details." + item.ID + ":" + Global.User.ID; })).done((settings) => {
                    let mainTable = $('<table class="panel-body table table-stripped table-bordered table-hover"></table>');
                    let tableBody = $('<tbody></tbody>');
                    for (let i = 0; i < environments.length; i++) {
                        let environment = environments[i];
                        let mainRow = $('<tr style="background:#f5f5f5"></tr>');
                        let mainCell = $('<td></td>');
                        let icon = $('<i id="img-' + environment.ID + '" class="k-icon k-i-expand"></i><span>' + environment.Name + '</span>').click(function () {
                            let img = $('#img-' + environment.ID);
                            let child = $('#auth-' + environment.ID);
                            let childGrid = $('#grid-' + environment.ID);
                            if (img.hasClass('k-i-expand')) {
                                img.removeClass('k-i-expand');
                                img.addClass('k-i-collapse');
                                // When the grid is hidden, size and virtual scroll options are not set.  Due to this, we need to show it, then resize and reset the virtual options.
                                child.show();
                                childGrid.data("kendoGrid").resize();
                                childGrid.data("kendoGrid").setOptions({
                                    scrollable: {
                                        virtual: true
                                    }
                                });
                                let setting = ko.utils.arrayFirst(settings, (item) => {
                                    return item.Key === "Users.Details." + environment.ID + ":" + Global.User.ID;
                                });
                                if (setting != null && setting !== undefined && setting.Setting.trim() !== "") {
                                    Global.Helpers.SetGridFromSettings(grid.data("kendoGrid"), setting.Setting);
                                }
                            }
                            else {
                                img.addClass('k-i-expand');
                                img.removeClass('k-i-collapse');
                                child.hide();
                            }
                        });
                        mainCell.append(icon);
                        mainRow.append(mainCell);
                        tableBody.append(mainRow);
                        let row = $('<tr id="auth-' + environment.ID + '" style="display: none;"></tr>');
                        let cell = $('<td></td>');
                        let grid = $('<div id="grid-' + environment.ID + '"></div>');
                        if (environment.ID === "Other") {
                            grid.kendoGrid({
                                dataSource: {
                                    serverPaging: true,
                                    serverSorting: true,
                                    serverGrouping: false,
                                    pageSize: 100,
                                    transport: {
                                        read: {
                                            cache: false,
                                            url: Global.Helpers.GetServiceUrl("/Users/ListAuthenticationAudits?$orderby=DateTime desc&$filter=Environment eq null and UserID eq " + this.User.ID() + ""),
                                            type: "GET",
                                            dataType: "json",
                                            beforeSend: function (request) {
                                                request.setRequestHeader('Authorization', "PopMedNet " + Global.User.AuthToken);
                                            }
                                        },
                                        parameterMap: function (data) {
                                            //need to modify the query string, inlinecount specified with the dollar sign is not accepted in the webapi, replace without dollar sign.
                                            //the map is an actual object representing the query string
                                            let map = kendo.data.transports.odata.parameterMap(data, "read");
                                            map.inlinecount = map["$inlinecount"];
                                            delete map.$inlinecount;
                                            return map;
                                        }
                                    },
                                    schema: {
                                        model: kendo.data.Model.define(Interfaces.KendoModelUserAuthenticationDTO)
                                    },
                                },
                                height: "500px",
                                sortable: true,
                                filterable: Global.Helpers.GetColumnFilterOperatorDefaults(),
                                resizable: true,
                                reorderable: true,
                                scrollable: {
                                    virtual: true
                                },
                                columnMenu: true,
                                pageable: false,
                                columns: [
                                    { field: 'DateTime', title: 'Date', format: Constants.DateTimeFormatter, width: 180 },
                                    { field: 'Description', title: 'Description' },
                                    { field: 'DMCVersion', title: 'DataMart Client Version' },
                                    { field: 'Success', title: 'Success', hidden: true },
                                    { field: 'Source', title: 'Source', hidden: true }
                                ]
                            }).data("kendoGrid");
                        }
                        else {
                            grid.kendoGrid({
                                dataSource: {
                                    serverPaging: true,
                                    serverSorting: true,
                                    serverGrouping: false,
                                    pageSize: 100,
                                    transport: {
                                        read: {
                                            cache: false,
                                            url: Global.Helpers.GetServiceUrl("/Users/ListAuthenticationAudits?$orderby=DateTime desc&$filter=Environment eq '" + environment.Name + "' and UserID eq " + this.User.ID() + ""),
                                            type: "GET",
                                            dataType: "json",
                                            beforeSend: function (request) {
                                                request.setRequestHeader('Authorization', "PopMedNet " + Global.User.AuthToken);
                                            }
                                        },
                                        parameterMap: function (data) {
                                            //need to modify the query string, inlinecount specified with the dollar sign is not accepted in the webapi, replace without dollar sign.
                                            //the map is an actual object representing the query string
                                            let map = kendo.data.transports.odata.parameterMap(data, "read");
                                            map.inlinecount = map["$inlinecount"];
                                            delete map.$inlinecount;
                                            return map;
                                        }
                                    },
                                    schema: {
                                        model: kendo.data.Model.define(Interfaces.KendoModelUserAuthenticationDTO)
                                    },
                                },
                                sortable: true,
                                filterable: Global.Helpers.GetColumnFilterOperatorDefaults(),
                                height: "500px",
                                resizable: true,
                                reorderable: true,
                                scrollable: {
                                    virtual: true
                                },
                                columnMenu: true,
                                pageable: false,
                                columns: [
                                    { field: 'DateTime', title: 'Date', format: Constants.DateTimeFormatter, width: 180 },
                                    { field: 'Description', title: 'Description' },
                                    { field: 'DMCVersion', title: 'DataMart Client Version' },
                                    { field: 'Success', title: 'Success', hidden: true },
                                    { field: 'Source', title: 'Source', hidden: true }
                                ]
                            }).data("kendoGrid");
                        }
                        grid.data("kendoGrid").bind("dataBound", function (e) {
                            UserSettingHelper.SetSetting("Users.Details." + environment.ID + ":" + Global.User.ID, Global.Helpers.GetGridSettings(grid.data("kendoGrid")));
                        });
                        grid.data("kendoGrid").bind("columnShow", function (e) {
                            UserSettingHelper.SetSetting("Users.Details." + environment.ID + ":" + Global.User.ID, Global.Helpers.GetGridSettings(grid.data("kendoGrid")));
                        });
                        grid.data("kendoGrid").bind("columnHide", function (e) {
                            UserSettingHelper.SetSetting("Users.Details." + environment.ID + ":" + Global.User.ID, Global.Helpers.GetGridSettings(grid.data("kendoGrid")));
                        });
                        grid.data("kendoGrid").bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
                        cell.append(grid);
                        row.append(cell);
                        tableBody.append(row);
                    }
                    mainTable.append(tableBody);
                    authGrid.append(mainTable);
                });
            });
        }
    }
    UpdateName(value) {
        this.Name(this.User.FirstName() + " " + this.User.LastName());
    }
    Reject() {
        Global.Helpers.ShowDialog("Reject User Registration: " + this.User.FirstName() + " " + this.User.LastName(), "/users/rejectregistration", ["close"], 600, 400).done((reason) => {
            if (reason) {
                this.User.RejectedBy("Me");
                this.User.RejectedOn(new Date());
                this.User.RejectReason(reason);
                this.User.RejectedByID(Global.User.ID);
            }
        });
    }
    ChangePassword() {
        this.Save(null, null, false).done(() => {
            Global.Helpers.ShowDialog("Change Password", "/users/changepassword?ID=" + this.User.ID(), ["Close"], 500, 400);
        });
    }
    Delete() {
        Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this user?</p>").done(() => {
            if (this.User.ID()) {
                WebApi.Users.Delete([this.User.ID()]).done(() => {
                    window.location.href = '/users';
                });
            }
            else {
                window.location.href = "/users"; //Return if they're new.
            }
        });
    }
    Cancel() {
        window.location.href = "/users";
    }
    Save(data, e, showPrompt = true) {
        let deferred = $.Deferred();
        if (this.User.Fax() == "")
            this.User.Fax(null);
        if (this.User.Phone() == "")
            this.User.Phone(null);
        if (!this.Validate()) {
            deferred.reject();
            return;
        }
        if (this.HasPermission(PMNPermissions.User.ManageSecurity)) {
            if (this.User.Active() && this.User.OrganizationID() == null) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have selected an organization.</p>");
                deferred.reject();
                return;
            }
            // Removed per PMNDEV-3085       
            //if (this.UserAcls().filter((a) => { return a.PermissionID() != null && (<string> a.PermissionID()).toUpperCase() == "2B42D2D7-F7A7-4119-9CC5-22991DC12AD3"; }).length == 0 ||
            //    this.UserAcls().filter((a) => { return a.PermissionID() != null && (<string> a.PermissionID()).toUpperCase() == "268E7007-E95F-435C-8FAF-0B9FBC9CA997"; }).length == 0) {
            //    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have selected at least one security group for this who has permission to edit this user and manage user security.</p>");
            //    return;
            //}
            if (this.User.Active() && this.HasPermission(PMNPermissions.User.ManageSecurity) && this.SecurityGroups().length == 0) {
                Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that this active user belongs to at least one security group.</p>");
                return;
            }
        }
        let user = this.User.toData();
        let self = this;
        //Always test save the primary entity first before anything else.
        WebApi.Users.InsertOrUpdate([user]).done((users) => {
            self.User.ID(users[0].ID);
            self.User.Timestamp(users[0].Timestamp);
            window.history.replaceState(null, window.document.title, "/users/details?ID=" + users[0].ID);
            let userAcls = null, userEvents = null, userSecurityGroups = null;
            if (self.HasPermission(PMNPermissions.User.ManageSecurity)) {
                userAcls = self.UserAcls().map((a) => {
                    a.UserID(self.User.ID());
                    return a.toData();
                });
                userEvents = self.UserEvents().map((e) => {
                    e.UserID(self.User.ID());
                    return e.toData();
                });
                let uSecurityGroups = self.SecurityGroups().map((sd) => {
                    let sg = {
                        ID: sd.ID(),
                        Name: sd.Name(),
                        Path: sd.Path(),
                        Type: sd.Type(),
                        OwnerID: sd.OwnerID(),
                        Owner: sd.Owner(),
                        ParentSecurityGroup: sd.ParentSecurityGroup(),
                        ParentSecurityGroupID: sd.ParentSecurityGroupID(),
                        Kind: sd.Kind()
                    };
                    return sg;
                });
                userSecurityGroups = {
                    UserID: self.User.ID(),
                    Groups: uSecurityGroups
                };
            }
            let convertToInt = (value) => {
                if (value == null || value == undefined || value == "")
                    return null;
                return parseInt(value);
            };
            let userSubscriptions = null;
            if (self.HasPermission(PMNPermissions.User.ManageNotifications)) {
                userSubscriptions = self.Subscriptions().map((s) => {
                    let subscription = {
                        EventID: s.EventID(),
                        Frequency: convertToInt(s.Frequency()),
                        LastRunTime: s.LastRunTime(),
                        NextDueTime: s.NextDueTime(),
                        FrequencyForMy: convertToInt(s.FrequencyForMy()),
                        UserID: self.User.ID()
                    };
                    return subscription;
                });
            }
            $.when(userAcls == null ? null : WebApi.Security.UpdateUserPermissions(userAcls), userEvents == null ? null : WebApi.Events.UpdateUserEventPermissions(userEvents), userSecurityGroups == null ? null : WebApi.Users.UpdateSecurityGroups(userSecurityGroups), userSubscriptions == null ? null : WebApi.Users.UpdateSubscribedEvents(userSubscriptions)).done(() => {
                if (showPrompt) {
                    Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                        deferred.resolve();
                    });
                }
                else {
                    deferred.resolve();
                }
            });
        }).fail(() => {
            deferred.reject();
        });
        return deferred;
    }
    SelectSecurityGroup() {
        Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done((result) => {
            if (!result)
                return;
            this.SecurityGroups.push({
                ID: ko.observable(result.ID),
                Name: ko.observable(result.Name),
                Path: ko.observable(result.Path),
                Type: ko.observable(result.Type),
                OwnerID: ko.observable(result.OwnerID),
                Owner: ko.observable(result.Owner),
                ParentSecurityGroup: ko.observable(result.ParentSecurityGroup),
                ParentSecurityGroupID: ko.observable(result.ParentSecurityGroupID),
                Kind: ko.observable(result.Kind)
            });
        });
    }
    HasEventNotificationDetails(eventID) {
        return this.AssignedNotificationsList().filter(function (item) { return item.EventID() == eventID(); }).length > 0;
    }
    GetEventNotificationDetails(eventID) {
        let filtered = this.AssignedNotificationsList().filter(function (item) { return item.EventID() == eventID(); });
        //    forEach((v) => {
        //        v.Description(v.Level() == "Global" && v.Description() == "" ? "Global" : v.Description());
        //});
        let arrItems = [];
        filtered.forEach((item) => {
            if (item.Level() == "Global" && item.Description() == "")
                item.Description("Global");
            arrItems.push(item.toData());
        });
        let dsResults = new kendo.data.DataSource({
            data: arrItems,
            sort: [{ field: 'Level' }],
            group: [{ field: 'Level' }]
        });
        return dsResults;
    }
    SecurityGroupMenu_Click(data, e) {
        e.stopPropagation();
        return false;
    }
    TranslateSecurityGroupKind(kind) {
        return Global.Helpers.GetEnumString(Enums.SecurityGroupKindsTranslation, kind);
    }
}
function init() {
    let id = Global.GetQueryParam("ID");
    let organizationId = Global.GetQueryParam("OrganizationID");
    let defaultScreenPermissions = [
        PMNPermissions.User.ChangeCertificate,
        PMNPermissions.User.ChangeLogin,
        PMNPermissions.User.ChangePassword,
        PMNPermissions.User.Delete,
        PMNPermissions.User.Edit,
        PMNPermissions.User.ManageNotifications,
        PMNPermissions.User.ManageSecurity,
        PMNPermissions.User.View,
    ];
    $.when(id == null ? null : WebApi.Users.GetPermissions([id], defaultScreenPermissions), id == null ? null : WebApi.Users.Get(id), id == null ? null : WebApi.Security.GetUserPermissions(id), id == null ? null : WebApi.Events.GetUserEventPermissions(id), WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.ApproveRejectRegistrations), WebApi.Organizations.List(), WebApi.Security.GetAvailableSecurityGroupTree(), WebApi.Security.GetPermissionsByLocation([Enums.PermissionAclTypes.Users]), id == Global.User.ID ? WebApi.Events.GetEventsByLocation([Enums.PermissionAclTypes.Users, Enums.PermissionAclTypes.UserProfile]) : WebApi.Events.GetEventsByLocation([Enums.PermissionAclTypes.Users]), id == null ? null : WebApi.Users.GetSubscribableEvents(id, null, null, "Name"), id == null ? null : WebApi.Users.GetAssignedNotifications(id)).done((screenPermissions, user, userAcls, userEvents, canApproveRejectGlobally, organizationList, securityGroupTree, permissionList, eventList, subscribableEventsList, assignedNotificationsList) => {
        if (user == null) {
            // New user
            user = new ViewModels.UserViewModel().toData();
            user.FirstName = "";
            user.MiddleName = "";
            user.LastName = "";
            user.OrganizationID = organizationId;
            //user.SignedUpOn = new Date(Date.now());
            user.Active = false;
            user.Deleted = false;
            screenPermissions = defaultScreenPermissions;
        }
        // If it's the current user (editing own profile), ensure they have view, edit and manage notification rights
        if (user.ID == Global.User.ID) {
            $.merge(screenPermissions, [PMNPermissions.User.View.toLowerCase(), PMNPermissions.User.Edit.toLowerCase(), PMNPermissions.User.ChangeLogin.toLowerCase(), PMNPermissions.User.ManageNotifications.toLowerCase()]);
            screenPermissions = $.unique(screenPermissions);
        }
        let deferred = $.Deferred();
        let subscribedEvents = [];
        let securityGroups = [];
        let canApproveRejectOrgLevel = false; //Set in the deferred below
        deferred.done(() => {
            $(() => {
                let bindingControl = $("#Content");
                let vm = new ViewModel(screenPermissions, canApproveRejectGlobally || canApproveRejectOrgLevel, user, securityGroups || [], userAcls || [], userEvents || [], subscribedEvents || [], organizationList || [], securityGroupTree, permissionList || [], eventList || [], subscribableEventsList || [], assignedNotificationsList || [], bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        });
        $.when(id && screenPermissions.indexOf(PMNPermissions.User.ManageSecurity.toLowerCase()) > -1 ? WebApi.Users.MemberOfSecurityGroups(id) : null, user.ID && (screenPermissions.indexOf(PMNPermissions.User.ManageNotifications.toLowerCase()) > -1 || screenPermissions.indexOf(PMNPermissions.User.ManageNotifications) > -1) ? WebApi.Users.GetSubscribedEvents(user.ID) : null, user.OrganizationID == null ? null : WebApi.Organizations.GetPermissions([user.OrganizationID], [PMNPermissions.Organization.ApproveRejectRegistrations])).done((sg, events, permission) => {
            subscribedEvents = events;
            securityGroups = sg;
            canApproveRejectOrgLevel = (permission == null || permission.length == 0) ? false : permission[0].toUpperCase() == PMNPermissions.Organization.ApproveRejectRegistrations;
            deferred.resolve();
        });
    });
}
init();
export class EventViewModel extends ViewModels.EventViewModel {
    Frequency;
    FrequencyForMy;
    SupportsMyFrequency;
    constructor(vm, event) {
        super(event);
        let subscriptionEvent = new ViewModels.UserEventSubscriptionViewModel({
            EventID: this.ID(),
            Frequency: null,
            FrequencyForMy: null,
            LastRunTime: null,
            NextDueTime: null,
            UserID: vm.User.ID()
        });
        this.SupportsMyFrequency = ko.observable(event.SupportsMyNotifications);
        this.FrequencyForMy = ko.computed({
            read: () => {
                for (let j = 0; j < vm.Subscriptions().length; j++) {
                    let sub = vm.Subscriptions()[j];
                    if (sub.EventID() == this.ID())
                        return sub.FrequencyForMy();
                }
                return null;
            },
            write: (value) => {
                for (let j = 0; j < vm.Subscriptions().length; j++) {
                    let sub = vm.Subscriptions()[j];
                    if (sub.EventID() == this.ID()) {
                        sub.FrequencyForMy(value);
                        return;
                    }
                }
                subscriptionEvent.FrequencyForMy(value);
                vm.Subscriptions.push(subscriptionEvent);
            }
        });
        this.Frequency = ko.computed({
            read: () => {
                for (let j = 0; j < vm.Subscriptions().length; j++) {
                    let sub = vm.Subscriptions()[j];
                    if (sub.EventID() == this.ID())
                        return sub.Frequency();
                }
                return null;
            },
            write: (value) => {
                for (let j = 0; j < vm.Subscriptions().length; j++) {
                    let sub = vm.Subscriptions()[j];
                    if (sub.EventID() == this.ID()) {
                        sub.Frequency(value);
                        return;
                    }
                }
                subscriptionEvent.Frequency(value);
                vm.Subscriptions.push(subscriptionEvent);
            }
        });
    }
}
//# sourceMappingURL=details.js.map