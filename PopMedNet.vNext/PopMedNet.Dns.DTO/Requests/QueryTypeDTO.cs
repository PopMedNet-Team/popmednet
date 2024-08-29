using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Query Type
    /// </summary>
    [DataContract]
    public class QueryTypeDTO
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [MaxLength(50), Required]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}