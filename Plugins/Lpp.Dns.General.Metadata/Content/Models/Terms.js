/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Models/Criteria.ts" />
var RequestCriteriaModels;
(function (RequestCriteriaModels) {
    ///////////////////////////////
    // list of all known term types, this is used by the term factory
    // so make sure support for the term is added to the factory
    // also, update the /Models/Terms.cs file, and make a corresponding /Models/Terms/xxx.cs class
    ///////////////////////////////
    var TermTypes;
    (function (TermTypes) {
        TermTypes[TermTypes["AgeRangeTerm"] = 1] = "AgeRangeTerm";
        TermTypes[TermTypes["AgeStratifierTerm"] = 2] = "AgeStratifierTerm";
        TermTypes[TermTypes["ClinicalSettingTerm"] = 3] = "ClinicalSettingTerm";
        TermTypes[TermTypes["CodesTerm"] = 4] = "CodesTerm";
        TermTypes[TermTypes["DateRangeTerm"] = 5] = "DateRangeTerm";
        TermTypes[TermTypes["ProjectTerm"] = 6] = "ProjectTerm";
        TermTypes[TermTypes["RequestStatusTerm"] = 7] = "RequestStatusTerm";
        TermTypes[TermTypes["SexTerm"] = 8] = "SexTerm";
        TermTypes[TermTypes["RaceTerm"] = 9] = "RaceTerm";
        TermTypes[TermTypes["EthnicityTerm"] = 10] = "EthnicityTerm";
        TermTypes[TermTypes["MetricTerm"] = 11] = "MetricTerm";
        TermTypes[TermTypes["DataPartnerTerm"] = 12] = "DataPartnerTerm";
        TermTypes[TermTypes["WorkplanTypeTerm"] = 13] = "WorkplanTypeTerm";
        TermTypes[TermTypes["RequesterCenterTerm"] = 14] = "RequesterCenterTerm";
        TermTypes[TermTypes["PDXTerm"] = 15] = "PDXTerm";
        TermTypes[TermTypes["RxSupTerm"] = 16] = "RxSupTerm";
        TermTypes[TermTypes["RxAmtTerm"] = 17] = "RxAmtTerm";
        TermTypes[TermTypes["EncounterTypeTerm"] = 18] = "EncounterTypeTerm";
        TermTypes[TermTypes["MetaDataTableTerm"] = 19] = "MetaDataTableTerm";
        TermTypes[TermTypes["ReportAggregationLevelTerm"] = 20] = "ReportAggregationLevelTerm";
    })(TermTypes = RequestCriteriaModels.TermTypes || (RequestCriteriaModels.TermTypes = {}));
    var CodesTermTypes;
    (function (CodesTermTypes) {
        CodesTermTypes[CodesTermTypes["Diagnosis_ICD9Term"] = 101] = "Diagnosis_ICD9Term";
        CodesTermTypes[CodesTermTypes["Drug_ICD9Term"] = 102] = "Drug_ICD9Term";
        CodesTermTypes[CodesTermTypes["DrugClassTerm"] = 103] = "DrugClassTerm";
        CodesTermTypes[CodesTermTypes["GenericDrugTerm"] = 104] = "GenericDrugTerm";
        CodesTermTypes[CodesTermTypes["HCPCSTerm"] = 105] = "HCPCSTerm";
        CodesTermTypes[CodesTermTypes["Procedure_ICD9Term"] = 106] = "Procedure_ICD9Term";
        CodesTermTypes[CodesTermTypes["NDCTerm"] = 107] = "NDCTerm";
    })(CodesTermTypes = RequestCriteriaModels.CodesTermTypes || (RequestCriteriaModels.CodesTermTypes = {}));
    var DateRangeTermTypes;
    (function (DateRangeTermTypes) {
        DateRangeTermTypes[DateRangeTermTypes["ObservationPeriod"] = 201] = "ObservationPeriod";
        DateRangeTermTypes[DateRangeTermTypes["SubmitDateRange"] = 202] = "SubmitDateRange";
    })(DateRangeTermTypes = RequestCriteriaModels.DateRangeTermTypes || (RequestCriteriaModels.DateRangeTermTypes = {}));
    var SearchMethodTypes;
    (function (SearchMethodTypes) {
        SearchMethodTypes[SearchMethodTypes["ExactMatch"] = 0] = "ExactMatch";
        SearchMethodTypes[SearchMethodTypes["StartsWith"] = 1] = "StartsWith";
    })(SearchMethodTypes = RequestCriteriaModels.SearchMethodTypes || (RequestCriteriaModels.SearchMethodTypes = {}));
})(RequestCriteriaModels || (RequestCriteriaModels = {}));
