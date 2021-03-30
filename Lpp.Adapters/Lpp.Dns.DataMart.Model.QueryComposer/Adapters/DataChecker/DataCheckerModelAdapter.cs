using log4net;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker
{
    public class DataCheckerModelAdapter : ModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string _connectionString = string.Empty;        
        DataContext db = null;

        public DataCheckerModelAdapter(RequestMetadata requestMetadata) : base(new Guid("321ADAA1-A350-4DD0-93DE-5DE658A507DF"), requestMetadata) { }

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
            return new string[] { "n" };
        }

        public override IEnumerable<QueryComposerResponseQueryResultDTO> Execute(QueryComposerQueryDTO query, bool viewSQL)
        {
            if (query.Header.QueryType.HasValue == false)
            {
                throw new Exception("The data adapter detail setting for the query template has not been set. Unable to determine the type of data characterization query.");
            }

            QueryComposerResponseQueryResultDTO results;
          
            switch (query.Header.QueryType.Value)
            {
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_AgeRange:
                    results = RunAgeQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Ethnicity:
                    results = RunEthnicityQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Race:
                    results = RunRaceQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Sex:
                    results = RunSexQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Procedure_ProcedureCodes:
                    results = RunProcedureQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Diagnosis_DiagnosisCodes:
                    results = RunDiagnosisQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Diagnosis_PDX:
                    results = RunPDXQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_NDC:
                    results = RunNDCQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_RxAmount:
                    results = RunRxAmtQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_RxSupply:
                    results = RunRxSupQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Metadata_DataCompleteness:
                    results = RunMetadataQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Vital_Height:
                    results = RunHeightQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Vital_Weight:
                    results = RunWeightQuery(query, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.Sql:
                    results = RunSqlDistributionQuery(query, viewSQL);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("QueryType", query.Header.QueryType, "The specified query type is not supported by the Data Characterization Adapter. Value:" + (query.Header.QueryType.HasValue ? query.Header.QueryType.Value.ToString() : string.Empty));
            }

            return new[] { results };
        }

        public override void PostProcess(QueryComposerResponseQueryResultDTO response)
        {
            base.PostProcess(response);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataCheckerModelAdapter()
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


        private QueryComposerResponseQueryResultDTO RunRaceQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var races = GetValuesFromTerm(query, ModelTermsFactory.DC_Race, "Races", null);
            var hasMissingCriteria = races.Contains("6");

            //Get Values Selected for DataPartner
            var dataPartners = GetDataPartnerCodes(query);

            var entityQuery = (from r in db.Races
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = hasMissingCriteria ? ((r.Value == null || r.Value.Trim() == string.Empty) ? "6" : races.Contains(r.Value) ? r.Value : "-1") : races.Contains(r.Value) ? r.Value : "-1"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value})
                    .Select(k => new { DataPartner = k.Key.DataPartner, Value = k.Key.Value, Count = k.Sum(r => r.n) });

            DateTimeOffset queryStart = DateTimeOffset.UtcNow;
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in entityQuery)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                Type itemType = item.GetType();
                foreach (var propInfo in itemType.GetProperties())
                {
                    if (propInfo.Name == "Value")
                    {
                        object value = propInfo.GetValue(item, null);
                        row.Add("Race", value);
                    }
                    else
                    {
                        object value = propInfo.GetValue(item, null);
                        row.Add(propInfo.Name, value);
                    }

                }


                if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                {
                    double value = Convert.ToDouble(row["n"]);
                    if (value > 0 && value < _lowThresholdValue)
                    {
                        //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                        if (!row.ContainsKey(LowThresholdColumnName))
                        {
                            row.Add(LowThresholdColumnName, true);
                        }
                        else
                        {
                            row[LowThresholdColumnName] = true;
                        }
                    }
                }

                queryResults.Add(row);
            }
            //Shape the Results
            var properties = GetRaceResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }

            return new QueryComposerResponseQueryResultDTO {
                ID = query.Header.ID,
                QueryStart = queryStart,
                QueryEnd = DateTimeOffset.UtcNow,
                LowCellThrehold = _lowThresholdValue,
                Properties = properties,
                Results = new[] { queryResults }
            };
        }

        private QueryComposerResponseQueryResultDTO RunEthnicityQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var ethnicityCodes = GetValuesFromTerm(query, ModelTermsFactory.DC_Ethnicity, "EthnicityValue", EthnicityDataTypeToDatabaseCode);

            var hasMissingCriteria = ethnicityCodes.Contains("MISSING");

            var dataPartners = GetDataPartnerCodes(query);

            var entityQuery = (from r in db.Hispanics
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = hasMissingCriteria ? ((r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : ethnicityCodes.Contains(r.Value) ? r.Value : "Other") : ethnicityCodes.Contains(r.Value) ? r.Value : "Other"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Value = k.Key.Value, Count = k.Sum(r => r.n) });

            //Shape the Results
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            DateTimeOffset queryStart = DateTimeOffset.UtcNow;
            foreach (var item in entityQuery)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                Type itemType = item.GetType();
                foreach (var propInfo in itemType.GetProperties())
                {
                    if(propInfo.Name == "Value")
                    {
                        object value = propInfo.GetValue(item, null);
                        row.Add("HISPANIC", value);
                    }
                    else
                    {
                        object value = propInfo.GetValue(item, null);
                        row.Add(propInfo.Name, value);
                    }
                   
                }

                if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                {
                    double value = Convert.ToDouble(row["n"]);
                    if (value > 0 && value < _lowThresholdValue)
                    {
                        //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                        if (!row.ContainsKey(LowThresholdColumnName))
                        {
                            row.Add(LowThresholdColumnName, true);
                        }
                        else
                        {
                            row[LowThresholdColumnName] = true;
                        }
                    }
                }

                queryResults.Add(row);
            }
            //Shape the Results
            var properties = GetRaceResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }

            return new QueryComposerResponseQueryResultDTO
            {
                ID = query.Header.ID,
                QueryStart = queryStart,
                QueryEnd = DateTimeOffset.UtcNow,
                LowCellThrehold = _lowThresholdValue,
                Properties = properties,
                Results = new[] { queryResults }
            };
        }

        private QueryComposerResponseQueryResultDTO RunDiagnosisQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            //Get Values Selected for Diagnosis
            var diagCodes = new List<string>();
            var diagCriteria = query.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DiagnosisCodes);
            var codes = (diagCriteria.GetStringValue("CodeValues")).Split(new[] { ',' }).Select(s => s.Trim()).Distinct().ToArray();
            var searchType = diagCriteria.GetStringValue("SearchMethodType");
            diagCodes.AddRange(codes);
            var codeType = diagCriteria.GetStringValue("CodeType");


            //Get Values Selected for DataPartner
            var dataPartners = GetDataPartnerCodes(query);

            var results = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, LowCellThrehold = _lowThresholdValue, QueryStart = DateTimeOffset.UtcNow };
            
            // 0 == Exact Match
            if (searchType == "0")
            {
                if(codeType == "")
                {
                    var entityQuery = from d in db.Diagnoses
                                 where diagCodes.Contains(d.DX) && dataPartners.Contains(d.DataPartner)
                                 select new
                                 {
                                    DataPartner = d.DataPartner,
                                    DX = d.DX,
                                    DxCodeType = d.DxCodeType,
                                    n = d.n
                                 };

                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();

                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "DxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Dx_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }
                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;
                    
                }
                else{
                        var entityQuery = from d in db.Diagnoses
                                where diagCodes.Contains(d.DX) && dataPartners.Contains(d.DataPartner) && d.DxCodeType == codeType
                                    select new
                                    {
                                        DataPartner = d.DataPartner,
                                        DX = d.DX,
                                        DxCodeType = d.DxCodeType,
                                        n = d.n
                                    };

                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "DxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Dx_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }
                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;

                }
            
            }
            // 1 == Starts With
            else if (searchType == "1")
            {
                if (codeType == "")
                {
                    var entityQuery = from d in db.Diagnoses
                                where diagCodes.Any(dc => d.DX.StartsWith(dc)) && dataPartners.Contains(d.DataPartner)
                                select new { DataPartner = d.DataPartner, DX = d.DX, DxCodeType = d.DxCodeType, n = d.n };
                    
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "DxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Dx_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }

                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;
                }
                else
                {
                    var entityQuery = from d in db.Diagnoses
                                where diagCodes.Any(dc => d.DX.StartsWith(dc)) && dataPartners.Contains(d.DataPartner) && d.DxCodeType == codeType
                                select new { DataPartner = d.DataPartner, DX = d.DX, DxCodeType = d.DxCodeType, n = d.n };
                    
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "DxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Dx_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }
                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;

                }

            }

            return results;           
        }

        private QueryComposerResponseQueryResultDTO RunProcedureQuery(QueryComposerQueryDTO query, bool viewSQL)
        {

            //Get Values Selected for Procedure
            var procedureCodes = new List<string>();
            var procedureCriteria = query.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_ProcedureCodes);
            var codes = (procedureCriteria.GetStringValue("CodeValues")).Split(new[] { ',' }).Select(s => s.Trim()).Distinct().ToArray();
            var searchType = procedureCriteria.GetStringValue("SearchMethodType");
            procedureCodes.AddRange(codes);
            var codeType = procedureCriteria.GetStringValue("CodeType");

            //Get Values Selected for DataPartner
            var dataPartners = GetDataPartnerCodes(query);

            var results = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, LowCellThrehold = _lowThresholdValue, QueryStart = DateTimeOffset.UtcNow };
            //Do Query
            // 0 == Exact Match
            if (searchType == "0")
            {

                if (codeType == "")
                {
                    var entityQuery = from d in db.Procedures
                                where procedureCodes.Contains(d.PX) && dataPartners.Contains(d.DataPartner)
                                select new 
                                { 
                                    DataPartner = d.DataPartner,
                                    PX = d.PX,
                                    Px_CodeType = d.PxCodeType,
                                    n = d.n
                                };
                    
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "PxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Px_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }
                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;
                }
                else
                {
                    var entityQuery = from d in db.Procedures
                                where procedureCodes.Contains(d.PX) && dataPartners.Contains(d.DataPartner) && d.PxCodeType == codeType
                                select new
                                {
                                    DataPartner = d.DataPartner,
                                    PX = d.PX,
                                    Px_CodeType = d.PxCodeType,
                                    n = d.n
                                }; 
                    
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "PxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Px_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }
                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;

                }

            }
            // 1 == Starts With
            else if (searchType == "1")
            {
                if (codeType == "")
                {
                    var entityQuery = from d in db.Procedures
                                where procedureCodes.Any(pc => d.PX.StartsWith(pc)) && dataPartners.Contains(d.DataPartner)
                                select new { DataPartner = d.DataPartner, PX = d.PX, PxCodeType = d.PxCodeType, n = d.n };
                    
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "PxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Px_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }
                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;
                }
                else
                {
                    var entityQuery = from d in db.Procedures
                                where procedureCodes.Any(pc => d.PX.StartsWith(pc)) && dataPartners.Contains(d.DataPartner) && d.PxCodeType == codeType
                                select new { DataPartner = d.DataPartner, PX = d.PX, PxCodeType = d.PxCodeType, n = d.n };
                    
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in entityQuery)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            if (propInfo.Name == "PxCodeType")
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add("Px_CodeType", value);
                            }
                            else
                            {
                                object value = propInfo.GetValue(item, null);
                                row.Add(propInfo.Name, value);
                            }

                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                        {
                            double value = Convert.ToDouble(row["n"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }
                        queryResults.Add(row);
                    }

                    results.QueryEnd = DateTimeOffset.UtcNow;
                    results.Results = new[] { queryResults };

                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }

                    results.Properties = properties;

                }

            }

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunNDCQuery(QueryComposerQueryDTO query, bool viewSQL)
        {

            //Get Values Selected for Diagnosis
            var ndcCriteria = query.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_NDCCodes);
            var ndcCodes = (ndcCriteria.GetStringValue("CodeValues")).Split(new[] { ',' }).Select(s => s.Trim()).Distinct().ToList();
            var searchType = ndcCriteria.GetStringValue("SearchMethodType");


            //Get Values Selected for DataPartner
            var dataPartners = GetDataPartnerCodes(query);

            var results = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, LowCellThrehold = _lowThresholdValue, QueryStart = DateTimeOffset.UtcNow };
            //Do Query
            // 0 == Exact Match
            if (searchType == "0")
            {
                var entityQuery = from d in db.NDCs
                            where ndcCodes.Contains(d.NDCs) && dataPartners.Contains(d.DataPartner)
                            select d;
                    
                List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                foreach (var item in entityQuery)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    Type itemType = item.GetType();
                    foreach (var propInfo in itemType.GetProperties())
                    {
                        if (propInfo.Name == "NDCs")
                        {
                            object value = propInfo.GetValue(item, null);
                            row.Add("NDC", value);
                        }
                        else
                        {
                            object value = propInfo.GetValue(item, null);
                            row.Add(propInfo.Name, value);
                        }

                    }

                    if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                    {
                        double value = Convert.ToDouble(row["n"]);
                        if (value > 0 && value < _lowThresholdValue)
                        {
                            //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                            if (!row.ContainsKey(LowThresholdColumnName))
                            {
                                row.Add(LowThresholdColumnName, true);
                            }
                            else
                            {
                                row[LowThresholdColumnName] = true;
                            }
                        }
                    }
                    queryResults.Add(row);
                }

                results.QueryEnd = DateTimeOffset.UtcNow;
                results.Results = new[] { queryResults };

                //Shape the Results
                var properties = GetNDCResponsePropertyDefinitions();
                if (_lowThresholdValue.HasValue)
                {
                    properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                }

                results.Properties = properties;

            }
            // 1 == Starts With
            else if (searchType == "1")
            {
                var entityQuery = from d in db.NDCs where dataPartners.Contains(d.DataPartner) select d;

                System.Linq.Expressions.Expression<Func<Model.NDC, bool>> codesPredicate = (d) => false;

                for (int i = 0; i < ndcCodes.Count; i++)
                {
                    string value = ndcCodes[i];
                    codesPredicate = codesPredicate.Or(d => d.NDCs.StartsWith(value));
                }

                entityQuery = entityQuery.Where(codesPredicate);

                List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                foreach (var item in entityQuery)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    Type itemType = item.GetType();
                    foreach (var propInfo in itemType.GetProperties())
                    {
                        if (propInfo.Name == "NDCs")
                        {
                            object value = propInfo.GetValue(item, null);
                            row.Add("NDC", value);
                        }
                        else
                        {
                            object value = propInfo.GetValue(item, null);
                            row.Add(propInfo.Name, value);
                        }
                    }

                    if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                    {
                        double value = Convert.ToDouble(row["n"]);
                        if (value > 0 && value < _lowThresholdValue)
                        {
                            //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                            if (!row.ContainsKey(LowThresholdColumnName))
                            {
                                row.Add(LowThresholdColumnName, true);
                            }
                            else
                            {
                                row[LowThresholdColumnName] = true;
                            }
                        }
                    }
                    queryResults.Add(row);
                }

                results.QueryEnd = DateTimeOffset.UtcNow;
                results.Results = new[] { queryResults };

                //Shape the Results
                var properties = GetNDCResponsePropertyDefinitions();
                if (_lowThresholdValue.HasValue)
                {
                    properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                }

                results.Properties = properties;

            }
            return results;
        }

        internal class PDXResult
        {
            public string DataPartner { get; set; }
            public string PDX { get; set; }
            public string EncType { get; set; }
            public double n { get; set; }
        }

        private QueryComposerResponseQueryResultDTO RunPDXQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var datapartners = GetDataPartnerCodes(query);
            var pdxCriteria = GetValuesFromTerm(query, ModelTermsFactory.DC_DiagnosisPDX, "PDXes", null);
            var encounterTypeCriteria = GetValuesFromTerm(query, ModelTermsFactory.DC_Encounter, "Encounters", null);

            bool hasPDXCriteria = pdxCriteria.Where(x => x != "MISSING").Any();
            bool hasEncounterCriteria = encounterTypeCriteria.Where(x => x != "MISSING" && x != "ALL").Any();
            bool hasAllEncountersCriteria = encounterTypeCriteria.Contains("ALL");
            bool hasMissingPDX = pdxCriteria.Contains("MISSING");
            bool hasMissingEncounters = encounterTypeCriteria.Contains("MISSING");

            var q1 = db.PDXs.Where(p => !string.IsNullOrEmpty(p.DataPartner)).Select(p => new PDXResult 
                { 
                    DataPartner = p.DataPartner, 
                    PDX = (hasPDXCriteria || hasMissingPDX) ? 
                            (
                                ((hasMissingPDX && string.IsNullOrEmpty(p.PDXs)) ? "MISSING" : 
                                    pdxCriteria.Where(x => x != "MISSING").Contains(p.PDXs) ? p.PDXs : "OTHER" ) 
                            ) 
                            : (string.IsNullOrEmpty(p.PDXs) ? "MISSING" : p.PDXs),
                    EncType = (hasEncounterCriteria || hasMissingEncounters) ?  
                            (
                                (hasMissingEncounters && string.IsNullOrEmpty(p.EncType)) ? "MISSING" : 
                                    encounterTypeCriteria.Where(x => x != "MISSING").Contains(p.EncType) ? p.EncType : "OTHER"
                            )
                            :(string.IsNullOrEmpty(p.EncType) ? "MISSING" : p.EncType), 
                    n = p.n 
                });
            if (datapartners.Any())
            {
                q1 = q1.Where(p => datapartners.Contains(p.DataPartner));
            }
            q1 = q1.GroupBy(p => new { p.DataPartner, p.PDX, p.EncType }).Select(p => new PDXResult { DataPartner = p.Key.DataPartner, PDX = p.Key.PDX, EncType = p.Key.EncType, n = p.Sum(k => k.n) });

            IQueryable<PDXResult> entityQuery;
            if (hasAllEncountersCriteria && hasEncounterCriteria)
            {
                var q2 = db.PDXs
                           .Where(p => !string.IsNullOrEmpty(p.DataPartner) && encounterTypeCriteria.Where(x => x != "MISSING" && x != "ALL" ).Contains(p.EncType))
                           .Select(p => new PDXResult {
                                    DataPartner = p.DataPartner,
                                    PDX = (hasPDXCriteria || hasMissingPDX) ?
                                            (
                                                ((hasMissingPDX && string.IsNullOrEmpty(p.PDXs)) ? "MISSING" :
                                                    pdxCriteria.Where(x => x != "MISSING").Contains(p.PDXs) ? p.PDXs : "OTHER")
                                            )
                                            : (string.IsNullOrEmpty(p.PDXs) ? "MISSING" : p.PDXs),
                                    EncType = "ALL",
                                    n = p.n 
                                });
                if (datapartners.Any())
                {
                    q2 = q2.Where(p => datapartners.Contains(p.DataPartner));
                }
                q2 = q2.GroupBy(p => new { p.DataPartner, p.PDX, p.EncType }).Select(p => new PDXResult { DataPartner = p.Key.DataPartner, PDX = p.Key.PDX, EncType = p.Key.EncType, n = p.Sum(k => k.n) });


                entityQuery = q1.Union(q2);
            }
            else
            {
                entityQuery = q1;
            };

            var results = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, LowCellThrehold = _lowThresholdValue, QueryStart = DateTimeOffset.UtcNow };
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in entityQuery)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                Type itemType = item.GetType();
                foreach (var propInfo in itemType.GetProperties())
                {
                    if (propInfo.Name == "PDXs")
                    {
                        object value = propInfo.GetValue(item, null);
                        row.Add("PDX", value);
                    }
                    else
                    {
                        object value = propInfo.GetValue(item, null);
                        row.Add(propInfo.Name, value);
                    }

                }


                if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                {
                    double value = Convert.ToDouble(row["n"]);
                    if (value > 0 && value < _lowThresholdValue)
                    {
                        //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                        if (!row.ContainsKey(LowThresholdColumnName))
                        {
                            row.Add(LowThresholdColumnName, true);
                        }
                        else
                        {
                            row[LowThresholdColumnName] = true;
                        }
                    }
                }
                queryResults.Add(row);
            }

            results.QueryEnd = DateTimeOffset.UtcNow;
            results.Results = new[] { queryResults };

            //Shape the Results
            var properties = GetPDXResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }

            results.Properties = properties;

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunRxAmtQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var rxAmtCodes = GetValuesFromTerm(query, ModelTermsFactory.DC_DispensingRXAmount, "RXAmounts", RxAmtDataTypeToDatabaseCode);
            var dataPartners = GetDataPartnerCodes(query);

            var squery = (from r in db.RXAmts
                            select new
                            {
                                r.DataPartner,
                                r.n,
                                RxAMT = (r.RxAmts < 0 ? "-1" : r.RxAmts >= 0 && r.RxAmts <= 1 ? "0" : r.RxAmts > 1 && r.RxAmts <= 30 ? "30" : r.RxAmts > 30 && r.RxAmts <= 60 ? "60" : r.RxAmts > 60 && r.RxAmts <= 90 ? "90" : r.RxAmts > 90 && r.RxAmts <= 120 ? "120" : r.RxAmts > 120 && r.RxAmts <= 180 ? "180" : r.RxAmts > 180 ? "181" : null)
                            }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxAMT }).Select(k => new { DataPartner = k.Key.DataPartner, RxAMT = k.Key.RxAMT, Count = k.Sum(r => r.n) });

             var entityQuery = (from r in squery
                         select new
                         {
                             r.DataPartner,
                             r.Count,
                             RxAMT = (r.RxAMT == null) ? "MISSING" : rxAmtCodes.Contains(r.RxAMT) ? r.RxAMT : "OTHER"
                         }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxAMT }).Select(k => new { DP = k.Key.DataPartner, RxAmt = k.Key.RxAMT, Total = k.Sum(r => r.Count) });

            var results = ExecuteQuery(query.Header.ID, entityQuery, GetRxAmtResponsePropertyDefinitions());

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunRxSupQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var rxSupCodes = GetValuesFromTerm(query, ModelTermsFactory.DC_DispensingRXSupply, "RXSupply", RxSupDataTypeToDatabaseCode);
            var dataPartners = GetDataPartnerCodes(query);

            var squery = (from r in db.RXSups
                          select new
                          {
                              r.DataPartner,
                              r.n,
                              RxSup = (r.RxSup < 0 ? "-1" : r.RxSup >= 0 && r.RxSup < 2 ? "0" : r.RxSup >= 2 && r.RxSup <= 30 ? "2" : r.RxSup > 30 && r.RxSup <= 60 ? "30" : r.RxSup > 60 && r.RxSup <= 90 ? "60" : r.RxSup > 90 ? "90" : null)
                          }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxSup }).Select(k => new { DataPartner = k.Key.DataPartner, RxSup = k.Key.RxSup, Count = k.Sum(r => r.n) });

            var entityQuery = (from r in squery
                         select new
                         {
                             r.DataPartner,
                             r.Count,
                             RxSup = (r.RxSup == null) ? "MISSING" : rxSupCodes.Contains(r.RxSup) ? r.RxSup : "OTHER"
                         }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxSup }).Select(k => new { DP = k.Key.DataPartner, RxSup = k.Key.RxSup, Total = k.Sum(r => r.Count) });

            var results = ExecuteQuery(query.Header.ID, entityQuery, GetRxSupResponsePropertyDefinitions());

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunMetadataQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var dataPartners = GetDataPartnerCodes(query);            

            var entityQuery = (from r in db.Metadatas
                         where dataPartners.Contains(r.DataPartner)
                         select r);

            var results = ExecuteQuery(query.Header.ID, entityQuery, GetMetadataResponsePropertyDefinitions());
            return results;
        }

        internal class AgeDistributionResult
        {
            public string DataPartner { get; set; }
            public string AgeRange { get; set; }
            public double n { get; set; }
        }

        private QueryComposerResponseQueryResultDTO RunAgeQuery(QueryComposerQueryDTO query, bool viewSQL)
        {   
            var ageCriteria = GetValuesFromTerm(query, ModelTermsFactory.DC_AgeDistribution, "AgeDistributionValue", AgeDataTypeToDatabaseCode);
            var datapartners = GetDataPartnerCodes(query);

            bool hasCritieria = ageCriteria.Any(c => c != "NULL or Missing");
            bool hasMissingCritieria = ageCriteria.Contains("NULL or Missing");

            var entityQuery = db.Ages.Where(a => !string.IsNullOrEmpty(a.DataPartner))
                               .Select(a => new AgeDistributionResult
                               {
                                   DataPartner = a.DataPartner,
                                   AgeRange = (hasMissingCritieria && string.IsNullOrEmpty(a.Value)) ? "NULL or Missing" :
                                                (hasCritieria && ageCriteria.Where(x => x != "NULL or Missing").Contains(a.Value)) ? a.Value :
                                                "Other",
                                   n = a.n
                               });

            if (datapartners.Any())
            {
                entityQuery = entityQuery.Where(a => datapartners.Contains(a.DataPartner));
            }

            entityQuery = entityQuery.GroupBy(k => new { k.DataPartner, k.AgeRange })
                         .Select(k => new AgeDistributionResult{ DataPartner = k.Key.DataPartner, AgeRange = k.Key.AgeRange, n = k.Sum(j => j.n) });


            var results = ExecuteQuery(query.Header.ID, entityQuery, GetAgeResponsePropertyDefinitions());

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunHeightQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var heightCodes = GetValuesFromTerm(query, ModelTermsFactory.DC_HeightDistribution, "HeightDistributions", HeightDataTypeToDatabaseCode);
            var dataPartners = GetDataPartnerCodes(query);
            
            var entityQuery = (from r in db.Heights
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = (r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : heightCodes.Contains(r.Value) ? r.Value : "OTHER"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Height = k.Key.Value, Count = k.Sum(r => r.n) });

            var results = ExecuteQuery(query.Header.ID, entityQuery, GetHeightResponsePropertyDefinitions());

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunSexQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var sexCodes = GetValuesFromTerm(query, ModelTermsFactory.DC_SexDistribution, "SexDistributions", SexDataTypeToDatabaseCode);
            var dataPartners = GetDataPartnerCodes(query);
            
            var entityQuery = (from r in db.Sexes
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = (r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : sexCodes.Contains(r.Value) ? r.Value : "OTHER"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Sex = k.Key.Value, Count = k.Sum(r => r.n) });

            var results = ExecuteQuery(query.Header.ID, entityQuery, GetSexResponsePropertyDefinitions());

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunWeightQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            var weightCodes = GetValuesFromTerm(query, ModelTermsFactory.DC_WeightDistribution, "WeightDistributions", WeightDataTypeToDatabaseCode);
            var dataPartners = GetDataPartnerCodes(query);
            
            var entityQuery = (from r in db.Weights
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = (r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : weightCodes.Contains(r.Value) ? r.Value : "OTHER"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Weight = k.Key.Value, Count = k.Sum(r => r.n) });

            var results = ExecuteQuery(query.Header.ID, entityQuery, GetWeightResponsePropertyDefinitions());

            return results;
        }

        private QueryComposerResponseQueryResultDTO RunSqlDistributionQuery(QueryComposerQueryDTO query, bool viewSQL)
        {
            if (query.Where.Criteria.Where(c => c.Terms.Any(d => d.Type != ModelTermsFactory.SqlDistributionID)).Count() > 0)
            {
                throw new NotSupportedException("Another Term is Included with Sql Distribution and this is not Supported");
            }
            //Get Values Selected for Sql Distribution
            var sqlCriteria = query.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.SqlDistributionID);
            string sql = sqlCriteria.GetStringValue("Sql");

            List<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> columnProperties = new List<QueryComposerResponsePropertyDefinitionDTO>();
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            var results = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, LowCellThrehold = _lowThresholdValue, QueryStart = DateTimeOffset.UtcNow };

            using(var conn = Utilities.OpenConnection(_settings, logger, true))
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

            return results;

        }

        static IEnumerable<string> GetDataPartnerCodes(QueryComposerQueryDTO query)
        {
            return GetValuesFromTerm(query, ModelTermsFactory.DC_DataPartners, "DataPartnersValue", null);
        }

        static IEnumerable<string> GetValuesFromTerm(QueryComposerQueryDTO query, Guid termTypeID, string property, Func<string, string> translateCode)
        {
            var terms = query.Where.Criteria.First().Terms.Where(t => t.Type == termTypeID);
            var codes = terms.SelectMany(t => t.GetStringCollection(property)).Select(s => s.Trim()).Distinct().ToArray();

            if (translateCode == null)
                return codes;

            var codeList = new List<string>(codes.Length * 2);
            foreach (var code in codes)
                codeList.Add(translateCode(code));

            return codeList;
        }

        QueryComposerResponseQueryResultDTO ExecuteQuery(Guid queryID, IQueryable entityQuery, IEnumerable<QueryComposerResponsePropertyDefinitionDTO> properties)
        {
            var results = new QueryComposerResponseQueryResultDTO { ID = queryID, LowCellThrehold = _lowThresholdValue, QueryStart = DateTimeOffset.UtcNow };

            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in entityQuery)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                Type itemType = item.GetType();
                foreach (var propInfo in itemType.GetProperties())
                {
                    object value = propInfo.GetValue(item, null);
                    row.Add(propInfo.Name, value);
                }

                if (_lowThresholdValue.HasValue && row.ContainsKey("n"))
                {
                    double value = Convert.ToDouble(row["n"]);
                    if (value > 0 && value < _lowThresholdValue)
                    {
                        //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                        if (!row.ContainsKey(LowThresholdColumnName))
                        {
                            row.Add(LowThresholdColumnName, true);
                        }
                        else
                        {
                            row[LowThresholdColumnName] = true;
                        }
                    }
                }
                queryResults.Add(row);
            }

            results.QueryEnd = DateTimeOffset.UtcNow;
            results.Results = new[] { queryResults };

            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }

            results.Properties = properties;

            return results;
        }

        private string EthnicityDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
            {
                case "3":
                    code = "MISSING";
                    break;
                case "0":
                    code = "U";
                    break;
                case "1":
                    code = "Y";
                    break;
                case "2":
                    code = "N";
                    break;
                default:
                case "Other":
                    code = "Other";
                    break;
            }
            return code;
        }

        private string EncounterDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
            {
                case "0":
                    code = "All";
                    break;
                case "1":
                    code = "AV";
                    break;
                case "2":
                    code = "ED";
                    break;
                case "3":
                    code = "IP";
                    break;
                case "4":
                    code = "IS";
                    break;
                case "5":
                    code = "OA";
                    break;
                case "6":
                    code = "MISSING";
                    break;
                default:
                    code = "U";
                    break;
            }
            return code;
        }

        private string PDXDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
            {
                case "0":
                    code = "P";
                    break;
                case "1":
                    code = "S";
                    break;
                case "2":
                    code = "X";
                    break;
                case "3":
                    code = "MISSING";
                    break;
                default:
                    code = "U";
                    break;
            }
            return code;
        }

        private string RxAmtDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
            {
                case "0":
                    code = "-1";
                    break;
                case "1":
                    code = "0";
                    break;
                case "2":
                    code = "30";
                    break;
                case "3":
                    code = "60";
                    break;
                case "4":
                    code = "90";
                    break;
                case "5":
                    code = "120";
                    break;
                case "6":
                    code = "180";
                    break;
                case "7":
                    code = "181";
                    break;
                case "8":
                    code = "OTHER";
                    break;
                case "9":
                    code = "MISSING";
                    break;
                default:
                    code = "MISSING";
                    break;
            }
            return code;
        }

        private string RxSupDataTypeToDatabaseCode(string Type)//
        {
            string code;
            switch (Type)
            {
                case "0":
                    code = "-1";
                    break;
                case "1":
                    code = "0";
                    break;
                case "2":
                    code = "2";
                    break;
                case "3":
                    code = "30";
                    break;
                case "4":
                    code = "60";
                    break;
                case "5":
                    code = "90";
                    break;
                case "6":
                    code = "OTHER";
                    break;
                case "7":
                    code = "MISSING";
                    break;
                default:
                    code = "MISSING";
                    break;
            }
            return code;
        }

        private string AgeDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
            {
                case "0":
                    code = "<0 yrs";
                    break;
                case "1":
                    code = "0-1 yrs";
                    break;
                case "2":
                    code = "2-4 yrs";
                    break;
                case "3":
                    code = "5-9 yrs";
                    break;
                case "4":
                    code = "10-14 yrs";
                    break;
                case "5":
                    code = "15-18 yrs";
                    break;
                case "6":
                    code = "19-21 yrs";
                    break;

                case "7":
                    code = "22-44 yrs";
                    break;
                case "8":
                    code = "45-64 yrs";
                    break;
                case "9":
                    code = "65-74 yrs";
                    break;
                case "10":
                    code = "75-110 yrs";
                    break;
                case "11":
                    code = ">110 yrs";
                    break;
                case "12":
                    code = "NULL or Missing";
                    break;
                case "13":
                    code = "Value outside of CDM specifications";
                    break;
                default:
                case "Other":
                    code = "Other";
                    break;
            }
            return code;
        }

        private string HeightDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
            {
                case "0":
                    code = "<0";
                    break;
                case "1":
                    code = "11-20";
                    break;
                case "2":
                    code = "21-45";
                    break;
                case "3":
                    code = "46-52";
                    break;
                case "4":
                    code = "53-58";
                    break;
                case "5":
                    code = "59-64";
                    break;
                case "6":
                    code = "65-70";
                    break;

                case "7":
                    code = "71-76";
                    break;
                case "8":
                    code = "77-82";
                    break;
                case "9":
                    code = "83-88";
                    break;
                case "10":
                    code = "89-94";
                    break;
                case "11":
                    code = "95+";
                    break;
                case "12":
                    code = "NULL or Missing";
                    break;
                default:
                case "OTHER":
                    code = "OTHER";
                    break;
            }
            return code;
        }

        private string SexDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
            {
                case "0":
                    code = "A";
                    break;
                case "1":
                    code = "F";
                    break;
                case "2":
                    code = "M";
                    break;
                case "3":
                    code = "NI";
                    break;
                case "4":
                    code = "NULL or Missing";
                    break;
                case "5":
                    code = "OT";
                    break;
                case "6":
                    code = "UN";
                    break;
                case "7":
                    code = "Values outside of CDM specifications";
                    break;
                default:
                case "OTHER":
                    code = "OTHER";
                    break;
            }
            return code;
        }

        string WeightDataTypeToDatabaseCode(string weightDataType)
        {
            string code;
            switch (weightDataType)
            {
                case "0":
                    code = "<0";
                    break;
                case "1":
                    code = "0-1";
                    break;
                case "2":
                    code = "2-6";
                    break;
                case "3":
                    code = "7-12";
                    break;
                case "4":
                    code = "13-20";
                    break;
                case "5":
                    code = "21-35";
                    break;
                case "6":
                    code = "36-50";
                    break;
                case "7":
                    code = "51-75";
                    break;
                case "8":
                    code = "76-100";
                    break;
                case "9":
                    code = "101-125";
                    break;
                case "10":
                    code = "126-150";
                    break;
                case "11":
                    code = "151-175";
                    break;
                case "12":
                    code = "176-200";
                    break;
                case "13":
                    code = "201-225";
                    break;
                case "14":
                    code = "226-250";
                    break;
                case "15":
                    code = "251-275";
                    break;
                case "16":
                    code = "276-300";
                    break;
                case "18":
                    code = "350+";
                    break;
                case "19":
                    code = "NULL or Missing";
                    break;
                default:
                case "OTHER":
                    code = "OTHER";
                    break;
            }
            return code;
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetRaceResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Race", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Total", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetEthnicityResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "HISPANIC", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Total", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetDiagnosisResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Dx_CodeType", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DX", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "n", Type = "System.Double" },
            };
        }
        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetProcedureResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Px_CodeType", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DX", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "n", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetNDCResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "NDC", Type = "System.String" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetPDXResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "PDX", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "EncType", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "n", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetRxAmtResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "RxAmt", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Total", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetRxSupResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "RxSup", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Total", Type = "System.Double" },
            };
        }
        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetMetadataResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DP", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "ETL", Type = "System.Double" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DIA_MIN", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DIA_MAX", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DIS_MIN", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DIS_MAX", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "ENC_MIN", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "ENC_MAX", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "ENR_MIN", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "ENR_MAX", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "PRO_MIN", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "PRO_MAX", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "MSDD_MIN", Type = "System.DateTime" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "MSDD_MAX", Type = "System.DateTime" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetAgeResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Age", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Count", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetHeightResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Height", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Count", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetSexResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Sex", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Count", Type = "System.Double" },
            };
        }

        private IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetWeightResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataPartner", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Weight", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Count", Type = "System.Double" },
            };
        }
    }
}
