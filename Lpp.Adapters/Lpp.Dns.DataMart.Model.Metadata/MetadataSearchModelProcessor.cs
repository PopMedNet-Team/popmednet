using log4net;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class MetadataSearchModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PROCESSOR_ID = "9D0CD143-7DCA-4953-8209-224BDD3AF718";
        private IModelMetadata modelMetadata = new MetadataQueryModelMetadata();
        private Document[] responseDocument;

        [Serializable]
        internal class MetadataQueryModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "8584F9CD-846E-4024-BD5C-C2A2DD48A5D3";

            readonly Settings.SQLProvider[] sqlProviders;
            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public MetadataQueryModelMetadata()
            {
                sqlProviders = new []{ Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer };

                capabilities = new Dictionary<string, bool>() { { "IsSingleton", false } };

                properties = new Dictionary<string, string>();
                Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
            }

            public string ModelName
            {
                get { return "MetadataQueryModelProcessor"; }
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
                    var doc = desiredDocuments.FirstOrDefault(d => d.DocumentID == documentId);
                    query = reader.ReadToEnd();
                    log.Debug(query);
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

                //var requests = Requests.All.Where(rs => rs.Submitted.HasValue).Select(r => r.Id);
                //string json = JsonConvert.SerializeObject(requests);

                string server = Settings.GetAsString("Server","");
                string port = Settings.GetAsString("Port","");
                string userId = Settings.GetAsString("UserID","");
                string password = Settings.GetAsString("Password","");
                string database = Settings.GetAsString("Database","");
                string connectionTimeout = Settings.GetAsString("ConnectionTimeout","15");
                string commandTimeout = Settings.GetAsString("CommandTimeout","120");

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

                string connectionString = string.Empty;

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
                        List<int> queries = new List<int>();
                        foreach (DataRow row in resultDataset.Tables[0].Rows)
                        {
                            queries.Add((int)row["Id"]);
                        }
                        string json = JsonConvert.SerializeObject(queries);
                    }
                    finally
                    {
                        connection.Close();
                    }
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
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void BuildResponseDocuments()
        {
            if (resultDataset != null)
            {
                responseDocument = new Document[1];
                responseDocument[0] = new Document("0", "x-application/lpp-dns-table", "MetadataQueryResponse.xml");
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
