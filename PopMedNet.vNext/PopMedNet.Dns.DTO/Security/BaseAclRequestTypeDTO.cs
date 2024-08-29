using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Base ACL Request type
    /// </summary>
    [DataContract]
    public abstract class BaseAclRequestTypeDTO : BaseAclDTO
    {
        /// <summary>
        /// Gets or sets the ID of Request Type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Request Type permissions
        /// </summary>
                [DataMember]
        public Enums.RequestTypePermissions? Permission { get; set; }
    }
}
