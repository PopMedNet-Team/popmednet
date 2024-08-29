using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Workflow Role
    /// </summary>
    [DataContract]
    public class WorkflowRoleDTO : EntityDtoWithID
    {
        /// <summary>
        /// ID of Workflow
        /// </summary>
        [DataMember]
        public Guid WorkflowID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the flag to indicate the requestor of role
        /// </summary>
        [DataMember]
        public bool IsRequestCreator { get; set; }
    }
}
