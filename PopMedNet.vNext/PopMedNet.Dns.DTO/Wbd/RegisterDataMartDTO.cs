using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Register DataMart
    /// </summary>
    [DataContract]
    public class RegisterDataMartDTO
    {
        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        [DataMember]
                public string Token { get; set; }
    }
}
