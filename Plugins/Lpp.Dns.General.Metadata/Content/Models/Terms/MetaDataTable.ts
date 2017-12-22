/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
    export interface IMetaDataTableTermData extends RequestCriteriaModels.ITermData {
        Tables: MetaDataTableTypes[];
    }

    export enum MetaDataTableTypes {
        Diagnosis = 0,
        Dispensing = 1,
        Encounter = 2,
        Enrollment = 3,
        Procedure = 4,
    }
} 