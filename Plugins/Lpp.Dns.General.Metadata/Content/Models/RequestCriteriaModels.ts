/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../../../Lpp.Dns.Api/Scripts/Lpp.Dns.Interfaces.ts" />
module RequestCriteriaModels {
    ///////////////////////////////
    // list of all known term types, this is used by the term factory
    // so make sure support for the term is added to the factory
    // also, update the /Models/Terms.cs file, and make a corresponding /Models/Terms/xxx.cs class
    ///////////////////////////////
    export enum TermTypes {
        AgeRangeTerm = 1,
        AgeStratifierTerm = 2,
        ClinicalSettingTerm = 3,
        CodesTerm = 4,
        DateRangeTerm = 5,
        ProjectTerm = 6,
        RequestStatusTerm = 7,
        SexTerm = 8,
        RaceTerm = 9,
        EthnicityTerm = 10,
        MetricTerm = 11,
        DataPartnerTerm = 12,
        WorkplanTypeTerm = 13,
        RequesterCenterTerm = 14,
        PDXTerm = 15,
        RxSupTerm = 16,
        RxAmtTerm = 17,
        EncounterTypeTerm = 18,
        MetaDataTableTerm = 19,
        ReportAggregationLevelTerm = 20
    }

    export enum CodesTermTypes {
        Diagnosis_ICD9Term = 101,
        Drug_ICD9Term = 102,
        DrugClassTerm = 103,
        GenericDrugTerm = 104,
        HCPCSTerm = 105,
        Procedure_ICD9Term = 106,
        NDCTerm = 107
    }

    export enum DateRangeTermTypes {
        ObservationPeriod = 201,
        SubmitDateRange = 202
    }

    ///////////////////////////////
    // base class for all term data
    ///////////////////////////////
    export interface ITermData {
        TermType: TermTypes;
        // each term adds its own data...
    }

    ///////////////////////////////
    // base class for all codeterm data
    ///////////////////////////////
    export interface ICodesTermData extends ITermData {
        Codes: string;
        CodeType: string;
        CodesTermType: CodesTermTypes;
        SearchMethodType: SearchMethodTypes;
    }

    ///////////////////////////////
    // base class for all date range term data
    ///////////////////////////////
    export interface IDateRangeTermData extends ITermData {
        Title: string;
        StartDate?: Date;
        EndDate?: Date;
        DateRangeTermType: DateRangeTermTypes;
    }

    ///////////////////////////////
    // base class for all terms
    ///////////////////////////////
    export interface ITerm {
        TermType: RequestCriteriaModels.TermTypes;
        TemplateName: KnockoutComputed<string>;
        toData: () => ITermData;
        // each term adds its own properties and methods...
    }

    export enum SearchMethodTypes {
        ExactMatch = 0,
        StartsWith = 1,
    }


    export interface ICriteriaData {
        Name: string;
        IsExclusion: boolean;
        IsPrimary: boolean;
        Terms: ITermData[];

        // The following used to display the Terms in two separate groups.
        HeaderTerms: ITermData[];
        RequestTerms: ITermData[];
    }

    export interface ITaskActivity {
        ProjectID: any;
        ActivityName: string;
        ActivityID: any;
        ParentID: any;
        TaskLevel: number;
    }

    export interface IWorkplanType {
        Key: string;
        Value: string;
    }

    export interface IRequesterCenter {
        Key: string;
        Value: string;
    }

    export interface IReportAggregationLevel {
        Key: string;
        Value: string;
    }

    export interface IRequestCriteriaData {
        Criterias: ICriteriaData[];
    }

    export interface IAgeRangeTermData extends ITermData {
        MinAge: number;
        MaxAge: number;
    }

    export interface IAgeStratifierTermData extends ITermData {
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

    export interface IClinicalSettingsTermData extends ITermData {
        ClinicalSetting: ClinicalSettingTypes;
    }

    export enum ClinicalSettingTypes {
        NotSpecified = -1,
        Any = 0,
        InPatient = 1,
        OutPatient = 2,
        Emergency = 3
    }

    export interface IDataPartnerTermData extends ITermData {
        DataPartners: string[];
    }

    export interface IProjectTermData extends ITermData {
        Project: string;
    }

    export interface IReportAggregationLevelTermData extends ITermData {
        ReportAggregationLevelID: string;
    }

    export interface IRequesterCenterTermData extends ITermData {
        RequesterCenterID: string;
    }

    export interface IRequestStatusTermData extends ITermData {
        RequestStatus: Dns.Enums.RequestStatuses;
    }

    export interface ISexTermData extends ITermData {
        Sex: SexTypes;
    }

    export enum SexTypes {
        NotSpecified = -1,
        Male = 1,
        Female = 2,
        Both = 3,
        Aggregated = 4,
    }

    export interface IWorkplanTypeTermData extends ITermData {
        WorkplanTypeID: string;
    }    
}