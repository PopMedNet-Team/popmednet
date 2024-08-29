using PopMedNet.Utilities;
using PopMedNet.Utilities.Objects;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("Users")]
    public partial class User : EntityWithID, IUser, IEntityWithName, IEntityWithDeleted, ISupportsSoftDelete
    {
        public User()
        {
            this.FailedLoginCount = 0;
            this.PasswordEncryptionLength = 14;
            this.Active = false;
            this.Deleted = false;
            this.PasswordVersion = 1;
            this.PasswordExpiration = DateTime.UtcNow.AddYears(1);
        }

        [Required, MaxLength(50)]
        public string UserName { get; set; } = "";

        [MaxLength(100), Column("Password")]
        public string? PasswordHash { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string? Title { get; set; }

        [MaxLength(100), Required]
        public string? FirstName { get; set; }

        [MaxLength(100), Required]
        public string? LastName { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets the User's full name in the format: {First} {Middle} {Last}.
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    FirstName,
                    FirstName.IsNullOrEmpty() ? MiddleName : string.Format(" {0}", MiddleName),
                    (FirstName.IsNullOrEmpty() && MiddleName.IsNullOrEmpty()) ? LastName : string.Format(" {0}", LastName)
                    ).Trim();
            }
        }

        [NotMapped]
        string IEntityWithName.Name
        {
            get
            {
                return FullName;
            }
        }

        public int PasswordEncryptionLength { get; set; } = 14;

        public int PasswordVersion { get; set; } = 1;

        public DateTime? LastUpdatedOn { get; set; }

        [MaxLength(50), Phone]
        public string? Phone { get; set; }

        [MaxLength(50), Phone]
        public string? Fax { get; set; }

        [MaxLength(400), EmailAddress]
        public string? Email { get; set; }

        public DateTime PasswordExpiration { get; set; } = DateTime.UtcNow.AddYears(1);

        public Guid? PasswordRestorationToken { get; set; }

        public DateTime? PasswordRestorationTokenExpiration { get; set; }

        public string? X509CertificateThumbprint { get; set; }

        [Column("isActive")]
        public bool Active { get; set; } = false;

        public DateTime? SignedUpOn { get; set; }

        public DateTime? ActivatedOn { get; set; }

        public DateTime? DeactivatedOn { get; set; }
        public Guid? DeactivatedByID { get; set; }
        public virtual User? DeactivatedBy { get; set; }
        public string? DeactivationReason { get; set; }

        public int FailedLoginCount { get; set; } = 0;

        [MaxLength(500)]
        public string? RejectReason { get; set; }
        public DateTime? RejectedOn { get; set; }
        public Guid? RejectedByID { get; set; }
        public virtual User? RejectedBy { get; set; }

        [MaxLength(255)]
        public string? RoleRequested { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; } = false;

        public Guid? OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }

        [MaxLength(255)]
        public string? OrganizationRequested { get; set; }

        public Guid? RoleID { get; set; }
        public virtual Role? Role { get; set; }

        public UserTypes UserType { get; set; } = UserTypes.User;

        public virtual ICollection<SecurityGroupUser> SecurityGroups { get; set; } = new HashSet<SecurityGroupUser>();

        public virtual ICollection<Response> SubmittedResponses { get; set; }

        public virtual ICollection<Response> RespondedResponses { get; set; }

        public virtual ICollection<Request> CreatedRequests { get; set; }
        public virtual ICollection<Request> UpdatedRequests { get; set; }
        public virtual ICollection<Request> SubmittedRequests { get; set; }
        public virtual ICollection<Request> DraftApprovalRequests { get; set; }
        public virtual ICollection<Request> RejectedRequests { get; set; }
        public virtual ICollection<Request> CancelledRequests { get; set; }
        public virtual ICollection<Template> Templates { get; set; }

        public virtual ICollection<PmnTaskUser> Actions { get; set; }

        public virtual ICollection<AclUser> UserAcls { get; set; } = new HashSet<AclUser>();
        public virtual ICollection<UserEvent> UserEvents { get; set; }

        public virtual ICollection<UserSetting> Settings { get; set; }
        public virtual ICollection<UserEventSubscription> Subscriptions { get; set; }

        public virtual ICollection<Audit.UserChangeLog> UserChangeLogs { get; set; }
        public virtual ICollection<Audit.UserPasswordChangeLog> UserPasswordLogs { get; set; }
        public virtual ICollection<Audit.UserPasswordChangeLog> ChangedUserPasswordLogs { get; set; }
        public virtual ICollection<Audit.ProfileUpdatedLog> ProfileUpdatedLogs { get; set; }
        public virtual ICollection<Audit.PasswordExpirationLog> PasswordExpirationLogs { get; set; }
        public virtual ICollection<Audit.UserRegistrationSubmittedLog> RegistrationSubmittedLogs { get; set; }
        public virtual ICollection<Audit.UserRegistrationChangedLog> RegistrationChangedLogs { get; set; }
        public virtual ICollection<User> RejectedUsers { get; set; } = new HashSet<User>();
        public virtual ICollection<User> DeactivatedUsers { get; set; } = new HashSet<User>();

        public virtual ICollection<Document> UploadedDocuments { get; set; }

        public virtual ICollection<NetworkMessageUser> NetworkMessages { get; set; }

        string IUser.LastOrCompanyName
        {
            get => LastName ?? string.Empty;
            set
            {
                LastName = value;
            }
        }

        /// <summary>
        /// Checks if a password is strong enough to be set.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordScores CheckPasswordStrength(string password)
        {
            if (password == null || password.Contains(":") || password.Contains(";") || password.Contains("<"))
                return PasswordScores.Invalid;

            int score = 1;
            if (password.Length < 1)
                return PasswordScores.Blank;
            if (password.Length < 4)
                return PasswordScores.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"\d", System.Text.RegularExpressions.RegexOptions.ECMAScript))   //number only
                score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", System.Text.RegularExpressions.RegexOptions.ECMAScript)) //both, lower and upper case
                score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[`,!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", System.Text.RegularExpressions.RegexOptions.ECMAScript)) //^[A-Z]+$
                score++;

            if (score >= 5)
            {
                return PasswordScores.VeryStrong;
            }
            else
            {
                return (PasswordScores)score;
            }
        }
    }

    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //use the build to configure the object
            builder.HasOne(u => u.RejectedBy).WithMany(r => r.RejectedUsers).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(u => u.DeactivatedBy).WithMany(d => d.DeactivatedUsers).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(u => u.SecurityGroups).WithOne(sgu => sgu.User).HasForeignKey(sgu => sgu.UserID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.UserAcls).WithOne(t => t.User).IsRequired(true).HasForeignKey(t => t.UserID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Templates).WithOne(t => t.CreatedBy).IsRequired(true).HasForeignKey(t => t.CreatedByID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.SubmittedResponses).WithOne(t => t.SubmittedBy).IsRequired(true).HasForeignKey(t => t.SubmittedByID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.RespondedResponses).WithOne(t => t.RespondedBy).IsRequired(false).HasForeignKey(t => t.RespondedByID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.CreatedRequests).WithOne(t => t.CreatedBy).IsRequired(true).HasForeignKey(t => t.CreatedByID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.UpdatedRequests).WithOne(t => t.UpdatedBy).IsRequired(true).HasForeignKey(t => t.UpdatedByID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.SubmittedRequests).WithOne(t => t.SubmittedBy).IsRequired(false).HasForeignKey(t => t.SubmittedByID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DraftApprovalRequests).WithOne(t => t.ApprovedForDraftBy).IsRequired(false).HasForeignKey(t => t.ApprovedForDraftByID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.RejectedRequests).WithOne(t => t.RejectedBy).IsRequired(false).HasForeignKey(t => t.RejectedByID).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(t => t.CancelledRequests)
                .WithOne(t => t.CancelledBy)
                .IsRequired(false)
                .HasForeignKey(t => t.CancelledByID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(t => t.UserEvents)
                .WithOne(t => t.User)
                .IsRequired(true)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Settings)
                .WithOne(t => t.User)
                .IsRequired(true)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Subscriptions)
                .WithOne(t => t.User)
                .IsRequired(true)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.UserChangeLogs)
                .WithOne(t => t.UserChanged)
                .IsRequired(true)
                .HasForeignKey(t => t.UserChangedID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Actions)
                .WithOne(t => t.User)
                .IsRequired(true)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProfileUpdatedLogs)
                .WithOne(t => t.UserChanged)
                .IsRequired(true)
                .HasForeignKey(t => t.UserChangedID)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(t => t.PasswordExpirationLogs)
                .WithOne(t => t.ExpiringUser)
                .IsRequired(true)
                .HasForeignKey(t => t.ExpiringUserID)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(t => t.RegistrationSubmittedLogs)
                .WithOne(t => t.RegisteredUser)
                .IsRequired(true)
                .HasForeignKey(t => t.RegisteredUserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RegistrationChangedLogs)
                .WithOne(t => t.RegisteredUser)
                .IsRequired(true)
                .HasForeignKey(t => t.RegisteredUserID)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(t => t.ChangedUserPasswordLogs)
                .WithOne(x => x.User)
                .IsRequired(true)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.UserPasswordLogs)
                .WithOne(x => x.UserChanged)
                .IsRequired(true)
                .HasForeignKey(x => x.UserChangedID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.UploadedDocuments)
                .WithOne(t => t.UploadedBy)
                .IsRequired(false)
                .HasForeignKey(t => t.UploadedByID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.NetworkMessages)
                .WithOne(t => t.User)
                .IsRequired(true)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.SignedUpOn, "IX_SignedUpOn").IsClustered(false);
            builder.HasIndex(u => u.ActivatedOn, "IX_ActivatedOn").IsClustered(false);
            builder.HasIndex(u => u.DeactivatedOn, "IX_DeactivatedOn").IsClustered(false);
            builder.HasIndex(u => u.Email, "IX_Email").IsClustered(false);
            builder.HasIndex(u => u.PasswordRestorationToken, "IX_PasswordRestorationToken").IsClustered(false);
            builder.HasIndex(u => u.UserName, "IX_UserName").IsClustered(false);

            builder.Property(u => u.UserType).HasConversion<int>();
        }
    }

    internal class UserSecurityConfiguration : DnsEntitySecurityConfiguration<User>
    {
        public override IQueryable<User> SecureList(DataContext db, IQueryable<User> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.User.View
                };

            var result1 = db.Users.Where(u => u.ID == identity.ID);

            var result2 = db.Filter(query, identity, permissions);

            return result1.Concat(result2).Distinct();
        }

        public override async Task<bool> CanInsert(DataContext db, ApiIdentity identity, params User[] objs)
        {
            var organizationIDs = objs.Select(u => u.OrganizationID ?? Guid.Empty).ToArray();

            var aclOrgs = db.OrganizationAcls.FilterAcl(identity, PermissionIdentifiers.Organization.CreateUsers).Where(a => organizationIDs.Contains(a.OrganizationID));

            var aclGlobal = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Organization.CreateUsers);

            return (await aclOrgs.AnyAsync() && await aclOrgs.AllAsync(a => a.Allowed)) || (await aclGlobal.AnyAsync() && await aclGlobal.AllAsync(a => a.Allowed));
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.User.Delete);
        }

        public override async Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return (keys.Length == 1 && keys[0] == identity.ID) || await HasPermissions(db, identity, keys, PermissionIdentifiers.User.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclUser, bool>> UserFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.UserID);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Users.Any(u => objIDs.Contains(u.ID));
        }
    }

    public class UserMappingProfile : AutoMapper.Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, DTO.UserDTO>()
                .ForMember(d => d.Organization, opt => opt.MapFrom(src => src.Organization!.Name))
                .ForMember(d => d.DeactivatedBy, opt => opt.MapFrom(src => src.DeactivatedBy == null ? string.Empty : $"{ src.DeactivatedBy.FirstName } { src.DeactivatedBy.LastName }"))
                .ForMember(d => d.RejectedBy, opt => opt.MapFrom(src => src.RejectedBy == null ? string.Empty : $"{ src.RejectedBy.FirstName } { src.RejectedBy.LastName }"))
                .ForMember(d => d.SignedUpOn, opt => opt.MapFrom(src => src.SignedUpOn.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.SignedUpOn.Value, TimeSpan.Zero) : null))
                .ForMember(d => d.ActivatedOn, opt => opt.MapFrom(src => src.ActivatedOn.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.ActivatedOn.Value, TimeSpan.Zero) : null))
                .ForMember(d => d.DeactivatedOn, opt => opt.MapFrom(src => src.DeactivatedOn.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.DeactivatedOn.Value, TimeSpan.Zero) : null))
                .ForMember(d => d.RejectedOn, opt => opt.MapFrom(src => src.RejectedOn.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.RejectedOn.Value, TimeSpan.Zero) : null));

            CreateMap<DTO.UserDTO, User>()
                .ForMember(u => u.Timestamp, opt => opt.Ignore())
                .ForMember(u => u.Organization, opt => opt.Ignore())
                .ForMember(u => u.FullName, opt => opt.Ignore())
                .ForMember(u => u.RejectedBy, opt => opt.Ignore())
                .ForMember(u => u.DeactivatedBy, opt => opt.Ignore())
                .ForMember(u => u.SignedUpOn, opt => opt.MapFrom(src => src.SignedUpOn.HasValue ? (DateTime?)src.SignedUpOn.Value.DateTime : null))
                .ForMember(u => u.ActivatedOn, opt => opt.MapFrom(src => src.ActivatedOn.HasValue ? (DateTime?)src.ActivatedOn.Value.DateTime : null))
                .ForMember(u => u.RejectedOn, opt => opt.MapFrom(src => src.RejectedOn.HasValue ? (DateTime?)src.RejectedOn.Value.DateTime : null))
                .ForMember(u => u.DeactivatedOn, opt => opt.MapFrom(src => src.DeactivatedOn.HasValue ? (DateTime?)src.DeactivatedOn.Value.DateTime : null));
        }
    }
}
