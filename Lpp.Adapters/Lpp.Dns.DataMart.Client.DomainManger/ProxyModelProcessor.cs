using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Client.DomainManger
{
    [Serializable]
    public class ProxyModelProcessor : MarshalByRefObject, Lpp.Dns.DataMart.Model.IModelProcessor
    {
        Lpp.Dns.DataMart.Model.IModelProcessor _processor;
        Model.IModelMetadata _modelMetadata;
        Type _processorType;

        public ProxyModelProcessor()
        {
        }

        public ProxyModelProcessor(Lpp.Dns.DataMart.Model.IModelProcessor processor)
        {
            _processor = processor;
            _processorType = processor.GetType();
            _modelMetadata = new ProxyModelMetadata(processor.ModelMetadata);
        }

        public override object InitializeLifetimeService()
        {
            //Disable lifetime management for this object, it will be explictly cleaned up when the DomainManager is disposed.
            //This is to prevent issues with the remote object being disconnected after a period of inactivity by the user.
            return null;
        }

        public Guid ModelProcessorId
        {
            get 
            {
                return _processor.ModelProcessorId; 
            }
        }

        public Model.IModelMetadata ModelMetadata
        {
            get 
            {
                return _modelMetadata;
            }
        }

        public IDictionary<string, object> Settings
        {
            get
            {
                return _processor.Settings;
            }
            set
            {
                _processor.Settings = value;
            }
        }

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
            _processor.SetRequestProperties(requestId, requestProperties);
        }

        public void Request(string requestId, Model.NetworkConnectionMetadata network, Model.RequestMetadata request, Model.Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Model.Document[] desiredDocuments)
        {
            _processor.Request(requestId, network, request, requestDocuments, out requestProperties, out desiredDocuments);
        }

        public void RequestDocument(string requestId, string documentId, System.IO.Stream contentStream)
        {
            _processor.RequestDocument(requestId, documentId, contentStream);
        }

        public void Start(string requestId, bool viewSQL)
        {
            _processor.Start(requestId, viewSQL);
        }

        public void Stop(string requestId, Model.StopReason reason)
        {
            _processor.Stop(requestId, reason);
        }

        public Model.RequestStatus Status(string requestId)
        {
            return _processor.Status(requestId);
        }

        public Model.Document[] Response(string requestId)
        {
            return _processor.Response(requestId);
        }

        public void AddResponseDocument(string requestId, string filePath)
        {
            _processor.AddResponseDocument(requestId, filePath);
        }

        public void RemoveResponseDocument(string requestId, string documentId)
        {
            _processor.RemoveResponseDocument(requestId, documentId);
        }

        public void ResponseDocument(string requestId, string documentId, out System.IO.Stream contentStream, int maxSize)
        {
            _processor.ResponseDocument(requestId, documentId, out contentStream, maxSize);
        }

        public void Close(string requestId)
        {
            _processor.Close(requestId);
        }

        public void PostProcess(string requestId)
        {
            _processor.PostProcess(requestId);
        }
    }
}
