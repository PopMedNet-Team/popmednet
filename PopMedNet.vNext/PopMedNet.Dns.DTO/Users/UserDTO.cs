using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Represents the User.
    /// </summary>
    [DataContract]
    public class UserDTO : EntityDtoWithID, ISupportsSoftDelete
    {
        /// <summary>
        /// Gets or sets the UserName of the User.
        /// </summary>
        [DataMember]
        [Required, MaxLength(50)]
        public string? UserName { get; set; }
        /// <summary>
        /// Gets or sets the Title of the User.
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string? Title { get; set; }
        /// <summary>
        /// Gets or sets the First Name of the User.
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string? FirstName { get; set; }
        /// <summary>
        /// Gets or sets the Last Name of the User.
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string? LastName { get; set; }
        /// <summary>
        /// Gets or sets the Middle Name of the User.
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string? MiddleName { get; set; }
        /// <summary>
        /// Gets or sets the Phone Number of the User.
        /// </summary>
        [DataMember]
        [MaxLength(50), Phone(ErrorMessage = "Phone Number must be in 123-456-7890 Format")]
        public string? Phone { get; set; }
        /// <summary>
        /// Gets or sets the Fax Number of the User.
        /// </summary>
        [DataMember]
        [MaxLength(50), Phone]
        public string? Fax { get; set; }
        /// <summary>
        /// Gets or sets the Email of the User.
        /// </summary>
        [DataMember]
        [MaxLength(400), EmailAddress]
        public string? Email { get; set; }
        /// <summary>
        /// Gets or sets if the User is Active.
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
        /// <summary>
        /// Gets or sets if the User is Deleted.
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of the Organization the User belongs to.
        /// </summary>
        [DataMember]
        public Guid? OrganizationID { get; set; }
        /// <summary>
        /// Gets or sets the Name of the Organization the User belongs to.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Organization { get; set; }
        /// <summary>
        /// Gets or sets the Organization the User requested during Registration.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? OrganizationRequested { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of the Role of the User.
        /// </summary>
        [DataMember]
        public Guid? RoleID { get; set; }
        /// <summary>
        /// Gets or sets the Role the User requested during Registration.
        /// </summary>
        [DataMember]
        public string? RoleRequested { get; set; }
        /// <summary>
        /// Gets or sets the Date the user registered on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? SignedUpOn { get; set; }
        /// <summary>
        /// Gets or sets the date the user was Activated on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? ActivatedOn { get; set; }
        /// <summary>
        /// Gets or sets the date the user was Deactivated on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? DeactivatedOn { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of the User who deactivated this User.
        /// </summary>
        [DataMember]
        public Guid? DeactivatedByID { get; set; }
        /// <summary>
        /// Gets or sets the name of the user who deactivated this User.
        /// </summary>
        [DataMember]
        public virtual string? DeactivatedBy { get; set; }
        /// <summary>
        /// Gets or sets the reason why the User was deactivated.
        /// </summary>
        [DataMember]
        public string? DeactivationReason { get; set; }
        /// <summary>
        /// Gets or sets the reason given that this user was rejected during registration.
        /// </summary>
        [DataMember, MaxLength(500)]
        public string? RejectReason { get; set; }
        /// <summary>
        /// Gets or sets the date when this user was rejected.
        /// </summary>
        [DataMember]
        public DateTimeOffset? RejectedOn { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of the User who rejected this User.
        /// </summary>
        [DataMember]
        public Guid? RejectedByID { get; set; }
        /// <summary>
        /// Gets or sets the name of the user who rejected this User.
        /// </summary>
        [DataMember]
        public string? RejectedBy { get; set; }
        
        public string? FullName
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    FirstName,
                     string.IsNullOrEmpty(FirstName) ? MiddleName : string.Format(" {0}", MiddleName),
                    (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(MiddleName)) ? LastName : string.Format(" {0}", LastName)
                    ).Trim();
            }
        }
    }
}

