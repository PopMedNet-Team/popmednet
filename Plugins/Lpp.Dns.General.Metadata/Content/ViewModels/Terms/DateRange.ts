/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../ViewModels/Terms.ts" />



module RequestCriteriaViewModels {
    export class DateRangeTerm extends RequestCriteriaViewModels.Term {
        public Title: KnockoutObservable<string>;
        public StartDate: KnockoutObservable<Date>;
        public EndDate: KnockoutObservable<Date>;
        public DateRangeTermType: KnockoutObservable<RequestCriteriaModels.DateRangeTermTypes>;

        constructor(dateRangeData: RequestCriteriaModels.IDateRangeTermData) {
            super(RequestCriteriaModels.TermTypes.DateRangeTerm);

            var start = null;
            if (dateRangeData.StartDate && dateRangeData.StartDate != null) {
                start = moment.utc(dateRangeData.StartDate).local().toDate();
            }
            var end = null;
            if (dateRangeData.EndDate && dateRangeData.EndDate != null) {
                end = moment.utc(dateRangeData.EndDate).local().toDate();
            }

            this.Title = ko.observable(dateRangeData.Title);
            this.StartDate = ko.observable(start);
            this.EndDate = ko.observable(end);
            this.DateRangeTermType = ko.observable(dateRangeData.DateRangeTermType);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.ITermData {
            var superdata = super.toData();

            var data: RequestCriteriaModels.IDateRangeTermData = {
                TermType: superdata.TermType,
                Title: this.Title(),
                StartDate: this.StartDate(),
                EndDate: this.EndDate(),
                DateRangeTermType: this.DateRangeTermType()
            };

            return data;
        }

        public static ObservationPeriod(): DateRangeTerm {
            return new DateRangeTerm({
                Title: "Observation Period",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.ObservationPeriod
            });
        }

        public static SubmitDateRange(): DateRangeTerm {
            return new DateRangeTerm({
                Title: "Submit Date Range",
                TermType: RequestCriteriaModels.TermTypes.DateRangeTerm,
                StartDate: null,
                EndDate: null,
                DateRangeTermType: RequestCriteriaModels.DateRangeTermTypes.SubmitDateRange
            });
        }
    }
}