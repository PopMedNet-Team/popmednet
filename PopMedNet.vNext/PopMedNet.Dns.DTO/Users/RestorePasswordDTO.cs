using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Restore Password
    /// </summary>
    [DataContract]
    public class RestorePasswordDTO
    {
        /// <summary>
        /// Gets or sets the password restore tokens
        /// </summary>
        [DataMember]
        public Guid PasswordRestoreToken { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }
    }
}
