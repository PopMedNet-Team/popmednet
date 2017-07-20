/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/DataPartner.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class DataPartnerTerm extends RequestCriteriaViewModels.Term {
        public DataPartners: KnockoutObservableArray<string>;

        constructor(dataPartnersData?: RequestCriteriaModels.IDataPartnerTermData) {
            super(RequestCriteriaModels.TermTypes.DataPartnerTerm);

            var dummy: string[] = [];
            this.DataPartners = ko.observableArray<string>(dataPartnersData ? dataPartnersData.DataPartners : dummy);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IDataPartnerTermData {
            var superdata = super.toData();

            var dataPartnersData: RequestCriteriaModels.IDataPartnerTermData = {
                TermType: superdata.TermType,
                DataPartners: this.DataPartners()
            };

            //console.log('Data Partners: ' + JSON.stringify(dataPartnersData));

            return dataPartnersData;
        }
    }
}