using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer Header
    /// </summary>
    [DataContract]
    public class QueryComposerQueryHeaderDTO : QueryComposerHeaderDTO
    {
        /// <summary>
        /// Gets or sets the sub type of the request.
        /// </summary>
        [DataMember]
        public Enums.QueryComposerQueryTypes? QueryType { get; set; }
        /// <summary>
        /// Gets or sets the type of composition interface to use for the query.
        /// </summary>
        public Enums.QueryComposerInterface? ComposerInterface { get; set; }
    }
}
