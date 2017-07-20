/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
    export interface IEthnicityTermData extends RequestCriteriaModels.ITermData {
        Ethnicities: EthnicityTypes[];
    }

    export enum EthnicityTypes {
        Hispanic = 0,
        NotHispanic = 1,
        Unknown = 2,
        Missing = 3
    }
}