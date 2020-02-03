/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/encountertype.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class EncounterTerm extends RequestCriteriaViewModels.Term {
        public Encounters: KnockoutObservableArray<DataCheckerModels.EncounterTypes>;

        constructor(encounterData?: DataCheckerModels.IEncounterTermData) {
            super(RequestCriteriaModels.TermTypes.EncounterTypeTerm);

            var dummy: DataCheckerModels.EncounterTypes[] = [];
            this.Encounters = ko.observableArray<DataCheckerModels.EncounterTypes>(encounterData ? encounterData.Encounters : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IEncounterTermData {
            var superdata = super.toData();

            var encounterData: DataCheckerModels.IEncounterTermData = {
                TermType: superdata.TermType,
                Encounters: this.Encounters()
            };

            return encounterData;
        }
    }
} 