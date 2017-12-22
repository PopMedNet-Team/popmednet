using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request Type
    /// </summary>
    [DataContract]
    public class RequestTypeDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if request metadata is filled
        /// </summary>
        [DataMember]
        public bool Metadata { get; set; }
        /// <summary>
        /// post process
        /// </summary>
        [DataMember]
        public bool PostProcess { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if files are added
        /// </summary>
        [DataMember]
        public bool AddFiles { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if request type requires processsing
        /// </summary>
        [DataMember]
        public bool RequiresProcessing { get; set; }
        /// <summary>
        /// Gets or sets the ID of template
        /// </summary>
        [DataMember]
        public Guid? TemplateID { get; set; }
        /// <summary>
        /// Template
        /// </summary>
        [DataMember]
        public string Template { get; set; }
        /// <summary>
        /// Gets or sets the ID of workflow
        /// </summary>
        [DataMember]
        public Guid? WorkflowID { get; set; }
        /// <summary>
        /// Workflow
        /// </summary>
        [DataMember]
        public string Workflow { get; set; }
    }
}
