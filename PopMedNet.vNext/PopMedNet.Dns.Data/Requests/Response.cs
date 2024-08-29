using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Workflow.Engine.Interfaces;

namespace PopMedNet.Dns.Data
{
    [Table("RequestDataMartResponses")]
    public class Response : EntityWithID, IWorkflowEntity
    {
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart? RequestDataMart { get; set; }

        public Guid? ResponseGroupID { get; set; }
        public virtual ResponseGroup? ResponseGroup { get; set; }

        public Guid? RespondedByID { get; set; }
        public virtual User? RespondedBy { get; set; }

        public DateTime? ResponseTime { get; set; }

        public int Count { get; set; }

        public DateTime SubmittedOn { get; set; }
        public Guid SubmittedByID { get; set; }
        public virtual User? SubmittedBy { get; set; }

        public string? SubmitMessage { get; set; }

        public string? ResponseMessage { get; set; }

        [MaxLength]
        public string? ResponseData { get; set; }

        public Guid? WorkflowID { get; set; }
        public virtual Workflow? Workflow { get; set; }
        public Guid? WorkFlowActivityID { get; set; }
        public virtual WorkflowActivity? WorkFlowActivity { get; set; }

        public virtual ICollection<RequestDocument> RequestDocument { get; set; } = new HashSet<RequestDocument>();
        public virtual ICollection<ResponseSearchResult> SearchResults { get; set; } = new HashSet<ResponseSearchResult>();
        public virtual ICollection<Audit.ResponseViewedLog> ViewLogs { get; set; } = new HashSet<Audit.ResponseViewedLog>();
    }

    internal class ResponseConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder.HasMany(t => t.SearchResults)
                .WithOne(t => t.Response)
                .IsRequired(true)
                .HasForeignKey(t => t.ResponseID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ViewLogs)
                .WithOne(t => t.Response)
                .IsRequired(true)
                .HasForeignKey(t => t.ResponseID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
