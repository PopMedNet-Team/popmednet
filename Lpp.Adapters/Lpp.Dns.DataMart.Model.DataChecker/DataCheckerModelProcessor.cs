using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Diagnostics;
using log4net;
using RequestCriteria.Models;
using Lpp.Dns.General.CriteriaGroupCommon;
using System.Text;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class DataCheckerModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PROCESSOR_ID = "5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F";
        private IModelMetadata modelMetadata = new DataCheckerModelMetadata();
        private Document[] responseDocument;

        [Serializable]
        internal class DataCheckerModelMetadata : IModelMetadata
        {
            const string MODEL_ID = "CE347EF9-3F60-4099-A221-85084F940EDE";
            readonly Settings.SQLProvider[] sqlProviders;
            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public DataCheckerModelMetadata()
            {
                sqlProviders = new[] { Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer, Lpp.Dns.DataMart.Model.Settings.SQLProvider.ODBC };

                capabilities = new Dictionary<string, bool>() { { "IsSingleton", false } };

                properties = new Dictionary<string, string>();
                Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
                properties.Add("Translator", string.Empty);
            }
            
            public string ModelName
            {
                get { return "DataCheckerModelProcessor"; }
            }

            public Guid ModelId
            {
                get { return Guid.Parse(MODEL_ID); }
            }

            public string Version
            {
                get { return "0.1"; }
            }

            public IDictionary<string, bool> Capabilities
            {
                get { return capabilities; }
            }

            public IDictionary<string, string> Properties
            {
                get { return properties; }
            }

            public ICollection<Settings.ProcessorSetting> Settings
            {
                get
                {
                    List<Lpp.Dns.DataMart.Model.Settings.ProcessorSetting> settings = new List<Settings.ProcessorSetting>();
                    settings.AddRange(Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.CreateDbSettings(SQlProviders));
                    settings.Add(new Settings.ProcessorSetting { Title = "Low Cell Count", Key = "ThreshHoldCellCount", DefaultValue = "0", ValueType = typeof(int) });
                    settings.Add(new Settings.ProcessorSetting { 
                        Title = "Translator",
                        Key = "Translator",
                        DefaultValue = "",
                        ValueType=typeof(string),
                        ValidValues = new[] { new KeyValuePair<string, object>("MiniSentinel Data Checker Model for ODBC", Lpp.Dns.DataMart.Model.Settings.RequestTranslator.MSDCM_ODBC), new KeyValuePair<string, object>("MiniSentinel Data Checker Model for SQL Server", Lpp.Dns.DataMart.Model.Settings.RequestTranslator.MSDCM_SQLServer) }
                    });

                    return settings;
                }
            }

            public IEnumerable<Settings.SQLProvider> SQlProviders
            {
                get
                {
                    return sqlProviders;
                }
            }
        }

        public Guid ModelProcessorId
        {
            get { return Guid.Parse(PROCESSOR_ID); }
        }

        public IModelMetadata ModelMetadata
        {
            get { return modelMetadata; }
        }

        public IDictionary<string, object> Settings
        {
            get;
            set;
        }

        #region Model Processor Life Cycle Methods

        private string requestTypeId = null;
        private RequestStatus status = new RequestStatus();
        private DataSet resultDataset;
        private string query = string.Empty;
        private bool hasCellCountAlert = false;
        private string styleXml;

        private int CellThreshold
        {
            get
            {
                int val = 0;
                try
                {
                    if (Settings != null && Settings.ContainsKey("ThreshHoldCellCount"))
                    {
                        int.TryParse(Settings.GetAsString("ThreshHoldCellCount", "0"), out val);
                    }
                }
                catch { }
                return val;
            }
        }

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
        }

        public void Request(string requestId, NetworkConnectionMetadata network, RequestMetadata md,
            Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments)
        {
            this.requestTypeId = md.RequestTypeId;
            requestProperties = null;
            Document doc = requestDocuments.Where(d => !d.IsViewable).FirstOrDefault();
            desiredDocuments = new Document[] { doc };
            resultDataset = new DataSet();
            status.Code = RequestStatus.StatusCode.InProgress;
            status.Message = "";
        }

        public void RequestDocument(string requestId, string documentId, Stream contentStream)
        {
            if (Settings == null || Settings.Count == 0)
                throw new Exception(CommonMessages.Exception_MissingSettings);

            if (!Settings.ContainsKey("DataProvider"))
                throw new Exception(CommonMessages.Exception_MissingDataProviderType);

            Guid typeID;
            if (!Guid.TryParse(requestTypeId, out typeID))
            {
                throw new ArgumentException("Invalid request type ID: " + requestTypeId);
            }

            try
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    RequestCriteriaData requestCriteriaData = null;
                    string criteriaGroupsJSON = reader.ReadToEnd();
                    requestCriteriaData = RequestCriteriaHelper.ToServerModel(criteriaGroupsJSON);

                    List<string> ethnicitiesCriteria = new List<string>();
                    List<int> raceCritieria = new List<int>();
                    List<string> pdxCriteria = new List<string>();
                    List<string> encounterTypeCriteria = new List<string>();
                    List<double> rxCriteria = new List<double>();

                    List<string> metadataTableTypeCriteria = new List<string>();

                    string where = string.Empty;
                    foreach (var criteria in requestCriteriaData.Criterias)
                    {
                        foreach (var term in criteria.Terms)
                        {
                            switch (term.TermType)
                            {
                                case TermTypes.EthnicityTerm:
                                    EthnicityData ethnicityData = term as EthnicityData;
                                    if (ethnicityData.Ethnicities != null)
                                    {
                                        ethnicitiesCriteria.AddRange(ethnicityData.Ethnicities.Select(eth => EthnicityDataTypeToDatabaseCode(eth)));
                                    }
                                    break;

                                case TermTypes.RaceTerm:
                                    RaceData raceData = term as RaceData;
                                    if (raceData.Races != null)
                                    {
                                        raceCritieria.AddRange(raceData.Races.Select(r => RaceDataTypeToDatabaseCode(r)));
                                    }
                                    break;

                                case TermTypes.MetricTerm:
                                    MetricsData metricData = term as MetricsData;
                                    break;
                                case TermTypes.CodesTerm:
                                    CodesData codesData = term as CodesData;

                                    if (codesData.Codes != null && codesData.Codes != string.Empty)
                                    {
                                        string columnName;
                                        switch (codesData.CodesTermType)
                                        {
                                            case CodesTermTypes.Diagnosis_ICD9Term:
                                                columnName = "DX";
                                                break;
                                            case CodesTermTypes.Procedure_ICD9Term:
                                                columnName = "PX";
                                                break;
                                            case CodesTermTypes.NDCTerm:
                                                columnName = "NDC";
                                                break;
                                            default:
                                                columnName = "NDC";
                                                break;
                                        }
                                        if (codesData.SearchMethodType == SearchMethodTypes.ExactMatch)
                                            where += BuildInList<string>(where, columnName, codesData.Codes.Split(','));
                                        else if (codesData.SearchMethodType == SearchMethodTypes.StartsWith)
                                            where += BuildLikeList<string>(where, columnName, codesData.Codes.Split(','));

                                        if (codesData.CodeType != null && codesData.CodeType != string.Empty)
                                        {
                                            switch (codesData.CodesTermType)
                                            {
                                                case CodesTermTypes.Diagnosis_ICD9Term:
                                                    where += BuildInList<string>(where, "DX_CODETYPE", new[]{ codesData.CodeType });
                                                    break;
                                                case CodesTermTypes.Procedure_ICD9Term:
                                                    where += BuildInList<string>(where, "PX_CODETYPE", new[]{ codesData.CodeType });
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case TermTypes.DataPartnerTerm:
                                    DataPartnersData dataPartnersData = term as DataPartnersData;
                                    if (dataPartnersData.DataPartners != null && dataPartnersData.DataPartners.Count() > 0)
                                    {
                                        where += BuildInList<string>(where, "DP", dataPartnersData.DataPartners);
                                    }
                                    break;

                                case TermTypes.PDXTerm:
                                    PDXData pdxData = term as PDXData;
                                    if (pdxData.PDXes != null)
                                    {
                                        pdxCriteria.AddRange(pdxData.PDXes.Select(pdx => PDXDataTypeToDatabaseCode(pdx)));
                                    }
                                    break;

                                case TermTypes.EncounterTypeTerm:
                                    EncounterData encounterTypeData = term as EncounterData;
                                    if (encounterTypeData.Encounters != null)
                                    {
                                        encounterTypeCriteria.AddRange(encounterTypeData.Encounters.Select(enc => EncounterDataTypeToDatabaseCode(enc)));
                                    }
                                    break;
								//case TermTypes.MetaDataTableTerm:
                                //    var tableTypeData = term as MetaDataTableData;
                                //    if (tableTypeData.Tables != null)
                                //    {
                                //        metadataTableTypeCriteria.AddRange(tableTypeData.Tables.Select(enc => MetaDataTableTypeToDatabaseCode(enc)));
                                //    }
                                //    break;
                                case TermTypes.RxSupTerm:
                                    RxSupData rxSupData = term as RxSupData;
                                    if (rxSupData != null)
                                    {
                                        rxCriteria.AddRange(rxSupData.RxSups.Select(rx => RxSupDataTypeToDatabaseCode(rx)));
                                    }
                                    break;
                                case TermTypes.RxAmtTerm:
                                    RxAmountData rxAmtData = term as RxAmountData;
                                    if (rxAmtData != null)
                                    {
                                        rxCriteria.AddRange(rxAmtData.RxAmounts.Select(rx => RxAmtDataTypeToDatabaseCode(rx)));
                                    }
                                    break;
                                default:
                                    break;

                            }
                        }
                    }
                    
                    query = "SELECT * FROM {0} WHERE " + where;
                    if (typeID == RequestTypes.DATA_CHECKER_DIAGNOSIS)
                    {
                        query = string.Format(query, "DIAGNOSES");
                    }                    
                    else if (typeID == RequestTypes.DATA_CHECKER_NDC)
                    {
                        query = string.Format(query, "NDCS");
                    }
                    else if (typeID == RequestTypes.DATA_CHECKER_PROCEDURE)
                    {
                        query = string.Format(query, "PROCEDURES");
                    }
                    else if (typeID == RequestTypes.DATA_CHECKER_ETHNICITY)
                    {
                        if (ethnicitiesCriteria.Count > 0)
                        {
                            bool hasCriterias = ethnicitiesCriteria.Where(x => x != "MISSING").Any();
                            bool hasMissingCriteria = ethnicitiesCriteria.Contains("MISSING");
                            
                            query = "SELECT dx.DP, dx.Eth AS [HISPANIC], SUM(dx.Total) AS Total FROM (";
                            if (IsSQLServerQuery)
                            {
                                query += "SELECT DP, CASE";
                                if (hasCriterias)
                                {
                                    query += " WHEN ";
                                    query += string.Join(" OR ", ethnicitiesCriteria.Where(x => x != "MISSING").Select(x => string.Format("HISPANIC = '{0}'", x)).ToArray());
                                    query += " THEN HISPANIC";
                                }
                                if (hasMissingCriteria)
                                {
                                    query += " WHEN HISPANIC IS NULL THEN 'MISSING'";
                                }

                                query += " ELSE 'OTHER' END AS Eth, SUM(n) AS Total FROM [HISPANIC] GROUP BY DP, HISPANIC";
                            }
                            else
                            {
                                //ODBC provider (Access), have to do case statement different.
                                query += "SELECT DP, Switch(";
                                if (hasCriterias)
                                {
                                    query += string.Join(", ", ethnicitiesCriteria.Where(x => x != "MISSING").Select(x => string.Format("[HISPANIC] = '{0}','{0}'", x)).ToArray()) + ",";
                                }
                                if (hasMissingCriteria)
                                {
                                    query += "IsNull([HISPANIC]) = true, 'MISSING',";
                                }
                                query += "true, 'OTHER') AS Eth, SUM(n) AS Total FROM [HISPANIC] GROUP BY [DP], [HISPANIC]";

                            }
                            query += ") dx";

                            if (!string.IsNullOrWhiteSpace(where))
                            {
                                query += " WHERE " + where;
                            }
                            query += " GROUP BY dx.DP, dx.Eth";
                        }
                        else
                        {
                            if (IsSQLServerQuery)
                            {
                                query = "SELECT [DP], COALESCE([HISPANIC], 'MISSING') AS [HISPANIC], SUM(n) AS Total FROM [HISPANIC]";
                                if (!string.IsNullOrWhiteSpace(where))
                                {
                                    query += " WHERE " + where;
                                }
                                query += " GROUP BY [DP], [HISPANIC]";
                            }
                            else
                            {
                                query = "SELECT x.[DP], IIF( ISNULL(x.[HISPANIC]), 'MISSING', x.[HISPANIC]) AS [HISPANIC], SUM(x.n) AS Total FROM [HISPANIC] x";
                                if (!string.IsNullOrWhiteSpace(where))
                                {
                                    query += " WHERE " + where;
                                }
                                query += " GROUP BY x.[DP], x.[HISPANIC]";
                            }
                            
                        }

                    }
                    else if (typeID == RequestTypes.DATA_CHECKER_RACE)
                    {
                        if (raceCritieria.Count > 0)
                        {
                            bool hasCriterias = raceCritieria.Where(x => x != 6).Any();
                            bool hasMissingCriteria = raceCritieria.Contains(6);

                            query = "SELECT dx.DP, dx.R AS [RACE], SUM(dx.Total) AS Total FROM (";
                            if (IsSQLServerQuery)
                            {
                                query += "SELECT DP, CASE";
                                if (hasCriterias)
                                {
                                    query += " WHEN ";
                                    query += string.Join(" OR ", raceCritieria.Where(x => x != 6).Select(x => string.Format("RACE = '{0}'", x)).ToArray());
                                    query += " THEN RACE";
                                }
                                if (hasMissingCriteria)
                                {
                                    query += " WHEN RACE IS NULL THEN '6'";
                                }
                                
                                query += " ELSE '-1' END AS R, SUM(n) AS Total FROM [RACE] GROUP BY DP, RACE";
                            }
                            else
                            {
                                //ODBC provider (Access), have to do case statement different. User "-1" as the "Other" value placeholder
                                query += "SELECT DP, Switch(";
                                if (hasCriterias)
                                {
                                    query += string.Join(", ", raceCritieria.Where(x => x != 6).Select(x => string.Format("[RACE] = '{0}','{0}'", x)).ToArray()) + ",";
                                }
                                if (hasMissingCriteria)
                                {
                                    query += "IsNull([RACE]) = true, '6',";
                                }
                                
                                query += "true, '-1') AS R, SUM(n) AS Total FROM [RACE] GROUP BY [DP], [RACE]";

                            }
                            query += ") dx WHERE dx.DP IS NOT NULL";

                            if (!string.IsNullOrWhiteSpace(where))
                            {
                                query += " AND " + where;
                            }
                            query += " GROUP BY dx.DP, dx.R";
                        }
                        else
                        {
                            if (IsSQLServerQuery)
                            {
                                query = "SELECT [DP], COALESCE([RACE], 'MISSING') AS [RACE], SUM(n) AS Total FROM [RACE]";
                                if (!string.IsNullOrWhiteSpace(where))
                                {
                                    query += " AND " + where;
                                }
                                query += " GROUP BY [DP], [RACE]";
                            }
                            else
                            {
                                query = "SELECT x.[DP], IIF( ISNULL(x.[RACE]), 'MISSING', x.[RACE]) AS [RACE], SUM(x.n) AS Total FROM [RACE] x";
                                if (!string.IsNullOrWhiteSpace(where))
                                {
                                    query += " WHERE " + where;
                                }
                                query += " GROUP BY x.[DP], x.[RACE]";
                            }
                            
                        }
                    }
                    else if (typeID == RequestTypes.DATA_CHECKER_DIAGNOSIS_PDX)
                    {
                        BuildDiagnosisPDXQuery(pdxCriteria, encounterTypeCriteria, where);
                    }
                    else if (typeID == RequestTypes.DATA_CHECKER_DISPENSING_RXAMT)
                    {
                        //minvalue == missing
                        //maxvalue == other and default
                        bool hasCriterias = rxCriteria.Where(x => x != Double.MinValue && x != Double.MaxValue).Any();
                        bool hasMissingCriteria = rxCriteria.Contains(Double.MinValue);

                        if (hasCriterias || hasMissingCriteria)
                        {
                            query = "SELECT rx.DP, rx.RxAmt, SUM(rx.Total) AS Total FROM (";
                            if (IsSQLServerQuery)
                            {
                                query += "SELECT r.DP, CASE";
                                if (hasMissingCriteria)
                                {
                                    query += " WHEN r.RxAmt IS NULL THEN 'MISSING'";
                                }
                                if (hasCriterias)
                                {
                                    query += " WHEN ";
                                    query += string.Join(" OR ", rxCriteria.Where(x => x != Double.MinValue && x != Double.MaxValue).Select(x => string.Format("r.RxAmt = {0}", x)).ToArray());
                                    query += " THEN CAST(r.RxAmt AS nvarchar(10))";
                                }

                                query += " ELSE 'OTHER' END AS RxAmt, SUM(r.n) AS Total FROM (";
                                //select the ranges into distinct groupings that can be used to build the aggregates
                                query += @"SELECT DP, 
		(CASE WHEN s.RxAmt < 0 THEN -1 
			  WHEN s.RxAmt >= 0 AND s.RxAmt <= 1 THEN 0
			  WHEN s.RxAmt > 1 AND s.RxAmt <= 30 THEN 30
			  WHEN s.RxAmt > 30 AND s.RxAmt <= 60 THEN 60
			  WHEN s.RxAmt > 60 AND s.RxAmt <= 90 THEN 90
			  WHEN s.RxAmt > 90 AND s.RxAmt <= 120 THEN 120
			  WHEN s.RxAmt > 120 AND s.RxAmt <= 180 THEN 180
			  WHEN s.RxAmt > 180 THEN 181
			  ELSE NULL END) AS RxAmt, s.n
		FROM RXAMT s WHERE s.DP IS NOT NULL ";
                                query += ") r GROUP BY r.DP, r.RxAmt";

                            }
                            else //ODBC Query
                            {
                                query += "SELECT r.DP, Switch(";
                                if (hasMissingCriteria)
                                {
                                    query += "r.RxAmt IS NULL, 'MISSING',";
                                }
                                if (hasCriterias)
                                {
                                    query += string.Join(",", rxCriteria.Where(x => x != Double.MinValue && x != Double.MaxValue).Select(x => string.Format("r.RxAmt = '{0}','{0}'", x)).ToArray()) + ",";
                                }
                                query += "true,'OTHER') AS RxAmt, SUM(r.n) AS Total FROM (";
                                query += "SELECT s.DP, Switch( ";
                                query += "s.RxAmt < 0, '-1', ";
                                query += "s.RxAmt >= 0 AND s.RxAmt <= 1, '0', ";
                                query += "s.RxAmt > 1 AND s.RxAmt <= 30, '30', ";
                                query += "s.RxAmt > 30 AND s.RxAmt <= 60, '60', ";
                                query += "s.RxAmt > 60 AND s.RxAmt <= 90, '90', ";
                                query += "s.RxAmt > 90 AND s.RxAmt <= 120, '120',";
                                query += "s.RxAmt > 120 AND s.RxAmt <= 180, '180',";
                                query += "s.RxAmt > 180, '181', ";
                                query += "true, NULL) AS RxAmt, s.n ";
                                query += "FROM RXAMT s WHERE s.DP IS NOT NULL ";
                                query += ") r GROUP BY r.DP, r.RxAmt ";
                            }

                            query += ") rx WHERE rx.DP IS NOT NULL";

                            if (!string.IsNullOrWhiteSpace(where))
                            {
                                query += " AND " + where;
                            }
                            query += " GROUP BY rx.DP, rx.RxAmt";
                        }
                        else
                        {
                            if (IsSQLServerQuery)
                            {
                                query = @"SELECT rx.DP, rx.RxAmt, SUM(rx.Total) AS Total FROM (
SELECT r.DP, CASE
	WHEN r.RxAmt IS NULL THEN 'MISSING'
	ELSE CAST(r.RxAmt AS nvarchar(10)) END AS RxAmt, SUM(r.n) AS Total FROM (
		SELECT DP, 
		(CASE WHEN s.RxAmt < 0 THEN -1 
			WHEN s.RxAmt >= 0 AND s.RxAmt <= 1 THEN 0
			WHEN s.RxAmt > 1 AND s.RxAmt <= 30 THEN 30
			WHEN s.RxAmt > 30 AND s.RxAmt <= 60 THEN 60
			WHEN s.RxAmt > 60 AND s.RxAmt <= 90 THEN 90
			WHEN s.RxAmt > 90 AND s.RxAmt <= 120 THEN 120
			WHEN s.RxAmt > 120 AND s.RxAmt <= 180 THEN 180
			WHEN s.RxAmt > 180 THEN 181
			ELSE NULL END) AS RxAmt, s.n
		FROM RXAMT s WHERE s.DP IS NOT NULL ) r 
		GROUP BY r.DP, r.RxAmt
) rx WHERE rx.DP IS NOT NULL";

                                if (!string.IsNullOrWhiteSpace(where))
                                {
                                    query += " AND " + where;
                                }
                                query += " GROUP BY rx.DP, rx.RxAmt";
                            }
                            else //ODBC Query
                            {
                                query = "SELECT rx.DP, rx.RxAmt, SUM(rx.Total) AS Total FROM ( ";
                                query += "SELECT r.DP, IIF( ISNULL(r.[RxAmt]), 'MISSING', r.[RxAmt]) AS [RxAmt], SUM(r.n) AS Total FROM ( ";
                                query += "SELECT s.DP, Switch(";
                                query += "s.RxAmt < 0, '-1', ";
                                query += "s.RxAmt >= 0 AND s.RxAmt <= 1, '0', ";
                                query += "s.RxAmt > 1 AND s.RxAmt <= 30, '30', ";
                                query += "s.RxAmt > 30 AND s.RxAmt <= 60, '60', ";
                                query += "s.RxAmt > 60 AND s.RxAmt <= 90, '90', ";
                                query += "s.RxAmt > 90 AND s.RxAmt <= 120, '120',";
                                query += "s.RxAmt > 120 AND s.RxAmt <= 180, '180',";
                                query += "s.RxAmt > 180, '181', ";
                                query += "true, NULL) AS RxAmt, s.n ";
                                query += "FROM RXAMT s WHERE s.DP IS NOT NULL";
                                query += " ) r GROUP BY r.DP, r.RxAmt";
                                query += " ) rx WHERE rx.DP IS NOT NULL";
                                if (!string.IsNullOrEmpty(where))
                                {
                                    query += " AND " + where;
                                }
                                query += " GROUP BY rx.DP, rx.RxAmt";
                                query += " ORDER BY rx.RxAmt, rx.DP";
                            }
                        }
                    }
                    else if (typeID == RequestTypes.DATA_CHECKER_DISPENSING_RXSUP)
                    {
                        bool hasCriterias = rxCriteria.Where(x => x != Double.MinValue && x != Double.MaxValue).Any();
                        bool hasMissingCriteria = rxCriteria.Contains(Double.MinValue);

                        if (hasCriterias || hasMissingCriteria)
                        {
                            query = "SELECT rx.DP, rx.RxSup, SUM(rx.Total) AS Total FROM (";
                            if (IsSQLServerQuery)
                            {
                                query += "SELECT r.DP, CASE";
                                if (hasMissingCriteria)
                                {
                                    query += " WHEN r.RxSup IS NULL THEN 'MISSING'";
                                }
                                if (hasCriterias)
                                {
                                    query += " WHEN ";
                                    query += string.Join(" OR ", rxCriteria.Where(x => x != Double.MinValue && x != Double.MaxValue).Select(x => string.Format("r.RxSup = {0}", x)).ToArray());
                                    query += " THEN CAST(r.RxSup AS nvarchar(10))";
                                }

                                query += " ELSE 'OTHER' END AS RxSup, SUM(r.n) AS Total FROM (";
                                //select the ranges into distinct groupings that can be used to build the aggregates
                                query += @"SELECT DP, 
		(CASE WHEN s.RxSup < 0 THEN -1 
			WHEN s.RxSup >= 0 AND s.RxSup < 2 THEN 0
			WHEN s.RxSup >= 2 AND s.RxSup <= 30 THEN 2
			WHEN s.RxSup > 30 AND s.RxSup <= 60 THEN 30
			WHEN s.RxSup > 60 AND s.RxSup <= 90 THEN 60
			WHEN s.RxSup > 90 THEN 90
			ELSE NULL END) AS RxSup, s.n
		FROM RXSUP s WHERE s.DP IS NOT NULL ";                                
                                query += ") r GROUP BY r.DP, r.RxSup";

                            }
                            else //ODBC Query
                            {
                                query += "SELECT r.DP, Switch(";
                                if (hasMissingCriteria)
                                {
                                    query += " r.RxSup IS NULL, 'MISSING', ";
                                }
                                if (hasCriterias)
                                {
                                    query += string.Join(", ", rxCriteria.Where(x => x != Double.MinValue && x != Double.MaxValue).Select(x => string.Format(" r.RxSup = '{0}', '{0}'", x)).ToArray()) + ", ";
                                }
                                query += " true, 'OTHER') AS RxSup, SUM(r.n) AS Total FROM (";
                                query += "SELECT s.DP, Switch( ";
                                query += "s.RxSup < 0, '-1', ";
                                query += "s.RxSup >= 0 AND s.RxSup < 2, '0', ";
                                query += "s.RxSup >= 2 AND s.RxSup <= 30, '2', ";
                                query += "s.RxSup > 30 AND s.RxSup <= 60, '30', ";
                                query += "s.RxSup > 60 AND s.RxSup <= 90, '60', ";
                                query += "s.RxSup > 90, '90', ";
                                query += "true, NULL) AS RxSup, s.n ";
                                query += "FROM RXSUP s WHERE s.DP IS NOT NULL ";
                                query += ") r GROUP BY r.DP, r.RxSup ";
                            }

                            query += ") rx WHERE rx.DP IS NOT NULL";

                            if (!string.IsNullOrWhiteSpace(where))
                            {
                                query += " AND " + where;
                            }
                            query += " GROUP BY rx.DP, rx.RxSup";
                        }
                        else
                        {
                            if (IsSQLServerQuery)
                            {
                                query = @"SELECT rx.DP, rx.RxSup, SUM(rx.Total) AS Total FROM (
SELECT r.DP, CASE
	WHEN r.RxSup IS NULL THEN 'MISSING'
	ELSE CAST(r.RxSup AS nvarchar(10)) END AS RxSup, SUM(r.n) AS Total FROM (
		SELECT DP, 
		(CASE WHEN s.RxSup < 0 THEN -1 
			WHEN s.RxSup >= 0 AND s.RxSup < 2 THEN 0
			WHEN s.RxSup >= 2 AND s.RxSup <= 30 THEN 2
			WHEN s.RxSup > 30 AND s.RxSup <= 60 THEN 30
			WHEN s.RxSup > 60 AND s.RxSup <= 90 THEN 60
			WHEN s.RxSup > 90 THEN 90
			ELSE NULL END) AS RxSup, s.n
		FROM RXSUP s WHERE s.DP IS NOT NULL ) r 
		GROUP BY r.DP, r.RxSup
) rx WHERE rx.DP IS NOT NULL";

                                if (!string.IsNullOrWhiteSpace(where))
                                {
                                    query += " AND " + where;
                                }
                                query += " GROUP BY rx.DP, rx.RxSup";
                            }
                            else //ODBC Query
                            {
                                query = "SELECT rx.DP, rx.RxSup, SUM(rx.Total) AS Total FROM ( ";
                                query += "SELECT r.DP, IIF( ISNULL(r.[RxSup]), 'MISSING', r.[RxSup]) AS [RxSup], SUM(r.n) AS Total FROM ( ";
                                query += "SELECT s.DP, Switch(";
                                query += "s.RxSup < 0, '-1', ";
                                query += "s.RxSup >= 0 AND s.RxSup < 2, '0', ";
                                query += "s.RxSup >= 2 AND s.RxSup <= 30, '2', ";
                                query += "s.RxSup > 30 AND s.RxSup <= 60, '30', ";
                                query += "s.RxSup > 60 AND s.RxSup <= 90, '60', ";
                                query += "s.RxSup > 90, '90', ";
                                query += "true, NULL) AS RxSup, s.n ";
                                query += "FROM RXSUP s WHERE s.DP IS NOT NULL";
                                query += " ) r GROUP BY r.DP, r.RxSup";
                                query += " ) rx WHERE rx.DP IS NOT NULL";
                                if (!string.IsNullOrEmpty(where))
                                {
                                    query += " AND " + where;
                                }
                                query += " GROUP BY rx.DP, rx.RxSup";
                                query += " ORDER BY rx.RxSup, rx.DP";
                            }
                        }

                    }
                    else if (typeID == RequestTypes.DATA_CHECKER_METADATA_COMPLETENESS)
                    {
                        query = string.Format(query, "METADATA");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid request type ID, does not match any expected types!");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message, ex);
                status.Code = RequestStatus.StatusCode.Error;
                status.Message = ex.Message;
                throw ex;
            } 
        }

        void BuildDiagnosisPDXQuery(IEnumerable<string> pdxCriteria, IEnumerable<string> encounterTypeCriteria, string where)
        {
            bool hasPDXCriteria = pdxCriteria.Where(x => x != "MISSING").Any();
            bool hasEncounterCriteria = encounterTypeCriteria.Where(x => x != "MISSING" && x != "ALL").Any();
            bool hasAllEncountersCriteria = encounterTypeCriteria.Contains("ALL");
            bool hasMissingPDX = pdxCriteria.Contains("MISSING");
            bool hasMissingEncounters = encounterTypeCriteria.Contains("MISSING");
            StringBuilder sb = new StringBuilder();

            if (hasPDXCriteria || hasEncounterCriteria || hasAllEncountersCriteria || hasMissingEncounters || hasMissingPDX)
            {
                bool sqlServer = IsSQLServerQuery;
                if (sqlServer)
                {
                    sb.AppendLine("SELECT pp.DP, pp.PDX, pp.EncType, SUM(n) as n FROM (");
                    sb.AppendLine("	SELECT p.DP,");
                    if (hasPDXCriteria || hasMissingPDX)
                    {
                        sb.Append("CASE ");
                        if (hasMissingPDX)
                        {
                            sb.AppendLine("WHEN p.[PDX] IS NULL THEN 'MISSING'");
                        }
                        if (hasPDXCriteria)
                        {                            
                            sb.AppendLine("WHEN " + string.Join(" OR ", pdxCriteria.Where(pdx => pdx != "MISSING" ).Select(pdx => string.Format("p.[PDX] = '{0}'", pdx)) ) + " THEN p.[PDX] ");
                        }
                        sb.AppendLine("ELSE 'OTHER' END AS PDX,");
                    }else{
                        sb.AppendLine("COALESCE(p.[PDX], 'MISSING') AS [PDX],");
                    }

                    if (hasEncounterCriteria || hasMissingEncounters)
                    {
                        sb.Append("CASE ");
                        if (hasMissingEncounters)
                        {
                            sb.AppendLine("WHEN p.[EncType] IS NULL THEN 'MISSING'");
                        }
                        if (hasEncounterCriteria)
                        {
                            sb.AppendLine("WHEN " + string.Join(" OR ", encounterTypeCriteria.Where(x => x != "MISSING" && x != "ALL").Select(x => string.Format("p.EncType = '{0}'", x))) + " THEN EncType ");
                        }
                        sb.AppendLine("ELSE 'OTHER' END AS EncType,");
                    }
                    else
                    {
                        sb.AppendLine("COALESCE(p.EncType, 'MISSING') AS EncType,");
                    }

                    sb.AppendLine("n FROM [PDX] p");
                    sb.AppendLine(") pp WHERE pp.DP IS NOT NULL");

                    if (!string.IsNullOrEmpty(where))
                    {
                        sb.AppendLine("AND " + where);
                    }
                    sb.AppendLine("GROUP BY pp.DP, pp.PDX, pp.EncType");

                    if (hasAllEncountersCriteria && hasEncounterCriteria)
                    {
                        sb.AppendLine("UNION");
                        sb.AppendLine("SELECT pp.DP,");

                        if (hasPDXCriteria || hasMissingPDX)
                        {
                            sb.AppendLine("CASE WHEN pp.[PDX] IS NULL THEN 'MISSING'");
                            if (hasPDXCriteria)
                            {
                                sb.AppendLine("WHEN " + string.Join(" OR ", pdxCriteria.Where(pdx => pdx != "MISSING").Select(pdx => string.Format("pp.[PDX] = '{0}'", pdx))) + " THEN pp.[PDX] ");
                            }
                            sb.AppendLine("ELSE 'OTHER' END AS PDX,");
                        }
                        sb.AppendLine("'ALL' AS EncType,");
                        sb.AppendLine("SUM(n) AS n");
                        sb.AppendLine("FROM PDX pp WHERE pp.DP IS NOT NULL");
                        sb.AppendLine("AND pp.EncType IN (" + string.Join(", ", encounterTypeCriteria.Where(x => x != "MISSING" && x != "ALL").Select(x => string.Format("'{0}'", x)).ToArray()) + ")");
                        if (!string.IsNullOrEmpty(where))
                        {
                            sb.AppendLine("AND " + where);
                        }
                        sb.AppendLine("GROUP BY pp.DP, pp.PDX");
                    }
                }
                else
                {
                    //ODBC Query

                    sb.AppendLine("SELECT pp.DP, pp.PDX, pp.EncType, SUM(pp.n) as n FROM (");
                    sb.AppendLine("	SELECT p.DP,");
                    if (hasPDXCriteria || hasMissingPDX)
                    {
                        sb.Append("Switch(");
                        if (hasMissingPDX)
                        {
                            sb.Append("p.[PDX] IS NULL, 'MISSING',");
                        }
                        if (hasPDXCriteria)
                        {
                            sb.Append(string.Join(",", pdxCriteria.Where(pdx => pdx != "MISSING").Select(pdx => string.Format("p.[PDX] = '{0}','{0}'", pdx))));
                            sb.Append(",");
                        }
                        sb.AppendLine("true,'OTHER') AS PDX,");

                    }else{
                        sb.AppendLine("IIF( ISNULL(p.[PDX]), 'MISSING', p.[PDX]) AS [PDX],");
                    }

                    if (hasEncounterCriteria || hasMissingEncounters)
                    {
                        sb.Append("Switch(");
                        if (hasMissingEncounters)
                        {
                            sb.Append("p.[EncType] IS NULL,'MISSING',");
                        }
                        if (hasEncounterCriteria)
                        {
                            sb.Append(string.Join(", ", encounterTypeCriteria.Where(x => x != "MISSING" && x != "ALL").Select(x => string.Format("p.EncType = '{0}','{0}'", x))));
                            sb.Append(",");
                        }
                        sb.AppendLine("true,'OTHER') AS EncType,");
                    }
                    else
                    {
                        sb.AppendLine("IIF( ISNULL(p.[EncType]), 'MISSING', p.[EncType]) AS [EncType],");

                    }

                    sb.AppendLine("p.n FROM [PDX] p");
                    sb.AppendLine(") pp WHERE pp.DP IS NOT NULL");

                    if (!string.IsNullOrEmpty(where))
                    {
                        sb.AppendLine("AND " + where);
                    }
                    sb.AppendLine("GROUP BY pp.DP, pp.PDX, pp.EncType");

                    if (hasAllEncountersCriteria && hasEncounterCriteria)
                    {
                        sb.AppendLine("UNION");
                        sb.AppendLine("SELECT pp.DP,");

                        if (hasPDXCriteria || hasMissingPDX)
                        {
                            sb.Append("Switch(pp.[PDX] IS NULL,'MISSING',");
                            if (hasPDXCriteria)
                            {
                                sb.Append(string.Join(",", pdxCriteria.Where(pdx => pdx != "MISSING").Select(pdx => string.Format("pp.[PDX] = '{0}','{0}'", pdx))));
                            }
                            sb.AppendLine(",true,'OTHER') AS PDX,");
                        }
                        sb.AppendLine("'ALL' AS EncType,");
                        sb.AppendLine("SUM(pp.n) AS n");
                        sb.AppendLine("FROM PDX pp WHERE pp.DP IS NOT NULL");
                        sb.AppendLine("AND pp.EncType IN (" + string.Join(", ", encounterTypeCriteria.Where(x => x != "MISSING" && x != "ALL").Select(x => string.Format("'{0}'", x)).ToArray()) + ")");
                        if (!string.IsNullOrEmpty(where))
                        {
                            sb.AppendLine("AND " + where);
                        }
                        sb.AppendLine("GROUP BY pp.DP, pp.PDX");
                    }
                }
            }
            else
            {
                if (IsSQLServerQuery)
                {
                    sb.AppendLine("SELECT pp.DP, pp.PDX, pp.EncType, SUM(n) as n FROM (");
                    sb.AppendLine("SELECT DP, COALESCE([PDX], 'MISSING') AS [PDX], COALESCE(EncType, 'MISSING') AS EncType, n FROM [PDX] p");
                    sb.AppendLine(") pp WHERE pp.DP IS NOT NULL");
                    if (!string.IsNullOrEmpty(where))
                    {
                        sb.Append(" AND ");
                        sb.Append(where);
                    }
                    sb.AppendLine("GROUP BY pp.DP, pp.PDX, pp.EncType");
                }
                else
                {
                    sb.AppendLine("SELECT pp.DP, pp.PDX, pp.EncType, SUM(pp.n) as n FROM (");
                    sb.AppendLine("SELECT p.DP, IIF( ISNULL(p.[PDX]), 'MISSING', p.[PDX]) AS [PDX], IIF( ISNULL(p.EncType), 'MISSING', p.EncType) AS EncType, p.n FROM [PDX] p");
                    sb.AppendLine(") pp WHERE pp.DP IS NOT NULL");
                    if (!string.IsNullOrEmpty(where))
                    {
                        sb.Append(" AND ");
                        sb.Append(where);
                    }
                    sb.AppendLine("GROUP BY pp.DP, pp.PDX, pp.EncType");
                }
            }

            query = sb.ToString();
        }

        bool IsSQLServerQuery
        {
            get
            {
                return (Lpp.Dns.DataMart.Model.Settings.SQLProvider)Enum.Parse(typeof(Lpp.Dns.DataMart.Model.Settings.SQLProvider), Settings.GetAsString("DataProvider", ""), true) == Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer;
            }
        }

        private string MetaDataTableTypeToDatabaseCode(MetaDataTableTypes type)
        {     
            //This likely isn't needed.
            switch (type)
            {
                case MetaDataTableTypes.Diagnosis:
                    return "DIA";
                case MetaDataTableTypes.Dispensing:
                    return "DIS";
                case MetaDataTableTypes.Encounter:
                    return "ENC";
                case MetaDataTableTypes.Enrollment:
                    return "ENR";
                case MetaDataTableTypes.Procedure:
                    return "PRO";
                default:

                    throw new NotImplementedException("The Meta Data Table Type was not recognized");
            }
        }

        private string EncounterDataTypeToDatabaseCode(EncounterTypes Type)
        {
            string code;
            switch (Type)
            {
                case EncounterTypes.Missing:
                    code = "MISSING"; // represents the value is null in the db
                    break;
                case EncounterTypes.All://represents a sum of all defined encounter types - ie ignoring missing
                    code = "ALL";
                    break;
                case EncounterTypes.AmbulatoryVisit:
                    code = "AV";
                    break;
                case EncounterTypes.EmergencyDepartment:
                    code = "ED";
                    break;
                case EncounterTypes.InpatientHospitalStay:
                    code = "IP";
                    break;
                case EncounterTypes.NonAcuteInstitutionalStay:
                    code = "IS";
                    break;
                case EncounterTypes.OtherAmbulatoryVisit:
                    code = "OA";
                    break;
                default:
                    code = "U"; // Encounter Type does not have an "Unknown" code, so "U" does not exist in the DB. It must be handled with special-case logic in RequestDocument().
                    break;
            }
            return code;
        }

        private string EthnicityDataTypeToDatabaseCode(EthnicityTypes Type)
        {
            string code;
            switch (Type)
            {
                case EthnicityTypes.Missing:
                    code = "MISSING";
                    break;
                case EthnicityTypes.Hispanic:
                    code = "Y";
                    break;
                case EthnicityTypes.NotHispanic:
                    code = "N";
                    break;
                case EthnicityTypes.Unknown:
                default:
                    code = "U";
                    break;
            }
            return code;
        }

        private string PDXDataTypeToDatabaseCode(PDXTypes Type)
        {
            string code;
            switch (Type)
            {
                case PDXTypes.Missing:
                    code = "MISSING"; // This value does not exist in the DB. It must be handled with special-case logic in RequestDocument().
                    break;
                case PDXTypes.Principle:
                    code = "P";
                    break;
                case PDXTypes.Secondary:
                    code = "S";
                    break;
                case PDXTypes.Other:
                    code = "X";
                    break;
                default:
                    code = "U"; // PDX does not have an "Unknown" code, so "U" does not exist in the DB. It must be handled with special-case logic in RequestDocument().
                    break;
            }
            return code;
        }

        private int RaceDataTypeToDatabaseCode(RaceTypes Type)
        {
            int code;
            switch (Type)
            {
                case RaceTypes.Unknown:
                    code = 0;
                    break;
                case RaceTypes.AmericanIndianOrAlaskaNative:
                    code = 1;
                    break;
                case RaceTypes.Asian:
                    code = 2;
                    break;
                case RaceTypes.BlackOrAfricanAmerican:
                    code = 3;
                    break;
                case RaceTypes.NativeHawaiianOrOtherPacificIslander:
                    code = 4;
                    break;
                case RaceTypes.White:
                    code = 5;
                    break;
                case RaceTypes.Missing:
                    code = 6;
                    break;
                default:
                    code = 0;
                    break;
            }
            return code;
        }

        private double RxAmtDataTypeToDatabaseCode(RxAmountTypes Type)
        {
            double code;
            switch (Type)
            {
                case RxAmountTypes.Missing:
                    code = Double.MinValue; // This value does not exist in the DB. It must be handled with special-case logic in RequestDocument().
                    break;
                case RxAmountTypes.LessThanZero:
                    code = -1; // x < 0
                    break;
                case RxAmountTypes.Zero:
                    code = 0; //  x >= 0 && x <= 1
                    break;
                case RxAmountTypes.TwoThroughThirty:
                    code = 30;// x > 1 && x <= 30
                    break;
                case RxAmountTypes.Thirty:
                    code = 60;// x > 30 && x <= 60
                    break;
                case RxAmountTypes.Sixty:
                    code = 90;// x > 60 && x <= 90
                    break;
                case RxAmountTypes.Ninety:
                    code = 120;// x > 90 && x <= 120
                    break;
                case RxAmountTypes.OneHundredTwenty:
                    code = 180;// x > 120 && x <= 180
                    break;
                case RxAmountTypes.OneHundredEighty:
                    code = 181;// x > 180
                    break;
                case RxAmountTypes.Other:
                default:
                    code = Double.MaxValue; // This value does not exist in the DB. It must be handled with special-case logic in RequestDocument().
                    break;
            }
            return code;
        }

        private double RxSupDataTypeToDatabaseCode(RxSupTypes Type)
        {
            double code;
            switch (Type)
            {
                case RxSupTypes.LessThanZero:
                    code = -1;// < 0
                    break;
                case RxSupTypes.Zero:
                    code = 0;//  x >= 0 && x < 2
                    break;
                case RxSupTypes.TwoThroughThirty:
                    code = 2;// x >= 2 && x < 30
                    break;
                case RxSupTypes.Thirty:
                    code = 30;// x >= 30 && x < 60
                    break;
                case RxSupTypes.Sixty:
                    code = 60;// x >= 60 && x < 90
                    break;
                case RxSupTypes.Ninety:
                    code = 90;// x >= 90
                    break;
                case RxSupTypes.Missing:
                    code = Double.MinValue;
                    break;
                case RxSupTypes.Other:// <= based on email that Melanie sent May 8, 2014 indicating the ranges and the fact the db column is a float/number this doesn't really even apply anymore
                default:
                    code = Double.MaxValue; // This value value exists in the db but is not part of the defined set of numbers and should be categorized as 'OtherRxSup'
                    break;
            }
            return code;
        }

        private string BuildLikeList<t>(string where, string columnName, IEnumerable<t> items)
        {
            string list = string.Empty;
            items.ToList<t>().ForEach(s => list += (list != string.Empty ? " OR " : "") + columnName + " LIKE '" + ((t)s).ToString().Trim() + "%'");
            return (where != string.Empty ? " and " : " ") + "( " + list + " )";
        }

        private string BuildInList<t>(string where, string columnName, IEnumerable<t> items)
        {
            return (where != string.Empty ? " and " : " ") + columnName + " IN ( " + BuildList<t>(items) + " ) ";
        }

        private string BuildList<t>(IEnumerable<t> items)
        {
            string list = string.Empty;
            items.ToList().ForEach(r => list += (list != string.Empty ? "," : "") + "'" + ((t)r).ToString().Trim() + "'");
            return list;
        }

        public void Start(string requestId, bool viewSQL)
        {
            try
            {
                if (Settings == null || Settings.Count == 0)
                    throw new Exception(CommonMessages.Exception_MissingSettings);

                status.Code = RequestStatus.StatusCode.InProgress;
                string server = Settings.GetAsString("Server", "");
                string port = Settings.GetAsString("Port", "");
                string userId = Settings.GetAsString("UserID", "");
                string password = Settings.GetAsString("Password", "");
                string database = Settings.GetAsString("Database", "");
                string dataSourceName = Settings.GetAsString("DataSourceName", "");
                string connectionTimeout = Settings.GetAsString("ConnectionTimeout", "15");
                string commandTimeout = Settings.GetAsString("CommandTimeout", "120");              

                log.Info("Connection timeout: " + connectionTimeout + ", Command timeout: " + commandTimeout);
                log.Info("Query: " + query);

                if (!Settings.ContainsKey("DataProvider"))
                    throw new Exception(CommonMessages.Exception_MissingDataProviderType);

                string connectionString = string.Empty;
                switch ((Lpp.Dns.DataMart.Model.Settings.SQLProvider)Enum.Parse(typeof(Lpp.Dns.DataMart.Model.Settings.SQLProvider), Settings.GetAsString("DataProvider", ""), true))
                {
                    case Lpp.Dns.DataMart.Model.Settings.SQLProvider.ODBC:
                        if (string.IsNullOrEmpty(dataSourceName))
                        {
                            throw new Exception(CommonMessages.Exception_MissingODBCDatasourceName);
                        }

                        using (OdbcConnection cn = new OdbcConnection(string.Format("DSN={0}", dataSourceName)))
                        try
                        {
                            OdbcDataAdapter da = new OdbcDataAdapter(query, cn);
                            da.Fill(resultDataset);
                        }
                        finally
                        {
                            cn.Close();
                        }
                        hasCellCountAlert = CheckCellCountsInQueryResult(resultDataset, CellThreshold);                        
                        break;

                    case Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer:
                        if (string.IsNullOrEmpty(server))
                        {
                            throw new Exception(CommonMessages.Exception_MissingDatabaseServer);
                        }
                        if (string.IsNullOrEmpty(database))
                        {
                            throw new Exception(CommonMessages.Exception_MissingDatabaseName);
                        }
                        if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(password))
                        {
                            throw new Exception(CommonMessages.Exception_MissingDatabasePassword);
                        }

                        if (port != null && port != string.Empty) server += ", " + port;
                        connectionString = userId != null && userId != string.Empty ? String.Format("server={0};User ID={1};Password={2};Database={3}; Connection Timeout={4}", server, userId, password, database, connectionTimeout) :
                                                                                        String.Format("server={0};integrated security=True;Database={1}; Connection Timeout={2}", server, database, connectionTimeout);
                        using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                                command.CommandTimeout = int.Parse(commandTimeout);
                                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(command);
                                resultDataset.Reset();
                                da.Fill(resultDataset);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        hasCellCountAlert = CheckCellCountsInQueryResult(resultDataset, CellThreshold);
                        break;
                    default:
                        throw new Exception(CommonMessages.Exception_InvalidDataProviderType);
                }
                if (hasCellCountAlert)
                {
                    status.Code = RequestStatus.StatusCode.CompleteWithMessage;
                    status.Message = "The query results have rows with low cell counts. You can choose to set the low cell count data to 0 by clicking the [Suppres Low Cells] button.";
                    status.PostProcess = true;
                }
                else
                {
                    status.Code = RequestStatus.StatusCode.Complete;
                    status.Message = "";
                }
            }
            catch (Exception e)
            {
                status.Code = RequestStatus.StatusCode.Error;
                status.Message = e.Message;
                throw e;
            }

        }

        public void Stop(string requestId, StopReason reason)
        {
        }

        public RequestStatus Status(string requestId)
        {
            return status;
        }

        public Document[] Response(string requestId)
        {
            BuildResponseDocuments();
            return responseDocument;
        }

        public void AddResponseDocument(string requestId, string filePath)
        {
            BuildResponseDocuments();
            string mimeType = GetMimeType(filePath);
            Document document = new Document(responseDocument.Length.ToString(), mimeType, filePath);
            IList<Document> responseDocumentList = responseDocument.ToList<Document>();
            responseDocumentList.Add(document);
            responseDocument = responseDocumentList.ToArray<Document>();
        }


        public void RemoveResponseDocument(string requestId, string documentId)
        {
            if (responseDocument != null && responseDocument.Length > 0)
            {
                IList<Document> responseDocumentList = responseDocument.ToList<Document>();
                foreach (Document doc in responseDocument)
                {
                    if (doc.DocumentID == documentId)
                        responseDocumentList.Remove(doc);
                }
                responseDocument = responseDocumentList.ToArray<Document>();
            }
        }

        public void ResponseDocument(string requestId, string documentId, out Stream contentStream, int maxSize)
        {
            contentStream = null;

            if (documentId == "0")
            {
                contentStream = new MemoryStream();
                resultDataset.WriteXml(contentStream, XmlWriteMode.WriteSchema);
                contentStream.Flush();
                contentStream.Position = 0;
            }
            else if (documentId == "1" && hasCellCountAlert)
            {
                contentStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(styleXml));
            }
        }


        public void Close(string requestId)
        {
        }

        public void PostProcess(string requestId)
        {
            ZeroLowCellCountsInQueryResult(resultDataset.Tables[0], CellThreshold);
            status.Code = RequestStatus.StatusCode.Complete;
            status.Message = "";
            status.PostProcess = false;
        }

        public static Boolean CheckCellCountsInQueryResult(DataSet ds, int ThresholdCount)
        {
            Boolean showAlert = false;
            DataTable dt = ds.Tables[0];
            if (dt.Columns.Contains("n"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string cellValue = dr["n"].ToString();
                    double Num;
                    bool isNum = double.TryParse(cellValue, out Num);
                    if (isNum && Num > 0 && Num < Convert.ToDouble(ThresholdCount))
                    {
                        showAlert = true;
                        break;
                    }
                }
            }
            return showAlert;
        }

        public static void ZeroLowCellCountsInQueryResult(DataTable queryResult, int ThresholdCount)
        {
            DataTable dt = queryResult;
            if (dt.Columns.Contains("n"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string cellValue = dr["n"].ToString();
                    double Num;
                    bool isNum = double.TryParse(cellValue, out Num);
                    if (isNum)
                    {
                        double dValue = Convert.ToDouble(dr["n"]);
                        if (dValue > 0 && dValue < Convert.ToDouble(ThresholdCount))
                        {
                            dr["n"] = 0;
                        }
                    }
                }
            }
        }

        public static int[] FindLowCellCountsInQueryResult(DataTable queryResult, int ThresholdCount)
        {
            DataTable dt = queryResult;
            List<int> zeroRows = new List<int>();

            if (dt.Columns.Contains("n"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string cellValue = dr["n"].ToString();
                    double Num;
                    bool isNum = double.TryParse(cellValue, out Num);
                    if (isNum && Num > 0 && Num < Convert.ToDouble(ThresholdCount))
                    {
                        zeroRows.Add(dt.Rows.IndexOf(dr));
                    }
                }
            }
            return zeroRows.ToArray();
        }

        #endregion

        #region Private Methods

        private void BuildResponseDocuments()
        {
                if (resultDataset != null && responseDocument == null)
                {
                    responseDocument = new Document[hasCellCountAlert ? 2 : 1];
                    responseDocument[0] = new Document("0", "x-application/lpp-dns-table", "DataCheckerResponse.xml");
                    responseDocument[0].IsViewable = true;

                    using (MemoryStream stream = new MemoryStream())
                    {
                        // Embed the schema. The schema contains column header information in case no rows are returned.
                        resultDataset.WriteXml(stream, XmlWriteMode.WriteSchema);
                        stream.Flush();
                        responseDocument[0].Size = Convert.ToInt32(stream.Length);
                    }
                    if (hasCellCountAlert)
                    {
                        ViewableDocumentStyle style = new ViewableDocumentStyle
                        {
                            Css = ".StyledRows { background-color: yellow; color: red }",
                            StyledRows = FindLowCellCountsInQueryResult(resultDataset.Tables[0], CellThreshold)
                        };

                        XmlSerializer serializer = new XmlSerializer(typeof(ViewableDocumentStyle));
                        using (StringWriter sw = new StringWriter())
                        {
                            using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
                            {
                                serializer.Serialize(xmlWriter, style, null);
                                styleXml = sw.ToString();
                            }
                        }

                        responseDocument[1] = new Document("1", "application/xml", "ViewableDocumentStyle.xml");
                        responseDocument[1].IsViewable = false;
                        responseDocument[1].Size = styleXml.Length;
                    }
            }   
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        #endregion


    }

}
