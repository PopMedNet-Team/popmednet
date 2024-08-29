using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Objects;

namespace PopMedNet.Dns.Data
{
    [Table("Comments")]
    public partial class Comment : EntityWithID
    {
        public Comment()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Text { get; set; }
        public Guid ItemID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedByID { get; set; }
        public virtual User? CreatedBy { get; set; }

        public virtual ICollection<CommentReference> References { get; set; }
    }

    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasMany(c => c.References).WithOne(r => r.Comment).IsRequired(true).HasForeignKey(r => r.CommentID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
