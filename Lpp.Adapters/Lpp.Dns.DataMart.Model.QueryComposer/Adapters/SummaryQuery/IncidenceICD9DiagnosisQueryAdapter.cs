using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Model.Settings;
using Lpp.QueryComposer;
using Lpp.Dns.DataMart.Model.QueryComposer;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class IncidenceICD9DiagnosisQueryAdapter : QueryAdapter
    {
        public IncidenceICD9DiagnosisQueryAdapter(IDictionary<string, object> settings) : base(settings) { }

        protected override SummaryRequestModel ConvertToModel(DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            var criteria = request.Where.Criteria.First();

            SummaryRequestModel model = new SummaryRequestModel();

            //var observationPeriodTerm = criteria.Terms.FirstOrDefault(t => t.Type == ModelTermsFactory.YearID);
            var observationPeriodTerm = GetAllCriteriaTerms(criteria, ModelTermsFactory.YearID).FirstOrDefault();
            if (observationPeriodTerm != null)
            {
                model.StartPeriod = observationPeriodTerm.GetStringValue("StartYear");
                model.EndPeriod = observationPeriodTerm.GetStringValue("EndYear");
                model.Period = string.Join(",", QueryAdapter.ExpandYears(model).Select(y => "'" + y + "'"));//used in query
            }

            //var codeTerms = criteria.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.ICD9DiagnosisCodes3digitID)).Concat(criteria.Terms.Where(t => t.Type == ModelTermsFactory.ICD9DiagnosisCodes3digitID));
            var codeTerms = GetAllCriteriaTerms(criteria, ModelTermsFactory.ICD9DiagnosisCodes3digitID);
            IEnumerable<string> codeTermValues = from t in codeTerms
                                                 from v in t.GetCodeStringCollection()
                                                 where !string.IsNullOrWhiteSpace(v)
                                                 select v.Trim();

            model.Codes = string.Join(",", codeTermValues.Distinct());
            model.CodeNames = null;//this is a collection of the full names of the codes selected, original query used in the crossjoin for the name of the code, pulling from db now

            DTO.Enums.Settings settingValue;
            var setting = GetAllCriteriaTerms(criteria, ModelTermsFactory.SettingID).FirstOrDefault();
            if (setting.GetEnumValue("Setting", out settingValue))
            {
                model.Setting = settingValue.ToString();
            }

            //These values are pulled from the stratification section of the request json
            var ageStratification = GetAgeField(request.Select.Fields.Where(f => f.Type == ModelTermsFactory.AgeRangeID));
            if (ageStratification != null)
            {
                QueryAdapter.SetAgeStratification(model, ageStratification);
            }

            //var sexTerm = criteria.Criteria.Select(y => y.Terms.Where( z => z.Type == ModelTermsFactory.SexID).FirstOrDefault());
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
       
        protected override bool IsMFU
        {
            get { return false; }
        }

        protected override string Template
        {
            get {
                if (DataProvider == Settings.SQLProvider.ODBC)
                {
                    return AccessQuery;
                }
                return SqlQuery;
            }
        }

        protected override void ReplaceParameters(ref string query)
        {
            query = query.Replace("%CODE_FIELD%", "DXCode")
                         .Replace("%NAME_FIELD%", "DXName")
                         .Replace("%SD_CODE_FIELD%", "code")
                         .Replace("%SD_TABLE%", "INCIDENT_ICD9_DIAGNOSIS");
        }

        protected override void ApplyCrossJoinForCodes(SummaryRequestModel args, ref string query, ref string cjcs)
        {
            if (string.IsNullOrWhiteSpace(args.Codes))
                return;


            string[] codes = ParseCodeValues(args, true).ToArray();
            string codeCJ = QueryAdapter.BuildCrossJoinClauseForICD9Diagnosis("code", codes, "sd");

            cjcs += "," + codeCJ;
            query = query.Replace("%CODES%", string.Join(",", codes).Replace("%comma;", ","));            
        }

        public override void Dispose()
        {
        }

        protected override IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Period", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Sex", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "AgeGroup", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Setting", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DXCode", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DXName", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Members90", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Events90", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Members180", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Events180", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Members270", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Events270", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Total Enrollment in Strata(Members)", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Days Covered", Type = "System.Double" },
            };
        }

        protected override DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetResponseAggregationDefinition()
        {
            return new DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO
            {
                GroupBy = new[] { "Period", "Sex", "AgeGroup", "DXCode", "DXName", "Setting" },
                Select = new[] {
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Period", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Sex", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "AgeGroup", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Setting", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DXCode", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DXName", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Members90", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Events90", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Members180", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Events180", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Members270", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Events270", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Total Enrollment in Strata(Members)", Type = "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Days Covered", Type = "System.Double", Aggregate = "Sum" },
                }
            };
        }


        const string SqlQuery = @"-- Inci_ICD9Diag
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, 

EnrollmentData.Setting, EnrollmentData.DXCode, EnrollmentData.DXName,
CAST(SUM(ISNULL(SummaryData.m90, 0)) AS FLOAT) AS Members90, 
CAST(SUM(ISNULL(SummaryData.e90, 0)) AS FLOAT) AS Events90, 
CAST(SUM(ISNULL(SummaryData.m180, 0)) AS FLOAT) AS Members180, 
CAST(SUM(ISNULL(SummaryData.e180, 0)) AS FLOAT) AS Events180, 
CAST(SUM(ISNULL(SummaryData.m270, 0)) AS FLOAT) AS Members270, 
CAST(SUM(ISNULL(SummaryData.e270, 0)) AS FLOAT) AS Events270,

CAST(SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) AS FLOAT) AS [Total Enrollment in Strata(Members)], 
CAST(SUM(ISNULL(EnrollmentData.DaysCovered, 0)) AS FLOAT) AS [Days Covered] 

FROM

	--
	-- Age Group and Enrollment Data Section
	--
	-- This part makes sure that all age groups for all desired enrollment years, genders and codes/names are represented in the result table
	-- even if there is no summary data.
	--

	(
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		AgeGroups.DXCode, AgeGroups.DXName, AgeGroups.Setting, 
		Sum(ed.Members) AS SumOfMembers, 
		Sum(ed.DaysCovered) AS DaysCovered 

		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period, 
				sd.Code AS DXCode, sd.Name AS DXName, %SETTING% AS Setting
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

			--(SELECT distinct code, dxname FROM Incident_ICD9_Diagnosis WHERE code in (%CODES%)) AS nm
			--ON nm.code=AgeGroups.dxcode

		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.DXCode, AgeGroups.DXName, AgeGroups.Setting

	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with medical coverage enrollment).
	--

	(
		SELECT code, DXName, Setting, 

		age_group_id, age_group, %MATCH_SEX3% period, 
		SUM(Members90) AS m90, SUM(Events90) AS e90,
		SUM(Members180) AS m180, SUM(Events180) AS e180,
		SUM(Members270) AS m270, SUM(Events270) AS e270

		FROM Incident_ICD9_diagnosis AS sd

		WHERE code IN (%CODES%)  AND period in (%PERIODS%) AND  SETTING IN (%SETTING%) AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
		GROUP BY code, DXName, Setting, age_group_id, age_group, %MATCH_SEX3% period

	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.code = EnrollmentData.DXCode)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.DXCode, EnrollmentData.DXName, EnrollmentData.Setting, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

        const string AccessQuery = @"-- Inci_ICD9Diag
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, 

EnrollmentData.Setting, EnrollmentData.DXCode, EnrollmentData.DXName,
SUM(IIF(ISNULL(SummaryData.m90), 0, SummaryData.m90)) AS Members90, 
SUM(IIF(ISNULL(SummaryData.e90), 0, SummaryData.e90)) AS Events90, 
SUM(IIF(ISNULL(SummaryData.m180), 0, SummaryData.m180)) AS Members180, 
SUM(IIF(ISNULL(SummaryData.e180), 0, SummaryData.e180)) AS Events180, 
SUM(IIF(ISNULL(SummaryData.m270), 0, SummaryData.m270)) AS Members270, 
SUM(IIF(ISNULL(SummaryData.e270), 0, SummaryData.e270)) AS Events270,

SUM(IIF(ISNULL(EnrollmentData.SumOfMembers), 0, EnrollmentData.SumOfMembers)) AS [Total Enrollment in Strata(Members)], 
SUM(IIF(ISNULL(EnrollmentData.DaysCovered), 0, EnrollmentData.DaysCovered)) AS [Days Covered] 

FROM

	--
	-- Age Group and Enrollment Data Section
	--
	-- This part makes sure that all age groups for all desired enrollment years, genders and codes/names are represented in the result table
	-- even if there is no summary data.
	--

	(
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		AgeGroups.DXCode, AgeGroups.DXName, AgeGroups.Setting, 
		Sum(ed.Members) AS SumOfMembers, 
		Sum(ed.DaysCovered) AS DaysCovered 

		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period, 
				sd.Code AS DXCode, sd.Name AS DXName, %SETTING% AS Setting
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

			--(SELECT distinct code, dxname FROM Incident_ICD9_Diagnosis WHERE code in (%CODES%)) AS nm
			--ON nm.code=AgeGroups.dxcode

		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.DXCode, AgeGroups.DXName, AgeGroups.Setting

	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with medical coverage enrollment).
	--

	(
		SELECT code, DXName, Setting, 

		age_group_id, age_group, %MATCH_SEX3% period, 
		SUM(Members90) AS m90, SUM(Events90) AS e90,
		SUM(Members180) AS m180, SUM(Events180) AS e180,
		SUM(Members270) AS m270, SUM(Events270) AS e270

		FROM Incident_ICD9_diagnosis AS sd

		WHERE code IN (%CODES%)  AND period in (%PERIODS%) AND  SETTING IN (%SETTING%) AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year=sd.period and medcov = 'Y') > 0)
		GROUP BY code, DXName, Setting, age_group_id, age_group, %MATCH_SEX3% period

	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.code = EnrollmentData.DXCode)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.DXCode, EnrollmentData.DXName, EnrollmentData.Setting, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";
        
    }
}
