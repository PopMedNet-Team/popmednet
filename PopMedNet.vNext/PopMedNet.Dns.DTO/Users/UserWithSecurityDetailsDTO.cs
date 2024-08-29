using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// User with security details
    /// </summary>
    [DataContract]
    public class UserWithSecurityDetailsDTO : UserDTO
    {
        /// <summary>
        /// Password Hash
        /// </summary>
        [DataMember]
        public string PasswordHash { get; set; }
    }
}
