using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Workflow
    /// </summary>
    [DataContract]
    public class WorkflowDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember, MaxLength(255), Required]
        public string? Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string? Description { get; set; }
    }
}
