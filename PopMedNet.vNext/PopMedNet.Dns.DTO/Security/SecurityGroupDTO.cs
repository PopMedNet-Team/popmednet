using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Security Group
    /// </summary>
    [DataContract]
    public class SecurityGroupDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Path
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Path { get; set; }
        /// <summary>
        /// Gets or sets the ID of the owner
        /// </summary>
        [DataMember]
        public Guid OwnerID { get; set; }
        /// <summary>
        /// Owner
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Owner { get; set; }
        /// <summary>
        /// Gets or sets the ID of Parent Security Group
        /// </summary>
        [DataMember]
        public Guid? ParentSecurityGroupID { get; set; }
        /// <summary>
        /// Parent SecurityGroup
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? ParentSecurityGroup { get; set; }
        /// <summary>
        /// Security Groups kinds
        /// </summary>
        [DataMember]
        public SecurityGroupKinds Kind { get; set; }
        /// <summary>
        /// Security Groups Types
        /// </summary>
        [DataMember]
        public SecurityGroupTypes Type { get; set; }
    }
}
