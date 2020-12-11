/// <reference path="../../Scripts/page/Page.ts" /> 
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Requests;
(function (Requests) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gResultsSettings, bindingControl, projects, projectID, requestableProjecs) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                //projects are the ones the user is able to see
                _this.Projects = ko.observableArray(projects.map(function (project) {
                    return new Dns.ViewModels.ProjectViewModel(project);
                }));
                _this.SelectedProjectID = ko.observable(projectID);
                //requestable project are the ones that the user can create new requests for.
                _this.RequestableProjects = requestableProjecs;
                var dsRequestSettings = gResultsSettings.filter(function (item) { return item.Key === "Requests.Index.gResults.User:" + User.ID; });
                _this.dataSourceSetting = (dsRequestSettings.length > 0 && dsRequestSettings[0] !== null) ? dsRequestSettings[0] : null;
                _this.dataSource = new kendo.data.DataSource({
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
                var originalFilter = _this.dataSource.filter;
                // Replace the original filter function.
                _this.dataSource.filter = function () {
                    //
                    if (arguments.length > 0) {
                        //Saving filter chnages right back to the DB.  
                        Users.SetSetting("Requests.Index.gResults.User:" + User.ID, Global.Helpers.GetGridSettings($("#gResults").data("kendoGrid")));
                    }
                    // Call the original filter function.
                    var result = originalFilter.apply(this, arguments);
                    return result;
                };
                return _this;
            }
            ViewModel.prototype.SelectProject = function (project) {
                vm.SelectedProjectID(project.ID());
                var originalFilters = vm.dataSource.filter();
                //PMNDEV-5057 - We need to keep the filters intact on project change, otherwise they are not retained.
                var filter = { logic: "and", filters: [] };
                originalFilters.filters.forEach(function (item) {
                    if (item.field != "ProjectID") {
                        filter.filters.push(item);
                    }
                });
                if (project.ID() == Constants.GuidEmpty) {
                    filter.filters.push({ field: "ProjectID", operator: "notequals", value: Constants.GuidEmpty });
                }
                else {
                    filter.filters.push({ field: "ProjectID", operator: "equals", value: project.ID() });
                }
                vm.dataSource.filter(filter.filters);
                return false;
            };
            ViewModel.prototype.AddClearAllFiltersMenuItem = function (e) {
                var self = this;
                var popup = e.container.data('kendoPopup');
                var menu = e.container.find(".k-menu").data("kendoMenu");
                var grid = e.sender;
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
            };
            ViewModel.prototype.ResultsGrid = function () {
                return $("#gResults").data("kendoGrid");
            };
            ViewModel.prototype.onCreateRequest = function (proj) {
                var projectID = proj.ID;
                vm.SelectedProjectID(proj.ID);
                Global.Helpers.ShowDialog("Choose Request Type", '/requests/createdialog', ["Close"], 400, 600, { ProjectID: projectID }).done(function (result) {
                    if (!result)
                        return;
                    var url;
                    if (!result.TemplateID && !result.WorkflowID) {
                        // Legacy Non-workflow request types
                        url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + projectID;
                    }
                    else if (!result.TemplateID) {
                        // Workflow based non-QueryComposer request types
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&WorkflowID=" + result.WorkflowID;
                    }
                    else {
                        // QueryComposer request types
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&templateID=" + result.TemplateID + "&WorkflowID=" + result.WorkflowID;
                    }
                    window.location.href = url;
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            if (dataItem.CurrentWorkFlowActivityID) {
                return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
            }
            else {
                return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
            }
        }
        Index.NameAchor = NameAchor;
        function DueDateTemplate(dataItem) {
            return dataItem.DueDate ? moment(dataItem.DueDate).format("MM/DD/YYYY") : "";
        }
        Index.DueDateTemplate = DueDateTemplate;
        function init() {
            $.when(Users.GetSettings(["Requests.Index.gResults.User:" + User.ID]), Dns.WebApi.Users.ListAvailableProjects(null, "ID, Name", "Name"), Dns.WebApi.Projects.RequestableProjects(null, "ID,Name", "Name")).done(function (gResultsSettings, projects, requestableProjects) {
                $(function () {
                    projects.unshift({ ID: Constants.GuidEmpty, Name: "All Projects" });
                    var bindingControl = $("#Content");
                    var projectID = Global.GetQueryParam("projectid");
                    if (!projectID)
                        projectID = Constants.GuidEmpty;
                    vm = new ViewModel(gResultsSettings, bindingControl, projects, projectID, requestableProjects);
                    ko.applyBindings(vm, bindingControl[0]);
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
                });
            });
        }
        init();
    })(Index = Requests.Index || (Requests.Index = {}));
})(Requests || (Requests = {}));
