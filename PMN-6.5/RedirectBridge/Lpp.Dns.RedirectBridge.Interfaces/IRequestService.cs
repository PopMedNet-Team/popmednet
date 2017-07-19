using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Xml.Serialization;

namespace Lpp.Dns
{
    [ServiceContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    [XmlSerializerFormat( Style=OperationFormatStyle.Document, SupportFaults=true, Use=OperationFormatUse.Literal)]
    public interface IRequestService
    {
        /// <summary>
        /// Retrieve metadata associated with the current session
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <returns>Metadata</returns>
        [OperationContract, WebGet(UriTemplate="{sessionToken}/Session")]
        [FaultContract(typeof(InvalidSessionFault))]
        SessionMetadata GetSessionMetadata( string sessionToken );

        /// <summary>
        /// This operation must be called by the Model Provider at the end of processing a request
        /// to indicate that the creation of the request has been completed, and to provide the
        /// result of that process - the request itself in serialized form.
        /// This must be the last call in the current session. The sessionToken will not be valid for any more calls after this.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <param name="requestHeader">Header. Common attributes of a request.</param>
        /// <param name="requestMimeType">MIME type of the request.</param>
        /// <param name="requestBody">The created request in serialized form.</param>
        /// <param name="applicableDataMartIds">The list of data marts that are applicable to the current request.
        /// This should be a subset of the list returned by <see cref="GetApplicableDataMarts"/>. If there are any extra
        /// data marts in this list, they are ignored. This list may be left empty, in which case all applicable data marts are used.</param>
        [OperationContract, WebInvoke( BodyStyle=WebMessageBodyStyle.Wrapped, UriTemplate="{sessionToken}/Commit" )]
        [FaultContract( typeof( InvalidSessionFault ) )]
        void RequestCreated( string sessionToken,
            [MessageParameter( Name = "Header" )] RequestHeader requestHeader,
            [MessageParameter( Name = "ApplicableDataMarts" )] Guid[] applicableDataMartIDs );

        /// <summary>
        /// This operation must be called by the Model Provider if the user has elected to abandon the request creation.
        /// This must be the last call in the current session. The sessionToken will not be valid for any more calls after this.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        [OperationContract, WebInvoke( BodyStyle=WebMessageBodyStyle.Wrapped, UriTemplate="{sessionToken}/Abort" )]
        [FaultContract( typeof( InvalidSessionFault ) )]
        void RequestAborted( string sessionToken );

        /// <summary>
        /// Adds an accompanying document to the request that is currently being constructed.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <param name="documentName">Name of the document. If there is already a document with this name associated
        /// with the request being constructed, that document will be replaced.</param>
        /// <param name="documentMimeType">MIME type of the document</param>
        /// <param name="documentBody">Content of the document</param>
        [OperationContract, WebInvoke( BodyStyle=WebMessageBodyStyle.Wrapped, UriTemplate="{sessionToken}/Document" )]
        [FaultContract( typeof( InvalidSessionFault ) )]
        void PostDocument( string sessionToken,
            [MessageParameter( Name = "Name" )] string documentName,
            [MessageParameter( Name = "MimeType" )] string documentMimeType,
            [MessageParameter( Name = "Viewable" )] bool isViewable,
            [MessageParameter( Name = "Body" )] byte[] documentBody );

        /// <summary>
        /// This operation may be called by the Model Provider during processing of a request to obtain a list
        /// of all data marts, along with their metadata, that are may be applicable to the current request.
        /// The primary purpose of this operation is to have the Model Provider filter out some data marts
        /// based on some model-specific criteria.
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <returns>List of data marts applicable to the current request</returns>
        [OperationContract, WebGet( UriTemplate="{sessionToken}/ApplicableDataMarts" )]
        [FaultContract( typeof( InvalidSessionFault ) )]
        DataMartList GetApplicableDataMarts( string sessionToken );
    }

    [CollectionDataContract( Name="DataMarts", ItemName="DataMart", Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    [XmlRoot( Namespace="http://lincolnpeak.com/schemas/DNS4/API", ElementName="DataMarts" )]
    public class DataMartList : List<DataMart>
    {
        public DataMartList( IEnumerable<DataMart> from ) : base( from ) { }
        public DataMartList() { }
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public enum RequestPriority
    {
        [EnumMember] Low,
        [EnumMember] Normal,
        [EnumMember] High
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public class RequestHeader
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string Activity { get; set; }
        [DataMember] public string ActivityDescription { get; set; }
        [DataMember] public DateTime? DueDate { get; set; }
        [DataMember] public RequestPriority? Priority { get; set; } 
    }
}