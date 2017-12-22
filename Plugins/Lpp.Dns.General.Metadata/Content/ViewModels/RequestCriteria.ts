/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />

/// <reference path="../Models/RequestCriteria.ts" />
/// <reference path="../ViewModels/Criteria.ts" />

module RequestCriteriaViewModels {
    export class RequestCriteria extends Dns.ChildViewModel {
        public Criterias: KnockoutObservableArray<RequestCriteriaViewModels.Criteria>;
        public static WorkplanTypeList: Dns.KeyValuePairData<string, string>[];
        public static RequesterCenterList: Dns.KeyValuePairData<string, string>[];
        public static ReportAggregationLevelList: Dns.KeyValuePairData<string, string>[];

        constructor(requestCriteriaData?: RequestCriteriaModels.IRequestCriteriaData, requesterCenters?: RequestCriteriaModels.IRequesterCenter[], workplanTypes?: RequestCriteriaModels.IWorkplanType[], reportAggregationLevels?: RequestCriteriaModels.IReportAggregationLevel[]) {
            super();

            // this gets initialized in the loop with AddCriteria
            this.Criterias = ko.observableArray<RequestCriteriaViewModels.Criteria>();

            if (requestCriteriaData) {
                requestCriteriaData.Criterias.forEach((criteria) => {
                    this.AddCriteria(new RequestCriteriaViewModels.Criteria(criteria));
                });
            }

            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList = [];
            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (workplanTypes != null) {
                workplanTypes.forEach(wt => {
                    RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData(wt.Key, wt.Value));
                });
            }

            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList = [];
            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (requesterCenters != null) {
                requesterCenters.forEach(rc => {
                    RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData(rc.Key, rc.Value));
                });
            }

            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList = [];
            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (reportAggregationLevels != null) {
                reportAggregationLevels.forEach(ral => {
                    RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData(ral.Key, ral.Value));
                });
            }

            super.subscribeObservables();
        }

        public AddCriteria( criteria: RequestCriteriaViewModels.Criteria ): void {
            //console.log( 'Adding CG: ' + JSON.stringify( criteria ) );

            if ( criteria.IsPrimary &&
                ( this.Criterias().filter( ( cg, index, groups ) => { return cg.IsPrimary() }).length > 0 ) )
                throw 'Only one primary criteria group is allowed';
            else
                this.Criterias().push( criteria );
        }

        public RemoveCriteria( criteria: RequestCriteriaViewModels.Criteria ): void {
            //console.log( 'Removing CG: ' + JSON.stringify( criteria ) );

            var index = this.Criterias().indexOf( criteria );

            if ( index > -1 )
                this.Criterias().splice( index, 1 );
        }

        public toData(): RequestCriteriaModels.IRequestCriteriaData {
            var requestCriteria: RequestCriteriaModels.IRequestCriteriaData = {
                Criterias: this.Criterias().map( ( cg, position ) => {
                    return cg.toData();
                })
            };

            //console.log( 'Request Criteria: ' + JSON.stringify( requestCriteria ) );

            return requestCriteria;
        }
    }
}