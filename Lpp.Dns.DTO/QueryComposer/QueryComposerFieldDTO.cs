using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer {
    /// <summary>
    /// Query composer Fields
    /// </summary>
    [DataContract]
    public class QueryComposerFieldDTO
    {
        /// <summary>
        /// Query composer field DTO
        /// </summary>
        public QueryComposerFieldDTO()
        {
            OrderBy = OrderByDirections.None;
        }
        /// <summary>
        /// Gets or set the Field Name
        /// </summary>
        [DataMember]
        public string FieldName { get; set; }
        /// <summary>
        /// Gets or set the Type
        /// </summary>
        [DataMember]
        public Guid Type { get; set; }
        /// <summary>
        /// performs the GroupBy operation
        /// </summary>
        [DataMember, Obsolete("Use StratifyBy instead.")]
        public object GroupBy { get; set; }
        /// <summary>
        /// Indicates stratifications to be applied to the query.
        /// </summary>
        [DataMember]
        public object StratifyBy { get; set; }
        /// <summary>
        /// Gets or set the query composer aggregates
        /// </summary>
        [DataMember]
        public QueryComposerAggregates? Aggregate { get; set; }
        /// <summary>
        /// Available Query composer selections
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerSelectDTO> Select { get; set; }
        /// <summary>
        /// Performs OrderBy operation
        /// </summary>
        [DataMember, DefaultValue(OrderByDirections.None)]
        public OrderByDirections OrderBy { get; set; }
    }
}