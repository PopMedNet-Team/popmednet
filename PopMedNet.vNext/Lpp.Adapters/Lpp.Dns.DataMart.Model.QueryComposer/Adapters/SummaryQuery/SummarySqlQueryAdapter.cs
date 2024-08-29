using log4net;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class SummarySqlQueryAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public QueryComposerResponseQueryResultDTO Execute(QueryComposerQueryDTO query, IDictionary<string, object> settings, bool viewSQL)
        {
            var allTerms = query.FlattenToTerms().Where(t => t.Type == ModelTermsFactory.SqlDistributionID).ToArray();
            if (allTerms.Length == 0)
            {
                //error: cannot mix sql distribution with any other term
                throw new NotSupportedException("Only a single Sql Distribution term can be specified per request. The term cannot be mixed with other terms.");
            }

            if (allTerms.Length > 1)
            {
                //limit to a single sql dist request
                throw new NotSupportedException("Only a single Sql Distribution term can be specified per request.");
            }

            string sql = allTerms[0].GetStringValue("Sql");

            var response = new QueryComposerResponseQueryResultDTO { ID = query.Header.ID, QueryStart = DateTimeOffset.UtcNow };

            if (viewSQL)
            {
                response.Results = new[] {
                    new [] {
                        new Dictionary<string,object>(){
                            { "SQL", sql }
                        }
                    }
                };

                response.Properties = new[] {
                    new QueryComposerResponsePropertyDefinitionDTO {
                        Name = "SQL",
                        Type = "System.String"
                    }
                };

                response.QueryEnd = DateTimeOffset.UtcNow;

                return response;
            }

            List<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> columnProperties = new List<QueryComposerResponsePropertyDefinitionDTO>();
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();

            using(var conn = Utilities.OpenConnection(settings, logger, true))
            using (var cmd = conn.CreateCommand())
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

            response.QueryEnd = DateTimeOffset.UtcNow;
            response.Results = new[] { queryResults };
            response.Properties = columnProperties;

            return response;
        }
    }
}
