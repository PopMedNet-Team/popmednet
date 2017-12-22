using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Template DTO
    /// </summary>
    [DataContract]
    public class TemplateDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the template description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// The ID of the user that created the template
        /// </summary>
        [DataMember]
        public Guid? CreatedByID { get; set; }
        /// <summary>
        /// Username of the person that created the template
        /// </summary>
        [DataMember]
        public string CreatedBy { get; set; }
        /// <summary>
        /// The date the template was created on
        /// </summary>
        [DataMember]
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        [DataMember]
        public string Data { get; set; }
        /// <summary>
        /// Gets or set the type of Template types 
        /// </summary>
        [DataMember]
        public TemplateTypes Type { get; set; }
        /// <summary>
        /// Gets or sets notes for the template.
        /// </summary>
        [DataMember]
        public string Notes { get; set; }
        /// <summary>
        /// Gets or sets the query subtype.
        /// </summary>
        [DataMember]
        public Enums.QueryComposerQueryTypes? QueryType { get; set; }
        /// <summary>
        /// Gets or sets the query composer interface capability
        /// </summary>
        [DataMember]
        public Enums.QueryComposerInterface? ComposerInterface { get; set; }
    }
}
