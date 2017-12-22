using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public enum PrevalencePharmaDispensingQueryType { DrugName = 0, DrugClass = 1 }

    public class PrevalencePharmaDispensingQueryAdapter : QueryAdapter
    {
        IncidencePharmaDispensingQueryType _queryType = IncidencePharmaDispensingQueryType.DrugClass;

        public PrevalencePharmaDispensingQueryAdapter(IDictionary<string, object> settings) : base(settings) { }

        protected override SummaryRequestModel ConvertToModel(DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            var criteria = request.Where.Criteria.First();

            SummaryRequestModel model = new SummaryRequestModel();
            var observationPeriodTerm = GetAllCriteriaTerms(criteria, ModelTermsFactory.QuarterYearID).FirstOrDefault();
            if (observationPeriodTerm != null)
            {
                model.StartPeriod = observationPeriodTerm.GetStringValue("StartYear");
                model.EndPeriod = observationPeriodTerm.GetStringValue("EndYear");                

                if(string.Equals(observationPeriodTerm.GetStringValue("ByYearsOrQuarters"), "ByQuarters", StringComparison.OrdinalIgnoreCase))
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

            IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> codeTerms = criteria.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.DrugNameID || t.Type == ModelTermsFactory.DrugClassID)).Concat(criteria.Terms.Where(t => t.Type == ModelTermsFactory.DrugNameID || t.Type == ModelTermsFactory.DrugClassID));
            if (codeTerms.Any(t => t.Type == ModelTermsFactory.DrugNameID))
            {
                _queryType = IncidencePharmaDispensingQueryType.DrugName;
            }
            else if (codeTerms.Any(t => t.Type == ModelTermsFactory.DrugClassID))
            {
                _queryType = IncidencePharmaDispensingQueryType.DrugClass;
            }
            else
            {
                throw new InvalidOperationException("Either a Drug Name term or a Drug Class term is required for the query.");
            }

            var codeTermValues = (from t in codeTerms
                                  let v = t.GetCodeSelectorValues()
                                  from c in v
                                  where c != null && !string.IsNullOrWhiteSpace(c.Code)
                                  select c).GroupBy(k => k.Code.Trim()).Select(k => new { Code = k.Key, Name = k.Select(c => c.Name).FirstOrDefault() ?? k.Key }).ToArray();

            model.Codes = string.Join(",", codeTermValues.Select(c => System.Net.WebUtility.HtmlEncode(c.Code).Replace(",", "&#44;")));
            model.CodeNames = codeTermValues.Select(c => c.Name).ToArray();

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

            model.Setting = null;//not applicable to this query
            model.Coverage = null;//not applicable to this query            
            model.MetricType = "0";//not applicable to this query
            model.OutputCriteria = "0";//not applicable to this query
            model.SubtypeId = 0;//value never gets set in ui of v5 summary query composer            

            return model;
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

        protected override void ReplaceParameters(ref string query)
        {
            if (_queryType == IncidencePharmaDispensingQueryType.DrugName)
            {
                query = query.Replace("%NAME_FIELD%", "GenericName")
                                 .Replace("%SD_TABLE%", "GENERIC_NAME");
            }
            else
            {
                query = query.Replace("%NAME_FIELD%", "DrugClass")
                                 .Replace("%SD_TABLE%", "DRUG_CLASS");
            }
        }

        protected override void ApplyCrossJoinForCodes(SummaryRequestModel args, ref string query, ref string cjcs)
        {
            if (string.IsNullOrWhiteSpace(args.Codes))
                return;

            string[] codes = ParseCodeValues(args, htmlDecode: true).ToArray();

            string codeCJ = QueryAdapter.BuildCrossJoinClause((_queryType == IncidencePharmaDispensingQueryType.DrugName) ? "GenericName" : "DrugClass", codes, "sd");

            cjcs += "," + codeCJ;
            query = query.Replace("%CODES%", string.Join(",", codes).Replace("%comma;", ","));
        }

        protected override IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            string name = _queryType == IncidencePharmaDispensingQueryType.DrugName ? "GenericName" : "DrugClass";
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
            string name = _queryType == IncidencePharmaDispensingQueryType.DrugName ? "GenericName" : "DrugClass";
            return new DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO
            {
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

        const string SqlQuery = @"-- Prev_Pharma
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, EnrollmentData.%NAME_FIELD%,
CAST(SUM(ISNULL(SummaryData.dp, 0)) AS FLOAT) AS Dispensings, 
CAST(SUM(ISNULL(SummaryData.ds, 0)) AS FLOAT) AS DaysSupply, 
CAST(SUM(ISNULL(SummaryData.mb, 0)) AS FLOAT) AS Members, 
CAST(SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) AS FLOAT) AS [Total Enrollment in Strata(Members)], 
CAST(SUM(ISNULL(EnrollmentData.DaysCovered, 0)) AS FLOAT) AS [Days Covered],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE SUM(ISNULL(SummaryData.mb, 0)) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) as [Prevalence Rate (Users per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) = 0 THEN 0 ELSE SUM(ISNULL(SummaryData.dp, 0)) / SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) * 1000 END, 1) AS FLOAT) AS [Dispensing Rate (Dispensings per 1000 enrollees)],
CAST(ROUND(CASE WHEN SUM(ISNULL(SummaryData.dp, 0)) = 0 THEN 0 ELSE SUM(ISNULL(SummaryData.ds, 0)) / SUM(ISNULL(SummaryData.dp, 0)) END, 1) AS FLOAT) AS [Days Per Dispensing],
CAST(ROUND(CASE WHEN SUM(ISNULL(SummaryData.mb, 0)) = 0 THEN 0 ELSE SUM(ISNULL(SummaryData.ds, 0)) / SUM(ISNULL(SummaryData.mb, 0)) END, 1) AS FLOAT) AS [Days per user]
FROM

	--
	-- Age Group and Enrollment Data Section
	--
	-- This part makes sure that all age groups for all desired enrollment years, genders and codes/names are represented in the result table
	-- even if there is no summary data.
	--

	(
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		AgeGroups.%NAME_FIELD%, 
		CAST(Sum(ed.Members) AS float) AS SumOfMembers, 
		CAST(Sum(ed.DaysCovered) AS float) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period, sd.%NAME_FIELD%
				FROM age_groups AS ag, 

				%CJC%

				--enrollment AS en, %SD_TABLE% AS sd
				--WHERE en.year in (%YEARS%) AND en.sex IN (%SEX%) AND en.drugcov='Y' AND sd.%NAME_FIELD% in (%CODES%)
			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE drugcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		--WHERE ed.drugcov='Y'
		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.%NAME_FIELD%
	
	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with drug coverage enrollment).
	--

	(
		SELECT %NAME_FIELD%, age_group_id, age_group, %MATCH_SEX3% period, 
		CAST(SUM(Members) AS float) AS mb, CAST(SUM(ISNULL(Dispensings, 0)) AS float) AS dp, CAST(SUM(ISNULL(DaysSupply, 0)) AS float) AS ds
		FROM %SD_TABLE% AS sd
		WHERE %NAME_FIELD% IN (%CODES%)  AND period in (%PERIODS%) AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year IN (%YEARS%) and drugcov = 'Y') > 0)
		GROUP BY %NAME_FIELD%, age_group_id, age_group, %MATCH_SEX3% period
	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.%NAME_FIELD% = EnrollmentData.%NAME_FIELD%)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.%NAME_FIELD%, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

        const string AccessQuery = @"-- Prev_Pharma
SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, EnrollmentData.%NAME_FIELD%,
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
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, 
		AgeGroups.%NAME_FIELD%, 
		Sum(ed.Members) AS SumOfMembers, 
		Sum(ed.DaysCovered) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period, sd.%NAME_FIELD%
				FROM age_groups AS ag, 

				%CJC%

				--enrollment AS en, %SD_TABLE% AS sd
				--WHERE en.year in (%YEARS%) AND en.sex IN (%SEX%) AND en.drugcov='Y' AND sd.%NAME_FIELD% in (%CODES%)
			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE drugcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
		)

		--WHERE ed.drugcov='Y'
		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.%NAME_FIELD%
	
	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with drug coverage enrollment).
	--

	(
		SELECT %NAME_FIELD%, age_group_id, age_group, %MATCH_SEX3% period, 
		SUM(Members) AS mb, SUM(Dispensings) AS dp, SUM(DaysSupply) AS ds
		FROM %SD_TABLE% AS sd
		WHERE %NAME_FIELD% IN (%CODES%)  AND period in (%PERIODS%) AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year IN (%YEARS%) and drugcov = 'Y') > 0)
		GROUP BY %NAME_FIELD%, age_group_id, age_group, %MATCH_SEX3% period
	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.%NAME_FIELD% = EnrollmentData.%NAME_FIELD%)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.%NAME_FIELD%, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

    }
}
