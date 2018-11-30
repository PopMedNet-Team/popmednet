using Lpp.Dns.DataMart.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client.Lib
{
    public class DummyPatientIdentifierProcessor : IEarlyInitializeModelProcessor, IPatientIdentifierProcessor
    {
        IModelMetadata _modelMetaData;
        public DummyPatientIdentifierProcessor()
        {
            _modelMetaData = new Lpp.Dns.DataMart.Client.DomainManger.ProxyModelMetadata.EmptyModelMetaData();
        }

        bool IPatientIdentifierProcessor.CanGenerateLists => false;

        Guid IModelProcessor.ModelProcessorId => Guid.Empty;

        IModelMetadata IModelProcessor.ModelMetadata => _modelMetaData;

        IDictionary<string, object> IModelProcessor.Settings { get; set; }

        void IModelProcessor.AddResponseDocument(string requestId, string filePath)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.Close(string requestId)
        {
            throw new NotSupportedException();
        }

        void IPatientIdentifierProcessor.GenerateLists(Guid requestID, NetworkConnectionMetadata network, RequestMetadata md, IDictionary<Guid, string> outputPaths, string format)
        {
            throw new NotSupportedException();
        }

        IDictionary<Guid,string> IPatientIdentifierProcessor.GetQueryIdentifiers()
        {
            throw new NotSupportedException();
        }

        void IEarlyInitializeModelProcessor.Initialize(Guid modelID, DocumentWithStream[] documents)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.PostProcess(string requestId)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.RemoveResponseDocument(string requestId, string documentId)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.Request(string requestId, NetworkConnectionMetadata network, RequestMetadata request, Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.RequestDocument(string requestId, string documentId, Stream contentStream)
        {
            throw new NotSupportedException();
        }

        Document[] IModelProcessor.Response(string requestId)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.ResponseDocument(string requestId, string documentId, out Stream contentStream, int maxSize)
        {
            throw new NotSupportedException();
        }

        void IPatientIdentifierProcessor.SetPatientIdentifierSources(IDictionary<Guid, string> inputPaths)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.Start(string requestId, bool ViewSQL)
        {
            throw new NotSupportedException();
        }

        RequestStatus IModelProcessor.Status(string requestId)
        {
            throw new NotSupportedException();
        }

        void IModelProcessor.Stop(string requestId, StopReason reason)
        {
            throw new NotSupportedException();
        }
    }
}
