using PopMedNet.Objects;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Base ACL
    /// </summary>
    [DataContract]
    public class BaseAclDTO : EntityDto
    {
        /// <summary>
        /// Gets or set the ID of SecurityGroup
        /// </summary>
        [DataMember]
        public Guid SecurityGroupID { get; set; }
        /// <summary>
        /// Security Group
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? SecurityGroup { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Base ACL is overridden
        /// </summary>
        [DataMember]
        public bool Overridden { get; set; }
    }
}
