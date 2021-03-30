using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.DataMart.Model.ESPQueryBuilder;
using System.Linq.Expressions;
using Lpp.QueryComposer;
using System.Data;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.ESP
{
    public class ESPModelAdapter : ModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string _connectionString = string.Empty;        
        DataContext db = null;

        public ESPModelAdapter(RequestMetadata requestMetadata) : base(new Guid("7C69584A-5602-4FC0-9F3F-A27F329B1113"), requestMetadata) { }

        public override void Initialize(IDictionary<string, object> settings, string requestId)
        {
            base.Initialize(settings, requestId);

            _connectionString = Utilities.BuildConnectionString(settings, logger);
            db = DataContext.Create(_connectionString);

            db.Database.Log = (sql) => {
                if(!string.IsNullOrWhiteSpace(sql))
                    logger.Debug(sql);
            };            
        }

        protected override string[] LowThresholdColumns(QueryComposerResponseQueryResultDTO response)
        {
            return new string[] {"Patients"};
        }

        public override IEnumerable<QueryComposerResponseQueryResultDTO> Execute(QueryComposerQueryDTO query, bool viewSQL)
        {
            //This is based on the xsl for QueryComposer in The ESPQueryBuilder project 

            //TODO: need to see how multiple criteria are handled by the current xsl
            if(query.Where.Criteria.SelectMany(c => GetAllTermsWithinCriteria(c, ModelTermsFactory.ESPDiagnosisCodesID)).Any())
            {
                throw new NotImplementedException("The Term ESP Diagnosis Codes is currently not implemented and cannot be apart of the query.");
            }
            bool hasSQLTerm = query.Where.Criteria.SelectMany(c => GetAllTermsWithinCriteria(c, ModelTermsFactory.SqlDistributionID)).Any();

            if (hasSQLTerm)
            {
                if (query.Where.Criteria.Where(c => c.Terms.Any(d => d.Type != ModelTermsFactory.SqlDistributionID)).Any())
                {
                    throw new NotSupportedException("Another Term is Included with Sql Distribution and this is not Supported");
                }
                var sqlCriteria = query.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.SqlDistributionID);
                string sql = (sqlCriteria.GetStringValue("Sql"));

                List<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> columnProperties = new List<QueryComposerResponsePropertyDefinitionDTO>();
                var results = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, QueryStart = DateTimeOffset.UtcNow };

                List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        int noNameIndex = 1;
                        DataTable schemaTable = reader.GetSchemaTable();
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            foreach (DataColumn column in schemaTable.Columns)
                            {
                                if (column.ColumnName == "ColumnName")
                                {
                                    string columnName = row[column].ToString();
                                    if (string.IsNullOrWhiteSpace(columnName))
                                    {
                                        columnName = "NoColumnName" + noNameIndex;
                                        noNameIndex++;
                                    }
                                    columnProperties.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = columnName, Type = column.DataType.FullName });
                                }
                            }
                        }

                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            //have to enumerate over the record using ordinal index since there may not be a column name in the reader
                            for (int i = 0; i < columnProperties.Count; i++)
                            {
                                row.Add(columnProperties[i].Name, reader.GetValue(i));
                            }
                            queryResults.Add(row);
                        }
                        reader.Close();
                    }
                }

                results.QueryEnd = DateTimeOffset.UtcNow;
                results.Results = new[] { queryResults };
                results.Properties = columnProperties;

                return new[] { results };
            }
            else
            {
                bool hasConditionsTerm = query.Where.Criteria.SelectMany(c => GetAllTermsWithinCriteria(c, ModelTermsFactory.ConditionsID)).Any();
                bool hasICD9CodesTerm = query.Where.Criteria.SelectMany(c => GetAllTermsWithinCriteria(c, ModelTermsFactory.ICD9DiagnosisCodes3digitID)).Any();

                var firstCriteria = query.Where.Criteria.First();
                QueryComposerTermDTO ageRangeTerm = GetAllTermsWithinCriteria(firstCriteria, ModelTermsFactory.AgeRangeID).FirstOrDefault();

                var diagnosisQuery = from d in db.Diagnosis select d;
                var diseasesQuery = from d in db.Diseases select d;
                var encounterQuery = db.Demographics.Join(db.Encounters, o => o.PatID, i => i.PatID, (o, i) => new { o.PatID, i.EncounterID, i.AgeGroup5yr, i.AgeGroup10yr, i.A_Date, i.AgeAtEncYear });

                if (ageRangeTerm != null)
                {
                    var ageRange = AdapterHelpers.ParseAgeRangeValues(ageRangeTerm);
                    if (ageRange.MinAge.HasValue)
                    {
                        diagnosisQuery = diagnosisQuery.Where(q => q.AgeAtEncYear >= ageRange.MinAge.Value);
                        diseasesQuery = diseasesQuery.Where(q => q.AgeAtDetectYear >= ageRange.MinAge.Value);
                        encounterQuery = encounterQuery.Where(q => q.AgeAtEncYear >= ageRange.MinAge.Value);
                    }
                    else if (ageRange.MaxAge.HasValue)
                    {
                        diagnosisQuery = diagnosisQuery.Where(q => q.AgeAtEncYear <= ageRange.MaxAge.Value);
                        diseasesQuery = diseasesQuery.Where(q => q.AgeAtDetectYear <= ageRange.MaxAge.Value);
                        encounterQuery = encounterQuery.Where(q => q.AgeAtEncYear <= ageRange.MaxAge.Value);
                    }
                }

                //var observationPeriod = firstCriteria.Terms.FirstOrDefault(t => t.Type == ModelTermsFactory.ObservationPeriodID);
                var observationPeriod = GetAllTermsWithinCriteria(firstCriteria, ModelTermsFactory.ObservationPeriodID).FirstOrDefault();
                if (observationPeriod != null)
                {
                    var obp_range = AdapterHelpers.ParseDateRangeValues(observationPeriod);

                    if (obp_range.StartDate.HasValue)
                    {
                        int startDate = ConvertDateToNumberOfDays(obp_range.StartDate.Value);
                        diagnosisQuery = diagnosisQuery.Where(q => q.A_Date >= startDate);
                        diseasesQuery = diseasesQuery.Where(q => q.Date >= startDate);
                        encounterQuery = encounterQuery.Where(q => q.A_Date >= startDate);
                    }
                    if (obp_range.EndDate.HasValue)
                    {
                        int endDate = ConvertDateToNumberOfDays(obp_range.EndDate.Value);
                        diagnosisQuery = diagnosisQuery.Where(q => q.A_Date <= endDate);
                        diseasesQuery = diseasesQuery.Where(q => q.Date <= endDate);
                        encounterQuery = encounterQuery.Where(q => q.A_Date <= endDate);
                    }
                }


                //TODO: need to determine the precision of icd9 code from somewhere
                int icd9Precision = hasICD9CodesTerm ? 3 : 0;//0 == exclude, 3,4,5

                var entityQuery = from demographics in db.Demographics
                            join race in db.UVT_Race on demographics.Race equals race.Code
                            join sex in db.UVT_Sex on demographics.Sex equals sex.Code
                            join ethnicity in db.UVT_Race_Ethnicity on demographics.Ethnicity equals ethnicity.Code
                            select new ESPQueryResult
                            {
                                PatientID = demographics.PatID,
                                Patients = null,
                                Sex = sex.Text,
                                EthnicityCode = demographics.Ethnicity,
                                Ethnicity = ethnicity.Text,
                                Race = race.Text,
                                Zip = demographics.Zip5,
                                TobaccoUse = demographics.Smoking,
                                Code = null,
                                CodeDescription = null,
                                Disease = null,
                                ObservationPeriod = null,
                                Age_10yrGroup = null,
                                Age_5yrGroup = null,
                                Age_Detect = null,
                                CenterID = null
                            };

                if (icd9Precision == 3)
                {
                    entityQuery = from o in entityQuery
                            from diagnosis_inc in diagnosisQuery.Where(d => d.PatID == o.PatientID).DefaultIfEmpty()
                            from dx in db.UVT_Dx3Digit.Where(x => x.Code == diagnosis_inc.DxCode3digit).DefaultIfEmpty()
                            select new ESPQueryResult
                            {
                                PatientID = o.PatientID,
                                Patients = null,
                                Sex = o.Sex,
                                EthnicityCode = o.EthnicityCode,
                                Ethnicity = o.Ethnicity,
                                Race = o.Race,
                                Zip = o.Zip,
                                TobaccoUse = o.TobaccoUse,

                                Code = dx.Code,
                                CodeDescription = dx.Text,

                                Disease = o.Disease,
                                //NOTE: Value in db is # of days from 1960-01-01
                                ObservationPeriod = diagnosis_inc.A_Date,

                                Age_10yrGroup = diagnosis_inc.AgeGroup10yr,
                                Age_5yrGroup = diagnosis_inc.AgeGroup5yr,
                                Age_Detect = diagnosis_inc.AgeAtEncYear,

                                CenterID = diagnosis_inc.CenterID
                            };
                }
                else if (icd9Precision == 4)
                {
                    entityQuery = from o in entityQuery
                            from diagnosis_inc in diagnosisQuery.Where(d => d.PatID == o.PatientID).DefaultIfEmpty()
                            from dx in db.UVT_Dx4Digit.Where(x => x.Code == diagnosis_inc.DxCode4digitWithDec).DefaultIfEmpty()
                            select new ESPQueryResult
                            {
                                PatientID = o.PatientID,
                                Patients = null,
                                Sex = o.Sex,
                                EthnicityCode = o.EthnicityCode,
                                Ethnicity = o.Ethnicity,
                                Race = o.Race,
                                Zip = o.Zip,
                                TobaccoUse = o.TobaccoUse,

                                Code = dx.Code,
                                CodeDescription = dx.Text,

                                Disease = o.Disease,
                                //NOTE: Value in db is # of days from 1960-01-01
                                ObservationPeriod = diagnosis_inc.A_Date,

                                Age_10yrGroup = diagnosis_inc.AgeGroup10yr,
                                Age_5yrGroup = diagnosis_inc.AgeGroup5yr,
                                Age_Detect = diagnosis_inc.AgeAtEncYear,

                                CenterID = diagnosis_inc.CenterID
                            };
                }
                else if (icd9Precision == 5)
                {
                    entityQuery = from o in entityQuery
                            from diagnosis_inc in diagnosisQuery.Where(d => d.PatID == o.PatientID).DefaultIfEmpty()
                            from dx in db.UVT_Dx5Digit.Where(x => x.Code == diagnosis_inc.DxCode5digitWithDec).DefaultIfEmpty()
                            select new ESPQueryResult
                            {
                                PatientID = o.PatientID,
                                Patients = null,
                                Sex = o.Sex,
                                EthnicityCode = o.EthnicityCode,
                                Ethnicity = o.Ethnicity,
                                Race = o.Race,
                                Zip = o.Zip,
                                TobaccoUse = o.TobaccoUse,

                                Code = dx.Code,
                                CodeDescription = dx.Text,

                                Disease = o.Disease,
                                //NOTE: Value in db is # of days from 1960-01-01
                                ObservationPeriod = diagnosis_inc.A_Date,

                                Age_10yrGroup = diagnosis_inc.AgeGroup10yr,
                                Age_5yrGroup = diagnosis_inc.AgeGroup5yr,
                                Age_Detect = diagnosis_inc.AgeAtEncYear,

                                CenterID = diagnosis_inc.CenterID
                            };
                }

                if (hasConditionsTerm)
                {
                    entityQuery = from o in entityQuery
                            from disease_inc in diseasesQuery.Where(d => d.PatID == o.PatientID).DefaultIfEmpty()
                            select new ESPQueryResult
                            {
                                PatientID = o.PatientID,
                                Patients = null,
                                Sex = o.Sex,
                                EthnicityCode = o.EthnicityCode,
                                Ethnicity = o.Ethnicity,
                                Race = o.Race,
                                Zip = o.Zip,
                                TobaccoUse = o.TobaccoUse,

                                Code = o.Code,
                                CodeDescription = o.CodeDescription,

                                Disease = disease_inc.Condition,
                                //NOTE: Value in db is # of days from 1960-01-01
                                ObservationPeriod = disease_inc.Date,

                                Age_10yrGroup = disease_inc.AgeGroup10yr,
                                Age_5yrGroup = disease_inc.AgeGroup5yr,
                                Age_Detect = disease_inc.AgeAtDetectYear,

                                //center ID is not set in xsl when has visits
                                CenterID = o.CenterID
                            };
                }

                List<string> selectProperties = new List<string> { "Patients" };//Patient aggregate is always included.

                bool primaryCriteria = true;
                foreach (var criteria in query.Where.Criteria)
                {
                    if (!primaryCriteria && criteria.Exclusion)
                    {
                        //exclusion is a not exists in the where clause
                        var subQuery = ApplyCriteria(criteria);
                        entityQuery = entityQuery.Where(q => !subQuery.Where(s => s.PatID == q.PatientID).Any());
                    }
                    else if (!primaryCriteria)
                    {
                        var subQuery = ApplyCriteria(criteria);
                        entityQuery = entityQuery.Where(q => subQuery.Where(s => s.PatID == q.PatientID).Any());
                    }
                    else
                    {
                        primaryCriteria = false;
                        //process the first criteria as the primary query

                        Expression<Func<ESPQueryResult, bool>> icdDiseasePredicate = null;

                        foreach (var tGroup in (criteria.Criteria.SelectMany(c => c.Terms).Concat(criteria.Terms)).GroupBy(k => k.Type))
                        {
                            var termID = tGroup.Key;

                            if (termID == Lpp.QueryComposer.ModelTermsFactory.AgeRangeID)
                            {
                                selectProperties.AddRange(new[] { "Age_5yrGroup", "Age_10yrGroup" });
                                //age range is incorporated into the diagnosis and disease joins
                            }
                            else if (termID == Lpp.QueryComposer.ModelTermsFactory.ConditionsID)
                            {
                                selectProperties.AddRange(new[] { "Disease" });

                                IEnumerable<string> diseases = tGroup.Select(g => TranslateCondition(g.GetStringValue("Condition"))).ToArray();
                                if (diseases.Any())
                                {
                                    if (icdDiseasePredicate == null)
                                    {
                                        icdDiseasePredicate = q => diseases.Contains(q.Disease);
                                    }
                                    else
                                    {
                                        icdDiseasePredicate = icdDiseasePredicate.Or(q => diseases.Contains(q.Disease));
                                    }
                                }

                            }
                            else if (termID == Lpp.QueryComposer.ModelTermsFactory.EthnicityID)
                            {
                                selectProperties.Add("Ethnicity");

                                var values = tGroup.SelectMany(t => t.GetValue("Ethnicities").Values<int>()).Distinct().Select(t => TranslateEthnicity(t)).ToArray();

                                Expression<Func<ESPQueryResult, bool>> ethnicityExpression = null;
                                foreach (int value in values)
                                {
                                    if (ethnicityExpression == null)
                                    {
                                        ethnicityExpression = eth => eth.EthnicityCode == value;
                                    }
                                    else
                                    {
                                        ethnicityExpression = ethnicityExpression.Or(q => q.EthnicityCode == value);
                                    }
                                }

                                if (ethnicityExpression != null)
                                    entityQuery = entityQuery.Where(ethnicityExpression);
                            }
                            else if (termID == Lpp.QueryComposer.ModelTermsFactory.SexID)
                            {
                                selectProperties.Add("Sex");

                                IEnumerable<string> sexs = tGroup.SelectMany(t => TranslateSex(t.GetStringValue("Sex"))).Distinct();
                                logger.Debug(sexs);
                                if (sexs.Any())
                                {
                                    entityQuery = entityQuery.Where(q => sexs.Contains(q.Sex.ToUpper()));
                                }
                            }
                            else if (termID == Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID)
                            {
                                selectProperties.AddRange(new[] { "Code", "CodeDescription" });

                                //TODO:going to have to be terms for 4 and 5 digit as well.
                                IEnumerable<string> codes = tGroup.SelectMany(t => AdapterHelpers.ParseCodeTermValues(t)).Select(t => t.Trim()).Distinct();
                                if (codes.Any())
                                {
                                    Expression<Func<ESPQueryResult, bool>> icd9Expression = null;
                                    foreach (string code in codes)
                                    {
                                        if (icd9Expression == null)
                                        {
                                            icd9Expression = q => q.Code.StartsWith(code.ToUpper());
                                        }
                                        else
                                        {
                                            icd9Expression = icd9Expression.Or(q => q.Code.StartsWith(code.ToUpper()));
                                        }
                                    }

                                    if (icdDiseasePredicate == null)
                                    {
                                        icdDiseasePredicate = icd9Expression;
                                    }
                                    else
                                    {
                                        icdDiseasePredicate = icdDiseasePredicate.Or(icd9Expression);
                                    }
                                }
                            }
                            else if (termID == Lpp.QueryComposer.ModelTermsFactory.ObservationPeriodID)
                            {
                                if (hasConditionsTerm || hasICD9CodesTerm)
                                {
                                    selectProperties.AddRange(new[] { "ObservationPeriod" });
                                }
                                //observation period is incorporated into the diagnosis and disease joins
                            }
                            else if (termID == Lpp.QueryComposer.ModelTermsFactory.VisitsID)
                            {
                                try
                                {
                                    IQueryable<EncountersGroupingResult> encountersAppliedQuery = encounterQuery.GroupBy(v => new { v.PatID, v.AgeGroup5yr, v.AgeGroup10yr }).Select(v => new EncountersGroupingResult { PatID = v.Key.PatID, AgeGroup5yr = v.Key.AgeGroup5yr, AgeGroup10yr = v.Key.AgeGroup10yr, Count = v.Count() });

                                    int visits = tGroup.Where(t => t.GetValue("Visits") != null).Select(t => Convert.ToInt32(t.GetStringValue("Visits"))).Min();

                                    if (!hasConditionsTerm && !hasICD9CodesTerm)
                                    {
                                        entityQuery = entityQuery.Join(encountersAppliedQuery.Where(q => q.Count >= visits), o => o.PatientID, i => i.PatID, (o, i) => new ESPQueryResult
                                        {
                                            PatientID = o.PatientID,
                                            Patients = null,
                                            Sex = o.Sex,
                                            Ethnicity = o.Ethnicity,
                                            Race = o.Race,
                                            Zip = o.Zip,
                                            TobaccoUse = o.TobaccoUse,
                                            Code = o.Code,
                                            CodeDescription = o.CodeDescription,
                                            Disease = o.Disease,
                                            ObservationPeriod = o.ObservationPeriod,
                                            Age_10yrGroup = i.AgeGroup10yr,
                                            Age_5yrGroup = i.AgeGroup5yr,
                                            Age_Detect = o.Age_Detect,
                                            CenterID = o.CenterID
                                        });
                                    }
                                    else
                                    {
                                        entityQuery = entityQuery.Join(encountersAppliedQuery.Where(q => q.Count >= visits), o => o.PatientID, i => i.PatID, (o, i) => o);
                                    }
                                }
                                catch { }

                            }
                            else if (termID == Lpp.QueryComposer.ModelTermsFactory.ZipCodeID)
                            {
                                selectProperties.Add("Zip");

                                var zipCodes = tGroup.SelectMany(t => AdapterHelpers.ParseCodeTermValues(t)).Select(t => t.Trim()).Distinct();

                                if (zipCodes.Any())
                                {
                                    entityQuery = entityQuery.Where(q => zipCodes.Contains(q.Code));
                                }
                            }
                            else
                            {
                                logger.Debug("Term specified but not implemented in criteria: " + termID);
                            }
                        }

                        if (icdDiseasePredicate != null)
                        {
                            entityQuery = entityQuery.Where(icdDiseasePredicate);
                        }
                    }
                }

                entityQuery = entityQuery.GroupBy(k => new
                {
                    k.Sex,
                    k.EthnicityCode,
                    k.Ethnicity,
                    k.Race,
                    k.Zip,
                    k.TobaccoUse,
                    k.Code,
                    k.CodeDescription,
                    k.Disease,
                    k.ObservationPeriod,
                    k.Age_10yrGroup,
                    k.Age_5yrGroup,
                    k.Age_Detect,
                    k.CenterID
                })
                .Select(k => new ESPQueryResult
                {
                    PatientID = null,
                    Patients = k.Count(),
                    Sex = k.Key.Sex,
                    EthnicityCode = k.Key.EthnicityCode,
                    Ethnicity = k.Key.Ethnicity,
                    Race = k.Key.Race,
                    Zip = k.Key.Zip,
                    TobaccoUse = k.Key.TobaccoUse,
                    Code = k.Key.Code,
                    CodeDescription = k.Key.CodeDescription,
                    Disease = k.Key.Disease,
                    ObservationPeriod = k.Key.ObservationPeriod,
                    Age_10yrGroup = k.Key.Age_10yrGroup,
                    Age_5yrGroup = k.Key.Age_5yrGroup,
                    Age_Detect = k.Key.Age_Detect,
                    CenterID = k.Key.CenterID
                });



                //TODO: generate the select fields and apply to the query            
                //NOTE: dynamically selecting into a result class didn't affect the generated query, skipping and filtering when building response.
                //var select = query.Select(Utility.MapToClass<ESPQueryResult, ESPAggregatedResult>(selectProperties));

                //TODO: if all projection are stratification and options specified? wrap with additional query 


                //TODO: apply the ordering to the select projection
                /*
                 * Here's a possiblity using reflection...
                    var param = "Address";    
                    var pi = typeof(Student).GetProperty(param);    
                    var orderByAddress = items.OrderBy(x => pi.GetValue(x, null));
                 * 
                 * see that will use expression:
                 * http://www.singingeels.com/Blogs/Nullable/2008/03/26/Dynamic_LINQ_OrderBy_using_String_Names.aspx
                 * */

                var response = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, QueryStart = DateTimeOffset.UtcNow };
                List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
                if (!viewSQL)
                {
                    logger.Debug(entityQuery.ToString());

                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (selectProperties.Contains(propInfo.Name))
                            {
                                object value = propInfo.GetValue(item, null);
                                if (propInfo.Name == "ObservationPeriod")
                                {
                                    //The value is currently the number of days from Jan 1, 1960, convert to date.
                                    value = new DateTime(1960, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddDays(Convert.ToInt32(value ?? 0));
                                }
                                row.Add(propInfo.Name, value);
                            }
                        }
                        results.Add(row);
                    }
                }
                else
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("SQL", entityQuery.ToString());
                    results.Add(row);
                }

                logger.Debug("Number of results found:" + results.Count);

                //QueryComposerResponseDTO response = new QueryComposerResponseDTO
                //{
                //    ResponseDateTime = DateTime.UtcNow,
                //    Results = new[] { results }
                //};

                //if (query.ID.HasValue)
                //    response.RequestID = query.ID.Value;

                response.QueryEnd = DateTimeOffset.UtcNow;
                response.Results = new[] { results };

                return new[] { response }; 
            }
        }

        static IEnumerable<QueryComposerTermDTO> GetAllTermsWithinCriteria(QueryComposerCriteriaDTO criteria, Guid termTypeID)
        {
            return criteria.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == termTypeID)).Concat(criteria.Terms.Where(t => t.Type == termTypeID));
        }


        IQueryable<Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model.Demographic> ApplyCriteria(QueryComposerCriteriaDTO criteria)
        {
            int? startDate = null;
            int? endDate = null;
            var observationPeriod = criteria.Terms.FirstOrDefault(t => t.Type == ModelTermsFactory.ObservationPeriodID);
            if (observationPeriod != null)
            {
                DateTimeOffset date;
                if (DateTimeOffset.TryParse((observationPeriod.Values["Start"] ?? string.Empty).ToString(), out date))
                {
                    startDate = ConvertDateToNumberOfDays(date);
                }
                if (DateTimeOffset.TryParse((observationPeriod.Values["End"] ?? string.Empty).ToString(), out date))
                {
                    endDate = ConvertDateToNumberOfDays(date);
                }
            }  
 
            //determine the smallest age range, ie the max min age, and the min max age.
            int? minAge = DetermineMaxMinValue(GetAllTermsWithinCriteria(criteria, ModelTermsFactory.AgeRangeID).Where(t => t.Values["MinAge"] != null));

            int? maxAge = DetermineMinMaxValue(GetAllTermsWithinCriteria(criteria, ModelTermsFactory.AgeRangeID).Where(t => t.Values["MaxAge"] != null));

            var query = db.Demographics.AsQueryable();

            foreach (var tGroup in (criteria.Criteria.SelectMany(c => c.Terms).Concat(criteria.Terms)).GroupBy(k => k.Type))
            {
                var termID = tGroup.Key;
                if (termID == Lpp.QueryComposer.ModelTermsFactory.AgeRangeID)
                {
                    //applied to the joins as applicable
                }
                else if (termID == Lpp.QueryComposer.ModelTermsFactory.ConditionsID)
                {
                    IEnumerable<string> diseases = tGroup.Select(g => TranslateCondition(g.Values["Condition"])).ToArray();
                    if (diseases.Any())
                    {
                        var d = db.Diseases.AsQueryable();
                        if (startDate.HasValue)
                        {
                            d = d.Where(x => x.Date >= startDate.Value);
                        }
                        if (endDate.HasValue)
                        {
                            d = d.Where(x => x.Date <= endDate.Value);
                        }
                         if (minAge.HasValue)
                         {
                             d = d.Where(x => x.AgeAtDetectYear >= minAge.Value);
                         }
                         else if (maxAge.HasValue)
                         {
                             d = d.Where(x => x.AgeAtDetectYear <= maxAge.Value);
                         }

                         query = from q in query
                                 from dd in d.Where(x => x.PatID == q.PatID).DefaultIfEmpty()
                                 where diseases.Contains(dd.Condition)
                                 select q;
                    }

                }
                else if (termID == Lpp.QueryComposer.ModelTermsFactory.EthnicityID)
                {
                    Expression<Func<Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model.Demographic, bool>> ethnicityExpression = null;
                    foreach (var term in tGroup)
                    {
                        int ethnicity = TranslateEthnicity(term.Values["Ethnicity"]);
                        if (ethnicityExpression == null)
                        {
                            ethnicityExpression = ex => ex.Ethnicity == ethnicity;
                        }
                        else
                        {
                            ethnicityExpression = ethnicityExpression.Or(q => q.Ethnicity == ethnicity);
                        }

                    }

                    if (ethnicityExpression != null)
                        query = query.Where(ethnicityExpression);
                }
                else if (termID == Lpp.QueryComposer.ModelTermsFactory.SexID)
                {
                    IEnumerable<string> sexs = tGroup.SelectMany(t => TranslateSex(t.Values["Sex"])).Distinct();
                    if (sexs.Any())
                    {
                        query = query.Where(q => sexs.Contains(q.Sex.ToUpper()));
                    }
                }
                else if (termID == Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID)
                {
                    
                    IEnumerable<string> codes = tGroup.SelectMany(t => (t.Values["Codes"] ?? string.Empty).ToString().Split(',')).Select(t => t.Trim()).Distinct();
                    if (codes.Any())
                    {
                        Expression<Func<Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model.Diagnosis, bool>> icd9Expression = null;
                        foreach (string code in codes)
                        {
                            //TODO:going to have to be terms for 4 and 5 digit as well.
                            if (icd9Expression == null)
                            {
                                icd9Expression = q => q.DxCode3digit.StartsWith(code.ToUpper());
                            }
                            else
                            {
                                icd9Expression = icd9Expression.Or(q => q.DxCode3digit.StartsWith(code.ToUpper()));
                            }
                        }

                        var diag = db.Diagnosis.AsQueryable();
                        if (startDate.HasValue)
                        {
                            diag = diag.Where(x => x.A_Date >= startDate.Value);
                        }
                        if (endDate.HasValue)
                        {
                            diag = diag.Where(x => x.A_Date <= endDate.Value);
                        }
                        if (minAge.HasValue)
                        {
                            diag = diag.Where(x => x.AgeAtEncYear >= minAge.Value);
                        }
                        else if (maxAge.HasValue)
                        {
                            diag = diag.Where(x => x.AgeAtEncYear <= maxAge.Value);
                        }

                        query = diag.Where(icd9Expression).Join(query, o => o.PatID, i => i.PatID, (o, i) => i);                        
                    }
                }
                else if (termID == Lpp.QueryComposer.ModelTermsFactory.ObservationPeriodID)
                {
                    //observation period is incorporated into the diagnosis and disease joins
                }
                else if (termID == Lpp.QueryComposer.ModelTermsFactory.VisitsID)
                {
                    int visits = tGroup.Where(t => t.Values.ContainsKey("Visits") && t.Values["Visits"] != null).Select(t => Convert.ToInt32(t.Values["Visits"])).Min();

                    var encountersQuery = db.Demographics
                                            .Join(db.Encounters, o => o.PatID, i => i.PatID, (o, i) => new { o.PatID, i.EncounterID, i.AgeGroup5yr, i.AgeGroup10yr, i.A_Date, i.AgeAtEncYear })
                                            .GroupBy(v => new { v.PatID, v.AgeGroup5yr, v.AgeGroup10yr }).Select(v => new EncountersGroupingResult { PatID = v.Key.PatID, AgeGroup5yr = v.Key.AgeGroup5yr, AgeGroup10yr = v.Key.AgeGroup10yr, Count = v.Count() })
                                            .Where(v => v.Count >= visits);

                    query = query.Join(encountersQuery, o => o.PatID, i => i.PatID, (o, i) => o);
                }
                else if (termID == Lpp.QueryComposer.ModelTermsFactory.ZipCodeID)
                {
                    IEnumerable<string> zipCodes = tGroup.SelectMany(t => (t.Values["Codes"] ?? string.Empty).ToString().Split(',')).Select(t => t.Trim()).Distinct();
                    if (zipCodes.Any())
                    {
                        query = query.Where(q => zipCodes.Contains(q.Zip5));
                    }
                }


            }
            return query;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ESPModelAdapter()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (db != null)
            {
                db.Dispose();
                db = null;
            }

        }

        static int ConvertDateToNumberOfDays(DateTimeOffset date)
        {
            //dates in database are based on # of days from Jan 1, 1960.
            return Convert.ToInt32((date.Date - new DateTime(1960, 1, 1)).TotalDays);
        }

        static string TranslateCondition(object raw)
        {
            string value = string.Empty;
            Lpp.Dns.DTO.Enums.ConditionClassifications condition;
            if (Enum.TryParse<Lpp.Dns.DTO.Enums.ConditionClassifications>((raw ?? string.Empty).ToString(), out condition))
            {                
                switch (condition)
                {
                    case DTO.Enums.ConditionClassifications.Influenza:
                        value = "ili";
                        break;
                    case DTO.Enums.ConditionClassifications.Type1Diabetes:
                        value = "diabetes:type-1";
                        break;
                    case DTO.Enums.ConditionClassifications.Type2Diabetes:
                        value = "diabetes:type-2";
                        break;
                    case DTO.Enums.ConditionClassifications.GestationalDiabetes:
                        value = "diabetes:gestational";
                        break;
                    case DTO.Enums.ConditionClassifications.Prediabetes:
                        value = "diabetes:prediabetes";
                        break;
                    case DTO.Enums.ConditionClassifications.Asthma:
                        value = "asthma";
                        break;
                    default:
                        throw new Exception("Value for Condition Term is not Valid");
                }
            }
            return value;
        }

        static int TranslateEthnicity(object raw)
        {
            int value = 0;
            Lpp.Dns.DTO.Enums.Ethnicities ethnicity;
            if(Enum.TryParse<Lpp.Dns.DTO.Enums.Ethnicities>((raw ?? string.Empty).ToString(), out ethnicity)){
                switch (ethnicity)
                {
                    case DTO.Enums.Ethnicities.Native:
                        value = 1;
                        break;
                    case DTO.Enums.Ethnicities.Asian:
                        value = 2;
                        break;
                    case DTO.Enums.Ethnicities.Black:
                        value = 3;
                        break;
                    case DTO.Enums.Ethnicities.White:
                        value = 5;
                        break;
                    case DTO.Enums.Ethnicities.Hispanic:
                        value = 6;
                        break;
                    default:
                        throw new Exception("Value for Ethnicity Term is not Valid");
                }
            }
            return value;
        }

        static IEnumerable<string> TranslateSex(object raw)
        {
            Lpp.Dns.DTO.Enums.SexStratifications sexValue;
            if (Enum.TryParse<Lpp.Dns.DTO.Enums.SexStratifications>((raw ?? string.Empty).ToString(), out sexValue))
            {
                if (sexValue == DTO.Enums.SexStratifications.FemaleOnly)
                {
                    return new[] { "F" };
                }
                if (sexValue == DTO.Enums.SexStratifications.MaleOnly)
                {
                    return new[] { "M" };
                }
                if (sexValue == DTO.Enums.SexStratifications.MaleAndFemale || sexValue == DTO.Enums.SexStratifications.MaleAndFemaleAggregated)
                {
                    return new[] { "F", "M" };
                }
            }
            else
            {
                throw new Exception("Value for Sex Term is not Valid");
            }

            return Enumerable.Empty<string>();
        }

        static int? DetermineMaxMinValue(IEnumerable<QueryComposerTermDTO> terms)
        {
            int? value = null;
            foreach (var term in terms)
            {
                AgeRangeValues v = AdapterHelpers.ParseAgeRangeValues(term);
                if (v.MinAge.HasValue && (value == null || v.MinAge.Value > value.Value))
                {
                    value = v.MinAge;
                }
            }
            return value;
        }

        static int? DetermineMinMaxValue(IEnumerable<QueryComposerTermDTO> terms)
        {
            int? value = null;
            foreach (var term in terms)
            {
                AgeRangeValues v = AdapterHelpers.ParseAgeRangeValues(term);
                if (v.MaxAge.HasValue && (value == null || v.MaxAge.Value > value.Value))
                {
                    value = v.MaxAge;
                }
            }
            return value;
        }

        



    }
}
