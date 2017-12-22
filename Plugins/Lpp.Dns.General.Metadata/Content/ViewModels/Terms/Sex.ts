/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />

/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Sex.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class SexTerm extends RequestCriteriaViewModels.Term {
        public Sex: KnockoutObservable<RequestCriteriaModels.SexTypes>;

        constructor(sexData?: RequestCriteriaModels.ISexTermData) {
            super(RequestCriteriaModels.TermTypes.SexTerm);
  
            this.Sex = ko.observable(sexData ? sexData.Sex : RequestCriteriaModels.SexTypes.NotSpecified);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.ISexTermData {
            var superdata = super.toData();

            var sexData: RequestCriteriaModels.ISexTermData = {
                TermType: superdata.TermType,
                Sex: this.Sex()
            };

            //console.log('Sex: ' + JSON.stringify(sexData));

            return sexData;
        }

        public static SexesList: Dns.KeyValuePairData<string, RequestCriteriaModels.SexTypes>[] = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.SexTypes.NotSpecified),
            new Dns.KeyValuePairData('Male', RequestCriteriaModels.SexTypes.Male),
            new Dns.KeyValuePairData('Female', RequestCriteriaModels.SexTypes.Female),
            new Dns.KeyValuePairData('Both', RequestCriteriaModels.SexTypes.Both),
            new Dns.KeyValuePairData('Both Aggregated', RequestCriteriaModels.SexTypes.Aggregated)
            ];
    }
}