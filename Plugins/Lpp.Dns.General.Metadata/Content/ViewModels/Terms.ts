/// <reference path="../../../../lpp.dns.portal/scripts/common.ts" />

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

module RequestCriteriaViewModels {
    export class Term extends Dns.ChildViewModel implements RequestCriteriaModels.ITerm {
        public TermType: RequestCriteriaModels.TermTypes;
        public TemplateName: KnockoutComputed<string>;
           
        constructor(termType: RequestCriteriaModels.TermTypes) {
            super();

            this.TermType = termType;

            this.TemplateName = ko.computed(() => {
                if (this.TermType == null)
                    throw "Term Type is Null";

                return RequestCriteriaModels.TermTypes[this.TermType];
            });
        }

        public static TermFactory(term: RequestCriteriaModels.ITermData): Term {
            switch (term.TermType) {
                case RequestCriteriaModels.TermTypes.AgeRangeTerm:
                    return <Term> new RequestCriteriaViewModels.AgeRangeTerm( <RequestCriteriaModels.IAgeRangeTermData> term );

                case RequestCriteriaModels.TermTypes.AgeStratifierTerm:
                    return <Term> new RequestCriteriaViewModels.AgeStratifierTerm( <RequestCriteriaModels.IAgeStratifierTermData> term );

                case RequestCriteriaModels.TermTypes.ClinicalSettingTerm:
                    return <Term> new RequestCriteriaViewModels.ClinicalSettingTerm(<RequestCriteriaModels.IClinicalSettingsTermData> term);

                case RequestCriteriaModels.TermTypes.CodesTerm:
                    return <Term> new RequestCriteriaViewModels.CodesTerm(<RequestCriteriaModels.ICodesTermData> term);

                case RequestCriteriaModels.TermTypes.DataPartnerTerm:
                    return <Term> new RequestCriteriaViewModels.DataPartnerTerm(<RequestCriteriaModels.IDataPartnerTermData> term);

                case RequestCriteriaModels.TermTypes.DateRangeTerm:
                    return <Term> new RequestCriteriaViewModels.DateRangeTerm(<RequestCriteriaModels.IDateRangeTermData> term);

                case RequestCriteriaModels.TermTypes.EthnicityTerm:
                    return <Term> new DataCheckerViewModels.EthnicityTerm(<DataCheckerModels.IEthnicityTermData> term);

                case RequestCriteriaModels.TermTypes.MetricTerm:
                    return <Term> new DataCheckerViewModels.MetricTerm(<DataCheckerModels.IMetricsTermData> term);

                case RequestCriteriaModels.TermTypes.ProjectTerm:
                    return <Term> new RequestCriteriaViewModels.ProjectTerm( <RequestCriteriaModels.IProjectTermData> term );

                case RequestCriteriaModels.TermTypes.RaceTerm:
                    return <Term> new DataCheckerViewModels.RaceTerm(<DataCheckerModels.IRaceTermData> term);

                case RequestCriteriaModels.TermTypes.RequestStatusTerm:
                    return <Term> new RequestCriteriaViewModels.RequestStatusTerm( <RequestCriteriaModels.IRequestStatusTermData> term );

                case RequestCriteriaModels.TermTypes.SexTerm:
                    return <Term> new RequestCriteriaViewModels.SexTerm(<RequestCriteriaModels.ISexTermData> term);

                case RequestCriteriaModels.TermTypes.WorkplanTypeTerm:
                    return <Term> new RequestCriteriaViewModels.WorkplanTypeTerm(<RequestCriteriaModels.IWorkplanTypeTermData> term);

                case RequestCriteriaModels.TermTypes.RequesterCenterTerm:
                    return <Term>new RequestCriteriaViewModels.RequesterCenterTerm(<RequestCriteriaModels.IRequesterCenterTermData>term);

                case RequestCriteriaModels.TermTypes.ReportAggregationLevelTerm:
                    return <Term>new RequestCriteriaViewModels.ReportAggregationLevelTerm(<RequestCriteriaModels.IReportAggregationLevelTermData>term);

                case RequestCriteriaModels.TermTypes.PDXTerm:
                    return <Term> new DataCheckerViewModels.PDXTerm(<DataCheckerModels.IPDXTermData> term);
                case RequestCriteriaModels.TermTypes.RxAmtTerm:
                    return <Term> new DataCheckerViewModels.RxAmtTerm(<DataCheckerModels.IRxAmtTermData> term);
                case RequestCriteriaModels.TermTypes.RxSupTerm:
                    return <Term> new DataCheckerViewModels.RxSupTerm(<DataCheckerModels.IRxSupTermData> term);
                case RequestCriteriaModels.TermTypes.EncounterTypeTerm:
                    return <Term> new DataCheckerViewModels.EncounterTerm(<DataCheckerModels.IEncounterTermData> term);
                case RequestCriteriaModels.TermTypes.MetaDataTableTerm:
                    return <Term> new DataCheckerViewModels.MetaDataTableTerm(<DataCheckerModels.IMetaDataTableTermData> term);
                default:
                    throw "RequestCriteriaViewModels.Term.TermFactory needs to construct term: " + term.TermType;
            }
        }

        public toData(): RequestCriteriaModels.ITermData {
            var term: RequestCriteriaModels.ITermData = {
                TermType: this.TermType
            };

            return term;
        }
    }
}