using System.Runtime.Serialization;
namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// User types
    /// </summary>
    [DataContract]
    public enum UserTypes
    {
        /// <summary>
        /// User
        /// </summary>
        [EnumMember]
        User = 0,
        /// <summary>
        /// Sso
        /// </summary>
        [EnumMember]
        Sso = 1,
        /// <summary>
        /// BackgroundTask
        /// </summary>
        [EnumMember]
        BackgroundTask = 2,
        /// <summary>
        /// DMCS Sync User
        /// </summary>
        [EnumMember]
        DMCSUser = 3
    }
}
