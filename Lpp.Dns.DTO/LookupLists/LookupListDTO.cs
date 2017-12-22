using Lpp.Dns.DTO.Enums;
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
    /// Lookup List
    /// </summary>
    [DataContract]
    public class LookupListDTO
    {
        /// <summary>
        /// List of id's
        /// </summary>
        [DataMember]
        public Lists ListId { get; set; }
        /// <summary>
        /// List Name
        /// </summary>
        [DataMember]
        [MaxLength(50)]
        public string ListName { get; set; }
        /// <summary>
        /// Item code Version number
        /// </summary>
        [DataMember]
        [MaxLength(200)]
        public string Version { get; set; }
    }
}
