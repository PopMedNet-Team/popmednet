using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRequestAssignmentChange")]
    public class RequestAssignmentChangeLog : ChangeLog
    {
        public RequestAssignmentChangeLog()
        {
            this.EventID = EventIdentifiers.Request.RequestAssignmentChange.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
        public Guid RequestUserUserID { get; set; }
        public virtual User? RequestUserUser { get; set; }
        public Guid WorkflowRoleID { get; set; }
        public virtual WorkflowRole? WorkflowRole { get; set; }
    }
    internal class RequestAssignmentChangeLogConfiguration : IEntityTypeConfiguration<RequestAssignmentChangeLog>
    {
        public void Configure(EntityTypeBuilder<RequestAssignmentChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID, e.RequestUserUserID, e.WorkflowRoleID }).HasName("PK_LogsRequestAssignmentChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
