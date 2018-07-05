/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../Models/RequestCriteria.ts" />
/// <reference path="../ViewModels/Criteria.ts" />
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var RequestCriteria = /** @class */ (function (_super) {
        __extends(RequestCriteria, _super);
        function RequestCriteria(requestCriteriaData, requesterCenters, workplanTypes, reportAggregationLevels) {
            var _this = _super.call(this) || this;
            // this gets initialized in the loop with AddCriteria
            _this.Criterias = ko.observableArray();
            if (requestCriteriaData) {
                requestCriteriaData.Criterias.forEach(function (criteria) {
                    _this.AddCriteria(new RequestCriteriaViewModels.Criteria(criteria));
                });
            }
            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList = [];
            RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (workplanTypes != null) {
                workplanTypes.forEach(function (wt) {
                    RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList.push(new Dns.KeyValuePairData(wt.Key, wt.Value));
                });
            }
            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList = [];
            RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (requesterCenters != null) {
                requesterCenters.forEach(function (rc) {
                    RequestCriteriaViewModels.RequestCriteria.RequesterCenterList.push(new Dns.KeyValuePairData(rc.Key, rc.Value));
                });
            }
            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList = [];
            RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData('00000000-0000-0000-0000-000000000000', 'Not Selected'));
            if (reportAggregationLevels != null) {
                reportAggregationLevels.forEach(function (ral) {
                    RequestCriteriaViewModels.RequestCriteria.ReportAggregationLevelList.push(new Dns.KeyValuePairData(ral.Key, ral.Value));
                });
            }
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        RequestCriteria.prototype.AddCriteria = function (criteria) {
            //console.log( 'Adding CG: ' + JSON.stringify( criteria ) );
            if (criteria.IsPrimary &&
                (this.Criterias().filter(function (cg, index, groups) { return cg.IsPrimary(); }).length > 0))
                throw 'Only one primary criteria group is allowed';
            else
                this.Criterias().push(criteria);
        };
        RequestCriteria.prototype.RemoveCriteria = function (criteria) {
            //console.log( 'Removing CG: ' + JSON.stringify( criteria ) );
            var index = this.Criterias().indexOf(criteria);
            if (index > -1)
                this.Criterias().splice(index, 1);
        };
        RequestCriteria.prototype.toData = function () {
            var requestCriteria = {
                Criterias: this.Criterias().map(function (cg, position) {
                    return cg.toData();
                })
            };
            //console.log( 'Request Criteria: ' + JSON.stringify( requestCriteria ) );
            return requestCriteria;
        };
        return RequestCriteria;
    }(Dns.ChildViewModel));
    RequestCriteriaViewModels.RequestCriteria = RequestCriteria;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
