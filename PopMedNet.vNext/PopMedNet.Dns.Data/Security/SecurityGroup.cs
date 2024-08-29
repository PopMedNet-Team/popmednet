using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Objects;
using PopMedNet.Utilities.Objects;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("SecurityGroups")]
    public class SecurityGroup : EntityWithID, IEntityWithName
    {
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public string? Path { get; set; }

        [MaxLength(255), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public string? Owner { get; set; }

        [Required]
        public SecurityGroupKinds Kind { get; set; }

        public Guid? ParentSecurityGroupID { get; set; }

        public Guid OwnerID { get; set; }

        public virtual SecurityGroup? ParentSecurityGroup { get; set; }

        public SecurityGroupTypes Type { get; set; }

        public virtual ICollection<SecurityGroupUser> Users { get; set; } = new HashSet<SecurityGroupUser>();
        public virtual ICollection<SecurityGroup> DependantSecurityGroups { get; set; } = new HashSet<SecurityGroup>();
        public virtual ICollection<WorkflowActivitySecurityGroup> WorkflowActivities { get; set; } = new HashSet<WorkflowActivitySecurityGroup>();
    }

    internal class SecurityGroupConfiguration : IEntityTypeConfiguration<SecurityGroup>
    {
        public void Configure(EntityTypeBuilder<SecurityGroup> builder)
        {
            builder.HasMany(sg => sg.Users).WithOne(sgu => sgu.SecurityGroup).HasForeignKey(sgu => sgu.SecurityGroupID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(sg => sg.DependantSecurityGroups).WithOne(sg => sg.ParentSecurityGroup).HasForeignKey(sg => sg.ParentSecurityGroupID).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(sg => sg.WorkflowActivities).WithOne(wa => wa.SecurityGroup).HasForeignKey(wa => wa.SecurityGroupID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sg => sg.OwnerID,"IX_OwnerID").IsClustered(false).IsUnique(false);

            builder.Property(sg => sg.Path).ValueGeneratedOnAddOrUpdate();
            builder.Property(sg => sg.Owner).ValueGeneratedOnAddOrUpdate();

            builder.Property(e => e.Type).HasConversion<int>();
            builder.Property(e => e.Kind).HasConversion<int>();
        }
    }

    internal class SecurityGroupSecurityConfiguration : DnsEntitySecurityConfiguration<SecurityGroup>
    {
        public override IQueryable<SecurityGroup> SecureList(DataContext db, IQueryable<SecurityGroup> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions != null && permissions.Length > 0)
                throw new ArgumentOutOfRangeException("Permissions must be null for Security Groups");

            return db.FilteredSecurityGroups(identity.ID);
        }

        public override async Task<bool> CanInsert(DataContext db, ApiIdentity identity, params SecurityGroup[] objs)
        {
            return await HasPermissions(db, identity, objs.Select(o => o.OwnerID).ToArray(), PermissionIdentifiers.Project.ManageSecurity)
            ||
            await HasPermissions(db, identity, objs.Select(o => o.OwnerID).ToArray(), PermissionIdentifiers.Organization.ManageSecurity);
        }

        public override async Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return await (from p in db.Filter(db.Projects, identity, PermissionIdentifiers.Project.ManageSecurity) join sg in db.SecurityGroups on p.ID equals sg.OwnerID where keys.Contains(sg.ID) select p.ID).AnyAsync()
                ||
                await (from o in db.Filter(db.Organizations, identity, PermissionIdentifiers.Organization.ManageSecurity) join sg in db.SecurityGroups on o.ID equals sg.OwnerID where keys.Contains(sg.ID) select o.ID).AnyAsync()
                ;
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.Edit);
        }
    }

    public class SecurityGroupMappingProfile : AutoMapper.Profile
    {
        public SecurityGroupMappingProfile()
        {
            CreateMap<SecurityGroup, DTO.SecurityGroupDTO>()
                .ForMember(d => d.ParentSecurityGroup, opt => opt.MapFrom(src => src.ParentSecurityGroupID.HasValue ? src.ParentSecurityGroup!.Name : null));

            CreateMap<DTO.SecurityGroupDTO, SecurityGroup>()
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.Path, opt => opt.Ignore())
                .ForMember(d => d.Owner, opt => opt.Ignore())
                .ForMember(d => d.ParentSecurityGroup, opt => opt.Ignore());
        }
    }
}
