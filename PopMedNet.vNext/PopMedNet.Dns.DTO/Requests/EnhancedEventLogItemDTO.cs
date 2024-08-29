using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// An enhanced event log item
    /// </summary>
    [DataContract]
    public class EnhancedEventLogItemDTO
    {
        /// <summary>
        /// Gets or sets the step, the step is composed of {iteration}.{step}.
        /// </summary>
        [DataMember]
        public decimal Step { get; set; }
        /// <summary>
        /// Gets or sets the timestamp of the event.
        /// </summary>
        [DataMember]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the event source as applicable (ie the datamart name)
        /// </summary>
        [DataMember]
        public string Source { get; set; }
        /// <summary>
        /// Gets or sets the type of event.
        /// </summary>
        [DataMember]
        public string EventType { get; set; }
    }
}
