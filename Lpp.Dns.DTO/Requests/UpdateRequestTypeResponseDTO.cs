using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Container class containing the entities updated by a request type save.
    /// </summary>
    [DataContract]
    public class UpdateRequestTypeResponseDTO
    {
        /// <summary>
        /// Gets or set the request type saved.
        /// </summary>
        [DataMember]
        public RequestTypeDTO RequestType { get; set; }

        /// <summary>
        /// Gets or sets the template associated with the request type.
        /// </summary>
        [DataMember]
        public TemplateDTO Template { get; set; }
    }
}
