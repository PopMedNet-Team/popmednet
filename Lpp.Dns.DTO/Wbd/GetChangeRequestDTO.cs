using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Get change request DTO
    /// </summary>
    [DataContract]
    public class GetChangeRequestDTO
    {
        /// <summary>
        /// Last checked
        /// </summary>
        [DataMember]
        public DateTimeOffset LastChecked {get; set;}
        /// <summary>
        /// Provider id's
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> ProviderIDs { get; set; }
    }
}
