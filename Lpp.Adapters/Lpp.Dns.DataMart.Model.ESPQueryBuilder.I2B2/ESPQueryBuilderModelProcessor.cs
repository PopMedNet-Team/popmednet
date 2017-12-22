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
    public class ESPQueryBuilderModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string ICD9_DIAGNOSIS = "{6A900001-CFC3-439C-9978-A22200FC5253}";
        private const string REPORTABLE_DISEASES = "{84CF0001-475C-46AB-998B-A22200FC6439}";
        private const string QUERY_COMPOSER = "{15830001-6DFF-47E9-B2FD-A22200FC77C3}";

        private const string PROCESSOR_ID = "1BD526D9-46D8-4F66-9191-5731CB8189EE";
        private IModelMetadata modelMetadata = new ESPQueryBuilderModelMetadata();
        private Document[] responseDocument;

        [Serializable]
        internal class ESPQueryBuilderModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "7C69584A-5602-4FC0-9F3F-A27F329B1113";

            readonly Lpp.Dns.DataMart.Model.Settings.SQLProvider[] sqlProviders;
            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public ESPQueryBuilderModelMetadata()
            {
                sqlProviders = new []{ Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer, Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL };

                capabilities = new Dictionary<string, bool>() {{ "IsSingleton", false }};

                properties = new Dictionary<string, string>();
                Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
                properties.Add("Translator", string.Empty);
            }
            
            public string ModelName
            {
                get { return "ESPQueryBuilderModelProcessor"; }
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
            Process translatorJARProcess = null;
            ProcessStartInfo translatorJARStartInfo = null;
            Lpp.Dns.DataMart.Model.Settings.RequestTranslator translator = Lpp.Dns.DataMart.Model.Settings.RequestTranslator.ESP_PostgreSQL;
            if (Settings.Keys.Contains("Translator"))
                translator = (Lpp.Dns.DataMart.Model.Settings.RequestTranslator)Enum.Parse(typeof(Lpp.Dns.DataMart.Model.Settings.RequestTranslator), Settings.GetAsString("Translator", ""), true);
            
            try
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var doc = desiredDocuments.FirstOrDefault( d => d.DocumentID == documentId );


                    // For non-HQMF queries, file with this name is the query file. Ignore others.
                    if (translator != Lpp.Dns.DataMart.Model.Settings.RequestTranslator.HQMF_PostgreSQL && doc.Filename == "ESPRequest.xml")
                    {
                        requestParameterXml = reader.ReadToEnd();
                        byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(requestParameterXml);
                        XPathDocument xml = new XPathDocument(new MemoryStream(bytes));
                        XslCompiledTransform xslt = new XslCompiledTransform();

                        string requestTypeGUID = "{" + requestTypeId.ToUpper() + "}";

                        if (requestTypeGUID == QUERY_COMPOSER)
                        {
                            // =============================================================\\\
                            // TODO: Remove this block, it's for testing purposes ONLY!
                            // =============================================================
                            //string testingXmlFileName = "Request.xml";
                            //string testingFolderPath = @"C:\XSLT-Testing";
                            //string testingOutputFilePrefix = "UIRequest_";
                            //if (File.Exists(testingFolderPath + @"\" + testingXmlFileName))
                            //{
                            //    xml = new XPathDocument(testingFolderPath + @"\" + testingXmlFileName);
                            //    testingOutputFilePrefix = "FileRequest_";
                            //}
                            // =============================================================///

                            using (Stream stream = (translator == Lpp.Dns.DataMart.Model.Settings.RequestTranslator.ESP_PostgreSQL ? typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.i2b2.QueryComposer.xsl") : typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.I2B2.QueryComposerI2B2.xsl")))
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
                        else
                        {
                            using (Stream stream = (translator == Lpp.Dns.DataMart.Model.Settings.RequestTranslator.ESP_PostgreSQL ? typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.I2B2.RequestBuilder.xsl") : typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.I2B2.RequestBuilderI2B2.xsl")))
                            {
                                //using (XmlTextReader transform = new XmlTextReader(new FileStream("RequestBuilder.xsl", FileMode.Open, FileAccess.Read)))
                                using (XmlTextReader transform = new XmlTextReader(stream))
                                {
                                    using (var writer = new StringWriter())
                                    {
                                        xslt.Load(transform);
                                        xslt.Transform(xml, null, writer);
                                        query = writer.ToString();
                                        log.Debug(query);
                                    }
                                }
                            }
                        }
                    }
                    // For HQMF queries, file with this name is the query file. Ignore others.
                    if (translator == Lpp.Dns.DataMart.Model.Settings.RequestTranslator.HQMF_PostgreSQL && doc.Filename == "ESPRequestHQMF.xml")
                    {
                        requestParameterXml = reader.ReadToEnd();
                        byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(requestParameterXml);
                        XPathDocument xml = new XPathDocument(new MemoryStream(bytes));
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        using (Stream stream = typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.HQMFtoIntermediate_v2.xsl"))
                        {
                            //using (XmlTextReader transform = new XmlTextReader(new FileStream("RequestBuilder.xsl", FileMode.Open, FileAccess.Read)))
                            using (XmlTextReader transform = new XmlTextReader(stream))
                            {
                                xslt.Load(transform);

                                // =============================================================\\\
                                // I'm using a MemoryStream here, rather than a StringWriter,
                                // because Dragon's Intermediate-to-SQL translator JAR utility
                                // chokes on anything other than UTF-8 encoding, and use of a
                                // StringWriter in xslt.Transform() always results in UTF-16.
                                // (At least according to StackOverflow and others.)
                                // =============================================================
                                XmlWriterSettings writerSettings = new XmlWriterSettings();
                                writerSettings.Encoding = System.Text.Encoding.UTF8;
                                writerSettings.Indent = true;

                                MemoryStream transformedXml = new MemoryStream();
                                xslt.Transform(xml, XmlWriter.Create(transformedXml, writerSettings));

                                transformedXml.Position = 0;
                                StreamReader transformedXmlReader = new StreamReader(transformedXml);
                                // =============================================================///

                                string tempHQMFFolderPath = Settings.GetAsString("AppFolderPath", "") + @"\HQMF\temp";
                                if (!Directory.Exists(tempHQMFFolderPath)) Directory.CreateDirectory(tempHQMFFolderPath);

                                // Output Intermediate XML.
                                StreamWriter swIntermediate = new StreamWriter(tempHQMFFolderPath + @"\Intermediate.xml");
                                swIntermediate.Write(transformedXmlReader.ReadToEnd());
                                swIntermediate.Close();

                                // =============================================================\\\
                                // TODO: Remove this once the XSLT works!
                                // Back up output, replace with a WORKING Intermediate XML file.
                                // =============================================================
                                //File.Move(tempHQMFFolderPath + @"\Intermediate.xml", tempHQMFFolderPath + @"\Intermediate_OUTPUT_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xml");
                                //File.Copy(tempHQMFFolderPath + @"\Intermediate_WORKING.xml", tempHQMFFolderPath + @"\Intermediate.xml");
                                // =============================================================///

                                string translatorJARFolderPath = Settings.GetAsString("AppFolderPath", "") + @"\HQMF\HQMF_SQL_Translator";

                                translatorJARStartInfo.FileName = "java";
                                translatorJARStartInfo.WorkingDirectory = translatorJARFolderPath;
                                translatorJARStartInfo.UseShellExecute = false;
                                translatorJARStartInfo.CreateNoWindow = true;
                                translatorJARStartInfo.RedirectStandardOutput = true;
                                translatorJARStartInfo.RedirectStandardError = true;

                                string quotedJARPath = @" """ + translatorJARFolderPath + @"\HQMF_SQL_Translator.jar"" ";
                                string quotedXMLPath = @" """ + tempHQMFFolderPath + @"\Intermediate.xml"" ";

                                translatorJARStartInfo = new ProcessStartInfo(); 
                                translatorJARStartInfo.Arguments = "-jar " + quotedJARPath + " " + quotedXMLPath;
                                translatorJARProcess = new Process();
                                translatorJARProcess.StartInfo = translatorJARStartInfo;
                                translatorJARProcess.Start();

                                // I'm using the asynchronous methods (BeginErrorReadLine and BeginOutputReadLine) because
                                // I can't use the synchronous methods (StandardError.ReadToEnd and StandardOutput.ReadToEnd)
                                // to retrieve *both* Error and Output, per this MSDN documentation page:
                                //     http://msdn.microsoft.com/en-us/library/system.diagnostics.process.standardoutput.aspx
                                // Attempting to do so will create a deadlock position if Output or Error is long.
                                // I decided to use the asynchronous methods on both streams for consistency.
                                translatorJARProcess.OutputDataReceived += new DataReceivedEventHandler(translatorJARProcess_OutputDataReceived);
                                translatorJARProcess.ErrorDataReceived += new DataReceivedEventHandler(translatorJARProcess_ErrorDataReceived);
                                translatorJARProcess.BeginErrorReadLine();
                                translatorJARProcess.BeginOutputReadLine();

                                // Wait for the process to exit, so that I'll have the complete Error and Output strings
                                // by the next line of code.
                                translatorJARProcess.WaitForExit();

                                if (translatorJARProcessError != "")
                                {
                                    throw new Exception(translatorJARProcessError);
                                }
                                string outputSQLFilePath = translatorJARFolderPath + @"\sql\SQLQuery" + DateTime.Now.ToString("yyyy-MM-dd") + ".sql";
                                if (!File.Exists(outputSQLFilePath))
                                {
                                    throw new Exception("Unable to generate SQL query.");
                                }

                                // =============================================================\\\
                                // TODO: Remove this once the JAR works!
                                // Back up output, replace with a WORKING SQL file.
                                // =============================================================
                                //File.Move(outputSQLFilePath, translatorJARFolderPath + @"\sql\SQLQuery" + DateTime.Now.ToString("yyyy-MM-dd") + "_OUTPUT_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql");
                                //File.Copy(translatorJARFolderPath + @"\sql\SQLQuery_WORKING.sql", outputSQLFilePath);
                                // =============================================================///

                                query = File.ReadAllText(outputSQLFilePath);
                                log.Debug(query);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (translatorJARProcess != null && !translatorJARProcess.HasExited)
                {
                    translatorJARProcess.Kill();
                }
                if (translatorJARProcess != null)
                {
                    translatorJARProcess.Dispose();
                    translatorJARStartInfo = null;
                }
                log.Debug(ex.Message, ex);
                status.Code = RequestStatus.StatusCode.Error;
                status.Message = ex.Message;
                throw ex;
            }
        }

        void translatorJARProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                translatorJARProcessError += e.Data + Environment.NewLine;
            }
        }
        void translatorJARProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                translatorJARProcessOutput += e.Data + Environment.NewLine;
                if (e.Data.StartsWith("ERROR -"))
                {
                    translatorJARProcessError += e.Data + Environment.NewLine;
                }
            }
        }

        public void Start(string requestId, bool viewSQL)
        {
            try
            {
                if (Settings.Count == 0)
                    throw new Exception(CommonMessages.Exception_MissingSettings);

                status.Code = RequestStatus.StatusCode.InProgress;

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

                if (!Settings.ContainsKey("DataProvider"))
                    throw new Exception(CommonMessages.Exception_MissingDataProviderType);

                string connectionString = string.Empty;
                switch ((Lpp.Dns.DataMart.Model.Settings.SQLProvider)Enum.Parse(typeof(Lpp.Dns.DataMart.Model.Settings.SQLProvider), Settings.GetAsString("DataProvider",""), true))
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
