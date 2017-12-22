/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />

///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Ethnicity.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class EthnicityTerm extends RequestCriteriaViewModels.Term {
        public Ethnicities: KnockoutObservableArray<DataCheckerModels.EthnicityTypes>;

        constructor(ethnicityData?: DataCheckerModels.IEthnicityTermData) {
            super(RequestCriteriaModels.TermTypes.EthnicityTerm);

            var dummy: DataCheckerModels.EthnicityTypes[] = [];
            this.Ethnicities = ko.observableArray<DataCheckerModels.EthnicityTypes>(ethnicityData ? ethnicityData.Ethnicities : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IEthnicityTermData {
            var superdata = super.toData();

            var ethnicityData: DataCheckerModels.IEthnicityTermData = {
                TermType: superdata.TermType,
                Ethnicities: this.Ethnicities()
            };

            //console.log('Race: ' + JSON.stringify(ethnicityData));

            return ethnicityData;
        }

        public static EthnicitiesList: Dns.KeyValuePairData<string, DataCheckerModels.EthnicityTypes>[] = [
            new Dns.KeyValuePairData('Unknown', DataCheckerModels.EthnicityTypes.Unknown),
            new Dns.KeyValuePairData('Hispanic', DataCheckerModels.EthnicityTypes.Hispanic),
            new Dns.KeyValuePairData('Not Hispanic', DataCheckerModels.EthnicityTypes.NotHispanic),
            new Dns.KeyValuePairData('Missing', DataCheckerModels.EthnicityTypes.Missing)
        ];
    }
}