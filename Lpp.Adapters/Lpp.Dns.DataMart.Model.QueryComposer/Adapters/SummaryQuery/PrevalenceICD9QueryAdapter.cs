using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class PrevalenceICD9QueryAdapter : QueryAdapter
    {
        public readonly Guid TermID;

        public PrevalenceICD9QueryAdapter(IDictionary<string, object> settings, Guid termID) : base(settings) 
        {
            TermID = termID;
        }

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

        protected override SummaryRequestModel ConvertToModel(DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            var criteria = request.Where.Criteria.First();

            SummaryRequestModel model = new SummaryRequestModel();
            var observationPeriodTerm = GetAllCriteriaTerms(criteria, ModelTermsFactory.YearID).FirstOrDefault();
            if (observationPeriodTerm != null)
            {
                model.StartPeriod = observationPeriodTerm.GetStringValue("StartYear");
                model.EndPeriod = observationPeriodTerm.GetStringValue("EndYear");
                model.Period = string.Join(",", QueryAdapter.ExpandYears(model).Select(y => "'" + y + "'"));//used in query
            }

            IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> codeTerms = criteria.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == TermID)).Concat(criteria.Terms.Where(t => t.Type == TermID));

            var codeTermValues = (from t in codeTerms
                                 let v = t.GetCodeSelectorValues()
                                 from c in v
                                 where c != null && !string.IsNullOrWhiteSpace(c.Code)                                 
                                 select c).GroupBy(k => k.Code.Trim()).Select(k => new { Code = k.Key, Name = k.Select(c => c.Name).FirstOrDefault() ?? k.Key }).ToArray();

            model.Codes = string.Join(",", codeTermValues.Select(c => c.Code));
            model.CodeNames = codeTermValues.Select(c => c.Name).ToArray();

            DTO.Enums.Settings settingValue;
            var set = GetAllCriteriaTerms(criteria, ModelTermsFactory.SettingID).FirstOrDefault();
            if (set.GetEnumValue("Setting", out settingValue))
            {
                model.Setting = settingValue.ToString();
            }

            //These values are pulled from the stratification section of the request json
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
            string codeCJ;
            if (args.CodeNames == null || args.CodeNames.Length == 0)
            {
                codeCJ = QueryAdapter.BuildCrossJoinClauseForICD9Diagnosis("code", codes, "sd");
            }
            else
            {
                codeCJ = QueryAdapter.BuildCrossJoinClauseForICD9Diagnosis(codes, args.CodeNames, "sd");
            }

            cjcs += "," + codeCJ;
            query = query.Replace("%CODES%", string.Join(",", codes).Replace("%comma;", ","));
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

        static void DetermineTermSpecifics(Guid termID, ref string sd_table, ref string sd_codePrefix, ref string codePrefix){
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
        }

        protected override IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            string sd_table = string.Empty;
            string sd_codePrefix = string.Empty;
            string codePrefix = string.Empty;

            DetermineTermSpecifics(TermID, ref sd_table, ref sd_codePrefix, ref codePrefix);

            return new[]{ 
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
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Prevalence Rate (Users per 1000 enrollees)", Type = "System.Double" },
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
                Select = new[]{
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

			(SELECT * FROM enrollment WHERE medcov='Y' and drugcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		--LEFT JOIN

			-- Add the names to the rows by matching against summary data.

			--(SELECT distinct %SD_CODE_FIELD%, %NAME_FIELD% FROM %SD_TABLE% WHERE %SD_CODE_FIELD% in (%CODES%)) AS nm
			--ON nm.%SD_CODE_FIELD%=AgeGroups.%CODE_FIELD%

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
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
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

			(SELECT * FROM enrollment WHERE medcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		--LEFT JOIN

			-- Add the names to the rows by matching against summary data.

			--(SELECT distinct %SD_CODE_FIELD%, %NAME_FIELD% FROM %SD_TABLE% WHERE %SD_CODE_FIELD% in (%CODES%)) AS nm
			--ON nm.%SD_CODE_FIELD%=AgeGroups.%CODE_FIELD%

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
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
		GROUP BY %SD_CODE_FIELD%, %NAME_FIELD%, Setting, age_group_id, age_group, %MATCH_SEX3% period
	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.%SD_CODE_FIELD% = EnrollmentData.%CODE_FIELD%)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.%CODE_FIELD%, EnrollmentData.%NAME_FIELD%, EnrollmentData.Setting, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

        
    }
}
