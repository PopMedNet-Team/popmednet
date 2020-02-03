using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// This class supports the where clause for Query composer
    /// </summary>
    [DataContract]
    public class QueryComposerWhereDTO
    {
        /// <summary>
        /// Query composer criteria
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerCriteriaDTO> Criteria { get; set; }
    }
}
