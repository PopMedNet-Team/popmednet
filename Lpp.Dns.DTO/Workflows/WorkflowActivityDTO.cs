using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Workflow Activity
    /// </summary>
    [DataContract]
    public class WorkflowActivityDTO : EntityDtoWithID
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, MaxLength(255), Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets if the activity is a starting point for a workflow.
        /// </summary>
        [DataMember]
        public bool Start { get; set; }
        /// <summary>
        /// Gets or sets if the activity is a termination point for a workflow.
        /// </summary>
        [DataMember]
        public bool End { get; set; }

    }
}
