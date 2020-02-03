using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Lpp.Dns.DataMart.Model.Settings;
using log4net;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public interface IQueryAdapter : IDisposable
    {
        DTO.QueryComposer.QueryComposerResponseDTO Execute(DTO.QueryComposer.QueryComposerRequestDTO request, bool viewSQL);
    }

    public abstract class QueryAdapter : IQueryAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly IDictionary<string, object> settings;

        protected double? _lowThresholdValue = null;

        public const string LowThresholdColumnName = "LowThreshold";

        public QueryAdapter(IDictionary<string, object> settings)
        {
            this.settings = settings;

            object settingValue;
            if (settings != null && settings.TryGetValue("LowThresholdValue", out settingValue))
            {
                double value;
                if (double.TryParse((settingValue ?? string.Empty).ToString(), out value))
                    _lowThresholdValue = value;
            }
        }

        public virtual DTO.QueryComposer.QueryComposerResponseDTO Execute(DTO.QueryComposer.QueryComposerRequestDTO request, bool viewSQL)
        {
            SummaryRequestModel args = ConvertToModel(request);

            IEnumerable<string> queries = BuildQueries(args);
            
            List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors = new List<DTO.QueryComposer.QueryComposerResponseErrorDTO>();

            List<Dictionary<string, object>> resultSets = new List<Dictionary<string, object>>(500);
            foreach (var query in queries)
            {
                IEnumerable<Dictionary<string, object>> resultSet = ExecuteQuery(query, errors, viewSQL);
                if (resultSet != null)
                {
                    resultSets.AddRange(resultSet);
                }

                if (errors.Count > 0)
                {
                    break;
                }
            }

            var response = new DTO.QueryComposer.QueryComposerResponseDTO
            {
                Errors = errors,
                RequestID = request.ID.HasValue ? request.ID.Value : default(Guid),
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { resultSets }
            };

            response.Properties = GetResponsePropertyDefinitions();
            if (_lowThresholdValue.HasValue)
            {
                response.Properties = response.Properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }
            response.Aggregation = GetResponseAggregationDefinition();

            return response;
        }

        public abstract void Dispose();

        protected Lpp.Dns.DataMart.Model.Settings.SQLProvider DataProvider
        {
            get
            {
                if (settings == null)
                    throw new Exception(CommonMessages.Exception_MissingSettings);

                if (!settings.ContainsKey("DataProvider"))
                    throw new Exception(CommonMessages.Exception_MissingDataProviderType);

                return (Lpp.Dns.DataMart.Model.Settings.SQLProvider)Enum.Parse(typeof(Lpp.Dns.DataMart.Model.Settings.SQLProvider), settings.GetAsString("DataProvider", ""), true);
            }
        }

        /// <summary>
        /// Indicates if the query if for MFU.
        /// </summary>
        protected abstract bool IsMFU { get; }

        /// <summary>
        /// Gets the sql template for the query adapter.
        /// </summary>
        protected abstract string Template { get; }

        /// <summary>
        /// Gets the raw MFU stratification clause.
        /// </summary>
        protected virtual string MFUStratificationClause(SummaryRequestModel args)
        {
            return null;
        } 

        /// <summary>
        /// Converts the request dto into a SummaryRequestModel that the query translator can use.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract SummaryRequestModel ConvertToModel(DTO.QueryComposer.QueryComposerRequestDTO request);

        /// <summary>
        /// Applies crossjoin (as applicable) to the query.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual void ApplyCrossJoinForCodes(SummaryRequestModel args, ref string query, ref string cjcs)
        {
        }

        /// <summary>
        /// Do any query adapter specific parameter replacement.
        /// </summary>
        /// <param name="query">The query string containing the paramters to replace.</param>
        /// <returns>The modified query string.</returns>
        protected abstract void ReplaceParameters(ref string query);

        /// <summary>
        /// Gets a collection of property definitions for the shape of the result set.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> GetResponsePropertyDefinitions()
        {
            return Enumerable.Empty<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO>();
        }

        /// <summary>
        /// Gets the aggregation definition for the response that indicates a valid shape of the results if aggregated.
        /// </summary>
        /// <returns></returns>
        protected virtual DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO GetResponseAggregationDefinition()
        {
            return null;
        }
        public IEnumerable<QueryComposerTermDTO> GetAllCriteriaTerms(QueryComposerCriteriaDTO paragraph, Guid termTypeID)
        {
            return paragraph.Terms.Where(t => t.Type == termTypeID).Concat(paragraph.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == termTypeID)));
        }
        protected IEnumerable<string> BuildQueries(SummaryRequestModel args)
        {
            IEnumerable<int> years = ExpandYears(args);
            List<string> queries = new List<string>();
            if (IsMFU)
            {
                foreach (int year in years)
                {
                    string yearString = "'" + year + "'";
                    queries.Add(MergeSql(args, yearString, yearString));
                }
            }
            else
            {
                queries.Add(MergeSql(args, string.Join(",", years.Select(y => "'" + y + "'")), args.Period));
            }

            return queries;
        }

        string MergeSql(SummaryRequestModel args, string years, string periods)
        {
            string query = Template;

            query = CleanComments(query);

            if (IsMFU)
            {
                string mfuStratClause = CleanComments(MFUStratificationClause(args));

                if(!string.IsNullOrWhiteSpace(mfuStratClause))
                    query = query.Replace("%STRATIFICATION_CLAUSE%", mfuStratClause);
            }

            // Build the cross join (zero rows) clauses and insert in the query.
            string periodCJ = BuildCrossJoinClause("Year", periods.Split(','), "en");
            string sexCJ = BuildCrossJoinClause("Sex", SEX_STRATIFICATIONS[args.SexStratification ?? 0].Split(','), "sx");
            string cjcs = periodCJ + "," + sexCJ;

            ApplyCrossJoinForCodes(args, ref query, ref cjcs);

            query = query.Replace("%CJC%", cjcs)
             .Replace("ag.%STRATIFICATION%_name", args.AgeStratification == 5 ? "'0+'" : "ag.%STRATIFICATION%_name")
             .Replace("ag.%STRATIFICATION%_sort_order", args.AgeStratification == 5 ? "0" : "ag.%STRATIFICATION%_sort_order")
             .Replace("%STRATIFICATION%", AGE_STRATIFICATIONS[args.AgeStratification ?? 0])
             .Replace("%SETTING%", "'" + (args.Setting ?? "") + "'")
             .Replace("%PERIODS%", periods)
             .Replace("%YEARS%", years)
             .Replace("%SEX%", SEX_STRATIFICATIONS[args.SexStratification ?? 0]);

            // MFU queries have metric types.
            if (!string.IsNullOrWhiteSpace(args.MetricType))
            {
                query = query.Replace("%METRIC_TYPE%", METRIC_TYPES[Convert.ToInt32(args.MetricType)])
                             .Replace("%SD_METRIC_TYPE%", SD_METRIC_TYPES[Convert.ToInt32(args.MetricType)]);
            }

            if (!string.IsNullOrWhiteSpace(args.OutputCriteria))
                query = query.Replace("%OUTPUT_CRITERIA%", args.OutputCriteria.ToString());

            // SexStratification == 4 is M and F aggregated
            if (args.SexStratification == 4)
            {
                query = query.Replace("%SEX_AGGREGATED%", "'All'")
                             .Replace("%MATCH_SEX%", "")
                             .Replace("%MATCH_SEX2%", "")
                             .Replace("%MATCH_SEX3%", "");
            }
            else
            {
                query = query.Replace("%SEX_AGGREGATED%", "sx.sex")
                             .Replace("%MATCH_SEX%", "AND ed.Sex = AgeGroups.Sex")
                             .Replace("%MATCH_SEX2%", "AND SummaryData.Sex = EnrollmentData.Sex")
                             .Replace("%MATCH_SEX3%", "sex,");
            }

            if (args.Coverage.HasValue)
            {
                switch (args.Coverage.Value)
                {
                    case DTO.Enums.Coverages.DRUG:
                        query = query.Replace("%DRUGCOV%", "'Y'")
                                     .Replace("%MEDCOV%", "'N'")
                                     .Replace("%MEDCOV_AGGREGATED%", "ed.MedCov")
                                     .Replace("%DRUGCOV_AGGREGATED%", "ed.DrugCov");
                        break;
                    case DTO.Enums.Coverages.MED:
                        query = query.Replace("%DRUGCOV%", "'N'")
                                     .Replace("%MEDCOV%", "'Y'")
                                     .Replace("%MEDCOV_AGGREGATED%", "ed.MedCov")
                                     .Replace("%DRUGCOV_AGGREGATED%", "ed.DrugCov");
                        break;
                    case DTO.Enums.Coverages.DRUG_MED:
                        query = query.Replace("%DRUGCOV%", "'Y'")
                                     .Replace("%MEDCOV%", "'Y'")
                                     .Replace("%MEDCOV_AGGREGATED%", "ed.MedCov")
                                     .Replace("%DRUGCOV_AGGREGATED%", "ed.DrugCov");
                        break;
                    case DTO.Enums.Coverages.ALL:
                    default: // ALL
                        query = query.Replace("%DRUGCOV%", "'Y','N'")
                                     .Replace("%MEDCOV%", "'Y','N'")
                                     .Replace("%MEDCOV_AGGREGATED%", "'All'")
                                     .Replace("%DRUGCOV_AGGREGATED%", "'All'");
                        break;
                }
            }

            ReplaceParameters(ref query);

            return query;
        }

        protected IEnumerable<Dictionary<string, object>> ExecuteQuery(string query, List<DTO.QueryComposer.QueryComposerResponseErrorDTO> errors, bool viewSQL)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            List<string> ltCols = new List<string>();

            System.Data.IDbConnection conn = null;
            try
            {
                if (!viewSQL)
                {
                    conn = Utilities.OpenConnection(settings, logger);

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandTimeout = Convert.ToInt32(settings.GetSetting("CommandTimeout", 120));

                        logger.Debug("Executing query:" + Environment.NewLine + query);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row.Add(reader.GetName(i), reader.GetValue(i).ConvertDBNullToNull());
                                }
                                results.Add(row);
                            }
                        }
                        var testRow = results.FirstOrDefault();
                        if (testRow != null && testRow.Count > 0)
                        {
                            try
                            {
                                foreach (string column in testRow.Keys)
                                {
                                    if (SummaryQueryUtil.IsCheckedColumn(column))
                                    {
                                        ltCols.Add(column);
                                        var depCols = SummaryQueryUtil.GetDependentComputedColumns(column, testRow);
                                        if (depCols != null && depCols.Count > 0)
                                            ltCols.AddRange(depCols);
                                    }
                                }
                            }
                            catch { }
                        }
                        var columnNames = ltCols.ToArray();
                        if (columnNames != null && columnNames.Length != 0 && _lowThresholdValue.HasValue)
                        {
                            foreach (Dictionary<string, object> row in results)
                            {
                                try
                                {
                                    foreach (string column in columnNames)
                                    {
                                        object currentValue;
                                        if (row.TryGetValue(column, out currentValue))
                                        {
                                            double value = Convert.ToDouble(currentValue);
                                            if (value > 0 && value < _lowThresholdValue)
                                            {
                                                row[LowThresholdColumnName] = true;
                                            }

                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                else
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("SQL", query);
                    results.Add(row);
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

            return results;
        }        

        protected static string BuildCrossJoinClause(string fieldName, string[] values, string asWhat)
        {
            // AGE_GROUPS is used as a dummy table here since {0} is a constant.
            string template = "SELECT {0} AS {1} FROM AGE_GROUPS WHERE id=1";
            return BuildCrossJoinClauseWithTemplate(template, fieldName, values, asWhat);
        }

        protected static string BuildCrossJoinClauseForICD9Diagnosis(string fieldName, string[] values, string asWhat)
        {
            // AGE_GROUPS is used as a dummy table here since {0} is a constant.
            string template = "SELECT {0} AS {1}, (SELECT TOP 1 dxname FROM INCIDENT_ICD9_DIAGNOSIS WHERE code = {0}) AS name FROM AGE_GROUPS WHERE id=1";
            return BuildCrossJoinClauseWithTemplate(template, fieldName, values, asWhat);
        }

        protected static string BuildCrossJoinClauseForICD9Diagnosis(string[] codes, string[] codeNames, string asWhat)
        {
            string crossJoinClause = "";
            //assumes code values have already been quoted
            const string template = "SELECT {0} AS code, '{1}' AS name FROM AGE_GROUPS WHERE id=1";
            for (int i = 0; i < codes.Length; i++)
            {
                crossJoinClause += string.Format(template, codes[i].Replace("%comma;", ","), codeNames[i].Replace("%comma;", ","));

                if (codes[i] != codes.Last())
                    crossJoinClause += " UNION ALL ";
            }

            return "(" + crossJoinClause + ") AS " + asWhat;
        }

        protected static string BuildCrossJoinClauseForHCPCSProcedures(string fieldName, string[] values, string asWhat)
        {
            string template = "SELECT {0} AS {1}, (SELECT TOP 1 pxname FROM HCPCS WHERE px_code = {0}) AS name FROM AGE_GROUPS WHERE id=1";
            return BuildCrossJoinClauseWithTemplate(template, fieldName, values, asWhat);
        }

        protected static string BuildCrossJoinClauseWithTemplate(string template, string fieldName, string[] values, string asWhat)
        {
            string crossJoinClause = "";

            for (int i = 0; i < values.Length; i++)
            {
                crossJoinClause += string.Format(template, values[i].Replace("%comma;", ","), fieldName);

                if (values[i] != values.Last())
                    crossJoinClause += " UNION ALL ";
            }

            return "(" + crossJoinClause + ") AS " + asWhat;
        }

        /// <summary>
        /// Parses the Codes property of the ars by splitting on ',' and trimming.
        /// Returns only distinct non-whitespace values as an enumerable string collection.
        /// </summary>
        /// <param name="args">The query args.</param>
        /// <param name="replaceDecimals">Indicates if decimals should be replaced with empty string, default is false.</param>
        /// <returns></returns>
        protected static IEnumerable<string> ParseCodeValues(SummaryRequestModel args, bool replaceDecimals = false, bool htmlDecode = false)
        {
            return (from c in args.Codes.Split(',').Select(p => htmlDecode ? System.Net.WebUtility.HtmlDecode(p.Trim().Replace("&#44;", ",")) : p.Trim())
                    where !string.IsNullOrWhiteSpace(c)
                    select "'" + (replaceDecimals ? c.Replace(".", string.Empty) : c) + "'").Distinct().ToArray();
        }

        /// <summary>
        /// Expands the range of years of the SummaryRequestModel.StartPeriod to SummaryRequestModel.EndPeriod to an enumerable collection of year values.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected static IEnumerable<int> ExpandYears(SummaryRequestModel args)
        {
            return ExpandYears(Convert.ToInt32(args.StartPeriod), Convert.ToInt32(args.EndPeriod));          
        }

        /// <summary>
        /// Expands the range of numbers specified into a collection of values.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected static IEnumerable<int> ExpandYears(int start, int end)
        {
            for (int i = start; i <= end; i++)
                yield return i;
        }

        /// <summary>
        /// Expands the range of year quarters starting at the specified year/quarter, and ending at the specified end year/quarter into a string collection.
        /// </summary>
        /// <param name="startYear">The starting year.</param>
        /// <param name="startQuarter">The starting quarter.</param>
        /// <param name="endYear">The ending year.</param>
        /// <param name="endQuarter">The ending quarter.</param>
        /// <returns></returns>
        protected static IEnumerable<string> ExpandYearsWithQuarters(int startYear, int startQuarter, int endYear, int endQuarter)
        {
            for(int year = startYear; year <= endYear; year++)
            {
                int startQ = year == startYear ? startQuarter : 1;
                int endQ = year == endYear ? endQuarter : 4;

                for(int quarter = startQ; quarter <= endQ; quarter++)
                {
                    yield return string.Format("{0}Q{1}", year, quarter);
                }
            }
        }

        protected static void SetAgeStratification(SummaryRequestModel model, DTO.QueryComposer.QueryComposerFieldDTO field)
        {
            DTO.Enums.AgeStratifications value;
            if (field.StratifyBy == null || !Enum.TryParse<DTO.Enums.AgeStratifications>((field.StratifyBy ?? string.Empty).ToString(), out value))
            {
                throw new Exception("Value for Age Term is not Valid");                
            }

            if (value == DTO.Enums.AgeStratifications.FiveYearGrouping || value == DTO.Enums.AgeStratifications.TenYearGrouping)
            {
                throw new ArgumentOutOfRangeException("AgeStratification", value.ToString(), "5 and 10 year groupings are not valid age stratifications for Summary queries.");
            }

            model.AgeStratification = (int)value;

            string description = Utilities.ToString(value);
            model.AgeStratificationBy = description.Substring(0, description.IndexOf("(")).Trim();
        }

        /// <summary>
        /// Attempts to set the sex stratifiction value of the SummaryRequestModel from the specified QueryComposerFieldDTO.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="field"></param>
        protected static void SetSexStratification(SummaryRequestModel model, DTO.QueryComposer.QueryComposerTermDTO field)
        {
            DTO.Enums.SexStratifications value;
            Newtonsoft.Json.Linq.JObject job = field.Values.Select(x => x.Value).FirstOrDefault() as Newtonsoft.Json.Linq.JObject;
            if (field.Values == null || !Enum.TryParse<DTO.Enums.SexStratifications>((job.GetValue("Sex") ?? string.Empty).ToString(), out value))
            {
                throw new Exception("Value for Sex Term is not Valid");
            }

            model.SexStratification = (int)value;
            model.Sex = Utilities.ToString(value);
        }

        /// <summary>
        /// Removes comments from sql.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        static string CleanComments(string query)
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = query.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("--"))
                    continue;
                
                int j = line.IndexOf("--");
                if (j > 0)
                {
                    line = line.Substring(0, j);
                }

                if (!string.IsNullOrWhiteSpace(line))
                    sb.AppendLine(line);
            }

            return sb.ToString();
        }

        public virtual DTO.QueryComposer.QueryComposerFieldDTO GetAgeField(IEnumerable<DTO.QueryComposer.QueryComposerFieldDTO> ageFields)
        {
            return ageFields.Where(x => x.StratifyBy != null).FirstOrDefault();
        }

        readonly string[] AGE_STRATIFICATIONS = { "", "strat10", "strat7", "strat4", "strat2", "strat0" };
        readonly string[] METRIC_TYPES = { "", "Events", "Members", "Dispensings", "DaysSupply" };
        readonly string[] SD_METRIC_TYPES = { "", "ev", "mb", "dp", "ds" };
        readonly string[] SEX_STRATIFICATIONS = { "", "'F'", "'M'", "'M', 'F'", "'M', 'F'" }; // Last one if M/F aggregated

    }
}
