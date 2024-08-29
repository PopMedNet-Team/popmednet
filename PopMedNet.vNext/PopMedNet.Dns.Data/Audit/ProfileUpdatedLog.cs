using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsProfileUpdated")]
    public class ProfileUpdatedLog : ChangeLog
    {
        public ProfileUpdatedLog()
        {
            this.EventID = EventIdentifiers.User.ProfileUpdated.ID;
        }

        public Guid UserChangedID { get; set; }
        public virtual User? UserChanged { get; set; }
    }
    internal class ProfileUpdatedLogConfiguration : IEntityTypeConfiguration<ProfileUpdatedLog>
    {
        public void Configure(EntityTypeBuilder<ProfileUpdatedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.UserChangedID }).HasName("PK_LogsProfileUpdated");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
