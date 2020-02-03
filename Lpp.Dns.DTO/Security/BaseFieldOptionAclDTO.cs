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
    /// Base ACL Field Option
    /// </summary>
    [DataContract]
    public class BaseFieldOptionAclDTO : EntityDto
    {
        /// <summary>
        /// Gets or sets the ID of Field
        /// </summary>
        [DataMember]
        public string FieldIdentifier { get; set; }
        /// <summary>
        /// Field Permissions
        /// </summary>
        [DataMember]
        public Enums.FieldOptionPermissions Permission { get; set; }

        /// <summary>
        /// Gets or sets the indicator to specify if Base ACL is overridden
        /// </summary>
        [DataMember]
        public bool Overridden { get; set; }

        /// <summary>
        /// Gets or sets the Secuirty Group ID for the field
        /// </summary>
        [DataMember]
        public Guid SecurityGroupID { get; set; }

        /// <summary>
        /// Gets or sets the Security Group name for the field
        /// </summary>
        [DataMember]
        public string SecurityGroup { get; set; }
    }
}
