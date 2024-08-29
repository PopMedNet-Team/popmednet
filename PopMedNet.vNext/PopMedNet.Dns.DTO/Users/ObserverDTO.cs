using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Notification
    /// </summary>
    [DataContract]
    public class ObserverDTO
    {
        /// <summary>
        /// The user or SecurityGroup ID
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// The display name for the observer
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// The display name for the observer
        /// </summary>
        [DataMember]
        public string DisplayNameWithType { get; set; }

        /// <summary>
        /// The type of the observer
        /// </summary>
        [DataMember]
        public DTO.Enums.ObserverTypes ObserverType { get; set; }
    }
}
