using System.Data.Entity.ModelConfiguration;
using Lpp.Dns.DTO;
using Lpp.Utilities.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Security;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    [Table("Permissions")]
    public class Permission : EntityWithID
    {
        [Required, Index, MaxLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<PermissionLocation> Locations { get; set; }
        //public virtual ICollection<AclGlobal> GlobalAcls { get; set; }
        //public virtual ICollection<AclProject> ProjectAcls { get; set; }
        //public virtual ICollection<AclDataMart> DataMartAcls { get; set; }
        //public virtual ICollection<AclDataMartEvent> DataMartEventAcls { get; set; }
        //public virtual ICollection<AclDataMartRequestType> DataMartRequestTypeAcls { get; set; }
        //public virtual ICollection<AclEvent> EventAcls { get; set; }
        //public virtual ICollection<AclGroup> GroupAcls { get; set; }
        //public virtual ICollection<AclGroupEvent> GroupEventAcls { get; set; }
        //public virtual ICollection<AclOrganization> OrganizationAcls { get; set; }
        //public virtual ICollection<AclOrganizationDataMart> OrganizationDataMarts { get; set; }
        //public virtual ICollection<AclOrganizationEvent> OrganizationEvents { get; set; }
        //public virtual ICollection<AclOrganizationUser> OrganizationUserAcls { get; set; }
        //public virtual ICollection<AclProjectDataMart> ProjectDataMartAcls { get; set; }
        //public virtual ICollection<AclProjectDataMartRequestType> ProjectDataMartRequestTypeAcls { get; set; }
        //public virtual ICollection<AclProjectOrganization> ProjectOrganizationAcls { get; set; }
        //public virtual ICollection<AclProjectOrganizationUser> ProjectOrganizationUserAcls { get; set; }
        //public virtual ICollection<AclProjectUser> ProjectUserAcls { get; set; }
        //public virtual ICollection<AclRegistry> RegistryAcls { get; set; }
        //public virtual ICollection<AclRegistryEvent> RegistryEvents { get; set; }
        //public virtual ICollection<AclRequest> RequestAcls { get; set; }
        //public virtual ICollection<AclRequestSharedFolder> RequestSharedFolderAcls { get; set; }
        //public virtual ICollection<AclRequestType> RequestTypeAcls { get; set; }
        //public virtual ICollection<AclUser> UserAcls { get; set; }
        //public virtual ICollection<AclUserEvent> UserEvents { get; set; }
    }

    internal class PermissionConfiguration : EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            HasMany(t => t.Locations)
                .WithRequired(t => t.Permission)
                .HasForeignKey(t => t.PermissionID)
                .WillCascadeOnDelete(true);

            //HasMany(t => t.GlobalAcls).WithRequired().Map(t => t.MapKey("PermissionIdentifiers"));

            //HasMany(t => t.DataMartAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.DataMartEventAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.DataMartRequestTypeAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.EventAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.GroupAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.GroupEventAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.OrganizationAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.OrganizationDataMarts).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.OrganizationEvents).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.OrganizationUserAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.ProjectAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.ProjectDataMartAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.ProjectDataMartRequestTypeAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.ProjectOrganizationAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.ProjectOrganizationUserAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.ProjectUserAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.RegistryAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.RegistryEvents).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.RequestAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.RequestSharedFolderAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.RequestTypeAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.UserAcls).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
            //HasMany(t => t.UserEvents).WithRequired(t => t.Permission).HasForeignKey(t => t.PermissionID).WillCascadeOnDelete(true);
        }
    }

    internal class PermissionSecurityConfiguration : DnsEntitySecurityConfiguration<Permission>
    {

        public override IQueryable<Permission> SecureList(DataContext db, IQueryable<Permission> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return query;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Permission[] objs)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }
    }

    internal class PermissionDtoMappingConfiguration : EntityMappingConfiguration<Permission, PermissionDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Permission, PermissionDTO>> MapExpression
        {
            get
            {
                return (p) => new PermissionDTO
                {
                    Description = p.Description,
                    ID = p.ID,
                    Locations = p.Locations.Select(l => l.Type),
                    Name = p.Name,
                    Timestamp = p.Timestamp
                };
            }
        }
    }
}
