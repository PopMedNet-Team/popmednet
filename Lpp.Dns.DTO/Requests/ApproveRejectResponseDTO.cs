using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Approve Reject Response
    /// </summary>
    [DataContract]
    public class ApproveRejectResponseDTO
    {
        /// <summary>
        /// Gets or sets the ID of Response
        /// </summary>
        [DataMember]
        public Guid ResponseID { get; set; }
    }
}
