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
/// <reference path="../../Models/Terms.ts" />
/// <reference path="../../Models/Terms/AgeStratifier.ts" />
/// <reference path="../../ViewModels/Terms.ts" />
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var AgeStratifierTerm = (function (_super) {
        __extends(AgeStratifierTerm, _super);
        function AgeStratifierTerm(stratifierData) {
            var _this = _super.call(this, RequestCriteriaModels.TermTypes.AgeStratifierTerm) || this;
            _this.AgeStratifier = ko.observable(stratifierData ? stratifierData.AgeStratifier : RequestCriteriaModels.AgeStratifierTypes.NotSpecified);
            _super.prototype.subscribeObservables.call(_this);
            return _this;
        }
        AgeStratifierTerm.prototype.toData = function () {
            var superdata = _super.prototype.toData.call(this);
            var data = {
                TermType: superdata.TermType,
                AgeStratifier: this.AgeStratifier()
            };
            //console.log('Stratifier Term: ' + JSON.stringify(data));
            return data;
        };
        return AgeStratifierTerm;
    }(RequestCriteriaViewModels.Term));
    AgeStratifierTerm.AgeStratifiersList = [
        new Dns.KeyValuePairData('Not Selected', RequestCriteriaModels.AgeStratifierTypes.NotSpecified),
        new Dns.KeyValuePairData('No Stratification', RequestCriteriaModels.AgeStratifierTypes.None),
        new Dns.KeyValuePairData('10 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Ten),
        new Dns.KeyValuePairData('7 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Seven),
        new Dns.KeyValuePairData('4 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Four),
        new Dns.KeyValuePairData('2 Stratifications', RequestCriteriaModels.AgeStratifierTypes.Two)
    ];
    RequestCriteriaViewModels.AgeStratifierTerm = AgeStratifierTerm;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
