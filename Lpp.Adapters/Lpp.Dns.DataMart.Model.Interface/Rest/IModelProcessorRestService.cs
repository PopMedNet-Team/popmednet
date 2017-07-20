using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;
using System.ServiceModel.Activation;

namespace Lpp.Dns.DataMart.Model.Rest
{
    /// <summary>
    /// A REST interface that allows an implementation of the IModelProcessor to delegate transfer and processing of Documents
    /// to a web service.
    /// </summary>
    [ServiceContract(Namespace="http://lincolnpeak.com/schemas/DNS4/API")] 
    public interface IModelProcessorRestService
    {
        /// <summary>
        /// Corresponds to IModelProcessor's Request method. 
        /// Initiates a request. Transfers a list of documents from the requestor.
        /// Does not transfer actual document contents.
        /// </summary>
        /// <param name="requestId">Identifies this request for subsequent calls</param>
        /// <param name="requestDocuments">List of Documents that may be sent with this request</param>
        /// <param name="requestTypeId">Request type identifier</param>
        /// <param name="settings">User settings for the Model</param>
        /// <param name="desiredDocuments">The document Documents desired by the Model</param>
        /// <returns>Request instance token to be used to identify all subsequent calls associated with this request</returns>
        [OperationContract, WebInvoke( Method = "POST", 
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle=WebMessageBodyStyle.Wrapped, 
            UriTemplate="PostRequest/{requestId}" )]
        string Request( 
            string requestId,
            [MessageParameter( Name = "RequestDocuments" )] Document[] requestDocuments,
            [MessageParameter( Name = "RequestTypeId" )] string requestTypeId,
            [MessageParameter( Name = "Settings" )] IDictionary<string, object> settings,
            [MessageParameter( Name = "DesiredDocuments" )] out string[] desiredDocumentIds
            );

        /// <summary>
        /// Corresponds to IModelProcessor's RequestDocument method.
        /// Called repeatedly to provide the Model with the specified document contents.
        /// </summary>
        /// <param name="requestId">Request instance id</param>
        /// <param name="documentId">The id of the Document being transferred</param>
        /// <param name="contentStream">Stream pointer to read the document</param>
        //[OperationContract, WebInvoke(Method = "POST",
        //    ResponseFormat = WebMessageFormat.Xml,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //    UriTemplate = "PostRequestDocumentStream/{requestId}/{documentId}")]
        //void RequestDocumentStream(
        //    string requestId,
        //    string documentId,
        //    [MessageParameter(Name = "DocumentStream")] Stream documentStream);

        /// <summary>
        /// Corresponds to IModelProcessor's RequestDocument method.
        /// Called repeatedly to provide the Model with the specified document contents.
        /// </summary>
        /// <param name="requestId">Request instance id</param>
        /// <param name="documentId">The id of the Document being transferred</param>
        /// <param name="contentStream">Stream pointer to read the document</param>
        /// <summary>
        [OperationContract, WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "PostRequestDocument/{requestId}/{documentId}/{offset}")]
        void RequestDocument(
            string requestId,
            string documentId,
            string offset,
            //[MessageParameter(Name = "Offset")] int offset,
            [MessageParameter(Name = "Data")] byte[] data);

        /// <summary>
        /// Corresponds to IModelProcessor's Start method.
        /// Tells the web service that all request Documents are transferred
        /// and that it may begin processing.
        /// </summary>
        /// <param name="requestId">Identifies this request for subsequent calls</param>
        [OperationContract, WebInvoke(Method = "PUT",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "Start/{requestId}")]
        void Start(string requestId);

        /// <summary>
        /// Corresponds to IModelProcessor's Stop method.
        /// Stops a request.
        /// </summary>
        /// <param name="RequestId">Request instance id</param>
        /// <param name="StopReason">Reason code</param>
        [OperationContract, WebInvoke(Method = "PUT",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "Stop/{requestId}")]
        void Stop(string requestId, 
            [MessageParameter(Name = "Reason")] StopReason reason);

        /// <summary>
        /// Corresponds to IModelProcessor's Status method.
        /// Return current status of request.
        /// </summary>
        /// <param name="RequestId">Request instance id</param>
        /// <returns>RequestStatus denoting the state of the request</returns>
        [OperationContract, WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetStatus/{requestId}")]
        RequestStatus Status(string requestId);

        /// <summary>
        /// Corresponds to IModelProcessor's Response method.
        /// Returns information about the list of Documents that can be returned.
        /// Called when RequestStatus is Complete. Does not return actual contents..
        /// </summary>
        /// <param name="requestId">Request instance idr</param>
        /// <returns>List of response Documents</returns>
        [OperationContract, WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetResponse/{requestId}")]
        Document[] Response(string requestId);

        /// <summary>
        /// Corresponds to IModelProcessor's ResponseDocument method.
        /// Gets a chunk of data from the service for the specified Document.
        /// </summary>
        /// <param name="requestId">Request instance idr</param>
        /// <param name="documentId">The id of the document being transferred</param>
        /// <param name="offset">Position of the document to start transfer</param>
        /// <param name="data">Chunk of data of the Document's content</param>
        [OperationContract, WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetResponseDocument/{requestId}/{documentId}/{offset}")]
        int ResponseDocument(
            string requestId,
            string documentId,
            string offset,
            [MessageParameter(Name = "Data")] out byte[] data
            );

        /// <summary>
        /// Corresponds to IModelProcessor's Close method.
        /// Closes the specified request. Local data and memory resident data for the request will be cleaned up.
        /// Closed request cannot be restarted.
        /// </summary>
        /// <param name="requestId">Request instance id</param>
        [OperationContract, WebInvoke(Method = "PUT",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "Close/{requestId}")]
        void Close(string requestId);

        /// <summary>
        /// Returns extra properties supported by the Model.
        /// </summary>
        /// <returns></returns>
        [OperationContract, WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetProperties")]
        IDictionary<string,string> Properties();
      
        /// <summary>
        /// Returns extra capabilities supported by the Model.
        /// </summary>
        /// <returns></returns>
        [OperationContract, WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetCapabilities")]
        IDictionary<string,bool> Capabilities();

        //[OperationContract]
        //[WebGet]
        //string EchoWithGet(string s);

        //[OperationContract]
        //[WebInvoke]
        //string EchoWithPost(string s);

        //[OperationContract]
        //[WebInvoke(Method = "GET",
        //    ResponseFormat = WebMessageFormat.Xml,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //    UriTemplate = "Test?format=xml&id={Id}")]
        //IList<string> XMLData(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "Test?format=json&id={id}")]
        string JSONData(string id);
    }
}
