using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Security Group with user
    /// </summary>
    [DataContract]
    public class SecurityGroupWithUsersDTO : SecurityGroupDTO
    {
        /// <summary>
        /// List of users
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Users { get; set; }
    }
}
