using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Lpp.Data.Composition;
using System.ComponentModel.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Model
{
    [Table("vwUsers")]
    public class User : ISecuritySubject, ISecurityObject, IHaveId<int>, INamed
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("User");

        public User()
        {
            this.UserPasswordTraces = new HashSet<UserPasswordTrace>();
            this.SID = UserDefinedFunctions.NewGuid();
            this.Subscriptions = new HashSet<Subscription>();
            this.PasswordExpiration = DateTime.Now.AddMonths(6);
            this.PasswordRestorationTokenExpiration = DateTime.Now;
        }

        [Key, Column("UserId")]
        public int Id { get; set; }
        [Column(TypeName = "varchar"), MaxLength(50)]
        public string Username { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string Password { get; set; }
        public string X509CertificateThumbprint { get; set; }
        public int FailedLoginCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime? SignupDate { get; set; }
        public DateTime? ActiveDate { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public bool EffectiveIsDeleted { get; set; }
        [Column(TypeName = "varchar"), MaxLength(50)]
        public string Email { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string Title { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string MiddleName { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return string.Format("{0}{1}{2}{3}{4}", FirstName, !FirstName.NullOrEmpty() ? " " : "", MiddleName, !MiddleName.NullOrEmpty() ? " " : "", LastName); } }
        string INamed.Name { get { return FullName; } }
        [Column(TypeName = "varchar"), MaxLength(50)]
        public string Phone { get; set; }
        [Column(TypeName = "varchar"), MaxLength(50)]
        public string Fax { get; set; }
        public string ClientSettingsXml { get; set; }
        [MaxLength(500)]
        public string RejectReason { get; set; }
        [MaxLength(255)]
        public string RoleRequested { get; set; }

        public Nullable<System.DateTime> LastUpdated { get; set; }
        public DateTime PasswordExpiration { get; set; }
        public Guid PasswordRestorationToken { get; set; }
        public DateTime PasswordRestorationTokenExpiration { get; set; }

        public Guid SID { get; set; }
        string ISecuritySubject.DisplayName { get { return this.ToString(); } }
        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }
        public override string ToString() { return Organization.Acronym + "\\" + Username; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual ICollection<UserPasswordTrace> UserPasswordTraces { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual ICollection<Request> CreatedRequests { get; set; }
        public virtual ICollection<Request> UpdatedRequests { get; set; }
        public virtual ICollection<RequestRouting> RequestRoutings { get; set; }

        public virtual ICollection<RequestRoutingInstance> RespondedRequestRoutingInstances { get; set; }
        public virtual ICollection<RequestRoutingInstance> SubmittedRequestRoutingInstances { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class UserPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var user = builder.Entity<User>();

            user.HasMany(t => t.UserPasswordTraces).WithRequired(t => t.User).HasForeignKey(t => t.UserId).WillCascadeOnDelete();
            user.HasMany(t => t.UserPasswordTraces).WithRequired(t => t.AddedByUser).HasForeignKey(t => t.AddedById).WillCascadeOnDelete(false);
            user.HasMany(t => t.Subscriptions).WithRequired(t => t.Owner).HasForeignKey(t => t.OwnerId).WillCascadeOnDelete(true);

            user.HasMany(t => t.CreatedRequests).WithRequired(t => t.CreatedByUser).HasForeignKey(t => t.CreatedByUserId).WillCascadeOnDelete(false);
            user.HasMany(t => t.UpdatedRequests).WithRequired(t => t.UpdatedByUser).HasForeignKey(t => t.UpdatedByUserId).WillCascadeOnDelete(false);
            user.HasMany(t => t.RequestRoutings).WithOptional(t => t.RespondedBy).HasForeignKey(t => t.RespondedByID).WillCascadeOnDelete(false);

            user.HasMany(t => t.RespondedRequestRoutingInstances).WithOptional(t => t.RespondedBy).HasForeignKey(t => t.RespondedByUserId).WillCascadeOnDelete(false);
            user.HasMany(t => t.SubmittedRequestRoutingInstances).WithOptional(t => t.SubmittedBy).HasForeignKey(t => t.SubmittedByUserId).WillCascadeOnDelete(false);
        }
    }
}