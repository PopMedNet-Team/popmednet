/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
    export interface IEncounterTermData extends RequestCriteriaModels.ITermData {
        Encounters: EncounterTypes[];
    }

    export enum EncounterTypes {
        All = 0,
        AmbulatoryVisit = 1,
        EmergencyDepartment = 2,
        InpatientHospitalStay = 3,
        NonAcuteInstitutionalStay = 4,
        OtherAmbulatoryVisit = 5,
        Missing = 6,
    }
} 