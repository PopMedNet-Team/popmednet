/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/rxamt.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class RxSupTerm extends RequestCriteriaViewModels.Term {
        public RxSups: KnockoutObservableArray<DataCheckerModels.RxSupTypes>;

        constructor(supData?: DataCheckerModels.IRxSupTermData) {
            super(RequestCriteriaModels.TermTypes.RxSupTerm);

            var dummy: DataCheckerModels.RxSupTypes[] = [];
            this.RxSups = ko.observableArray<DataCheckerModels.RxSupTypes>(supData ? supData.RxSups : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IRxSupTermData {
            var superdata = super.toData();

            var encounterData: DataCheckerModels.IRxSupTermData = {
                TermType: superdata.TermType,
                RxSups: this.RxSups()
            };

            return encounterData;
        }
    }
} 