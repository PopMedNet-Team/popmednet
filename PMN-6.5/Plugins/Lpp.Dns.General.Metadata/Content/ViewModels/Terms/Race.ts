/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />

///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Race.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class RaceTerm extends RequestCriteriaViewModels.Term {
        public Races: KnockoutObservableArray<DataCheckerModels.RaceTypes>;

        constructor(raceData?: DataCheckerModels.IRaceTermData) {
            super(RequestCriteriaModels.TermTypes.RaceTerm);

            var dummy: DataCheckerModels.RaceTypes[] = [];
            this.Races = ko.observableArray<DataCheckerModels.RaceTypes>(raceData ? raceData.Races : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IRaceTermData {
            var superdata = super.toData();

            var raceData: DataCheckerModels.IRaceTermData = {
                TermType: superdata.TermType,
                Races: this.Races()
            };

            return raceData;
        }

        public static RacesList: Dns.KeyValuePairData<string, DataCheckerModels.RaceTypes>[] = [
            new Dns.KeyValuePairData('Unknown', DataCheckerModels.RaceTypes.Unknown),
            new Dns.KeyValuePairData('American Indian/Alaska Native', DataCheckerModels.RaceTypes.AmericanIndianOrAlaskaNative),
            new Dns.KeyValuePairData('Asian', DataCheckerModels.RaceTypes.Asian),
            new Dns.KeyValuePairData('Black/African American', DataCheckerModels.RaceTypes.BlackOrAfricanAmerican),
            new Dns.KeyValuePairData('Native Hawaiian/Pacific Islander', DataCheckerModels.RaceTypes.NativeHawaiianOrOtherPacificIslander),
            new Dns.KeyValuePairData('White', DataCheckerModels.RaceTypes.White),
            new Dns.KeyValuePairData('Missing', DataCheckerModels.RaceTypes.Missing)
        ];
    }
}