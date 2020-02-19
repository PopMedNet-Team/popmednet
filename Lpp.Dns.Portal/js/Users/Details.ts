/// <reference path="../_rootlayout.ts" />
module Users.Details {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        private self: ViewModel;

        public User: Dns.ViewModels.UserViewModel;
        public IsProfile: boolean;
        public SecurityGroups: KnockoutObservableArray<Dns.ViewModels.SecurityGroupViewModel>;
        public Name: KnockoutObservable<string>;
        public AccountStatus: KnockoutComputed<string>;
        public ShowActivation: KnockoutComputed<boolean>;

        //Security
        public Security: Security.Acl.AclEditViewModel<Dns.ViewModels.AclUserViewModel>;
        public UserAcls: KnockoutObservableArray<Dns.ViewModels.AclUserViewModel>;

        //Events
        public Events: Events.Acl.EventAclEditViewModel<Dns.ViewModels.UserEventViewModel>;
        public UserEvents: KnockoutObservableArray<Dns.ViewModels.UserEventViewModel>;

        //Subscriptions
        public Subscriptions: KnockoutObservableArray<Dns.ViewModels.UserEventSubscriptionViewModel>;

        //Lists
        public OrganizationList: Dns.Interfaces.IOrganizationDTO[];
        public SecurityGroupTree: Dns.Interfaces.ITreeItemDTO[];
        public dsSecurityGroupsTree: kendo.data.HierarchicalDataSource;
        public SecurityGroupSelected: (e: kendo.ui.TreeViewSelectEvent) => boolean;
        public SubscribableEventsList: KnockoutObservableArray<EventViewModel>;

        public AssignedNotificationsList: KnockoutObservableArray<Dns.ViewModels.AssignedUserNotificationViewModel>;
        public dataSource: kendo.data.DataSource;
        //UI Events
        public RemoveSecurityGroup: (data: Dns.ViewModels.SecurityGroupViewModel) => void;

        //For temporarily stopping this.Active subscription callback
        public StopActiveCallback: boolean = false;

        constructor(
            screenPermissions: any[],
            canApproveReject: boolean,
            user: Dns.Interfaces.IUserDTO,
            securityGroups: Dns.Interfaces.ISecurityGroupDTO[],
            userAcls: Dns.Interfaces.IAclUserDTO[],
            userEvents: Dns.Interfaces.IUserEventDTO[],
            subscribedEvents: Dns.Interfaces.IUserEventSubscriptionDTO[],
            organizationList: Dns.Interfaces.IOrganizationDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            permissionList: Dns.Interfaces.IPermissionDTO[],
            eventList: Dns.Interfaces.IEventDTO[],
            subscribableEventsList: Dns.Interfaces.IEventDTO[],
            assignedNotificationsList: Dns.Interfaces.IAssignedUserNotificationDTO[],
            bindingControl: JQuery) {

            super(bindingControl, screenPermissions);
            this.self = this;
            this.IsProfile = User.ID == user.ID;
            this.User = new Dns.ViewModels.UserViewModel(user);
            this.SecurityGroups = ko.observableArray(securityGroups.map((sg) => {
                return new Dns.ViewModels.SecurityGroupViewModel(sg);
            }));

            // Allow activate visibility for the login user only if s/he has Approve Registration rights
            // for the new user's organization either via group ownership or the new user's organization is null.
            this.ShowActivation = ko.computed<boolean>(() => {
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
                        Dns.WebApi.Users.HasPassword(this.User.ID()).done((hasPassword: boolean[]) => {
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
                        });;
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
                } else {
                    Global.Helpers.ShowDialog("Deactivate User: " + vm.User.FirstName() + " " + vm.User.LastName(), "/users/deactivate", ["close"], 600, 400).done((reason: string) => {
                        if (reason != null) {
                            this.User.DeactivatedBy("Me");
                            this.User.DeactivatedByID(User.ID);
                            this.User.DeactivatedOn(new Date());
                            this.User.DeactivationReason(reason);
                        } else {
                            this.User.Active(true);
                        }
                    });
                }
            });            

            //Permissions
            this.UserAcls = ko.observableArray(userAcls.map((a) => {
                return new Dns.ViewModels.AclUserViewModel(a);
            }));

            this.Security = new Security.Acl.AclEditViewModel<Dns.ViewModels.AclUserViewModel>(permissionList, securityGroupTree, this.UserAcls, [{ Field: "UserID", Value: this.User ? this.User.ID() : null }], Dns.ViewModels.AclUserViewModel);

            //Events
            this.UserEvents = ko.observableArray(userEvents.map((e) => {
                return new Dns.ViewModels.UserEventViewModel(e);
            }));

            this.Events = new Events.Acl.EventAclEditViewModel<Dns.ViewModels.UserEventViewModel>(eventList, securityGroupTree, this.UserEvents, [{ Field: "UserID", Value: this.User.ID() }], Dns.ViewModels.UserEventViewModel);

            //Subscriptions
            this.Subscriptions = ko.observableArray(subscribedEvents.map((se) => {
                return new Dns.ViewModels.UserEventSubscriptionViewModel(se);
            }));
            this.SubscribableEventsList = ko.observableArray(subscribableEventsList.map((el) => {
                return new EventViewModel(this, el);
            }));

            this.AssignedNotificationsList = ko.observableArray(assignedNotificationsList.map((el) => {
                return new Dns.ViewModels.AssignedUserNotificationViewModel(el);
            }));
			
            //Lists
			
            this.OrganizationList = organizationList
			this.OrganizationList.sort(
				function(a,b)
				{ 
					if(a.Name > b.Name)
					{
						return 1;
					}
					if(a.Name < b.Name)
					{
						return -1;
					}
					else 
					{
						return 0;
					}
				}
			);
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

            this.SecurityGroupSelected = (e: kendo.ui.TreeViewSelectEvent) => {
                var tree: kendo.ui.TreeView = $("#tvSecurityGroupSelector").data("kendoTreeView");

                var node = tree.dataItem(e.node);

                if (!node || !node.id) {
                    e.preventDefault();
                    tree.expand(e.node);
                    return;
                }

                var hasGroup = false;
                this.self.SecurityGroups().forEach((g) => {
                    if (g.ID() == node.id) {
                        hasGroup = true;
                        return;
                    }
                });

                if (!hasGroup) {
                    //Do the add of the group here with an empty Acl
                    this.self.SecurityGroups.push(new Dns.ViewModels.SecurityGroupViewModel({
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
            }


            //Account Status Badge
            this.AccountStatus = ko.computed<string>(() => {
                return this.User.Deleted() ? "Deleted" : this.User.RejectedOn() != null ? "Rejected" : this.User.DeactivatedOn() != null ? "Deactivated" : this.User.ActivatedOn() == null && !this.User.Active() ? "Pending" : this.User.Active() ? "Active" : "Locked";
            });

            //Title
            this.Name = ko.observable(user.FirstName + " " + user.LastName);
            this.User.FirstName.subscribe(this.UpdateName);
            this.User.LastName.subscribe(this.UpdateName);
            this.WatchTitle(this.Name, "User: ");

            //Events
            this.RemoveSecurityGroup = (data: Dns.ViewModels.SecurityGroupViewModel) => {
                Global.Helpers.ShowConfirm("Removal Confirmation", "<p>Are you sure that you wish to remove " + this.self.Name() + " from the " + data.Path() + " group?</p>").done(() => {
                    this.self.SecurityGroups.remove(data);
                });
            }
        }

        public onActivate(e) {
            if (e != undefined && e.item.id == "tbAuthentication") {
                
                let authGrid = $('#AuthGrid');
                if (authGrid.children().length > 0) {
                    for (var i = 0; i < authGrid.children().length; i++) {
                        let child = authGrid.children()[i];
                        child.remove();
                    }
                }

                Dns.WebApi.Users.ListDistinctEnvironments(vm.User.ID()).done((audits) => {
                    let environments = ko.utils.arrayGetDistinctValues(audits.map(item => {
                        if (item.Environment === null || item.Environment.trim() === "") {
                            return {
                                Name: "Other",
                                ID: "Other"
                            }
                        }
                        return {
                            Name: item.Environment,
                            ID: item.Environment.replace(" ", "")
                        }
                    }));

                    Dns.WebApi.Users.GetSetting(environments.map((item) => { return "Users.Details." + item.ID + ":" + User.ID })).done(settings => {
                        let mainTable = $('<table class="panel-body table table-stripped table-bordered table-hover"></table>');
                        let tableBody = $('<tbody></tbody>')

                        for (var i = 0; i < environments.length; i++) {
                            let environment = environments[i];

                            let mainRow = $('<tr style="background:#f5f5f5"></tr>');
                            let mainCell = $('<td></td>');
                            let icon = $('<i id="img-' + environment.ID + '" class="k-icon k-i-expand"></i><span>' + environment.Name + '</span>').click(function () {
                                var img = $('#img-' + environment.ID);
                                var child = $('#auth-' + environment.ID);
                                var childGrid = $('#grid-' + environment.ID);
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
                                        return item.Key === "Users.Details." + environment.ID + ":" + User.ID
                                    })

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
                                        type: "webapi",
                                        serverPaging: true,
                                        serverSorting: true,
                                        serverGrouping: false,
                                        pageSize: 100,
                                        transport: {
                                            read: {
                                                cache: false,
                                                url: Global.Helpers.GetServiceUrl("/Users/ListAuthenticationAudits?$orderby=DateTime desc&$filter=Environment eq null and UserID eq " + vm.User.ID() + ""),
                                            }
                                        },
                                        schema: {
                                            model: kendo.data.Model.define(Dns.Interfaces.KendoModelUserAuthenticationDTO)
                                        },
                                    },
                                    height: "500px",
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

                            } else {
                                grid.kendoGrid({
                                    dataSource: {
                                        type: "webapi",
                                        serverPaging: true,
                                        serverSorting: true,
                                        serverGrouping: false,
                                        pageSize: 100,
                                        transport: {
                                            read: {
                                                cache: false,
                                                url: Global.Helpers.GetServiceUrl("/Users/ListAuthenticationAudits?$orderby=DateTime desc&$filter=Environment eq '" + environment.Name + "' and UserID eq " + vm.User.ID() + ""),
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
                                Users.SetSetting("Users.Details." + environment.ID + ":" + User.ID, Global.Helpers.GetGridSettings(grid.data("kendoGrid")));
                            });

                            grid.data("kendoGrid").bind("columnShow", function (e) {
                                Users.SetSetting("Users.Details." + environment.ID + ":" + User.ID, Global.Helpers.GetGridSettings(grid.data("kendoGrid")));
                            });

                            grid.data("kendoGrid").bind("columnHide", function (e) {
                                Users.SetSetting("Users.Details." + environment.ID + ":" + User.ID, Global.Helpers.GetGridSettings(grid.data("kendoGrid")));
                            });

                            grid.data("kendoGrid").bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);


                            cell.append(grid);
                            row.append(cell);
                            tableBody.append(row);
                        }
                        mainTable.append(tableBody);
                        authGrid.append(mainTable);

                    })
                });
            }
        }

        private UpdateName(value: string) {
            vm.Name(vm.User.FirstName() + " " + vm.User.LastName());
        }

        public Reject() {
            Global.Helpers.ShowDialog("Reject User Registration: " + vm.User.FirstName() + " " + vm.User.LastName(), "/users/rejectregistration", ["close"], 600, 400).done((reason) => {
                if (reason) {
                    this.User.RejectedBy("Me");
                    this.User.RejectedOn(new Date());
                    this.User.RejectReason(reason);
                    this.User.RejectedByID(User.ID);
                }
            });
        }

        public ChangePassword() {
            this.Save(null, null, false).done(() => {
                Global.Helpers.ShowDialog("Change Password", "/users/changepassword?ID=" + this.User.ID(), ["Close"], 500, 300);
            });
        }

        public Delete() {            
            Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this user?</p>").done(() => {
                if (vm.User.ID()) {
                    Dns.WebApi.Users.Delete([vm.User.ID()]).done(() => {
                        window.location.href = '/users';
                    });
                } else {
                    window.location.href = "/users"; //Return if they're new.
                }
            });
        }

        public Cancel() {
            window.history.back();
        }

        public Save(data, e, showPrompt: boolean = true): JQueryDeferred<void> {
            var deferred = $.Deferred<void>();

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


            var user = this.User.toData();


            //Always test save the primary entity first before anything else.
            Dns.WebApi.Users.InsertOrUpdate([user]).done((users) => {
                this.User.ID(users[0].ID);
                this.User.Timestamp(users[0].Timestamp);

                window.history.replaceState(null, window.document.title, "/users/details?ID=" + users[0].ID);

                if (this.HasPermission(PMNPermissions.User.ManageSecurity)) {
                    var userAcls = this.UserAcls().map((a) => {
                        a.UserID(this.User.ID());
                        return a.toData();
                    });

                    var userEvents = this.UserEvents().map((e) => {
                        e.UserID(this.User.ID());
                        return e.toData();
                    });
                   var uSecurityGroups = vm.SecurityGroups().map((sd) => {
                       var sg : Dns.Interfaces.ISecurityGroupDTO = {
                            ID: sd.ID(),
                            Name: sd.Name(),
                            Path: sd.Path(),
                            Type: sd.Type(),
                            OwnerID: sd.OwnerID(),
                            Owner: sd.Owner(),
                            ParentSecurityGroup: sd.ParentSecurityGroup(),
                            ParentSecurityGroupID: sd.ParentSecurityGroupID(),
                            Kind: sd.Kind()
                        }
                       return sg;
                    });
                   
                    var userSecurityGroups: Dns.Interfaces.IUpdateUserSecurityGroupsDTO = {
                        UserID: this.User.ID(),
                        Groups: uSecurityGroups
                    };
                }

                if (this.HasPermission(PMNPermissions.User.ManageNotifications)) {
                    var userSubscriptions = this.Subscriptions().map((s) => {
                        var subscription: Dns.Interfaces.IUserEventSubscriptionDTO = {
                            EventID: s.EventID(),
                            Frequency: s.Frequency(),
                            LastRunTime: s.LastRunTime(),
                            NextDueTime: s.NextDueTime(),
                            FrequencyForMy: s.FrequencyForMy(),
                            UserID: this.User.ID()
                        };
                        return subscription;
                    });
                }

                $.when<any>(
                    userAcls == null ? null : Dns.WebApi.Security.UpdateUserPermissions(userAcls),
                    userEvents == null ? null : Dns.WebApi.Events.UpdateUserEventPermissions(userEvents),
                    userSecurityGroups == null ? null : Dns.WebApi.Users.UpdateSecurityGroups(userSecurityGroups),
                    userSubscriptions == null ? null : Dns.WebApi.Users.UpdateSubscribedEvents(userSubscriptions)
                    ).done(() => {
                        if (showPrompt) {
                            Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>").done(() => {
                                deferred.resolve();
                            });
                        } else {
                            deferred.resolve();
                        }
                    });
            }).fail(() => {
                deferred.reject();
            });

            return deferred;
        }

        public SelectSecurityGroup() {

            Global.Helpers.ShowDialog("Add Security Group", "/security/SecurityGroupWindow", ["close"], 950, 550).done((result: Dns.Interfaces.ISecurityGroupDTO) => {
                if (!result)
                    return;
                this.SecurityGroups.push(<any>{
                    ID: ko.observable(result.ID),
                    Name: ko.observable(result.Name),
                    Path: ko.observable(result.Path),
                    Type:  ko.observable(result.Type),
                    OwnerID:  ko.observable(result.OwnerID),
                    Owner:  ko.observable(result.Owner),
                    ParentSecurityGroup: ko.observable(result.ParentSecurityGroup),
                    ParentSecurityGroupID: ko.observable(result.ParentSecurityGroupID),
                    Kind: ko.observable(result.Kind)
                });
                
            });

        }

        public HasEventNotificationDetails(eventID) {
            return this.AssignedNotificationsList().filter(function (item) { return item.EventID() == eventID() }).length > 0;
        }

        public GetEventNotificationDetails(eventID) {
            var filtered = this.AssignedNotificationsList().filter(function (item) { return item.EventID() == eventID() });
            //    forEach((v) => {
            //        v.Description(v.Level() == "Global" && v.Description() == "" ? "Global" : v.Description());
            //});

            var arrItems = [];
            filtered.forEach((item) => {
                if (item.Level() == "Global" && item.Description() == "")
                    item.Description("Global");
                arrItems.push(item.toData());
            });

            var dsResults = new kendo.data.DataSource({
                data: arrItems,
                sort: [{ field: 'Level' }],
                group: [{field: 'Level' }]
            });

            return dsResults;
        }

        public SecurityGroupMenu_Click(data, e: JQueryEventObject): boolean {
            e.stopPropagation();
            return false;
        }
    }

    function init() {
        var id: any = $.url().param("ID");
        var organizationId: any = $.url().param("OrganizationID");

        var defaultScreenPermissions = [
            PMNPermissions.User.ChangeCertificate,
            PMNPermissions.User.ChangeLogin,
            PMNPermissions.User.ChangePassword,
            PMNPermissions.User.Delete,
            PMNPermissions.User.Edit,
            PMNPermissions.User.ManageNotifications,
            PMNPermissions.User.ManageSecurity,
            PMNPermissions.User.View,
        ];

        $.when<any>(
            id == null ? null : Dns.WebApi.Users.GetPermissions([id], defaultScreenPermissions),
            id == null ? null : Dns.WebApi.Users.Get(id),
            id == null ? null : Dns.WebApi.Security.GetUserPermissions(id),
            id == null ? null : Dns.WebApi.Events.GetUserEventPermissions(id),
            Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.ApproveRejectRegistrations),
            Dns.WebApi.Organizations.List(),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.Users]),
            id == User.ID ? Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Users, Dns.Enums.PermissionAclTypes.UserProfile]) : Dns.WebApi.Events.GetEventsByLocation([Dns.Enums.PermissionAclTypes.Users]),
            id == null ? null : Dns.WebApi.Users.GetSubscribableEvents(id, null, null, "Name"),
            id == null ? null : Dns.WebApi.Users.GetAssignedNotifications(id)
         ).done((
            screenPermissions: any[],
            users,
            userAcls,
            userEvents,
            canApproveRejectGlobally,
            organizationList,
            securityGroupTree,
            permissionList,
            eventList,
            subscribableEventsList,
            assignedNotificationsList
         ) => {
            var user: Dns.Interfaces.IUserDTO;

             if (users == null || users.length == 0) {
                // New user
                user = new Dns.ViewModels.UserViewModel().toData();
                user.FirstName = "";
                user.MiddleName = "";
                user.LastName = "";
                user.OrganizationID = organizationId;
                //user.SignedUpOn = new Date(Date.now());
                user.Active = false;
                user.Deleted = false;

                screenPermissions = defaultScreenPermissions;
             } else {
                // Existing user
                user = users[0];
            }

            // If it's the current user (editing own profile), ensure they have view, edit and manage notification rights
            if (user.ID == User.ID) {
                $.merge(screenPermissions, [PMNPermissions.User.View.toLowerCase(), PMNPermissions.User.Edit.toLowerCase(), PMNPermissions.User.ChangeLogin.toLowerCase(), PMNPermissions.User.ManageNotifications.toLowerCase()]);
                screenPermissions = $.unique(screenPermissions);
            }

            var deferred = $.Deferred<void>();
            var subscribedEvents: Dns.Interfaces.IUserEventSubscriptionDTO[] = [];
             var securityGroups: Dns.Interfaces.ISecurityGroupDTO[] = [];
            var canApproveRejectOrgLevel: boolean = false; //Set in the deferred below

            deferred.done(() => {
                $(() => {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(screenPermissions,
                        (canApproveRejectGlobally && canApproveRejectGlobally.length >= 1 && canApproveRejectGlobally[0]) || canApproveRejectOrgLevel,
                        user,
                        securityGroups || [],
                        userAcls || [],
                        userEvents || [],
                        subscribedEvents || [],
                        organizationList || [],
                        securityGroupTree,
                        permissionList || [],
                        eventList || [],
                        subscribableEventsList || [],
                        assignedNotificationsList || [],
                        bindingControl);

                    ko.applyBindings(vm, bindingControl[0]);
                });
            });

            $.when<any>(
                id && screenPermissions.indexOf(PMNPermissions.User.ManageSecurity.toLowerCase()) > -1 ? Dns.WebApi.Users.MemberOfSecurityGroups(id) : null,
                user.ID && (screenPermissions.indexOf(PMNPermissions.User.ManageNotifications.toLowerCase()) > -1 || screenPermissions.indexOf(PMNPermissions.User.ManageNotifications) > -1) ? Dns.WebApi.Users.GetSubscribedEvents(user.ID) : null,
                user.OrganizationID == null ? null : Dns.WebApi.Organizations.GetPermissions([user.OrganizationID], [PMNPermissions.Organization.ApproveRejectRegistrations])
            ).done((sg, events, permission) => {
                subscribedEvents = events;
                securityGroups = sg;
                canApproveRejectOrgLevel = (permission == null || permission.length == 0) ? false : permission[0].toUpperCase() == PMNPermissions.Organization.ApproveRejectRegistrations;
                deferred.resolve();
            });
        });
    }

    init();

    export class EventViewModel extends Dns.ViewModels.EventViewModel {
        public Frequency: KnockoutComputed<Dns.Enums.Frequencies>;
        public FrequencyForMy: KnockoutComputed<Dns.Enums.Frequencies>;
        public SupportsMyFrequency: KnockoutObservable<Boolean>;

        constructor(vm: ViewModel, event: Dns.Interfaces.IEventDTO) {
            super(event);
            var subscriptionEvent = new Dns.ViewModels.UserEventSubscriptionViewModel({
                EventID: this.ID(),
                Frequency: null,
                FrequencyForMy: null,
                LastRunTime: null,
                NextDueTime: null,
                UserID: vm.User.ID()
            });

            this.SupportsMyFrequency = ko.observable(event.SupportsMyNotifications);
            
            this.FrequencyForMy = ko.computed<Dns.Enums.Frequencies>({
                read: () => {
                    for (var j = 0; j < vm.Subscriptions().length; j++) {
                        var sub = vm.Subscriptions()[j];
                        if (sub.EventID() == this.ID())
                            return sub.FrequencyForMy();
                    }

                    return null;
                },
                write: (value) => {
                    for (var j = 0; j < vm.Subscriptions().length; j++) {
                        var sub = vm.Subscriptions()[j];
                        if (sub.EventID() == this.ID()) {
                            sub.FrequencyForMy(value);
                            return;
                        }
                    }
                    
                    subscriptionEvent.FrequencyForMy(value);

                    vm.Subscriptions.push(subscriptionEvent);
                }

            });
            this.Frequency = ko.computed<Dns.Enums.Frequencies>({
                read: () => {
                    for (var j = 0; j < vm.Subscriptions().length; j++) {
                        var sub = vm.Subscriptions()[j];
                        if (sub.EventID() == this.ID())
                            return sub.Frequency();
                    }

                    return null;
                },
                write: (value) => {
                    for (var j = 0; j < vm.Subscriptions().length; j++) {
                        var sub = vm.Subscriptions()[j];
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
}