using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public class ConditionsModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string CONDITIONS = "{4EEE0635-AC4C-49A2-9CF7-2A6C923DC176}";

        private const string PROCESSOR_ID = "D1C750B3-BA77-4F40-BA7E-F5FF28137CAF";
        private IModelMetadata modelMetadata = new ConditionsModelMetadata();
        private Document[] responseDocument;

        [Serializable]
        internal class ConditionsModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "EA26172E-1B5F-4616-B082-7DABFA66E1D2";

            readonly Settings.SQLProvider[] sqlProviders;
            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public ConditionsModelMetadata()
            {
                sqlProviders = new[] { Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer, Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL };

                capabilities = new Dictionary<string, bool>() {{ "IsSingleton", false }};

                properties = new Dictionary<string, string>();
                Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
                properties.Add("Translator", string.Empty);
            }
            
            public string ModelName
            {
                get { return "ConditionsModelProcessor"; }
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

                    List<KeyValuePair<string,object>> values = new List<KeyValuePair<string,object>>();
                    foreach (var value in Enum.GetValues(typeof(Settings.RequestTranslator)))
                    {
                        values.Add(new KeyValuePair<string, object>(((Settings.RequestTranslator)value).ToString("f").Replace("_", " "), (Settings.RequestTranslator)value));
                    }
                    
                    settings.Add(new Settings.ProcessorSetting { 
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
        private string requestParameterXml;
        private string query;
        private RequestStatus status = new RequestStatus();
        private DataSet resultDataset;
        private Document[] desiredDocuments;
        private string translatorJARProcessError;
        private string translatorJARProcessOutput;

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
        }

        public void Request( string requestId, NetworkConnectionMetadata network, RequestMetadata md,
            Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments )
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

                    // File with this name is the query file. Ignore others.
                    if (doc.Filename == "ConditionsRequest.xml")
                    {
                        requestParameterXml = reader.ReadToEnd();
                        byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(requestParameterXml);
                        XPathDocument xml = new XPathDocument(new MemoryStream(bytes));
                        XslCompiledTransform xslt = new XslCompiledTransform();

                        string requestTypeGUID = "{" + requestTypeId.ToUpper() + "}";

                        if (requestTypeGUID == CONDITIONS)
                        {

                            using (Stream stream = (typeof(ConditionsModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.Conditions.Conditions.xsl") ) )
                            {
                                //using (XmlTextReader transform = new XmlTextReader(new FileStream("RequestBuilder.xsl", FileMode.Open, FileAccess.Read)))
                                using (XmlTextReader transform = new XmlTextReader(stream))
                                {
                                    using (var writer = new StringWriter())
                                    {
                                        xslt.Load(transform);
                                        xslt.Transform(xml, null, writer);
                                        query = writer.ToString();

                                        // =============================================================\\\
                                        // TODO: Remove this block, it's for testing purposes ONLY!
                                        // =============================================================
                                        // Write XSLT output to a file.
                                        //if (Directory.Exists(testingFolderPath))
                                        //{
                                        //    string testingOutputFileName = testingOutputFilePrefix + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql";
                                        //    StreamWriter swOutputFileName = new StreamWriter(testingFolderPath + @"\" + testingOutputFileName);
                                        //    swOutputFileName.Write(query);
                                        //    swOutputFileName.Close();
                                        //}
                                        // =============================================================///

                                        log.Debug(query);
                                    }
                                }
                            }
                        }
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
                string connectionTimeout = Settings.GetAsString("ConnectionTimeout", "15");
                string commandTimeout = Settings.GetAsString("CommandTimeout", "120");

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

                log.Debug("Connection timeout: " + connectionTimeout + ", Command timeout: " + commandTimeout);
                log.Debug("Query: " + query);

                if (!Settings.ContainsKey("DataProvider"))
                    throw new Exception(CommonMessages.Exception_MissingDataProviderType);

                string connectionString = string.Empty;
                switch ((Lpp.Dns.DataMart.Model.Settings.SQLProvider)Enum.Parse(typeof(Lpp.Dns.DataMart.Model.Settings.SQLProvider), Settings.GetAsString("DataProvider", ""), true))
                {
                    case Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL:
                    if (port == null || port == string.Empty)
                        port = "5432";
                        connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};;Timeout={5};CommandTimeout={6}", server, port, userId, password, database, connectionTimeout, commandTimeout);
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
                        if (port != null && port != string.Empty) server += ", " + port;
                        connectionString = userId != null && userId != string.Empty ?  String.Format("server={0};User ID={1};Password={2};Database={3}; Connection Timeout={4}", server, userId, password, database, connectionTimeout) :
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

                    case Lpp.Dns.DataMart.Model.Settings.SQLProvider.Oracle:
                        // TODO: Implement this provider
                        throw new NotImplementedException("Oracle client not implemented yet");
                        //connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};;Timeout={5};CommandTimeout={6}", server, port, userId, password, database, connectionTimeout, commandTimeout);
                        //// TODO: Upgrade Oracle client
                        //using (System.Data.OracleClient.OracleConnection connection = new System.Data.OracleClient.OracleConnection(connectionString))
                        //{
                        //    try
                        //    {
                        //        connection.Open();
                        //        System.Data.OracleClient.OracleCommand command = new System.Data.OracleClient.OracleCommand(query, connection);
                        //        System.Data.OracleClient.OracleDataAdapter da = new System.Data.OracleClient.OracleDataAdapter(command);
                        //        resultDataset.Reset();
                        //        da.Fill(resultDataset);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        throw ex;
                        //    }
                        //    finally
                        //    {
                        //        connection.Close();
                        //    }
                        //}
                        //break;
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

            if ( documentId == "0" )
            {
                var stream = new MemoryStream();

                // Embed the schema. The schema contains column header information in case no rows are returned.
                resultDataset.WriteXml( stream, XmlWriteMode.WriteSchema );
                stream.Seek( 0, SeekOrigin.Begin );
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
                responseDocument[0] = new Document("0", "x-application/lpp-dns-table", "ESPResponse.xml");
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
