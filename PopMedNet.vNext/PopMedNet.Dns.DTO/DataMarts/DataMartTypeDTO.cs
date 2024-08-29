using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DataMart Types
    /// </summary>
    [DataContract]
    public class DataMartTypeDTO
    {
        /// <summary>
        /// ID of DataMart type
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }
        /// <summary>
        /// Name 
        /// </summary>
        [DataMember]
        public string Name { get; set; }
       
    }
}
