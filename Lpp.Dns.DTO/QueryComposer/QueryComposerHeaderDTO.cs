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
    public class QueryComposerHeaderDTO
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or set the Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the url to view the request criteria. This will typically point to a site that will provide the request details in html format.
        /// </summary>
        [DataMember]
        public string ViewUrl { get; set; }
        /// <summary>
        /// Gets or sets the grammar.
        /// </summary>
        [DataMember]
        public string Grammar { get; set; }
        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        [DataMember]
        public Lpp.Dns.DTO.Enums.Priorities? Priority { get; set; }
        ///<summary>
        /// Due Date
        /// </summary>
        [DataMember]
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Gets or sets the sub type of the request.
        /// </summary>
        [DataMember]
        public Enums.QueryComposerQueryTypes? QueryType { get; set; }
    }
}
