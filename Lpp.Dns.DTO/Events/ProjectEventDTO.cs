using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Project Events
    /// </summary>
    [DataContract]
    public class ProjectEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the Project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
    }
}
