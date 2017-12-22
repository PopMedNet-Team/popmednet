/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
    export interface IPDXTermData extends RequestCriteriaModels.ITermData {
        PDXes: PDXTypes[];
    }

    export enum PDXTypes {
        Principle = 0,
        Secondary = 1,
        Other = 2,
        Missing = 3,
    }
} 