﻿using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// User Registration
    /// </summary>
    [DataContract]
    public class UserRegistrationDTO
    {
        /// <summary>
        /// User Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(50)]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string? Title { get; set; }
        /// <summary>
        /// First Name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Last Name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Middle name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string? MiddleName { get; set; }
        /// <summary>
        /// phone number
        /// </summary>
        [DataMember]
        [MaxLength(50), Phone(ErrorMessage = "Phone Number must be in 123-456-7890 Format")]
        public string? Phone { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        [DataMember]
        [MaxLength(50), Phone]
        public string? Fax { get; set; }
        /// <summary>
        /// user email
        /// </summary>
        [DataMember]
        [MaxLength(400), EmailAddress]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets flag to indicate whether user registration is active
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
        /// <summary>
        /// The date that the user was signed up on
        /// </summary>
        [DataMember]
        public DateTimeOffset? SignedUpOn { get; set; }
        /// <summary>
        /// Requested organization by the user
        /// </summary>
        [DataMember, MaxLength(255)]
        public string? OrganizationRequested { get; set; }
        /// <summary>
        /// Requested role by the user
        /// </summary>
        [DataMember, MaxLength(255)]
        public string? RoleRequested { get; set; }
    }
}
