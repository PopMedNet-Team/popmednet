using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using PopMedNet.Utilities.Objects;
using PopMedNet.Dns.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO.Enums;

namespace PopMedNet.Dns.Data
{
    [Table("Registries")]
    public class Registry : EntityWithID, IEntityWithDeleted, IEntityWithName
    {
        public Registry() 
        {
            this.Deleted = false;
            this.Type = RegistryTypes.Registry;
            this.Name = string.Empty;
        }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Column(TypeName = "tinyint")]
        public RegistryTypes Type { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? RoPRUrl { get; set; }

        public virtual ICollection<OrganizationRegistry> Organizations { get; set; } = new HashSet<OrganizationRegistry>();
        public virtual ICollection<RegistryItemDefinition> Items { get; set; } = new HashSet<RegistryItemDefinition>();
        public virtual ICollection<AclRegistry> RegistryAcls { get; set; } = new HashSet<AclRegistry>();
        public virtual ICollection<RegistryEvent> RegistryEvents { get; set; } = new HashSet<RegistryEvent>();
        public virtual ICollection<Audit.RegistryChangeLog> RegistryChangeLogs { get; set; } = new HashSet<Audit.RegistryChangeLog>();

    }
    internal class RegistryConfiguration : IEntityTypeConfiguration<Registry>
    {
        public void Configure(EntityTypeBuilder<Registry> builder)
        {
            builder.HasMany(t => t.Organizations)
                .WithOne(t => t.Registry)
                .IsRequired(true)
                .HasForeignKey(t => t.RegistryID)
                .OnDelete(DeleteBehavior.Cascade);

            //map the relation between the registry items and registries using a join table without a defined join entity
            builder.HasMany(t => t.Items)
                .WithMany(i => i.Registries)
                .UsingEntity<Dictionary<string, object>>("RegistryDefinitions",
                    x => x.HasOne<RegistryItemDefinition>().WithMany().HasForeignKey("RegistryItemDefinitionID"),
                    x => x.HasOne<Registry>().WithMany().HasForeignKey("RegistryID"),
                    x => x.ToTable("RegistryDefinitions"));

            builder.HasMany(t => t.RegistryAcls)
                .WithOne(t => t.Registry)
                .IsRequired(true)
                .HasForeignKey(t => t.RegistryID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RegistryEvents)
                .WithOne(t => t.Registry)
                .IsRequired(true)
                .HasForeignKey(t => t.RegistryID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RegistryChangeLogs)
                .WithOne(t => t.Registry)
                .IsRequired(true)
                .HasForeignKey(t => t.RegistryID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class RegistrySecurityConfiguration : DnsEntitySecurityConfiguration<Registry>
    {
        public override IQueryable<Registry> SecureList(DataContext db, IQueryable<Registry> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Registry.View
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Registry[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateRegistries);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclRegistry, bool>> RegistryFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.RegistryID);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Registries.Any(r => objIDs.Contains(r.RegistryID));
        }
    }

    public class RegistryMappingProfile : AutoMapper.Profile
    {
        public RegistryMappingProfile()
        {
            CreateMap<Registry, DTO.RegistryDTO>();
            CreateMap<DTO.RegistryDTO, Registry>().ForPath(g => g.Timestamp, opt => opt.Ignore());
        }
    }
}
