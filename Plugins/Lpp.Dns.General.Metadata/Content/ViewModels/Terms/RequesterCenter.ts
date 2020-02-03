/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/RequesterCenter.ts" />
/// <reference path="../../ViewModels/Terms.ts" />


module RequestCriteriaViewModels {
    export class RequesterCenterTerm extends RequestCriteriaViewModels.Term {
        public RequesterCenter: KnockoutObservable<string>;

        constructor(requesterCenterData?: RequestCriteriaModels.IRequesterCenterTermData) {
            super(RequestCriteriaModels.TermTypes.RequesterCenterTerm);
  
            this.RequesterCenter = ko.observable(requesterCenterData == undefined ? "00000000-0000-0000-0000-000000000000" : requesterCenterData.RequesterCenterID);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IRequesterCenterTermData {
            var superdata = super.toData();

            var requesterCenterData: RequestCriteriaModels.IRequesterCenterTermData = {
                TermType: superdata.TermType,
                RequesterCenterID: this.RequesterCenter()
            };

            return requesterCenterData;
        }
    }

}