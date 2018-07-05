/// <reference path="../_rootlayout.ts" />
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
var Home;
(function (Home) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(projects, settings, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.gRequestsRowSelector = 'multiple,row';
                var self = _this;
                _this.gRequestsHeight = ko.observable(null);
                _this.gTasksHeight = ko.observable(null);
                _this.gMessagesHeight = ko.observable(null);
                _this.gNotificationsHeight = ko.observable(null);
                _this.gDataMartsHeight = ko.observable(400);
                if (ViewModel.editMetadataPermissions.CanEditRequestMetadata == false) {
                    _this.gRequestsRowSelector = false;
                }
                _this.SelectedRequests = ko.observableArray();
                _this.EnableRequestBulkEdit = ko.computed(function () {
                    //at least one row selected and all the rows are allowed to edit metadata
                    return self.SelectedRequests().length > 0 && ko.utils.arrayFirst(self.SelectedRequests(), function (rq) { return rq.CanEditMetadata == false; }) == null;
                }, _this, { pure: true });
                _this.InvalidSelectedRequests = ko.computed(function () { return ko.utils.arrayFilter(self.SelectedRequests(), function (rq) { return rq.CanEditMetadata == false; }); }, _this, { pure: true });
                _this.Projects = ko.observableArray(projects.map(function (item) {
                    return new Dns.ViewModels.ProjectViewModel(item);
                }));
                _this.gNotificationsVisible = ko.observable(false);
                _this.dsNotifications = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 100,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/users/getnotifications?UserID=" + User.ID),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelNotificationDTO)
                    },
                    sort: {
                        field: "Timestamp", dir: "desc"
                    },
                    change: function (e) {
                        vm.gNotificationsVisible(e.items != null && e.items.length > 0);
                        vm.gNotificationsHeight(e.items != null && e.items.length > 0 ? "200px" : "34px");
                    }
                });
                var gNotificationsSettings = settings.filter(function (item) { return item.Key === "Home.Index.gNotifications.User:" + User.ID; });
                if (gNotificationsSettings.length > 0 && gNotificationsSettings[0] !== null) {
                    Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsNotifications, gNotificationsSettings[0].Setting, ["Timestamp"]);
                }
                _this.dsRequest = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 500,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl('/requests/listforhomepage'),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelHomepageRequestDetailDTO)
                    },
                    sort: { field: "SubmittedOn", dir: "desc" },
                    change: function (e) {
                        vm.gRequestsHeight(e.items != null && e.items.length > 0 ? "600px" : "34px");
                    }
                });
                var dsRequestSettings = settings.filter(function (item) { return item.Key === "Home.Index.gRequests.User:" + User.ID; });
                if (dsRequestSettings.length > 0 && dsRequestSettings[0] !== null) {
                    Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsRequest, dsRequestSettings[0].Setting, ["SubmittedOn", "DueDate"]);
                }
                self.onRequestRowSelectionChange = function (e) {
                    var selectedRequests = [];
                    var grid = $(e.sender.wrapper).data('kendoGrid');
                    var rows = grid.select();
                    if (rows.length > 0) {
                        for (var i = 0; i < rows.length; i++) {
                            var request = grid.dataItem(rows[i]);
                            selectedRequests.push(request);
                        }
                    }
                    self.SelectedRequests(selectedRequests);
                };
                self.onRequestBulkEdit = function (data, evt) {
                    evt.stopPropagation();
                    evt.preventDefault();
                    if (self.SelectedRequests().length == 0 || self.InvalidSelectedRequests().length > 0)
                        return;
                    var selected = $.map(self.SelectedRequests(), function (r) { return r.ID; });
                    location.href = '/requests/bulkedit/?r=' + selected.join(',');
                };
                self.onClickRequestsFooter = function (data, evt) {
                    var grid = self.RequestsGrid();
                    grid.clearSelection();
                    evt.preventDefault();
                };
                _this.dsMessages = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: false,
                    serverSorting: true,
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/networkmessages/list/" /*lastdays?days=15"*/),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelNetworkMessageDTO)
                    },
                    sort: { field: "CreatedOn", dir: "desc" },
                    change: function (e) {
                        vm.gMessagesHeight(e.items != null && e.items.length > 0 ? "200px" : "34px");
                    }
                });
                var dsMessagesSettings = settings.filter(function (item) { return item.Key === "Home.Index.gMessages.User:" + User.ID; });
                if (dsMessagesSettings.length > 0 && dsMessagesSettings[0] !== null) {
                    Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsMessages, dsMessagesSettings[0].Setting, ["CreatedOn"]);
                }
                _this.dsTasks = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: false,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 100,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/users/GetWorkflowTasks?UserID=" + User.ID)
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelHomepageTaskSummaryDTO)
                    },
                    sort: {
                        field: "CreatedOn", dir: "desc"
                    },
                    change: function (e) {
                        vm.gTasksHeight(e.items != null && e.items.length > 0 ? "300px" : "34px");
                    }
                });
                var dsTasksSettings = settings.filter(function (item) { return item.Key === "Home.Index.gTasks.User:" + User.ID; });
                if (dsTasksSettings.length > 0 && dsTasksSettings[0] !== null) {
                    Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsTasks, dsTasksSettings[0].Setting, ["CreatedOn", "StartOn", "EndOn"]);
                }
                var now = moment().add(5, 'days');
                var userdate = moment(User.PasswordExpiration);
                if (userdate <= now)
                    Global.Helpers.ShowToast("Your Password is Expiring soon.  Please update your password.");
                _this.dsDataMarts = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: false,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 1000,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/datamarts/listbasic")
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartListDTO)
                    },
                    sort: {
                        field: "Name", dir: "asc"
                    }
                });
                var dsDataMartsSettings = settings.filter(function (item) { return item.Key === "Home.Index.gDataMarts.User:" + User.ID; });
                if (dsDataMartsSettings.length > 0 && dsDataMartsSettings[0] !== null) {
                    Global.Helpers.SetDataSourceFromSettings(_this.dsDataMarts, dsDataMartsSettings[0].Setting);
                }
                return _this;
            }
            ViewModel.prototype.RequestsGrid = function () {
                return $("#gRequests").data("kendoGrid");
            };
            ViewModel.prototype.TasksGrid = function () {
                return $("#gTasks").data("kendoGrid");
            };
            ViewModel.prototype.NotificationsGrid = function () {
                return $("#gNotifications").data("kendoGrid");
            };
            ViewModel.prototype.MessagesGrid = function () {
                return $("#gMessages").data("kendoGrid");
            };
            ViewModel.prototype.DataMartsGrid = function () {
                return $("#gDataMarts").data("kendoGrid");
            };
            ViewModel.prototype.CreateRequest = function (project) {
                Global.Helpers.ShowDialog("Choose Request Type", "/requests/createdialog", ["Close"], 400, 600, { ProjectID: project.ID() }).done(function (result) {
                    if (!result)
                        return;
                    var url;
                    if (!result.TemplateID && !result.WorkflowID) {
                        // Legacy Non-workflow request types
                        url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + project.ID();
                    }
                    else if (!result.TemplateID) {
                        // Workflow based non-QueryComposer request types
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + project.ID() + "&WorkflowID=" + result.WorkflowID;
                    }
                    else {
                        // QueryComposer request types
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + project.ID() + "&templateID=" + result.TemplateID + "&WorkflowID=" + result.WorkflowID;
                    }
                    window.location.href = url;
                });
            };
            ViewModel.prototype.FormatTaskName = function (e) {
                if (e.DirectToRequest) {
                    return '<a href="/requests/details?ID=' + e.RequestID + '&taskID=' + e.TaskID + '">' + e.TaskName + '</a>';
                }
                return '<a href="/tasks/details?ID=' + e.TaskID + '">' + e.TaskName + '</a>';
            };
            ViewModel.prototype.FormatNameForTask = function (e) {
                if (e.DirectToRequest) {
                    return '<a href="/requests/details?ID=' + e.RequestID + '&taskID=' + e.TaskID + '">' + e.Name + '</a>';
                }
                if (e.NewUserID != null) {
                    return '<a href="/users/details?ID=' + e.NewUserID + '">' + e.Name + '</a>';
                }
                return '<a href="/tasks/details?ID=' + e.TaskID + '">' + e.Name + '</a>';
            };
            ViewModel.prototype.FormatAssignedResources = function (e) {
                return decodeHtml(e.AssignedResources);
            };
            ViewModel.prototype.onDataMartsDetailInit = function (e) {
                var datamart = e.data;
                var canEditAnyMetadata = ko.utils.arrayFirst(ViewModel.editMetadataPermissions.EditableDataMarts, function (id) { return datamart.ID == id; }) != null;
                var grid = $('<div style="min-height:155px;"/>').kendoGrid({
                    sortable: true,
                    filterable: {
                        operators: {
                            date: {
                                gt: 'Is after',
                                lt: 'Is before'
                            }
                        }
                    },
                    autoBind: true,
                    resizable: true,
                    reorderable: true,
                    pageable: false,
                    groupable: false,
                    columnMenu: { columns: true },
                    columnMenuInit: function (e) {
                        var menu = e.container.find(".k-menu").data("kendoMenu");
                        menu.bind("close", function (e) {
                            //update the local grid settings
                            //vm.gDataMartsRoutesSetting = Global.Helpers.GetGridSettings(grid.data('kendoGrid'));
                            //save the grid settings
                            //Users.SetSetting("Home.Index.gDataMartsRoutes.User:" + User.ID, this.gDataMartsRoutesSetting);
                        });
                    },
                    selectable: canEditAnyMetadata ? 'multiple,row' : false,
                    height: '100%',
                    columns: [
                        { field: 'Name', title: 'Name', template: '<a href="# if (IsWorkflowRequest) {# /requests/details?ID=#:RequestID# #}else{# /request/#:RequestID# #}#">#=Name#</a>', width: 200 },
                        {
                            field: 'Identifier', title: 'System Number', width: 90, filterable: {
                                ui: function (element) {
                                    element.kendoNumericTextBox({
                                        format: '####',
                                        decimals: 0
                                    });
                                }
                            }
                        },
                        { field: 'SubmittedOn', title: 'Date Submitted', format: Constants.DateTimeFormatter, width: 165 },
                        { field: 'SubmittedByName', title: 'Submitter', width: 100 },
                        { field: 'StatusText', title: 'Request Status', width: 125 },
                        { field: 'RoutingStatusText', title: 'Routing Status', width: 125 },
                        { field: 'RequestType', title: 'Type', width: 175 },
                        { field: 'Project', title: 'Project', width: 125 },
                        { field: 'Priority', title: 'Priority', values: Dns.Enums.PrioritiesTranslation, width: 100 },
                        { field: 'DueDate', title: 'Due Date', format: Constants.DateFormatter, width: 120 },
                        { field: 'MSRequestID', title: 'Request ID', width: 120 },
                    ],
                    change: function (e) {
                        var grd = $(e.sender.wrapper).data('kendoGrid');
                        var rows = grd.select();
                        var btn = $('#dmbulkedit-' + datamart.ID);
                        if (btn) {
                            var container = $('#dmfooter-errorcontainer-' + datamart.ID);
                            ko.cleanNode(container[0]);
                            var viewmodel = {
                                InvalidSelectedRequests: []
                            };
                            if (rows.length > 0) {
                                var invalidRequests = [];
                                for (var i = 0; i < rows.length; i++) {
                                    var request = grd.dataItem(rows[i]);
                                    if (request.CanEditMetadata == false) {
                                        invalidRequests.push(request);
                                    }
                                }
                                viewmodel.InvalidSelectedRequests = invalidRequests;
                            }
                            if (rows.length == 0 || viewmodel.InvalidSelectedRequests.length > 0) {
                                btn.attr('disabled', '');
                            }
                            else {
                                btn.removeAttr('disabled');
                            }
                            ko.applyBindings(viewmodel, container[0]);
                        }
                    }
                });
                var url = Global.Helpers.GetServiceUrl("requests/requestsbyroute?id=" + datamart.ID);
                var datasource = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 1000,
                    transport: {
                        read: {
                            url: url,
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelHomepageRouteDetailDTO)
                    },
                    sort: { field: "Identifier", dir: "desc" }
                });
                var gd = grid.data('kendoGrid');
                gd.setDataSource(datasource);
                var panel = $('<div class="panel panel-default">');
                var gridContainer = $('<div class="panel-body" style="height:400px;width:990px;overflow:auto;overflow-y:hidden;padding:0px;"></div>');
                $(grid).appendTo(gridContainer);
                $(gridContainer).appendTo(panel);
                if (canEditAnyMetadata) {
                    var bulkEditButton = $('<button disabled="" class="btn btn-default">Edit Requests</button>');
                    bulkEditButton.attr('id', 'dmbulkedit-' + datamart.ID);
                    bulkEditButton.click(function (evt) {
                        evt.stopPropagation();
                        evt.preventDefault();
                        var grd = gd;
                        var rows = grd.select();
                        if (rows.length == 0)
                            return;
                        var selected = $.map(rows, function (r) {
                            var route = grd.dataItem(r);
                            return route.RequestDataMartID;
                        });
                        location.href = '/requests/bulkeditroutes/?r=' + selected.join(',');
                    });
                    var footer = $('<div class="panel-footer">');
                    footer.attr('id', 'dmfooter-' + datamart.ID);
                    footer.click(function (e) {
                        var grid = gd;
                        gd.clearSelection();
                        e.preventDefault();
                        var container = $('#dmfooter-errorcontainer-' + datamart.ID);
                        ko.cleanNode(container[0]);
                        var viewmodel = {
                            InvalidSelectedRequests: []
                        };
                        ko.applyBindings(viewmodel, container[0]);
                    });
                    var footerrow = $('<div class="row">');
                    footerrow.appendTo(footer);
                    var footerbuttoncell = $('<div class="col-xs-2">');
                    footerbuttoncell.appendTo(footerrow);
                    bulkEditButton.appendTo(footerbuttoncell);
                    var footererrorcell = $('<div class="col-xs-9">');
                    footererrorcell.appendTo(footerrow);
                    $('<div id="dmfooter-errorcontainer-' + datamart.ID + '" data-bind="template: { name: \'invalid-requests-template\'}">').appendTo(footererrorcell);
                    $(footer).appendTo(panel);
                }
                $(panel).appendTo(e.detailCell);
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function decodeHtml(value) {
            var txt = document.createElement('textarea');
            txt.innerHTML = value;
            return txt.value;
        }
        Index.decodeHtml = decodeHtml;
        function NameAnchor(dataItem) {
            if (dataItem.IsWorkflowRequest) {
                return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
            }
            else {
                return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
            }
        }
        Index.NameAnchor = NameAnchor;
        function SubjectAnchor(dataItem) {
            return "<a href=\"/tasks/details?ID=" + dataItem.ID + "\">" + dataItem.Subject + "</a>";
        }
        Index.SubjectAnchor = SubjectAnchor;
        function DueDateTemplate(dataItem) {
            return dataItem.DueDate ? moment(dataItem.DueDate).format("MM/DD/YYYY") : "";
        }
        Index.DueDateTemplate = DueDateTemplate;
        function init() {
            $.when(Dns.WebApi.Projects.RequestableProjects(null, "ID,Name", "Name"), Users.GetSettings([
                "Home.Index.gRequests.User:" + User.ID,
                "Home.Index.gTasks.User:" + User.ID,
                "Home.Index.gNotifications.User:" + User.ID,
                "Home.Index.gMessages.User:" + User.ID,
                "Home.Index.gDataMarts.User:" + User.ID,
                "Home.Index.gDataMartsRoutes.User:" + User.ID
            ]), Dns.WebApi.Users.GetMetadataEditPermissionsSummary()).done(function (projects, settings, editMetadataPermissions) {
                $(function () {
                    var bindingControl = $("#Content");
                    ViewModel.editMetadataPermissions = editMetadataPermissions.length > 0 ? editMetadataPermissions[0] : { CanEditRequestMetadata: false, EditableDataMarts: [] };
                    vm = new ViewModel(projects, settings, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                    vm.TasksGrid().bind("dataBound", function (e) {
                        Users.SetSetting("Home.Index.gTasks.User:" + User.ID, Global.Helpers.GetGridSettings(vm.TasksGrid()));
                    });
                    vm.MessagesGrid().bind("dataBound", function (e) {
                        Users.SetSetting("Home.Index.gMessages.User:" + User.ID, Global.Helpers.GetGridSettings(vm.MessagesGrid()));
                    });
                    vm.NotificationsGrid().bind("dataBound", function (e) {
                        Users.SetSetting("Home.Index.gNotifications.User:" + User.ID, Global.Helpers.GetGridSettings(vm.NotificationsGrid()));
                    });
                    vm.RequestsGrid().bind("dataBound", function (e) {
                        Users.SetSetting("Home.Index.gRequests.User:" + User.ID, Global.Helpers.GetGridSettings(vm.RequestsGrid()));
                    });
                    vm.DataMartsGrid().bind("dataBound", function (e) {
                        Users.SetSetting("Home.Index.gDataMarts.User:" + User.ID, Global.Helpers.GetGridSettings(vm.DataMartsGrid()));
                    });
                    for (var i = 0; i < vm.NotificationsGrid().columns.length; i++) {
                        var tasksGridOptions = $.extend({}, vm.NotificationsGrid().getOptions());
                        if (vm.NotificationsGrid().columns[i].title == 'Date') {
                            tasksGridOptions.columns[i].width = 100;
                            vm.NotificationsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.NotificationsGrid().columns[i].title == 'Event') {
                            tasksGridOptions.columns[i].width = 125;
                            vm.NotificationsGrid().setOptions(tasksGridOptions);
                        }
                    }
                    vm.NotificationsGrid().bind("columnShow", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.NotificationsGrid().columns.length; i++) {
                            if (vm.NotificationsGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.NotificationsGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.NotificationsGrid().columns.length; i++) {
                            var notificationsGridOptions = $.extend({}, vm.NotificationsGrid().getOptions());
                            if (numAvailCol < 3) {
                                if (vm.NotificationsGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.NotificationsGrid().table.width();
                                    notificationsGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.NotificationsGrid().setOptions(notificationsGridOptions);
                                }
                                else if (vm.NotificationsGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.NotificationsGrid().table.width();
                                    notificationsGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.NotificationsGrid().setOptions(notificationsGridOptions);
                                }
                                else {
                                    notificationsGridOptions.columns[i].hidden = true;
                                    vm.NotificationsGrid().setOptions(notificationsGridOptions);
                                }
                            }
                            else {
                                if (vm.NotificationsGrid().columns[i].title == 'Date') {
                                    notificationsGridOptions.columns[i].width = 100;
                                    vm.NotificationsGrid().setOptions(notificationsGridOptions);
                                }
                                else if (vm.NotificationsGrid().columns[i].title == 'Event') {
                                    notificationsGridOptions.columns[i].width = 125;
                                    vm.NotificationsGrid().setOptions(notificationsGridOptions);
                                }
                            }
                        }
                    });
                    vm.NotificationsGrid().bind("columnHide", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.NotificationsGrid().columns.length; i++) {
                            if (vm.NotificationsGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.NotificationsGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.NotificationsGrid().columns.length; i++) {
                            var taskGridOptions = $.extend({}, vm.NotificationsGrid().getOptions());
                            if (numAvailCol < 3) {
                                if (vm.NotificationsGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.NotificationsGrid().table.width();
                                    taskGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.NotificationsGrid().setOptions(taskGridOptions);
                                }
                                else if (vm.NotificationsGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.NotificationsGrid().table.width();
                                    taskGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.NotificationsGrid().setOptions(taskGridOptions);
                                }
                                else {
                                    taskGridOptions.columns[i].hidden = true;
                                    vm.NotificationsGrid().setOptions(taskGridOptions);
                                }
                            }
                            else {
                                if (vm.NotificationsGrid().columns[i].title == 'Date') {
                                    taskGridOptions.columns[i].width = 100;
                                    vm.NotificationsGrid().setOptions(taskGridOptions);
                                }
                                else if (vm.NotificationsGrid().columns[i].title == 'Event') {
                                    taskGridOptions.columns[i].width = 125;
                                    vm.NotificationsGrid().setOptions(taskGridOptions);
                                }
                            }
                        }
                    });
                    for (var i = 0; i < vm.MessagesGrid().columns.length; i++) {
                        var tasksGridOptions = $.extend({}, vm.MessagesGrid().getOptions());
                        if (vm.MessagesGrid().columns[i].title == 'Date') {
                            tasksGridOptions.columns[i].width = 100;
                            vm.MessagesGrid().setOptions(tasksGridOptions);
                        }
                    }
                    vm.MessagesGrid().bind("columnShow", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.MessagesGrid().columns.length; i++) {
                            if (vm.MessagesGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.MessagesGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.MessagesGrid().columns.length; i++) {
                            var messagesGridOptions = $.extend({}, vm.MessagesGrid().getOptions());
                            if (numAvailCol < 2) {
                                if (vm.MessagesGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.MessagesGrid().table.width();
                                    messagesGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                                else if (vm.MessagesGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.MessagesGrid().table.width();
                                    messagesGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                                else {
                                    messagesGridOptions.columns[i].hidden = true;
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                            }
                            else {
                                if (vm.MessagesGrid().columns[i].title == 'Date') {
                                    messagesGridOptions.columns[i].width = 100;
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                            }
                        }
                    });
                    vm.MessagesGrid().bind("columnHide", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.MessagesGrid().columns.length; i++) {
                            if (vm.MessagesGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.MessagesGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.MessagesGrid().columns.length; i++) {
                            var messagesGridOptions = $.extend({}, vm.MessagesGrid().getOptions());
                            if (numAvailCol < 2) {
                                if (vm.MessagesGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.MessagesGrid().table.width();
                                    messagesGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                                else if (vm.MessagesGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.MessagesGrid().table.width();
                                    messagesGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                                else {
                                    messagesGridOptions.columns[i].hidden = true;
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                            }
                            else {
                                if (vm.MessagesGrid().columns[i].title == 'Date') {
                                    messagesGridOptions.columns[i].width = 100;
                                    vm.MessagesGrid().setOptions(messagesGridOptions);
                                }
                            }
                        }
                    });
                    for (var i = 0; i < vm.TasksGrid().columns.length; i++) {
                        var tasksGridOptions = $.extend({}, vm.TasksGrid().getOptions());
                        if (vm.TasksGrid().columns[i].title == 'Task') {
                            tasksGridOptions.columns[i].width = 150;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Name') {
                            tasksGridOptions.columns[i].width = 180;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Task Status') {
                            tasksGridOptions.columns[i].width = 150;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Created') {
                            tasksGridOptions.columns[i].width = 160;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Start Date') {
                            tasksGridOptions.columns[i].width = 160;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'End Date') {
                            tasksGridOptions.columns[i].width = 160;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Assignees') {
                            tasksGridOptions.columns[i].width = 300;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Type') {
                            tasksGridOptions.columns[i].width = 160;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Request ID') {
                            tasksGridOptions.columns[i].width = 120;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'System Number') {
                            tasksGridOptions.columns[i].width = 150;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.TasksGrid().columns[i].title == 'Request Status') {
                            tasksGridOptions.columns[i].width = 150;
                            vm.TasksGrid().setOptions(tasksGridOptions);
                        }
                    }
                    vm.TasksGrid().bind("columnShow", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.TasksGrid().columns.length; i++) {
                            if (vm.TasksGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.TasksGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.TasksGrid().columns.length; i++) {
                            var tasksGridOptions = $.extend({}, vm.TasksGrid().getOptions());
                            if (numAvailCol < 11) {
                                if (vm.TasksGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.TasksGrid().table.width();
                                    tasksGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.TasksGrid().table.width();
                                    tasksGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else {
                                    tasksGridOptions.columns[i].hidden = true;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                            }
                            else {
                                if (vm.TasksGrid().columns[i].title == 'Task') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Name') {
                                    tasksGridOptions.columns[i].width = 180;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Task Status') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Created') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Start Date') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'End Date') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Assignees') {
                                    tasksGridOptions.columns[i].width = 300;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Type') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Request ID') {
                                    tasksGridOptions.columns[i].width = 120;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'System Number') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Request Status') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                            }
                        }
                    });
                    vm.TasksGrid().bind("columnHide", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.TasksGrid().columns.length; i++) {
                            if (vm.TasksGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.TasksGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.TasksGrid().columns.length; i++) {
                            var tasksGridOptions = $.extend({}, vm.TasksGrid().getOptions());
                            if (numAvailCol < 11) {
                                if (vm.TasksGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.TasksGrid().table.width();
                                    tasksGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.TasksGrid().table.width();
                                    tasksGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else {
                                    tasksGridOptions.columns[i].hidden = true;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                            }
                            else {
                                if (vm.TasksGrid().columns[i].title == 'Task') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Name') {
                                    tasksGridOptions.columns[i].width = 180;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Task Status') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Created') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Start Date') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'End Date') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Assignees') {
                                    tasksGridOptions.columns[i].width = 300;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Type') {
                                    tasksGridOptions.columns[i].width = 160;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Request ID') {
                                    tasksGridOptions.columns[i].width = 120;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'System Number') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                                else if (vm.TasksGrid().columns[i].title == 'Request Status') {
                                    tasksGridOptions.columns[i].width = 150;
                                    vm.TasksGrid().setOptions(tasksGridOptions);
                                }
                            }
                        }
                    });
                    for (var i = 0; i < vm.RequestsGrid().columns.length; i++) {
                        var tasksGridOptions = $.extend({}, vm.RequestsGrid().getOptions());
                        if (vm.RequestsGrid().columns[i].title == 'Name') {
                            tasksGridOptions.columns[i].width = 200;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'System Number') {
                            tasksGridOptions.columns[i].width = 90;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Date Submitted') {
                            tasksGridOptions.columns[i].width = 165;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Submitter') {
                            tasksGridOptions.columns[i].width = 100;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Status') {
                            tasksGridOptions.columns[i].width = 125;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Type') {
                            tasksGridOptions.columns[i].width = 175;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Project') {
                            tasksGridOptions.columns[i].width = 125;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Priority') {
                            tasksGridOptions.columns[i].width = 100;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Due Date') {
                            tasksGridOptions.columns[i].width = 120;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                        else if (vm.RequestsGrid().columns[i].title == 'Request ID') {
                            tasksGridOptions.columns[i].width = 120;
                            vm.RequestsGrid().setOptions(tasksGridOptions);
                        }
                    }
                    vm.RequestsGrid().bind("columnShow", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.RequestsGrid().columns.length; i++) {
                            if (vm.RequestsGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.RequestsGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.RequestsGrid().columns.length; i++) {
                            var requestGridOptions = $.extend({}, vm.RequestsGrid().getOptions());
                            if (numAvailCol < 10) {
                                if (vm.RequestsGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.RequestsGrid().table.width();
                                    requestGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.RequestsGrid().table.width();
                                    requestGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else {
                                    requestGridOptions.columns[i].hidden = true;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                            }
                            else {
                                if (vm.RequestsGrid().columns[i].title == 'Name') {
                                    requestGridOptions.columns[i].width = 200;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'System Number') {
                                    requestGridOptions.columns[i].width = 90;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Date Submitted') {
                                    requestGridOptions.columns[i].width = 165;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Submitter') {
                                    requestGridOptions.columns[i].width = 100;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Status') {
                                    requestGridOptions.columns[i].width = 125;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Type') {
                                    requestGridOptions.columns[i].width = 175;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Project') {
                                    requestGridOptions.columns[i].width = 125;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Priority') {
                                    requestGridOptions.columns[i].width = 100;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Due Date') {
                                    requestGridOptions.columns[i].width = 120;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Request ID') {
                                    requestGridOptions.columns[i].width = 120;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                            }
                        }
                    });
                    vm.RequestsGrid().bind("columnHide", function (e) {
                        var numAvailCol = 0;
                        for (var i = 0; i < vm.RequestsGrid().columns.length; i++) {
                            if (vm.RequestsGrid().columns[i].hidden == undefined) {
                                numAvailCol++;
                            }
                            else if (vm.RequestsGrid().columns[i].hidden == false) {
                                numAvailCol++;
                            }
                        }
                        for (var i = 0; i < vm.RequestsGrid().columns.length; i++) {
                            var requestGridOptions = $.extend({}, vm.RequestsGrid().getOptions());
                            if (numAvailCol < 10) {
                                if (vm.RequestsGrid().columns[i].hidden == undefined) {
                                    var totalWidth = vm.RequestsGrid().table.width();
                                    requestGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].hidden == false) {
                                    var totalWidth = vm.RequestsGrid().table.width();
                                    requestGridOptions.columns[i].width = (totalWidth / numAvailCol);
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else {
                                    requestGridOptions.columns[i].hidden = true;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                            }
                            else {
                                if (vm.RequestsGrid().columns[i].title == 'Name') {
                                    requestGridOptions.columns[i].width = 200;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'System Number') {
                                    requestGridOptions.columns[i].width = 90;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Date Submitted') {
                                    requestGridOptions.columns[i].width = 165;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Submitter') {
                                    requestGridOptions.columns[i].width = 100;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Status') {
                                    requestGridOptions.columns[i].width = 125;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Type') {
                                    requestGridOptions.columns[i].width = 175;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Project') {
                                    requestGridOptions.columns[i].width = 125;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Priority') {
                                    requestGridOptions.columns[i].width = 100;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Due Date') {
                                    requestGridOptions.columns[i].width = 120;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                                else if (vm.RequestsGrid().columns[i].title == 'Request ID') {
                                    requestGridOptions.columns[i].width = 120;
                                    vm.RequestsGrid().setOptions(requestGridOptions);
                                }
                            }
                        }
                    });
                });
            });
        }
        init();
    })(Index = Home.Index || (Home.Index = {}));
})(Home || (Home = {}));
//# sourceMappingURL=index.js.map