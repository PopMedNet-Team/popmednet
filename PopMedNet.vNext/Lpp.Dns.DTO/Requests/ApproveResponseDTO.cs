using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Approve Response
    /// </summary>
    [DataContract]
    public class ApproveResponseDTO
    {
        /// <summary>
        /// Response Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// Response ID's
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> ResponseIDs { get; set; }
    }
}
