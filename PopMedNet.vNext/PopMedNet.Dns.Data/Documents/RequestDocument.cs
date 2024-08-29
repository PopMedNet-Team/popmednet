using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("RequestDocuments")]
    public class RequestDocument
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid RevisionSetID { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ResponseID { get; set; }
        public Response? Response { get; set; }
        public RequestDocumentType DocumentType { get; set; }
    }
    internal class RequestDocumentConfiguration : IEntityTypeConfiguration<RequestDocument>
    {
        public void Configure(EntityTypeBuilder<RequestDocument> builder)
        {
            builder.HasKey(e => new { e.RevisionSetID, e.ResponseID}).HasName("PK_dbo.RequestDocuments");
            builder.Property(e => e.DocumentType).HasConversion<int>();

        }
    }
}
