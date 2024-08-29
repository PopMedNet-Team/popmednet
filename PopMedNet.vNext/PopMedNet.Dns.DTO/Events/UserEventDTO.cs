using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// User Events
    /// </summary>
    [DataContract]
    public class UserEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
    }
}
