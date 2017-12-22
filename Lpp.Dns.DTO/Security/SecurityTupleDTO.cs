using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Security Tuple
    /// </summary>
    [DataContract]
    public class SecurityTupleDTO
    {
        /// <summary>
        /// Gets or sets the ID1
        /// </summary>
        [DataMember]
        public Guid ID1 { get; set; }
        /// <summary>
        /// Gets or sets the ID2
        /// </summary>
        [DataMember]
        public Guid? ID2 { get; set; }
        /// <summary>
        /// Gets or sets the ID3
        /// </summary>
        [DataMember]
        public Guid? ID3 { get; set; }
        /// <summary>
        /// Gets or sets the ID4
        /// </summary>
        [DataMember]
        public Guid? ID4 { get; set; }
        /// <summary>
        /// Gets or sets the ID of subject
        /// </summary>

        [DataMember]
        public Guid SubjectID { get; set; }
        /// <summary>
        /// Gets or sets the ID of privilege
        /// </summary>
        [DataMember]
        public Guid PrivilegeID { get; set; }
        /// <summary>
        /// Gets or sets the membership
        /// </summary>
        [DataMember]
        public int ViaMembership { get; set; }
        /// <summary>
        /// Denied entries
        /// </summary>
        [DataMember]
        public int DeniedEntries { get; set; }
        /// <summary>
        /// Explicit denied entries
        /// </summary>
        [DataMember]
        public int ExplicitDeniedEntries { get; set; }
        /// <summary>
        /// Explicit Allowed entries
        /// </summary>
        [DataMember]
        public int ExplicitAllowedEntries { get; set; }
        /// <summary>
        /// The date the security tuple was changed on
        /// </summary>
        [DataMember]
        public DateTimeOffset ChangedOn { get; set; }
    }
}
