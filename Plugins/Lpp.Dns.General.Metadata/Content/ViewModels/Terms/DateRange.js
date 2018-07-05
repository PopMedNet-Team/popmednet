/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
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
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var DateRangeTerm = /** @class */ (function (_super) {
        __extends(DateRangeTerm, _super);
        function DateRangeTerm(dateRangeData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.DateRangeTerm) || this;
            var start = null;
            if (dateRangeData.StartDate && dateRangeData.StartDate != null) {
                start = moment.utc(dateRangeData.StartDate).local().toDate();
            }
            var end = null;
            if (dateRangeData.EndDate && dateRangeData.EndDate != null) {
                end = moment.utc(dateRangeData.EndDate).local().toDate();
            }
            _this.Title = ko.observable(dateRangeData.Title);
            _this.StartDate = ko.observable(start);
            _this.EndDate = ko.observable(end);
            _this.DateRangeTermType = ko.observable(dateRangeData.DateRangeTermType);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        DateRangeTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var data = {
                TermType: superdata.TermType,
                Title: this.Title(),
                StartDate: this.StartDate(),
                EndDate: this.EndDate(),
                DateRangeTermType: this.DateRangeTermType()
            };
            return data;
        };
        DateRangeTerm.ObservationPeriod = function () {
            return new DateRangeTerm({
                Title: "Observation Period",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.ObservationPeriod
            });
        };
        DateRangeTerm.SubmitDateRange = function () {
            return new DateRangeTerm({
                Title: "Submit Date Range",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.SubmitDateRange
            });
        };
        return DateRangeTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.DateRangeTerm = DateRangeTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
