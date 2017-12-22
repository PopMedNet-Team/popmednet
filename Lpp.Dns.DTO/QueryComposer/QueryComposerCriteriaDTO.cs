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
    /// Query composer criteria
    /// </summary>
    [DataContract]
    public class QueryComposerCriteriaDTO
    {
        /// <summary>
        /// The unique ID of the paragraph
        /// </summary>
        [DataMember]
        public Guid? ID { get; set; }

        /// <summary>
        /// The paragraph this paragraph is related to. 
        /// For time-construct that allows us to map a subsequent event to another event.
        /// </summary>
        [DataMember]
        public Guid? RelatedToID { get; set; }

        /// <summary>
        /// Gets or set the ID of task
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or set the operators of Query composer
        /// </summary>
        [DataMember]
        public QueryComposerOperators Operator { get; set; }
        /// <summary>
        /// Determines the Index event
        /// </summary>
        [DataMember]
        public bool IndexEvent { get; set; }
        /// <summary>
        /// Detemines Exclusion
        /// </summary>
        [DataMember]
        public bool Exclusion { get; set; }
        /// <summary>
        /// Available Query composer criteria's
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerCriteriaDTO> Criteria { get; set; }
        /// <summary>
        /// Available Terms
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerTermDTO> Terms { get; set; }

        /// <summary>
        /// The type of the criteria group
        /// </summary>
        [DataMember]
        public QueryComposerCriteriaTypes Type { get; set; }
    }
}
