using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// ACL
    /// </summary>
    [DataContract]
    public class AclDTO : BaseAclDTO
    {
        /// <summary>
        /// Gets or sets the indicator to specify if ACL is Allowed
        /// </summary>
        [DataMember]
        public bool? Allowed { get; set; }
        /// <summary>
        /// Gets or sets the ID of the permission
        /// </summary>
        [DataMember]
        public Guid PermissionID { get; set; }
        /// <summary>
        /// Permission
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Permission { get; set; }
    }
}
