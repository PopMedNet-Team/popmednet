/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/AgeRange.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class AgeRangeTerm extends RequestCriteriaViewModels.Term {
        public MinAge: KnockoutObservable<number>;
        public MaxAge: KnockoutObservable<number>;

        constructor(ageRangeData?: RequestCriteriaModels.IAgeRangeTermData) {
            super(RequestCriteriaModels.TermTypes.AgeRangeTerm);

            this.MinAge = ko.observable(ageRangeData ? ageRangeData.MinAge : 0);
            this.MaxAge = ko.observable(ageRangeData ? ageRangeData.MaxAge : 0);

            super.subscribeObservables(); 
        }

        public toData(): RequestCriteriaModels.ITermData {
            var superdata = super.toData();

            var data: RequestCriteriaModels.IAgeRangeTermData = {
                TermType: superdata.TermType,
                MinAge: this.MinAge(),
                MaxAge: this.MaxAge()
            };

            return data;
        }
    }
}