using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;

namespace Lpp.Dns.DataMart.Model.Rest
{
    public class SampleModelProcessorRestService : IModelProcessorRestService        
    {
        private static Document[] requestDocuments;

        //public string EchoWithGet(string s)
        //{
        //    return "You said " + s;
        //}

        //public string EchoWithPost(string s)
        //{
        //    return "You said " + s;
        //}

        //public IList<string> XMLData(string id)
        //{
        //    IList<string> urls = new List<string>();
        //    urls.Add("http://www.apple.com");
        //    urls.Add("http://www.hp.com");
        //    urls.Add("http://www.wigitek.com");
        //    return urls;
        //}

        public string JSONData(string id)
        {
            return "You requested product " + id;
        }

        public string Request(string requestId, Document[] requestDocuments, string requestTypeId, IDictionary<string, object> settings, out string[] desiredDocuments)
        {
            IList<Document> requiredDocumentList = new List<Document>();
            IList<string> desiredDocumentIds = new List<string>();
            //foreach (Document requestDocument in requestDocuments)
            //{
                string desiredDocumentId;
                Document requiredDocument = new Document();
                //requiredDocument.DocumentId = desiredDocumentId = requestDocument.DocumentId;
                //requiredDocument.Filename = "Desired-" + requestDocument.Filename;
                //requiredDocument.Viewable = requestDocument.Viewable;
                //requiredDocument.MimeType = requestDocument.MimeType;
                //requiredDocument.Size = requestDocument.Size;
                requiredDocument.DocumentId = desiredDocumentId = requestDocuments[0].DocumentId;
                requiredDocument.Filename = "Desired-" + requestDocuments[0].Filename;
                requiredDocument.Viewable = requestDocuments[0].Viewable;
                requiredDocument.MimeType = requestDocuments[0].MimeType;
                requiredDocument.Size = requestDocuments[0].Size;
                desiredDocumentIds.Add(desiredDocumentId);
                requiredDocumentList.Add(requiredDocument);
            //}
            desiredDocuments = desiredDocumentIds.ToArray<string>();
            SampleModelProcessorRestService.requestDocuments = requiredDocumentList.ToArray<Document>();
            //return requestId;
            return "4f3ab08a4f85cf5bdb000019";
        }

        //public void RequestDocumentStream(string requestId, string documentId, Stream inStream)
        //{
        //    Document document = requestDocuments[Convert.ToInt32(documentId)];
        //    FileStream outStream = new FileStream(@"C:\Users\daniel\tmp\" + document.Filename, FileMode.Create);
        //    //FileStream outStream = new FileStream(@"C:\Users\daniel\tmp\UploadSequenceDiagram.jpg", FileMode.Create);

        //    int bytesRead = 0;
        //    byte[] buffer = new byte[5000];
        //    while ((bytesRead = inStream.Read(buffer, 0, 5000)) != 0)
        //    {
        //        outStream.Write(buffer, 0, 5000);
        //    }
        //    inStream.Close();
        //    outStream.Close();
        //}

        public void RequestDocument(string requestId, string documentId, string offset, byte[] data)
        {
            FileStream outStream = null;
            try
            {
                Document document = requestDocuments[Convert.ToInt32(documentId)];
                outStream = new FileStream(@"C:\Users\daniel\tmp\" + document.Filename, FileMode.OpenOrCreate);
                outStream.Seek(Convert.ToInt32(offset), SeekOrigin.Begin);
                outStream.Write(data, 0, data.Length);
            }
            catch (Exception)
            {
                
            }
            finally
            {
                if(outStream != null)
                    outStream.Close();
            }
        }

        public void Start(string requestId)
        {
        }

        public void Stop(string requestId, StopReason reason)
        {
        }

        public RequestStatus Status(string requestId)
        {
            RequestStatus status = new RequestStatus();
            status.Code = StatusCode.Complete;
            status.Message = "Done. Wancheng. Finito.";
            return status;
        }

        public Document[] Response(string requestId)
        {
            IList<Document> desiredDocumentList = new List<Document>();
            int i = 0;
            foreach (Document requestDocument in requestDocuments)
            {
                Document desiredDocument = new Document();
                desiredDocument.DocumentId = requestDocument.DocumentId;
                desiredDocument.Filename = requestDocument.Filename;
                desiredDocument.Viewable = requestDocument.Viewable;
                desiredDocument.MimeType = requestDocument.MimeType;
                desiredDocument.Size = requestDocument.Size;
                desiredDocumentList.Add(desiredDocument);
                i++;
            }

            return desiredDocumentList.ToArray<Document>();
        }

        //public Stream ResponseDocumentStream(string requestId, string documentId)
        //{
        //    var range = WebOperationContext.Current.IncomingRequest.Headers.Get("Range");
        //    var userAgent = WebOperationContext.Current.IncomingRequest.UserAgent;
        //    return null;
        //}

        public int ResponseDocument(string requestId, string documentId, string offset, out byte[] data)
        {
            data = new byte[5000];
            FileStream inStream = null;
            try
            {
                Document document = requestDocuments[Convert.ToInt32(documentId)];
                inStream = new FileStream(@"C:\Users\daniel\tmp\" + document.Filename, FileMode.Open);
                inStream.Seek(Convert.ToInt32(offset), SeekOrigin.Begin);
                return inStream.Read(data, 0, data.Length);
            }
            catch (Exception)
            {
                
            }
            finally
            {
                if(inStream != null)
                    inStream.Close();
            }

            return 0;
        }

        public void Close(string requestId)
        {
        }

        public IDictionary<string, string> Properties()
        {
            return null;
        }

        public IDictionary<string, bool> Capabilities()
        {
            return null;
        }
    }

}
