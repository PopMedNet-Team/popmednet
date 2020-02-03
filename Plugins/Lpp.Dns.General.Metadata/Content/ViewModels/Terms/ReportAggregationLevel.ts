/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/ReportAggregationLevel.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class ReportAggregationLevelTerm extends RequestCriteriaViewModels.Term {
        public ReportAggregationLevel: KnockoutObservable<string>;

        constructor(reportAggregationLevelData?: RequestCriteriaModels.IReportAggregationLevelTermData) {
            super(RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm);

            this.ReportAggregationLevel = ko.observable(reportAggregationLevelData == undefined ? "00000000-0000-0000-0000-000000000000" : reportAggregationLevelData.ReportAggregationLevelID);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IReportAggregationLevelTermData {
            var superdata = super.toData();

            var reportAggregationLevelData: RequestCriteriaModels.IReportAggregationLevelTermData = {
                TermType: superdata.TermType,
                ReportAggregationLevelID: this.ReportAggregationLevel()
            };

            return reportAggregationLevelData;
        }

    }

} 