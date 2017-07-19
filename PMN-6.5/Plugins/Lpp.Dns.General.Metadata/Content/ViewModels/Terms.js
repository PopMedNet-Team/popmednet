/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../Models/RequestCriteria.ts" />
/// <reference path="../Models/Terms.ts" />
/// <reference path="../Models/Terms/AgeRange.ts" />
/// <reference path="../Models/Terms/AgeStratifier.ts" />
/// <reference path="../Models/Terms/ClinicalSetting.ts" />
/// <reference path="../Models/Terms/DataPartner.ts" />
/// <reference path="../Models/Terms/Ethnicity.ts" />
/// <reference path="../Models/Terms/Metric.ts" />
/// <reference path="../Models/Terms/Project.ts" />
/// <reference path="../Models/Terms/Race.ts" />
/// <reference path="../Models/Terms/RequestStatus.ts" />
/// <reference path="../Models/Terms/Sex.ts" />
/// <reference path="../models/terms/pdx.ts" />
/// <reference path="../models/terms/encountertype.ts" />
/// <reference path="../models/terms/metadatatable.ts" />
/// <reference path="../models/terms/rxamt.ts" />
/// <reference path="../models/terms/rxsup.ts" />
/// <reference path="../models/Terms/ReportAggregationLevel.ts" />
/// <reference path="Terms/AgeRange.ts" />
/// <reference path="Terms/AgeStratifier.ts" />
/// <reference path="Terms/ClinicalSetting.ts" />
/// <reference path="Terms/CodesTerm.ts" />
/// <reference path="Terms/DataPartner.ts" />
/// <reference path="Terms/DateRange.ts" />
/// <reference path="Terms/Ethnicity.ts" />
/// <reference path="Terms/Metric.ts" />
/// <reference path="Terms/Project.ts" />
/// <reference path="Terms/Race.ts" />
/// <reference path="Terms/RequestStatus.ts" />
/// <reference path="Terms/Sex.ts" />
/// <reference path="Terms/RequesterCenter.ts" />
/// <reference path="Terms/WorkplanType.ts" />
/// <reference path="terms/pdx.ts" />
/// <reference path="terms/encountertype.ts" />
/// <reference path="terms/metadatatable.ts" />
/// <reference path="terms/rxamt.ts" />
/// <reference path="terms/rxsup.ts" />
/// <reference path="Terms/ReportAggregationLevel.ts" />
// TODO - add data partner
var RequestCriteriaViewModels;
(function (RequestCriteriaViewModels) {
    var Term = (function (_super) {
        __extends(Term, _super);
        function Term(termType) {
            var _this = _super.call(this) || this;
            _this.TermType = termType;
            _this.TemplateName = ko.computed(function () {
                if (_this.TermType == null)
                    throw "Term Type is Null";
                return RequestCriteriaModels.TermTypes[_this.TermType];
            });
            return _this;
        }
        Term.TermFactory = function (term) {
            switch (term.TermType) {
                case RequestCriteriaModels.TermTypes.AgeRangeTerm:
                    return new RequestCriteriaViewModels.AgeRangeTerm(term);
                case RequestCriteriaModels.TermTypes.AgeStratifierTerm:
                    return new RequestCriteriaViewModels.AgeStratifierTerm(term);
                case RequestCriteriaModels.TermTypes.ClinicalSettingTerm:
                    return new RequestCriteriaViewModels.ClinicalSettingTerm(term);
                case RequestCriteriaModels.TermTypes.CodesTerm:
                    return new RequestCriteriaViewModels.CodesTerm(term);
                case RequestCriteriaModels.TermTypes.DataPartnerTerm:
                    return new RequestCriteriaViewModels.DataPartnerTerm(term);
                case RequestCriteriaModels.TermTypes.DateRangeTerm:
                    return new RequestCriteriaViewModels.DateRangeTerm(term);
                case RequestCriteriaModels.TermTypes.EthnicityTerm:
                    return new DataCheckerViewModels.EthnicityTerm(term);
                case RequestCriteriaModels.TermTypes.MetricTerm:
                    return new DataCheckerViewModels.MetricTerm(term);
                case RequestCriteriaModels.TermTypes.ProjectTerm:
                    return new RequestCriteriaViewModels.ProjectTerm(term);
                case RequestCriteriaModels.TermTypes.RaceTerm:
                    return new DataCheckerViewModels.RaceTerm(term);
                case RequestCriteriaModels.TermTypes.RequestStatusTerm:
                    return new RequestCriteriaViewModels.RequestStatusTerm(term);
                case RequestCriteriaModels.TermTypes.SexTerm:
                    return new RequestCriteriaViewModels.SexTerm(term);
                case RequestCriteriaModels.TermTypes.WorkplanTypeTerm:
                    return new RequestCriteriaViewModels.WorkplanTypeTerm(term);
                case RequestCriteriaModels.TermTypes.RequesterCenterTerm:
                    return new RequestCriteriaViewModels.RequesterCenterTerm(term);
                case RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm:
                    return new RequestCriteriaViewModels.ReportAggregationLevelTerm(term);
                case RequestCriteriaModels.TermTypes.PDXTerm:
                    return new DataCheckerViewModels.PDXTerm(term);
                case RequestCriteriaModels.TermTypes.RxAmtTerm:
                    return new DataCheckerViewModels.RxAmtTerm(term);
                case RequestCriteriaModels.TermTypes.RxSupTerm:
                    return new DataCheckerViewModels.RxSupTerm(term);
                case RequestCriteriaModels.TermTypes.EncounterTypeTerm:
                    return new DataCheckerViewModels.EncounterTerm(term);
                case RequestCriteriaModels.TermTypes.MetaDataTableTerm:
                    return new DataCheckerViewModels.MetaDataTableTerm(term);
                default:
                    throw "RequestCriteriaViewModels.Term.TermFactory needs to construct term: " + term.TermType;
            }
        };
        Term.prototype.toData = function () {
            var term = {
                TermType: this.TermType
            };
            return term;
        };
        return Term;
    }(Dns.ChildViewModel));
    RequestCriteriaViewModels.Term = Term;
})(RequestCriteriaViewModels || (RequestCriteriaViewModels = {}));
