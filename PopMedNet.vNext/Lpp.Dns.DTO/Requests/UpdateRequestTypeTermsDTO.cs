using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Defines a terms collection update for a specific RequestType.
    /// </summary>
    [DataContract]
    public class UpdateRequestTypeTermsDTO
    {
        /// <summary>
        /// Gets or sets the ID of the RequestType to update the terms for.
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Gets or sets the collection of term IDs to be associated with the RequestType.
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Terms { get; set; }
    }
}
