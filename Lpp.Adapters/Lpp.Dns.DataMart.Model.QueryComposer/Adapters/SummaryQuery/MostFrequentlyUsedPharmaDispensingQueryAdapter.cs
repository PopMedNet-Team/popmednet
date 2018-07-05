using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class MostFrequentlyUsedPharmaDispensingQueryAdapter : QueryAdapter
    {
        readonly Guid TermID;

        public MostFrequentlyUsedPharmaDispensingQueryAdapter(IDictionary<string, object> settings, Guid termID)
            : base(settings)
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

            var observationPeriodTerm = GetAllCriteriaTerms(criteria, ModelTermsFactory.QuarterYearID).FirstOrDefault();
            if (observationPeriodTerm != null)
            {
                model.StartPeriod = observationPeriodTerm.GetStringValue("StartYear");
                model.EndPeriod = observationPeriodTerm.GetStringValue("EndYear");

                if (string.Equals(observationPeriodTerm.GetStringValue("ByYearsOrQuarters"), "ByQuarters", StringComparison.OrdinalIgnoreCase))
                {
                    model.StartQuarter = observationPeriodTerm.GetStringValue("StartQuarter");
                    model.EndQuarter = observationPeriodTerm.GetStringValue("EndQuarter");
                    model.Period = string.Join(",", QueryAdapter.ExpandYearsWithQuarters(Convert.ToInt32(model.StartPeriod), Convert.ToInt32(model.StartQuarter[1].ToString()), Convert.ToInt32(model.EndPeriod), Convert.ToInt32(model.EndQuarter[1].ToString())).Select(y => "'" + y + "'"));//used in query
                }
                else
                {
                    model.StartQuarter = null;
                    model.EndQuarter = null;
                    model.Period = string.Join(",", QueryAdapter.ExpandYears(model).Select(y => "'" + y + "'"));//used in query
                }
            }

            DTO.Enums.DispensingMetric metricValue;
            var dispensing = GetAllCriteriaTerms(criteria, ModelTermsFactory.DispensingMetricID).FirstOrDefault();
            if (dispensing.GetEnumValue("Metric", out metricValue))
            {
                model.MetricType = metricValue.ToString("D");
            }

            DTO.Enums.OutputCriteria outputValue;
            var cri = GetAllCriteriaTerms(criteria, ModelTermsFactory.CriteriaID).FirstOrDefault();
            if (cri.GetEnumValue("Criteria", out outputValue))
            {
                model.OutputCriteria = outputValue.ToString("D");
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

            model.Setting = null;
            model.Codes = null;//not applicable to this query  
            model.CodeNames = null;//not applicable to this query  
            model.Coverage = null;//not applicable to this query 
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
            if (TermID == ModelTermsFactory.DrugClassID)
            {
                query = query.Replace("%NAME_FIELD%", "DrugClass")
                                 .Replace("%SD_TABLE%", "DRUG_CLASS");
            }
            if (TermID == ModelTermsFactory.DrugNameID)
            {
                query = query.Replace("%NAME_FIELD%", "GenericName")
                                 .Replace("%SD_TABLE%", "GENERIC_NAME");
            }
        }

        protected override IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            string name = TermID == ModelTermsFactory.DrugNameID ? "GenericName" : "DrugClass";
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = name, Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaysSupply", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Prevalence Rate (Users per 1000 enrollees)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensing Rate (Dispensings per 1000 enrollees)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Per Dispensing", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days per user", Type = "System.Double" }
            };
        }

        protected override DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetResponseAggregationDefinition()
        {
            string name = TermID == ModelTermsFactory.DrugNameID ? "GenericName" : "DrugClass";
            return new DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO {
                GroupBy = new[] { "Period", "Sex", "AgeGroup", name },
                Select = new[] {
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = name, Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaysSupply", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Prevalence Rate (Users per 1000 enrollees)", Type = "System.Double", Aggregate = "Average" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensing Rate (Dispensings per 1000 enrollees)", Type = "System.Double", Aggregate = "Average" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Per Dispensing", Type = "System.Double", Aggregate = "Average" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days per user", Type = "System.Double", Aggregate = "Average" }
                }
            };
        }

        const string SqlQuery = @"-- MFU_Pharma
-- METRIC_TYPE = DaysSupply, Dispensing
-- SD_METRIC_TYPE = ds, dp
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, SummaryData.%NAME_FIELD%, 
CAST(SUM(ISNULL(SummaryData.dp, 0)) AS FLOAT) AS Dispensings, 
CAST(SUM(ISNULL(SummaryData.ds, 0)) AS FLOAT) AS DaysSupply, 
CAST(SUM(ISNULL(SummaryData.mb, 0)) AS FLOAT) AS Members, 
CAST(SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) AS FLOAT) AS [Total Enrollment in Strata(Members)], 
CAST(SUM(ISNULL(EnrollmentData.DaysCovered, 0)) AS FLOAT) AS [Days Covered],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE CAST(SUM(ISNULL(SummaryData.mb, 0)) AS float) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) as [Prevalence Rate (Users per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE CAST(SUM(ISNULL(SummaryData.dp, 0)) AS float) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) AS [Dispensing Rate (Dispensings per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(SummaryData.dp, 0)) = 0 THEN 0 ELSE CAST(SUM(ISNULL(SummaryData.ds, 0)) AS float) / CAST(SUM(ISNULL(SummaryData.dp, 0)) AS float) END, 1) AS FLOAT) AS [Days Per Dispensing],
CAST(ROUND(CASE WHEN SUM(ISNULL(SummaryData.mb, 0)) = 0 THEN  0 ELSE CAST(SUM(ISNULL(SummaryData.ds, 0)) AS float) / CAST(SUM(ISNULL(SummaryData.mb, 0)) AS float) END, 1) AS FLOAT) AS [Days per user]
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
				--enrollment AS en
				--WHERE en.year in (%YEARS%) AND en.sex IN (%SEX%) AND en.medcov='Y' AND en.drugcov='Y'
			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE drugcov='Y' and medcov='Y') AS ed
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

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, SummaryData.%NAME_FIELD%, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort, SUM(ISNULL(SummaryData.%SD_METRIC_TYPE%, 0)) DESC";
        const string AccessQuery = @"-- MFU_Pharma
-- METRIC_TYPE = DaysSupply, Dispensing
-- SD_METRIC_TYPE = ds, dp
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, SummaryData.%NAME_FIELD%, 
SUM(IIF(ISNULL(SummaryData.dp), 0, SummaryData.dp)) AS Dispensings, 
SUM(IIF(ISNULL(SummaryData.ds), 0, SummaryData.ds)) AS DaysSupply, 
SUM(IIF(ISNULL(SummaryData.mb), 0, SummaryData.mb)) AS Members, 
SUM(IIF(ISNULL(EnrollmentData.SumOfMembers), 0, EnrollmentData.SumOfMembers)) AS [Total Enrollment in Strata(Members)], 
SUM(IIF(ISNULL(EnrollmentData.DaysCovered), 0, EnrollmentData.DaysCovered)) AS [Days Covered],
ROUND(IIF([Total Enrollment in Strata(Members)] = 0, 0, Members / [Total Enrollment in Strata(Members)] * 1000), 1) as [Prevalence Rate (Users per 1000 enrollees)],
ROUND(IIF([Total Enrollment in Strata(Members)] = 0, 0, Dispensings / [Total Enrollment in Strata(Members)] * 1000), 1) AS [Dispensing Rate (Dispensings per 1000 enrollees)],
ROUND(IIF(Dispensings = 0, 0, DaysSupply / Dispensings), 1) AS [Days Per Dispensing],
ROUND(IIF(Members = 0, 0, DaysSupply / Members), 1) AS [Days per user]
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
				--enrollment AS en
				--WHERE en.year in (%YEARS%) AND en.sex IN (%SEX%) AND en.medcov='Y'
			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE drugcov='Y') AS ed
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

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, SummaryData.%NAME_FIELD%, EnrollmentData.AgeGroupSort
ORDER BY AgeGroups.Period, AgeGroups.Sex, AgeGroups.AgeGroupSort, SUM(IIF(ISNULL(SummaryData.%SD_METRIC_TYPE%), 0, SummaryData.%SD_METRIC_TYPE%)) DESC";

        const string Strat0_Query = @"-- MFU_Strat0
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '0+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64','65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '0+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64','65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";
        const string Strat0_MF_Query = @"-- MFU_Strat0
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '0+' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64','65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC";

        const string Strat10_Query = @"-- MFU_Strat10
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-1' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('0-1') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 2-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '10-14' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('10-14') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '15-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND   sex in ('F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65-74' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('65-74') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '75+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-1' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('0-1') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 2-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '10-14' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('10-14') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '15-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND   sex in ('M') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65-74' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('65-74') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '75+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat10_MF_Query = @"-- MFU_Strat10
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-1' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('0-1') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 2-4' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 5-9' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '10-14' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('10-14') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '15-18' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '19-21' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND   sex in ('M','F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65-74' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('65-74') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '75+' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat2_Query = @"-- MFU_Strat2
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, 'Under 65' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, 'Under 65' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat2_MF_Query = @"-- MFU_Strat2
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, 'Under 65' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21','22-44','45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat4_Query = @"-- MFU_Strat4
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND   sex in ('F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat4_MF_Query = @"-- MFU_Strat4
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-21' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('0-1','2-4','5-9','10-14','15-18','19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND   sex in ('M','F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat7_Query = @"-- MFU_Strat7
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('0-1','2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '10-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('10-14','15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE%
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('19-21')
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND   sex in ('F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

-----

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-4' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('0-1','2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 5-9' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '10-18' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('10-14','15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '19-21' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('19-21') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE%
WHERE period in (%PERIODS%) AND   sex in ('M') and age_group in ('22-44')
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group, sex, period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%, sex, period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";

        const string Strat7_MF_Query = @"-- MFU_Strat7
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 0-4' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('0-1','2-4') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, ' 5-9' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('5-9') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '10-18' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('10-14','15-18') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '19-21' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE%
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('19-21')
GROUP BY %NAME_FIELD%,   period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '22-44' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND   sex in ('F') and age_group in ('22-44') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '45-64' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('M','F') and age_group in ('45-64') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)

UNION ALL

(
SELECT TOP %OUTPUT_CRITERIA% %NAME_FIELD%, '65+' AS age_group,  period, SUM(Members) AS mb, SUM(dayssupply) AS ds, SUM(dispensings) AS dp
FROM %SD_TABLE% AS sd
WHERE period in (%PERIODS%) AND  sex in ('F') and age_group in ('65-74','75+') AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and drugcov = 'Y') > 0)
GROUP BY %NAME_FIELD%,  period
ORDER BY SUM(%METRIC_TYPE%) DESC
)";


    }
}
