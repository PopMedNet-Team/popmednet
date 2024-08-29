using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Notification crud
    /// </summary>
    [DataContract]
    public class NotificationCrudDTO
    {
        /// <summary>
        /// Object id
        /// </summary>
        [DataMember]
        public Guid ObjectID { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [DataMember]
        public ObjectStates State { get; set; }
    }
}
