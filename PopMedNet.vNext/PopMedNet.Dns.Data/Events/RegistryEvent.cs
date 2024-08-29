using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace PopMedNet.Dns.Data
{
    [Table("RegistryEvents")]
    public class RegistryEvent : BaseEventPermission
    {
        public RegistryEvent() { }

        public Guid RegistryID { get; set; }
        public virtual Registry? Registry { get; set; }
        public virtual Event? Event { get; set; }
    }
    internal class RegistryEventConfiguration : IEntityTypeConfiguration<RegistryEvent>
    {
        public void Configure(EntityTypeBuilder<RegistryEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.RegistryID, e.EventID }).HasName("PK_dbo.RegistryEvents");
        }
    }
}
