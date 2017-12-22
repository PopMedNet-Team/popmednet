/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Models/Criteria.ts" />

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
    export interface ICodesTermData extends RequestCriteriaModels.ITermData {
        Codes: string;        
        CodeType: string;
        CodesTermType: CodesTermTypes;
        SearchMethodType: SearchMethodTypes;
    }

    ///////////////////////////////
    // base class for all date range term data
    ///////////////////////////////
    export interface IDateRangeTermData extends RequestCriteriaModels.ITermData {
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
        toData: () => RequestCriteriaModels.ITermData;
        // each term adds its own properties and methods...
    }

    export enum SearchMethodTypes {        
        ExactMatch = 0,
        StartsWith = 1,
    }
}