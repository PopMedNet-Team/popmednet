using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request user
    /// </summary>
    [DataContract]
    public class RequestUserDTO
    {
        /// <summary>
        /// Gets or Sets the ID of request
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// Gets or Sets the ID of user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        [DataMember]
        public string Username { get; set; }
        /// <summary>
        /// User full name
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Gets or Sets the ID of workflow role
        /// </summary>
        [DataMember]
        public Guid WorkflowRoleID { get; set; }
        /// <summary>
        /// Workflow role
        /// </summary>
        [DataMember]
        public string WorkflowRole { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if requestor role is filled
        /// </summary>
        [DataMember]
        public bool IsRequestCreatorRole { get; set; }
    }
}
