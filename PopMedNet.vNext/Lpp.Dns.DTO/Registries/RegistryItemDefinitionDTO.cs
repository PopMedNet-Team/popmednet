using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
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
        public string Category { get; set; }
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [DataMember]
        [MaxLength(100), Required]
        public string Title { get; set; }
    }
}
