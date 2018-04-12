/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />
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
/// <reference path="../Models/Criteria.ts" />
/// <reference path="../ViewModels/Terms.ts" />
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var Criteria = (function (_super) {
        __extends(Criteria, _super);
        function Criteria(criteriaData, name, isPrimary, isExclusion, terms, requestTerms) {
            if (name === void 0) { name = 'Primary'; }
            if (isPrimary === void 0) { isPrimary = true; }
            if (isExclusion === void 0) { isExclusion = false; }
            if (terms === void 0) { terms = []; }
            if (requestTerms === void 0) { requestTerms = []; }
            var _this = _super.call(this) || this;
            criteriaData = criteriaData || {
                Name: name,
                IsExclusion: isExclusion,
                IsPrimary: isPrimary,
                Terms: terms,
                RequestTerms: [],
                HeaderTerms: []
            };
            _this.IsExclusion = ko.observable(criteriaData.IsExclusion);
            _this.IsPrimary = ko.observable(criteriaData.IsPrimary);
            _this.Name = ko.observable(criteriaData.Name);
            // this gets initialized in the loop with AddTerm
            //this.Terms = ko.observableArray<RequestCriteriaModels.ITerm>();
            _this.HeaderTerms = ko.observableArray();
            _this.RequestTerms = ko.observableArray();
            // Add each Term supplied and separate into Header and Body Terms.
            criteriaData.Terms.forEach(function (term) {
                var thisTerm = RequestCriteriaViewModels.Term.TermFactory(term);
                _this.AddTerm(thisTerm);
                // if the term supports the Added method, call it...
                if (thisTerm.Added)
                    thisTerm.Added(_this);
            });
            _this.HeaderTerms.subscribe(function (newValue) {
                // initialize the controls
                //$(".DatePicker").kendoDatePicker({
                //    changeMonth: true,
                //    changeYear: true,
                //    dateFormat: 'mm/dd/yy',
                //    defaultDate: +0,
                //    maxDate: '12/31/2299',
                //    minDate: '01/01/1900',
                //    showButtonPanel: true
                //});
                $(".DatePicker").kendoDatePicker({
                    format: 'MM/dd/yy',
                    max: new Date(2299, 12, 31),
                    min: new Date(1900, 1, 1)
                });
            });
            _this.RequestTerms.subscribe(function (newValue) {
                // initialize the controls
                //$(".DatePicker").kendoDatePicker({
                //    changeMonth: true,
                //    changeYear: true,
                //    dateFormat: 'mm/dd/yy',
                //    defaultDate: +0,
                //    maxDate: '12/31/2299',
                //    minDate: '01/01/1900',
                //    showButtonPanel: true
                //});
                $(".DatePicker").kendoDatePicker({
                    format: 'MM/dd/yy',
                    max: new Date(2299, 12, 31),
                    min: new Date(1900, 1, 1)
                });
            });
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        Criteria.prototype.AddTerm = function (term) {
            if (term instanceof RequestCriteriaViewModels.ProjectTerm || term instanceof RequestCriteriaViewModels.RequestStatusTerm || (term instanceof RequestCriteriaViewModels.DateRangeTerm && term.Title() == 'Submit Date Range')
                || term instanceof RequestCriteriaViewModels.RequesterCenterTerm || term instanceof RequestCriteriaViewModels.WorkplanTypeTerm || term instanceof RequestCriteriaViewModels.ReportAggregationLevelTerm) {
                this.HeaderTerms.push(term);
            }
            else {
                this.RequestTerms.push(term);
            }
            // to support chaining, return the new term
            return term;
        };
        Criteria.prototype.RemoveTerm = function (term) {
            var index = this.HeaderTerms.indexOf(term);
            if (index > -1) {
                this.HeaderTerms.splice(index, 1);
                return;
            }
            index = this.RequestTerms.indexOf(term);
            if (index > -1) {
                this.RequestTerms.splice(index, 1);
                return;
            }
        };
        Criteria.prototype.ReplaceTerm = function (targetTerm, newTerm) {
            var index = this.RequestTerms.indexOf(targetTerm);
            if (index > -1)
                this.RequestTerms.splice(index, 1, newTerm);
            index = this.HeaderTerms.indexOf(targetTerm);
            if (index > -1)
                this.HeaderTerms.splice(index, 1, newTerm);
            return newTerm;
        };
        Criteria.prototype.toData = function () {
            var criteria = {
                Name: this.Name(),
                IsExclusion: this.IsExclusion(),
                IsPrimary: this.IsPrimary(),
                Terms: [],
                RequestTerms: this.RequestTerms().map(function (term, postion) {
                    return term.toData();
                }),
                HeaderTerms: this.HeaderTerms().map(function (term, postion) {
                    return term.toData();
                })
            };
            return criteria;
        };
        return Criteria;
    }(Dns.ChildViewModel));
    RequestCriteriaViewModels.Criteria = Criteria;
    var TaskActivities = (function () {
        function TaskActivities(activityData) {
            this.ProjectID = ko.observable();
            this.AllActivities = activityData; //<= flat list of activities available
            this.dsTaskOrders = new kendo.data.DataSource({ data: [] });
            this.dsActivities = new kendo.data.DataSource({ data: [] });
            this.dsActivityProjects = new kendo.data.DataSource({ data: [] });
            this.dsTaskOrders.data(ko.utils.arrayFilter(this.AllActivities, function (a) { return a.TaskLevel == 1; }));
            this.dsActivities.data(ko.utils.arrayFilter(this.AllActivities, function (a) { return a.TaskLevel == 2; }));
            this.dsActivityProjects.data(ko.utils.arrayFilter(this.AllActivities, function (a) { return a.TaskLevel == 3; }));
            var self = this;
            this.SelectProject = function (projectID) {
                self.ProjectID(projectID);
                if (projectID) {
                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities, function (a) { return a.TaskLevel == 1 && a.ProjectID.toLowerCase() == projectID.toLowerCase(); }));
                }
                else {
                    self.dsTaskOrders.data(ko.utils.arrayFilter(self.AllActivities, function (a) { return a.TaskLevel == 1; }));
                }
                alert("Project Changed");
            };
        }
        TaskActivities.prototype.SelectActivity = function (level, activityID) {
            //this should all be handled by the datasources and kendo dropdowns automatically. Not removing at this time since can't determine if will break other things.
        };
        return TaskActivities;
    }());
    RequestCriteriaViewModels.TaskActivities = TaskActivities;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
