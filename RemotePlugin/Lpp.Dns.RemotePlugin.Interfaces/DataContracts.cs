using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.RemotePlugin
{
    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/DMClient" )]
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/DMClient" )]
    public class DataMartId
    {
        public int Id { get; set; }

        public DataMartId( int value ) 
        {
            Id = value;
        }
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public struct DataMart
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public class RequestHeader
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string Activity { get; set; }
        [DataMember] public string ActivityDescription { get; set; }
        [DataMember] public DateTime? DueDate { get; set; }
        [DataMember] public Priorities? Priority { get; set; } 
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/DMClient" )]
    public class Request
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public DateTime CreateDate { get; set; }
        [DataMember] public string RequestTypeId { get; set; }
        [DataMember] public List<Document> Documents { get; set; }
        [DataMember] public List<DataMartResponse> DataMartResponses { get; set; }
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/DMClient" )]
    public enum RequestStatus
    {
        [EnumMember] Submitted,
        [EnumMember] AwaitingResponseApproval,
        [EnumMember] Completed,
        [EnumMember] Rejected,
        [EnumMember] Hold,
        [EnumMember] Failed,
        [EnumMember] Canceled
    }

    /// <summary>
    /// Describes response that came from a single datamart - which includes identification
    /// of the datamart in question and one or more documents.
    /// </summary>
    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public struct DataMartResponse
    {
        [DataMember] public DataMart DataMart { get; set; }
        [DataMember] public Document[] Documents { get; set; }
    }

    /// <summary>
    /// Identifies a document which is a part of a response. A response consists of multiple documents,
    /// because it may have come from multiple datamarts and each datamart may return multiple documents as well.
    /// </summary>
    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public class Document
    {
        /// <summary>
        /// Identifier, unique in the context of current communication session
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// Name of the document
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Total size of the document, in bytes
        /// </summary>
        [DataMember]
        public int Size { get; set; }

        /// <summary>
        /// MIME type of the document
        /// </summary>
        [DataMember]
        public string MimeType { get; set; }

        [DataMember]
        public byte[] Content { get; set; }

        /// <summary>
        /// URL that the document may be downloaded from
        /// </summary>
        [DataMember]
        public string LiveUrl { get; set; }
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public struct InvalidSessionFault
    {
        public static readonly InvalidSessionFault Instance = new InvalidSessionFault();
    }

    [DataContract]
    public class AuthenticationFault : FaultException
    { 
        public override string ToString() { return "Authentication Failure"; } 
    }

    [DataContract]
    public class InternalErrorFault : FaultException
    {
        public IEnumerable<string> ErrorMessages { get; set; }
        public override string ToString() { return string.Join( Environment.NewLine, ErrorMessages ?? Enumerable.Empty<string>() ); }
    }

    [DataContract]
    public class CannotCreateRequestFault
    {
        public string Message { get; set; }

        public override string ToString() { return "Cannot create request: " + Message; }
    }
}