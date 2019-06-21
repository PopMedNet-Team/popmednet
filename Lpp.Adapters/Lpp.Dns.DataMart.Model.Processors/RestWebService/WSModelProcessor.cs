using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Model;
using System.ServiceModel;
using Lpp.Dns.DataMart.Model.Rest;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using log4net;
using System.Threading;
using Lpp.Dns.DataMart.Model.Settings;
using System.Data;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class WSModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PROCESSOR_ID = "55C48A42-B800-4A55-8134-309CC9954D4C";
        private const int BUFFER_SIZE = 10000;

        private IModelMetadata modelMetadata = new WSModelMetadata();
        private IDictionary<string, IDictionary<string, string>> requestProperties; // Key: requestId, Value: Request Properties
        private IDictionary<string, RequestStatus> requestStatuses = new Dictionary<string, RequestStatus>(); // Key: requestId, Value: Request Status

        [Serializable]
        internal class WSModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "F4EE0D81-DE74-4B3F-ADAC-8E75301407FD";
            private const int BUFFER_SIZE = 10000;

            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public WSModelMetadata()
            {
                capabilities = new Dictionary<string, bool>() { { "IsSingleton", true } };
                properties = new Dictionary<string, string>() { { "ServiceURL", "" } };
            }

            public string ModelName
            {
                get { return "WSModelProcessor"; }
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
                    List<Settings.ProcessorSetting> settings = new List<Settings.ProcessorSetting>();
                    settings.Add(new Settings.ProcessorSetting { Title = "Service URL", Key = "ServiceURL", DefaultValue = "", ValueType = typeof(string), Required = true });
                    return settings;
                }
            }

            public IEnumerable<Settings.SQLProvider> SQlProviders
            {
                get { return Enumerable.Empty<Settings.SQLProvider>(); }
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

        private Guid requestTypeId = Guid.Empty;
        private string requestText;
        private string responsesText;
        private RequestStatus status = new RequestStatus();
        private DataSet viewableResultDataset;
        private Document[] desiredDocuments;
        private Document[] responseDocument;
        private RequestStatus _status;

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
            if (this.requestProperties == null)
                this.requestProperties = new Dictionary<string, IDictionary<string, string>>();

            if (requestProperties == null)
                return;

            if (!this.requestProperties.ContainsKey(requestId))
                this.requestProperties.Add(requestId, requestProperties);
            else
                this.requestProperties[requestId] = requestProperties;
        }

        public void Request(string requestId, NetworkConnectionMetadata network, RequestMetadata md,
            Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments)
        {
            try
            {
                viewableResultDataset = new DataSet();

                this.requestTypeId = new Guid(md.RequestTypeId);
                requestProperties = null;
                desiredDocuments = requestDocuments;
                this.desiredDocuments = requestDocuments;
                status.Code = RequestStatus.StatusCode.InProgress;
                status.Message = "";
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                throw new ModelProcessorError(ex.Message, ex);
            }
        }

        public void RequestDocument(string requestId, string documentId, System.IO.Stream contentStream)
        {
            try
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var doc = desiredDocuments.FirstOrDefault(d => d.DocumentID == documentId);
                    requestText = reader.ReadToEnd();

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

        const string RESPONSE_DOCUMENTID = "5F58F341-5981-4D74-BCE2-46BE5BDCAAF8";

        public void Start(string requestId, bool viewSQL)
        {
            try
            {
                string serviceUrl = Settings.GetAsString("ServiceURL", "");


                var doc = desiredDocuments.FirstOrDefault();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl + "?requestID=" + requestId + "&data=" + requestText);

                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.ContentType = "application/xml";
                request.Method = "POST";
                var Data = request.GetRequestStream();
                byte[] buffer = Encoding.ASCII.GetBytes("");
                Data.Write(buffer, 0, buffer.Length);

                var responseText = request.GetResponse();
                Stream responseStream = responseText.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseFromServer = reader.ReadToEnd();
                responsesText = responseFromServer.ToString();
                var documents = new List<Document>();
                documents.Add(BuildDocument(RESPONSE_DOCUMENTID, responseFromServer, "Response.xml", true, true));
                responseDocument = documents.ToArray();
                status.Code = RequestStatus.StatusCode.Complete;

                
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                while (Status(requestId).Code == RequestStatus.StatusCode.InProgress)
                {
                    Thread.Sleep(10000);
                }

                if (Status(requestId).Code == RequestStatus.StatusCode.Error)
                    throw new ModelProcessorError(ex.Message, ex);
            }
        }

        public void Stop(string requestId, StopReason reason)
        {
            try
            {
                string serviceUrl = Settings.GetAsString("ServiceURL", "");

                if (requestProperties == null || !requestProperties.ContainsKey(requestId) || !requestProperties[requestId].ContainsKey("RequestToken"))
                    return;

                string requestToken = requestProperties[requestId]["RequestToken"];

                using (var factory = new WebChannelFactory<IModelProcessorRestService>(new Uri(serviceUrl)))
                {
                    var channel = factory.CreateChannel();
                    //channel.Stop(requestId, (Lpp.Dns.DataMart.Model.Rest.StopReason)Enum.ToObject(typeof(StopReason), (int) reason));
                    channel.Stop(requestToken, (Lpp.Dns.DataMart.Model.Rest.StopReason)Enum.ToObject(typeof(StopReason), (int)reason));
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                throw new ModelProcessorError(ex.Message, ex);
            }
        }

        public RequestStatus Status(string requestId)
        {
            return status;
        }

        public Document[] Response(string requestId)
        {
            try
            {
                    IList<Document> documents = new List<Document>();
                    foreach(Document doc in responseDocument)
                    {
                        documents.Add(doc);
                    }
                 
                    return documents.ToArray<Document>();
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                throw new ModelProcessorError(ex.Message, ex);
            }
        }

        public void AddResponseDocument(string requestId, string filePath)
        {
        }

        public void RemoveResponseDocument(string requestId, string filePath)
        {
        }

        public void ResponseDocument(string requestId, string documentId, out System.IO.Stream contentStream, int maxSize)
        {
            contentStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(responsesText));           
        }

        public void Close(string requestId)
        {
            throw new NotImplementedException();
        }

        public void PostProcess(string requestId)
        {
            throw new NotImplementedException();
        }

        #endregion

        public class DocumentEx
        {
            public DocumentEx()
            {
                ID = Guid.NewGuid();
            }

            public Guid ID { get; set; }
            public Document Document { get; set; }
            public byte[] Content { get; set; }
            public FileInfo FileInfo { get; set; }
        }

        private Document BuildDocument(string id, string ds, string name, bool viewable, bool xmlFormat)
        {
            Document document = new Document(id, "x-application/lpp-dns-table", name);
            document.IsViewable = viewable;
            string data;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                data = new StreamReader(stream).ReadToEnd();
               
                document.Size = data.Length;
            }
            return document;
        }
    }
}
