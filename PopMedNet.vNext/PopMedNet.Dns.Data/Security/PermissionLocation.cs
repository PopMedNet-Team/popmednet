using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("PermissionLocations")]
    public class PermissionLocation : Entity
    {
        public Guid PermissionID { get; set; }
        public virtual Permission? Permission { get; set; }
        public PermissionAclTypes Type { get; set; }
    }
    internal class PermissionLocationConfiguration : IEntityTypeConfiguration<PermissionLocation>
    {
        public void Configure(EntityTypeBuilder<PermissionLocation> builder)
        {
            builder.HasKey(e => new { e.PermissionID, e.Type }).HasName("PK_dbo.PermissionLocations");
            builder.Property(e => e.Type).HasConversion<int>();
        }
    }
}
