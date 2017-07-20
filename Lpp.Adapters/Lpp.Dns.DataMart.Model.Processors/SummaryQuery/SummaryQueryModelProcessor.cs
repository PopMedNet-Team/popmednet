using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class SummaryQueryModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private string PROCESSOR_ID = Guid.Empty.ToString();
        private const string PROCESSOR_ID = "cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb";
        private Guid PREV_REFRESH_DATES_ID = Guid.Parse("58190001-7DE2-4CC0-9D68-A22200F85BAF");
        private Guid INCIDENT_REFRESH_DATES_ID = Guid.Parse("55290001-A764-482B-A4AB-A22200F8D293");
        private Guid REFRESH_DATES_ID = Guid.Parse("70DD0001-ABFC-4D71-91F4-A22200F9A30F");

        private IModelMetadata modelMetadata = new SummaryQueryModelMetadata();
        private Document[] responseDocument;

        [Serializable]
        internal class SummaryQueryModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "4c99fa21-cdea-4b09-b95b-eebdda05adea";

            readonly Settings.SQLProvider[] sqlProviders;
            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public SummaryQueryModelMetadata()
            {
                sqlProviders = new[] { Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer, Lpp.Dns.DataMart.Model.Settings.SQLProvider.ODBC };

                capabilities = new Dictionary<string, bool>() { { "IsSingleton", false }, { "CanViewSQL", true } }; 
                
                properties = new Dictionary<string, string>();
                Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
                properties.Add("ThreshHoldCellCount", "0");                
            }

            public string ModelName
            {
                get { return "SummaryQueryModelProcessor"; }
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

        private bool IsMetadataRequest { get; set; }
        private Guid requestTypeId = Guid.Empty;
        private IList<DataSet> datasets;
        private string[] queries;
        private string argsXml = null;
        private RequestStatus status = new RequestStatus();
        private DataSet viewableResultDataset;
        private DataSet metadataResultDataset;
        private string styleXml;
        private bool hasCellCountAlert = false;
        private Document[] desiredDocuments;

        private int CellThreshHold
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
                catch
                {
                }

                return val;
            }
        }

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
        }

        public void Request( string requestId, NetworkConnectionMetadata network, RequestMetadata md, 
            Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments )
        {
            viewableResultDataset = new DataSet();
            datasets = new List<DataSet>();
            this.requestTypeId = new Guid(md.RequestTypeId);
            IsMetadataRequest = md.IsMetadataRequest;
            requestProperties = null;
            desiredDocuments = requestDocuments;
            this.desiredDocuments = requestDocuments;
            status.Code = RequestStatus.StatusCode.InProgress;
            status.Message = "";
        }

        public void RequestDocument( string requestId, string documentId, Stream contentStream )
        {
            if (Settings == null || Settings.Count == 0)
                throw new Exception(CommonMessages.Exception_MissingSettings);

            if (!Settings.ContainsKey("DataProvider"))
                throw new Exception(CommonMessages.Exception_MissingDataProviderType);

            var doc = desiredDocuments.FirstOrDefault( d => d.DocumentID == documentId );

            if (string.Equals(doc.Filename, "SummaryRequestArgs.xml", StringComparison.InvariantCultureIgnoreCase))
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    argsXml = reader.ReadToEnd();
                    log.Debug("Arguments to SQL Query");
                    log.Debug(argsXml);
                    Lpp.Dns.DataMart.Model.Settings.SQLProvider provider = (Model.Settings.SQLProvider)Enum.Parse(typeof(Model.Settings.SQLProvider), Settings.GetAsString("DataProvider", ""), true);
                    queries = SummaryQueryBuilder.Instance.BuildSQLQueries(requestTypeId, argsXml, IsMetadataRequest, provider);
                }
            }
        }

        public void Start(string requestId, bool viewSQL)
        {
            try
            {
                log.Debug("SummaryQueryModelProcessor:Start: RequestId=" + requestId + ", viewSQL=" + viewSQL.ToString());
                if (Settings.Count == 0)
                    throw new Exception(CommonMessages.Exception_MissingSettings);

                status.Code = RequestStatus.StatusCode.InProgress;
                //styleXml = null;
                DataSet combinedDataset = new DataSet();
                DataTable dtSQL = new DataTable();
                dtSQL.Columns.Add("SQL");
                foreach (string query in queries)
                {                    
                    if (!string.IsNullOrEmpty(query.Trim()))
                    {
                        if (!viewSQL)
                        {
                            var ds = ExecuteQuery(query);
                            var table = ds.Tables[0];
                            combinedDataset.Merge(table);
                        }
                        else
                        {
                            dtSQL.Rows.Add(query);
                        }
                    }
                }
                if (viewSQL)
                    combinedDataset.Merge(dtSQL);

                hasCellCountAlert = SummaryQueryUtil.CheckCellCountsInQueryResult(combinedDataset, CellThreshHold);

                DataTable datatable;

                if (IsMetadataRequest)
                {
                    //datatable = combinedDataset.Tables[0];
                    datatable = AugmentAdminQuery(combinedDataset);
                    metadataResultDataset = combinedDataset;
                }
                else
                {
                    //datatable = AugmentSummaryQuery(combinedDataset);
                    datatable = combinedDataset.Tables[0].Copy();
                    metadataResultDataset = null;
                }

                // Log the DataSet result.
                MemoryStream s = new MemoryStream();
                combinedDataset.WriteXml(s, XmlWriteMode.WriteSchema);
                s.Seek(0, SeekOrigin.Begin);
                string x = new StreamReader(s).ReadToEnd();
                // Can never leave this code enabled with a production build
                //log.Debug("Result of SQL Query");
                //log.Debug(x);

                viewableResultDataset.Tables.Add(datatable);

                if (hasCellCountAlert)
                {
                    status.Code = RequestStatus.StatusCode.CompleteWithMessage;
                    status.Message = "The query results have rows with low cell counts. You can choose to set the low cell count data to 0 by clicking the [Post Process] button.";
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
            if (documentId == "0") // Viewable result
            {
                contentStream = CreateMemoryStreamForDataSet(viewableResultDataset);
                return;
            }

            if (documentId == "1")
            {
                if (hasCellCountAlert)
                    contentStream = new MemoryStream(Encoding.UTF8.GetBytes(styleXml));
                else if (IsMetadataRequest)
                    contentStream = CreateMemoryStreamForDataSet(metadataResultDataset);

                return;
            }

            contentStream = new FileStream( responseDocument[Convert.ToInt32( documentId )].Filename, FileMode.Open );
        }

        private Stream CreateMemoryStreamForDataSet(DataSet ds)
        {
            // Embed the schema. The schema contains column header information in case no rows are returned.
            var stream = new MemoryStream();
            ds.WriteXml(stream, XmlWriteMode.WriteSchema);
            stream.Seek(0, SeekOrigin.Begin);
            return stream; 
        }

        public void Close(string requestId)
        {
        }

        /// <summary>
        /// This method will be called in SummaryQuery only the zero-ing threshold is greater than 0,
        /// a status message is generated, and the user wants to zero out results under the threshold.
        /// </summary>
        /// <param name="requestId"></param>
        public void PostProcess(string requestId)
        {
            SummaryQueryUtil.ZeroLowCellCountsInQueryResult(viewableResultDataset.Tables[0], CellThreshHold);
            status.Code = RequestStatus.StatusCode.Complete;
            status.Message = ""; 
            status.PostProcess = false;
        }

        #endregion

        #region Private Methods

        private const string QUERY_TYPE_ID = "QueryTypeId";
        private const string QUERY_TYPE = "Query Type";
        private const string START_YEAR = "Data Availability Start Year";
        private const string END_YEAR = "Data Availability End Year";
        private const string START_QUARTER = "Data Availability Start Quarter";
        private const string END_QUARTER = "Data Availability End Quarter";
        private const string ANNUAL = "Data Availability (Annual)";
        private const string QUARTERLY = "Data Availability (Quarterly)";

        private DataTable AugmentAdminQuery(DataSet dataset)
        {
            DataTable _dataTable = new DataTable();
            DataColumn _dColumn = new DataColumn();

            _dataTable.Columns.Add(QUERY_TYPE_ID);
            _dataTable.Columns.Add(QUERY_TYPE);
            _dataTable.Columns.Add(ANNUAL);
            _dataTable.Columns.Add(QUARTERLY);
            //_dataTable.Columns[0].ColumnMapping = MappingType.Hidden;

            //_dataTable.Columns.Add(START_YEAR);
            //_dataTable.Columns.Add(END_YEAR);
            //_dataTable.Columns.Add(START_QUARTER);
            //_dataTable.Columns.Add(END_QUARTER);

            DataRow drHeader = _dataTable.NewRow();
            drHeader[QUERY_TYPE_ID] = string.Empty;
            drHeader[QUERY_TYPE] = string.Empty;
            drHeader[ANNUAL] = string.Format("{0}      {1}", "From", "To");
            drHeader[QUARTERLY] = string.Format("{0}          {1}", "From", "To");

            //drHeader["Start Year"] = string.Empty;
            //drHeader["End Year"] = string.Empty;
            //drHeader["Start Quarter"] = string.Empty;
            //drHeader["End Quarter"] = string.Empty;

            _dataTable.Rows.InsertAt(drHeader, 0);

            //foreach (QueryType qt in queryTypeCollection)
            DataTable distinctQueryTypeTable = dataset.Tables[0].DefaultView.ToTable("DistinctQueryTypeTable", true, new string[] { "QueryTypeId" });
            foreach (DataRow row in distinctQueryTypeTable.Rows)
            {
                object _startYear, _endYear, _startQuarter, _endQuarter = null;

                int queryTypeId = (int)row[QUERY_TYPE_ID];
                _startYear = dataset.Tables[0].Compute("Min(Period)", "QueryTypeId = " + queryTypeId + " AND Period not like '*Q*'");
                _endYear = dataset.Tables[0].Compute("Max(Period)", "QueryTypeId = " + queryTypeId + " AND Period not like '*Q*'");
                _startQuarter = dataset.Tables[0].Compute("Min(Period)", "QueryTypeId = " + queryTypeId + " AND Period like '*Q*'");
                _endQuarter = dataset.Tables[0].Compute("Max(Period)", "QueryTypeId = " + queryTypeId + " AND Period like '*Q*'");

                DataRow dr = _dataTable.NewRow();
                dr[QUERY_TYPE] = dataset.Tables[0].Select("QueryTypeId = " + queryTypeId)[0]["Query Type"]; // TODO: query should be fixed to send over the name with the select
                dr["Data Availability (Annual)"] = string.Format("{0}      {1}", (null != _startYear) ? _startYear.ToString() : string.Empty, (null != _endYear) ? _endYear.ToString() : string.Empty);
                dr["Data Availability (Quarterly)"] = string.Format("{0}      {1}", (null != _startQuarter) ? _startQuarter.ToString() : string.Empty, (null != _endQuarter) ? _endQuarter.ToString() : string.Empty);
                
                dr[QUERY_TYPE_ID] = row["QueryTypeId"];
                //dr[START_YEAR] = _startYear ?? string.Empty;
                //dr[END_YEAR] = _endYear ?? string.Empty;
                //dr[START_QUARTER] = _startQuarter ?? string.Empty;
                //dr[END_QUARTER] = _endQuarter ?? string.Empty;

                _dataTable.Rows.Add(dr);
            }

            return _dataTable;
        }

        //Execute query on appropriate database based on the configured data source
        private DataSet ExecuteQuery(string query)
        {
            DataSet ds = new DataSet();
            status.Code = RequestStatus.StatusCode.InProgress;

            string server = Settings.GetAsString("Server", "");
            string port = Settings.GetAsString("Port", "");
            string userId = Settings.GetAsString("UserID", "");
            string password = Settings.GetAsString("Password", "");
            string database = Settings.GetAsString("Database", "");
            string dataSourceName = Settings.GetAsString("DataSourceName", "");
            string connectionTimeout = Settings.GetAsString("ConnectionTimeout", "15");
            string commandTimeout = Settings.GetAsString("CommandTimeout", "120");

            log.Debug("Connection timeout: " + connectionTimeout + ", Command timeout: " + commandTimeout);
            log.Debug("Executing Query: " + query);

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
                    using (OdbcConnection connection = new OdbcConnection(string.Format("DSN={0}", dataSourceName)))
                    try
                    {
                        OdbcDataAdapter da = new OdbcDataAdapter(query, connection);
                        da.Fill(ds);
                    }
                    finally
                    {
                        connection.Close();
                    }                        
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
                            da.Fill(ds);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;
                default:
                    throw new Exception(CommonMessages.Exception_InvalidDataProviderType);

            }
            status.Code = RequestStatus.StatusCode.Complete;
            status.Message = "";
            return ds;
        }

        private void BuildResponseDocuments()
        {
            if (responseDocument == null)
            {
                // Metadata request returns two documents, a display version of the metadata result, and processable version.
                // Summary query requests returns two documents if necessary, the query result and the style document.
                responseDocument = new Document[IsMetadataRequest || hasCellCountAlert ? 2 : 1];
                responseDocument[0] = new Document("0", "x-application/lpp-dns-table", "SummaryQueryResponse.xml");
                responseDocument[0].IsViewable = true;
                responseDocument[0].Size = viewableResultDataset.GetXml().Length;

                if (hasCellCountAlert)
                {
                    ViewableDocumentStyle style = new ViewableDocumentStyle
                    {
                        Css = ".StyledRows { background-color: yellow; color: red }",
                        StyledRows = SummaryQueryUtil.FindLowCellCountsInQueryResult(viewableResultDataset.Tables[0], CellThreshHold)
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
                else if (IsMetadataRequest)
                {
                    responseDocument[1] = new Document("1", "application/xml", "RefreshDatesResponse.xml");
                    responseDocument[1].IsViewable = false;
                    responseDocument[1].Size = metadataResultDataset == null ? 0 : metadataResultDataset.GetXml().Length;
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

        #region Unfinished

        // TODO[ddee] Loads ODBC connection string for Access database. Moved from old DMClient.
        //            Used to persist results.
        //internal static void LoadClientDataStoreConnectionString()
        //{
        //    Global.ClientDataStoreConString = string.Empty;
        //    OdbcConnection oODBCConnection = null;
        //    string mdbPath = string.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppDataFolderName, "DataStore\\ClientDataStore.mdb");
        //    try
        //    {
        //        string sConnString = "Driver={Microsoft Access Driver (*.mdb)}; Dbq= " + mdbPath + ";";
        //        oODBCConnection = new OdbcConnection(sConnString);
        //        oODBCConnection.Open();
        //        Global.ClientDataStoreConString = sConnString;    
        //    }
        //    catch (Exception)
        //    {           
        //        // 32 bit attempt failed. so try with 64 bit
        //    }

        //    if (!string.IsNullOrEmpty(Global.ClientDataStoreConString))
        //        return;
                  
        //    try
        //    {
        //        string sConnString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq= " + mdbPath + ";";
        //        oODBCConnection = new OdbcConnection(sConnString);
        //        oODBCConnection.Open();

        //        Global.ClientDataStoreConString = sConnString;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

       //internal static bool InsertProcessedResultsToDataStore(int QueryId, int DataMartId, int NetworkId, string QueryName, string ResponseXml, int UserId, int QueryTypeId)
       // {
       //     if (string.IsNullOrEmpty(Global.ClientDataStoreConString))
       //         return false;

       //     string mdbPath = string.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppDataFolderName, "DataStore\\ClientDataStore.mdb");
       //     string tableName = "ProcessedQueryResults";
       //     OdbcConnection oODBCConnection = null;
       //     bool isInsertedSuccessfully = false;
       //     try
       //     {

       //         //string sConnString = "Driver={Microsoft Access Driver (*.mdb)}; Dbq= " + mdbPath + ";";
       //         oODBCConnection = new OdbcConnection(Global.ClientDataStoreConString);

       //         string commandStr = "INSERT INTO " + tableName + " VALUES ('" + NetworkId + "','" + QueryId + "','" + DataMartId + "','" + QueryName + "','" + ResponseXml + "', '" + UserId + "','" + DateTime.Now + "','" + QueryTypeId + "' )";
       //         oODBCConnection.Open();

       //         OdbcCommand odbcCommand = new OdbcCommand(commandStr, oODBCConnection); ;

       //         odbcCommand.ExecuteNonQuery();
       //         isInsertedSuccessfully = true;                

       //     }
       //     catch (Exception ex)
       //     {
       //         isInsertedSuccessfully = false;

       //         AuditManager.WriteExceptionLogEntry(String.Format("An error occurred while attempting to insert processed query {0} result to local database.", QueryId), ex);
       //     }
       //     finally
       //     {
       //         if (null != oODBCConnection && oODBCConnection.State != ConnectionState.Closed)
       //             oODBCConnection.Close();               
       //     }
       //     return isInsertedSuccessfully;
       // }

        #endregion
    }
}