var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Home;
(function (Home) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(projects, gRequestsSetting, gTasksSetting, gNotificationsSetting, gMessagesSetting, gDataMartsSetting, gDataMartsRoutesSetting, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.gRequestsSetting = null;
                _this.gDataMartsRoutesSetting = null;
                _this.gRequestsRowSelector = 'multiple,row';
                var self = _this;
                _this.gRequestsHeight = ko.observable(null);
                _this.gTasksHeight = ko.observable(null);
                _this.gMessagesHeight = ko.observable(null);
                _this.gNotificationsHeight = ko.observable(null);
                _this.gDataMartsHeight = ko.observable(400);
                _this.gRequestsSetting = gRequestsSetting;
                _this.gDataMartsRoutesSetting = gDataMartsRoutesSetting;
                if (ViewModel.editMetadataPermissions.CanEditRequestMetadata == false) {
                    _this.gRequestsRowSelector = false;
                }
                _this.SelectedRequests = ko.observableArray();
                _this.EnableRequestBulkEdit = ko.computed(function () {
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
                Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsNotifications, gNotificationsSetting, ["Timestamp"]);
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
                Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsRequest, gRequestsSetting, ["SubmittedOn", "DueDate"]);
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
                            url: Global.Helpers.GetServiceUrl("/networkmessages/list/"),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelNetworkMessageDTO)
                    },
                    sort: { field: "CreatedOn", dir: "desc" },
                    change: function (e) {
                        vm.gMessagesHeight(e.items != null && e.items.length > 0 ? "200px" : "34px");
                        console.log(vm.gMessagesHeight());
                    }
                });
                Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsMessages, gMessagesSetting, ["CreatedOn"]);
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
                Global.Helpers.SetDataSourceFromSettingsWithDates(_this.dsTasks, gTasksSetting, ["CreatedOn", "StartOn", "EndOn"]);
                var now = moment().add(5, 'days');
                var userdate = moment(User.PasswordExpiration);
                if (userdate <= now)
                    Global.Helpers.ShowToast("Your Password is Expiring soon.  Please update your password.");
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.Save();
                    });
                };
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
                Global.Helpers.SetDataSourceFromSettings(_this.dsDataMarts, gDataMartsSetting);
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
                        url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + project.ID();
                    }
                    else if (!result.TemplateID) {
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + project.ID() + "&WorkflowID=" + result.WorkflowID;
                    }
                    else {
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + project.ID() + "&templateID=" + result.TemplateID + "&WorkflowID=" + result.WorkflowID;
                    }
                    window.location.href = url;
                });
            };
            ViewModel.prototype.Save = function () {
                Users.SetSetting("Home.Index.gRequests.User:" + User.ID, Global.Helpers.GetGridSettings(this.RequestsGrid()));
                Users.SetSetting("Home.Index.gTasks.User:" + User.ID, Global.Helpers.GetGridSettings(this.TasksGrid()));
                Users.SetSetting("Home.Index.gNotifications.User:" + User.ID, Global.Helpers.GetGridSettings(this.NotificationsGrid()));
                Users.SetSetting("Home.Index.gMessages.User:" + User.ID, Global.Helpers.GetGridSettings(this.MessagesGrid()));
                Users.SetSetting("Home.Index.gDataMarts.User:" + User.ID, Global.Helpers.GetGridSettings(this.DataMartsGrid()));
                Users.SetSetting("Home.Index.gDataMartsRoutes.User:" + User.ID, this.gDataMartsRoutesSetting);
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
                var _this = this;
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
                            vm.gDataMartsRoutesSetting = Global.Helpers.GetGridSettings(grid.data('kendoGrid'));
                            Users.SetSetting("Home.Index.gDataMartsRoutes.User:" + User.ID, _this.gDataMartsRoutesSetting);
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
                Global.Helpers.SetGridFromSettings(grid.data('kendoGrid'), vm.gDataMartsRoutesSetting);
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
                Global.Helpers.SetDataSourceFromSettings(datasource, vm.gDataMartsRoutesSetting);
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
        function init() {
            $.when(Dns.WebApi.Projects.RequestableProjects(null, "ID,Name", "Name"), Users.GetSetting("Home.Index.gRequests.User:" + User.ID), Users.GetSetting("Home.Index.gTasks.User:" + User.ID), Users.GetSetting("Home.Index.gNotifications.User:" + User.ID), Users.GetSetting("Home.Index.gMessages.User:" + User.ID), Users.GetSetting("Home.Index.gDataMarts.User:" + User.ID), Users.GetSetting("Home.Index.gDataMartsRoutes.User:" + User.ID), Dns.WebApi.Users.GetMetadataEditPermissionsSummary()).done(function (projects, gRequestsSetting, gTasksSetting, gNotificationsSetting, gMessagesSetting, gDataMartsSetting, gDataMartsRoutesSetting, editMetadataPermissions) {
                $(function () {
                    var bindingControl = $("#Content");
                    ViewModel.editMetadataPermissions = editMetadataPermissions.length > 0 ? editMetadataPermissions[0] : { CanEditRequestMetadata: false, EditableDataMarts: [] };
                    vm = new ViewModel(projects, gRequestsSetting, gTasksSetting, gNotificationsSetting, gMessagesSetting, gDataMartsSetting, gDataMartsRoutesSetting, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () { return vm.Save(); });
                    Global.Helpers.SetGridFromSettings(vm.RequestsGrid(), gRequestsSetting);
                    Global.Helpers.SetGridFromSettings(vm.TasksGrid(), gTasksSetting);
                    Global.Helpers.SetGridFromSettings(vm.NotificationsGrid(), gNotificationsSetting);
                    Global.Helpers.SetGridFromSettings(vm.MessagesGrid(), gMessagesSetting);
                    Global.Helpers.SetGridFromSettings(vm.DataMartsGrid(), gDataMartsSetting);
                });
            });
        }
        init();
    })(Index = Home.Index || (Home.Index = {}));
})(Home || (Home = {}));
