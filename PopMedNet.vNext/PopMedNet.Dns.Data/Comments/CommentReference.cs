using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("CommentReferences")]
    public class CommentReference : Entity
    {
        public Guid CommentID { get; set; }
        public virtual Comment? Comment { get; set; }
        public Guid ItemID { get; set; }
        public string? ItemTitle { get; set; }
        public CommentItemTypes Type { get; set; }
    }
    internal class CommentReferenceConfiguration : IEntityTypeConfiguration<CommentReference>
    {
        public void Configure(EntityTypeBuilder<CommentReference> builder)
        {
            builder.HasKey(e => new { e.CommentID, e.ItemID }).HasName("PK_dbo.CommentReferences");
            builder.Property(e => e.Type).HasConversion<int>();

        }
    }
}
