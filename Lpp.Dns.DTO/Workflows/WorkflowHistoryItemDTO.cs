using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Workflow History Item
    /// </summary>
    [DataContract]
    public class WorkflowHistoryItemDTO
    {
        /// <summary>
        /// ID of Task
        /// </summary>
        [DataMember]
        public Guid TaskID { get; set; }
        /// <summary>
        /// Task Name
        /// </summary>
        [DataMember]
        public string TaskName { get; set; }
        /// <summary>
        /// ID of the user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// Name of the user
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Full name of the user
        /// </summary>
        [DataMember]
        public string UserFullName { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public DateTimeOffset Date { get; set; }
        /// <summary>
        /// ID of the routing
        /// </summary>
        [DataMember]
        public Guid? RoutingID { get; set; }
        /// <summary>
        /// DataMart
        /// </summary>
        [DataMember]
        public string DataMart { get; set; }
        /// <summary>
        /// ID of the Workflow Activity
        /// </summary>
        [DataMember]
        public Guid? WorkflowActivityID { get; set; }
    }
}
