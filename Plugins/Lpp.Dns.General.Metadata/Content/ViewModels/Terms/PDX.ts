/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/PDX.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class PDXTerm extends RequestCriteriaViewModels.Term {
        public PDXes: KnockoutObservableArray<DataCheckerModels.PDXTypes>;

        constructor(pdxData?: DataCheckerModels.IPDXTermData) {
            super(RequestCriteriaModels.TermTypes.PDXTerm);

            var dummy: DataCheckerModels.PDXTypes[] = [];
            this.PDXes = ko.observableArray<DataCheckerModels.PDXTypes>(pdxData ? pdxData.PDXes : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IPDXTermData {
            var superdata = super.toData();

            var pdxData: DataCheckerModels.IPDXTermData = {
                TermType: superdata.TermType,
                PDXes: this.PDXes()
            };

            return pdxData;
        }
    }
} 