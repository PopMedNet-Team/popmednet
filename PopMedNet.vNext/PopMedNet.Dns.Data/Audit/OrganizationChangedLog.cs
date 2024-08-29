using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsOrganizationChange")]
    public class OrganizationChangedLog : ChangeLog
    {
        public OrganizationChangedLog()
        {
            this.EventID = EventIdentifiers.Organization.Change.ID;
        }

        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
    }
    internal class OrganizationChangedLogConfiguration : IEntityTypeConfiguration<OrganizationChangedLog>
    {
        public void Configure(EntityTypeBuilder<OrganizationChangedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.OrganizationID }).HasName("PK_LogsOrganizationChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
