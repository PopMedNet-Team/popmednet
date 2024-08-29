using Lpp.QueryComposer;
using Lpp.Dns.DTO.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class PrevalenceEnrollmentQueryAdapter : QueryAdapter
    {

        public PrevalenceEnrollmentQueryAdapter(IDictionary<string, object> settings) : base(settings) { }

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

            DTO.Enums.Coverages coverage;
            var cov = GetAllCriteriaTerms(criteria, ModelTermsFactory.CoverageID).FirstOrDefault();
            if (cov.GetEnumValue("Coverage", out coverage))
            {
                model.Coverage = coverage;
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

            model.Codes = null;//not applicable to this query
            model.CodeNames = null;//not applicable to this query
            model.Setting = null;//not applicable to this query                      
            model.MetricType = "0";//not applicable to this query
            model.OutputCriteria = "0";//not applicable to this query
            model.StartQuarter = null;//not applicable to this query
            model.EndQuarter = null;//not applicable to this query
            model.SubtypeId = 0;//value never gets set in ui of v5 summary query composer

            return model;
        }

        protected override void ReplaceParameters(ref string query)
        {
            //no adapter specific parameters to replace
        }

        protected override IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            return new []{ 
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Year", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "MedCov", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DrugCov", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Decimal" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Decimal" }            
            };
        }

        protected override DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetResponseAggregationDefinition()
        {
            return new DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO {
                GroupBy = new []{ "Year", "Sex", "AgeGroup", "DrugCov", "MedCov" },
                Select = new []{
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Year", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "MedCov", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DrugCov", Type = "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type = "System.Decimal", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type = "System.Decimal", Aggregate = "Sum" }  
                }
            };
        }

        const string SqlQuery = @"-- Prev_Enroll
-- SEX_AGGREGATION = en.sex, 'All' (if M/F aggregated)
-- MATCH_SEX = AND ed.Sex = AgeGroups.Sex (if M/F aggregated)
SELECT EnrollmentData.AgeGroup, EnrollmentData.Sex AS Sex, EnrollmentData.Period AS Year, EnrollmentData.MedCov, EnrollmentData.DrugCov,

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
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, %MEDCOV_AGGREGATED% AS MedCov, %DRUGCOV_AGGREGATED% AS DrugCov,
		Sum(ed.Members) AS SumOfMembers, 
		Sum(ed.DaysCovered) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort, %SEX_AGGREGATED% AS Sex, en.Year AS Period
				FROM age_groups AS ag, 

				%CJC%

			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE medcov IN (%MEDCOV%) AND drugcov IN (%DRUGCOV%)) AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX% AND ed.Year = AgeGroups.Period 
		)

		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, ed.DrugCov, ed.MedCov

	) AS EnrollmentData

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.AgeGroupSort, EnrollmentData.MedCov, EnrollmentData.DrugCov
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

        const string AccessQuery = @"-- Prev_Enroll
-- SEX_AGGREGATION = en.sex, 'All' (if M/F aggregated)
-- MATCH_SEX = AND ed.Sex = AgeGroups.Sex (if M/F aggregated)
SELECT EnrollmentData.AgeGroup, EnrollmentData.Sex AS Sex, EnrollmentData.Period AS Year, EnrollmentData.MedCov, EnrollmentData.DrugCov,

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
		SELECT AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, %MEDCOV_AGGREGATED% AS MedCov, %DRUGCOV_AGGREGATED% AS DrugCov,
		Sum(ed.Members) AS SumOfMembers, 
		Sum(ed.DaysCovered) AS DaysCovered 
		FROM

		(
			-- Cross join ensures all age groups for all desired enrollment years, genders and codes/names are represented in the result table.

			(
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort, %SEX_AGGREGATED% AS Sex, en.Year AS Period
				FROM age_groups AS ag, 

				%CJC%

			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE medcov IN (%MEDCOV%) AND drugcov IN (%DRUGCOV%)) AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX% AND ed.Year = AgeGroups.Period 
		)

		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, ed.DrugCov, ed.MedCov

	) AS EnrollmentData

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.AgeGroupSort, EnrollmentData.MedCov, EnrollmentData.DrugCov
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";
    }
}
