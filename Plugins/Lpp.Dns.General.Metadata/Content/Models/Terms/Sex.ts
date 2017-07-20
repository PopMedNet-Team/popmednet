/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module RequestCriteriaModels {
    export interface ISexTermData extends RequestCriteriaModels.ITermData {
        Sex: SexTypes;
    }

    export enum SexTypes {
        NotSpecified = -1,
        Male = 1,
        Female = 2,
        Both = 3,
        Aggregated = 4,
    }
}