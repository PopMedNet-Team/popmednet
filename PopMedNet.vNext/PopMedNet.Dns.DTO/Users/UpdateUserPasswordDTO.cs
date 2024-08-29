using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Update user password
    /// </summary>
    [DataContract]
    public class UpdateUserPasswordDTO
    {
        /// <summary>
        /// Gets or sets the ID of the user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }
    }
}
