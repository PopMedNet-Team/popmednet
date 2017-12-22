/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
    export interface IRxAmtTermData extends RequestCriteriaModels.ITermData {
        RxAmounts: RxAmtTypes[];
    }

    export enum RxAmtTypes {
        LessThanZero = 0,
        Zero = 1,
        TwoThroughThirty = 2,
        Thirty = 3,
        Sixty = 4,
        Ninety = 5,
        OneHundredTwenty = 6,
        OneHundredEighty = 7,
        Other = 8,
        Missing = 9
    }
} 