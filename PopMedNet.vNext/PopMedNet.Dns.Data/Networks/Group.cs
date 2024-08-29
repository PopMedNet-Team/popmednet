using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using PopMedNet.Utilities.Objects;
using PopMedNet.Dns.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("Groups")]
    public class Group : EntityWithID, ISupportsSoftDelete, IEntityWithName, IEntityWithDeleted
    {
        public Group()
        {
            this.Deleted = false;
            this.ApprovalRequired = true;
            this.Name = string.Empty;
            this.Projects = new HashSet<Project>();
        }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Column("isApprovalRequired")]
        public bool ApprovalRequired { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
        public virtual ICollection<OrganizationGroup> Organizations { get; set; } = new HashSet<OrganizationGroup>();
        public virtual ICollection<AclGroup> GroupAcls { get; set; } = new HashSet<AclGroup>();
        public virtual ICollection<GroupEvent> GroupEvents { get; set; } = new HashSet<GroupEvent>();
        public virtual ICollection<Audit.GroupChangeLog> ChangeLogs { get; set; } = new HashSet<Audit.GroupChangeLog>();
    }

    internal class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasMany(t => t.Projects)
                .WithOne(t => t.Group)
                .IsRequired(false)
                .HasForeignKey(t => t.GroupID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Organizations)
                .WithOne(t => t.Group)
                .IsRequired(true)
                .HasForeignKey(t => t.GroupID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.GroupAcls)
                .WithOne(t => t.Group)
                .IsRequired(true)
                .HasForeignKey(t => t.GroupID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.GroupEvents)
                .WithOne(t => t.Group)
                .IsRequired(true)
                .HasForeignKey(t => t.GroupID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ChangeLogs)
                .WithOne(t => t.Group)
                .IsRequired(true)
                .HasForeignKey(t => t.GroupID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class GroupSecurityConfiguration : DnsEntitySecurityConfiguration<Group>
    {
        public override IQueryable<Group> SecureList(DataContext db, IQueryable<Group> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Group.View
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Group[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateGroups);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclGroup, bool>> GroupFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.GroupID);
        }
    }

    public class GroupMappingProfile : AutoMapper.Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<Group, DTO.GroupDTO>();
            CreateMap<DTO.GroupDTO, Group>().ForPath(g => g.Timestamp, opt => opt.Ignore());
        }
    }
}
