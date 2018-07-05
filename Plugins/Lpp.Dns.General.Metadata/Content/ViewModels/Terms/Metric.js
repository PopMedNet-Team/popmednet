/// <reference path="../../../../../lpp.dns.portal/scripts/common.ts" />
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
///// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/Ethnicity.ts" />
///// <reference path="../Terms.ts" />
var DataCheckerViewModels;
(function (DataCheckerViewModels) {
    var MetricTerm = /** @class */ (function (_super) {
        __extends(MetricTerm, _super);
        function MetricTerm(metricData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.MetricTerm) || this;
            _this.MetricsTermType = ko.observable(metricData.MetricsTermType);
            _this.Metrics = ko.observableArray(metricData.Metrics);
            _this.MetricsList = ko.observableArray();
            switch (_this.MetricsTermType()) {
                case DataCheckerModels.MetricsTermTypes.Race:
                case DataCheckerModels.MetricsTermTypes.Ethnicity:
                    _this.MetricsList.push(new Dns.KeyValuePairData('Overall', DataCheckerModels.MetricsTypes.Overall));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Percent within Data Partner', DataCheckerModels.MetricsTypes.DataPartnerPercent));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Percent of Data Partner Contribution', DataCheckerModels.MetricsTypes.DataPartnerPercentContribution));
                    break;
                case DataCheckerModels.MetricsTermTypes.Diagnoses:
                case DataCheckerModels.MetricsTermTypes.Procedures:
                    _this.MetricsList.push(new Dns.KeyValuePairData('Overall Count', DataCheckerModels.MetricsTypes.OverallCount));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Count by Data Partner', DataCheckerModels.MetricsTypes.DataPartnerCount));
                    break;
                case DataCheckerModels.MetricsTermTypes.NDC:
                    _this.MetricsList.push(new Dns.KeyValuePairData('Overall Presence', DataCheckerModels.MetricsTypes.OverallPresence));
                    _this.MetricsList.push(new Dns.KeyValuePairData('Presence by Data Partner', DataCheckerModels.MetricsTypes.DataPartnerPresence));
                    break;
            }
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        MetricTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var metricData = {
                TermType: superdata.TermType,
                Metrics: this.Metrics(),
                MetricsTermType: this.MetricsTermType(),
            };
            //console.log('Race: ' + JSON.stringify(metricData));
            return metricData;
        };
        return MetricTerm;
    }(RequestCriteriaViewModels.Term));
    DataCheckerViewModels.MetricTerm = MetricTerm;
})(DataCheckerViewModels || (DataCheckerViewModels = {}));
