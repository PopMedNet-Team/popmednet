using PopMedNet.Objects.ValidationAttributes;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Definition of Registry item
    /// </summary>
    [DataContract]
    public class RegistryItemDefinitionDTO
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid? ID { get; set; }
        /// <summary>
        /// Gets or sets the category
        /// </summary>
        [DataMember]
        [MaxLength(80), Required]
        public string? Category { get; set; }
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [DataMember]
        [MaxLength(100), Required]
        public string? Title { get; set; }
    }
}
