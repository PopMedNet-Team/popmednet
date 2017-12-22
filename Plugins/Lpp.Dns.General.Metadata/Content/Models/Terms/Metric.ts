/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module DataCheckerModels {
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

    export enum MetricsTypes
    {
        DataPartnerCount = 0,
        DataPartnerPercent = 1,
        DataPartnerPercentContribution = 2,
        DataPartnerPresence = 3,
        Overall = 4,
        OverallCount = 5,
        OverallPresence = 6 
    }
}