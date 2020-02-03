/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
    export interface IRaceTermData extends RequestCriteriaModels.ITermData {
        Races: RaceTypes[];
    }

    export enum RaceTypes {
        Unknown = 0,
        AmericanIndianOrAlaskaNative = 1,
        Asian = 2,
        BlackOrAfricanAmerican = 3,
        NativeHawaiianOrOtherPacificIslander = 4,
        White = 5,
        Missing = 6
    }
}