/// <reference path="../../Scripts/page/Page.ts" /> 

module Requests.Index {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public SelectedProjectID: KnockoutObservable<any>;
        public dataSource: kendo.data.DataSource;
        public Projects: KnockoutObservableArray<Dns.ViewModels.ProjectViewModel>;
        public RequestableProjects: Dns.Interfaces.IProjectDTO[];

        constructor(gResultsSettings: string, bindingControl: JQuery, projects: Dns.Interfaces.IProjectDTO[], projectID: any, requestableProjecs: Dns.Interfaces.IProjectDTO[]) {
            super(bindingControl);
            var self = this;

            //projects are the ones the user is able to see
            this.Projects = ko.observableArray(projects.map((project) => {
                return new Dns.ViewModels.ProjectViewModel(project);
            }));

            this.SelectedProjectID = ko.observable(projectID);

            //requestable project are the ones that the user can create new requests for.
            this.RequestableProjects = requestableProjecs;

            this.dataSource = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 100,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/requests/list?$select=ID,Name,Identifier,MajorEventDate,MajorEventBy,CreatedBy,SubmittedOn,SubmittedByName,Status,StatusText,RequestType,Project,Priority,DueDate,MSRequestID,CurrentWorkFlowActivityID"),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelRequestDTO)
                },
                sort: Global.Helpers.GetSortsFromUrl() || { field: "SubmittedOn", dir: "desc" },
            });

            // Save the reference to the original filter function.
            var originalFilter = this.dataSource.filter;
  
            // Replace the original filter function.
            this.dataSource.filter = function () {
                //
                if (arguments.length > 0) {
                    //Saving filter chnages right back to the DB.  
                  Users.SetSetting("Requests.Index.gResults.User:" + User.ID, Global.Helpers.GetGridSettings($("#gResults").data("kendoGrid")));
                }
                // Call the original filter function.
                var result = originalFilter.apply(this, arguments);

                return result;
            }
        }

        public SelectProject(project: Dns.ViewModels.ProjectViewModel) {
            vm.SelectedProjectID(project.ID());

            var originalFilters: kendo.data.DataSourceFilters = vm.dataSource.filter();

            //PMNDEV-5057 - We need to keep the filters intact on project change, otherwise they are not retained.
            var filter = { logic: "and", filters: [] };
            originalFilters.filters.forEach((item: kendo.data.DataSourceFilterItem) => {
                if (item.field != "ProjectID") {
                    filter.filters.push(item);
                }
            });

            if (project.ID() == Constants.GuidEmpty) {
                filter.filters.push({ field: "ProjectID", operator: "notequals", value: Constants.GuidEmpty });
            } else {
                filter.filters.push({ field: "ProjectID", operator: "equals", value: project.ID() });
            }

            vm.dataSource.filter(filter.filters);

            return false;
      }

        public AddClearAllFiltersMenuItem(e: any): void {
            let self = this;
            let popup = e.container.data('kendoPopup');
            let menu = e.container.find(".k-menu").data("kendoMenu");
            let grid: kendo.ui.Grid = e.sender;
            menu.append({ text: "Clear All Filters", spriteCssClass: 'k-i-filter-clear' });
            menu.bind("select", function (e) {
              if ($(e.item).text() == "Clear All Filters") {
                //First Clear the Filter of the grid, then close the menu, then Popup.  Must be done in this order.  See https://www.telerik.com/forums/close-menu-on-custom-column-menu-item
                if (vm.SelectedProjectID() != Constants.GuidEmpty) {
                  grid.dataSource.filter({ field: "ProjectID", operator: "equals", value: vm.SelectedProjectID() });
                }
                else {
                  grid.dataSource.filter({ field: "ProjectID", operator: "notequals", value: Constants.GuidEmpty });
                }
                menu.close();
                popup.close();
              }
            });
          }
  
        public ResultsGrid(): kendo.ui.Grid {
            return $("#gResults").data("kendoGrid");
        }

        public onCreateRequest(proj: Dns.Interfaces.IProjectDTO) {
            var projectID = proj.ID;
            vm.SelectedProjectID(proj.ID);
            Global.Helpers.ShowDialog("Choose Request Type", '/requests/createdialog', ["Close"], 400, 600, { ProjectID: projectID }).done((result: Dns.Interfaces.IRequestTypeDTO) => {
                if (!result)
                    return;
                var url;
                if (!result.TemplateID && !result.WorkflowID) {
                    // Legacy Non-workflow request types
                    url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + projectID;
                } else if (!result.TemplateID) {
                    // Workflow based non-QueryComposer request types
                    url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&WorkflowID=" + result.WorkflowID;
                } else {
                    // QueryComposer request types
                    url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&templateID=" + result.TemplateID + "&WorkflowID=" + result.WorkflowID;
                }
                window.location.href = url;
            });
        }
    }

    export function NameAchor(dataItem: Dns.Interfaces.IRequestDTO): string {
        if (dataItem.CurrentWorkFlowActivityID) {
            return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        } else {
            return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
    }

    export function DueDateTemplate(dataItem: Dns.Interfaces.IRequestDTO): string {
      return dataItem.DueDate ? moment(dataItem.DueDate).format("MM/DD/YYYY") : "";
    }


    function init() {
        $.when<any>(Users.GetSetting("Requests.Index.gResults.User:" + User.ID),
            Dns.WebApi.Users.ListAvailableProjects(null, "ID, Name", "Name"),
            Dns.WebApi.Projects.RequestableProjects(null, "ID,Name", "Name")
            ).done((gResultsSettings, projects, requestableProjects) => {
            $(() => {
                projects.unshift(<any>{ ID: Constants.GuidEmpty, Name: "All Projects" });

                var bindingControl = $("#Content");
                var projectID = Global.GetQueryParam("projectid");
                if (!projectID)
                    projectID = Constants.GuidEmpty;

                vm = new ViewModel(gResultsSettings, bindingControl, projects, projectID, requestableProjects);
                ko.applyBindings(vm, bindingControl[0]);

                //The collection of Date type columns in the grid.
                var arrDateColumns = [];
                arrDateColumns.push("SubmittedOn", "DueDate");
                //This specific method ensures that date filters are processed accordingly.
                Global.Helpers.SetGridFromSettingsWithDates(vm.ResultsGrid(), gResultsSettings, arrDateColumns);
                if (projectID != Constants.GuidEmpty) {
                  vm.ResultsGrid().dataSource.filter({
                    field: "ProjectID",
                    operator: "equals",
                    value: projectID
                  });
                }
                else {
                  vm.ResultsGrid().dataSource.filter({
                    field: "ProjectID",
                    operator: "notequals",
                    value: projectID
                  });
                }
                
                vm.ResultsGrid().bind("dataBound", function (e) {
                  Users.SetSetting("Requests.Index.gResults.User:" + User.ID, Global.Helpers.GetGridSettings(vm.ResultsGrid()));
                });
                vm.ResultsGrid().bind("columnMenuInit", vm.AddClearAllFiltersMenuItem);
            });
        });
    }

    init();
}