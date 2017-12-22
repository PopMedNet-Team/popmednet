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
