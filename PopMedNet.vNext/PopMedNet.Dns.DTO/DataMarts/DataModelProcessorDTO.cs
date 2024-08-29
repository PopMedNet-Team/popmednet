using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Data Models
    /// </summary>
    [DataContract]
    public class DataModelProcessorDTO
    {
        public DataModelProcessorDTO() { }

        /// <summary>
        /// The Data Model ID
        /// </summary>
        [DataMember]
        public Guid ModelID { get; set; }
        
        /// <summary>
        /// The name of the processor
        /// </summary>
        [DataMember]
        public string Processor { get; set; }

        /// <summary>
        /// The ID of the processor
        /// </summary>
        [DataMember]
        public Guid ProcessorID { get; set; }
    }
}
