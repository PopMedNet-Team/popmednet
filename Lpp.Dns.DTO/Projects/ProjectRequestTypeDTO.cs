using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request types in project
    /// </summary>
    [DataContract]
    public class ProjectRequestTypeDTO : EntityDto
    {
        /// <summary>
        /// Gets or set the ID of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Gets or set the ID of Request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Gets or sets the Request type
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string RequestType { get; set; }
        /// <summary>
        /// Gets or set the ID of workflow
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid? WorkflowID { get; set; }
        /// <summary>
        /// Gets or set the workflow
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Workflow { get; set; }
        /// <summary>
        /// Gets or set the template
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Template { get; set; }
    }
}
