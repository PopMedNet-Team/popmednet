/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Models/Terms.ts" />

module RequestCriteriaModels {
    export interface ICriteriaData {
        Name: string;
        IsExclusion: boolean;
        IsPrimary: boolean;
        Terms: RequestCriteriaModels.ITermData[];

        // The following used to display the Terms in two separate groups.
        HeaderTerms: RequestCriteriaModels.ITermData[];
        RequestTerms: RequestCriteriaModels.ITermData[];
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
}