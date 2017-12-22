using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using log4net;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class FileDistributionModelProcessor : IModelProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PROCESSOR_ID = "C8BC0BD9-A50D-4B9C-9A25-472827C8640A";
        private IModelMetadata modelMetadata = new FileDistributionModelMetadata();
        
        [Serializable]
        internal class FileDistributionModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "00BF515F-6539-405B-A617-CA9F8AA12970";
            readonly IDictionary<string, bool> capabilities;
            readonly IDictionary<string, string> properties;

            public FileDistributionModelMetadata()
            {
                capabilities = new Dictionary<string, bool>() { { "IsSingleton", false }, { "RequiresConfig", false }, { "AddFiles", true }, { "CanRunAndUpload", false }, { "CanUploadWithoutRun", true } };
                properties = new Dictionary<string, string>() { };
            }

            public string ModelName
            {
                get { return "FileDistributionModelProcessor"; }
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
                get { return new List<Settings.ProcessorSetting>(); }
            }

            public IEnumerable<Settings.SQLProvider> SQlProviders
            {
                get { return Enumerable.Empty<Lpp.Dns.DataMart.Model.Settings.SQLProvider>(); }
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

        public class InternalDocument
        {
            public InternalDocument( Document Doc, string Path, long Size)
            {
                document = Doc;
                document.Size = Convert.ToInt32(Size);
                path = Path;
                size = Size;
            }
            public Document document { get; set; }
            public String path {get; set; }
            public long size { get; set; }
        }

        private System.Collections.Generic.List<InternalDocument> responseDocuments = new System.Collections.Generic.List<InternalDocument>();
        RequestStatus status = new RequestStatus(RequestStatus.StatusCode.Pending);

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
        }

        public void Request(string requestId, NetworkConnectionMetadata network, RequestMetadata md,
            Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments)
        {
            throw new NotImplementedException();
        }

        public void RequestDocument(string requestId, string documentId, Stream contentStream)
        {
            throw new NotImplementedException();
        }

        public void Start(string requestId, bool viewSQL)
        {
            throw new NotImplementedException();
        }

        public void Stop(string requestId, StopReason reason)
        {
            throw new NotImplementedException();
        }

        public RequestStatus Status(string requestId)
        {
            //return new RequestStatus(RequestStatus.StatusCode.Pending);
            return status;
        }

        public Document[] Response(string requestId)
        {
            return responseDocuments.Select(s => s.document).ToArray(); 
        }

        public void AddResponseDocument(string requestId, string filePath)
        {
            responseDocuments.Add( new InternalDocument(new Document( SASModelProcessor.NewGuid().ToString(), GetMimeType( filePath ), Path.GetFileName(filePath)), filePath, new FileInfo(filePath).Length));
            status.Code = RequestStatus.StatusCode.AwaitingResponseApproval;
        }

        public void RemoveResponseDocument(string requestId, string documentId)
        {
            if (responseDocuments != null )
            {
                List<InternalDocument> ToRemove = new List<InternalDocument>();
                foreach (InternalDocument doc in responseDocuments)
                {
                    if (doc.document.DocumentID == documentId)
                        ToRemove.Add(doc);
                }
                foreach (InternalDocument doc in ToRemove)
                    responseDocuments.Remove(doc);
                
            }

            status.Code = (responseDocuments == null || responseDocuments.Count == 0) ? RequestStatus.StatusCode.Pending : RequestStatus.StatusCode.AwaitingResponseApproval;
        }

        public void ResponseDocument(string requestId, string documentId, out Stream contentStream, int maxSize)
        {
            contentStream = responseDocuments
                .Where(s => s.document.DocumentID == documentId)
                .Select( f => new FileStream( f.path, FileMode.Open ) )
                .FirstOrDefault();
        }

        public void Close(string requestId)
        {
        }

        public void PostProcess(string requestId)
        {
           // throw new NotImplementedException();
        }

        /*
        public String DocumentView(Document document, Stream contentStream)
        {
            String view = string.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    String requestParameterXml = reader.ReadToEnd();
                    byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(requestParameterXml);
                    XPathDocument xml = new XPathDocument(new MemoryStream(bytes));
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    using (Stream stream = typeof(Lpp.Dns.DataMart.Model.FileDistributionModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ModularProgram.xsl"))
                    {
                        using (XmlTextReader transform = new XmlTextReader(new FileStream("ModularProgram.xsl", FileMode.Open, FileAccess.Read)))
                        //using (XmlTextReader transform = new XmlTextReader(stream))
                        {
                            using (var writer = new StringWriter())
                            {
                                xslt.Load(transform);
                                xslt.Transform(xml, null, writer);
                                view = writer.ToString();
                                log.Debug(view);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message, ex);
                throw ex;
            }
            return view;
        }
        */

        #endregion

        #region Private Methods
        private string GetMimeType(string fileName)
        {
            string mimeType = "application/octet-stream";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        #endregion
    }
}
