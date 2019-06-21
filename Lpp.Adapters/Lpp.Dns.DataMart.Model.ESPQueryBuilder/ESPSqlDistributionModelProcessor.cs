using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using log4net;
using Npgsql;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class ESPSqlDistributionModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PROCESSOR_ID = "AE85D3E6-93F8-4CB5-BD45-D2F84AB85D83";
        private IModelMetadata modelMetadata = new ESPSqlDistributionModelMetadata();
        private Document[] responseDocument;

        [Serializable]
        internal class ESPSqlDistributionModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "3178367A-65BA-4DAE-9070-CD786E925635";

            readonly Settings.SQLProvider[] sqlProviders;
            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public ESPSqlDistributionModelMetadata()
            {
                sqlProviders = new[] { Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL };

                capabilities = new Dictionary<string, bool>() {{ "IsSingleton", false }};

                properties = new Dictionary<string, string>();
                Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
            }

            public string ModelName
            {
                get { return "ESPSqlDistributionModelProcessor"; }
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

                // PostgeSQL-style connection string
                string server = Settings.GetAsString("Server", "");
                string port = Settings.GetAsString("Port", "");
                string userId = Settings.GetAsString("UserID", "");
                string password = Settings.GetAsString("Password", "");
                string database = Settings.GetAsString("Database", "");
                string connectionTimout = Settings.GetAsString("ConnectionTimeout", "15");
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

                string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Timeout={5};CommandTimeout={6}", server, port, userId, password, database, connectionTimout, commandTimeout);
                log.Debug("Connection timeout: " + connectionTimout + ", Command timeout: " + commandTimeout);
                log.Debug("Query: " + query);

                // Making connection with Npgsql provider
                using (NpgsqlConnection conn = new NpgsqlConnection(connstring))
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand command = new NpgsqlCommand(query, conn);
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                        resultDataset.Reset();
                        da.Fill(resultDataset);
                    }
                    catch (Npgsql.NpgsqlException ex)
                    {
                        status.Code = RequestStatus.StatusCode.Complete;
                        status.Message = ex.Message;
                        // This code will catch SQL errors, both conneciton related and syntax which I expect will happen often, so we really don't want to throw an error here.  
                        // A better approach would be to handle this by displaying a message in the the result area.  The output of this request is a table, so perhaps we could
                        // build a table with a single rown whose colums are the information returned by the exception.
                        DataTable table = resultDataset.Tables.Add("Error");
                        table.Columns.Add("ErrorMessage");
                        DataRow row = table.Rows.Add();
                        row["ErrorMessage"] = ex.Message;
                        //throw ex;
                    }
                    catch (Exception exc)
                    {
                        status.Code = RequestStatus.StatusCode.Error;
                        status.Message = exc.Message;
                        throw exc;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                if (status.Code == RequestStatus.StatusCode.InProgress)
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
            Document document = new Document(Guid.NewGuid().ToString("D"), mimeType, filePath);
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

        const string RESPONSE_DOCUMENTID = "757E51F0-9C0B-4A34-AF0A-AE80D817845B";
        public void ResponseDocument(string requestId, string documentId, out Stream contentStream, int maxSize)
        {
            contentStream = null;

            if ( documentId == RESPONSE_DOCUMENTID )
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
            //throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void BuildResponseDocuments()
        {
            if (resultDataset != null)
            {
                responseDocument = new Document[1];
                responseDocument[0] = new Document(RESPONSE_DOCUMENTID, "x-application/lpp-dns-table", "SQLResponse.xml");
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
