using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request DataMart
    /// </summary>
    [DataContract]
    public class RequestDataMartDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or sets the Identifier of request.
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of DataMart.
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Gets or sets the Name of the DataMart.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string DataMart { get; set; }
        /// <summary>
        /// Gets or sets the status of Datamart routing
        /// </summary>
        [DataMember]
        public RoutingStatus Status { get; set; }
        /// <summary>
        /// Gets or sets the Priority of the Routing.
        /// </summary>
        [DataMember]
        public Priorities Priority { get; set; }
        ///<summary>
        /// Gets or Sets the Due Date of the Routing.
        /// </summary>
        [DataMember]
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Gets or Sets the Error message of the Routing.
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or Sets the Error detail of the Routing.
        /// </summary>
        [DataMember]
        public string ErrorDetail { get; set; }
        /// <summary>
        /// Gets or Sets the Reject Reason of the Routing.
        /// </summary>
        [DataMember]
        public string RejectReason { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if results are grouped.
        /// </summary>
        [DataMember]
        public bool? ResultsGrouped { get; set; }
        /// <summary>
        /// Gets or sets the Properties of the Routing.
        /// </summary>
        [DataMember]
        public string Properties { get; set; }
        /// <summary>
        /// Gets or Sets the Type of Routing.
        /// </summary>
        [DataMember]
        public RoutingType? RoutingType { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of response.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid? ResponseID { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of Response Group.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid? ResponseGroupID { get; set; }
        /// <summary>
        /// Gets or sets the name of the Response Group.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string ResponseGroup { get; set; }
        /// <summary>
        /// Gets or sets the Response message.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string ResponseMessage { get; set; }
        /// <summary>
        /// Gets or Sets When the Current Response was Submitted on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? ResponseSubmittedOn { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier of the The User who Submitted the Current Response.
        /// </summary>
        [DataMember]
        public Guid? ResponseSubmittedByID { get; set; }
        /// <summary>
        /// Gets or Sets the user that submitted the Current Response.
        /// </summary>
        [DataMember]
        public string ResponseSubmittedBy { get; set; }
        /// <summary>
        /// Gets or Sets the Time the Current Response is Responded to.
        /// </summary>
        [DataMember]
        public DateTime? ResponseTime { get; set; }
    }
}
