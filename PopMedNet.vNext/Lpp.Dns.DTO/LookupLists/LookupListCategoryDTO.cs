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
    /// Category List for LookUps
        /// </summary>
    [DataContract]
    public class LookupListCategoryDTO
    {
        /// <summary>
        /// List of id's
        /// </summary>
        [DataMember]
        public Lists ListId { get; set; }
        /// <summary>
        /// Gets or sets the id of Category 
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }
        /// <summary>
        /// Category Name
        /// </summary>
        [DataMember]
        [MaxLength(500)]
        public string CategoryName { get; set; }
    }
}
