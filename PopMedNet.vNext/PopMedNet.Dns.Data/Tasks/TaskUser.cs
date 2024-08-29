using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("TaskUsers")]
    public class PmnTaskUser : Entity
    {
        public Guid TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
        public TaskRoles Role { get; set; }
    }
    internal class PmnTaskUserConfiguration : IEntityTypeConfiguration<PmnTaskUser>
    {
        public void Configure(EntityTypeBuilder<PmnTaskUser> builder)
        {
            builder.HasKey(e => new { e.TaskID, e.UserID }).HasName("PK_dbo.TaskUsers");
            builder.Property(e => e.Role).HasConversion<int>();
        }
    }
}
