using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Notification
    /// </summary>
    [DataContract]
    public class NotificationDTO
    {
        /// <summary>
        /// Gets or set the date time
        /// </summary>
        [DataMember]
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Event
        /// </summary>
        [DataMember]
        public string? Event { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract]
    public class UserNotificationDTO
    {
        /// <summary>
        /// Gets or set the date time
        /// </summary>
        [DataMember]
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Event
        /// </summary>
        [DataMember]
        public string? Event { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string? Message { get; set; }
        [DataMember]
        public Guid UserID { get; set; }
    }

}
