using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    [DataContract]
    public abstract class QueryComposerHeaderDTO
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [DataMember]
        public virtual Guid ID { get; set; }
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }
        /// <summary>
        /// Gets or set the Description
        /// </summary>
        [DataMember]
        public virtual string Description { get; set; }
        /// <summary>
        /// Gets or sets the url to view the request criteria. This will typically point to a site that will provide the request details in html format.
        /// </summary>
        [DataMember]
        public virtual string ViewUrl { get; set; }
        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        [DataMember]
        public virtual Enums.Priorities? Priority { get; set; }
        ///<summary>
        /// Due Date
        /// </summary>
        [DataMember]
        public virtual DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Gets or sets the request submission date.
        /// </summary>
        [DataMember]
        public virtual DateTimeOffset? SubmittedOn { get; set; }
    }
}
