/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="requestcriteriamodels.ts" />
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

    export interface IMetricsTermData extends RequestCriteriaModels.ITermData {
        Metrics: MetricsTypes[];
        MetricsTermType: MetricsTermTypes;
    }

    export enum MetricsTermTypes {
        Race = 0,
        Ethnicity = 1,
        Diagnoses = 2,
        Procedures = 3,
        NDC = 4
    }

    export enum MetricsTypes {
        DataPartnerCount = 0,
        DataPartnerPercent = 1,
        DataPartnerPercentContribution = 2,
        DataPartnerPresence = 3,
        Overall = 4,
        OverallCount = 5,
        OverallPresence = 6
    }

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

    export interface IEthnicityTermData extends RequestCriteriaModels.ITermData {
        Ethnicities: EthnicityTypes[];
    }

    export enum EthnicityTypes {
        Hispanic = 0,
        NotHispanic = 1,
        Unknown = 2,
        Missing = 3
    }

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