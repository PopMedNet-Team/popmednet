import * as Global from "../../scripts/page/global.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';
import { UserSettingHelper } from "../_RootLayout.js";
import * as Interfaces from "../Dns.Interfaces.js";
import { ProjectViewModel } from "../Lpp.Dns.ViewModels.js";
import * as Constants from "../../scripts/page/constants.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";
import * as Enums from "../Dns.Enums.js";

export default class ViewModel extends Global.PageViewModel {
    public dsRequest: kendo.data.DataSource;
    public requestSetting: Dns.Interfaces.IUserSettingDTO;
    public dsTasks: PMNGridDataSource<typeof Interfaces.KendoModelNotificationDTO>;
    public taskSetting: Dns.Interfaces.IUserSettingDTO;
    public dsMessages: PMNGridDataSource<typeof Interfaces.KendoModelNetworkMessageDTO>;
    public messageSetting: Interfaces.IUserSettingDTO;
    public dsNotifications: PMNGridDataSource<typeof Interfaces.KendoModelNotificationDTO>;
    public notificationSetting: Dns.Interfaces.IUserSettingDTO;
    public dsDataMarts: PMNGridDataSource<typeof Interfaces.KendoModelDataMartListDTO>;
    public dataMartSetting: Dns.Interfaces.IUserSettingDTO;
    public gRequestsHeight: KnockoutObservable<string>;
    public gTasksHeight: KnockoutObservable<string>;
    public gMessagesHeight: KnockoutObservable<string>;
    public gNotificationsHeight: KnockoutObservable<string>;
    public gDataMartsHeight: KnockoutObservable<string>;
    public Projects: KnockoutObservableArray<ProjectViewModel>;
    public InvalidSelectedRequests: KnockoutComputed<Interfaces.IHomepageRequestDetailDTO[]>;
    public SelectedRequests: KnockoutObservableArray<Interfaces.IHomepageRequestDetailDTO>;
    public EnableRequestBulkEdit: KnockoutComputed<boolean>;

    public onRequestRowSelectionChange: (e) => void;
    public onRequestBulkEdit: (data, evt) => void;
    public onClickRequestsFooter: (data, evt) => void;

    public editMetadataPermissions: Interfaces.IMetadataEditPermissionsSummaryDTO = { CanEditRequestMetadata: false, EditableDataMarts: [] };
    public gRequestsRowSelector: any = 'multiple,row';

    public readonly DateTimeFormaterString: string = Constants.DateTimeFormatter;
    public readonly PrioritiesTranslation = Enums.PrioritiesTranslation;


    constructor(projects: Interfaces.IProjectDTO[], settings: Interfaces.IUserSettingDTO[], bindingControl: JQuery, editMetadataPermissions: Interfaces.IMetadataEditPermissionsSummaryDTO) {
        super(bindingControl);
        let self = this;
        this.gRequestsHeight = ko.observable<string>(null);
        this.gTasksHeight = ko.observable<string>(null);
        this.gMessagesHeight = ko.observable<string>(null);
        this.gNotificationsHeight = ko.observable<string>("200px");
        this.gDataMartsHeight = ko.observable<string>(null);

        if (editMetadataPermissions) {
            this.editMetadataPermissions = editMetadataPermissions;
        }

        if (this.editMetadataPermissions.CanEditRequestMetadata == false) {
            this.gRequestsRowSelector = false;
        }

        this.SelectedRequests = ko.observableArray<Interfaces.IHomepageRequestDetailDTO>();
        this.EnableRequestBulkEdit = ko.computed<boolean>(() => {
            //at least one row selected and all the rows are allowed to edit metadata
            return self.SelectedRequests().length > 0 && ko.utils.arrayFirst(self.SelectedRequests(), (rq) => rq.CanEditMetadata == false) == null;
        }, this, { pure: true });

        this.InvalidSelectedRequests = ko.computed<Interfaces.IHomepageRequestDetailDTO[]>(() => ko.utils.arrayFilter(self.SelectedRequests(), (rq) => rq.CanEditMetadata == false), this, { pure: true });

        this.Projects = ko.observableArray(projects.map((item) => {
            return new ProjectViewModel(item);
        }));

        let gNotificationsSettings = settings.filter((item) => { return item.Key === "Home.Index.gNotifications.User:" + Global.User.ID });
        this.notificationSetting = (gNotificationsSettings.length > 0 && gNotificationsSettings[0] !== null) ? gNotificationsSettings[0] : null;

        this.dsNotifications = new PMNGridDataSource<typeof Interfaces.KendoModelNotificationDTO>(Global.Helpers.GetServiceUrl("/users/getnotifications?UserID=" + Global.User.ID), { field: "Timestamp", dir: "desc" }, (e) => {
            self.gNotificationsHeight(e.response != null && e.response.Results != null && e.response.Results.length > 0 ? "200px" : "34px");
        }, Interfaces.KendoModelNotificationDTO);


        let dsRequestSettings = settings.filter((item) => { return item.Key === "Home.Index.gRequests.User:" + Global.User.ID });
        this.requestSetting = (dsRequestSettings.length > 0 && dsRequestSettings[0] !== null) ? dsRequestSettings[0] : null;

        this.dsRequest = new PMNGridDataSource<typeof Interfaces.KendoModelHomepageRequestDetailDTO>(Global.Helpers.GetServiceUrl("/requests/listforhomepage"), { field: "SubmittedOn", dir: "desc" }, (e) => {
            this.gRequestsHeight(e.response != null && e.response.Results != null && e.response.Results.length > 0 ? "600px" : "34px");
        }, Interfaces.KendoModelHomepageRequestDetailDTO);

        self.onRequestRowSelectionChange = (e) => {
            let selectedRequests: Interfaces.IHomepageRequestDetailDTO[] = [];

            let grid = $(e.sender.wrapper).data('kendoGrid');
            let rows = grid.select();

            if (rows.length > 0) {
                for (let i = 0; i < rows.length; i++) {
                    let request: any = grid.dataItem(rows[i]);
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

            let selected: any[] = $.map(self.SelectedRequests(), (r: Interfaces.IHomepageRequestDetailDTO) => r.ID);

            location.href = '/requests/bulkedit/?r=' + selected.join(',');
        };

        self.onClickRequestsFooter = (data, evt) => {
            let grid = self.RequestsGrid();
            grid.clearSelection();
            evt.preventDefault();
        };



        let dsMessagesSettings = settings.filter((item) => { return item.Key === "Home.Index.gMessages.User:" + Global.User.ID });
        this.messageSetting = (dsMessagesSettings.length > 0 && dsMessagesSettings[0] !== null) ? dsMessagesSettings[0] : null;

        this.dsMessages = new PMNGridDataSource<typeof Interfaces.KendoModelNetworkMessageDTO>(Global.Helpers.GetServiceUrl("/networkmessages/list"), { field: "CreatedOn", dir: "desc" }, (e) => {
            this.gMessagesHeight(e.response != null && e.response.Results != null && e.response.Results.length > 0 ? "200px" : "34px");
        }, Interfaces.KendoModelNetworkMessageDTO);


        let dsTasksSettings = settings.filter((item) => { return item.Key === "Home.Index.gTasks.User:" + Global.User.ID });
        this.taskSetting = (dsTasksSettings.length > 0 && dsTasksSettings[0] !== null) ? dsTasksSettings[0] : null;

        this.dsTasks = new PMNGridDataSource<typeof Interfaces.KendoModelHomepageTaskSummaryDTO>(Global.Helpers.GetServiceUrl("/users/GetWorkflowTasks?UserID=" + Global.User.ID), { field: "CreatedOn", dir: "desc" }, (e) => {
            this.gTasksHeight(e.response != null && e.response.Results != null && e.response.Results.length > 0 ? "300px" : "34px");
        }, Interfaces.KendoModelHomepageTaskSummaryDTO);

        
        let now = kendo.date.addDays(kendo.date.today(), 5);
        let userdate = kendo.date.getDate(kendo.parseDate(Global.User.PasswordExpiration));
        if (userdate <= now)
            Global.Helpers.ShowToast("Your Password is Expiring soon.  Please update your password.");

        let dsDataMartsSettings = settings.filter((item) => { return item.Key === "Home.Index.gDataMarts.User:" + Global.User.ID });
        this.dataMartSetting = (dsDataMartsSettings.length > 0 && dsDataMartsSettings[0] !== null) ? dsDataMartsSettings[0] : null;

        this.dsDataMarts = new PMNGridDataSource<typeof Interfaces.KendoModelDataMartListDTO>(Global.Helpers.GetServiceUrl("/datamarts/list"), { field: "Name", dir: "asc" }, (e) => {
            this.gDataMartsHeight(e.response != null && e.response.Results != null && e.response.Results.length > 0 ? "800px" : "34px");
        }, Interfaces.KendoModelDataMartListDTO);

    }

    public RequestsGrid(): kendo.ui.Grid {
        return $("#gRequests").data("kendoGrid");
    }

    public TasksGrid(): kendo.ui.Grid {
        return $("#gTasks").data("kendoGrid");
    }

    public MessagesGrid(): kendo.ui.Grid {
        return $("#gMessages").data("kendoGrid");
    }

    public DataMartsGrid(): kendo.ui.Grid {
        return $("#gDataMarts").data("kendoGrid");
    }

    public CreateRequest(project: ProjectViewModel) {
        Global.Helpers.ShowDialog("Choose Request Type", "/requests/createdialog", ["Close"], 400, 600, { ProjectID: project.ID() }).done((result: Interfaces.IRequestTypeDTO) => {
            if (!result)
                return;
            var url;
            if (!result.WorkflowID) {
                // Legacy Non-workflow request types
                url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + project.ID();
            } else {
                // Workflow based non-QueryComposer request types
                url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + project.ID() + "&WorkflowID=" + result.WorkflowID;
            }
            window.location.href = url;
        });
    }

    public FormatTaskName(e: Interfaces.IHomepageTaskSummaryDTO) {
        if (e.DirectToRequest) {
            return '<a href="/requests/details?ID=' + e.RequestID + '&taskID=' + e.TaskID + '">' + e.TaskName + '</a>';
        }

        return '<a href="/tasks/details?ID=' + e.TaskID + '">' + e.TaskName + '</a>';
    }

    public FormatNameForTask(e: Interfaces.IHomepageTaskSummaryDTO) {
        if (e.DirectToRequest) {
            return '<a href="/requests/details?ID=' + e.RequestID + '&taskID=' + e.TaskID + '">' + e.Name + '</a>';
        }
        if (e.NewUserID != null) {
            return '<a href="/users/details?ID=' + e.NewUserID + '">' + e.Name + '</a>';
        }
        return '<a href="/tasks/details?ID=' + e.TaskID + '">' + e.Name + '</a>';
    }

    public FormatAssignedResources(e: Interfaces.IHomepageTaskSummaryDTO) {
        return decodeHtml(e.AssignedResources);
    }

    public NameAnchor(dataItem: Interfaces.IHomepageRequestDetailDTO): string {
        if (dataItem.IsWorkflowRequest) {
            return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        } else {
            return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
    }

    public DueDateTemplate(dataItem: Dns.Interfaces.IRequestDTO): string {
        return dataItem.DueDate ? kendo.format("MM/DD/YYYY", dataItem.DueDate) : "";
    }

    public onDataMartsDetailInit(e: any) {
        let datamart = <Interfaces.IDataMartListDTO>e.data;

        let canEditAnyMetadata = ko.utils.arrayFirst(this.editMetadataPermissions.EditableDataMarts, (id) => datamart.ID == id) != null;

        let grid = $('<div style="min-height:155px;"/>').kendoGrid({
            sortable: true,
            filterable: Global.Helpers.GetColumnFilterOperatorDefaults(),
            autoBind: true,
            resizable: true,
            reorderable: true,
            pageable: false,
            groupable: false,
            scrollable: {
                virtual: true
            },
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
                { field: 'Priority', title: 'Priority', values: Enums.PrioritiesTranslation, width: 100 },
                { field: 'DueDate', title: 'Due Date', format: Constants.DateFormatter, width: 120 },
                { field: 'MSRequestID', title: 'Request ID', width: 120 },
            ],
            change: (e) => {
                let grd = $(e.sender.wrapper).data('kendoGrid');
                let rows = grd.select();
                let btn = $('#dmbulkedit-' + datamart.ID);
                if (btn) {
                    let container = $('#dmfooter-errorcontainer-' + datamart.ID);
                    ko.cleanNode(container[0]);

                    let viewmodel = {
                        InvalidSelectedRequests: []
                    };

                    if (rows.length > 0) {

                        let invalidRequests: Interfaces.IHomepageRouteDetailDTO[] = [];

                        for (let i = 0; i < rows.length; i++) {
                            let request: any = grd.dataItem(rows[i]);
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

        let datasource = new PMNGridDataSource<typeof Interfaces.KendoModelHomepageRouteDetailDTO>(Global.Helpers.GetServiceUrl("requests/requestsbyroute?id=" + datamart.ID), { field: "Identifier", dir: "desc" }, (e) => {
            //this.gRequestsHeight(e.response != null && e.response.Results != null && e.response.Results.length > 0 ? "600px" : "34px");
        }, Interfaces.KendoModelHomepageRouteDetailDTO);

        let gd = grid.data('kendoGrid');
        gd.setDataSource(datasource);

        let panel = $('<div class="panel panel-default">');

        let gridContainer = $('<div class="panel-body" style="height:400px;width:990px;overflow:auto;overflow-y:hidden;padding:0px;position:relative;"></div>');
        $(grid).appendTo(gridContainer);
        $(gridContainer).appendTo(panel);

        if (canEditAnyMetadata) {
            let bulkEditButton = $('<button disabled="" class="btn btn-default">Edit Requests</button>');
            bulkEditButton.attr('id', 'dmbulkedit-' + datamart.ID);
            bulkEditButton.click((evt) => {
                evt.stopPropagation();
                evt.preventDefault();

                let grd = gd;
                let rows = grd.select();

                if (rows.length == 0)
                    return;

                let selected: any[] = $.map(rows, (r: any) => {
                    let route: any = grd.dataItem(r);
                    return route.RequestDataMartID;
                });

                location.href = '/requests/bulkeditroutes/?r=' + selected.join(',');
            });


            let footer = $('<div class="panel-footer">');
            footer.attr('id', 'dmfooter-' + datamart.ID);
            footer.click((e) => {
                let grid = gd;
                gd.clearSelection();
                e.preventDefault();

                let container = $('#dmfooter-errorcontainer-' + datamart.ID);
                ko.cleanNode(container[0]);

                let viewmodel = {
                    InvalidSelectedRequests: []
                };
                ko.applyBindings(viewmodel, container[0]);
            });

            let footerrow = $('<div class="row">');
            footerrow.appendTo(footer);

            let footerbuttoncell = $('<div class="col-xs-2">');
            footerbuttoncell.appendTo(footerrow);
            bulkEditButton.appendTo(footerbuttoncell);

            let footererrorcell = $('<div class="col-xs-9">');
            footererrorcell.appendTo(footerrow);

            $('<div id="dmfooter-errorcontainer-' + datamart.ID + '" data-bind="template: { name: \'invalid-requests-template\'}">').appendTo(footererrorcell);

            $(footer).appendTo(panel);
        }

        $(panel).appendTo(e.detailCell);
        kendo.ui.progress(grid, true);
    }
}

export function decodeHtml(value): string {
    let txt = document.createElement('textarea');
    txt.innerHTML = value;
    return txt.value;
}

//export function SubjectAnchor(dataItem: Dns.Interfaces.ITaskDTO): string {
//    return "<a href=\"/tasks/details?ID=" + dataItem.ID + "\">" + dataItem.Subject + "</a>";
//}



$.when<any>(
    WebApi.Projects.RequestableProjects(null, "ID,Name", "Name"),
    UserSettingHelper.GetSettings([
        "Home.Index.gRequests.User:" + Global.User.ID,
        "Home.Index.gTasks.User:" + Global.User.ID,
        "Home.Index.gNotifications.User:" + Global.User.ID,
        "Home.Index.gMessages.User:" + Global.User.ID,
        "Home.Index.gDataMarts.User:" + Global.User.ID,
        "Home.Index.gDataMartsRoutes.User:" + Global.User.ID
    ]),
    WebApi.Users.GetMetadataEditPermissionsSummary()
).done((projects, settings, editMetadataPermissions) => {
    $(() => {
        let bindingControl = $("#Content");

        let vm = new ViewModel(projects, settings, bindingControl, editMetadataPermissions);
        ko.applyBindings(vm, bindingControl[0]);

    });
}).then(() => {
    $('#PageLoadingMessage').remove();
});