using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Diagnostics;
using log4net;
using Npgsql;
using Newtonsoft.Json;
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

            readonly Settings.SQLProvider[] sqlProviders;
            readonly IDictionary<string, bool> capabilities; 
            readonly IDictionary<string, string> properties;

            public ESPQueryBuilderModelMetadata()
            {
                sqlProviders = new[] { Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL };

                capabilities = new Dictionary<string, bool>() {{ "IsSingleton", false }, {"CanViewSQL", true}};

                properties = new Dictionary<string, string>();
                Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
                properties.Add("UseHQMF", string.Empty);
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
                    settings.Add(new Settings.ProcessorSetting { Title = "Low Cell Count", Key = "ThreshHoldCellCount", DefaultValue = "0", ValueType = typeof(int) });
                    settings.Add(new Settings.ProcessorSetting { Title = "Use HQMF Documents", Key = "UseHQMF", ValueType = typeof(bool), DefaultValue = "false" });
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
        private string demoQuery = null;
        private RequestStatus status = new RequestStatus();
        private DataSet resultDataset;
        private DataSet resultDemoDataset;
        private Document[] desiredDocuments;
        private string translatorJARProcessError;
        private string translatorJARProcessOutput;
        private bool hasCellCountAlert = false;
        private string styleXml;

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
            this.requestTypeId = md.RequestTypeId;
            requestProperties = null;
            desiredDocuments = requestDocuments;
            this.desiredDocuments = requestDocuments;
            resultDataset = new DataSet();
            resultDemoDataset = new DataSet();
            status.Code = RequestStatus.StatusCode.InProgress;
            status.Message = "";
        }

        public void RequestDocument(string requestId, string documentId, Stream contentStream)
        {
            Process translatorJARProcess = null;
            ProcessStartInfo translatorJARStartInfo = null; 
            translatorJARProcessError = "";
            translatorJARProcessOutput = "";

            try
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var doc = desiredDocuments.FirstOrDefault( d => d.DocumentID == documentId );

                    bool useHQMF = false;

                    // For non-HQMF queries, file with this name is the query file. Ignore others.
                    if (!useHQMF && (doc.Filename == "ESPRequest.xml" || doc.Filename == "ESPDemographicRequest.xml"))
                    {
                        requestParameterXml = reader.ReadToEnd();
                        byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(requestParameterXml);
                        XPathDocument xml = new XPathDocument(new MemoryStream(bytes));
                        XslCompiledTransform xslt = new XslCompiledTransform();

                        string requestTypeGUID = "{" + requestTypeId.ToUpper() + "}";

                        if (requestTypeGUID == QUERY_COMPOSER)
                        {
                            using (Stream stream = typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.QueryComposer.xsl"))
                            {
                                using (XmlTextReader transform = new XmlTextReader(stream))
                                {
                                    using (var writer = new StringWriter())
                                    {
                                        xslt.Load(transform);
                                        xslt.Transform(xml, null, writer);
                                        if (doc.Filename == "ESPRequest.xml")
                                        {
                                            query = writer.ToString();
                                            //log.Debug(query);
                                        }
                                        else
                                        {
                                            demoQuery = writer.ToString();
                                            //log.Debug(demoQuery);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (Stream stream = typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.RequestBuilder.xsl"))
                            {
                                using (XmlTextReader transform = new XmlTextReader(stream))
                                {
                                    using (var writer = new StringWriter())
                                    {
                                        xslt.Load(transform);
                                        xslt.Transform(xml, null, writer);
                                        query = writer.ToString();
                                        //log.Debug(query);
                                    }
                                }
                            }
                        }
                    }
                    // For HQMF queries, file with this name is the query file. Ignore others.
                    if (useHQMF && doc.Filename == "ESPRequestHQMF.xml")
                    {
                        requestParameterXml = reader.ReadToEnd();
                        byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(requestParameterXml);
                        XPathDocument xml = new XPathDocument(new MemoryStream(bytes));
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        using (Stream stream = typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.HQMFtoIntermediate_v2.xsl"))
                        {
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
                if (viewSQL)
                {
                    status.Code = RequestStatus.StatusCode.InProgress;
                    DataTable dtSQL = new DataTable();
                    dtSQL.Columns.Add("SQL");
                    dtSQL.Rows.Add(query);
                    resultDataset.Reset();
                    resultDataset.Tables.Add(dtSQL);
                }
                else
                { 
                    if (Settings == null || Settings.Count == 0)
                        throw new Exception(CommonMessages.Exception_MissingSettings);

                    status.Code = RequestStatus.StatusCode.InProgress;

                    // PostgeSQL-style connection string
                    string server = GetSetting("Server", string.Empty);
                    string port = GetSetting("Port", string.Empty);
                    string userId = GetSetting("UserID", string.Empty);
                    string password = GetSetting("Password", string.Empty);
                    string database = GetSetting("Database", string.Empty);
                    string connectionTimout = GetSetting("ConnectionTimeout", "15");
                    string commandTimeout = GetSetting("CommandTimeout", "120");

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
                    
                    
                    using (NpgsqlConnection conn = new NpgsqlConnection(connstring))
                    {
                        try
                        {
                            conn.Open();

                            log.Debug((string.IsNullOrEmpty(demoQuery) ? "Query:" : "Numerator Query:") + Environment.NewLine + query);

                            NpgsqlCommand command = new NpgsqlCommand(query, conn);
                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                            resultDataset.Reset();
                            da.Fill(resultDataset);

                            hasCellCountAlert = CheckCellCountsInQueryResult(resultDataset, CellThreshHold);

                            if (!string.IsNullOrEmpty(demoQuery))
                            {
                                log.Debug("Denominator Query:" + Environment.NewLine + demoQuery);
                                NpgsqlCommand command2 = new NpgsqlCommand(demoQuery, conn);
                                NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(command2);
                                resultDemoDataset.Reset();
                                da2.Fill(resultDemoDataset);
                            }
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                if (hasCellCountAlert)
                {
                    status.Code = RequestStatus.StatusCode.CompleteWithMessage;
                    status.Message = "The query results have rows with low cell counts. You can choose to set the low cell count data to 0 by clicking the [Suppress Low Cells] button.";
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
                try
                {
                    status.Code = RequestStatus.StatusCode.Error;
                    status.Message = e.Message;
                    throw e;
                }
                catch
                {
                    throw new Exception("Please check model configuration: Incorrect database configuration settings.");
                }
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
            if(responseDocument != null && responseDocument.Length>0)
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
            // Embed the schema. The schema contains column header information in case no rows are returned.
            if (documentId == "0" || documentId == "2")
            {
                // XML file format
                var stream = new MemoryStream();
                if (documentId == "0")
                    resultDataset.WriteXml(stream, XmlWriteMode.WriteSchema);
                else
                    resultDemoDataset.WriteXml(stream, XmlWriteMode.WriteSchema);
                stream.Seek(0, SeekOrigin.Begin);
                contentStream = stream;
            }
            else if (documentId == "1" || documentId == "3")
            {
                // Json file format
                contentStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(documentId == "1" ? resultDataset : resultDemoDataset)));
            }
            else if (documentId == "4" && hasCellCountAlert)
            {
                contentStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(styleXml));
            }
            else
            {
                contentStream = null;
            }
        }


        public void Close(string requestId)
        {
        }

        public void PostProcess(string requestId)
        {
            ZeroLowCellCountsInQueryResult(resultDataset.Tables[0], CellThreshHold);
            status.Code = RequestStatus.StatusCode.Complete;
            status.Message = "";
            status.PostProcess = false;
        }
        #endregion

        #region Private Methods

        private Document BuildDocument(string id, DataSet ds, string name, bool viewable, bool xmlFormat)
        {
            Document document = new Document(id, "x-application/lpp-dns-table", name);
            document.IsViewable = viewable;
            string data;
            using (MemoryStream stream = new MemoryStream())
            {
                // Embed the schema. The schema contains column header information in case no rows are returned.
                if (xmlFormat)
                {
                    ds.WriteXml(stream, XmlWriteMode.WriteSchema);
                    stream.Seek(0, SeekOrigin.Begin);
                    data = new StreamReader(stream).ReadToEnd();
                }
                else 
                {
                    data = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                document.Size = data.Length;
            }
            return document;
        }

        private void BuildResponseDocuments()
        {
            if (responseDocument == null)
            {
                var documents = new List<Document>();
                if (resultDataset != null)
                {
                    documents.Add(BuildDocument("0", resultDataset, "ESPResponse.xml", true, true));
                    documents.Add(BuildDocument("1", resultDataset, "ESPResponse.json", false, false));
                }
                if (resultDemoDataset != null)
                {
                    documents.Add(BuildDocument("2", resultDemoDataset, "ESPDemographicResponse.xml", false, true));
                    documents.Add(BuildDocument("3", resultDemoDataset, "ESPDemographicResponse.json", false, false));
                }
                if (hasCellCountAlert)
                {
                    ViewableDocumentStyle style = new ViewableDocumentStyle
                    {
                        Css = ".StyledRows { background-color: yellow; color: red }",
                        StyledRows = FindLowCellCountsInQueryResult(resultDataset.Tables[0], CellThreshHold)
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
                    Document styleDoc = new Document("4", "application/xml", "ViewableDocumentStyle.xml");
                    styleDoc.IsViewable = false;
                    styleDoc.Size = styleXml.Length;
                    documents.Add(styleDoc);
                }
                responseDocument = documents.ToArray();
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

        string GetSetting(string key, string defaultValue)
        {
            if (Settings == null || !Settings.ContainsKey(key))
                return defaultValue;

            object value;
            if (!Settings.TryGetValue(key, out value))
                return defaultValue;

            if (value == null)
                return defaultValue;

            return value.ToString();
        }

        public static Boolean CheckCellCountsInQueryResult(DataSet ds, int ThresholdCount)
        {
            Boolean showAlert = false;

            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                string cellValue = dr["Patients"].ToString();
                double Num;
                bool isNum = double.TryParse(cellValue, out Num);
                if (isNum && Num > 0 && Num < Convert.ToDouble(ThresholdCount))
                {
                    showAlert = true;
                    break;
                }

            }

            return showAlert;
        }

        /// <summary>
        /// Set the low-cell counts in the query result to zero.
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="ThresholdCount"></param>
        public static void ZeroLowCellCountsInQueryResult(DataTable queryResult, int ThresholdCount)
        {
            DataTable dt = queryResult;
            
            foreach (DataRow dr in dt.Rows)
            {
                string cellValue = dr["Patients"].ToString();
                double Num;
                bool isNum = double.TryParse(cellValue, out Num);
                if (isNum)
                {
                    double dValue = Convert.ToDouble(dr["Patients"]);
                    if (dValue > 0 && dValue < Convert.ToDouble(ThresholdCount))
                    {
                        dr["Patients"] = 0;
                    }
                }
                
           }

        }

        /// <summary>
        /// Find low cell count rows
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="ThresholdCount"></param>
        public static int[] FindLowCellCountsInQueryResult(DataTable queryResult, int ThresholdCount)
        {
            DataTable dt = queryResult;
            List<int> zeroRows = new List<int>();

            foreach (DataRow dr in dt.Rows)
            {
                string cellValue = dr["Patients"].ToString();
                double Num;
                bool isNum = double.TryParse(cellValue, out Num);
                if (isNum && Num > 0 && Num < Convert.ToDouble(ThresholdCount))
                {
                    zeroRows.Add(dt.Rows.IndexOf(dr));
                }
            }

            return zeroRows.ToArray();

        }

        #endregion


    }

}
