using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    [DataContract]
    public class AvailableTermsRequestDTO
    {
        /// <summary>
        /// Gets or sets the collection of Adapter ID's the terms must belong to.
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Adapters { get; set; }
        /// <summary>
        /// Gets or sets the adapter detail value.
        /// </summary>
        [DataMember]
        public DTO.Enums.QueryComposerQueryTypes? QueryType { get; set; }
    }
}
