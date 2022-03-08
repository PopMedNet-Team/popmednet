using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using log4net;
using Lpp.Dns.DataMart.Model.Settings;
using Lpp.QueryComposer;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class QueryComposerModelMetadata : IModelMetadata
    {
        static readonly Guid MODEL_ID = new Guid("455C772A-DF9B-4C6B-A6B0-D4FD4DD98488");

        public static readonly Guid ESPModelID = new Guid("7C69584A-5602-4FC0-9F3F-A27F329B1113");
        public static readonly Guid ModularProgramModelID = new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154");
        public static readonly Guid SummaryTableModelID = new Guid("CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB");
        public static readonly Guid PCORIID = new Guid("85EE982E-F017-4BC4-9ACD-EE6EE55D2446");
        public static readonly Guid DataCheckerID = new Guid("321ADAA1-A350-4DD0-93DE-5DE658A507DF");
        public static readonly Guid DistributedRegressionID = new Guid("4C8A25DC-6816-4202-88F4-6D17E72A43BC");

        readonly List<Settings.SQLProvider> sqlProviders;
        readonly IDictionary<string, bool> capabilities;
        readonly IDictionary<string, string> properties;
        readonly List<Lpp.Dns.DataMart.Model.Settings.ProcessorSetting> settings;

        Guid? _modelID = null;

        public QueryComposerModelMetadata()
        {
            capabilities = new Dictionary<string, bool>() { { "IsSingleton", false }, { "CanViewSQL", true }, { "CanRunAndUpload", true }, { "CanUploadWithoutRun", true }, { "RequiresConfig", true }, { "AddFiles", false } };

            settings = new List<Settings.ProcessorSetting>();

            sqlProviders = new List<SQLProvider>();
            properties = new Dictionary<string, string>();
            Model.Settings.ProcessorSettings.AddDbSettingKeys(properties, sqlProviders);
        }

        public string ModelName
        {
            get { return "QueryComposerModelProcessor"; }
        }

        public Guid ModelId
        {
            get { return MODEL_ID; }
        }

        public string Version
        {
            get { return "0.1"; }
        }

        public void SetModel(Guid? modelID)
        {
            _modelID = modelID;

            settings.Clear();
            sqlProviders.Clear();
            capabilities.Clear();
            properties.Clear();
            

            if(modelID.HasValue == false || modelID.Value == ModularProgramModelID)
            {
                return;
            }

            if(modelID.Value == DataCheckerID || modelID.Value == SummaryTableModelID)
            {
                sqlProviders.AddRange(new[] { Model.Settings.SQLProvider.SQLServer });

            }
            else if (modelID.Value == ESPModelID)
            {
                sqlProviders.AddRange(new[] { Model.Settings.SQLProvider.SQLServer, Model.Settings.SQLProvider.PostgreSQL });
            }
            else if (modelID.Value == PCORIID)
            {
                sqlProviders.AddRange(new[] { Model.Settings.SQLProvider.SQLServer, Model.Settings.SQLProvider.PostgreSQL, Model.Settings.SQLProvider.Oracle });
            }

            if (modelID.Value != DistributedRegressionID)
            {
                settings.AddRange(Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.CreateDbSettings(SQlProviders));
                settings.Add(
                new ProcessorSetting
                {
                    Title = "Low Threshold Value",
                    Key = "LowThresholdValue",
                    DefaultValue = string.Empty,
                    Required = false,
                    ValueType = typeof(double)
                });
            }

            if (modelID.Value == DistributedRegressionID)
            {
                settings.Add(new ProcessorSetting
                {
                    Title = "Monitor Folder",
                    Key = "MonitorFolder",
                    DefaultValue = string.Empty,
                    Required = true,
                    ValueType = typeof(string)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Trigger filename: Successful Initialization",
                    Key = "SuccessfulInitializationFilename",
                    DefaultValue = "job_started.ok",
                    Required = true,
                    ValueType = typeof(string)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Trigger filename: Execution Complete",
                    Key = "ExecutionCompleteFilename",
                    DefaultValue = "files_done.ok",
                    Required = true,
                    ValueType = typeof(string)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Trigger filename: Execution Fail",
                    Key = "ExecutionFailFilename",
                    DefaultValue = "job_fail.ok",
                    Required = true,
                    ValueType = typeof(string)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Trigger filename: Execution Stop",
                    Key = "ExecutionStopFilename",
                    DefaultValue = "job_done.ok",
                    Required = true,
                    ValueType = typeof(string)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Output Manifest Filename",
                    Key = "ManifestFilename",
                    DefaultValue = "file_list.csv",
                    Required = true,
                    ValueType = typeof(string)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Maximum Monitor Time (hours)",
                    Key = "MaxMonitorTime",
                    DefaultValue = "12",
                    Required = true,
                    ValueType = typeof(int)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Maximum Read Attempt Time (minutes)",
                    Key = "MaxReadWaitTime",
                    DefaultValue = "5",
                    Required = true,
                    ValueType = typeof(int)
                });
                settings.Add(new ProcessorSetting
                {
                    Title = "Monitoring Frequency (seconds)",
                    Key = "MonitoringFrequency",
                    DefaultValue = "5",
                    Required = true,
                    ValueType = typeof(decimal)
                });
            }

            

            if (modelID.Value == PCORIID)
            {
                settings.Add(
                    new ProcessorSetting
                    {
                        Title = "PCORnet Schema",
                        Key = "DatabaseSchema",
                        DefaultValue = string.Empty,
                        Required = false,
                        ValueType = typeof(string)
                    }
                );
            }
            
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

    [Serializable]
    public class QueryComposerModelProcessor : IEarlyInitializeModelProcessor, IPatientIdentifierProcessor
    {
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Guid PROCESSOR_ID = new Guid("AE0DA7B0-0F73-4D06-B70B-922032B7F0EB");

        readonly QueryComposerModelMetadata modelMetadata = new QueryComposerModelMetadata();

        public Guid ModelProcessorId
        {
            get { return PROCESSOR_ID; }
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

        bool IPatientIdentifierProcessor.CanGenerateLists
        {
            get
            {
                if (ModelMetadata.Capabilities.ContainsKey("CanGeneratePatientIdentifierLists"))
                {
                    return modelMetadata.Capabilities["CanGeneratePatientIdentifierLists"];
                }

                return false;
            }
        }

        string requestTypeId = null;
        RequestStatus status = new RequestStatus();

        Model.DocumentWithStream[] _drDocuments;
        Document[] _requestDocuments;
        Document[] _desiredDocuments;
        List<DocumentEx> _responseDocuments = new List<DocumentEx>();

        //have a specific document ID for sql responses to keep it separate from proper response documents.
        static readonly Guid SqlResponseDocumentID = new Guid("D8FAAC11-9D5B-4337-AC28-D75071D57372");
        DocumentEx _SqlResponseDocument = null;

        DTO.QueryComposer.QueryComposerRequestDTO _request = null;
        DTO.QueryComposer.QueryComposerResponseDTO _currentResponse = null;

        Guid ModelID = Guid.Empty;        
        bool IsFileDistributionRequest = false;
        bool IsDistributedRegressionRequest = false;
        string _requestDetails = string.Empty;
        RequestMetadata _requestMetadata = null;

        public void Initialize(Guid modelID, Model.DocumentWithStream[] documents)
        {
            ModelID = modelID;
            modelMetadata.SetModel(ModelID);
            if (modelID == QueryComposerModelMetadata.DistributedRegressionID)
            {
                _drDocuments = documents;
                IsDistributedRegressionRequest = true;
                ModelID = QueryComposerModelMetadata.DistributedRegressionID;
            }
            else if (documents != null && documents.Any())
            {

                _requestDocuments = documents.Select(d => d.Document).ToArray();

                IEnumerable<DocumentWithStream> requestDocuments = documents.Where(d => string.Equals(d.Document.Kind, DTO.Enums.DocumentKind.Request, StringComparison.OrdinalIgnoreCase)).ToArray();
                _desiredDocuments = requestDocuments.Select(d => d.Document).ToArray();

                DocumentWithStream requestJson = documents.Where(d => string.Equals(d.Document.Kind, DTO.Enums.DocumentKind.Request, StringComparison.OrdinalIgnoreCase) && string.Equals(d.Document.Filename, "request.json", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (requestJson != null)
                {

                    try
                    {
                        using (StreamReader reader = new StreamReader(requestJson.Stream))
                        {
                            string json = reader.ReadToEnd();
                            log.Debug("Request json:" + Environment.NewLine + json);

                            _request = Lpp.Dns.DTO.QueryComposer.QueryComposerDTOHelpers.DeserializeRequest(json);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug(ex.Message, ex);
                        status.Code = RequestStatus.StatusCode.Error;
                        status.Message = ex.Message;
                        throw ex;
                    }

                    //inspect for modular or file dist. terms to determine type of request regardless of specified model for datamart
                    var allTerms = Lpp.Dns.DTO.QueryComposer.QueryComposerDTOHelpers.FlattenToTerms(_request).Where(t => t != null && (t.Type == ModelTermsFactory.ModularProgramID || t.Type == ModelTermsFactory.FileUploadID));
                    if (allTerms.Any())
                    {

                        IsFileDistributionRequest = true;
                        ModelID = QueryComposerModelMetadata.ModularProgramModelID;
                    }
                }
                else
                {
                    log.Debug("No request.json found, assuming the request is for Modular Program/File Distribution.");
                    IsFileDistributionRequest = true;
                }
            }
            else
            {
                IsFileDistributionRequest = true;
            }

            //the criteria is only required to determine the sub adapter type based on the query type. The ability to run and view sql is true by default for the model adapters.
            using (QueryComposer.IModelAdapter adapter = GetModelAdapter(null))
            {
                log.Debug(string.Format("Updating processor metadata based on the selected model adapter: CanViewSQL = {0}, CanRunAndUpload = {1}, CanUploadWithoutRun = {2}, AddFiles = {3}", adapter.CanViewSQL, adapter.CanRunAndUpload, adapter.CanUploadWithoutRun, adapter.CanAddResponseFiles));

                modelMetadata.Capabilities["CanViewSQL"] = adapter.CanViewSQL;
                modelMetadata.Capabilities["CanRunAndUpload"] = adapter.CanRunAndUpload;
                modelMetadata.Capabilities["CanUploadWithoutRun"] = adapter.CanUploadWithoutRun;
                modelMetadata.Capabilities["AddFiles"] = adapter.CanAddResponseFiles;
                modelMetadata.Capabilities["IsFileDistributionRequest"] = IsFileDistributionRequest;
                modelMetadata.Capabilities["IsDistributedRegressionRequest"] = IsDistributedRegressionRequest;
                modelMetadata.Capabilities["CanGeneratePatientIdentifierLists"] = adapter.CanGeneratePatientIdentifierLists;
            }
        }

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
        }

        public void Request(string requestId, NetworkConnectionMetadata network, RequestMetadata md, Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments)
        {
            _requestMetadata = md;
            this.requestTypeId = md.RequestTypeId;
            if (Settings.ContainsKey("MSRequestID"))
            {
                Settings["MSRequestID"] = md.MSRequestID;
            }
            else
            {
                Settings.Add("MSRequestID", md.MSRequestID);
            }
            requestProperties = null;
            desiredDocuments = Array.Empty<Document>();
            status.Code = RequestStatus.StatusCode.InProgress;
            status.Message = "";
        }

        public void RequestDocument(string requestId, string documentId, Stream contentStream)
        {
        }        

        public void Start(string requestId, bool viewSQL = false)
        {

            if (_request == null)
                throw new NullReferenceException("The deserialized request is null, please make sure that model processor is properly initialized and that the request document is not null.");

            var queryResults = new List<DTO.QueryComposer.QueryComposerResponseQueryResultDTO>();
            var queryCanPostProcess = new List<Tuple<Guid, bool, string>>();

            _request.SyncHeaders();

            foreach(var query in _request.Queries)
            {
                using(QueryComposer.IModelAdapter adapter = GetModelAdapter(query))
                {
                    adapter.Initialize(Settings, requestId);

                    if (viewSQL && !adapter.CanViewSQL)
                    {
                        queryResults.Add(
                            new DTO.QueryComposer.QueryComposerResponseQueryResultDTO {
                                ID = query.Header.ID,
                                QueryStart = DateTimeOffset.UtcNow,
                                QueryEnd = DateTimeOffset.UtcNow,
                                Errors = new[] {
                                    new DTO.QueryComposer.QueryComposerResponseErrorDTO{
                                        QueryID = query.Header.ID,
                                        Code = "-1",
                                        Description = $"The adapter \"{ adapter.GetType().FullName }\" does not support providing the SQL of a query."
                                    }
                                }
                            }
                        );

                        queryCanPostProcess.Add(new Tuple<Guid, bool, string>(query.Header.ID, false, string.Empty));

                        continue;
                    }                    

                    if (IsDistributedRegressionRequest)
                    {
                        var drAdapter = adapter as QueryComposer.Adapters.DistributedRegression.DistributedRegressionModelAdapter;
                        var queryResult = new DTO.QueryComposer.QueryComposerResponseQueryResultDTO { ID = query.Header.ID, QueryStart = DateTimeOffset.UtcNow };

                        try
                        {
                            DocumentEx[] outputDocuments = drAdapter.StartRequest(_drDocuments).ToArray();
                            if (outputDocuments != null && outputDocuments.Length > 0)
                            {
                                _responseDocuments.AddRange(outputDocuments);
                            }
                        }
                        catch (Exception ex)
                        {
                            queryResult.Errors = new[] {
                                new DTO.QueryComposer.QueryComposerResponseErrorDTO{
                                    QueryID = query.Header.ID,
                                    Code = "-1",
                                    Description = QueryComposer.ExceptionHelpers.UnwindException(ex)
                                }
                            };
                        }

                        queryResult.QueryEnd = DateTimeOffset.UtcNow;

                        queryResults.Add(queryResult);
                        queryCanPostProcess.Add(new Tuple<Guid, bool, string>(query.Header.ID, false, string.Empty));

                        continue;
                    }

                    DateTimeOffset queryStart = DateTimeOffset.UtcNow;
                    try
                    {
                        foreach(var result in adapter.Execute(query, viewSQL))
                        {
                            queryResults.Add(result);
                            if (adapter.CanPostProcess(result, out string postProcessMessage))
                            {
                                queryCanPostProcess.Add(new Tuple<Guid, bool, string>(query.Header.ID, true, postProcessMessage));
                            }
                            else
                            {
                                queryCanPostProcess.Add(new Tuple<Guid, bool, string>(query.Header.ID, false, string.Empty));
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        queryResults.Add(
                            new DTO.QueryComposer.QueryComposerResponseQueryResultDTO {
                                ID = query.Header.ID,
                                QueryStart = queryStart,
                                QueryEnd = DateTimeOffset.UtcNow,
                                Errors = new[] {
                                    new DTO.QueryComposer.QueryComposerResponseErrorDTO{
                                        QueryID = query.Header.ID,
                                        Code = "-1",
                                        Description = QueryComposer.ExceptionHelpers.UnwindException(ex)
                                    }
                                }
                            }
                        );
                        queryCanPostProcess.Add(new Tuple<Guid, bool, string>(query.Header.ID, false, string.Empty));
                    }


                }//end of adapter using
            }//end of processing each query


            var response = new DTO.QueryComposer.QueryComposerResponseDTO {
                Header = new DTO.QueryComposer.QueryComposerResponseHeaderDTO {
                    DocumentID = viewSQL ? SqlResponseDocumentID : QueryComposerModelProcessor.NewGuid(),
                    RequestID = _request.Header.ID
                },
                Queries = queryResults
            };

            response.RefreshQueryDates();
            response.RefreshErrors();

            string serializedResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.None);
            byte[] resultContent = System.Text.Encoding.UTF8.GetBytes(serializedResponse);

            Document resultDocument = new Document(response.Header.DocumentID.Value.ToString("D"), "application/json", "response.json");
            resultDocument.Size = resultContent.Length;
            resultDocument.IsViewable = true;
            resultDocument.Kind = "MultiQuery.JSON";

            if (viewSQL)
            {
                _SqlResponseDocument = new DocumentEx { ID = SqlResponseDocumentID, Document = resultDocument, Content = resultContent };
            }
            else
            {
                _SqlResponseDocument = null;
                _responseDocuments.Clear();
                _responseDocuments.Add(new DocumentEx { ID = response.Header.DocumentID.Value, Document = resultDocument, Content = resultContent });
                _currentResponse = response;
            }

            status.PostProcess = queryCanPostProcess.Any(q => q.Item2 == true);
            status.Message = queryCanPostProcess.Where(q => !string.IsNullOrEmpty(q.Item3)).Any() ? string.Join("\r\n", queryCanPostProcess.Where(q => !string.IsNullOrEmpty(q.Item3)).Select(q => q.Item3).ToArray()) : string.Empty;
            status.Code = (response.Errors != null && response.Errors.Any()) ? RequestStatus.StatusCode.Error : (string.IsNullOrEmpty(status.Message) ? RequestStatus.StatusCode.Complete : RequestStatus.StatusCode.CompleteWithMessage);          
        }

        QueryComposer.IModelAdapter GetModelAdapter(DTO.QueryComposer.QueryComposerQueryDTO query, bool requireRequestDTO = false)
        {
            if (IsFileDistributionRequest)
            {
                return new QueryComposer.Adapters.ModularProgram.ModularProgramModelAdapter(_requestMetadata);
            }

            if (IsDistributedRegressionRequest)
            {
                return new QueryComposer.Adapters.DistributedRegression.DistributedRegressionModelAdapter(_requestMetadata);
            }

            if (ModelID == QueryComposerModelMetadata.SummaryTableModelID)
            {
                if ((requireRequestDTO == false) && (_request == null || query == null))
                {
                    return new QueryComposer.Adapters.SummaryQuery.DummyModelAdapter(_requestMetadata);
                }

                if (!query.Header.QueryType.HasValue)
                    throw new Exception($"Unable to determine the Summary Query Type supported by the adapter required by query \"{ query.Header.Name }\". Invalid request JSON.");

                switch (query.Header.QueryType.Value)
                {
                    case Dns.DTO.Enums.QueryComposerQueryTypes.SummaryTable_Incidence:
                        return new QueryComposer.Adapters.SummaryQuery.IncidenceModelAdapter(_requestMetadata);
                    case Dns.DTO.Enums.QueryComposerQueryTypes.SummaryTable_MFU:
                        return new QueryComposer.Adapters.SummaryQuery.MostFrequentlyUsedQueriesModelAdapter(_requestMetadata);
                    case Dns.DTO.Enums.QueryComposerQueryTypes.SummaryTable_Prevalence:
                        return new QueryComposer.Adapters.SummaryQuery.PrevalenceModelAdapter(_requestMetadata);
                    case DTO.Enums.QueryComposerQueryTypes.Sql:
                        return new QueryComposer.Adapters.SummaryQuery.SqlDistributionAdapter(_requestMetadata);
                    default:
                        throw new Exception($"Cannot determine model query adapter:{ query.Header.QueryType.Value} for query \"{ query.Header.Name }\".");
                }
                
            }


            var adaptersTypes = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                 from t in GetLoadableTypes(a).Where(i => i.FullName.StartsWith("System.") == false)
                                 let interfaces = t.GetInterfaces().DefaultIfEmpty()
                                 where interfaces.Any(i => i == typeof(QueryComposer.IModelAdapter))
                                 && !t.IsInterface
                                 select t).ToArray();

            foreach (Type type in adaptersTypes.Where(t => t.GetConstructor(Type.EmptyTypes) != null))
            {
                var adapter = (QueryComposer.IModelAdapter)Activator.CreateInstance(type);

                if (adapter.ModelID == ModelID)
                    return adapter;
            }

            foreach (Type type in adaptersTypes.Where(t => t.GetConstructor(new[] { typeof(RequestMetadata) }) != null))
            {
                var adapter = (QueryComposer.IModelAdapter)Activator.CreateInstance(type, _requestMetadata);

                if (adapter.ModelID == ModelID)
                {
                    return adapter;
                }
            }

            throw new Exception("Model adapter for modelID: " + ModelID + " not found.");

        }

        static IEnumerable<Type> GetLoadableTypes(System.Reflection.Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (System.Reflection.ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public void PostProcess(string requestId)
        {
            if (_currentResponse == null)
                return;

            foreach(var queryResult in _currentResponse.Queries.Where(qr => qr.Errors == null || qr.Errors.Any() == false))
            {
                var query = _request.Queries.FirstOrDefault(q => q.Header.ID == queryResult.ID);
                if (query == null)
                    continue;

                using(var adapter = GetModelAdapter(query, requireRequestDTO: true))
                {
                    adapter.Initialize(Settings, requestId);

                    if (!adapter.CanPostProcess(queryResult, out string message))
                    {
                        continue;
                    }

                    queryResult.PostProcessStart = DateTimeOffset.UtcNow;
                    adapter.PostProcess(queryResult);
                    queryResult.PostProcessEnd = DateTimeOffset.UtcNow;
                }

            }

            _currentResponse.RefreshQueryDates();
            _currentResponse.RefreshErrors();

            string serializedResponse = Newtonsoft.Json.JsonConvert.SerializeObject(_currentResponse, Newtonsoft.Json.Formatting.None);
            byte[] resultContent = System.Text.Encoding.UTF8.GetBytes(serializedResponse);

            Document resultDocument = new Document(_currentResponse.Header.DocumentID.Value.ToString("D"), "application/json", "response.json");
            resultDocument.Size = resultContent.Length;
            resultDocument.IsViewable = true;
            resultDocument.Kind = "MultiQuery.JSON";
            _responseDocuments.Clear();
            _responseDocuments.Add(new DocumentEx { ID = _currentResponse.Header.DocumentID.Value, Document = resultDocument, Content = resultContent });

            status.Code = RequestStatus.StatusCode.Complete;
            status.Message = string.Empty;
        }

        public void Stop(string requestId, StopReason reason)
        {
            //not ever called
        }

        public RequestStatus Status(string requestId)
        {
            return status;
        }

        public Document[] Response(string requestId)
        {
            //returns any available response documents metadata.
            if (_SqlResponseDocument != null)
            {
                //if the sql response document exists return it instead of any existing result documents.
                //the sql response document gets cleared when a proper execution is done.
                return new[] { _SqlResponseDocument.Document };
            }

            return _responseDocuments.Select(d => d.Document).ToArray();
        }

        public void AddResponseDocument(string requestId, string filePath)
        {
            if (!_responseDocuments.Any(d => d.FileInfo != null && string.Equals(d.FileInfo.FullName, filePath, StringComparison.OrdinalIgnoreCase)))
            {
                log.Debug("Add response document: " + filePath);

                FileInfo file = new FileInfo(filePath);
                Guid id = QueryComposerModelProcessor.NewGuid();
                
                Document document = new Document(id.ToString("D"), GetMimeType(filePath), file.Name);
                document.Size = Convert.ToInt32(file.Length);

                _responseDocuments.Add(new DocumentEx { ID = id, Document = document, FileInfo = file });
            }

            if(status.Code == RequestStatus.StatusCode.Pending)
                status.Code = RequestStatus.StatusCode.AwaitingResponseApproval;
        }

        public void RemoveResponseDocument(string requestId, string documentId)
        {
            Guid id;
            if (!Guid.TryParse(documentId, out id))
            {
                log.Debug("Unable to remove response document, unable to parse document id:" + documentId);
                return;
            }

            log.Debug("Removing response document id:" + documentId);
            _responseDocuments.RemoveAll(d => d.ID == id);

            if (_responseDocuments.Count == 0)
                status.Code = RequestStatus.StatusCode.Pending;
        }

        public void ResponseDocument(string requestId, string documentId, out Stream contentStream, int maxSize)
        {
            contentStream = null;

            Guid id;
            if (!Guid.TryParse(documentId, out id))
            {
                log.Debug("Unable to set the content stream for response document, unable to parse document id:" + documentId);
                return;
            }                

            DocumentEx document = id == SqlResponseDocumentID ? _SqlResponseDocument : _responseDocuments.FirstOrDefault(d => d.ID == id);
            if (document != null)
            {
                if (document.Content != null)
                {
                    contentStream = new System.IO.MemoryStream(document.Content);
                }
                else if (document.FileInfo != null)
                {
                    contentStream = document.FileInfo.OpenRead();
                }

                if (id == SqlResponseDocumentID)
                {
                    //clear the document after the content has been returned to allow for the proper documents to be returned on subsequent requests.                    
                    _SqlResponseDocument = null;
                }
            }
            else
            {
                log.Debug("Unable to set content stream based on response document id:" + documentId + ". Document not found.");
            }
        }

        public void Close(string requestId)
        {
        }

        IDictionary<Guid, string> IPatientIdentifierProcessor.GetQueryIdentifiers()
        {
            return _request.Queries.ToDictionary(q => q.Header.ID, q => q.Header.Name);
        }

        void IPatientIdentifierProcessor.GenerateLists(Guid requestID, NetworkConnectionMetadata network, RequestMetadata md, IDictionary<Guid, string> outputPaths, string format)
        {
            using (QueryComposer.IModelAdapter adapter = GetModelAdapter( _request.Queries.First(), true))
            {
                if (Settings.ContainsKey("MSRequestID"))
                {
                    Settings["MSRequestID"] = md.MSRequestID;
                }
                else
                {
                    Settings.Add("MSRequestID", md.MSRequestID);
                }

                adapter.Initialize(Settings, requestID.ToString());
                adapter.GeneratePatientIdentifierLists(_request, outputPaths, format);
            }
        }

        void IPatientIdentifierProcessor.SetPatientIdentifierSources(IDictionary<Guid, string> inputPaths)
        {
            //TODO: accept the input paths and store the identifiers, to be used when the adapter queries are run.
            throw new NotImplementedException();
        }

        public class DocumentEx
        {
            public DocumentEx()
            {
                ID = QueryComposerModelProcessor.NewGuid();
            }

            public Guid ID { get; set; }
            public Document Document { get; set; }
            public byte[] Content { get; set; }
            public FileInfo FileInfo { get; set; }
        }

        internal static string GetMimeType(string fileName)
        {
            string mimeType = "application/octet-stream";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        /// Gets a new sequential GUID that can be stored in a primary key
        /// </summary>
        /// <returns></returns>
        internal static Guid NewGuid()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }

    }

}
