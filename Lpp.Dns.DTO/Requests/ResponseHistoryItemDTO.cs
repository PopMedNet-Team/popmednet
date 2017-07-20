using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Response History Item
    /// </summary>
    [DataContract]
    public class ResponseHistoryItemDTO
    {
        /// <summary>
        /// Response ID
        /// </summary>
        [DataMember]
        public Guid ResponseID { get; set; }
        /// <summary>
        /// Request ID
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// DateTime of Action
        /// </summary>
        [DataMember]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Response Action
        /// </summary>
        [DataMember]
        public string Action { get; set; }
        /// <summary>
        /// Usename of user who carried out the action
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// Is Response Item
        /// </summary>
        [DataMember]
        public bool IsResponseItem { get; set; }
        /// <summary>
        /// Is Current
        /// </summary>
        [DataMember]
        public bool IsCurrent { get; set; }

    }
}
