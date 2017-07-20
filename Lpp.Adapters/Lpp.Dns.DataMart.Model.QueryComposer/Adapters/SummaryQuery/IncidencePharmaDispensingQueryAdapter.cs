using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Model.Settings;
using Lpp.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public enum IncidencePharmaDispensingQueryType { DrugName = 0, DrugClass = 1 }

    public class IncidencePharmaDispensingQueryAdapter : QueryAdapter
    {

        IncidencePharmaDispensingQueryType _queryType = IncidencePharmaDispensingQueryType.DrugClass;

        public IncidencePharmaDispensingQueryAdapter(IDictionary<string, object> settings) : base(settings) { }

        protected override SummaryRequestModel ConvertToModel(DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            var criteria = request.Where.Criteria.First();

            SummaryRequestModel model = new SummaryRequestModel();
            
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

            IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> codeTerms = criteria.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.DrugClassID || t.Type == ModelTermsFactory.DrugNameID)).Concat(criteria.Terms.Where(t => t.Type == ModelTermsFactory.DrugClassID || t.Type == ModelTermsFactory.DrugNameID)).ToArray();
            if (codeTerms.Any(t => t.Type == ModelTermsFactory.DrugNameID))
            {
                _queryType = IncidencePharmaDispensingQueryType.DrugName;
            }
            else if(codeTerms.Any(t => t.Type == ModelTermsFactory.DrugClassID))
            {
                _queryType = IncidencePharmaDispensingQueryType.DrugClass;
            }
            else
            {
                throw new InvalidOperationException("Either a Drug Name term or a Drug Class term is required for the query.");
            }

            IEnumerable<string> codeTermValues = from t in codeTerms
                                                 from v in t.GetCodeStringCollection()
                                                 where !string.IsNullOrWhiteSpace(v)
                                                 select v.Trim();
            
            model.Codes = string.Join(",", codeTermValues.Distinct().Select(c => System.Net.WebUtility.HtmlEncode(c).Replace(",", "&#44;")));
            model.CodeNames = null;//not applicable to this query

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
                                     .Replace("%SD_TABLE%", "INCIDENT_GENERIC_NAME");
            }
            else
            {
                query = query.Replace("%NAME_FIELD%", "DrugClass")
                                                 .Replace("%SD_TABLE%", "INCIDENT_DRUG_CLASS");
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
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type= "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type= "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type= "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = name, Type= "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Episodespans90", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings90", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaySupply90", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q1", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q2", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q3", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q4", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Episodespans180", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings180", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaySupply180", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q1", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q2", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q3", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q4", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Episodespans270", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings270", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaySupply270", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q1", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q2", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q3", Type= "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q4", Type= "System.Double" },
            };
        }

        protected override DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetResponseAggregationDefinition()
        {
            string name = _queryType == IncidencePharmaDispensingQueryType.DrugName ? "GenericName" : "DrugClass";
            return new DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO 
            {
                GroupBy = new[] { "Period", "Sex", "AgeGroup", name },
                Select = new[] {
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Period", Type= "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Sex", Type= "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "AgeGroup", Type= "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = name, Type= "System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Total Enrollment in Strata(Members)", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings90", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaySupply90", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q1", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q2", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q3", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members90Q4", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings180", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaySupply180", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q1", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q2", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q3", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members180Q4", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Dispensings270", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "DaySupply270", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q1", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q2", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q3", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Members270Q4", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Days Covered", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Episodespans90", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Episodespans180", Type= "System.Double", Aggregate = "Sum" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name = "Episodespans270", Type= "System.Double", Aggregate = "Sum" },
                }
            };
        }


        const string SqlQuery = @"-- Inci_Pharma
-- NAME_FIELD = DrugClass, GenericName
-- DRUG_TABLE = Incident_Drug_Class, Incident_Generic_Name

SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, 

EnrollmentData.%NAME_FIELD%,

CAST(SUM(ISNULL(EnrollmentData.SumOfMembers, 0)) AS FLOAT) AS [Total Enrollment in Strata(Members)], 
CAST(SUM(ISNULL(EnrollmentData.DaysCovered, 0)) AS FLOAT) AS [Days Covered],

CAST(SUM(ISNULL(SummaryData.m90, 0)) AS FLOAT) AS Members90, 
CAST(SUM(ISNULL(SummaryData.e90, 0)) AS FLOAT) AS Episodespans90, 
CAST(SUM(ISNULL(SummaryData.d90, 0)) AS FLOAT) AS Dispensings90, 
CAST(SUM(ISNULL(SummaryData.ds90, 0)) AS FLOAT) AS DaySupply90, 
CAST(SUM(ISNULL(SummaryData.m90Q1, 0)) AS FLOAT) AS Members90Q1, 
CAST(SUM(ISNULL(SummaryData.m90Q2, 0)) AS FLOAT) AS Members90Q2, 
CAST(SUM(ISNULL(SummaryData.m90Q3, 0)) AS FLOAT) AS Members90Q3, 
CAST(SUM(ISNULL(SummaryData.m90Q4, 0)) AS FLOAT) AS Members90Q4, 
CAST(SUM(ISNULL(SummaryData.m180, 0)) AS FLOAT) AS Members180, 
CAST(SUM(ISNULL(SummaryData.e180, 0)) AS FLOAT) AS Episodespans180, 
CAST(SUM(ISNULL(SummaryData.d180, 0)) AS FLOAT) AS Dispensings180, 
CAST(SUM(ISNULL(SummaryData.ds180, 0)) AS FLOAT) AS DaySupply180, 
CAST(SUM(ISNULL(SummaryData.m180Q1, 0)) AS FLOAT) AS Members180Q1, 
CAST(SUM(ISNULL(SummaryData.m180Q2, 0)) AS FLOAT) AS Members180Q2, 
CAST(SUM(ISNULL(SummaryData.m180Q3, 0)) AS FLOAT) AS Members180Q3, 
CAST(SUM(ISNULL(SummaryData.m180Q4, 0)) AS FLOAT) AS Members180Q4, 
CAST(SUM(ISNULL(SummaryData.m270, 0)) AS FLOAT) AS Members270,
CAST(SUM(ISNULL(SummaryData.e180, 0)) AS FLOAT) AS Episodespans270, 
CAST(SUM(ISNULL(SummaryData.d180, 0)) AS FLOAT) AS Dispensings270, 
CAST(SUM(ISNULL(SummaryData.ds180, 0)) AS FLOAT) AS DaySupply270, 
CAST(SUM(ISNULL(SummaryData.m180Q1, 0)) AS FLOAT) AS Members270Q1, 
CAST(SUM(ISNULL(SummaryData.m180Q2, 0)) AS FLOAT) AS Members270Q2, 
CAST(SUM(ISNULL(SummaryData.m180Q3, 0)) AS FLOAT) AS Members270Q3, 
CAST(SUM(ISNULL(SummaryData.m180Q4, 0)) AS FLOAT) AS Members270Q4

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
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period, 
				sd.%NAME_FIELD% AS %NAME_FIELD%
				FROM age_groups AS ag, 

				%CJC%

			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE drugcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
			
		)
		
		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.%NAME_FIELD%

	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with drug coverage enrollment).
	--

	(
		SELECT %NAME_FIELD%, 

		age_group_id, age_group, %MATCH_SEX3% period, 

		Sum(Members90) as m90, 
		Sum(Episodespan90) as e90, 
		Sum(Dispensings90) as d90, 
		Sum(DaysSupply90) as ds90,  
		Sum(Members90Q1) as m90Q1, 
		Sum(Members90Q2) as m90Q2, 
		Sum(Members90Q3) as m90Q3, 
		Sum(Members90Q4) as m90Q4,  
		Sum(Members180) as m180, 
		Sum(Episodespan180) as e180, 
		Sum(Dispensings180) as d180, 
		Sum(DaysSupply180) as ds180,  
		Sum(Members180Q1) as m180Q1, 
		Sum(Members180Q2) as m180Q2, 
		Sum(Members180Q3) as m180Q3, 
		Sum(Members180Q4) as m180Q4,  
		Sum(Members270) as m270, 
		Sum(Episodespan270) as e270, 
		Sum(Dispensings270) as d270, 
		Sum(DaysSupply270) as ds270,  
		Sum(Members270Q1) as m270Q1, 
		Sum(Members270Q2) as m270Q2, 
		Sum(Members270Q3) as m270Q3, 
		Sum(Members270Q4) as m270Q4  

		FROM %SD_TABLE% AS sd
		WHERE %NAME_FIELD% IN (%CODES%)  AND period in (%PERIODS%) AND 
		      ((SELECT COUNT(age_group_id) FROM enrollment WHERE age_group_id=sd.age_group_id and sex=sd.sex and year IN (%YEARS%) and drugcov = 'Y') > 0) 
		GROUP BY %NAME_FIELD%, age_group_id, age_group, %MATCH_SEX3% period

	) AS SummaryData

ON (SummaryData.age_group_id = EnrollmentData.agegroupid %MATCH_SEX2% and SummaryData.Period = EnrollmentData.Period and SummaryData.%NAME_FIELD% = EnrollmentData.%NAME_FIELD%)

GROUP BY EnrollmentData.AgeGroup, EnrollmentData.Sex, EnrollmentData.Period, EnrollmentData.%NAME_FIELD%, EnrollmentData.AgeGroupSort
ORDER BY EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroupSort";

        const string AccessQuery = @"-- Inci_Pharma
-- NAME_FIELD = DrugClass, GenericName
-- DRUG_TABLE = Incident_Drug_Class, Incident_Generic_Name

SELECT EnrollmentData.Period, EnrollmentData.Sex, EnrollmentData.AgeGroup, 

EnrollmentData.%NAME_FIELD%,

SUM(IIF(ISNULL(EnrollmentData.SumOfMembers), 0, EnrollmentData.SumOfMembers)) AS [Total Enrollment in Strata(Members)], 
SUM(IIF(ISNULL(EnrollmentData.DaysCovered), 0, EnrollmentData.DaysCovered)) AS [Days Covered],

SUM(IIF(ISNULL(SummaryData.m90), 0, SummaryData.m90)) AS Members90, 
SUM(IIF(ISNULL(SummaryData.e90), 0, SummaryData.e90)) AS Episodespans90, 
SUM(IIF(ISNULL(SummaryData.d90), 0, SummaryData.d90)) AS Dispensings90, 
SUM(IIF(ISNULL(SummaryData.ds90), 0, SummaryData.ds90)) AS DaySupply90, 
SUM(IIF(ISNULL(SummaryData.m90Q1), 0, SummaryData.m90Q1)) AS Members90Q1, 
SUM(IIF(ISNULL(SummaryData.m90Q2), 0, SummaryData.m90Q2)) AS Members90Q2, 
SUM(IIF(ISNULL(SummaryData.m90Q3), 0, SummaryData.m90Q3)) AS Members90Q3, 
SUM(IIF(ISNULL(SummaryData.m90Q4), 0, SummaryData.m90Q4)) AS Members90Q4, 
SUM(IIF(ISNULL(SummaryData.m180), 0, SummaryData.m180)) AS Members180, 
SUM(IIF(ISNULL(SummaryData.e180), 0, SummaryData.e180)) AS Episodespans180, 
SUM(IIF(ISNULL(SummaryData.d180), 0, SummaryData.d180)) AS Dispensings180, 
SUM(IIF(ISNULL(SummaryData.ds180), 0, SummaryData.ds180)) AS DaySupply180, 
SUM(IIF(ISNULL(SummaryData.m180Q1), 0, SummaryData.m180Q1)) AS Members180Q1, 
SUM(IIF(ISNULL(SummaryData.m180Q2), 0, SummaryData.m180Q2)) AS Members180Q2, 
SUM(IIF(ISNULL(SummaryData.m180Q3), 0, SummaryData.m180Q3)) AS Members180Q3, 
SUM(IIF(ISNULL(SummaryData.m180Q4), 0, SummaryData.m180Q4)) AS Members180Q4, 
SUM(IIF(ISNULL(SummaryData.m270), 0, SummaryData.m270)) AS Members270,
SUM(IIF(ISNULL(SummaryData.e180), 0, SummaryData.e270)) AS Episodespans270, 
SUM(IIF(ISNULL(SummaryData.d180), 0, SummaryData.d270)) AS Dispensings270, 
SUM(IIF(ISNULL(SummaryData.ds180), 0, SummaryData.ds270)) AS DaySupply270, 
SUM(IIF(ISNULL(SummaryData.m180Q1), 0, SummaryData.m270Q1)) AS Members270Q1, 
SUM(IIF(ISNULL(SummaryData.m180Q2), 0, SummaryData.m270Q2)) AS Members270Q2, 
SUM(IIF(ISNULL(SummaryData.m180Q3), 0, SummaryData.m270Q3)) AS Members270Q3, 
SUM(IIF(ISNULL(SummaryData.m180Q4), 0, SummaryData.m270Q4)) AS Members270Q4

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
				SELECT distinct ag.id AS AgeGroupId, ag.%STRATIFICATION%_name AS AgeGroup, ag.%STRATIFICATION%_sort_order AS AgeGroupSort,  %SEX_AGGREGATED% AS Sex, en.Year AS Period, 
				sd.%NAME_FIELD% AS %NAME_FIELD%
				FROM age_groups AS ag, 

				%CJC%

			) AS AgeGroups

		LEFT JOIN

			-- Add the enrollment data to the rows (where drug coverage is Y).

			(SELECT * FROM enrollment WHERE drugcov='Y') AS ed
			ON ed.age_group_id = AgeGroups.AgeGroupId %MATCH_SEX%  AND ed.Year = AgeGroups.Period
			
		)
		
		GROUP BY AgeGroups.AgeGroupId, AgeGroups.AgeGroup, AgeGroups.AgeGroupSort, AgeGroups.Sex, AgeGroups.Period, AgeGroups.%NAME_FIELD%

	) AS EnrollmentData

LEFT JOIN

	--
	-- Summary Data Section
	--
	-- Now add the corresponding summary data to the table (for those with drug coverage enrollment).
	--

	(
		SELECT %NAME_FIELD%, 

		age_group_id, age_group, %MATCH_SEX3% period, 

		Sum(Members90) as m90, 
		Sum(Episodespan90) as e90, 
		Sum(Dispensings90) as d90, 
		Sum(DaysSupply90) as ds90,  
		Sum(Members90Q1) as m90Q1, 
		Sum(Members90Q2) as m90Q2, 
		Sum(Members90Q3) as m90Q3, 
		Sum(Members90Q4) as m90Q4,  
		Sum(Members180) as m180, 
		Sum(Episodespan180) as e180, 
		Sum(Dispensings180) as d180, 
		Sum(DaysSupply180) as ds180,  
		Sum(Members180Q1) as m180Q1, 
		Sum(Members180Q2) as m180Q2, 
		Sum(Members180Q3) as m180Q3, 
		Sum(Members180Q4) as m180Q4,  
		Sum(Members270) as m270, 
		Sum(Episodespan270) as e270, 
		Sum(Dispensings270) as d270, 
		Sum(DaysSupply270) as ds270,  
		Sum(Members270Q1) as m270Q1, 
		Sum(Members270Q2) as m270Q2, 
		Sum(Members270Q3) as m270Q3, 
		Sum(Members270Q4) as m270Q4  

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
