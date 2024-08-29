using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// User Event Subscription
    /// </summary>
    [DataContract]
    public class UserEventSubscriptionDTO : EntityDto
    { 
        /// <summary>
        /// ID of the user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// ID of the event
        /// </summary>
        [DataMember]
        public Guid EventID { get; set; }
        /// <summary>
        /// Event last run time
        /// </summary>
        [DataMember]
        public DateTimeOffset? LastRunTime { get; set; }
        /// <summary>
        /// Event nxt due time
        /// </summary>
        [DataMember]
        public DateTimeOffset? NextDueTime { get; set; }
        /// <summary>
        /// Frequency
        /// </summary>
        [DataMember]
        public Frequencies? Frequency { get; set; }
        /// <summary>
        /// Frequency for My Notifications
        /// </summary>
        [DataMember]
        public Frequencies? FrequencyForMy { get; set; }
    }
}
