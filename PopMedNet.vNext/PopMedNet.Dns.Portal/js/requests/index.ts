import * as Global from "../../scripts/page/global.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';
import { UserSettingHelper } from "../_RootLayout.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as ViewModels from "../Lpp.Dns.ViewModels.js";
import * as Constants from "../../scripts/page/constants.js";
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";
import * as Enums from "../Dns.Enums.js";

export class ViewModel extends Global.PageViewModel {
    public SelectedProjectID: KnockoutObservable<any>;
    public dataSource: PMNGridDataSource<typeof Interfaces.KendoModelRequestDTO>;
    public Projects: KnockoutObservableArray<ViewModels.ProjectViewModel>;
    public RequestableProjects: Interfaces.IProjectDTO[];
    public dataSourceSetting: Interfaces.IUserSettingDTO;
    public PrioritiesTranslation = Enums.PrioritiesTranslation;
    public DateTimeFormatter = Constants.DateTimeFormatter;

    constructor(gResultsSettings: Interfaces.IUserSettingDTO[], bindingControl: JQuery, projects: Interfaces.IProjectDTO[], projectID: any, requestableProjecs: Interfaces.IProjectDTO[]) {
        super(bindingControl);

        //projects are the ones the user is able to see
        this.Projects = ko.observableArray(projects.map((project) => {
            return new ViewModels.ProjectViewModel(project);
        }));

        this.SelectedProjectID = ko.observable(projectID);

        //requestable project are the ones that the user can create new requests for.
        this.RequestableProjects = requestableProjecs;


        let dsRequestSettings = gResultsSettings.filter((item) => { return item.Key === "Requests.Index.gResults.User:" + Global.User.ID });
        this.dataSourceSetting = null;
        if (dsRequestSettings.length > 0 && dsRequestSettings[0] !== null) {
            this.dataSourceSetting = dsRequestSettings[0];

            //need to make sure the current filter for project ID matches what is specified in the query string
            let gridSetting = JSON.parse(this.dataSourceSetting.Setting) as Global.IKendoGridSettings;

            let filters = gridSetting.filter.filters;
            let item = ko.utils.arrayFirst(filters, (item) => (<kendo.data.DataSourceFilterItem>item).field.toLowerCase() == "projectid") as kendo.data.DataSourceFilterItem;
            if (item == null && !Constants.Guid.equals(projectID, Constants.GuidEmpty)) {
                //add the project id
                filters.push({ field: "ProjectID", operator: "equals", value: projectID });
            } else if (item != null) {
                if (Constants.Guid.equals(projectID, Constants.GuidEmpty)) {
                    ko.utils.arrayRemoveItem(filters, item);
                } else {
                    if (item.value != null && !Constants.Guid.equals(projectID, item.value)) {
                        item.value = projectID;
                    }
                }
            }

            this.dataSourceSetting.Setting = JSON.stringify(gridSetting);

        }
        

        this.dataSource = new PMNGridDataSource<typeof Interfaces.KendoModelRequestDTO>(Global.Helpers.GetServiceUrl("/requests/list?$select=ID,Name,Identifier,MajorEventDate,MajorEventBy,CreatedBy,SubmittedOn,SubmittedByName,Status,StatusText,RequestType,Project,Priority,DueDate,MSRequestID,CurrentWorkFlowActivityID"), Global.Helpers.GetSortsFromUrl() || { field: "SubmittedOn", dir: "desc" }, null, Interfaces.KendoModelRequestDTO);
        
    }

    public SelectProject(project: ViewModels.ProjectViewModel) {
        this.SelectedProjectID(project.ID());

        //PMNDEV-5057 - We need to keep the filters intact on project change, otherwise they are not retained.
        let filter = this.createEmptyFilter();

        let originalFilters: kendo.data.DataSourceFilters = this.dataSource.filter();
        if (originalFilters != undefined || originalFilters != null) {
            originalFilters.filters.forEach((item: kendo.data.DataSourceFilterItem) => {
                if (item.field != "ProjectID") {
                    filter.filters.push(item);
                }
            });
        }

        if (project.ID() == Constants.GuidEmpty) {
            //filter.filters.push({ field: "ProjectID", operator: "notequals", value: Constants.GuidEmpty });
            //going to leave the filter empty instead of adding the bogus predicated of projectID != Guid.Empty.
            //just update the url to reflect the selected project.
            window.history.pushState('','',"/requests/");
        } else {
            //update the grid filter based on the selected project ID and update the url to reflect the selected project.
            filter.filters.push({ field: "ProjectID", operator: "equals", value: project.ID() });
            window.history.pushState('','',"/requests?projectID=" + project.ID());
        }

        this.dataSource.filter(filter.filters);

        return false;
    }

    private createEmptyFilter() {
        return { logic: "and", filters: [] };
    }

    public AddClearAllFiltersMenuItem(e: any): void {
        let popup = e.container.data('kendoPopup');
        let menu = e.container.find(".k-menu").data("kendoMenu");
        let grid: kendo.ui.Grid = e.sender;
        menu.append({ text: "Clear All Filters", spriteCssClass: 'k-i-filter-clear' });
        menu.bind("select", function (e) {
            if ($(e.item).text() == "Clear All Filters") {
                //First Clear the Filter of the grid, then close the menu, then Popup.  Must be done in this order.  See https://www.telerik.com/forums/close-menu-on-custom-column-menu-item
                if (this.SelectedProjectID() != Constants.GuidEmpty) {
                    grid.dataSource.filter({ field: "ProjectID", operator: "equals", value: this.SelectedProjectID() });
                }

                menu.close();
                popup.close();
            }
        });
    }

    public ResultsGrid(): kendo.ui.Grid {
        return $("#gResults").data("kendoGrid");
    }

    public onCreateRequest(proj: Interfaces.IProjectDTO) {
        let projectID = proj.ID;
        this.SelectedProjectID(proj.ID);
        Global.Helpers.ShowDialog("Choose Request Type", '/requests/createdialog', ["Close"], 400, 600, { ProjectID: projectID }).done((result: Interfaces.IRequestTypeDTO) => {
            if (!result)
                return;

            let url;
            //if (!result.TemplateID && !result.WorkflowID) {
            //    // Legacy Non-workflow request types
            //    url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + projectID;
            //} else if (!result.TemplateID) {
            //    // Workflow based non-QueryComposer request types
            //    url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&WorkflowID=" + result.WorkflowID;
            //} else {
            //    // QueryComposer request types
            //    url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&templateID=" + result.TemplateID + "&WorkflowID=" + result.WorkflowID;
            //}

            if (!result.WorkflowID) {
                //Legacy Non-workflow request types
                url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + projectID;
            } else {
                url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&WorkflowID=" + result.WorkflowID;
            }
            window.location.href = url;
        });
    }

    public NameAchor(dataItem: Interfaces.IRequestDTO): string {
        if (dataItem.CurrentWorkFlowActivityID) {
            return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        } else {
            return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
    }

    public DueDateTemplate(dataItem: Interfaces.IRequestDTO): string {
        
        return dataItem.DueDate ? kendo.format("MM/DD/YYYY", dataItem.DueDate) : "";
    }
}

$.when<any>(UserSettingHelper.GetSettings(["Requests.Index.gResults.User:" + Global.User.ID]),
    WebApi.Users.ListAvailableProjects(null, "ID, Name", "Name"),
    WebApi.Projects.RequestableProjects(null, "ID,Name", "Name")
).done((gResultsSettings, projects, requestableProjects) => {
    $(() => {
        projects.unshift(<any>{ ID: Constants.GuidEmpty, Name: "All Projects" });

        let bindingControl = $("#Content");
        let projectID = Global.GetQueryParam("projectid");
        if (!projectID)
            projectID = Constants.GuidEmpty;

        let vm = new ViewModel(gResultsSettings, bindingControl, projects, projectID, requestableProjects);
        ko.applyBindings(vm, bindingControl[0]);
    });
});