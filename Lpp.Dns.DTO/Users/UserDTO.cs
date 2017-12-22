using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// User
    /// </summary>
    [DataContract]
    public class UserDTO : EntityDtoWithID, ISupportsSoftDelete
    {
        /// <summary>
        /// User Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(50)]
        public string UserName { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// User First Name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string FirstName { get; set; }
        /// <summary>
        /// User Last Name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string LastName { get; set; }
        /// <summary>
        /// user Middle name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string MiddleName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        [DataMember]
        [MaxLength(50), Phone(ErrorMessage="Phone Number must be in 123-456-7890 Format")]
        public string Phone { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        [DataMember]
        [MaxLength(50), Phone]
        public string Fax { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        [MaxLength(400), EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// To indicate whether the user is Active
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
        /// <summary>
        /// To indicate whether the user is deleted
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// ID of an organization
        /// </summary>
        [DataMember]
        public Guid? OrganizationID { get; set; }
        /// <summary>
        /// Organization
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Organization { get; set; }
        /// <summary>
        /// Requested Organization
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string OrganizationRequested { get; set; }
        /// <summary>
        /// ID of Role
        /// </summary>
        [DataMember]
        public Guid? RoleID { get; set; }
        /// <summary>
        /// Requested role
        /// </summary>
        [DataMember]
        public string RoleRequested { get; set; }
        /// <summary>
        /// The date the user was signed up on
        /// </summary>
        [DataMember]
        public DateTimeOffset? SignedUpOn { get; set; }
        /// <summary>
        /// The date the user was activated on
        /// </summary>
        [DataMember]
        public DateTimeOffset? ActivatedOn { get; set; }
        /// <summary>
        /// Th date the user was deactivated on
        /// </summary>
        [DataMember]
        public DateTimeOffset? DeactivatedOn { get; set; }
        /// <summary>
        /// Gets or sets the ID of Deactivated by
        /// </summary>
        [DataMember]
        public Guid? DeactivatedByID { get; set; }
        /// <summary>
        /// Gets or sets the name who dactivated the account
        /// </summary>
        [DataMember]
        public virtual string DeactivatedBy { get; set; }
        /// <summary>
        /// Reason of de-activating the user account
        /// </summary>
        [DataMember]
        public string DeactivationReason { get; set; }
        /// <summary>
        /// Reason for rejection
        /// </summary>

        [DataMember, MaxLength(500)]
        public string RejectReason { get; set; }
        /// <summary>
        /// The date that the user was rejected on
        /// </summary>
        [DataMember]
        public DateTimeOffset? RejectedOn { get; set; }
        /// <summary>
        /// ID of Rejected By
        /// </summary>
        [DataMember]
        public Guid? RejectedByID { get; set; }
        /// <summary>
        /// Rejected By
        /// </summary>
        [DataMember]
        public string RejectedBy { get; set; }
    }
}
