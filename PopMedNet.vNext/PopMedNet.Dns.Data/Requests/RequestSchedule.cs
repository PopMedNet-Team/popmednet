using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PopMedNet.Dns.Data
{
    [Table("RequestSchedules")]
    public class RequestSchedule : EntityWithID
    {
        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
        public string? ScheduleID { get; set; }
        public RequestScheduleTypes ScheduleType { get; set; } = RequestScheduleTypes.Recurring;
    }

    internal class RequestScheduleConfiguration : IEntityTypeConfiguration<RequestSchedule>
    {
        public void Configure(EntityTypeBuilder<RequestSchedule> builder)
        {
            builder.Property(e => e.ScheduleType).HasConversion<int>();
        }
    }
}
