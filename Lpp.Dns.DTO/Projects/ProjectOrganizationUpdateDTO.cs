using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Project Organization update
    /// </summary>
    [DataContract]
    public class ProjectOrganizationUpdateDTO 
    {
        /// <summary>
        /// Gets or sets the ID of Project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Available Organizations in Project
        /// </summary>
        [DataMember]
        public IEnumerable<ProjectOrganizationDTO> Organizations { get; set; }
    }
}
