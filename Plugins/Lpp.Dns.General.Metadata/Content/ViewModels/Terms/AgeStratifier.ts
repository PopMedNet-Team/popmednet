/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />

/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/AgeStratifier.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class AgeStratifierTerm extends RequestCriteriaViewModels.Term {
        public AgeStratifier: KnockoutObservable<RequestCriteriaModels.AgeStratifierTypes>;

        constructor(stratifierData?: RequestCriteriaModels.IAgeStratifierTermData) {
            super(RequestCriteriaModels.TermTypes.AgeStratifierTerm);

            this.AgeStratifier = ko.observable(stratifierData ? stratifierData.AgeStratifier : RequestCriteriaModels.AgeStratifierTypes.NotSpecified);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IAgeStratifierTermData {
            var superdata = super.toData();

            var data: RequestCriteriaModels.IAgeStratifierTermData = {
                TermType: superdata.TermType,
                AgeStratifier: this.AgeStratifier()
            };

            //console.log('Stratifier Term: ' + JSON.stringify(data));

            return data;
        }

        public static AgeStratifiersList: Dns.KeyValuePairData<string, RequestCriteriaModels.AgeStratifierTypes>[] = [
            new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.AgeStratifierTypes.NotSpecified),
            new Dns.KeyValuePairData('No Stratification', RequestCriteriaModels.AgeStratifierTypes.None),
            new Dns.KeyValuePairData('10 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Ten),
            new Dns.KeyValuePairData('7 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Seven),
            new Dns.KeyValuePairData('4 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Four),
            new Dns.KeyValuePairData('2 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Two)
        ];
    }
}