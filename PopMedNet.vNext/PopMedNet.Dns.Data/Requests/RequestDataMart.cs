using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Enums;

namespace PopMedNet.Dns.Data
{
    [Table("RequestDataMarts")]
    public class RequestDataMart : EntityWithID
    {
        public RequestDataMart()
        {
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }

        [Column("QueryStatusTypeID")]
        public RoutingStatus Status { get; set; }

        [Column(TypeName = "tinyint")]
        public Priorities Priority { get; set; }

        public DateTime? DueDate { get; set; }


        public string? ErrorMessage { get; set; }

        public string? ErrorDetail { get; set; }

        public string? RejectReason { get; set; }

        [Column("isResultsGrouped")]
        public bool? ResultsGrouped { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        [Column("PropertiesXml")]
        public string? Properties { get; set; }

        public RoutingType? RoutingType { get; set; }

        public virtual ICollection<Response> Responses { get; set; } = new HashSet<Response>();
        public virtual ICollection<Audit.UploadedResultNeedsApprovalLog> UploadedResultNeedsApprovalLogs { get; set; } = new HashSet<Audit.UploadedResultNeedsApprovalLog>();
        public virtual ICollection<Audit.RoutingStatusChangeLog> RoutingStatusChangeLogs { get; set; } = new HashSet<Audit.RoutingStatusChangeLog>();
        public virtual ICollection<Audit.NewRequestSubmittedLog> NewRequestSubmittedLogs { get; set; } = new HashSet<Audit.NewRequestSubmittedLog>();
        public virtual ICollection<Audit.RequestDataMartMetadataChangeLog> RequestDataMartMetadataChangeLogs { get; set; } = new HashSet<Audit.RequestDataMartMetadataChangeLog>();

        public Response AddResponse(Guid submittedByID)
        {
            Response response = new Response { RequestDataMart = this, SubmittedByID = submittedByID, SubmittedOn = DateTime.UtcNow };

            if (this.Responses == null)
                this.Responses = new HashSet<Response>();

            if (this.Responses.Count == 0)
            {
                response.Count = 1;
            }
            else
            {
                response.Count = this.Responses.Max(r => r.Count) + 1;
            }

            this.Responses.Add(response);

            return response;
        }

        public Response AddResponse(Guid submittedByID, int count)
        {
            Response response = new Response { RequestDataMart = this, SubmittedByID = submittedByID, SubmittedOn = DateTime.UtcNow, Count = count };

            if (this.Responses == null)
                this.Responses = new HashSet<Response>();

            this.Responses.Add(response);

            return response;
        }

        public static RequestDataMart Create(Guid requestID, Guid dataMartID, Guid submittedByID)
        {
            RequestDataMart routing = new RequestDataMart { DataMartID = dataMartID, RequestID = requestID, Status = RoutingStatus.Draft };
            routing.AddResponse(submittedByID);
            return routing;
        }
    }

    internal class RequestDataMartConfiguration : IEntityTypeConfiguration<RequestDataMart>
    {
        public void Configure(EntityTypeBuilder<RequestDataMart> builder)
        {
            builder.HasMany(t => t.Responses)
                .WithOne(t => t.RequestDataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestDataMartID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.UploadedResultNeedsApprovalLogs)
                .WithOne(t => t.RequestDataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestDataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RoutingStatusChangeLogs)
                .WithOne(t => t.RequestDataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestDataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.NewRequestSubmittedLogs)
                .WithOne(t => t.RequestDataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestDataMartID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.RequestDataMartMetadataChangeLogs)
                .WithOne(t => t.RequestDataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestDataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Priority).HasConversion<int>();
            builder.Property(e => e.Status).HasConversion<int>();
            builder.Property(e => e.RoutingType).HasConversion<int>();
        }
    }
}
