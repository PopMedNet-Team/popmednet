/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module RequestCriteriaModels {
    export interface IProjectTermData extends RequestCriteriaModels.ITermData {
        Project: string;
    }
}