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

        public DataCheckerModelAdapter() : base(new Guid("321ADAA1-A350-4DD0-93DE-5DE658A507DF")) { }

        public override void Initialize(IDictionary<string, object> settings)
        {
            base.Initialize(settings);

            _connectionString = Utilities.BuildConnectionString(settings, logger);
            db = DataContext.Create(_connectionString);

            db.Database.Log = (sql) => {
                if(!string.IsNullOrWhiteSpace(sql))
                    logger.Debug(sql);
            };            
        }

        protected override string[] LowThresholdColumns(DTO.QueryComposer.QueryComposerResponseDTO response)
        {
            return new string[] { "n" };
        }

        public override QueryComposerResponseDTO Execute(QueryComposerRequestDTO request, bool viewSQL)
        {
            if (request.Header.QueryType.HasValue == false)
            {
                throw new Exception("The data adapter detail setting for the query template has not been set. Unable to determine the type of data characterization query.");
            }

            var results = new QueryComposerResponseDTO();
          
            switch (request.Header.QueryType.Value)
            {
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_AgeRange:
                    results = RunAgeQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Ethnicity:
                    results = RunEthnicityQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Race:
                    results = RunRaceQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Demographic_Sex:
                    results = RunSexQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Procedure_ProcedureCodes:
                    results = RunProcedureQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Diagnosis_DiagnosisCodes:
                    results = RunDiagnosisQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Diagnosis_PDX:
                    results = RunPDXQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_NDC:
                    results = RunNDCQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_RxAmount:
                    results = RunRxAmtQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Dispensing_RxSupply:
                    results = RunRxSupQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Metadata_DataCompleteness:
                    results = RunMetadataQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Vital_Height:
                    results = RunHeightQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.DataCharacterization_Vital_Weight:
                    results = RunWeightQuery(request, viewSQL);
                    break;
                case DTO.Enums.QueryComposerQueryTypes.Sql:
                    results = RunSqlDistributionQuery(request, viewSQL);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("QueryType", request.Header.QueryType, "The specified query type is not supported by the Data Characterization Adapter. Value:" + (request.Header.QueryType.HasValue ? request.Header.QueryType.Value.ToString() : string.Empty));
            }
            return results;
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


        private QueryComposerResponseDTO RunRaceQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            //Get Values Selected for Race
            var races = new List<string>();
            var raceCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_Race);
            //GetStringCollection
            //var codes = (raceCriteria.GetStringValue("Races") ?? "").Split(new[] { ',' }).Select(s => s.Trim()).Distinct().ToArray();
            var codes = (raceCriteria.GetStringCollection("Races")).Select(s => s.Trim()).Distinct().ToArray();
            var hasMissingCriteria = codes.Contains("6");
            races.AddRange(codes);
            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            //Do Query
            var query = (from r in db.Races
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = hasMissingCriteria ? ((r.Value == null || r.Value.Trim() == string.Empty) ? "6" : races.Contains(r.Value) ? r.Value : "-1") : races.Contains(r.Value) ? r.Value : "-1"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value})
                    .Select(k => new { DataPartner = k.Key.DataPartner, Value = k.Key.Value, Count = k.Sum(r => r.n) });


            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults},
                Properties = GetRaceResponsePropertyDefinitions()
            };
            return results;

        }

        private QueryComposerResponseDTO RunEthnicityQuery(QueryComposerRequestDTO request, bool viewSQL)
        {

            //Get Values Selected for Ethnicity
            var ethnicities = new List<string>();
            var ethnicityCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_Ethnicity);
            var codes = (ethnicityCriteria.GetStringCollection("EthnicityValue")).Select(s => s.Trim()).Distinct().ToArray();
            ethnicities.AddRange(codes);
            var ethnicityCodes = new List<string>();
            foreach (var code in codes)
                ethnicityCodes.Add(EthnicityDataTypeToDatabaseCode(code));
            var hasMissingCriteria = ethnicityCodes.Contains("MISSING");
            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            
            //Do Query
            //var query = from d in db.Hispanics
            //            where ethnicityCodes.Contains(d.Value) && dataPartners.Contains(d.DataPartner)
            //            select d;
            var query = (from r in db.Hispanics
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = hasMissingCriteria ? ((r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : ethnicityCodes.Contains(r.Value) ? r.Value : "Other") : ethnicityCodes.Contains(r.Value) ? r.Value : "Other"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Value = k.Key.Value, Count = k.Sum(r => r.n) });
            //Shape the Results
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;

        }

        private QueryComposerResponseDTO RunDiagnosisQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            //Get Values Selected for Diagnosis
            var diagCodes = new List<string>();
            var diagCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DiagnosisCodes);
            var codes = (diagCriteria.GetStringValue("CodeValues")).Split(new[] { ',' }).Select(s => s.Trim()).Distinct().ToArray();
            var searchType = diagCriteria.GetStringValue("SearchMethodType");
            diagCodes.AddRange(codes);
            var codeType = diagCriteria.GetStringValue("CodeType");


            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            var results = new QueryComposerResponseDTO();
            //Do Query
            // 0 == Exact Match
            if (searchType == "0")
            {
                if(codeType == "")
                {
                    var query = from d in db.Diagnoses
                                 where diagCodes.Contains(d.DX) && dataPartners.Contains(d.DataPartner)
                                 select new
                                 {
                                    DataPartner = d.DataPartner,
                                    DX = d.DX,
                                    DxCodeType = d.DxCodeType,
                                    n = d.n
                                 };
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };
                }
                else{
                        var query = from d in db.Diagnoses
                                where diagCodes.Contains(d.DX) && dataPartners.Contains(d.DataPartner) && d.DxCodeType == codeType
                                    select new
                                    {
                                        DataPartner = d.DataPartner,
                                        DX = d.DX,
                                        DxCodeType = d.DxCodeType,
                                        n = d.n
                                    };
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };

                }
            
            }
            // 1 == Starts With
            else if (searchType == "1")
            {
                if (codeType == "")
                {
                    var query = from d in db.Diagnoses
                                where diagCodes.Any(dc => d.DX.StartsWith(dc)) && dataPartners.Contains(d.DataPartner)
                                select new { DataPartner = d.DataPartner, DX = d.DX, DxCodeType = d.DxCodeType, n = d.n };
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };
                }
                else
                {
                    var query = from d in db.Diagnoses
                                where diagCodes.Any(dc => d.DX.StartsWith(dc)) && dataPartners.Contains(d.DataPartner) && d.DxCodeType == codeType
                                select new { DataPartner = d.DataPartner, DX = d.DX, DxCodeType = d.DxCodeType, n = d.n };
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetDiagnosisResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };

                }

            }

            return results;

           
        }

        private QueryComposerResponseDTO RunProcedureQuery(QueryComposerRequestDTO request, bool viewSQL)
        {

            //Get Values Selected for Procedure
            var procedureCodes = new List<string>();
            var procedureCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_ProcedureCodes);
            var codes = (procedureCriteria.GetStringValue("CodeValues")).Split(new[] { ',' }).Select(s => s.Trim()).Distinct().ToArray();
            var searchType = procedureCriteria.GetStringValue("SearchMethodType");
            procedureCodes.AddRange(codes);
            var codeType = procedureCriteria.GetStringValue("CodeType");

            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            var results = new QueryComposerResponseDTO();
            //Do Query
            // 0 == Exact Match
            if (searchType == "0")
            {

                if (codeType == "")
                {
                    var query = from d in db.Procedures
                                where procedureCodes.Contains(d.PX) && dataPartners.Contains(d.DataPartner)
                                select new 
                                { 
                                    DataPartner = d.DataPartner,
                                    PX = d.PX,
                                    Px_CodeType = d.PxCodeType,
                                    n = d.n
                                };
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };
                }
                else
                {
                    var query = from d in db.Procedures
                                where procedureCodes.Contains(d.PX) && dataPartners.Contains(d.DataPartner) && d.PxCodeType == codeType
                                select new
                                {
                                    DataPartner = d.DataPartner,
                                    PX = d.PX,
                                    Px_CodeType = d.PxCodeType,
                                    n = d.n
                                }; 
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };

                }

            }
            // 1 == Starts With
            else if (searchType == "1")
            {
                if (codeType == "")
                {
                    var query = from d in db.Procedures
                                where procedureCodes.Any(pc => d.PX.StartsWith(pc)) && dataPartners.Contains(d.DataPartner)
                                select new { DataPartner = d.DataPartner, PX = d.PX, PxCodeType = d.PxCodeType, n = d.n };
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };
                }
                else
                {
                    var query = from d in db.Procedures
                                where procedureCodes.Any(pc => d.PX.StartsWith(pc)) && dataPartners.Contains(d.DataPartner) && d.PxCodeType == codeType
                                select new { DataPartner = d.DataPartner, PX = d.PX, PxCodeType = d.PxCodeType, n = d.n };
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetProcedureResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };

                }

            }

            //var results = new QueryComposerResponseDTO();
            return results;
        }

        private QueryComposerResponseDTO RunNDCQuery(QueryComposerRequestDTO request, bool viewSQL)
        {

            //Get Values Selected for Diagnosis
            var ndcCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_NDCCodes);
            var ndcCodes = (ndcCriteria.GetStringValue("CodeValues")).Split(new[] { ',' }).Select(s => s.Trim()).Distinct().ToList();
            var searchType = ndcCriteria.GetStringValue("SearchMethodType");
            

            //Get Values Selected for DataPartner
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dataPartners = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToList();

            var results = new QueryComposerResponseDTO();
            //Do Query
            // 0 == Exact Match
            if (searchType == "0")
            {
                    var query = from d in db.NDCs
                                where ndcCodes.Contains(d.NDCs) && dataPartners.Contains(d.DataPartner)
                                select d;
                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetNDCResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };

            }
            // 1 == Starts With
            else if (searchType == "1")
            {
                    //var query = from d in db.NDCs
                    //            where ndcCodes.Any(code => d.NDCs.StartsWith(code)) && dataPartners.Contains(d.DataPartner)
                    //            select d;

                    var query = from d in db.NDCs where dataPartners.Contains(d.DataPartner) select d;

                    System.Linq.Expressions.Expression<Func<Model.NDC, bool>> codesPredicate = (d) => false;

                    for (int i = 0; i < ndcCodes.Count; i++)
                    {
                        string value = ndcCodes[i];
                        codesPredicate = codesPredicate.Or(d => d.NDCs.StartsWith(value));
                    }

                    query = query.Where(codesPredicate);

                    List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
                    List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                    foreach (var item in query)
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
                    //Shape the Results
                    var properties = GetNDCResponsePropertyDefinitions();
                    if (_lowThresholdValue.HasValue)
                    {
                        properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
                    }
                    results = new QueryComposerResponseDTO
                    {
                        Errors = errors,
                        RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                        ResponseDateTime = DateTime.UtcNow,
                        Results = new[] { queryResults },
                        Properties = properties
                    };
        

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

        private QueryComposerResponseDTO RunPDXQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            var terms = request.Where.Criteria.First().Terms;

            var datapartners = terms.Where(t => t.Type == ModelTermsFactory.DC_DataPartners).SelectMany(t => t.GetStringCollection("DataPartnersValue").Select(s => s.Trim()).Distinct()).ToArray();
            var pdxCriteria = terms.Where(t => t.Type == ModelTermsFactory.DC_DiagnosisPDX).SelectMany(t => t.GetStringCollection("PDXes").Select(s => s.Trim()).Distinct()).ToArray();
            var encounterTypeCriteria = terms.Where(t => t.Type == ModelTermsFactory.DC_Encounter).SelectMany(t => t.GetStringCollection("Encounters").Select(s => s.Trim()).Distinct()).ToArray();

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

            IQueryable<PDXResult> query;
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


                query = q1.Union(q2);
            }
            else
            {
                query = q1;
            };
            
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            //Shape the Results
            var properties = GetPDXResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),                
                Properties = properties
            };
            return results;
        }

        private QueryComposerResponseDTO RunRxAmtQuery(QueryComposerRequestDTO request, bool viewSQL)
        {

            //Get Values Selected for RxAmount
            var rxAmtCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DispensingRXAmount);
            var codes = (rxAmtCriteria.GetStringCollection("RXAmounts")).Select(s => s.Trim()).Distinct().ToArray();
            var rxAmtCodes = new List<string>();
            foreach (var code in codes)
                rxAmtCodes.Add(RxAmtDataTypeToDatabaseCode(code));


            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();


            var squery = (from r in db.RXAmts
                            select new
                            {
                                r.DataPartner,
                                r.n,
                                RxAMT = (r.RxAmts < 0 ? "-1" : r.RxAmts >= 0 && r.RxAmts <= 1 ? "0" : r.RxAmts > 1 && r.RxAmts <= 30 ? "30" : r.RxAmts > 30 && r.RxAmts <= 60 ? "60" : r.RxAmts > 60 && r.RxAmts <= 90 ? "90" : r.RxAmts > 90 && r.RxAmts <= 120 ? "120" : r.RxAmts > 120 && r.RxAmts <= 180 ? "180" : r.RxAmts > 180 ? "181" : null)
                            }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxAMT }).Select(k => new { DataPartner = k.Key.DataPartner, RxAMT = k.Key.RxAMT, Count = k.Sum(r => r.n) });

             var query = (from r in squery
                         select new
                         {
                             r.DataPartner,
                             r.Count,
                             RxAMT = (r.RxAMT == null) ? "MISSING" : rxAmtCodes.Contains(r.RxAMT) ? r.RxAMT : "OTHER"
                         }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxAMT }).Select(k => new { DP = k.Key.DataPartner, RxAmt = k.Key.RxAMT, Total = k.Sum(r => r.Count) });



            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            //Shape the Results
            var properties = GetRxAmtResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;
        }

        private QueryComposerResponseDTO RunRxSupQuery(QueryComposerRequestDTO request, bool viewSQL)
        {

            //Get Values Selected for RxSupply
            var rxSupCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DispensingRXSupply);
            var codes = (rxSupCriteria.GetStringCollection("RXSupply")).Select(s => s.Trim()).Distinct().ToArray();
            var rxSupCodes = new List<string>();
            foreach (var code in codes)
                rxSupCodes.Add(RxSupDataTypeToDatabaseCode(code));


            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();


            var squery = (from r in db.RXSups
                          select new
                          {
                              r.DataPartner,
                              r.n,
                              RxSup = (r.RxSup < 0 ? "-1" : r.RxSup >= 0 && r.RxSup < 2 ? "0" : r.RxSup >= 2 && r.RxSup <= 30 ? "2" : r.RxSup > 30 && r.RxSup <= 60 ? "30" : r.RxSup > 60 && r.RxSup <= 90 ? "60" : r.RxSup > 90 ? "90" : null)
                          }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxSup }).Select(k => new { DataPartner = k.Key.DataPartner, RxSup = k.Key.RxSup, Count = k.Sum(r => r.n) });

            var query = (from r in squery
                         select new
                         {
                             r.DataPartner,
                             r.Count,
                             RxSup = (r.RxSup == null) ? "MISSING" : rxSupCodes.Contains(r.RxSup) ? r.RxSup : "OTHER"
                         }).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.RxSup }).Select(k => new { DP = k.Key.DataPartner, RxSup = k.Key.RxSup, Total = k.Sum(r => r.Count) });



            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            //Shape the Results
            var properties = GetRxSupResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;
        }

        private QueryComposerResponseDTO RunMetadataQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            //Do Query
            var query = (from r in db.Metadatas
                         where dataPartners.Contains(r.DataPartner)
                         select r);


            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            //Shape the Results
            var properties = GetMetadataResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;
        }

        internal class AgeDistributionResult
        {
            public string DataPartner { get; set; }
            public string AgeRange { get; set; }
            public double n { get; set; }
        }

        private QueryComposerResponseDTO RunAgeQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            var terms = request.Where.Criteria.First().Terms;

            var datapartners = terms.Where(t => t.Type == ModelTermsFactory.DC_DataPartners).SelectMany(t => t.GetStringCollection("DataPartnersValue").Select(s => s.Trim()).Distinct()).ToArray();
            var ageCriteria = terms.Where(t => t.Type == ModelTermsFactory.DC_AgeDistribution).SelectMany(t => t.GetStringCollection("AgeDistributionValue").Select(s => s.Trim()).Distinct()).Select(s => AgeDataTypeToDatabaseCode(s)).ToArray();

            bool hasCritieria = ageCriteria.Any(c => c != "NULL or Missing");
            bool hasMissingCritieria = ageCriteria.Contains("NULL or Missing");

            var query = db.Ages.Where(a => !string.IsNullOrEmpty(a.DataPartner))
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
                query = query.Where(a => datapartners.Contains(a.DataPartner));
            }

            query = query.GroupBy(k => new { k.DataPartner, k.AgeRange })
                         .Select(k => new AgeDistributionResult{ DataPartner = k.Key.DataPartner, AgeRange = k.Key.AgeRange, n = k.Sum(j => j.n) });


            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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


            //Shape the Results
            var properties = GetAgeResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue) {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;

        }

        private QueryComposerResponseDTO RunHeightQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            //Get Values Selected for Race
            var heightCodes = new List<string>();
            var heightCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_HeightDistribution);
            var codes = (heightCriteria.GetStringCollection("HeightDistributions")).Select(s => s.Trim()).Distinct().ToArray();
            foreach (var code in codes)
                heightCodes.Add(HeightDataTypeToDatabaseCode(code));
            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            //Do Query
            var query = (from r in db.Heights
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = (r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : heightCodes.Contains(r.Value) ? r.Value : "OTHER"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Height = k.Key.Value, Count = k.Sum(r => r.n) });


            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            //Shape the Results
            var properties = GetHeightResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;

        }

        private QueryComposerResponseDTO RunSexQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            //Get Values Selected for Race
            var sexCodes = new List<string>();
            var sexCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_SexDistribution);
            var codes = (sexCriteria.GetStringCollection("SexDistributions")).Select(s => s.Trim()).Distinct().ToArray();
            foreach (var code in codes)
                sexCodes.Add(SexDataTypeToDatabaseCode(code));
            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            //Do Query
            var query = (from r in db.Sexes
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = (r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : sexCodes.Contains(r.Value) ? r.Value : "OTHER"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Sex = k.Key.Value, Count = k.Sum(r => r.n) });


            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            //Shape the Results
            var properties = GetSexResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;

        }

        private QueryComposerResponseDTO RunWeightQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            //Get Values Selected for Race
            var weightCodes = new List<string>();
            var weightCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_WeightDistribution);
            var codes = (weightCriteria.GetStringCollection("WeightDistributions")).Select(s => s.Trim()).Distinct().ToArray();
            foreach (var code in codes)
                weightCodes.Add(WeightDataTypeToDatabaseCode(code));
            //Get Values Selected for DataPartner
            var dataPartners = new List<string>();
            var dpCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DataPartners);
            var dp = (dpCriteria.GetStringCollection("DataPartnersValue")).Select(s => s.Trim()).Distinct().ToArray();
            dataPartners.AddRange(dp);
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            //Do Query
            var query = (from r in db.Weights
                         select new
                         {
                             r.DataPartner,
                             r.n,
                             Value = (r.Value == null || r.Value.Trim() == string.Empty) ? "MISSING" : weightCodes.Contains(r.Value) ? r.Value : "OTHER"
                         }
                    ).Where(r => dataPartners.Contains(r.DataPartner)).GroupBy(k => new { k.DataPartner, k.Value })
                    .Select(k => new { DataPartner = k.Key.DataPartner, Weight = k.Key.Value, Count = k.Sum(r => r.n) });


            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
            foreach (var item in query)
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
            //Shape the Results
            var properties = GetWeightResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                properties = properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = properties
            };
            return results;

        }

        private QueryComposerResponseDTO RunSqlDistributionQuery(QueryComposerRequestDTO request, bool viewSQL)
        {
            if (request.Where.Criteria.Where(c => c.Terms.Any(d => d.Type != ModelTermsFactory.SqlDistributionID)).Count() > 0)
            {
                throw new NotSupportedException("Another Term is Included with Sql Distribution and this is not Supported");
            }
            //Get Values Selected for Sql Distribution
            var sqlCriteria = request.Where.Criteria.First().Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.SqlDistributionID);
            string sql = (sqlCriteria.GetStringValue("Sql"));
            List<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> columnProperties = new List<QueryComposerResponsePropertyDefinitionDTO>();
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();

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
           
            var results = new QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { queryResults },
                Properties = columnProperties
            };
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

        private string WeightDataTypeToDatabaseCode(string Type)
        {
            string code;
            switch (Type)
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
