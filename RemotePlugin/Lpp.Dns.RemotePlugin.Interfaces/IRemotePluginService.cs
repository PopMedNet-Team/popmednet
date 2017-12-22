using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Lpp.Dns.RemotePlugin
{
    /// <summary>
    /// This web service interface is designed to allow remote client to construct and submit requests to data marts through DNS.
    /// Client-specific data can be associated with a request and delivered by DNS to the destination data marts.
    /// </summary>
    [ServiceContract(Namespace="http://lincolnpeak.com/schemas/DNS4/API")] 
    [XmlSerializerFormat( Style=OperationFormatStyle.Document, SupportFaults=true, Use=OperationFormatUse.Literal)]
    public interface IRemotePluginService
    {
        /// <summary>
        /// This operation must be called by the model provider before all others to get a session token for all subsequent operations.
        /// </summary>
        /// <param name="credentials">login user</param>
        /// <returns>session token</returns>
        [OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{requestor}/Start")]
        string StartSession( Credentials credentials, string requestor, Guid projectId );

        /// <summary>
        /// This operation must be called by the model provider before all others to get a session token for all subsequent operations.
        /// </summary>
        /// <param name="credentials">login user</param>
        /// <returns>session token</returns>
        [OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{requestor}/{projectAcronym}/Start")]
        string StartProjectSession(Credentials credentials, string requestor, string projectAcronym);

        /// <summary>
        /// This operation must be called by the model provider to close the session. 
        /// All open requests must be submitted or aborted before closing the session.
        /// The session token will not be valid for any more calls after this.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        [OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{sessionToken}/Close")]
        void CloseSession(string sessionToken);

        /// <summary>
        /// Starts a new request that is to be constructed.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <param name="requestTypeId">Identifies the type of request being constructed.</param>
        /// <returns>A request identifier</returns>
        [OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{sessionToken}/{requestTypeId}/Create")]
        string CreateRequest(string sessionToken, string requestTypeId);

        ///// <summary>
        ///// This operation may be called by the model provider to obtain a list
        ///// of all data marts, along with their metadata, that may be applicable to the specified request.
        ///// The primary purpose of this operation is to have the model provider filter out some data marts
        ///// based on some model-specific criteria.
        ///// </summary>
        ///// <param name="sessionToken">Identifies the current communication session.</param>
        ///// <returns>List of data marts applicable to the current request</returns>
        //[OperationContract, WebGet(UriTemplate = "{sessionToken}/{requestId}/ApplicableDataMarts")]
        //[DataContractFormat]
        //IEnumerable<DataMart> GetApplicableDataMarts(string sessionToken, string requestId);

        /// <summary>
        /// Adds an accompanying document to the request that is being constructed.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <param name="requestId">Identifies the request to which this document is being added</param>
        /// <param name="documentName">Name of the document. If there is already a document with this name associated
        /// with the request being constructed, that document will be replaced.</param>
        /// <param name="documentMimeType">MIME type of the document</param>
        /// <param name="documentBody">Content of the document</param>
        /// <param name="filterByDataMartIds">List of IDs of datamarts for which to return requests. May be null, in which case all user's datamarts are assumed.</param>
        [OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{sessionToken}/{requestId}/Document")]
        void PostDocument(string sessionToken, string requestId,
            [MessageParameter(Name = "Name")] string documentName,
            [MessageParameter(Name = "MimeType")] string documentMimeType,
            [MessageParameter(Name = "Viewable")] bool isViewable,
            [MessageParameter(Name = "Body")] byte[] documentBody);

        /// <summary>
        /// This operation must be called by the Model Provider at the end of processing a request
        /// to indicate that the creation of the request has been completed, and to provide the
        /// result of that process - the request itself in serialized form.
        /// This must be the last call in the specified request. The requestId will not be valid for any more Post, Submit or Abort after this.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <param name="requestId">Identifies the request to be submitted for processing</param>
        /// <param name="requestHeader">Header. Common attributes of a request.</param>
        /// <param name="applicableDataMartIds">The list of data marts that are applicable to the current request.
        /// This should be a subset of the list returned by <see cref="GetApplicableDataMarts"/>. If there are any extra
        /// data marts in this list, they are ignored. This list may be left empty, in which case all applicable data marts are used.</param>
        [OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{sessionToken}/{requestId}/Submit")]
        void SubmitRequest(string sessionToken, string requestId,
            [MessageParameter(Name = "Header")] RequestHeader requestHeader,
            [MessageParameter(Name = "ApplicableDataMarts")] int[] applicableDataMartIds);

        ///// <summary>
        ///// This operation must be called by the Model Provider if the user has elected to abandon the request creation.
        ///// This must be the last call in the current request. The requestId will not be valid for any more Post, Submit or Abort calls after this.
        ///// </summary>
        ///// <param name="sessionToken">Identifies the current communication session.</param>
        ///// <param name="requestId">Identifies the request to be aborted</param>
        //[OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{sessionToken}/{requestId}/Abort")]
        //void AbortRequest(string sessionToken, string requestId);

        /// <summary>
        /// Retrieves a list of requests accessible to the current session,
        /// and optionally filtered for particular datamarts
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <param name="fromDate">The earliest date of requests in the returned list. May be null for no start date.</param>
        /// <param name="toDate">The latest date of the requests in the returned list. May be null for no end date.</param>
        /// <param name="filterByDataMartIds">List of IDs of datamarts for which to return requests. May be null, in which case all user's datamarts are assumed.</param>
        /// <param name="filterByStatus">List of request statuses to filter the requests by. May be null, in which case all requests are returned</param>
        [OperationContract, WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "{sessionToken}/Requests")]
        List<Request> GetRequests(string sessionToken, DateTime? fromDate, DateTime? toDate,
            List<int> filterByDataMartIds, List<RequestStatus> filterByStatus);

        ///// <summary>
        ///// Retrieves responses associated with a request optionally filtered by DataMarts.
        ///// </summary>
        ///// <param name="sessionToken">Identifies the current communication session.</param>
        ///// <param name="requestId">Identifies the request whose responses are desired.</param>
        ///// <returns>Request</returns>
        [OperationContract, WebGet(UriTemplate = "{sessionToken}/{requestId}/Request")]
        Request GetRequest(string sessionToken, string requestId);

    }

}
