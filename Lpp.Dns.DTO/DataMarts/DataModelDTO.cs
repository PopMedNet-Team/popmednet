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
    /// Data Models
    /// </summary>
    [DataContract]
    public class DataModelDTO : EntityDtoWithID
    {
        /// <summary>
        /// Data Model Name
        /// </summary>
        public DataModelDTO() { }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Determines whether Data Model Requires Configuration
        /// </summary>
        [DataMember]
        public bool RequiresConfiguration { get; set; }
        /// <summary>
        /// Gets or sets if the datamodel is supported by QueryComposer processor.
        /// </summary>
        [DataMember]
        public bool QueryComposer { get; set; }
    }
}
