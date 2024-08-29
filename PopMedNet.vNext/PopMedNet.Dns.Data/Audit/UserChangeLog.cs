using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsUserChange")]
    public class UserChangeLog : ChangeLog
    {
        public UserChangeLog()
        {
            this.EventID = EventIdentifiers.User.Change.ID;
        }

        public Guid UserChangedID { get; set; }
        public virtual User? UserChanged { get; set; }
    }
    internal class UserChangeLogConfiguration : IEntityTypeConfiguration<UserChangeLog>
    {
        public void Configure(EntityTypeBuilder<UserChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.UserChangedID }).HasName("PK_LogsUserChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
