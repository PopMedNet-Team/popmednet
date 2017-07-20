var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Users;
(function (Users) {
    var Details;
    (function (Details) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(screenPermissions, canApproveReject, user, securityGroups, userAcls, userEvents, subscribedEvents, organizationList, securityGroupTree, permissionList, eventList, subscribableEventsList, assignedNotificationsList, bindingControl) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.StopActiveCallback = false;
                _this.self = _this;
                _this.IsProfile = User.ID == user.ID;
                _this.User = new Dns.ViewModels.UserViewModel(user);
                _this.SecurityGroups = ko.observableArray(securityGroups.map(function (sg) {
                    return new Dns.ViewModels.SecurityGroupViewModel(sg);
                }));
                _this.ShowActivation = ko.computed(function () {
                    return canApproveReject;
                });
                _this.User.Active.subscribe(function (value) {
                    if (_this.StopActiveCallback == true) {
                        _this.StopActiveCallback = false;
                        return;
                    }
                    if (value) {
                        if (typeof _this.User.ID() === 'undefined' || _this.User.ID() == null) {
                            _this.StopActiveCallback = true;
                            _this.User.Active(false);
                            Global.Helpers.ShowAlert("Validation Error", "<p>Set password before marking user active.</p>");
                            return;
                        }
                        else {
                            Dns.WebApi.Users.HasPassword(_this.User.ID()).done(function (hasPassword) {
                                if (hasPassword.length > 0 && hasPassword[0] == false) {
                                    _this.StopActiveCallback = true;
                                    _this.User.Active(false);
                                    Global.Helpers.ShowAlert("Validation Error", "<p>Set password before marking user active.</p>");
                                    return;
                                }
                            }).fail(function () {
                                _this.StopActiveCallback = true;
                                _this.User.Active(false);
                                Global.Helpers.ShowAlert("Validation Error", "<p>Set password before marking user active.</p>");
                                return;
                            });
                            ;
                        }
                        if (!_this.User.ActivatedOn())
                            _this.User.ActivatedOn(new Date(Date.now()));
                    }
                    if (value) {
                        _this.User.RejectedByID(null);
                        _this.User.RejectedOn(null);
                        _this.User.RejectReason(null);
                        _this.User.RejectedBy(null);
                        _this.User.DeactivatedByID(null);
                        _this.User.DeactivatedOn(null);
                        _this.User.DeactivatedOn(null);
                        _this.User.DeactivatedBy(null);
                    }
                    else {
                        Global.Helpers.ShowDialog("Deactivate User: " + Details.vm.User.FirstName() + " " + Details.vm.User.LastName(), "/users/deactivate", ["close"], 600, 400).done(function (reason) {
                            if (reason != null) {
                                _this.User.DeactivatedBy("Me");
                                _this.User.DeactivatedByID(User.ID);
                                _this.User.DeactivatedOn(new Date());
                                _this.User.DeactivationReason(reason);
                            }
                            else {
                                _this.User.Active(true);
                            }
                        });
                    }
                });
                _this.UserAcls = ko.observableArray(userAcls.map(function (a) {
                    return new Dns.ViewModels.AclUserViewModel(a);
                }));
                _this.Security = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, _this.UserAcls, [{ Field: "UserID", Value: _this.User ? _this.User.ID() : null }], Dns.ViewModels.AclUserViewModel);
                _this.UserEvents = ko.observableArray(userEvents.map(function (e) {
                    return new Dns.ViewModels.UserEventViewModel(e);
                }));
                _this.Events = new Events.Acl.EventAclEditViewModel(eventList, securityGroupTree, _this.UserEvents, [{ Field: "UserID", Value: _this.User.ID() }], Dns.ViewModels.UserEventViewModel);
                _this.Subscriptions = ko.observableArray(subscribedEvents.map(function (se) {
                    return new Dns.ViewModels.UserEventSubscriptionViewModel(se);
                }));
                _this.SubscribableEventsList = ko.observableArray(subscribableEventsList.map(function (el) {
                    return new EventViewModel(_this, el);
                }));
                _this.AssignedNotificationsList = ko.observableArray(assignedNotificationsList.map(function (el) {
                    return new Dns.ViewModels.AssignedUserNotificationViewModel(el);
                }));
                debugger;
                _this.OrganizationList = organizationList;
                _this.OrganizationList.sort(function (a, b) {
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
                _this.SecurityGroupTree = securityGroupTree;
                _this.dsSecurityGroupsTree = new kendo.data.HierarchicalDataSource({
                    data: _this.SecurityGroupTree,
                    schema: {
                        model: {
                            id: "ID",
                            hasChildren: "HasChildren",
                            children: "SubItems"
                        }
                    }
                });
                _this.SecurityGroupSelected = function (e) {
                    var tree = $("#tvSecurityGroupSelector").data("kendoTreeView");
                    var node = tree.dataItem(e.node);
                    if (!node || !node.id) {
                        e.preventDefault();
                        tree.expand(e.node);
                        return;
                    }
                    var hasGroup = false;
                    _this.self.SecurityGroups().forEach(function (g) {
                        if (g.ID() == node.id) {
                            hasGroup = true;
                            return;
                        }
                    });
                    if (!hasGroup) {
                        _this.self.SecurityGroups.push(new Dns.ViewModels.SecurityGroupViewModel({
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
                    $('#btnAddSecurityGroup').dropdown('toggle');
                    return false;
                };
                _this.AccountStatus = ko.computed(function () {
                    return _this.User.Deleted() ? "Deleted" : _this.User.RejectedOn() != null ? "Rejected" : _this.User.DeactivatedOn() != null ? "Deactivated" : _this.User.ActivatedOn() == null && !_this.User.Active() ? "Pending" : _this.User.Active() ? "Active" : "Locked";
                });
                _this.Name = ko.observable(user.FirstName + " " + user.LastName);
                _this.User.FirstName.subscribe(_this.UpdateName);
                _this.User.LastName.subscribe(_this.UpdateName);
                _this.WatchTitle(_this.Name, "User: ");
                _this.RemoveSecurityGroup = function (data) {
                    Global.Helpers.ShowConfirm("Removal Confirmation", "<p>Are you sure that you wish to remove " + _this.self.Name() + " from the " + data.Path() + " group?</p>").done(function () {
                        _this.self.SecurityGroups.remove(data);
                    });
                };
                return _this;
            }
            ViewModel.prototype.onActivate = function (e) {
                if (e != undefined && e.item.id == "tbAuthentication") {
                    var authGrid = $('#AuthGrid');
                    if (authGrid.hasClass("k-grid")) {
                        authGrid.empty();
                    }
                    authGrid.kendoGrid({
                        dataSource: {
                            type: "webapi",
                            serverPaging: true,
                            serverSorting: true,
                            pageSize: 25,
                            transport: {
                                read: {
                                    cache: false,
                                    url: Global.Helpers.GetServiceUrl("/Users/ListSuccessfulAudits?$orderby=DateTime desc&$filter=Success ne false and UserID eq " + Details.vm.User.ID() + ""),
                                }
                            },
                            schema: {
                                model: kendo.data.Model.define(Dns.Interfaces.KendoModelUserAuthenticationDTO)
                            },
                        },
                        sortable: true,
                        filterable: {
                            operators: {
                                date: {
                                    gt: 'Is after',
                                    lt: 'Is before'
                                }
                            }
                        },
                        resizable: true,
                        reorderable: true,
                        scrollable: {
                            virtual: true
                        },
                        pageable: false,
                        columns: [
                            { field: 'DateTime', title: 'Date', format: Constants.DateTimeFormatter, width: 180 },
                            { field: 'Description', title: 'Description' }
                        ]
                    }).data("kendoGrid");
                }
            };
            ViewModel.prototype.UpdateName = function (value) {
                Details.vm.Name(Details.vm.User.FirstName() + " " + Details.vm.User.LastName());
            };
            ViewModel.prototype.Reject = function () {
                var _this = this;
                Global.Helpers.ShowDialog("Reject User Registration: " + Details.vm.User.FirstName() + " " + Details.vm.User.LastName(), "/users/rejectregistration", ["close"], 600, 400).done(function (reason) {
                    if (reason) {
                        _this.User.RejectedBy("Me");
                        _this.User.RejectedOn(new Date());
                        _this.User.RejectReason(reason);
                        _this.User.RejectedByID(User.ID);
                    }
                });
            };
            ViewModel.prototype.ChangePassword = function () {
                var _this = this;
                this.Save(null, null, false).done(function () {
                    Global.Helpers.ShowDialog("Change Password", "/users/changepassword?ID=" + _this.User.ID(), ["Close"], 500, 300);
                });
            };
            ViewModel.prototype.Delete = function () {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this user?</p>").done(function () {
                    if (Details.vm.User.ID()) {
                        Dns.WebApi.Users.Delete([Details.vm.User.ID()]).done(function () {
                            window.location.href = '/users';
                        });
                    }
                    else {
                        window.location.href = "/users";
                    }
                });
            };
            ViewModel.prototype.Cancel = function () {
                window.history.back();
            };
            ViewModel.prototype.Save = function (data, e, showPrompt) {
                var _this = this;
                if (showPrompt === void 0) { showPrompt = true; }
                var deferred = $.Deferred();
                if (this.User.Fax() == "")
                    this.User.Fax(null);
                if (this.User.Phone() == "")
                    this.User.Phone(null);
                if (!this.Validate()) {
                    deferred.reject();
                    return;
                }
                if (this.HasPermission(Permissions.User.ManageSecurity)) {
                    if (this.User.Active() && this.User.OrganizationID() == null) {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have selected an organization.</p>");
                        deferred.reject();
                        return;
                    }
                    if (this.User.Active() && this.HasPermission(Permissions.User.ManageSecurity) && this.SecurityGroups().length == 0) {
                        Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that this active user belongs to at least one security group.</p>");
                        return;
                    }
                }
                var user = this.User.toData();
                Dns.WebApi.Users.InsertOrUpdate([user]).done(function (users) {
                    _this.User.ID(users[0].ID);
                    _this.User.Timestamp(users[0].Timestamp);
                    window.history.replaceState(null, window.document.title, "/users/details?ID=" + users[0].ID);
                    if (_this.HasPermission(Permissions.User.ManageSecurity)) {
                        var userAcls = _this.UserAcls().map(function (a) {
                            a.UserID(_this.User.ID());
                            return a.toData();
                        });
                        var userEvents = _this.UserEvents().map(function (e) {
                            e.UserID(_this.User.ID());
                            return e.toData();
                        });
                        var uSecurityGroups = Details.vm.SecurityGroups().map(function (sd) {
                            var sg = {
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
                        var userSecurityGroups = {
                            UserID: _this.User.ID(),
                            Groups: uSecurityGroups
                        };
                    }
                    if (_this.HasPermission(Permissions.User.ManageNotifications)) {
                        var userSubscriptions = _this.Subscriptions().map(function (s) {
                            var subscription = {
                                EventID: s.EventID(),
                                Frequency: s.Frequency(),
                                LastRunTime: s.LastRunTime(),
                                NextDueTime: s.NextDueTime(),
                                FrequencyForMy: s.FrequencyForMy(),
                                UserID: _this.User.ID()
                            };
                            return subscription;
                        });
                    }
                    $.when(userAcls == null ? null : Dns.WebApi.Security.UpdateUserPermissions(userAcls), userEvents == null ? null : Dns.WebApi.Events.UpdateUserEventPermissions(userEvents), userSecurityGroups == null ? null : Dns.WebApi.Users.UpdateSecurityGroups(userSecurityGroups), userSubscriptions == null ? null : Dns.WebApi.Users.UpdateSubscribedEvents(userSubscriptions)).done(function () {
                        if (showPrompt) {
                            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(function () {
                                deferred.resolve();
                            });
                        }
                        else {
                            deferred.resolve();
                        }
                    });
                }).fail(function () {
                    deferred.reject();
                });
                return deferred;
            };
            ViewModel.prototype.SelectSecurityGroup = function () {
                var _this = this;
                Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done(function (result) {
                    if (!result)
                        return;
                    _this.SecurityGroups.push({
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
            };
            ViewModel.prototype.HasEventNotificationDetails = function (eventID) {
                return this.AssignedNotificationsList().filter(function (item) { return item.EventID() == eventID(); }).length > 0;
            };
            ViewModel.prototype.GetEventNotificationDetails = function (eventID) {
                var filtered = this.AssignedNotificationsList().filter(function (item) { return item.EventID() == eventID(); });
                var arrItems = [];
                filtered.forEach(function (item) {
                    if (item.Level() == "Global" && item.Description() == "")
                        item.Description("Global");
                    arrItems.push(item.toData());
                });
                var dsResults = new kendo.data.DataSource({
                    data: arrItems,
                    sort: [{ field: 'Level' }],
                    group: [{ field: 'Level' }]
                });
                return dsResults;
            };
            ViewModel.prototype.SecurityGroupMenu_Click = function (data, e) {
                e.stopPropagation();
                return false;
            };
            return ViewModel;
        }(Global.PageViewModel));
        Details.ViewModel = ViewModel;
        function init() {
            var id = $.url().param("ID");
            var organizationId = $.url().param("OrganizationID");
            var defaultScreenPermissions = [
                Permissions.User.ChangeCertificate,
                Permissions.User.ChangeLogin,
                Permissions.User.ChangePassword,
                Permissions.User.Delete,
                Permissions.User.Edit,
                Permissions.User.ManageNotifications,
                Permissions.User.ManageSecurity,
                Permissions.User.View,
            ];
            $.when(id == null ? null : Dns.WebApi.Users.GetPermissions([id], defaultScreenPermissions), id == null ? null : Dns.WebApi.Users.Get(id), id == null ? null : Dns.WebApi.Security.GetUserPermissions(id), id == null ? null : Dns.WebApi.Events.GetUserEventPermissions(id), Dns.WebApi.Users.GetGlobalPermission(Permissions.Organization.ApproveRejectRegistrations), Dns.WebApi.Organizations.List(), Dns.WebApi.Security.GetAvailableSecurityGroupTree(), Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Users]), id == User.ID ? Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Users, Dns.Enums.PermissionAclTypes.UserProfile]) : Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Users]), id == null ? null : Dns.WebApi.Users.GetSubscribableEvents(id, null, null, "Name"), id == null ? null : Dns.WebApi.Users.GetAssignedNotifications(id)).done(function (screenPermissions, users, userAcls, userEvents, canApproveRejectGlobally, organizationList, securityGroupTree, permissionList, eventList, subscribableEventsList, assignedNotificationsList) {
                var user;
                if (users == null || users.length == 0) {
                    user = new Dns.ViewModels.UserViewModel().toData();
                    user.FirstName = "";
                    user.MiddleName = "";
                    user.LastName = "";
                    user.OrganizationID = organizationId;
                    user.Active = false;
                    user.Deleted = false;
                    screenPermissions = defaultScreenPermissions;
                }
                else {
                    user = users[0];
                }
                if (user.ID == User.ID) {
                    $.merge(screenPermissions, [Permissions.User.View.toLowerCase(), Permissions.User.Edit.toLowerCase(), Permissions.User.ChangeLogin.toLowerCase(), Permissions.User.ManageNotifications.toLowerCase()]);
                    screenPermissions = $.unique(screenPermissions);
                }
                var deferred = $.Deferred();
                var subscribedEvents = [];
                var securityGroups = [];
                var canApproveRejectOrgLevel = false;
                deferred.done(function () {
                    $(function () {
                        var bindingControl = $("#Content");
                        Details.vm = new ViewModel(screenPermissions, (canApproveRejectGlobally && canApproveRejectGlobally.length >= 1 && canApproveRejectGlobally[0]) || canApproveRejectOrgLevel, user, securityGroups || [], userAcls || [], userEvents || [], subscribedEvents || [], organizationList || [], securityGroupTree, permissionList || [], eventList || [], subscribableEventsList || [], assignedNotificationsList || [], bindingControl);
                        ko.applyBindings(Details.vm, bindingControl[0]);
                    });
                });
                $.when(id && screenPermissions.indexOf(Permissions.User.ManageSecurity.toLowerCase()) > -1 ? Dns.WebApi.Users.MemberOfSecurityGroups(id) : null, user.ID && (screenPermissions.indexOf(Permissions.User.ManageNotifications.toLowerCase()) > -1 || screenPermissions.indexOf(Permissions.User.ManageNotifications) > -1) ? Dns.WebApi.Users.GetSubscribedEvents(user.ID) : null, user.OrganizationID == null ? null : Dns.WebApi.Organizations.GetPermissions([user.OrganizationID], [Permissions.Organization.ApproveRejectRegistrations])).done(function (sg, events, permission) {
                    subscribedEvents = events;
                    securityGroups = sg;
                    canApproveRejectOrgLevel = (permission == null || permission.length == 0) ? false : permission[0].toUpperCase() == Permissions.Organization.ApproveRejectRegistrations;
                    deferred.resolve();
                });
            });
        }
        init();
        var EventViewModel = (function (_super) {
            __extends(EventViewModel, _super);
            function EventViewModel(vm, event) {
                var _this = _super.call(this, event) || this;
                var subscriptionEvent = new Dns.ViewModels.UserEventSubscriptionViewModel({
                    EventID: _this.ID(),
                    Frequency: null,
                    FrequencyForMy: null,
                    LastRunTime: null,
                    NextDueTime: null,
                    UserID: vm.User.ID()
                });
                _this.SupportsMyFrequency = ko.observable(event.SupportsMyNotifications);
                _this.FrequencyForMy = ko.computed({
                    read: function () {
                        for (var j = 0; j < vm.Subscriptions().length; j++) {
                            var sub = vm.Subscriptions()[j];
                            if (sub.EventID() == _this.ID())
                                return sub.FrequencyForMy();
                        }
                        return null;
                    },
                    write: function (value) {
                        for (var j = 0; j < vm.Subscriptions().length; j++) {
                            var sub = vm.Subscriptions()[j];
                            if (sub.EventID() == _this.ID()) {
                                sub.FrequencyForMy(value);
                                return;
                            }
                        }
                        subscriptionEvent.FrequencyForMy(value);
                        vm.Subscriptions.push(subscriptionEvent);
                    }
                });
                _this.Frequency = ko.computed({
                    read: function () {
                        for (var j = 0; j < vm.Subscriptions().length; j++) {
                            var sub = vm.Subscriptions()[j];
                            if (sub.EventID() == _this.ID())
                                return sub.Frequency();
                        }
                        return null;
                    },
                    write: function (value) {
                        for (var j = 0; j < vm.Subscriptions().length; j++) {
                            var sub = vm.Subscriptions()[j];
                            if (sub.EventID() == _this.ID()) {
                                sub.Frequency(value);
                                return;
                            }
                        }
                        subscriptionEvent.Frequency(value);
                        vm.Subscriptions.push(subscriptionEvent);
                    }
                });
                return _this;
            }
            return EventViewModel;
        }(Dns.ViewModels.EventViewModel));
        Details.EventViewModel = EventViewModel;
    })(Details = Users.Details || (Users.Details = {}));
})(Users || (Users = {}));
