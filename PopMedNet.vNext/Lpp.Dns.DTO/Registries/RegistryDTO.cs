using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Registry
    /// </summary>
    [DataContract]
    public class RegistryDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or sets the indicator to specify if deleted
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the registrytypes
        /// </summary>
        [DataMember]
        public RegistryTypes Type { get; set; }
        /// <summary>
        /// Gets or sets the registry name
        /// </summary>
        [DataMember, MaxLength(100), Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the registry description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the RoPRUrl
        /// </summary>
        [DataMember, MaxLength(500)]
        public string RoPRUrl { get; set; }

    }
}
