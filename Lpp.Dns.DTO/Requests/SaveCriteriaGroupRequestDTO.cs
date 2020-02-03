using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DTO for initiating a save of a new criteria group.
    /// </summary>
    [DataContract]
    public class SaveCriteriaGroupRequestDTO
    {
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        [DataMember, Lpp.Objects.ValidationAttributes.Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the template.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the criteria group json to save.
        /// </summary>
        [DataMember, Lpp.Objects.ValidationAttributes.Required]
        public string Json { get; set; }
        /// <summary>
        /// Gets or sets the adapter detail that is applicable to the template.
        /// </summary>
        [DataMember]
        public Enums.QueryComposerQueryTypes? AdapterDetail { get; set; }
        /// <summary>
        /// Gets or sets the template ID of the parent template the criteria group is being created from.
        /// </summary>
        [DataMember]
        public Guid? TemplateID { get; set; }
        /// <summary>
        /// Gets or sets the request type of the parent template the criteria group is being created from.
        /// </summary>
        [DataMember]
        public Guid? RequestTypeID { get; set; }
        /// <summary>
        /// Gets or sets the request to determine the parent template the criteria group is being created from.
        /// </summary>
        [DataMember]
        public Guid? RequestID { get; set; }
    }
}
