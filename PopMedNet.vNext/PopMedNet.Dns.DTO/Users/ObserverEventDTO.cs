using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Notification
    /// </summary>
    [DataContract]
    public class ObserverEventDTO
    {
        /// <summary>
        /// The Event ID
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// The name of the event
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
