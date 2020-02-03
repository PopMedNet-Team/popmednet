using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Permission
    /// </summary>
    [DataContract]
    public class PermissionDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(250)]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Available Locations
        /// </summary>

        [DataMember]
        public IEnumerable<PermissionAclTypes> Locations { get; set; }
    }
}
