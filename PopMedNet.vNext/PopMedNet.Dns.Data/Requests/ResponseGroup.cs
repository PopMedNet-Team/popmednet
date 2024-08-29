using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("ResponseGroups")]
    public class ResponseGroup : EntityWithID
    {
        public ResponseGroup()
        {
        }

        public ResponseGroup(string name)
        {
            Name = name;
        }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Response> Responses { get; set; } = new HashSet<Response>();
    }

    internal class ResponseGroupConfiguration : IEntityTypeConfiguration<ResponseGroup>
    {
        public void Configure(EntityTypeBuilder<ResponseGroup> builder)
        {
            builder.HasMany(t => t.Responses)
                .WithOne(t => t.ResponseGroup)
                .IsRequired(false)
                .HasForeignKey(t => t.ResponseGroupID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
