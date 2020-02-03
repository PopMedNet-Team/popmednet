/// <reference path="../_rootlayout.ts" />

module Home.Index {
	var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public dsRequest: kendo.data.DataSource;
        public dsTasks: kendo.data.DataSource;
        public dsMessages: kendo.data.DataSource;
        public dsNotifications: kendo.data.DataSource;
        public dsDataMarts: kendo.data.DataSource;
        public gRequestsHeight: KnockoutObservable<string>;
        public gTasksHeight: KnockoutObservable<string>;
        public gMessagesHeight: KnockoutObservable<string>;
        public gNotificationsHeight: KnockoutObservable<string>;
        public gNotificationsVisible: KnockoutObservable<boolean>;
        public gDataMartsHeight: KnockoutObservable<number>;
        public Projects: KnockoutObservableArray<Dns.ViewModels.ProjectViewModel>;
        public InvalidSelectedRequests: KnockoutComputed<Dns.Interfaces.IHomepageRequestDetailDTO[]>;
        public SelectedRequests: KnockoutObservableArray<Dns.Interfaces.IHomepageRequestDetailDTO>;
        public EnableRequestBulkEdit: KnockoutComputed<boolean>;

        public onColumnMenuInit: (e: any) => void;
        public onRequestRowSelectionChange: (e) => void;
        public onRequestBulkEdit: (data, evt) => void;
				public onClickRequestsFooter: (data, evt) => void;

        static editMetadataPermissions: Dns.Interfaces.IMetadataEditPermissionsSummaryDTO;
        private gRequestsRowSelector: any = 'multiple,row';

        constructor(projects: Dns.Interfaces.IProjectDTO[], settings: Dns.Interfaces.IUserSettingDTO[], bindingControl: JQuery) {
            super(bindingControl);
            var self = this;
            this.gRequestsHeight = ko.observable<string>(null);
            this.gTasksHeight = ko.observable<string>(null);
            this.gMessagesHeight = ko.observable<string>(null);
            this.gNotificationsHeight = ko.observable<string>(null);
            this.gDataMartsHeight = ko.observable<number>(400);
            if (ViewModel.editMetadataPermissions.CanEditRequestMetadata == false) {
                this.gRequestsRowSelector = false;
            }

            this.SelectedRequests = ko.observableArray<Dns.Interfaces.IHomepageRequestDetailDTO>();
            this.EnableRequestBulkEdit = ko.computed<boolean>(() => {
                //at least one row selected and all the rows are allowed to edit metadata
                return self.SelectedRequests().length > 0 && ko.utils.arrayFirst(self.SelectedRequests(), (rq) => rq.CanEditMetadata == false) == null;
            }, this, { pure: true });

            this.InvalidSelectedRequests = ko.computed<Dns.Interfaces.IHomepageRequestDetailDTO[]>(() => ko.utils.arrayFilter(self.SelectedRequests(), (rq) => rq.CanEditMetadata == false), this, { pure: true });

            this.Projects = ko.observableArray(projects.map((item) => {
                return new Dns.ViewModels.ProjectViewModel(item);
            }));
            this.gNotificationsVisible = ko.observable(false);
            this.dsNotifications = new kendo.data.DataSource({
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
                change: (e) => {
                    vm.gNotificationsVisible(e.items != null && e.items.length > 0);
                    vm.gNotificationsHeight(e.items != null && e.items.length > 0 ? "200px" : "34px");
                }
            });

            let gNotificationsSettings = settings.filter((item) => { return item.Key === "Home.Index.gNotifications.User:" + User.ID });
            if (gNotificationsSettings.length > 0 && gNotificationsSettings[0] !== null) {
                Global.Helpers.SetDataSourceFromSettingsWithDates(this.dsNotifications, gNotificationsSettings[0].Setting, ["Timestamp"]);
            }

            this.dsRequest = new kendo.data.DataSource({
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
                change: (e) => {
                    vm.gRequestsHeight(e.items != null && e.items.length > 0 ? "600px" : "34px");
                }
            });

            let dsRequestSettings = settings.filter((item) => { return item.Key === "Home.Index.gRequests.User:" + User.ID });
            if (dsRequestSettings.length > 0 && dsRequestSettings[0] !== null) {
                Global.Helpers.SetDataSourceFromSettingsWithDates(this.dsRequest, dsRequestSettings[0].Setting, ["SubmittedOn", "DueDate"]);
            }

            self.onRequestRowSelectionChange = (e) => {
                var selectedRequests: Dns.Interfaces.IHomepageRequestDetailDTO[] = [];

                var grid = $(e.sender.wrapper).data('kendoGrid');
                var rows = grid.select();
                
                if (rows.length > 0) {
                    for (var i = 0; i < rows.length; i++) {
                        var request: any = grid.dataItem(rows[i]);
                        selectedRequests.push(request);
                    }
                }
                self.SelectedRequests(selectedRequests);
            };

            self.onRequestBulkEdit = (data, evt) => {
                evt.stopPropagation();
                evt.preventDefault();

                if (self.SelectedRequests().length == 0 || self.InvalidSelectedRequests().length > 0)
                    return;

                var selected: any[] = $.map(self.SelectedRequests(), (r: Dns.Interfaces.IHomepageRequestDetailDTO) => r.ID);
                
                location.href = '/requests/bulkedit/?r=' + selected.join(',');
            };

            self.onClickRequestsFooter = (data, evt) => {
                var grid = self.RequestsGrid();
                grid.clearSelection();
                evt.preventDefault();
            };


            this.dsMessages = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: false,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                       url: Global.Helpers.GetServiceUrl("/networkmessages/list/"/*lastdays?days=15"*/),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelNetworkMessageDTO)
                },
                sort: { field: "CreatedOn", dir: "desc" },
                change: (e) => {
                    
                    vm.gMessagesHeight(e.items != null && e.items.length > 0 ? "200px" : "34px");
                }
            }); 

            let dsMessagesSettings = settings.filter((item) => { return item.Key === "Home.Index.gMessages.User:" + User.ID })
            if (dsMessagesSettings.length > 0 && dsMessagesSettings[0] !== null) {
                Global.Helpers.SetDataSourceFromSettingsWithDates(this.dsMessages, dsMessagesSettings[0].Setting, ["CreatedOn"]);
            }

            this.dsTasks = new kendo.data.DataSource({
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
                change: (e) => {
                  vm.gTasksHeight(e.items != null && e.items.length > 0 ? "300px" : "34px");
                }
            });

            let dsTasksSettings = settings.filter((item) => { return item.Key === "Home.Index.gTasks.User:" + User.ID });
            if (dsTasksSettings.length > 0 && dsTasksSettings[0] !== null) {
                Global.Helpers.SetDataSourceFromSettingsWithDates(this.dsTasks, dsTasksSettings[0].Setting, ["CreatedOn", "StartOn", "EndOn"]);
            }

            let now = moment().add(5, 'days');
            let userdate = moment(User.PasswordExpiration);
            if (userdate <= now)
                Global.Helpers.ShowToast("Your Password is Expiring soon.  Please update your password.");

            this.dsDataMarts = new kendo.data.DataSource({
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

            let dsDataMartsSettings = settings.filter((item) => { return item.Key === "Home.Index.gDataMarts.User:" + User.ID });
            if (dsDataMartsSettings.length > 0 && dsDataMartsSettings[0] !== null) {
                Global.Helpers.SetDataSourceFromSettings(this.dsDataMarts, dsDataMartsSettings[0].Setting);
            }

        }

        public RequestsGrid(): kendo.ui.Grid {
            return $("#gRequests").data("kendoGrid");
        }

        public TasksGrid(): kendo.ui.Grid {
            return $("#gTasks").data("kendoGrid");
        }

        public NotificationsGrid(): kendo.ui.Grid {
            return $("#gNotifications").data("kendoGrid");
        }

        public MessagesGrid(): kendo.ui.Grid {
            return $("#gMessages").data("kendoGrid");
        }

        public DataMartsGrid(): kendo.ui.Grid {
            return $("#gDataMarts").data("kendoGrid");
        }

        public CreateRequest(project: Dns.ViewModels.ProjectViewModel) {
            Global.Helpers.ShowDialog("Choose Request Type", "/requests/createdialog", ["Close"], 400, 600, { ProjectID: project.ID() }).done((result: Dns.Interfaces.IRequestTypeDTO) => {
                if (!result)
                    return;
                var url;
                if (!result.TemplateID && !result.WorkflowID) {
                    // Legacy Non-workflow request types
                    url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + project.ID();
                } else if (!result.TemplateID) {
                    // Workflow based non-QueryComposer request types
                    url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + project.ID() + "&WorkflowID=" + result.WorkflowID;
                } else {
                    // QueryComposer request types
                    url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + project.ID() + "&templateID=" + result.TemplateID + "&WorkflowID=" + result.WorkflowID;
                }
                window.location.href = url;
            });
        }

        public FormatTaskName(e: Dns.Interfaces.IHomepageTaskSummaryDTO) {
            if (e.DirectToRequest) {
                return '<a href="/requests/details?ID=' + e.RequestID + '&taskID=' + e.TaskID + '">' + e.TaskName + '</a>';
            }
           
            return '<a href="/tasks/details?ID=' + e.TaskID + '">' + e.TaskName + '</a>';
        }

        public FormatNameForTask(e: Dns.Interfaces.IHomepageTaskSummaryDTO) {
            if (e.DirectToRequest) {
                return '<a href="/requests/details?ID=' + e.RequestID + '&taskID=' + e.TaskID + '">' + e.Name + '</a>';
            }
            if (e.NewUserID != null) {
                return '<a href="/users/details?ID=' + e.NewUserID +'">' + e.Name + '</a>';
            }
            return '<a href="/tasks/details?ID=' + e.TaskID + '">' + e.Name + '</a>';
        }

        public FormatAssignedResources(e: Dns.Interfaces.IHomepageTaskSummaryDTO) {
            return decodeHtml(e.AssignedResources);
        }

        public onDataMartsDetailInit(e: any) {
            var datamart = <Dns.Interfaces.IDataMartListDTO>e.data;

            var canEditAnyMetadata = ko.utils.arrayFirst(ViewModel.editMetadataPermissions.EditableDataMarts, (id) => datamart.ID == id) != null;

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
                columnMenuInit: (e) => {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", (e) => {

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
                change: (e) => {                    
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

                            var invalidRequests: Dns.Interfaces.IHomepageRouteDetailDTO[] = [];

                            for (var i = 0; i < rows.length; i++) {
                                var request: any = grd.dataItem(rows[i]);
                                if (request.CanEditMetadata == false) {
                                    invalidRequests.push(request);
                                }
                            }                            

                            viewmodel.InvalidSelectedRequests = invalidRequests;
                        }

                        if (rows.length == 0 || viewmodel.InvalidSelectedRequests.length > 0) {
                            btn.attr('disabled', '');
                        } else {
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
                bulkEditButton.click((evt) => {
                    evt.stopPropagation();
                    evt.preventDefault();

                    var grd = gd;
                    var rows = grd.select();

                    if (rows.length == 0)
                        return;

                    var selected: any[] = $.map(rows, (r: any) => {
                        var route: any = grd.dataItem(r);
                        return route.RequestDataMartID;
                    });

                    location.href = '/requests/bulkeditroutes/?r=' + selected.join(',');
                });


                var footer = $('<div class="panel-footer">');
                footer.attr('id', 'dmfooter-' + datamart.ID);
                footer.click((e) => {
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
        }
    }

    export function decodeHtml(value): string {
        var txt = document.createElement('textarea');
        txt.innerHTML = value;
        return txt.value;
    }

    export function NameAnchor(dataItem: Dns.Interfaces.IHomepageRequestDetailDTO): string {
        if (dataItem.IsWorkflowRequest) {
            return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        } else {
            return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
    }

    export function SubjectAnchor(dataItem: Dns.Interfaces.ITaskDTO): string {
        return "<a href=\"/tasks/details?ID=" + dataItem.ID + "\">" + dataItem.Subject + "</a>";
    }

    function init() {
	    $.when<any>(
		    Dns.WebApi.Projects.RequestableProjects(null, "ID,Name", "Name"),
		    Users.GetSettings([
					"Home.Index.gRequests.User:" + User.ID,
			    "Home.Index.gTasks.User:" + User.ID,
					"Home.Index.gNotifications.User:" + User.ID,
			    "Home.Index.gMessages.User:" + User.ID,
					"Home.Index.gDataMarts.User:" + User.ID,
			    "Home.Index.gDataMartsRoutes.User:" + User.ID
		    ]),
        Dns.WebApi.Users.GetMetadataEditPermissionsSummary()
      ).done((projects, settings, editMetadataPermissions: Dns.Interfaces.IMetadataEditPermissionsSummaryDTO[]) => {
            $(() => {
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
            });
        });
    }

    init();
}