using Lpp.Objects;
using System;
using System.Collections.Generic;
using Lpp.Objects.ValidationAttributes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Workflow
    /// </summary>
    [DataContract]
    public class WorkflowDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember, MaxLength(255), Required]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
