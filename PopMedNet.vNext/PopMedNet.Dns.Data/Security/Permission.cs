using PopMedNet.Utilities.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("Permissions")]
    public class Permission : EntityWithID
    {
        [Required, MaxLength(250)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual ICollection<PermissionLocation> Locations { get; set; } = new HashSet<PermissionLocation>();
    }

    internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasIndex(p => p.Name, "IX_Name")
                .IsUnique(false)
                .IsClustered(false);

            builder.HasMany(p => p.Locations)
                .WithOne(l => l.Permission)
                .IsRequired(true)
                .HasForeignKey(l => l.PermissionID)
                .OnDelete(DeleteBehavior.Cascade);
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

    public class PermissionMappingProfile : AutoMapper.Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, DTO.PermissionDTO>()
                .ForMember(d => d.Locations, opt => opt.MapFrom(src => src.Locations.Select(l => l.Type)));
        }
    }
}
