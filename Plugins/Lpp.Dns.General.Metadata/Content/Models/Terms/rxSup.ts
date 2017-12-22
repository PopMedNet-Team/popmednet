/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
    export interface IRxSupTermData extends RequestCriteriaModels.ITermData {
        RxSups: RxSupTypes[];
    }

    export enum RxSupTypes {
        LessThanZero = 0,
        Zero = 1,
        TwoThroughThirty = 2,
        Thirty = 3,
        Sixty = 4,
        Ninety = 5,
        Other = 6,
        Missing = 7
    }
} 