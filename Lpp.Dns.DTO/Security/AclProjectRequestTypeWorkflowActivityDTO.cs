using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Security
{
    /// <summary>
    /// Project RequestType Workflow Activity ACL
    /// </summary>
    [DataContract]
    public class AclProjectRequestTypeWorkflowActivityDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Project
        /// </summary>
        [DataMember]
        public string Project { get; set; }
        /// <summary>
        /// Gets or set the ID of Request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Request type
        /// </summary>
        [DataMember]
        public string RequestType { get; set; }
        /// <summary>
        /// Gets or set the ID of Workflow activity
        /// </summary>
        [DataMember]
        public Guid WorkflowActivityID { get; set; }
        /// <summary>
        /// Workflow activity
        /// </summary>
        [DataMember]
        public string WorkflowActivity { get; set; }
    }
}
