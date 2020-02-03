var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Requests;
(function (Requests) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gResultsSettings, bindingControl, projects, projectID, requestableProjecs) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                _this.Projects = ko.observableArray(projects.map(function (project) {
                    return new Dns.ViewModels.ProjectViewModel(project);
                }));
                _this.SelectedProjectID = ko.observable(projectID);
                _this.RequestableProjects = requestableProjecs;
                var filters = [];
                if (projectID != Constants.GuidEmpty)
                    filters.push({ field: "ProjectID", operator: "equals", value: projectID });
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
                    filter: filters
                });
                var originalFilter = _this.dataSource.filter;
                _this.dataSource.filter = function () {
                    if (arguments.length > 0) {
                        vm.Save();
                    }
                    var result = originalFilter.apply(this, arguments);
                    return result;
                };
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.Save();
                    });
                };
                return _this;
            }
            ViewModel.prototype.SelectProject = function (project) {
                vm.SelectedProjectID(project.ID());
                var originalFilters = vm.dataSource.filter();
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
                        url = '/request/create?requestTypeID=' + result.ID + '&projectID=' + projectID;
                    }
                    else if (!result.TemplateID) {
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&WorkflowID=" + result.WorkflowID;
                    }
                    else {
                        url = '/requests/details?requestTypeID=' + result.ID + '&projectID=' + projectID + "&templateID=" + result.TemplateID + "&WorkflowID=" + result.WorkflowID;
                    }
                    window.location.href = url;
                });
            };
            ViewModel.prototype.Save = function () {
                Users.SetSetting("Requests.Index.gResults.User:" + User.ID, Global.Helpers.GetGridSettings(this.ResultsGrid()));
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
        function init() {
            $.when(Users.GetSetting("Requests.Index.gResults.User:" + User.ID), Dns.WebApi.Users.ListAvailableProjects(null, "ID, Name", "Name"), Dns.WebApi.Projects.RequestableProjects(null, "ID,Name", "Name")).done(function (gResultsSettings, projects, requestableProjects) {
                $(function () {
                    projects.unshift({ ID: Constants.GuidEmpty, Name: "All Projects" });
                    var bindingControl = $("#Content");
                    var projectID = Global.GetQueryParam("projectid");
                    if (!projectID)
                        projectID = Constants.GuidEmpty;
                    vm = new ViewModel(gResultsSettings, bindingControl, projects, projectID, requestableProjects);
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () { return vm.Save(); });
                    var arrDateColumns = [];
                    arrDateColumns.push("SubmittedOn", "DueDate");
                    Global.Helpers.SetGridFromSettingsWithDates(vm.ResultsGrid(), gResultsSettings, arrDateColumns);
                });
            });
        }
        init();
    })(Index = Requests.Index || (Requests.Index = {}));
})(Requests || (Requests = {}));
