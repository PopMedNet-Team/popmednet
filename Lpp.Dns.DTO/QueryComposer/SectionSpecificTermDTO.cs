using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// A DTO for a term with a section
    /// </summary>
    [DataContract]
    public class SectionSpecificTermDTO
    {
        /// <summary>
        /// Gets or Sets The ID of the Term
        /// </summary>
        [DataMember]
        public Guid TermID { get; set; }

        /// <summary>
        /// Gets or Sets Whether the term belongs to the criteria (0) or stratification (1) section
        /// </summary>
        [DataMember]
        public QueryComposerSections Section { get; set; }
    }
}
