using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Lpp.Dns.DataMart.Model.Settings;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class MetadataRefreshModelAdapter : ModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MetadataRefreshModelAdapter(RequestMetadata requestMetadata) : base(QueryComposerModelMetadata.SummaryTableModelID, requestMetadata)
        {

        }

        protected DTO.QueryComposer.QueryComposerResponseDTO _currentViewableResponse = null;
        protected DTO.QueryComposer.QueryComposerResponseDTO _currentNonViewableResponse = null;

        public override QueryComposerResponseDTO Execute(QueryComposerRequestDTO request, bool viewSQL)
        {
            List<Dictionary<string, object>> formattedResults = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> unFormattedResults = new List<Dictionary<string, object>>();
            List<MetadataRefreshResult> results = new List<MetadataRefreshResult>();
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<QueryComposerResponseErrorDTO>();           

            System.Data.IDbConnection conn = null;
            try
            {
                conn = Utilities.OpenConnection(_settings, logger);
                List<string> tables = new List<string>();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    cmd.CommandTimeout = Convert.ToInt32(_settings.GetSetting("CommandTimeout", 120));

                    logger.Debug("Executing query:" + Environment.NewLine + "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }

                var sqlSB = new StringBuilder();

                if (tables.Contains("drug_class", StringComparer.OrdinalIgnoreCase))
                {
                    sqlSB.Append("SELECT DISTINCT(Period), 'Drug Class' as [Query Type] FROM DRUG_CLASS");
                }
                if(tables.Contains("generic_name", StringComparer.OrdinalIgnoreCase))
                {
                    if(sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'Generic Name' as [Query Type] FROM GENERIC_NAME");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'Generic Name' as [Query Type] FROM GENERIC_NAME");
                    }
                }
                if (tables.Contains("hcpcs", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'HCPCS' as [Query Type] FROM hcpcs");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'HCPCS' as [Query Type] FROM hcpcs");
                    }
                }
                if (tables.Contains("enrollment", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Year), 'Enrollment' as [Query Type] FROM ENROLLMENT");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Year), 'Enrollment' as [Query Type] FROM ENROLLMENT");
                    }
                }
                if (tables.Contains("icd9_diagnosis", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'ICD9 Diagnosis' as [Query Type] FROM icd9_diagnosis");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'ICD9 Diagnosis' as [Query Type] FROM icd9_diagnosis");
                    }
                }
                if (tables.Contains("icd9_diagnosis_4_digit", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'ICD9 Diagnosis 4 Digit' as [Query Type] FROM icd9_diagnosis_4_digit");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'ICD9 Diagnosis 4 Digit' as [Query Type] FROM icd9_diagnosis_4_digit");
                    }
                }
                if (tables.Contains("icd9_diagnosis_5_digit", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'ICD9 Diagnosis 5 Digit' as [Query Type] FROM icd9_diagnosis_5_digit");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'ICD9 Diagnosis 5 Digit' as [Query Type] FROM icd9_diagnosis_5_digit");
                    }
                }
                if (tables.Contains("icd9_procedure", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'ICD9 Procedure' as [Query Type] FROM icd9_procedure");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'ICD9 Procedure' as [Query Type] FROM icd9_procedure");
                    }
                }
                if (tables.Contains("icd9_procedure_4_digit", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'ICD9 Procedure 4 Digit' as [Query Type] FROM icd9_procedure_4_digit");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'ICD9 Procedure 4 Digit' as [Query Type] FROM icd9_procedure_4_digit");
                    }
                }
                if (tables.Contains("incident_drug_class", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'Incident Drug Class' as [Query Type] FROM incident_drug_class");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'Incident Drug Class' as [Query Type] FROM incident_drug_class");
                    }
                }
                if (tables.Contains("incident_generic_name", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'Incident Generic Name' as [Query Type] FROM incident_generic_name");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'Incident Generic Name' as [Query Type] FROM incident_generic_name");
                    }
                }
                if (tables.Contains("incident_icd9_diagnosis", StringComparer.OrdinalIgnoreCase))
                {
                    if (sqlSB.Length > 0)
                    {
                        sqlSB.Append(" UNION SELECT DISTINCT(Period), 'Incident ICD9 Diagnosis' as [Query Type] FROM incident_icd9_diagnosis");
                    }
                    else
                    {
                        sqlSB.Append("SELECT DISTINCT(Period), 'Incident ICD9 Diagnosis' as [Query Type] FROM incident_icd9_diagnosis");
                    }
                }

                if (!viewSQL)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sqlSB.ToString();
                        cmd.CommandTimeout = Convert.ToInt32(_settings.GetSetting("CommandTimeout", 120));

                        logger.Debug("Executing query:" + Environment.NewLine + sqlSB.ToString());

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new MetadataRefreshResult
                                {
                                    Raw_Period = reader.GetString(0),
                                    Period = reader.GetString(0).IndexOf('Q') > 0 ? reader.GetString(0).Substring(0, reader.GetString(0).IndexOf('Q')) : reader.GetString(0),
                                    QueryType = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
                else
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("SQL", sqlSB.ToString());
                    formattedResults.Add(row);
                }
            }
            catch (Exception ex)
            {
                errors.Add(new DTO.QueryComposer.QueryComposerResponseErrorDTO
                {
                    Code = "999",
                    Description = ex.UnwindException(true)
                });
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                    conn = null;
                }
            }

            if (!viewSQL)
            {
                Dictionary<string, object> ageGroupsRow = new Dictionary<string, object>();
                ageGroupsRow.Add("DataTable", "Age Group");
                ageGroupsRow.Add("DataAvailabilityAnnualFrom", "N/A");
                ageGroupsRow.Add("DataAvailabilityAnnualTo", "N/A");
                ageGroupsRow.Add("DataAvailabilityQuarterlyFrom", "N/A");
                ageGroupsRow.Add("DataAvailabilityQuarterlyTo", "N/A");
                formattedResults.Add(ageGroupsRow);

                foreach (var res in results.GroupBy(x => x.QueryType))
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("DataTable", res.Key);
                    row.Add("DataAvailabilityAnnualFrom", res.Min(x => x.Period));
                    row.Add("DataAvailabilityAnnualTo", res.Max(x => x.Period));
                    row.Add("DataAvailabilityQuarterlyFrom", res.Any(x => x.Raw_Period.Contains("Q")) ? res.Where(x => x.Raw_Period.Contains("Q")).Min(x => x.Raw_Period) : "N/A");
                    row.Add("DataAvailabilityQuarterlyTo", res.Any(x => x.Raw_Period.Contains("Q")) ? res.Where(x => x.Raw_Period.Contains("Q")).Max(x => x.Raw_Period) : "N/A");
                    formattedResults.Add(row);
                }

                foreach (var res in results)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("DataTable", res.QueryType);
                    row.Add("Period", res.Raw_Period);
                    unFormattedResults.Add(row);
                }
            }

            var viewableResponse = new DTO.QueryComposer.QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { formattedResults }
            };

            var nonViewableResponse = new DTO.QueryComposer.QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { unFormattedResults }
            };

            viewableResponse.Properties = GetViewableResponsePropertyDefinitions();
            viewableResponse.Aggregation = GetViewableResponseAggregationDefinition();

            nonViewableResponse.Properties = GetNonViewableResponsePropertyDefinitions();
            nonViewableResponse.Aggregation = GetNonViewableResponseAggregationDefinition();

            _currentViewableResponse = viewableResponse;

            _currentNonViewableResponse = nonViewableResponse;

            return _currentViewableResponse;
        }

        public override QueryComposerModelProcessor.DocumentEx[] OutputDocuments()
        {
            List<QueryComposerModelProcessor.DocumentEx> docs = new List<QueryComposerModelProcessor.DocumentEx>();

            if(_currentViewableResponse == null || _currentNonViewableResponse == null)
                return new QueryComposerModelProcessor.DocumentEx[0];

            return new[] { SerializeResponse(_currentViewableResponse, QueryComposerModelProcessor.NewGuid(), "response.json"), SerializeResponse(_currentNonViewableResponse, QueryComposerModelProcessor.NewGuid(), "refresh-dates.json", "SummaryTables.RefreshDates", false) };
        }

        public override bool CanPostProcess(QueryComposerResponseDTO response, out string message)
        {
            message = "Post processing not supported by Metadata Refresh queries.";

            return false;
        }

        public override void PostProcess(QueryComposerResponseDTO response)
        {
            
        }

        IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetViewableResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataTable", As = "Data Table",  Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataAvailabilityAnnualFrom", As = "Data Availability (Annual) (From)", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataAvailabilityAnnualTo", As = "Data Availability (Annual) (To)", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataAvailabilityQuarterlyFrom", As = "Data Availability (Quarterly) (From)", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataAvailabilityQuarterlyTo", As = "Data Availability (Quarterly) (To)", Type = "System.String" },
            };
        }

        DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetViewableResponseAggregationDefinition()
        {
            return null;
        }

        IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetNonViewableResponsePropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "DataTable", As="Data Table", Type = "System.String" },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = "Period", Type = "System.String" },
            };
        }

        DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetNonViewableResponseAggregationDefinition()
        {
            return null;
        }

        public override void Dispose()
        {
        }

        private class MetadataRefreshResult
        {
            public string Raw_Period { get; set; }
            public string Period { get; set; }
            public string QueryType { get; set; }
        }
    }
}
