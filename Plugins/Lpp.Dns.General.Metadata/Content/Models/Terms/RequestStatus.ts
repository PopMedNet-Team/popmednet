/// <reference path="../../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../Terms.ts" />

module RequestCriteriaModels {
    export interface IRequestStatusTermData extends RequestCriteriaModels.ITermData {
        RequestStatus: Dns.Enums.RequestStatuses;
    }

}