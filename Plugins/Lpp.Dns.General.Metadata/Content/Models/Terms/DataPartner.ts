/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module RequestCriteriaModels {
    export interface IDataPartnerTermData extends RequestCriteriaModels.ITermData {
        DataPartners: string[];
    }
}