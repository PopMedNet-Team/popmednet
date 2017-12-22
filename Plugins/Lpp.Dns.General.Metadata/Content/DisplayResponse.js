/// <reference path="../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var MetaData;
(function (MetaData) {
    var DisplayResponse;
    (function (DisplayResponse) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(data) {
                var _this = _super.call(this, null) || this;
                _this.Results = ko.observableArray();
                data.forEach(function (item) {
                    _this.Results.push(new ResultViewModel(item));
                });
                return _this;
            }
            return ViewModel;
        }(Dns.PageViewModel));
        DisplayResponse.ViewModel = ViewModel;
        function init(data, bindingControl) {
            vm = new ViewModel(JSON.parse(data));
            $(function () {
                ko.applyBindings(vm, bindingControl[0]);
                $("[name='RequestorEmail']").click(function (e) {
                    e.preventDefault();
                    var a = window.open(e.target.href);
                    a.close();
                    return false;
                });
            });
        }
        DisplayResponse.init = init;
        var ResultViewModel = (function () {
            function ResultViewModel(data) {
                this.Expanded = ko.observable(false);
                this.ID = ko.observable(data.ID);
                this.Identifier = ko.observable(data.Identifier);
                this.Project = ko.observable(data.Project);
                this.RequestType = ko.observable(data.RequestType);
                this.Name = ko.observable(data.Name);
                this.SubmitDate = ko.observable(new Date(data.SubmitDate));
                this.DueDate = ko.observable(!data.DueDate ? null : new Date(data.DueDate));
                this.RequestorUserName = ko.observable(data.RequestorUserName);
                this.RequestorFullName = ko.observable(data.RequestorFullName);
                this.RequestorEmail = ko.observable(data.RequestorEmail);
                this.SubmittedBy = ko.observable(data.SubmittedBy);
                this.Organization = ko.observable(data.Organization);
                this.Priority = ko.observable(data.Priority);
                this.Description = ko.observable(data.Description);
                this.TaskOrder = ko.observable(data.TaskOrder);
                this.Activity = ko.observable(data.Activity);
                this.ActivityProject = ko.observable(data.ActivityProject);
                this.PurposeOfUse = ko.observable(data.PurposeOfUse);
                this.LevelofPHIDisclosure = ko.observable(data.LevelofPHIDisclosure);
                this.RequesterCenter = ko.observable(data.RequesterCenter);
                this.WorkplanType = ko.observable(data.WorkplanType);
                this.ReportAggregationLevel = ko.observable(data.ReportAggregationLevel);
                this.SourceTaskOrder = ko.observable(data.SourceTaskOrder);
                this.SourceActivity = ko.observable(data.SourceActivity);
                this.SourceActivityProject = ko.observable(data.SourceActivityProject);
            }
            ResultViewModel.prototype.ExpandCollapse = function (data, event) {
                data.Expanded(!data.Expanded());
            };
            return ResultViewModel;
        }());
        DisplayResponse.ResultViewModel = ResultViewModel;
    })(DisplayResponse = MetaData.DisplayResponse || (MetaData.DisplayResponse = {}));
})(MetaData || (MetaData = {}));
