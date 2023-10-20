using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Lpp.Objects;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Enums;
using System.Web.Configuration;
using System.Data.Entity.Infrastructure;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    [Table("Users")]
    public partial class User : EntityWithID, ISupportsSoftDelete, IUser, IEntityWithName, IEntityWithDeleted, Lpp.Security.ISecurityObject, Lpp.Security.ISecuritySubject
    {
        public User()
        {
            this.FailedLoginCount = 0;
            this.PasswordEncryptionLength = 14;
            this.Active = false;
            this.Deleted = false;
            this.PasswordVersion = 1;
            this.PasswordExpiration = DateTime.UtcNow.AddYears(1);

            NetworkMessages = new HashSet<NetworkMessageUser>();
        }

        [Required, MaxLength(50)]
        public string UserName {get; set;}

        [MaxLength(100), Column("Password")]
        public string PasswordHash { get; set; }

        [MaxLength(100), Column(TypeName="varchar")]
        public string Title { get; set; }

        [MaxLength(100), Required]
        public string FirstName { get; set; }

        [MaxLength(100), Required]
        public string LastName { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string MiddleName { get; set; }

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

        [NotMapped]
        string Lpp.Security.ISecuritySubject.DisplayName
        {
            get
            {
                //originally was Organization.Acronym\\Username, but that will blowup if the Org is not loaded.
                return UserName;
            }
        }

        public int PasswordEncryptionLength { get; set; }
        
        public int PasswordVersion { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        [MaxLength(50), Phone]
        public string Phone { get; set; }

        [MaxLength(50), Phone]
        public string Fax { get; set; }

        [MaxLength(400), EmailAddress]
        public string Email { get; set; }

        public DateTime? PasswordExpiration { get; set; }

        [Index]
        public Guid? PasswordRestorationToken { get; set; }

        public DateTime? PasswordRestorationTokenExpiration { get; set; }

        public string X509CertificateThumbprint { get; set; }

        [Column("isActive")]
        public bool Active { get; set; }

        [Index]
        public DateTime? SignedUpOn { get; set; }

        [Index]
        public DateTime? ActivatedOn { get; set; }
        [Index]
        public DateTime? DeactivatedOn { get; set; }
        public Guid? DeactivatedByID {get; set;}
        public virtual User DeactivatedBy { get; set; }
        public string DeactivationReason { get; set; }

        public int FailedLoginCount { get; set; }

        [MaxLength(500)]
        public string RejectReason { get; set; }
        public DateTime? RejectedOn { get; set; }
        public Guid? RejectedByID { get; set; }
        public virtual User RejectedBy { get; set; }

        [MaxLength(255)]
        public string RoleRequested { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        public Guid? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        [MaxLength(255)]
        public string OrganizationRequested { get; set; }

        public Guid? RoleID { get; set; }
        public virtual Role Role { get; set; }

        public UserTypes UserType { get; set; }

        public virtual ICollection<SecurityGroupUser> SecurityGroups { get; set; }

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

        public virtual ICollection<AclUser> UserAcls { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }

        public virtual ICollection<UserSetting> Settings { get; set; }
        public virtual ICollection<UserEventSubscription> Subscriptions { get; set; }

        public virtual ICollection<Audit.UserChangeLog> UserChangeLogs { get; set; }
        public virtual ICollection<Audit.UserPasswordChangeLog> UserPasswordLogs { get; set; }
        public virtual ICollection<Audit.UserPasswordChangeLog> ChangedUserPassowrdLogs { get; set; }
        public virtual ICollection<Audit.ProfileUpdatedLog> ProfileUpdatedLogs { get; set; }
        public virtual ICollection<Audit.PasswordExpirationLog> PasswordExpirationLogs { get; set; }
        public virtual ICollection<Audit.UserRegistrationSubmittedLog> RegistrationSubmittedLogs { get; set; }
        public virtual ICollection<Audit.UserRegistrationChangedLog> RegistrationChangedLogs { get; set; }
        public virtual ICollection<User> RejectedUsers { get; set; }
        public virtual ICollection<User> DeactivatedUsers { get; set; }

        public virtual ICollection<Document> UploadedDocuments { get; set; }

        public virtual ICollection<NetworkMessageUser> NetworkMessages { get; set; }

        
        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("User");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get
            {
                return ObjectKind;
            }
        }

        string IUser.LastOrCompanyName
        {
            get
            {
                return LastName;
            }
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
            var ps = Password.Strength(password);
            PasswordScores[] passwordStrengthVerdicts = new[] { PasswordScores.VeryWeak, PasswordScores.Weak, PasswordScores.Average, PasswordScores.Strong, PasswordScores.VeryStrong };
            var s = ps * passwordStrengthVerdicts.Length;
            int passwordVerdictIndex = Math.Min((int)s, passwordStrengthVerdicts.Length - 1);
            PasswordScores resultScore = passwordStrengthVerdicts[passwordVerdictIndex];

            if (password == null || password.Contains(":") || password.Contains(";") || password.Contains("<"))
                return PasswordScores.Invalid;

			return resultScore;

			//int score = 1;
			//if (password.Length < 1)
			//	return PasswordScores.Blank;
			//if (password.Length < 4)
			//	return PasswordScores.VeryWeak;

			//if (password.Length >= 8)
			//	score++;
			//if (password.Length >= 12)
			//	score++;
			//if (System.Text.RegularExpressions.Regex.IsMatch(password, @"\d", System.Text.RegularExpressions.RegexOptions.ECMAScript, TimeSpan.FromSeconds(3)))   //number only
			//	score++;
			//if (System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", System.Text.RegularExpressions.RegexOptions.ECMAScript, TimeSpan.FromSeconds(3))) //both, lower and upper case
			//	score++;
			//if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[`,!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", System.Text.RegularExpressions.RegexOptions.ECMAScript, TimeSpan.FromSeconds(3))) //^[A-Z]+$
			//	score++; //  "!,@,#,$,%,^,&,*,?,_,~,-,£,(,)"

			//if (score >= 5)
			//{
			//	return PasswordScores.VeryStrong;
			//}
			//else
			//{
			//	return (PasswordScores)score;
			//}
		}
    }

    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasMany(t => t.Templates).WithRequired(t => t.CreatedBy).HasForeignKey(t => t.CreatedByID).WillCascadeOnDelete(true);
            //Other relationships should be defined here.
            HasMany(t => t.SecurityGroups).WithRequired(t => t.User).HasForeignKey(t => t.UserID).WillCascadeOnDelete(true);

            HasMany(t => t.SubmittedResponses).WithRequired(t => t.SubmittedBy).HasForeignKey(t => t.SubmittedByID).WillCascadeOnDelete(true);
            HasMany(t => t.RespondedResponses).WithOptional(t => t.RespondedBy).HasForeignKey(t => t.RespondedByID).WillCascadeOnDelete(true);

            HasMany(t => t.CreatedRequests).WithRequired(t => t.CreatedBy).HasForeignKey(t => t.CreatedByID).WillCascadeOnDelete(true);
            HasMany(t => t.UpdatedRequests).WithRequired(t => t.UpdatedBy).HasForeignKey(t => t.UpdatedByID).WillCascadeOnDelete(true);
            HasMany(t => t.SubmittedRequests).WithOptional(t => t.SubmittedBy).HasForeignKey(t => t.SubmittedByID).WillCascadeOnDelete(true);

            HasMany(t => t.DraftApprovalRequests).WithOptional(t => t.ApprovedForDraftBy).HasForeignKey(t => t.ApprovedForDraftByID).WillCascadeOnDelete(true);
            HasMany(t => t.RejectedRequests).WithOptional(t => t.RejectedBy).HasForeignKey(t => t.RejectedByID).WillCascadeOnDelete(true);
            HasMany(t => t.CancelledRequests).WithOptional(t => t.CancelledBy).HasForeignKey(t => t.CancelledByID).WillCascadeOnDelete(true);

            HasMany(t => t.UserAcls).WithRequired(t => t.User).HasForeignKey(t => t.UserID).WillCascadeOnDelete(true);
            HasMany(t => t.UserEvents).WithRequired(t => t.User).HasForeignKey(t => t.UserID).WillCascadeOnDelete(true);

            HasMany(t => t.Settings).WithRequired(t => t.User).HasForeignKey(t => t.UserID).WillCascadeOnDelete(true);
            HasMany(t => t.Subscriptions).WithRequired(t => t.User).HasForeignKey(t => t.UserID).WillCascadeOnDelete(true);
            HasMany(t => t.UserChangeLogs).WithRequired(t => t.UserChanged).HasForeignKey(t => t.UserChangedID).WillCascadeOnDelete(true);
            HasMany(t => t.Actions).WithRequired(t => t.User).HasForeignKey(t => t.UserID).WillCascadeOnDelete(true);
            HasMany(t => t.RejectedUsers).WithOptional(t => t.RejectedBy).HasForeignKey(t => t.RejectedByID).WillCascadeOnDelete(false);
            HasMany(t => t.DeactivatedUsers).WithOptional(t => t.DeactivatedBy).HasForeignKey(t => t.DeactivatedByID).WillCascadeOnDelete(false);
            HasMany(t => t.ProfileUpdatedLogs).WithRequired(t => t.UserChanged).HasForeignKey(t => t.UserChangedID).WillCascadeOnDelete(true);
            HasMany(t => t.PasswordExpirationLogs).WithRequired(t => t.ExpiringUser).HasForeignKey(t => t.ExpiringUserID).WillCascadeOnDelete(true);
            HasMany(t => t.RegistrationSubmittedLogs).WithRequired(t => t.RegisteredUser).HasForeignKey(t => t.RegisteredUserID).WillCascadeOnDelete(true);
            HasMany(t => t.RegistrationChangedLogs).WithRequired(t => t.RegisteredUser).HasForeignKey(t => t.RegisteredUserID).WillCascadeOnDelete(true);
            HasMany(t => t.ChangedUserPassowrdLogs).WithRequired(x => x.User).HasForeignKey(x => x.UserID).WillCascadeOnDelete(false);
            HasMany(t => t.UserPasswordLogs).WithRequired(x => x.UserChanged).HasForeignKey(x => x.UserChangedID).WillCascadeOnDelete(false);

            HasMany(t => t.UploadedDocuments).WithOptional(t => t.UploadedBy).HasForeignKey(t => t.UploadedByID).WillCascadeOnDelete(false);

            HasMany(t => t.NetworkMessages)
                .WithRequired(t => t.User)
                .HasForeignKey(t => t.UserID)
                .WillCascadeOnDelete(true);
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

    internal class UserDtoMappingConfiguration : EntityMappingConfiguration<User, UserDTO>
    {
        public override System.Linq.Expressions.Expression<Func<User, UserDTO>> MapExpression
        {
            get
            {
                return (u) => new UserDTO
                {
                    ActivatedOn = u.ActivatedOn,
                    Active = u.Active,
                    Deleted = u.Deleted,
                    Email = u.Email,
                    Fax = u.Fax,
                    FirstName = u.FirstName,
                    ID = u.ID,
                    LastName = u.LastName,
                    MiddleName = u.MiddleName,
                    Organization = u.Organization.Name,
                    OrganizationID = u.OrganizationID,
                    Phone = u.Phone,
                    RoleID = u.RoleID,
                    SignedUpOn = u.SignedUpOn,
                    Timestamp = u.Timestamp,
                    Title = u.Title,
                    UserName = u.UserName,
                    RejectedBy = u.RejectedByID == null ? "" : u.RejectedBy.FirstName + " " + u.RejectedBy.LastName,
                    RejectedOn = u.RejectedOn,
                    RejectReason = u.RejectReason,
                    DeactivatedBy = u.DeactivatedByID == null ? "" : u.DeactivatedBy.FirstName + " " + u.DeactivatedBy.LastName,
                    DeactivatedOn = u.DeactivatedOn,
                    DeactivationReason = u.DeactivationReason,
                    OrganizationRequested = u.OrganizationRequested,
                    RoleRequested = u.RoleRequested
                };
            }
        }
    }

    internal class UserLogConfiguration : EntityLoggingConfiguration<DataContext, User>
    {
        public override IEnumerable<AuditLog> ProcessEvents(DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();

            var user = obj.Entity as User;
            user.Organization = db.Organizations.Where(o => o.ID == user.OrganizationID).FirstOrDefault();
            if (user == null)
                throw new InvalidCastException("The entity passed is not an user");

            //Registration Submitted
            // A registrant has a signed up date and a created user does not.
            // A registrant is notified only the first time by checking ActivatedOn.
            if (obj.State == EntityState.Added && user.ActivatedOn == null && user.SignedUpOn != null && !user.Active)
            {                
                var userRegistrationSubmittedLogItem = new Audit.UserRegistrationSubmittedLog
                {
                    Description = string.Format("User '{0}' submitted a registration", (string.IsNullOrWhiteSpace(user.OrganizationRequested) ? "(No Organization Requested)" : user.OrganizationRequested) + @"\" + user.UserName),
                    RegisteredUser = user,
                    RegisteredUserID = user.ID,
                    UserID = identity == null ? Guid.Empty : identity.ID
                };
                db.LogsUserRegistrationSubmitted.Add(userRegistrationSubmittedLogItem);
                logs.Add(userRegistrationSubmittedLogItem);

                var userRegistrationSubmittedLogItem2 = new Audit.UserRegistrationSubmittedLog
                {
                    Description = string.Format("Your registration for {0} is acknowledged", (string.IsNullOrWhiteSpace(user.OrganizationRequested) ? "(No Organization Requested)" : user.OrganizationRequested) + @"\" + user.UserName),
                    RegisteredUser = user,
                    RegisteredUserID = user.ID,
                    UserID = user.ID
                };
                db.LogsUserRegistrationSubmitted.Add(userRegistrationSubmittedLogItem2);
                logs.Add(userRegistrationSubmittedLogItem2);
            }

            //Registration Status changed
            // A registrant has a signed up date and a created user does not.
            // This notification should only occur the first time a registration is activated or rejected.
            if (obj.State == EntityState.Modified && identity != null && user.SignedUpOn != null)
            {
                DateTime? origActivateDate = (DateTime?)obj.OriginalValues["ActivatedOn"];
                DateTime? currActivateDate = (DateTime?)obj.CurrentValues["ActivatedOn"];
                DateTime? origRejectDate = (DateTime?)obj.OriginalValues["RejectedOn"];
                DateTime? currRejectDate = (DateTime?)obj.CurrentValues["RejectedOn"];

                if ((origActivateDate.IsNull() && !currActivateDate.IsNull()) || (origRejectDate.IsNull() && !currRejectDate.IsNull()))
                {
                    var userRegistrationChangedLogItem = new Audit.UserRegistrationChangedLog
                    {
                        Description = "The registration status of " + user.UserName + " was changed from <b>Submitted</b> to " + (user.RejectedOn.HasValue ? "Rejected" : "Approved"),
                        RegisteredUser = user,
                        RegisteredUserID = user.ID,
                        UserID = identity == null ? Guid.Empty : identity.ID
                    };
                    db.LogsUserRegistrationChanged.Add(userRegistrationChangedLogItem);
                    logs.Add(userRegistrationChangedLogItem);

                    var userRegistrationChangedLogItem2 = new Audit.UserRegistrationChangedLog
                    {
                        Description = "Your registration for " + user.UserName + " was " + (user.RejectedOn.HasValue ? "Rejected" : "Approved"),
                        RegisteredUser = user,
                        RegisteredUserID = user.ID,
                        UserID = user.ID
                    };
                    db.LogsUserRegistrationChanged.Add(userRegistrationChangedLogItem2);
                    logs.Add(userRegistrationChangedLogItem2);
                }
            }

            if (user.OrganizationID == null || identity == null)
                return logs;

            string message = string.Format("User '{0}' has been {1}", (user.Organization.Acronym + @"\" + user.UserName), obj.State);
            if (identity != null)
            {
                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault() ?? new { UserName = "<unknown>", Acronym = "<unknown>" };
                message = string.Format("User '{0}' has been {1} by {2}", (user.Organization.Acronym + @"\" + user.UserName), obj.State, (orgUser.Acronym + @"\" + orgUser.UserName));
            }

            var logItem = new Audit.UserChangeLog
            {
                Description = message,
                Reason = obj.State,
                UserID = identity == null ? Guid.Empty : identity.ID,
                UserChangedID = user.ID,
                UserChanged = user
            };

            db.LogsUserChange.Add(logItem);
            logs.Add(logItem);

            if (user.Deleted)
                return logs;

            if (identity != null && user.Active)
            {
                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault() ?? new
				{
					UserName = "<unknown>",
					Acronym = "<unknown>"
				};

                //Handle all of the profile specific notifications
                var profileLogItem = new Audit.ProfileUpdatedLog
                {
                    Description = string.Format("Profile of user '{0}' has been updated by '{1}'", (user.Organization.Acronym + @"\" + user.UserName), (orgUser.Acronym + @"\" + orgUser.UserName)),
                    Reason = obj.State,
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    UserChangedID = user.ID,
                    UserChanged = user
                };

                db.LogsProfileChange.Add(profileLogItem);
                logs.Add(profileLogItem);
            }

            //Password Expiration Reminder
            if (user.PasswordExpiration.HasValue && user.PasswordExpiration.Value <= DateTime.UtcNow.AddDays(-5) && user.Active)
            {
                var passwordExpirationLogItem = new Audit.PasswordExpirationLog
                {
                    Description = user.FirstName + " " + user.LastName + "'s password " + (user.PasswordExpiration.Value > DateTime.UtcNow ? " will expire" : " has expired") + " on " + user.PasswordExpiration.Value.ToString("MM/d/yyyy"),
                    ExpiringUser = user,
                    ExpiringUserID = user.ID,
                    UserID = identity == null ? Guid.Empty : identity.ID
                };

                db.LogsPasswordExpiration.Add(passwordExpirationLogItem);
                logs.Add(passwordExpirationLogItem);
            }

            return logs.AsEnumerable();
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();

            if (typeof(T) == typeof(Audit.UserChangeLog))
            {
                var log = logItem as Audit.UserChangeLog;
                db.Entry(log).Reference(l => l.UserChanged).Load();

                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                var body = GenerateTimestampText(log) + "<p>Here are your most recent <b>User Change</b>s notifications from <b>" + networkName + "</b>.</p>" +
                    "<p>A change has been made to account <b>" + (log.UserChanged == null ? "Unknown Account" : log.UserChanged.UserName) + "</b> by <b>" + (actingUser == null ? "Unknown User" : actingUser.FullName) + "</b>.</p>";
   

                var notification = new Notification
                {
                    Subject = "User Change Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                       where s.EventID == EventIdentifiers.User.Change.ID && !s.User.Deleted && s.User.Active &&
                                       (
                                           (db.UserEvents.Any(a => a.EventID == EventIdentifiers.User.Change.ID && a.UserID == log.UserChangedID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                         || db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.User.Change.ID && a.OrganizationID == log.UserChanged.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                         || db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.User.Change.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()
                                           )
                                       &&
                                           (db.UserEvents.Where(a => a.EventID == EventIdentifiers.User.Change.ID && a.UserID == log.UserChangedID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                         && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.User.Change.ID && a.OrganizationID == log.UserChanged.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                         && db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.User.Change.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                           )
                                       )
                                  && ((!immediate && (Frequencies)s.Frequency != Frequencies.Immediately && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (immediate && (Frequencies)s.Frequency == Frequencies.Immediately))
                                  select new Recipient
                                       {
                                           Email = s.User.Email,
                                           Phone = s.User.Phone,
                                           Name = s.User.FirstName + " " + s.User.LastName,
                                           UserID = s.UserID
                                       }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.ProfileUpdatedLog))
            {
                var log = logItem as Audit.ProfileUpdatedLog;
                db.Entry(log).Reference(l => l.UserChanged).Load();

                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                var body = GenerateTimestampText(log) + "<p>Here are your most recent <b>My Profile Has Been Updated</b> notifications from <b>" + networkName + "</b>.</p>" +
                           "<p>A change has been made to your profile <b>" + (log.UserChanged == null ? "Unknown Account" : log.UserChanged.UserName) + "</b> by <b>" + (actingUser == null ? "Unknown User" : actingUser.FullName) + "</b>.</p>";


                var notification = new Notification
                {
                    Subject = "Your Profile Updated Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                        where s.EventID == EventIdentifiers.User.ProfileUpdated.ID && s.UserID == log.UserChangedID && !s.User.Deleted && s.User.Active
                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                        select new Recipient
                                        {
                                            Email = s.User.Email,
                                            Phone = s.User.Phone,
                                            Name = s.User.FirstName + " " + s.User.LastName,
                                            UserID = s.UserID
                                        }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.PasswordExpirationLog))
            {
                var log = logItem as Audit.PasswordExpirationLog;
                if(log.ExpiringUser == null)
                    db.Entry(log).Reference(l => l.ExpiringUser).Load();

                var body = GenerateTimestampText(log) + "<p>Here are your most recent <b>Password Expire Reminder</b> notifications from <b>" + networkName + "</b>.</p>" +
                           "<p>The password for account <b>" + log.ExpiringUser.UserName + "</b> will expire in <b>" + log.ExpiringUser.PasswordExpiration.Value.Subtract(DateTime.UtcNow) + "</b> days.</p>" +
                           "<p>Please log on to the portal and reset your password from your profile. If you are a DataMart administrator, please note you will also need to enter the new password in the DataMart Client.";

                var notification = new Notification
                {
                    Subject = "Password Expiration Reminder Notification",
                    Body = log.Description,
                    Recipients = (from s in db.UserEventSubscriptions
                                  where s.EventID == EventIdentifiers.User.PasswordExpirationReminder.ID && s.UserID == log.ExpiringUserID && !s.User.Deleted && s.User.Active
                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                  select new Recipient
                                        {
                                            Email = s.User.Email,
                                            Phone = s.User.Phone,
                                            Name = s.User.FirstName + " " + s.User.LastName,
                                            UserID = s.UserID
                                        }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }

            else if (typeof(T) == typeof(Audit.UserRegistrationSubmittedLog))
            {
                var log = logItem as Audit.UserRegistrationSubmittedLog;
                db.Entry(log).Reference(l => l.RegisteredUser).Load();
                db.Entry(log.RegisteredUser).Reference(u => u.Organization).Load();

                var forRegistrant = log.Description.StartsWith("Your registration");

                var body = GenerateTimestampText(log) + (forRegistrant ?
                           "<p>Thank you for your registration.</p>" +
                           "<p>You are registering username <b>" + log.RegisteredUser.UserName + "</b> in the organization <b>" + log.RegisteredUser.OrganizationRequested + "</b> for the role <b>" +
                           log.RegisteredUser.RoleRequested + "</b>.</p>" +
                           "<p>Once processing is completed, you will be sent a confirmation email that will provide any special instructions you may need to login.</p>" +
                           "<p>Remember that you are not registered until you receive a confirmation email.</p>" +
                           "<p>Thank you again for your registration!</p>"
                           :
                           "<p>Here are your most recent Registration Submitted notifications from <b>" + networkName + "</b>.</p>" +
                           "<p><b>" + log.RegisteredUser.FullName + "</b> has registered for account <b>" + log.RegisteredUser.UserName + "</b> with <b>" +
                           log.RegisteredUser.OrganizationRequested + "</b>.</p>");

                var recipients = forRegistrant ?
                    (from u in db.Users
                                    where u.ID == log.RegisteredUserID
                                    select new Recipient
                                    {
                                        Email = u.Email,
                                        Phone = u.Phone,
                                        Name = u.FirstName + " " + u.LastName,
                                        UserID = u.ID
                                    }
                                  ).ToArray()
                                  :
                    (from s in db.UserEventSubscriptions
                                  where s.EventID == EventIdentifiers.User.RegistrationSubmitted.ID  && !s.User.Deleted && s.User.Active &&    // Subscribed to RegistrationSubmitted event.
                                     (
                                        // Subscribing User has ApproveRejectRegistration permission (via sec group membership) on any Registrant @ Site-wide Org location
                                          (db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.User.RegistrationSubmitted.ID
                                                                    && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any() 
                                        && db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.User.RegistrationSubmitted.ID
                                                                    && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed))
                                     )
                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                    select new Recipient
                                      {
                                          Email = s.User.Email,
                                          Phone = s.User.Phone,
                                          Name = s.User.FirstName + " " + s.User.LastName,
                                          UserID = s.UserID
                                      }
                                  ).ToArray();

                var notification = new Notification
                {
                    Subject = forRegistrant ? "Registration Submitted" : "Registration Submitted Notification", //log.RegisteredUser.FirstName + " "  + log.RegisteredUser.LastName + " registered",
                    Body = body,
                    Recipients = recipients,
                    NeedsPostScript = forRegistrant ? false : true
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.UserRegistrationChangedLog))
            {
                var log = logItem as Audit.UserRegistrationChangedLog;
                var forRegistrant = log.Description.StartsWith("Your registration");
                var status = log.RegisteredUser.RejectedOn.HasValue ? "Rejected" : "Approved";
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault() ?? new User { FirstName = string.Empty, LastName = "<unknown>" };

                db.Entry(log).Reference(l => l.RegisteredUser).Load();
                if (log.RegisteredUser.OrganizationID.HasValue && log.RegisteredUser.Organization == null)
                {
                    db.Entry(log.RegisteredUser).Reference((u) => u.Organization).Load();
                }

                var subj = forRegistrant ? "Your registration has been " + status : "Registration Status Changed Notification";

                var body = GenerateTimestampText(log) + (forRegistrant ?
                    "<p>Please be informed that your registration for the username <b>" + log.RegisteredUser.UserName + "</b> in the organization <b>" + (log.RegisteredUser.OrganizationID == null ? log.RegisteredUser.OrganizationRequested : log.RegisteredUser.Organization.Name) +
                    "</b> has been <b>" + status + "</b>.<p>" + 
                    "<p>Thank you again for your registration.</p>"
                    :
                    "<p>Here are your most recent <b>Registration Status Changed</b> notifications from <b>" + networkName + "</b>.<p>" +
                    "<p>The registration status for <b>" + log.RegisteredUser.UserName + "</b> was changed from Submitted to <b>" + (log.RegisteredUser.RejectedOn.HasValue ? "Rejected" : "Approved") + 
                    "</b> by " + actingUser.FullName +"</b>.</p>");

                var recipients = forRegistrant ?
                                (from u in db.Users
                                    where u.ID == log.RegisteredUserID
                                    select new Recipient
                                    {
                                        Email = u.Email,
                                        Phone = u.Phone,
                                        Name = u.FirstName + " " + u.LastName,
                                        UserID = u.ID
                                    }
                                  ).ToArray()
                                  :
                                  (from s in db.UserEventSubscriptions
                                   where s.EventID == EventIdentifiers.User.RegistrationStatusChanged.ID  && !s.User.Deleted && s.User.Active &&
                                     (log.RegisteredUser.OrganizationID.HasValue 
                                      && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.User.RegistrationStatusChanged.ID
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active 
                                                                                                      && u.User.OrganizationID == log.RegisteredUser.OrganizationID.Value)).Any() 
                                   || db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.User.RegistrationStatusChanged.ID 
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()
                                     )
                                  && 
                                     (db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.User.RegistrationStatusChanged.ID 
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                   && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.User.RegistrationStatusChanged.ID
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active 
                                                                                                      && u.User.OrganizationID == log.RegisteredUser.OrganizationID.Value)).All(a => a.Allowed)
                                     )

                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                   select new Recipient
                                   {
                                       Email = s.User.Email,
                                       Phone = s.User.Phone,
                                       Name = s.User.FirstName + " " + s.User.LastName,
                                       UserID = s.UserID
                                   }).ToArray();

                var notification = new Notification
                {
                    Subject = subj,
                    Body = body,
                    Recipients = recipients
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }


            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the User Logging Configuration");
        }

        //private Notification CreateNotificationForRegistrant<T>(T logItem, DataContext db, bool immediate)
        //{
        //    var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();

        //    if (typeof(T) == typeof(Audit.UserRegistrationSubmittedLog))
        //    {
        //        var log = logItem as Audit.UserRegistrationSubmittedLog;
        //        log.RegisteredUser.Organization = db.Organizations.Where(o => o.ID == log.RegisteredUser.OrganizationID).FirstOrDefault();

        //        var body = "<p>Thank you for your registration.</p>" +
        //                   "<p>You are registering username \"" + log.RegisteredUser.UserName + "\" in the organization " + log.RegisteredUser.Organization.Name + " for the role " +
        //                   log.RegisteredUser.RoleRequested + ".</p>" +
        //                   "<p>Once processing is completed, you will be sent a confirmation email that will provide any special instructions you may need to login.</p>" +
        //                   "<p>Remember that you are not registered until you receive a confirmation email.</p>" +
        //                   "<p>Thank you again for your registration!</p>";

        //        var notification = new Notification
        //        {
        //            Subject = "Registration Submitted",
        //            Body = body,
        //            Recipients = (from u in db.Users
        //                            where u.ID == log.RegisteredUserID
        //                            select new Recipient
        //                            {
        //                                Email = u.Email,
        //                                Phone = u.Phone,
        //                                Name = u.FirstName + " " + u.LastName,
        //                                UserID = u.ID
        //                            }
        //                          ).ToArray()
        //        };

        //        return notification;
        //    }
        //    else if (typeof(T) == typeof(Audit.UserRegistrationChangedLog))
        //    {
        //        var log = logItem as Audit.UserRegistrationChangedLog;
        //        log.RegisteredUser.Organization = db.Organizations.Where(o => o.ID == log.RegisteredUser.OrganizationID).FirstOrDefault();
        //        var status = log.RegisteredUser.RejectedOn.HasValue ? "Rejected" : "Activated";

        //        var body = "<p>Please be informed that your registration for the username \"" + log.RegisteredUser.UserName + "\" in the organization \"" + log.RegisteredUser.Organization.Name +
        //            "\" has been " + status + ".<p>" + 
        //            "<p>Thank you again for your registration.</p>";

        //        var notification = new Notification
        //        {
        //            Subject = "Your registration has been " + status,
        //            Body = body,
        //            Recipients = 
        //        };

        //        return notification;
        //    }


        //    throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the User Logging Configuration");
        //}

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsUserChange.Include(x => x.UserChanged) select l, db.UserEventSubscriptions, EventIdentifiers.User.Change.ID).GroupBy(g => new {g.UserChangedID, g.UserID}).ToArrayAsync();

            var notifications = new List<Notification>();
            foreach (var log in logs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            var profileChangeLogs = await FilterAuditLog(from l in db.LogsProfileChange.Include(x => x.UserChanged) select l, db.UserEventSubscriptions, EventIdentifiers.User.ProfileUpdated.ID).GroupBy(g => new {g.UserChangedID, g.UserID}).ToArrayAsync();
            foreach (var log in profileChangeLogs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }


            var expirationDate = DateTime.UtcNow.AddDays(-5);
            var passwordExpiringUsers = await (from s in db.UserEventSubscriptions where s.EventID == EventIdentifiers.User.PasswordExpirationReminder.ID && (s.NextDueTime == null || s.NextDueTime.Value <= DateTime.UtcNow) && s.Frequency != Frequencies.Immediately && s.User.PasswordExpiration.HasValue && s.User.PasswordExpiration.Value > expirationDate select s.User).ToArrayAsync();

            foreach (var user in passwordExpiringUsers)
            {
                var notification = CreateNotifications(new Audit.PasswordExpirationLog {
                    Description = "Your password is expiring",
                    ExpiringUser = user,
                    ExpiringUserID = user.ID,
                    TimeStamp = DateTime.UtcNow,
                    UserID = user.ID
                }, db, false);
                if (notification != null)
                    foreach (Notification notify in notification)
                        notifications.Add(notify);
                //notifications.Add(CreateNotification(new Audit.PasswordExpirationLog {
                //    Description = "Your password is expiring",
                //    ExpiringUser = user,
                //    ExpiringUserID = user.ID,
                //    TimeStamp = DateTime.UtcNow,
                //    UserID = user.ID
                //}, db, false));
            }

            var userRegistrationSubmittedLogs = await FilterAuditLog(from l in db.LogsUserRegistrationSubmitted.Include(x => x.RegisteredUser) select l, db.UserEventSubscriptions, EventIdentifiers.User.RegistrationSubmitted.ID).GroupBy(g => new { g.RegisteredUserID, g.UserID }).ToArrayAsync();
            foreach (var log in userRegistrationSubmittedLogs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null)
                    foreach (Notification notify in notification)
                        notifications.Add(notify);
            }

            var userRegistrationChangedLogs = await FilterAuditLog(from l in db.LogsUserRegistrationChanged.Include(x => x.RegisteredUser) select l, db.UserEventSubscriptions, EventIdentifiers.User.RegistrationStatusChanged.ID).GroupBy(g => new { g.RegisteredUserID, g.UserID }).ToArrayAsync();
            foreach (var log in userRegistrationChangedLogs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null)
                    foreach (Notification notify in notification)
                        notifications.Add(notify);
            }

            return notifications.AsEnumerable();
        }
    }
}
