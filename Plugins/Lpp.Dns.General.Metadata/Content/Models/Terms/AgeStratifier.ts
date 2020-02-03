/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module RequestCriteriaModels {
    export interface IAgeStratifierTermData extends RequestCriteriaModels.ITermData {
        AgeStratifier: AgeStratifierTypes;
    }

    export enum AgeStratifierTypes {
        NotSpecified = -1,
        None = 0,
        Ten = 1,
        Seven = 2,
        Four = 3,
        Two = 4
    }
}