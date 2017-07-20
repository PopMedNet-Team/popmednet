using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Group Events
    /// </summary>
    [DataContract]
    public class GroupEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of Group
        /// </summary>
        [DataMember]
        public Guid GroupID { get; set; }
    }
}
