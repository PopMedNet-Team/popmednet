using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsProjectChange")]
    public class ProjectChangeLog : ChangeLog
    {
        public ProjectChangeLog()
        {
            this.EventID = EventIdentifiers.Project.Change.ID;
        }

        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
    }
    internal class ProjectChangeLogConfiguration : IEntityTypeConfiguration<ProjectChangeLog>
    {
        public void Configure(EntityTypeBuilder<ProjectChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.ProjectID }).HasName("PK_LogsProjectChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
