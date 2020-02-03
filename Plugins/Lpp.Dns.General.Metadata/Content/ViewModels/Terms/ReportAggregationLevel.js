/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/ReportAggregationLevel.ts" />
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
    var ReportAggregationLevelTerm = /** @class */ (function (_super) {
        __extends(ReportAggregationLevelTerm, _super);
        function ReportAggregationLevelTerm(reportAggregationLevelData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm) || this;
            _this.ReportAggregationLevel = ko.observable(reportAggregationLevelData == undefined ? "00000000-0000-0000-0000-000000000000" : reportAggregationLevelData.ReportAggregationLevelID);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        ReportAggregationLevelTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var reportAggregationLevelData = {
                TermType: superdata.TermType,
                ReportAggregationLevelID: this.ReportAggregationLevel()
            };
            return reportAggregationLevelData;
        };
        return ReportAggregationLevelTerm;
    }(RequestCriteriaViewModels.Term));
    RequestCriteriaViewModels.ReportAggregationLevelTerm = ReportAggregationLevelTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
