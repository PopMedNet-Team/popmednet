using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Network Group
    /// </summary>
    [DataContract]
    public class GroupDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Network Group is deleted
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the indicator if network Group requires Approval
        /// </summary>
        [DataMember]
        public bool ApprovalRequired { get; set; }
    }
}
