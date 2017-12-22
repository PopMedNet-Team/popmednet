/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/rxamt.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class RxAmtTerm extends RequestCriteriaViewModels.Term {
        public RxAmounts: KnockoutObservableArray<DataCheckerModels.RxAmtTypes>;

        constructor(amtData?: DataCheckerModels.IRxAmtTermData) {
            super(RequestCriteriaModels.TermTypes.RxAmtTerm);

            var dummy: DataCheckerModels.RxAmtTypes[] = [];
            this.RxAmounts = ko.observableArray<DataCheckerModels.RxAmtTypes>(amtData ? amtData.RxAmounts : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IRxAmtTermData {
            var superdata = super.toData();

            var encounterData: DataCheckerModels.IRxAmtTermData = {
                TermType: superdata.TermType,
                RxAmounts: this.RxAmounts()
            };

            return encounterData;
        }
    }
} 