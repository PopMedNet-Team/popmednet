using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Registry Events
    /// </summary>
    [DataContract]
    public class RegistryEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the Registry
        /// </summary>
        [DataMember]
        public Guid RegistryID { get; set; }
    }
}
