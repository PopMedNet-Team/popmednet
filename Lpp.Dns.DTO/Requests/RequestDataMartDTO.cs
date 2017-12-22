using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request DataMart
    /// </summary>
    [DataContract]
    public class RequestDataMartDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or sets the ID of request
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// Gets or sets the ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// DataMArt
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string DataMart { get; set; }
        /// <summary>
        /// Gets or sets the status of Datamart routing
        /// </summary>
        [DataMember]
        public RoutingStatus Status { get; set; }
        /// <summary>
        /// Priority
        /// </summary>
        [DataMember]
        public Priorities Priority { get; set; }
        ///<summary>
        /// Due Date
        /// </summary>
        [DataMember]
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Time of request
        /// </summary>
        [DataMember]
        public DateTimeOffset? RequestTime { get; set; }
        /// <summary>
        /// Response Time
        /// </summary>
        [DataMember]
        public DateTimeOffset? ResponseTime { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Error detail
        /// </summary>
        [DataMember]
        public string ErrorDetail { get; set; }
        /// <summary>
        /// Reject Reason
        /// </summary>
        [DataMember]
        public string RejectReason { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if results are grouped
        /// </summary>
        [DataMember]
        public bool? ResultsGrouped { get; set; }
        /// <summary>
        /// Properties
        /// </summary>
        [DataMember]
        public string Properties { get; set; }

        /// <summary>
        /// Analysis center or Data Partner routing
        /// </summary>
        [DataMember]
        public RoutingType? RoutingType { get; set; }

        /// <summary>
        /// Gets or sets the ID of response
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid? ResponseID { get; set; }
        /// <summary>
        /// Gets or sets the ID of Response Group
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid? ResponseGroupID { get; set; }
        /// <summary>
        /// Response Group
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string ResponseGroup { get; set; }
        /// <summary>
        /// Response message
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string ResponseMessage { get; set; }
    }
}
