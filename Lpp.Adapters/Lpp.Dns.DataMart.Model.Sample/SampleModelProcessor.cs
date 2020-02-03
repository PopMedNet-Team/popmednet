using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Diagnostics;
using log4net;
using Npgsql;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class SampleModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PROCESSOR_ID = "F985DBD9-DA7E-41B4-8FBD-2A73B7FCF6DD";
        private IModelMetadata modelMetadata = new SampleModelMetadata();
        private Document[] responseDocument;

        [Serializable]
        internal class SampleModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "C59F449C-230C-4A6D-B37F-AB62C60ED471";

            readonly Model.Settings.SQLProvider[] sqlProviders;
            readonly  IDictionary<string, bool> capabilities = new Dictionary<string, bool>() { { "IsSingleton", false } };
            readonly  IDictionary<string, string> properties = new Dictionary<string, string>() { { "Server", "" }, { "Port", "" }, { "UserId", "" }, { "Password", "" }, { "Database", "" }, { "ConnectionTimeout", "" }, { "CommandTimeout", "" }, { "Translator", "" }, { "DataProvider", "" }, { "DataSourceName", "" } };

            public SampleModelMetadata()
            {
                sqlProviders = new[] { Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer, Lpp.Dns.DataMart.Model.Settings.SQLProvider.ODBC, Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL };
                
                capabilities = new Dictionary<string, bool>() { { "IsSingleton", false } };
                
                properties = new Dictionary<string, string>();
                Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
                properties.Add("Translator", string.Empty);
            }
            
            public string ModelName
            {
                get { return "SampleModelProcessor"; }
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

                    List<KeyValuePair<string, object>> values = new List<KeyValuePair<string, object>>();
                    foreach (var value in Enum.GetValues(typeof(Settings.RequestTranslator)))
                    {
                        values.Add(new KeyValuePair<string, object>(((Settings.RequestTranslator)value).ToString("f").Replace("_", " "), (Settings.RequestTranslator)value));
                    }

                    settings.Add(new Settings.ProcessorSetting
                    {
                        Title = "Translator",
                        Key = "Translator",
                        ValueType = typeof(Settings.RequestTranslator),
                        DefaultValue = Lpp.Dns.DataMart.Model.Settings.RequestTranslator.ESP_PostgreSQL.ToString("d"),
                        ValidValues = values
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
        private string query;
        private RequestStatus status = new RequestStatus();
        private DataSet resultDataset;
        private Document[] desiredDocuments;

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
        }

        public void Request(string requestId, NetworkConnectionMetadata network, RequestMetadata md,
            Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments)
        {
            this.requestTypeId = md.RequestTypeId;
            requestProperties = null;
            desiredDocuments = requestDocuments;
            this.desiredDocuments = requestDocuments;
            resultDataset = new DataSet();
            status.Code = RequestStatus.StatusCode.InProgress;
            status.Message = "";
        }

        public void RequestDocument(string requestId, string documentId, Stream contentStream)
        {
            try
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var doc = desiredDocuments.FirstOrDefault( d => d.DocumentID == documentId );
                    query = reader.ReadToEnd();
                    log.Debug( query );
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

        public void Start(string requestId, bool viewSQL)
        {
            try
            {
                if (Settings.Count == 0)
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

                log.Debug("Connection timeout: " + connectionTimeout + ", Command timeout: " + commandTimeout);

                if (!Settings.ContainsKey("DataProvider"))
                    throw new Exception(CommonMessages.Exception_MissingDataProviderType);

                string connectionString = string.Empty;
                switch ((Lpp.Dns.DataMart.Model.Settings.SQLProvider)Enum.Parse(typeof(Lpp.Dns.DataMart.Model.Settings.SQLProvider), Settings.GetAsString("DataProvider", ""), true))
                {
                    case Lpp.Dns.DataMart.Model.Settings.SQLProvider.ODBC:
                        if (string.IsNullOrWhiteSpace(dataSourceName))
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
                        break;

                    case Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL:
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
                        if (port == null || port == string.Empty)
                            port = "5432";
                        connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Timeout={5};CommandTimeout={6}", server, port, userId, password, database, connectionTimeout, commandTimeout);
                        // Making connection with Npgsql provider
                        using (NpgsqlConnection connnection = new NpgsqlConnection(connectionString))
                        {
                            try
                            {
                                connnection.Open();
                                NpgsqlCommand command = new NpgsqlCommand(query, connnection);
                                NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                                resultDataset.Reset();
                                da.Fill(resultDataset);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                connnection.Close();
                            }
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
                        break;

                    //case Lpp.Dns.DataMart.Model.Settings.SQLProvider.Oracle:
                    //    // TODO: Implement this provider
                    //    //throw new NotImplementedException("Oracle client not implemented yet");
                    //    //connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};;Timeout={5};CommandTimeout={6}", server, port, userId, password, database, connectionTimeout, commandTimeout);
                    //    //// TODO: Upgrade Oracle client
                    //    //using (System.Data.OracleClient.OracleConnection connection = new System.Data.OracleClient.OracleConnection(connectionString))
                    //    //{
                    //    //    try
                    //    //    {
                    //    //        connection.Open();
                    //    //        System.Data.OracleClient.OracleCommand command = new System.Data.OracleClient.OracleCommand(query, connection);
                    //    //        System.Data.OracleClient.OracleDataAdapter da = new System.Data.OracleClient.OracleDataAdapter(command);
                    //    //        resultDataset.Reset();
                    //    //        da.Fill(resultDataset);
                    //    //    }
                    //    //    catch (Exception ex)
                    //    //    {
                    //    //        throw ex;
                    //    //    }
                    //    //    finally
                    //    //    {
                    //    //        connection.Close();
                    //    //    }
                    //    //}
                    //    break;
                    default:
                        throw new Exception(CommonMessages.Exception_InvalidDataProviderType);
                }
                status.Code = RequestStatus.StatusCode.Complete;
                status.Message = "";
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
                var stream = new MemoryStream();

                // Embed the schema. The schema contains column header information in case no rows are returned.
                resultDataset.WriteXml(stream, XmlWriteMode.WriteSchema);
                stream.Seek(0, SeekOrigin.Begin);
                contentStream = stream;
            }
        }


        public void Close(string requestId)
        {
        }

        public void PostProcess(string requestId)
        {
            // throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void BuildResponseDocuments()
        {
            if (resultDataset != null)
            {
                responseDocument = new Document[1];
                responseDocument[0] = new Document("0", "x-application/lpp-dns-table", "SampleModelResponse.xml");
                responseDocument[0].IsViewable = true;

                using (MemoryStream stream = new MemoryStream())
                {
                    // Embed the schema. The schema contains column header information in case no rows are returned.
                    resultDataset.WriteXml(stream, XmlWriteMode.WriteSchema);
                    stream.Seek(0, SeekOrigin.Begin);
                    string xml = new StreamReader(stream).ReadToEnd();
                    responseDocument[0].Size = xml.Length;
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
