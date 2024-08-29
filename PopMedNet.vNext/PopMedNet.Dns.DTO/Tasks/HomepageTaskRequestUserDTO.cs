using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request User Task on Home page
    /// </summary>
    [DataContract]
    public class HomepageTaskRequestUserDTO
    {
        /// <summary>
        /// Gets or sets the ID of the request if applicable
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the task if applicable
        /// </summary>
        [DataMember]
        public Guid TaskID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the user if applicable
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// The name of the user
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// First name of the user
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name of the user
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Gets or set the ID of the role of workflow
        /// </summary>
        [DataMember]
        public Guid WorkflowRoleID { get; set; }
        /// <summary>
        /// Gets or sets the role of Workflow
        /// </summary>
        [DataMember]
        public string WorkflowRole { get; set; }
    }
}
