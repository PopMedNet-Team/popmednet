using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Lpp.Dns.DataMart.Model
{
    /// <summary>
    /// Interface the Model must implement for the plugin.
    /// </summary>
    public interface IModelProcessor
    {
        Guid ModelProcessorId { get; }
        IModelMetadata ModelMetadata { get; }
        IDictionary<string, object> Settings { get; set; }

        /// <summary>
        /// Associates the request properties for the specified request.
        /// </summary>
        /// <param name="requestId">Values set by user for the model's properties</param>
        /// <param name="requestProperties">Values of properties associated with the request</param>
        void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties);

        /// <summary>
        /// Initiates a request. Transfers a list of documents from the requestor.
        /// Does not transfer actual document contents.
        /// </summary>
        /// <param name="requestId">Identifies this request for subsequent calls</param>
        /// <param name="requestDocuments">List of Documents that may be sent with this request</param>
        /// <param name="requestTypeId">Request type identifier</param>
        /// <param name="desiredDocuments">The document Documents desired by the Model</param>
        void Request(string requestId, NetworkConnectionMetadata network, RequestMetadata request, Document[] requestDocuments, out IDictionary<string,string> requestProperties, out Document[] desiredDocuments);

        /// <summary>
        /// Called repeatedly to provide the Model with the specified document contents.
        /// </summary>
        /// <param name="requestId">Request instance id</param>
        /// <param name="documentId">The id of the Document being transferred</param>
        /// <param name="contentStream">Stream pointer to read the document</param>
        void RequestDocument(string requestId, string documentId, Stream contentStream);

        /// <summary>
        /// Corresponds to IModelProcessor's Start method.
        /// Tells the web service that all request Documents are transferred
        /// and that it may begin processing.
        /// </summary>
        /// <param name="requestId">Identifies this request for subsequent calls</param>
        /// <param name="ViewSQL">Denotes the generated SQL should be returned as a result</param>
        void Start(string requestId, bool ViewSQL = false);

        /// <summary>
        /// Stops a request. Multiple calls may be made and the processor implementation should be able
        /// to handle or ignore redundant stop calls.
        /// </summary>
        /// <param name="RequestId">Request instance id</param>
        /// <param name="StopReason">Reason code</param>
        void Stop(string requestId, StopReason reason);

        /// <summary>
        /// Return current status of request.
        /// </summary>
        /// <param name="RequestId">Request instance id</param>
        /// <returns>RequestStatus denoting the state of the request</returns>
        RequestStatus Status(string requestId);

        /// <summary>
        /// Returns information about the list of Documents that can be returned.
        /// Called when RequestStatus is Complete. Does not return actual contents.
        /// </summary>
        /// <param name="Request">Request instance id</param>
        /// <returns>List of response Documents</returns>
        Document[] Response(string requestId);

        /// <summary>
        /// Appends a file to the list of response Documents.
        /// </summary>
        /// <param name="requestId">Request instance id</param>
        /// <param name="filePath">Local path to the file to attach</param>
        void AddResponseDocument(string requestId, string filePath);

        /// <summary>
        /// Removes a file from the list of response Documents.
        /// </summary>
        /// <param name="requestId">Request instance id</param>
        /// <param name="filePath">Local path to the file to attach</param>
        void RemoveResponseDocument(string requestId, string documentId);

        /// <summary>
        /// Gets input stream for the specified Document.
        /// </summary>
        /// <param name="requestId">Request instance idr</param>
        /// <param name="documentId">The id of the document being transferred</param>
        /// <param name="contentStream">Stream pointer to a specified Document</param>
        /// <param name="maxSize">Maximum chunk size (returned chunk may be smaller)</param>
        void ResponseDocument(string requestId, string documentId, out Stream contentStream, int maxSize);

        /// <summary>
        /// Closes the specified request. Local and memory resident data for the request will be cleaned up.
        /// Closed request cannot be restarted.
        /// </summary>
        /// <param name="requestId">Request instance id</param>
        void Close(string requestId);

        /// <summary>
        /// Runs the post processor. This method is called by the model processor only if the status returned
        /// has a message to be displayed and the user responded "yes".
        /// </summary>
        /// <param name="requestId"></param>
        void PostProcess(string requestId);
    }
}