using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DTO for referencing Term
    /// </summary>
    [DataContract]
    public class TermDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember, Lpp.Objects.ValidationAttributes.MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// OID
        /// </summary>
        [DataMember, Lpp.Objects.ValidationAttributes.MaxLength(100)]
        public string OID { get; set; }
        /// <summary>
        /// ReferenceUrl
        /// </summary>
        [DataMember, Lpp.Objects.ValidationAttributes.MaxLength(450)]
        public string ReferenceUrl { get; set; }
        /// <summary>
        /// Gets or sets the type of Term types
        /// </summary>
        [DataMember]
        public TermTypes Type { get; set; }
    }
}
