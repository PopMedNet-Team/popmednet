using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.QueryComposer;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class PrevalenceHCPCSProceduresQueryAdapter : QueryAdapter
    {
        public PrevalenceHCPCSProceduresQueryAdapter(IDictionary<string, object> settings) : base(settings) { }

        public override void Dispose()
        {
        }

        protected override bool IsMFU
        {
            get { return false; }
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

        protected override SummaryRequestModel ConvertToModel(DTO.QueryComposer.QueryComposerQueryDTO query)
        {
            var criteria = query.Where.Criteria.First();

            SummaryRequestModel model = new SummaryRequestModel();
            var observationPeriodTerm = GetAllCriteriaTerms(criteria, ModelTermsFactory.YearID).FirstOrDefault();
            if (observationPeriodTerm != null)
            {
                model.StartPeriod = observationPeriodTerm.GetStringValue("StartYear");
                model.EndPeriod = observationPeriodTerm.GetStringValue("EndYear");
                model.Period = string.Join(",", QueryAdapter.ExpandYears(model).Select(y => "'" + y + "'"));//used in query
            }

            var codeTerms = criteria.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.HCPCSProcedureCodesID)).Concat(criteria.Terms.Where(t => t.Type == ModelTermsFactory.HCPCSProcedureCodesID));
            IEnumerable<string> codeTermValues = from t in codeTerms
                                                 from v in t.GetCodeStringCollection()
                                                 where !string.IsNullOrWhiteSpace(v)
                                                 select v.Trim();

            model.Codes = string.Join(",", codeTermValues.Distinct());

            IEnumerable<string> codeNameValues = from t in codeTerms
                                                 from v in t.GetCodeNameStringCollection()
                                                 where !string.IsNullOrWhiteSpace(v)
                                                 select v.Trim();

            model.CodeNames = codeNameValues.Distinct().ToArray();

            DTO.Enums.Settings settingValue;
            var setting = GetAllCriteriaTerms(criteria, ModelTermsFactory.SettingID).FirstOrDefault();
            if (setting.GetEnumValue("Setting", out settingValue))
            {
                model.Setting = settingValue.ToString();
            }

            //These values are pulled from the stratification section of the request json
            var ageStratification = GetAgeField(query.Select.Fields.Where(f => f.Type == ModelTermsFactory.AgeRangeID));
            if (ageStratification != null)
            {
                QueryAdapter.SetAgeStratification(model, ageStratification);
            }

            var sexStratification = GetAllCriteriaTerms(criteria, ModelTermsFactory.SexID).FirstOrDefault();
            if (sexStratification != null)
            {
                QueryAdapter.SetSexStratification(model, sexStratification);
            }

            model.Coverage = null;//not applicable to this query                    
            model.MetricType = "0";//not applicable to this query
            model.OutputCriteria = "0";//not applicable to this query
            model.StartQuarter = null;//not applicable to this query
            model.EndQuarter = null;//not applicable to this query
            model.SubtypeId = 0;//value never gets set in ui of v5 summary query composer

            return model;
        }

        protected override void ApplyCrossJoinForCodes(SummaryRequestModel args, ref string query, ref string cjcs)
        {
            if (string.IsNullOrWhiteSpace(args.Codes))
                return;


            string[] codes = ParseCodeValues(args, true).ToArray();
            string codeCJ = QueryAdapter.BuildCrossJoinClauseForHCPCSProcedures("code", codes, args.CodeNames, "sd");

            cjcs += "," + codeCJ;
            query = query.Replace("%CODES%", string.Join(",", codes).Replace("%comma;", ",")); 
        }

        protected override void ReplaceParameters(ref string query)
        {
            query = query.Replace("%CODE_FIELD%", "PXCode")
                                 .Replace("%NAME_FIELD%", "PXName")
                                 .Replace("%SD_CODE_FIELD%", "px_code")
                                 .Replace("%SD_TABLE%", "HCPCS");
        }

        protected override IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            return new []{ 
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Setting", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "PXCode", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "PXName", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Prevalence Rate (Users per 1000 enrollees)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Event Rate (Events per 1000 enrollees)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events per member", Type = "System.Double" }            
            };
        }

        protected override DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetResponseAggregationDefinition()
        {
            return new DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO
            {
                GroupBy = new[] { "Period", "Sex", "AgeGroup", "PXCode", "PXName", "Setting" },
                Select = new[]{
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Setting", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "PXCode", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "PXName", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Prevalence Rate (Users per 1000 enrollees)", Type = "System.Double", Aggregate = "Average" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Event Rate (Events per 1000 enrollees)", Type = "System.Double", Aggregate = "Average" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Events per member", Type = "System.Double", Aggregate = "Average" }  
                }
            };
        }

        const string SqlQuery = @"-- Prev_ICD9_HCPCS
-- SD_CODE_FIELD = code, px_code
-- CODE_FIELD = DXCode, PXCode
-- NAME_FIELD = DXName, PXName
-- SD_TABLE = ICD9_Diagnosis, ICD9_Diagnosis_4_Digit, ICD9_Diagnosis_5_Digit, ICD9_Procedure_4_Digit, ICD9_Procedure, HCPCS

SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, EnrollmentData.Setting, EnrollmentData.%CODE_FIELD%, EnrollmentData.%NAME_FIELD%,
CAST(SUM(ISNULL(SummaryData.mb, 0)) AS FLOAT) AS Members, 
CAST(SUM(ISNULL(SummaryData.ev, 0)) AS FLOAT) AS [Events], 
CAST(SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) AS FLOAT) AS [Total Enrollment in Strata(Members)], 
CAST(SUM(ISNULL(EnrollmentData.DaysCovered, 0)) AS FLOAT) AS [Days Covered],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE SUM(ISNULL(SummaryData.mb, 0)) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) AS [Prevalence Rate (Users per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE SUM(ISNULL(SummaryData.ev, 0)) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) AS [Event Rate (Events per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(SummaryData.mb, 0)) = 0 THEN 0 ELSE SUM(ISNULL(SummaryData.ev, 0)) / SUM(ISNULL(SummaryData.mb, 0)) END, 1) AS FLOAT) AS [Events per member]
FROM

	--
	-- Age Group and Enrollment Data Section
	--
	-- This part makes sure that all age groups for all desired enrollment years, genders and codes/names are represented in the result table
	-- even if there is no summary data.
	--

	(
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		AgeGroups.%CODE_FIELD% AS %CODE_FIELD%, AgeGroups.%NAME_FIELD%, AgeGroups.Setting, 
		CAST(Sum(ed.Members) AS float) AS SumOfMembers, 
		CAST(Sum(ed.DaysCovered) AS float) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort, %SEX_AGGREGATED% AS Sex, en.Year AS Period, 
				sd.Code AS %CODE_FIELD%, sd.Name AS %NAME_FIELD%, %SETTING% AS Setting
				FROM age_groups AS ag,

				%CJC%

			) AS AgeGroups
	

		LEFT JOIN

			-- Add the enrollment data to the rows (where medical coverage is Y).

			(SELECT * FROM enrollment WHERE medcov='Y' AND drugcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.%CODE_FIELD%, AgeGroups.%NAME_FIELD%, AgeGroups.Setting
		
	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with medical coverage enrollment).
	--

	(
		SELECT %SD_CODE_FIELD%, %NAME_FIELD%, Setting, age_group_id, age_group, %MATCH_SEX3% period, 
		CAST(SUM(Members) AS float) AS mb, CAST(SUM([Events]) AS float) AS ev
		FROM %SD_TABLE% AS sd
		WHERE %SD_CODE_FIELD% IN (%CODES%)  AND period in (%PERIODS%) AND  SETTING IN (%SETTING%) AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y' and drugcov = 'Y') > 0)
		GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, age_group_id, age_group, %MATCH_SEX3% period
	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.%SD_CODE_FIELD% = EnrollmentData.%CODE_FIELD%)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.%CODE_FIELD%, EnrollmentData.%NAME_FIELD%, EnrollmentData.Setting, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

        const string AccessQuery = @"-- Prev_ICD9_HCPCS
-- SD_CODE_FIELD = code, px_code
-- CODE_FIELD = DXCode, PXCode
-- NAME_FIELD = DXName, PXName
-- SD_TABLE = ICD9_Diagnosis, ICD9_Diagnosis_4_Digit, ICD9_Diagnosis_5_Digit, ICD9_Procedure_4_Digit, ICD9_Procedure, HCPCS

SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, EnrollmentData.Setting, EnrollmentData.%CODE_FIELD%, EnrollmentData.%NAME_FIELD%,
SUM(IIF(ISNULL(SummaryData.mb), 0, SummaryData.mb)) AS Members, 
SUM(IIF(ISNULL(SummaryData.ev), 0, SummaryData.ev)) AS Events, 
SUM(IIF(ISNULL(EnrollmentData.SumOfMembers), 0, EnrollmentData.SumOfMembers)) AS [Total Enrollment in Strata(Members)], 
SUM(IIF(ISNULL(EnrollmentData.DaysCovered), 0, EnrollmentData.DaysCovered)) AS [Days Covered],
ROUND(IIF([Total Enrollment in Strata(Members)] = 0, 0, Members / [Total Enrollment in Strata(Members)] * 1000), 1) AS [Prevalence Rate (Users per 1000 enrollees)],
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
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		AgeGroups.%CODE_FIELD% AS %CODE_FIELD%, AgeGroups.%NAME_FIELD%, AgeGroups.Setting, 
		Sum(ed.Members) AS SumOfMembers, 
		Sum(ed.DaysCovered) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort, %SEX_AGGREGATED% AS Sex, en.Year AS Period, 
				sd.Code AS %CODE_FIELD%, sd.Name AS %NAME_FIELD%, %SETTING% AS Setting
				FROM age_groups AS ag,

				%CJC%

			) AS AgeGroups
	

		LEFT JOIN

			-- Add the enrollment data to the rows (where medical coverage is Y).

			(SELECT * FROM enrollment WHERE medcov='Y' AND drugcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.%CODE_FIELD%, AgeGroups.%NAME_FIELD%, AgeGroups.Setting
		
	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with medical coverage enrollment).
	--

	(
		SELECT %SD_CODE_FIELD%, %NAME_FIELD%, Setting, age_group_id, age_group, %MATCH_SEX3% period, 
		SUM(Members) AS mb, SUM(Events) AS ev
		FROM %SD_TABLE% AS sd
		WHERE %SD_CODE_FIELD% IN (%CODES%)  AND period in (%PERIODS%) AND  SETTING IN (%SETTING%) AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y' and drugcov = 'Y') > 0)
		GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, age_group_id, age_group, %MATCH_SEX3% period
	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.%SD_CODE_FIELD% = EnrollmentData.%CODE_FIELD%)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.%CODE_FIELD%, EnrollmentData.%NAME_FIELD%, EnrollmentData.Setting, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

    }
}
