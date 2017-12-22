/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../models/terms/metadatatable.ts" />
///// <reference path="../Terms.ts" />

module DataCheckerViewModels {
    export class MetaDataTableTerm extends RequestCriteriaViewModels.Term {
        public Tables: KnockoutObservableArray<DataCheckerModels.MetaDataTableTypes>;

        constructor(tableData?: DataCheckerModels.IMetaDataTableTermData) {
            super(RequestCriteriaModels.TermTypes.MetaDataTableTerm);

            var dummy: DataCheckerModels.MetaDataTableTypes[] = [];
            this.Tables = ko.observableArray<DataCheckerModels.MetaDataTableTypes>(tableData ? tableData.Tables : dummy);

            super.subscribeObservables();
        }

        public toData(): DataCheckerModels.IMetaDataTableTermData {
            var superdata = super.toData();

            var pdxData: DataCheckerModels.IMetaDataTableTermData = {
                TermType: superdata.TermType,
                Tables: this.Tables()
            };

            return pdxData;
        }
    }
} 