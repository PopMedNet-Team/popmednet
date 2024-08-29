using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Forgot Password
    /// </summary>
    [DataContract]
    public class ForgotPasswordDTO
    {
        /// <summary>
        /// The Name of the User
        /// </summary>
        [DataMember, MaxLength(50)]
        public string? UserName { get; set; }
        /// <summary>
        /// User Email address
        /// </summary>
        [DataMember, EmailAddress, MaxLength(255)]
        public string? Email { get; set; }
    }
}
