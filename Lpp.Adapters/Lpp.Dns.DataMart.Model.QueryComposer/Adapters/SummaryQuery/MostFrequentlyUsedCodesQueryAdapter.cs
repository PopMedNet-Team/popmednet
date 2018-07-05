using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class MostFrequentlyUsedCodesQueryAdapter : QueryAdapter
    {
        readonly Guid TermID;

        public MostFrequentlyUsedCodesQueryAdapter(IDictionary<string, object> settings, Guid termID) : base(settings) 
        {
            TermID = termID;
        }

        public override void Dispose()
        {
        }

        protected override bool IsMFU
        {
            get { return true; }
        }

        protected override string Template
        {
            get
            {
                if (DataProvider == Settings.SQLProvider.ODBC)
                {
                    return AccessQuery;
                }
                return SqlQuery;
            }
        }

        protected override SummaryRequestModel ConvertToModel(DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            SummaryRequestModel model = new SummaryRequestModel();

            var criteria = request.Where.Criteria.First();

            var observationPeriodTerm = GetAllCriteriaTerms(criteria, ModelTermsFactory.YearID).FirstOrDefault();
            if (observationPeriodTerm != null)
            {
                model.StartPeriod = observationPeriodTerm.GetStringValue("StartYear");
                model.EndPeriod = observationPeriodTerm.GetStringValue("EndYear");
                model.Period = string.Join(",", QueryAdapter.ExpandYears(model).Select(y => "'" + y + "'"));//used in query
            }

            DTO.Enums.CodeMetric metricValue;
            var metric = GetAllCriteriaTerms(criteria, ModelTermsFactory.CodeMetricID).FirstOrDefault();
            if (metric.GetEnumValue("Metric", out metricValue))
            {
                model.MetricType = metricValue.ToString("D");
            }

            DTO.Enums.OutputCriteria outputValue;
            var cri = GetAllCriteriaTerms(criteria, ModelTermsFactory.CriteriaID).FirstOrDefault();
            if (cri.GetEnumValue("Criteria", out outputValue))
            {
                model.OutputCriteria = outputValue.ToString("D");
            }

            DTO.Enums.Settings settingValue;
            var seeting = GetAllCriteriaTerms(criteria, ModelTermsFactory.SettingID).FirstOrDefault();
            if (seeting.GetEnumValue("Setting", out settingValue))
            {
                model.Setting = settingValue.ToString();
            }

            var ageStratification = GetAgeField(request.Select.Fields.Where(f => f.Type == ModelTermsFactory.AgeRangeID));
            if (ageStratification != null)
            {
                QueryAdapter.SetAgeStratification(model, ageStratification);
            }

            var sexStratification = GetAllCriteriaTerms(criteria, ModelTermsFactory.SexID).FirstOrDefault();
            if (sexStratification != null)
            {
                QueryAdapter.SetSexStratification(model, sexStratification);
            }

            model.Codes = null;//not applicable to this query  
            model.CodeNames = null;//not applicable to this query  
            model.Coverage = null;//not applicable to this query     
            model.StartQuarter = null;//not applicable to this query
            model.EndQuarter = null;//not applicable to this query
            model.SubtypeId = 0;//value never gets set in ui of v5 summary query composer

            return model;
        }

        protected override string MFUStratificationClause(SummaryRequestModel args)
        {
            switch (args.AgeStratification.Value)
            {
                case 1:
                    return args.SexStratification == 4 ? Strat10_MF_Query : Strat10_Query;
                case 2:
                    return args.SexStratification == 4 ? Strat7_MF_Query : Strat7_Query;
                case 3:
                    return args.SexStratification == 4 ? Strat4_MF_Query : Strat4_Query;
                case 4:
                    return args.SexStratification == 4 ? Strat2_MF_Query : Strat2_Query;
                case 5:
                    return args.SexStratification == 4 ? Strat0_MF_Query : Strat0_Query;
            }           

            return string.Empty;
        }

        protected override void ReplaceParameters(ref string query)
        {
            string sd_table = string.Empty;
            string sd_codePrefix = string.Empty;
            string codePrefix = string.Empty;

            DetermineTermSpecifics(TermID, ref sd_table, ref sd_codePrefix, ref codePrefix);

            query = query.Replace("%CODE_FIELD%", codePrefix + "XCode")
                                 .Replace("%NAME_FIELD%", codePrefix + "XName")
                                 .Replace("%SD_CODE_FIELD%", sd_codePrefix + "code")
                                 .Replace("%SD_TABLE%", sd_table);
        }

        static void DetermineTermSpecifics(Guid termID, ref string sd_table, ref string sd_codePrefix, ref string codePrefix)
        {
            if (termID == ModelTermsFactory.HCPCSProcedureCodesID)
            {
                sd_table = "HCPCS";
                sd_codePrefix = "px_";
                codePrefix = "P";
            }

            if (termID == ModelTermsFactory.ICD9DiagnosisCodes3digitID)
            {
                sd_table = "ICD9_DIAGNOSIS";
                codePrefix = "D";
            }
            if (termID == ModelTermsFactory.ICD9DiagnosisCodes4digitID)
            {
                sd_table = "ICD9_DIAGNOSIS_4_DIGIT";
                codePrefix = "D";
            }
            if (termID == ModelTermsFactory.ICD9DiagnosisCodes5digitID)
            {
                sd_table = "ICD9_DIAGNOSIS_5_DIGIT";
                codePrefix = "D";
            }

            if (termID == ModelTermsFactory.ICD9ProcedureCodes3digitID)
            {
                sd_table = "ICD9_PROCEDURE";
                codePrefix = "P";
            }
            if (termID == ModelTermsFactory.ICD9ProcedureCodes4digitID)
            {
                sd_table = "ICD9_PROCEDURE_4_DIGIT";
                sd_codePrefix = "px_";
                codePrefix = "P";
            }
        }

        protected override IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            string sd_table = string.Empty;
            string sd_codePrefix = string.Empty;
            string codePrefix = string.Empty;

            DetermineTermSpecifics(TermID, ref sd_table, ref sd_codePrefix, ref codePrefix);

            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Setting", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = codePrefix + "XCode", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = codePrefix + "XName", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "User Rate (Users per 1000 enrollees)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Event Rate (Events per 1000 enrollees)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events per member", Type = "System.Double" }
            };
        }

        protected override DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetResponseAggregationDefinition()
        {
            string sd_table = string.Empty;
            string sd_codePrefix = string.Empty;
            string codePrefix = string.Empty;

            DetermineTermSpecifics(TermID, ref sd_table, ref sd_codePrefix, ref codePrefix);

            return new DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO
            {
                GroupBy = new[] { "Period", "Sex", "AgeGroup", codePrefix + "XCode", codePrefix + "XName", "Setting" },
                Select = new[] {
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Setting", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = codePrefix + "XCode", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = codePrefix + "XName", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "User Rate (Users per 1000 enrollees)", Type = "System.Double", Aggregate = "Average" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Event Rate (Events per 1000 enrollees)", Type = "System.Double", Aggregate = "Average" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events per member", Type = "System.Double", Aggregate = "Average" }
                }
            };
        }

        const string SqlQuery = @"-- MFU_ICD9_HCPCS
-- METRIC_TYPE = Events, Members
-- SD_METRIC_TYPE = ev, mb
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, SummaryData.Setting, SummaryData.%CODE_FIELD%, SummaryData.%NAME_FIELD%,
CAST(SUM(ISNULL(SummaryData.mb, 0)) AS FLOAT) AS Members, 
CAST(SUM(ISNULL(SummaryData.ev, 0)) AS FLOAT) AS Events,
CAST(SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) AS FLOAT) AS [Total Enrollment in Strata(Members)],
CAST(SUM(ISNULL(EnrollmentData.DaysCovered, 0)) AS FLOAT) AS [Days Covered],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE CAST(SUM(ISNULL(SummaryData.mb, 0)) AS float) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) AS [User Rate (Users per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE CAST(SUM(ISNULL(SummaryData.ev, 0)) AS float) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) AS [Event Rate (Events per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(SummaryData.mb, 0)) = 0 THEN 0 ELSE CAST(SUM(ISNULL(SummaryData.ev, 0)) AS float) / CAST(SUM(ISNULL(SummaryData.mb, 0)) AS float) END, 1) AS FLOAT) AS [Events per member]
FROM

	--
	-- Age Group and Enrollment Data Section
	--
	-- This part makes sure that all age groups for all desired enrollment years, genders and codes/names are represented in the result table
	-- even if there is no summary data.
	--

	(
		SELECT AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		CAST(Sum(ed.Members) AS float) AS SumOfMembers, 
		CAST(Sum(ed.DaysCovered) AS float) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period
				FROM age_groups AS ag, 

				%CJC%

				--WHERE en.year in (%YEARS%) AND en.sex IN (%SEX%) AND en.medcov='Y' AND en.drugcov='Y'
			) AS AgeGroups

			LEFT JOIN

				-- Add the enrollment data to the rows (where medical coverage is Y).

				(SELECT * FROM enrollment WHERE medcov='Y' and drugcov='Y') AS ed
				ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		GROUP BY AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period
	
	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Add the top N rows (by events or members) for each age group stratum.
	--

	(

	%STRATIFICATION_CLAUSE%

	) AS SummaryData

ON (SummaryData.age_group = EnrollmentData.agegroup %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, SummaryData.%CODE_FIELD%, SummaryData.%NAME_FIELD%, SummaryData.Setting, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort, SUM(ISNULL(SummaryData.%SD_METRIC_TYPE%, 0)) DESC
";

        const string AccessQuery = @"-- MFU_ICD9_HCPCS
-- METRIC_TYPE = Events, Members
-- SD_METRIC_TYPE = ev, mb
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, SummaryData.Setting, SummaryData.%CODE_FIELD%, SummaryData.%NAME_FIELD%,
SUM(IIF(ISNULL(SummaryData.mb), 0, SummaryData.mb)) AS Members, 
SUM(IIF(ISNULL(SummaryData.ev), 0, SummaryData.ev)) AS Events,
SUM(IIF(ISNULL(EnrollmentData.SumOfMembers), 0, EnrollmentData.SumOfMembers)) AS [Total Enrollment in Strata(Members)],
SUM(IIF(ISNULL(EnrollmentData.DaysCovered), 0, EnrollmentData.DaysCovered)) AS [Days Covered],
ROUND(IIF([Total Enrollment in Strata(Members)] = 0, 0, Members / [Total Enrollment in Strata(Members)] * 1000), 1) AS [User Rate (Users per 1000 enrollees)],
ROUND(IIF([Total Enrollment in Strata(Members)] = 0, 0, Events / [Total Enrollment in Strata(Members)] * 1000), 1) AS [Event Rate (Events per 1000 enrollees)],
ROUND(IIF(Members = 0, 0, Events / Members), 1) AS [Events per member]
FROM

	--
	-- Age Group and Enrollment Data Section
	--
	-- This part makes sure that all age groups for all desired enrollment years, genders and codes/names are represented in the result table
	-- even if there is no summary data.
	--

	(
		SELECT AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		Sum(ed.Members) AS SumOfMembers, 
		Sum(ed.DaysCovered) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period
				FROM age_groups AS ag, 

				%CJC%

				--WHERE en.year in (%YEARS%) AND en.sex IN (%SEX%) AND en.medcov='Y'
			) AS AgeGroups

			LEFT JOIN

				-- Add the enrollment data to the rows (where medical coverage is Y).

				(SELECT * FROM enrollment WHERE medcov='Y') AS ed
				ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		GROUP BY AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period
	
	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Add the top N rows (by events or members) for each age group stratum.
	--

	(

	%STRATIFICATION_CLAUSE%

	) AS SummaryData

ON (SummaryData.age_group = EnrollmentData.agegroup %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, SummaryData.%CODE_FIELD%, SummaryData.%NAME_FIELD%, SummaryData.Setting, EnrollmentData.AgeGroupSort
ORDER BY AgeGroups.Period, AgeGroups.Sex, AgeGroups.AgeGroupSort, SUM(IIF(ISNULL(SummaryData.%SD_METRIC_TYPE%), 0, SummaryData.%SD_METRIC_TYPE%)) DESC
";

        const string Strat10_Query = @"-- MFU_Strat10
SELECT TOP %OUTPUT_CRITERIA%  %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-1' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('0-1') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 2-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '10-14' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('10-14') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '15-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65-74' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('65-74') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '75+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-1' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('0-1') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 2-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '10-14' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('10-14') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '15-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65-74' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('65-74') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '75+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";
        const string Strat10_MF_Query = @"-- MFU_Strat10
SELECT TOP %OUTPUT_CRITERIA%  %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-1' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('0-1') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 2-4' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 5-9' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '10-14' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('10-14') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '15-18' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '19-21' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65-74' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('65-74') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '75+' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat7_Query = @"-- MFU_Strat7
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('0-1','2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '10-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('10-14','15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M') and age_group in ('0-1','2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '10-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('10-14','15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";
        const string Strat7_MF_Query = @"-- MFU_Strat7
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-4' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('0-1','2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 5-9' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '10-18' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('10-14','15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '19-21' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat4_Query = @"-- MFU_Strat4
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21')  AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";
        const string Strat4_MF_Query = @"-- MFU_Strat4
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, ' 0-21' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21')  AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '22-44' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '45-64' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND  sex in ('M','F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group,  period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat2_Query = @"-- MFU_Strat2
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, 'Under 65' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, 'Under 65' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";
        const string Strat2_MF_Query = @"-- MFU_Strat2
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, 'Under 65' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '65+' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat0_Query = @"-- MFU_Strat0
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '0+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64','65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '0+' AS age_group, sex, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64','65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";
        const string Strat0_MF_Query = @"-- MFU_Strat0
SELECT TOP %OUTPUT_CRITERIA% %SD_CODE_FIELD% AS %CODE_FIELD%, %NAME_FIELD%, Setting, '0+' AS age_group, period, SUM(Members) AS mb, SUM(Events) AS ev
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND SETTING IN (%SETTING%) AND sex in ('M','F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64','65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, period
ORDER BY SUM(%METRIC_TYPE%) DESC";


    }
}
