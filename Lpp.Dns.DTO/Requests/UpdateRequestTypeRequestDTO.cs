using Lpp.Dns.DTO.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Container class for holding the request type, template, and associated models and terms to be saved.
    /// </summary>
    [DataContract]
    public class UpdateRequestTypeRequestDTO
    {
        /// <summary>
        /// Gets or set the request type to save.
        /// </summary>
        [DataMember]
        public RequestTypeDTO RequestType { get; set; }


        /// <summary>
        /// Gets or sets the template associated with the request type.
        /// </summary>
        [DataMember]
        public TemplateDTO Template { get; set; }

        /// <summary>
        /// Gets or sets the collection of term ids to associated the request type.
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Terms { get; set; }

        /// <summary>
        /// A collection of the terms that were chosen to be excluded
        /// </summary>
        [DataMember]
        public IEnumerable<SectionSpecificTermDTO> NotAllowedTerms { get; set; }

        /// <summary>
        /// Gets or sets the models to associate with the request type.
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Models { get; set; }
    }
}
