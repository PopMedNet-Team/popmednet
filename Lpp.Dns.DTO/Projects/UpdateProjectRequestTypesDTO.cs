using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Update Project request types
    /// </summary>
    [DataContract]
    public class UpdateProjectRequestTypesDTO
    {
        /// <summary>
        /// Gets or set the ID of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Available Request types in Project
        /// </summary>
        [DataMember]
        public IEnumerable<ProjectRequestTypeDTO> RequestTypes { get; set; }
    }
}
