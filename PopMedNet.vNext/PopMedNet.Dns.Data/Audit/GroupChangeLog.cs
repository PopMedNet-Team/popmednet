using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsGroupChange")]
    public class GroupChangeLog : ChangeLog
    {
        public GroupChangeLog()
        {
            this.EventID = EventIdentifiers.Group.Change.ID;
        }

        public Guid GroupID { get; set; }
        public virtual Group? Group { get; set; }
    }
    internal class GroupChangeLogConfiguration : IEntityTypeConfiguration<GroupChangeLog>
    {
        public void Configure(EntityTypeBuilder<GroupChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.GroupID }).HasName("PK_LogsGroupChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
