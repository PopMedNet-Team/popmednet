using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer GroupBy
    /// </summary>
    [DataContract]
    public class QueryComposerGroupByDTO
    {
        /// <summary>
        /// Gets or Sets the name of Field
        /// </summary>
        [DataMember]
        public string Field { get; set; }
        /// <summary>
        /// Gets or sets the Querycomposer Aggregate
        /// </summary>
        [DataMember]
        public QueryComposerAggregates Aggregate { get; set; }
    }
}
