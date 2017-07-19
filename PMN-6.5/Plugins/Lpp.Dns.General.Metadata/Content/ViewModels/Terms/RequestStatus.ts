/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />

/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/RequestStatus.ts" />
/// <reference path="../../ViewModels/Terms.ts" />

module RequestCriteriaViewModels {
    export class RequestStatusTerm extends RequestCriteriaViewModels.Term {
        public RequestStatus: KnockoutObservable<Dns.Enums.RequestStatuses>;

        constructor(requestStatusData?: RequestCriteriaModels.IRequestStatusTermData) {
            super(RequestCriteriaModels.TermTypes.RequestStatusTerm);

            this.RequestStatus = ko.observable(requestStatusData ? requestStatusData.RequestStatus : null);

            super.subscribeObservables();
        }

        public toData(): RequestCriteriaModels.IRequestStatusTermData {
            var superdata = super.toData();

            var requestStatusData: RequestCriteriaModels.IRequestStatusTermData = {
                TermType: superdata.TermType,
                RequestStatus: this.RequestStatus()
            };

            //console.log('Request Status: ' + JSON.stringify(requestStatusData));

            return requestStatusData;
        }

    }
}