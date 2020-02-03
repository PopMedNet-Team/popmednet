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
/// <reference path="../../Scripts/Common.ts" />
var Reports;
(function (Reports) {
    var NewActivityReport;
    (function (NewActivityReport) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(projects, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.SelectedProjects = ko.observableArray();
                _this.ShowResults = ko.observable(false);
                _this.StartDate = ko.observable();
                _this.EndDate = ko.observable();
                _this.HeaderStartDate = ko.observable();
                _this.HeaderEndDate = ko.observable();
                _this.HeaderSelectedProjectText = ko.observable();
                _this.ProjectList = projects;
                _this.Results = ko.observable([]);
                _this.Summary = ko.observable([]);
                _this.Total = ko.observable();
                _this.SelectedProjectText = ko.computed(function () {
                    var text = '';
                    _this.SelectedProjects().forEach(function (projectID) {
                        var item = ko.utils.arrayFirst(_this.ProjectList, function (proj) {
                            return proj.ID == projectID;
                        });
                        text += ", " + item.Name;
                    });
                    if (text.length > 0)
                        text = text.substr(2);
                    return text;
                });
                return _this;
            }
            ViewModel.prototype.btnExecute_Click = function (data, event) {
                var _this = this;
                var dStartDate = this.StartDate() ? new Date(this.StartDate()) : null;
                var dEndDate = this.EndDate() ? new Date(this.EndDate()) : null;
                var sStartDate = this.StartDate() ? dStartDate.toISOString() : null;
                var sEndDate = this.EndDate() ? dEndDate.toISOString() : null;
                var url = "/reports/NetworkActivityReportResults?";
                url += "StartDate=" + sStartDate;
                url += "&EndDate=" + sEndDate;
                this.SelectedProjects().forEach(function (projectID) {
                    url += "&Projects=" + projectID;
                });
                //Execute and display the data retrieved
                $.getJSON(url).done(function (data) {
                    var model = JSON.parse(data);
                    _this.Results(model.Results);
                    //Calculate the summary data here
                    _this.Summary(model.Summary);
                    _this.Total(model.Results.length);
                    _this.HeaderStartDate(dStartDate);
                    _this.HeaderEndDate(dEndDate);
                    _this.HeaderSelectedProjectText(_this.SelectedProjectText());
                    _this.ShowResults(true);
                });
            };
            ViewModel.prototype.Print = function (data, event) {
                event.preventDefault();
                window.print();
            };
            return ViewModel;
        }(Global.PageViewModel));
        NewActivityReport.ViewModel = ViewModel;
        function init() {
            Dns.WebApi.Projects.List().done(function (projects) {
                $(function () {
                    var bindingControl = $("#reportContainer");
                    vm = new ViewModel(projects, bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        NewActivityReport.init = init;
        init();
    })(NewActivityReport = Reports.NewActivityReport || (Reports.NewActivityReport = {}));
})(Reports || (Reports = {}));
//# sourceMappingURL=NewActivityReport.js.map