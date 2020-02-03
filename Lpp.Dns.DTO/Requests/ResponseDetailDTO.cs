using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Response Detail
    /// </summary>
    [DataContract]
    public class ResponseDetailDTO : ResponseDTO
    {
        /// <summary>
        /// Request
        /// </summary>
        [DataMember]
        public string Request { get; set; }
        /// <summary>
        /// Gets or Sets the ID of request
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// DataMart
        /// </summary>
        [DataMember]
        public string DataMart { get; set; }
        /// <summary>
        /// Gets or Sets the ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Submitted By
        /// </summary>
        [DataMember]
        public string SubmittedBy { get; set; }
        /// <summary>
        /// Who responds to the request
        /// </summary>
        [DataMember]
        public string RespondedBy { get; set; }
        /// <summary>
        /// Status of DataMart routing status
        /// </summary>
        [DataMember]
        public RoutingStatus Status { get; set; }
    }
}
