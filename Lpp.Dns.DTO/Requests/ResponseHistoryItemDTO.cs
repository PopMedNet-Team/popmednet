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
        /// Gets or Sets the Identifier of the Response.
        /// </summary>
        [DataMember]
        public Guid ResponseID { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier of the Request.
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// Gets or Sets DateTime of Action.
        /// </summary>
        [DataMember]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Gets or Sets the Response Action.
        /// </summary>
        [DataMember]
        public string Action { get; set; }
        /// <summary>
        /// Gets or Sets the Username of user who carried out the action.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Gets or Sets the Message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// Gets or Sets if Is Response Item.
        /// </summary>
        [DataMember]
        public bool IsResponseItem { get; set; }
        /// <summary>
        /// Gets or Sets if Is Current.
        /// </summary>
        [DataMember]
        public bool IsCurrent { get; set; }

    }
}
