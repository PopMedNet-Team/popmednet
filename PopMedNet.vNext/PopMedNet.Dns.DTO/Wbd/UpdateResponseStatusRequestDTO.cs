using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Update response status request
    /// </summary>
    [DataContract]
    public class UpdateResponseStatusRequestDTO
    {
        /// <summary>
        /// Gets or set the ID of the request
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// Gets or set the ID of response
        /// </summary>
        [DataMember]
        public Guid ResponseID { get; set; }
        /// <summary>
        /// Gets or set the ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Gets or set the ID of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Gets or set the ID of Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        
        /// <summary>
        /// Gets or sets the SID of the user that acted on the respones (ie not the api user used for authentication to the service)
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// status id
        /// </summary>
        [DataMember]
        public RoutingStatus StatusID { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// Reject reason
        /// </summary>
        [DataMember]
        public string RejectReason { get; set; }
        /// <summary>
        /// Hold Reason
        /// </summary>
        [DataMember]
        public string HoldReason { get; set; }
        /// <summary>
        /// Gets or set the ID of Request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Request Type Name
        /// </summary>
        [DataMember]
        public string RequestTypeName { get; set; }

    }
}
