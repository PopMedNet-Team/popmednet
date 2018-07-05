using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Lpp.Dns.DataMart.Model
{
    public class SummaryQueryBuilder
    {
        private static string[] queryDelimiters = new string[] {";;"};
        private static SummaryQueryBuilder instance = null;
        private IDictionary<Guid, string> requestTypeQueryTemplate = new Dictionary<Guid, string>
        {
            // Prevalence
            { Guid_GenericName,                          "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_Pharma.txt" },
            { Guid_DrugClass,                            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_Pharma.txt" },
            // NDC no longer supported
            //{ Guid_NDC,                                  "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_NDC.txt" },
            { Guid_ICD9Diagnosis,                        "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_ICD9_HCPCS.txt" },
            { Guid_ICD9Procedures,                       "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_ICD9_HCPCS.txt" },
            { Guid_HCPCSProcedures,                      "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_ICD9_HCPCS.txt" },
            { Guid_EligibilityAndEnrollment,             "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_Enroll.txt" },
            { Guid_ICD9Diagnosis_4_digit,                "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_ICD9_HCPCS.txt" },
            { Guid_ICD9Diagnosis_5_digit,                "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_ICD9_HCPCS.txt" },
            { Guid_ICD9Procedures_4_digit,               "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_ICD9_HCPCS.txt" },
            { Guid_RefreshDates,                         "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_RefreshDates.txt" }, 

            // Incident
            { Guid_Incident_GenericName,                 "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Inci_Pharma.txt" },
            { Guid_Incident_DrugClass,                   "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Inci_Pharma.txt" },
            { Guid_Incident_ICD9Diagnosis,               "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Inci_ICD9Diag.txt" },
            { Guid_Incident_RefreshDates,                "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Inci_RefreshDates.txt" },

            // MFU
            { Guid_MFU_GenericName,                          "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_Pharma.txt" },
            { Guid_MFU_DrugClass,                            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_Pharma.txt" },
            { Guid_MFU_ICD9Diagnosis,                        "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_ICD9_HCPCS.txt" },
            { Guid_MFU_ICD9Procedures,                       "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_ICD9_HCPCS.txt" },
            { Guid_MFU_HCPCSProcedures,                      "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_ICD9_HCPCS.txt" },
            { Guid_MFU_ICD9Diagnosis_4_digit,                "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_ICD9_HCPCS.txt" },
            { Guid_MFU_ICD9Diagnosis_5_digit,                "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_ICD9_HCPCS.txt" },
            { Guid_MFU_ICD9Procedures_4_digit,               "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_ICD9_HCPCS.txt" },
            { Guid_MFU_RefreshDates,                         "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_RefreshDates.txt" },
        };

        private List<string> SQLOverrides = new List<string>(new string[] {
            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_ICD9_HCPCS.txt", /*Tested*/
            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Inci_Pharma.txt", /*Tested - Same Error as Access --*/
            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Inci_ICD9Diag.txt", /*Tested --*/
            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.MFU_Pharma.txt", /*Tested*/
            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_Enroll.txt", /*Tested --*/
            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_ICD9_HCPCS.txt", /*Tested - Same error as Access*/
            "Lpp.Dns.DataMart.Model.Processors.SummaryQuery.Queries.Prev_Pharma.txt" /*Tested - Same Error As Access*/
            /*Refresh Dates Tested*/
            /*No others appear to be in the list of mappings even though they exist in the folder*/
        });

        //Need to test all of the queries not just the ones listed here and may need to fix all of them. *sigh*

        public static SummaryQueryBuilder Instance
        {
            get
            {
                if (instance == null)
                    instance = new SummaryQueryBuilder();

                return instance;
            }
        }

        /// <summary>
        /// Remove comment lines (starting with any spaces followed by dash-dash)
        /// since Access does not understand these.
        /// </summary>
        /// <returns></returns>
        private string RemoveComments(string text)
        {
            var regex = new Regex(@"^[\s]*(--).*$", RegexOptions.Multiline);
            return regex.Replace(text, "");
        }

        public string[] BuildSQLQueries(Guid requestTypeId, string argsXml, bool isMetadataRequest, Lpp.Dns.DataMart.Model.Settings.SQLProvider provider)
        {
            var templateName = requestTypeQueryTemplate[requestTypeId];
            if (provider == Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer && SQLOverrides.Contains(templateName))
                templateName = templateName.Replace(".txt", ".SQLtxt");

            using (StreamReader r = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(templateName)))
            {
                string template = RemoveComments(r.ReadToEnd());

                XmlSerializer serializer = new XmlSerializer(typeof(SummaryRequestModel));
                using (XmlTextReader reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(argsXml))))
                {
                    SummaryRequestModel deserializedModel = (SummaryRequestModel)serializer.Deserialize(reader);
                    //var q = isMetadataRequest ? template : MergeSQL(requestTypeId, template, requestTypeQueryTemplate[requestTypeId], deserializedModel);
                    var q = isMetadataRequest ? template :
                        IsMFU(requestTypeId) ? MergeSQLForMFU(requestTypeId, template, requestTypeQueryTemplate[requestTypeId], deserializedModel) :
                                               MergeSQLNonMFU(requestTypeId, template, requestTypeQueryTemplate[requestTypeId], deserializedModel);
                    return q.Split(queryDelimiters, StringSplitOptions.None);
                }
            }
        }

        private string MergeSQLForMFU(Guid requestTypeId, string template, string templateName, SummaryRequestModel args)
        {
            string query = "";
            string[] years = ExpandYears(args.StartPeriod, args.EndPeriod).Split(',');
            foreach (var year in years)
            {
                query += MergeSQL(requestTypeId, template, templateName, args, year, year) + queryDelimiters[0];
            }
            return query;
        }

        private string MergeSQLNonMFU(Guid requestTypeId, string template, string templateName, SummaryRequestModel args)
        {
            string years = ExpandYears(args.StartPeriod, args.EndPeriod);
            return MergeSQL(requestTypeId, template, templateName, args, years, args.Period);
        }

        private string ExpandYears(string StartPeriod, string EndPeriod)
        {
            string years = "";
            for (int i = Convert.ToInt32(StartPeriod); i <= Convert.ToInt32(EndPeriod) && StartPeriod != EndPeriod; i++)
                years += "'" + i.ToString() + "',";
            return years.EndsWith(",") ? years.Substring(0, years.Length - 1) : "'" + StartPeriod + "'"; // trim last comma
        }

        private string MergeSQL(Guid requestTypeId, string template, string templateName, SummaryRequestModel args, string years, string periods)
        {
            string query = template;

            // Build the MFU stratification union clauses and insert in the query.
            if (IsMFU(requestTypeId))
            {
                using (StreamReader r = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(templateName.Substring(0, templateName.IndexOf(".txt")) + "_" + AGE_STRATIFICATIONS[args.AgeStratification ?? 0] + (args.SexStratification == 4 ? "_MF" : "") + ".txt")))
                {
                    string mfuStratClause = RemoveComments(r.ReadToEnd());
                    query = query.Replace("%STRATIFICATION_CLAUSE%", mfuStratClause);
                }
            }

            // Build the cross join (zero rows) clauses and insert in the query.
            string periodCJ = BuildCrossJoinClause("Year", periods.Split(','), "en");
            string sexCJ = BuildCrossJoinClause("Sex", SEX_STRATIFICATIONS[args.SexStratification ?? 0].Split(','), "sx");
            string cjcs = periodCJ + "," + sexCJ;

            // MFU queries do not have codes.
            // Incident and prevalence pharma codes are drug class and generic names.
            if (!string.IsNullOrWhiteSpace(args.Codes)) 
            {
                // Remove duplicate codes
                string[] codes = (from c in args.Codes.Split(',').Select(p => p.Trim())
                                  where !string.IsNullOrWhiteSpace(c)
                                  select "'" + System.Net.WebUtility.HtmlDecode(c) + "'").Distinct().ToArray();

                //Note: at this point the value of each code may contain a htmldecoded comma.
                
                string codeCJ = "", param;
                switch ("{" + requestTypeId.ToString().ToUpper() + "}")
                {
                    // Prev and MFU
                    case GenericName:
                    case MFU_GenericName:
                    case Incident_GenericName:
                        param = "GenericName";
                        codeCJ = BuildCrossJoinClause(param, codes, "sd");
                        break;
                    case DrugClass:
                    case MFU_DrugClass:
                    case Incident_DrugClass:
                        param = "DrugClass";
                        codeCJ = BuildCrossJoinClause(param, codes, "sd");
                        break;
                    default:
                        param = "code";
                        codes = codes.Select(c => c.Replace(".", "")).ToArray(); // remove period from numeric codes
                        codeCJ = BuildCrossJoinClause(param, "name", codes, args.CodeNames, "sd");
                        break;
                }

                cjcs += "," + codeCJ;
                query = query.Replace("%CODES%", string.Join(",", codes.Select(c => c.Replace("%comma;", ",")).ToArray())); 
            }

            query = query.Replace("%CJC%", cjcs)
             .Replace("ag.%STRATIFICATION%_name", args.AgeStratification == 5 ? "'0+'" : "ag.%STRATIFICATION%_name")
             .Replace("ag.%STRATIFICATION%_sort_order", args.AgeStratification == 5 ? "0" : "ag.%STRATIFICATION%_sort_order")
             .Replace("%STRATIFICATION%", AGE_STRATIFICATIONS[args.AgeStratification ?? 0])
             .Replace("%SETTING%", "'" + (args.Setting ?? "") + "'")
             .Replace("%PERIODS%", periods)
             .Replace("%YEARS%", years)
             .Replace("%SEX%", SEX_STRATIFICATIONS[args.SexStratification ?? 0]);

            // MFU queries have metric types.
            if (args.MetricType != null)
            {
                query = query.Replace("%METRIC_TYPE%", METRIC_TYPES[Convert.ToInt32(args.MetricType)])
                             .Replace("%SD_METRIC_TYPE%", SD_METRIC_TYPES[Convert.ToInt32(args.MetricType)]);
            }

            if (args.OutputCriteria != null)
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

            if (args.Coverage != null)
            {
                switch (args.Coverage)
                {
                    case "DRUG":
                        query = query.Replace("%DRUGCOV%", "'Y'")
                                     .Replace("%MEDCOV%", "'N'")
                                     .Replace("%MEDCOV_AGGREGATED%", "ed.MedCov")
                                     .Replace("%DRUGCOV_AGGREGATED%", "ed.DrugCov");
                        break;
                    case "MED":
                        query = query.Replace("%DRUGCOV%", "'N'")
                                     .Replace("%MEDCOV%", "'Y'")
                                     .Replace("%MEDCOV_AGGREGATED%", "ed.MedCov")
                                     .Replace("%DRUGCOV_AGGREGATED%", "ed.DrugCov");
                        break;
                    case "DRUG|MED":
                        query = query.Replace("%DRUGCOV%", "'Y'")
                                     .Replace("%MEDCOV%", "'Y'")
                                     .Replace("%MEDCOV_AGGREGATED%", "ed.MedCov")
                                     .Replace("%DRUGCOV_AGGREGATED%", "ed.DrugCov");
                        break;
                    default: // ALL
                        query = query.Replace("%DRUGCOV%", "'Y','N'")
                                     .Replace("%MEDCOV%", "'Y','N'")
                                     .Replace("%MEDCOV_AGGREGATED%", "'All'")
                                     .Replace("%DRUGCOV_AGGREGATED%", "'All'");
                        break;
                }
            }

            switch ("{" + requestTypeId.ToString().ToUpper() + "}")
            {
                // Prev and MFU
                case GenericName:
                case MFU_GenericName:
                    query = query.Replace("%NAME_FIELD%", "GenericName")
                                 .Replace("%SD_TABLE%", "GENERIC_NAME");
                    break;
                case DrugClass:
                case MFU_DrugClass:
                    query = query.Replace("%NAME_FIELD%", "DrugClass")
                                 .Replace("%SD_TABLE%", "DRUG_CLASS");
                    break;
                case ICD9Diagnosis:
                case MFU_ICD9Diagnosis:
                    query = query.Replace("%CODE_FIELD%", "DXCode")
                                 .Replace("%NAME_FIELD%", "DXName")
                                 .Replace("%SD_CODE_FIELD%", "code")
                                 .Replace("%SD_TABLE%", "ICD9_DIAGNOSIS");
                    break;
                case ICD9Procedures:
                case MFU_ICD9Procedures:
                    query = query.Replace("%CODE_FIELD%", "PXCode")
                                 .Replace("%NAME_FIELD%", "PXName")
                                 .Replace("%SD_CODE_FIELD%", "code")
                                 .Replace("%SD_TABLE%", "ICD9_PROCEDURE");
                    break;
                case HCPCSProcedures:
                case MFU_HCPCSProcedures:
                    query = query.Replace("%CODE_FIELD%", "PXCode")
                                 .Replace("%NAME_FIELD%", "PXName")
                                 .Replace("%SD_CODE_FIELD%", "px_code")
                                 .Replace("%SD_TABLE%", "HCPCS");
                    break;
                case ICD9Diagnosis_4_digit:
                case MFU_ICD9Diagnosis_4_digit:
                    query = query.Replace("%CODE_FIELD%", "DXCode")
                                 .Replace("%NAME_FIELD%", "DXName")
                                 .Replace("%SD_CODE_FIELD%", "code")
                                 .Replace("%SD_TABLE%", "ICD9_DIAGNOSIS_4_DIGIT");
                    break;
                case ICD9Diagnosis_5_digit:
                case MFU_ICD9Diagnosis_5_digit:
                    query = query.Replace("%CODE_FIELD%", "DXCode")
                                 .Replace("%NAME_FIELD%", "DXName")
                                 .Replace("%SD_CODE_FIELD%", "code")
                                 .Replace("%SD_TABLE%", "ICD9_DIAGNOSIS_5_DIGIT");
                    break;
                case ICD9Procedures_4_digit:
                case MFU_ICD9Procedures_4_digit:
                    query = query.Replace("%CODE_FIELD%", "PXCode")
                                 .Replace("%NAME_FIELD%", "PXName")
                                 .Replace("%SD_CODE_FIELD%", "px_code")
                                 .Replace("%SD_TABLE%", "ICD9_PROCEDURE_4_DIGIT");
                    break;

                // Incidence
                case Incident_GenericName:
                    query = query.Replace("%NAME_FIELD%", "GenericName")
                                 .Replace("%SD_TABLE%", "INCIDENT_GENERIC_NAME");
                    break;
                case Incident_DrugClass:
                    query = query.Replace("%NAME_FIELD%", "DrugClass")
                                 .Replace("%SD_TABLE%", "INCIDENT_DRUG_CLASS");
                    break;
                case Incident_ICD9Diagnosis:
                    query = query.Replace("%CODE_FIELD%", "DXCode")
                                 .Replace("%NAME_FIELD%", "DXName")
                                 .Replace("%SD_CODE_FIELD%", "code")
                                 .Replace("%SD_TABLE%", "INCIDENT_ICD9_DIAGNOSIS");
                    break;
            }

            return query;
        }

        private bool IsMFU(Guid requestTypeId)
        {
            switch ("{" + requestTypeId.ToString().ToUpper() + "}")
            {
                case MFU_GenericName:
                case MFU_DrugClass:
                case MFU_ICD9Diagnosis:
                case MFU_ICD9Procedures:
                case MFU_HCPCSProcedures:
                case MFU_ICD9Diagnosis_4_digit:
                case MFU_ICD9Diagnosis_5_digit:
                case MFU_ICD9Procedures_4_digit:
                    return true;
                default:
                    return false;
            }
        }

        private string BuildCrossJoinClause(string fieldName, string[] values, string asWhat)
        {
            // AGE_GROUPS is used as a dummy table here since {0} is a constant.
            string template = "SELECT {0} AS {1} FROM AGE_GROUPS WHERE id=1";
            string crossJoinClause = "";

            foreach (string value in values)
            {
                crossJoinClause += string.Format(template, value.Replace("%comma;", ","), fieldName);
                if (value != values.Last())
                    crossJoinClause += " UNION ALL ";
            }

            return "(" + crossJoinClause + ") AS " + asWhat;
        }

        private string BuildCrossJoinClause(string fieldName, string fieldName2, string[] values, string[] values2, string asWhat)
        {
            // AGE_GROUPS is used as a dummy table here since {0} is a constant.
            string template = "SELECT {0} AS {1}, {2} AS {3} FROM AGE_GROUPS WHERE id=1";
            string crossJoinClause = "";

            for (int i=0; i < values.Length; i++)
            {
                crossJoinClause += string.Format(template, values[i].Replace("%comma;", ","), fieldName, 
                                                           values2[i].Replace("%comma;", ","), fieldName2);
                if (values[i] != values.Last())
                    crossJoinClause += " UNION ALL ";
            }

            return "(" + crossJoinClause + ") AS " + asWhat;
        }

        #region New Query Type GUID staticants
        //
        // BMS: Just stumbled across this, there are actually two lists that contain the GUID for the summary query types, this one and another in RequestTypes.cs, no idea why, but sure seems like a really bad idea!
        // What is going on with all these guids, there needs to be one pre query type and I see multiple copies and versions, WTF?
        // =================================================
        // Used in the SummaryRequestType declarations above
        // =================================================

        // ALWAYS use uppercase in these GUID staticant declarations,
        // or the switch statements in SummaryQueryBuilder.cs will fail.

        // Prevalence
        public const string GenericName = "{0FFC0001-A09D-423C-B47A-A22200F72944}";
        public const string DrugClass = "{C97B0001-E7AC-483D-ACFA-A22200F7577D}";
        //public const string NDC = "{6EBA70E0-1E79-4F58-ABA3-D349AA0F4D29}"; // No longer supported
        public const string ICD9Diagnosis = "{3B200001-591E-4A38-BE42-A22200F77798}";
        public const string ICD9Procedures = "{2B3E0001-4D8B-4F9D-B07E-A22200F79B6E}";
        public const string HCPCSProcedures = "{57530001-68D5-4A8D-BDD2-A22200F7CFB9}";
        public const string EligibilityAndEnrollment = "{C8F50001-7893-43C4-A9A0-A22200F7EA52}";
        public const string ICD9Diagnosis_4_digit = "{3D2B0001-37AD-4294-BC9A-A22200F80CBA}";
        public const string ICD9Diagnosis_5_digit = "{98F50001-C222-4444-83BD-A22200F82A09}";
        public const string ICD9Procedures_4_digit = "{19420001-59CA-4968-8B5A-A22200F8429B}";
        public const string RefreshDates = "{58190001-7DE2-4CC0-9D68-A22200F85BAF}";

        // Incidence
        public const string Incident_GenericName = "{BB4E0001-B88E-422D-91EC-A22200F88AB7}";
        public const string Incident_DrugClass = "{50B60001-C065-4803-BD3F-A22200F8A07D}";
        public const string Incident_ICD9Diagnosis = "{826D0001-42FD-4B7E-923B-A22200F8B54C}";
        public const string Incident_RefreshDates = "{55290001-A764-482B-A4AB-A22200F8D293}";

        // MFU (and Refresh Dates)
        public const string MFU_GenericName = "{33F80001-0E03-473E-AE7A-A22200F8F0E0}";
        public const string MFU_DrugClass = "{DF4A0001-CD75-43F8-9771-A22200F908F4}";
        public const string MFU_ICD9Diagnosis = "{D44C0001-7E49-4808-899A-A22200F91E84}"; // 3 digits
        public const string MFU_ICD9Procedures = "{B4B60001-C6DA-4AE1-A794-A22200F93498}"; // 3 digits
        public const string MFU_HCPCSProcedures = "{151E0001-0DC7-43BA-BEF0-A22200F94A81}";
        public const string MFU_ICD9Diagnosis_4_digit = "{CD9F0001-6CDA-4107-875E-A22200F9606E}";
        public const string MFU_ICD9Diagnosis_5_digit = "{27B00001-F8EE-4D6A-B714-A22200F9768D}";
        public const string MFU_ICD9Procedures_4_digit = "{E7B10001-96D6-4896-846F-A22200F98D02}";
        public const string MFU_RefreshDates = "{70DD0001-ABFC-4D71-91F4-A22200F9A30F}";

        // Prevalence
        public static Guid Guid_GenericName = Guid.Parse(GenericName);
        public static Guid Guid_DrugClass = Guid.Parse(DrugClass);
        //public static Guid Guid_NDC = Guid.Parse(NDC); // no longer supported
        public static Guid Guid_ICD9Diagnosis = Guid.Parse(ICD9Diagnosis);
        public static Guid Guid_ICD9Procedures = Guid.Parse(ICD9Procedures);
        public static Guid Guid_HCPCSProcedures = Guid.Parse(HCPCSProcedures);
        public static Guid Guid_EligibilityAndEnrollment = Guid.Parse(EligibilityAndEnrollment);
        public static Guid Guid_ICD9Diagnosis_4_digit = Guid.Parse(ICD9Diagnosis_4_digit);
        public static Guid Guid_ICD9Diagnosis_5_digit = Guid.Parse(ICD9Diagnosis_5_digit);
        public static Guid Guid_ICD9Procedures_4_digit = Guid.Parse("{19420001-59CA-4968-8B5A-A22200F8429B}");
        public static Guid Guid_RefreshDates = Guid.Parse("{58190001-7DE2-4CC0-9D68-A22200F85BAF}");

        // Incidence
        public static Guid Guid_Incident_GenericName = Guid.Parse("{BB4E0001-B88E-422D-91EC-A22200F88AB7}");
        public static Guid Guid_Incident_DrugClass = Guid.Parse("{50B60001-C065-4803-BD3F-A22200F8A07D}");
        public static Guid Guid_Incident_ICD9Diagnosis = Guid.Parse("{826D0001-42FD-4B7E-923B-A22200F8B54C}");
        public static Guid Guid_Incident_RefreshDates = Guid.Parse("{55290001-A764-482B-A4AB-A22200F8D293}");

        // MFU (and Refresh Dates)
        public static Guid Guid_MFU_GenericName = Guid.Parse("{33F80001-0E03-473E-AE7A-A22200F8F0E0}");
        public static Guid Guid_MFU_DrugClass = Guid.Parse("{DF4A0001-CD75-43F8-9771-A22200F908F4}");
        public static Guid Guid_MFU_ICD9Diagnosis = Guid.Parse("{D44C0001-7E49-4808-899A-A22200F91E84}"); // 3 digits
        public static Guid Guid_MFU_ICD9Procedures = Guid.Parse("{B4B60001-C6DA-4AE1-A794-A22200F93498}"); // 3 digits
        public static Guid Guid_MFU_HCPCSProcedures = Guid.Parse("{151E0001-0DC7-43BA-BEF0-A22200F94A81}");
        public static Guid Guid_MFU_ICD9Diagnosis_4_digit = Guid.Parse("{CD9F0001-6CDA-4107-875E-A22200F9606E}");
        public static Guid Guid_MFU_ICD9Diagnosis_5_digit = Guid.Parse("{27B00001-F8EE-4D6A-B714-A22200F9768D}");
        public static Guid Guid_MFU_ICD9Procedures_4_digit = Guid.Parse("{E7B10001-96D6-4896-846F-A22200F98D02}");
        public static Guid Guid_MFU_RefreshDates = Guid.Parse("{70DD0001-ABFC-4D71-91F4-A22200F9A30F}");

        #endregion

        private string[] AGE_STRATIFICATIONS = { "", "strat10", "strat7", "strat4", "strat2", "strat0" };
        private string[] METRIC_TYPES = { "", "Events", "Members", "Dispensings", "DaysSupply" };
        private string[] SD_METRIC_TYPES = { "", "ev", "mb", "dp", "ds" };
        private string[] SEX_STRATIFICATIONS = { "", "'F'", "'M'", "'M', 'F'", "'M', 'F'" }; // Last one if M/F aggregated
    }
}
