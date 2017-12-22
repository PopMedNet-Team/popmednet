using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DataMart List
    /// </summary>
    [DataContract]
    public class DataMartListDTO : EntityDtoWithID
    {
        /// <summary>
        /// DataMart Name
        /// </summary>
        [DataMember]
        [MaxLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// DataMart Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// DataMart Acronym
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string Acronym { get; set; }
        /// <summary>
        /// The date the DataMart was created on
        /// </summary>
        [DataMember]
        public DateTimeOffset? StartDate { get; set; }
        /// <summary>
        /// The date the DataMart was ended on
        /// </summary>
        [DataMember]
        public DateTimeOffset? EndDate { get; set; }
        /// <summary>
        /// Identifier of Organization
        /// </summary>
        [DataMember]
        public Guid? OrganizationID { get; set; }
        /// <summary>
        /// Organization
        /// </summary>
        [DataMember]
        public string Organization { get; set; }
        /// <summary>
        /// Gets or set the ID of the Organizations parent organization.
        /// </summary>
        [DataMember]
        public Guid? ParentOrganziationID { get; set; }
        /// <summary>
        /// Gets or sets the name of the Organizations parent organization.
        /// </summary>
        [DataMember]
        public string ParentOrganization { get; set; }
        /// <summary>
        /// Priority
        /// </summary>
        public Enums.Priorities Priority { get; set; }
        /// <summary>
        /// Due Date
        /// </summary>
        public DateTime DueDate { get; set; }
    }
}
