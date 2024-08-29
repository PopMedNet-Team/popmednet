using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// User Request Observer Event Subscription
    /// </summary>
    [DataContract]
    public class RequestObserverEventSubscriptionDTO : EntityDto
    {
        /// <summary>
        /// ID of the RequestObserver
        /// </summary>
        [DataMember]
        public Guid RequestObserverID { get; set; }
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
    }
}
