/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/WorkplanType.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class WorkplanTypeTerm extends RequestCriteriaViewModels.Term {
        public WorkplanType: KnockoutObservable<string>;

        constructor(workplanTypeData?: RequestCriteriaModels.IWorkplanTypeTermData) {
            super(RequestCriteriaModels.TermTypes.WorkplanTypeTerm);
  
            this.WorkplanType = ko.observable(workplanTypeData == undefined ? "00000000-0000-0000-0000-000000000000" : workplanTypeData.WorkplanTypeID);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IWorkplanTypeTermData {
            var superdata = super.toData();

            var workplanTypeData: RequestCriteriaModels.IWorkplanTypeTermData = {
                TermType: superdata.TermType,
                WorkplanTypeID: this.WorkplanType()
            };

            return workplanTypeData;
        }

    }

}